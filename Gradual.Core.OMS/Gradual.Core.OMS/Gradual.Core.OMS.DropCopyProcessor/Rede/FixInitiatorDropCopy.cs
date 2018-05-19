using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using QuickFix;
using QuickFix.FIX44;
using System.IO;
using System.Reflection;
using QuickFix.Transport;
using QuickFix.Fields;
using System.Threading;
using Gradual.Core.OMS.DropCopy.Lib.Dados;
using Gradual.Core.OMS.DropCopyProcessor.Database;
using System.Globalization;
using Gradual.Core.OMS.DropCopy.Lib.Util;
using Gradual.Core.OMS.DropCopyProcessor.MessageRules;
using System.Collections.Concurrent;

namespace Gradual.Core.OMS.DropCopyProcessor.Rede
{
    public class FixInitiatorDropCopy: QuickFix.MessageCracker, QuickFix.IApplication
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        FixSessionItem _config;
        bool _bConectadoBolsa;
        SessionID _session;
        QuickFix.Transport.SocketInitiator _socketInitiator;
        bool _finalizarSinalizado;
        int _intCanal;
        bool _running;
#if _CQUEUE
        ConcurrentQueue<TODropCopyDB> _queueToDB;
#else
        Queue<TODropCopyDB> _queueToDB;
#endif
        Thread _thToDb;
        MessageProcessor _rulesMsg = null;

        object _sync = new object();
        
        //Dictionary<SessionID, SessionAcceptor> _dicSessionAcceptor;
        //Dictionary <string, TOOrderSession> _dicMsgs;
        //OrdensTransicao _orderRules;
        
        // Serialization Parameters
        //string _fileBackupPath;
        //string _fileBackupName;
        //string _fileBackupLimitManager;

        //bool _serializing;


        //List<QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup> _lstPIDInitiator; // PID = PartyIds
        //int _lenPartyIDs = 0;
        //Queue<TOCrack> _queueMsgToCrack;
        //Thread _thCrack;

        #endregion

        #region Properties
        public bool Conectado
        {
            get
            {
                return _bConectadoBolsa && !_finalizarSinalizado;
            }
        }

        public int Canal
        {
            get
            {
                return _intCanal;
            }
            internal set{}
        }

        #endregion


        public FixInitiatorDropCopy(FixSessionItem config)
        {
            _config = config;
            _bConectadoBolsa = false;
            _socketInitiator = null;
            _finalizarSinalizado = false;
            _intCanal = _config.Operador;
            _running = false;
            
#if _CQUEUE            
            _queueToDB = new ConcurrentQueue<TODropCopyDB>();
#else
            _queueToDB = new Queue<TODropCopyDB>();
#endif
            //_db = new DbDropCopy();

            _rulesMsg = new MessageProcessor();
        }
        /// <summary>
        /// Desctructor, clear all collections
        /// </summary>
        ~FixInitiatorDropCopy()
        {
            _running = false;
            if (null!=_queueToDB)
            {
#if _CQUEUE
                _queueToDB = null;
#else               
                lock (_queueToDB)
                {
                    _queueToDB.Clear();
                    _queueToDB  = null;
                }
#endif 
            }
            _rulesMsg = null;
        }


        #region Start/Stop Controls
        public void Start()
        {
            try
            {
                logger.Info("FixInitiatorDropCopy Start(): Iniciando FixInitiatorDropCopy....");
                _running = true;
                

                // Buscar Application Path para compor diretorios de dicionario e afins(Substring(6) para retirar 'file:\')
                string path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(FixInitiatorDropCopy)).CodeBase).Substring(6);
                //logger.Info("Application Path: " + path);

                // Cria sessao que será usada para mandar as mensagens
                _session = new SessionID(_config.BeginString, _config.SenderCompID, _config.TargetCompID);
                
                // Cria dicionario da configuracao 
                Dictionary mainDic = new Dictionary();

                mainDic.SetLong("SocketConnectPort", _config.SocketPort );
                mainDic.SetLong("HeartBtInt", _config.HeartBtInt);
                mainDic.SetLong("ReconnectInterval", _config.ReconnectInterval);

                mainDic.SetBool("ResetOnLogon", _config.ResetSeqNum);
                mainDic.SetBool("PersistMessages", _config.PersistMessages);

                // Ver
                // ret.setString("ConnectionType", ConnectionType.ToLower());
                mainDic.SetString("ConnectionType", _config.ConnectionType.ToLower());
                mainDic.SetString("SocketConnectHost", _config.Host);
                mainDic.SetString("FileStorePath", path + Path.DirectorySeparatorChar + _config.FileStorePath);

                //string  = path + Path.DirectorySeparatorChar + info.DataDictionary;

                logger.Debug("FileLogPath: " + path + Path.DirectorySeparatorChar + _config.FileLogPath);
                mainDic.SetString("FileLogPath", path + Path.DirectorySeparatorChar + _config.FileLogPath);
                mainDic.SetString("DebugFileLogPath", path + Path.DirectorySeparatorChar + _config.DebugFileLogPath);
                mainDic.SetString("StartTime", _config.StartTime);
                mainDic.SetString("EndTime", _config.EndTime);
                mainDic.SetString("ConnectionType", "initiator");

                Dictionary sessDic = new Dictionary();

                sessDic.SetString("BeginString", _config.BeginString);
                sessDic.SetString("SenderCompID", _config.SenderCompID);
                sessDic.SetString("TargetCompID", _config.TargetCompID);
                sessDic.SetString("DataDictionary", path + Path.DirectorySeparatorChar + _config.DataDictionary);
                sessDic.SetBool("UseDataDictionary", _config.UseDataDictionary);

                // Configure the session settings
                SessionSettings settings = new SessionSettings();

                settings.Set(mainDic);
                settings.Set(_session, sessDic);

                //FileStoreFactory store = new FileStoreFactory(settings);
                MemoryStoreFactory store = new MemoryStoreFactory();
                FileLogFactory logs = new FileLogFactory(settings);
                IMessageFactory msgs = new DefaultMessageFactory();
                logger.InfoFormat("Iniciando ThreadedSocketInitiator...  SenderCompID: [{0}] TargetCompID: [{1}] SocketConnectHost:[{2}] SocketConnectPort:[{3}]",
                                   _config.SenderCompID, _config.TargetCompID, _config.Host, _config.SocketPort);
                _socketInitiator = new SocketInitiator(this, store, settings, logs, msgs);
                _socketInitiator.Start();

                // Thread para receber as mensagens de acceptors
                _thToDb = new Thread(new ThreadStart(this._sendtoDb));
                _thToDb.Start();

                logger.Info("FixInitiatorDropCopy Start(): Fim de inicializacao FixInitiator....");
            }

            catch (Exception ex)
            {
                logger.Error("Erro ao inicializar Fix Initiator DropCopy", ex);
                throw ex;
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("FixInitiator Stop(): Finalizando FixInitiator");
                _running = false;
                _finalizarSinalizado = true;
                logger.Info("FixInitiator Stop(): Finalizando SocketInitiator");
                if (_socketInitiator != null)
                {
                    _socketInitiator.Stop();
                    _socketInitiator = null;
                }
                logger.Info("FixInitiator Stop(): Finalizando Thread thToDb");
                
                if (_thToDb.IsAlive)
                {
                    _thToDb.Abort();
                    _thToDb = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao parar o Fix Initiator DropCopy", ex);
            }
        }

        #endregion

        #region QuickFix Members
        public void FromAdmin(QuickFix.Message message, QuickFix.SessionID session)
        {
            try
            {
                this.Crack(message, session);
            }
            catch (Exception ex)
            {
                logger.Error("FromAdmin() Erro: " + ex.Message, ex);
            }
        }
        public void FromApp(QuickFix.Message message, QuickFix.SessionID session)
        {
            // Faz o processamento
            try
            {
                Crack(message, session);
            }
            catch (Exception ex)
            {
                logger.Error("FromApp() Erro: " + ex.Message, ex);
            }
        }
        public void OnCreate(QuickFix.SessionID session)
        {
            logger.Debug("onCreate().Session id : " + session.ToString());
        }

        public void OnLogon(QuickFix.SessionID session)
        {
            logger.Info("FixInitiatorDropCopy onLogon(): " + session.ToString());
            _bConectadoBolsa = true;
        }

        public void OnLogout(QuickFix.SessionID session)
        {
            logger.Info("FixInitiatorDropCopy onLogout(): " + session.ToString());
            _bConectadoBolsa = false;
        }

        public void ToAdmin(QuickFix.Message message, QuickFix.SessionID session)
        {
            // Faz o processamento
            try
            {
                // Complementa a mensagem de logon com a senha 
                if (message.GetType() == typeof(Logon))
                {
                    Logon message2 = (Logon)message;
                    if (_config.LogonPassword != "")
                    {
                        message2.Set(new RawData(_config.LogonPassword));
                        message2.Set(new RawDataLength(_config.LogonPassword.Length));
                        if (_config.NewPassword != null && _config.NewPassword.Length > 0)
                        {
                            // message2.setString(925, _config.NewPassword.Trim());
                            NewPassword newpwd = new NewPassword(_config.NewPassword.Trim());
                            message2.SetField(newpwd);
                        }
                    }

                    if (_config.CancelOnDisconnect >=0)
                    {
                        char codtype = _config.CancelOnDisconnect.ToString()[0];
                        if (codtype >= '0' && codtype <= '3')
                        {
                            message2.SetField(new CharField(35002, codtype));
                        }
                        if (_config.CODTimeout >= 0 && _config.CODTimeout <= 60)
                        {
                            message2.SetField(new IntField(35003, _config.CODTimeout * 1000));
                        }
                    }
                    message2.Set(new HeartBtInt(_config.HeartBtInt));
                    message2.Set(new EncryptMethod(0));
                    message2.Set(new ResetSeqNumFlag(_config.ResetSeqNum));
                }
                if (message.Header.GetField(Tags.MsgType) != MsgType.HEARTBEAT)
                    this.Crack(message, session);
            }
            catch (Exception ex)
            {
                logger.Error("toAdmin() Erro: " + ex.Message, ex);
            }
        }

        public void ToApp(QuickFix.Message message, QuickFix.SessionID session)
        {
        }
        #endregion

        #region QuickFix Messages
        /// <summary>
        /// Tratamento do New Order Single  - Efetuar inserção na tabela Ordem
        /// </summary>
        /// <param name="nos"></param>
        /// <param name="s"></param>
        public void OnMessage(NewOrderSingle nos, SessionID s)
        {
            try
            {
                TODropCopyDB to = new TODropCopyDB();
                to.MensagemQF = nos;
                to.Sessao = s;
                to.TipoMsg = MsgType.NEWORDERSINGLE;
                this._addMessage(to);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem NewOrderSingle: " + ex.Message, ex);
            }

        }
        
        /// <summary>
        /// Tratamento do New Order Cross - TODO [FF] - Verificar tratamento do Order Cross
        /// </summary>
        /// <param name="noc"></param>
        /// <param name="s"></param>
        public void OnMessage(NewOrderCross noc, SessionID s)
        {
            try
            {
                TODropCopyDB to = new TODropCopyDB();
                to.MensagemQF = noc;
                to.Sessao = s;
                to.TipoMsg = MsgType.NEWORDERCROSS;
                this._addMessage(to);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem de NewOrderCross: " + ex.Message, ex);
            }
        }
        
        public void OnMessage(OrderCancelReplaceRequest ocrr, SessionID s)
        {
            try
            {
                TODropCopyDB to = new TODropCopyDB();
                to.MensagemQF = ocrr;
                to.Sessao = s;
                to.TipoMsg = MsgType.ORDER_CANCEL_REPLACE_REQUEST;
                this._addMessage(to);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem de OrderCancelReplaceRequest: " + ex.Message, ex);
            }
        }

        public void OnMessage(OrderCancelRequest ocr, SessionID s)
        {
            try
            {
                TODropCopyDB to = new TODropCopyDB();
                to.MensagemQF = ocr;
                to.Sessao = s;
                to.TipoMsg = MsgType.ORDER_CANCEL_REQUEST;
                this._addMessage(to);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem de OrderCancelRequest: " + ex.Message, ex);
            }
        }

        public void OnMessage(ExecutionReport er, SessionID s)
        {
            try
            {
                TODropCopyDB to = new TODropCopyDB();
                to.MensagemQF = er;
                to.Sessao = s;
                to.TipoMsg = MsgType.EXECUTION_REPORT;
                this._addMessage(to);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem de ExecutionReport: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Trata mensagem BusinessReject
        /// </summary>
        /// <param name="message">QuickFix44.Reject message</param>
        /// <param name="session">QuickFix.SessionID session</param>
        public void OnMessage(BusinessMessageReject message, SessionID s)
        {
            try
            {
                TODropCopyDB to = new TODropCopyDB();
                to.MensagemQF = message;
                to.Sessao = s;
                to.TipoMsg = MsgType.BUSINESSMESSAGEREJECT;
                this._addMessage(to);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem de BusinessMessageReject: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="session"></param>
        public void OnMessage(OrderCancelReject message, SessionID session)
        {
            try
            {
                TODropCopyDB to = new TODropCopyDB();
                to.MensagemQF = message;
                to.Sessao = session;
                to.TipoMsg = MsgType.ORDER_CANCEL_REJECT;
                this._addMessage(to);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem de OrderCancelReject: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Trata mensagem de rejeição de ordem
        /// </summary>
        /// <param name="message">QuickFix44.Reject message</param>
        /// <param name="session">QuickFix.SessionID session</param>
        public void OnMessage(Reject message, SessionID session)
        {
            try
            {
                TODropCopyDB to = new TODropCopyDB();
                to.MensagemQF = message;
                to.Sessao = session;
                to.TipoMsg = MsgType.REJECT;
                this._addMessage(to);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem de Reject: " + ex.Message, ex);
            }
        }

        public void OnMessage(Heartbeat message, SessionID session)
        {
            _bConectadoBolsa = true;
        }

        public void OnMessage(Logon message, SessionID session)
        { 
        }

        public void OnMessage(Logout message, SessionID session)
        { 
        }

        #endregion

        #region Message Management
        private void _addMessage(TODropCopyDB msg)
        {
            try
            {
#if _CQUEUE      
                _queueToDB.Enqueue(msg);
                lock (_queueToDB)
                    Monitor.Pulse(_queueToDB);

        
#else          
                lock (_queueToDB)
                {
                    _queueToDB.Enqueue(msg);
                    Monitor.Pulse(_queueToDB);
                }
#endif
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na adicao da mensagem para processamento: " + ex.Message, ex);
            }
        }

        private void _sendtoDb()
        {
            try
            {
                while (_running)
                {
                    try
                    {
                        TODropCopyDB to = null;
#if _CQUEUE
                        lock(_sync)
                        {
                            if (!_queueToDB.TryDequeue(out to))
                            {
                                lock (_queueToDB)
                                    Monitor.Wait(_queueToDB, 50);
                                
                            }
                            if (null != to)
                            {
                                this._processMessage(to);
                                to = null;
                            }
                        }
#else                        
                        lock (_queueToDB)
                        {
                            if (_queueToDB.Count > 0)
                                to = _queueToDB.Dequeue();
                            else
                                Monitor.Wait(_queueToDB, 5);
                        }

                        if (null != to)
                        {
                            this._processMessage(to);
                            to = null;
                        }
#endif
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao desenfileirar a mensagem: " + ex.Message, ex);
            }
        }

        private void _processMessage(TODropCopyDB to)
        {
            try
            {
                //lock (_sync)
                {
                    switch (to.TipoMsg)
                    {
                        // Requests
                        case MsgType.NEWORDERSINGLE:
                            _rulesMsg.ProcessNewOrderSingle(to.MensagemQF, _config.Bolsa, _config.Operador);
                            break;
                        case MsgType.ORDER_CANCEL_REPLACE_REQUEST:
                            _rulesMsg.ProcessOrderCancelReplaceRequest(to.MensagemQF, _config.Bolsa);
                            break;
                        case MsgType.ORDER_CANCEL_REQUEST:
                            _rulesMsg.ProcessOrderCancelRequest(to.MensagemQF, _config.Bolsa);
                            break;
                        // Responses
                        case MsgType.EXECUTION_REPORT:
                            _rulesMsg.ProcessExecutionReport(to.MensagemQF, _config.Bolsa);
                            break;
                        case MsgType.ORDERCANCELREJECT:
                            _rulesMsg.ProcessOrderCancelReject(to.MensagemQF, _config.Bolsa);
                            break;
                        // Teoricamente nao deve receber mensagens de Reject e BusinessMessageReject, pois gera-se um ExecutionReport de rejeicao
                        case MsgType.REJECT:
                            _rulesMsg.ProcessReject(to.MensagemQF);
                            break;
                        case MsgType.BUSINESSMESSAGEREJECT:
                            _rulesMsg.ProcessBusinessMessageReject(to.MensagemQF);
                            break;
                    }
                    to = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem: " + ex.Message, ex);
            }
        }
        #endregion


    }
}
