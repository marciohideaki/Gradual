using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;

using log4net;

using QuickFix;
using QuickFix.Fields;
using QuickFix.Transport;
using QuickFix.FIX44;

using Gradual.Core.OMS.FixServerLowLatency.Regras;
using Gradual.Core.OMS.LimiteManager.Lib.Dados;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using Gradual.Core.OMS.FixServerLowLatency.Util;
using Gradual.Core.OMS.FixServerLowLatency.Memory;
using System.Collections.Concurrent;
using Gradual.Core.OMS.FixServerLowLatency.Database;

using Gradual.Core.OMS.DropCopy.Lib.Dados;

namespace Gradual.Core.OMS.FixServerLowLatency.Rede
{
    public class FixInitiator : QuickFix.MessageCracker, QuickFix.IApplication
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
        DbFix _dbFix;
        int _orderSessionID = 0;
#if _CQUEUE
        ConcurrentQueue<TOMessage> _queueToExchange;
#else    
        Queue<TOMessage> _queueToExchange;
#endif 
        Thread _thToExchange;
        
        Dictionary<SessionID, SessionAcceptor> _dicSessionAcceptor;
        //Dictionary <string, TOOrderSession> _dicMsgs;
        OrdensTransicao _orderRules;
        OrderSessionManager _orderMngr;
        // Serialization Parameters
        string _fileBackupPath;
        string _fileBackupName;
        //string _fileBackupLimitManager;
        long _seqExchangeNum;
        


        List<QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup> _lstPIDInitiator; // PID = PartyIds
        int _lenPartyIDs = 0;
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


        public FixInitiator(FixSessionItem config)
        {
            _config = config;
            _bConectadoBolsa = false;
            _socketInitiator = null;
            _finalizarSinalizado = false;
            _intCanal = _config.Operador;
            _running = false;
#if _CQUEUE
            _queueToExchange = new ConcurrentQueue<TOMessage>();
#else            
            _queueToExchange = new Queue<TOMessage>();
#endif
            //_dicMsgs = new Dictionary<string, TOOrderSession>();
            _orderMngr = new OrderSessionManager();
            _dicSessionAcceptor = new Dictionary<SessionID, SessionAcceptor>();
            _thToExchange = null;
            _orderRules = new OrdensTransicao();
            _seqExchangeNum = 0;
        }
        /// <summary>
        /// Desctructor, clear all collections
        /// </summary>
        ~FixInitiator()
        {
            _running = false;
            if (null!=_queueToExchange)
            {
#if _CQUEUE
                _queueToExchange = null;
#else                
                lock (_queueToExchange)
                {
                    _queueToExchange.Clear();
                    _queueToExchange = null;
                }
#endif
            }
            if (null!=_dicSessionAcceptor)
            {
                _dicSessionAcceptor.Clear();
                _dicSessionAcceptor = null;
            }

            if (null != _orderMngr)
            {
                _orderMngr.Clear();
                _orderMngr = null;
            }
/*
            if (null != _queueToExchange)
            {
                
#if _CQUEUE
                _queueToExchange = null;
                _queueRstEvt.Reset();
                _queueRstEvt = null;
#else                
                lock (_queueToExchange)
                {
                    _queueToExchange.Clear();
                    _queueToExchange = null;
                }
#endif

            }
 */
            _orderRules = null;
        }


        #region Start/Stop Controls
        public void Start()
        {
            try
            {
                logger.Info("FixInitiator Start(): Iniciando FixInitiator....");
                _running = true;
                _dbFix = new DbFix();

                // Buscar Application Path para compor diretorios de dicionario e afins(Substring(6) para retirar 'file:\')
                string path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(FixInitiator)).CodeBase).Substring(6);
                logger.Info("Application Path: " + path);

                // Cria sessao que será usada para mandar as mensagens
                if (string.IsNullOrEmpty(_config.SenderSubID) || string.IsNullOrEmpty(_config.TargetCompID))
                    _session = new SessionID(_config.BeginString, _config.SenderCompID, _config.TargetCompID);
                else
                    _session = new SessionID(_config.BeginString, _config.SenderCompID,_config.SenderSubID, _config.TargetCompID, _config.TargetSubID);
                
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
                mainDic.SetString("ConnectionType", _config.ConnectionType.ToLower());

                Dictionary sessDic = new Dictionary();

                sessDic.SetString("BeginString", _config.BeginString);
                sessDic.SetString("SenderCompID", _config.SenderCompID);
                if (!string.IsNullOrEmpty(_config.SenderSubID))
                    sessDic.SetString("SenderSubID", _config.SenderSubID);
                sessDic.SetString("TargetCompID", _config.TargetCompID);
                if (!string.IsNullOrEmpty(_config.TargetSubID))
                    sessDic.SetString("TargetSubID", _config.TargetSubID);
                sessDic.SetString("DataDictionary", path + Path.DirectorySeparatorChar + _config.DataDictionary);
                sessDic.SetBool("UseDataDictionary", _config.UseDataDictionary);
                QuickFix.DataDictionary.DataDictionary dataDic = new QuickFix.DataDictionary.DataDictionary(path + Path.DirectorySeparatorChar + _config.DataDictionary);
                // Atualizar o data dicitonary para o OrderSessionManager
                _orderMngr.Dict = dataDic;

                // Configure the session settings
                SessionSettings settings = new SessionSettings();

                settings.Set(mainDic);
                settings.Set(_session, sessDic);

                
                // Montagem da lista de partyIDs da Sessao
                _lstPIDInitiator = new List<NewOrderSingle.NoPartyIDsGroup>();
                int aux = _config.PartyIDs.Count;
                for (int x = 0; x < aux; x++)
                {
                    QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup grp = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
                    grp.Set(new PartyID(_config.PartyIDs[x].PartyID));
                    grp.Set(new PartyIDSource(_config.PartyIDs[x].PartyIDSource));
                    grp.Set(new PartyRole(_config.PartyIDs[x].PartyRole));
                    _lstPIDInitiator.Add(grp);
                }
                _lenPartyIDs = _lstPIDInitiator.Count;
                
                FileStoreFactory store = new FileStoreFactory(settings);
                //MemoryStoreFactory store = new MemoryStoreFactory();
                FileLogFactory logs = new FileLogFactory(settings);
                IMessageFactory msgs = new DefaultMessageFactory();
                logger.InfoFormat("Iniciando ThreadedSocketInitiator...  SenderCompID: [{0}] SenderSubID: [{1}] TargetCompID: [{2}] TargetSubID [{3}] SocketConnectHost:[{4}] SocketConnectPort:[{5}]",
                                   _config.SenderCompID, _config.SenderSubID, _config.TargetCompID, _config.TargetSubID, _config.Host, _config.SocketPort);
                _socketInitiator = new SocketInitiator(this, store, settings, logs, msgs);
                _socketInitiator.Start();

                // Serialization controls
                _fileBackupPath = ConfigurationManager.AppSettings["BackupPath"].ToString();
                if (string.IsNullOrEmpty(_fileBackupPath))
                    _fileBackupPath = path + Path.DirectorySeparatorChar + "backup";
                if (!Directory.Exists(_fileBackupPath))
                {
                    Directory.CreateDirectory(_fileBackupPath);
                }
                string fileBack = _config.BeginString + "."  + _config.SenderCompID + "." + _config.TargetCompID + ".order.session.dat";
                _fileBackupName = _fileBackupPath + Path.DirectorySeparatorChar + fileBack;
                
                logger.Info("BackupFile: " + _fileBackupName);
                //logger.Info("LimitManager BackupFile: " + _fileBackupLimitManager);
                //this.LoadSessionIDs(_dicMsgs, _fileBackupName);
                if (null != _orderMngr)
                {
                    OrderSessionInfo ret = _dbFix.ProcesssarOrderSession(fileBack);
                    _orderSessionID = ret.Id;
                    
                    // Gravacao das session orders nos arquivo
                    logger.Info("Carregando Mensagens x SessionIDs - Compondo o dicionario");           
                    // _orderMngr.LoadSessionIDs(_fileBackupName, dataDic);
                    _orderMngr.LoadOrderSessionIDsFromDB(_orderSessionID, dataDic);
                    logger.Info("Expirando validade dos registros Mensagens x SessionIDs");
                    _orderMngr.ExpireOrderSessions();
                }
                
                 
                // Thread para receber as mensagens de acceptors
                _thToExchange = new Thread(new ThreadStart(this.SendToExchange));
                _thToExchange.Priority = ThreadPriority.AboveNormal;
                _thToExchange.Start();

                //logger.Info("Iniciando cracker thread");
                //_thCrack = new Thread(new ThreadStart(this.CrackMessage));
                //_thCrack.Priority = ThreadPriority.AboveNormal;
                //_thCrack.Start();

                logger.Info("FixInitiator Start(): Fim de inicializacao FixInitiator....");
            }

            catch (Exception ex)
            {
                logger.Error("Erro ao inicializar Fix Initiator: " + ex.Message, ex);
                throw ex;
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("FixInitiator Stop(): Finalizando FixInitiator");
                _running = false;
                _dbFix = null;
                _finalizarSinalizado = true;
                logger.Info("FixInitiator Stop(): Finalizando SocketInitiator");
                if (_socketInitiator != null)
                {
                    _socketInitiator.Stop();
                    _socketInitiator = null;
                }
                logger.Info("FixInitiator Stop(): Finalizando Thread thToExchange");
                if (_thToExchange.IsAlive)
                {
                    _thToExchange.Abort();
                    _thToExchange = null;
                }

                if (null!=_lstPIDInitiator)
                {
                    for (int i = 0; i < _lstPIDInitiator.Count; i++)
                    {
                        _lstPIDInitiator[i] = null;
                    }
                    _lstPIDInitiator.Clear();
                    _lstPIDInitiator = null;
                }
                logger.Info("FixInitiator Stop(): Copiando mensagens x SessionIDs para o arquivo");
                if (null != _orderMngr)
                    //_orderMngr.Save SaveSessionIDS(_fileBackupName);
                    _orderMngr.SaveOrderSessionIDsToDB(_orderSessionID);
                logger.Info("FixInitiator Stop(): Fim de Inicializacao Stop SocketInitiator");
            }

            catch (Exception ex)
            {
                logger.Error("Erro ao parar o Fix Initiator", ex);
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
            logger.Info("FixInitiator onLogon(): " + session.ToString());
            _bConectadoBolsa = true;
            //_sendConnectionStatus();
        }

        public void OnLogout(QuickFix.SessionID session)
        {
            logger.Info("FixInitiator onLogout(): " + session.ToString());
            _bConectadoBolsa = false;
            //_sendConnectionStatus();
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
        /// Trata mensagem BusinessReject
        /// </summary>
        /// <param name="message">QuickFix44.Reject message</param>
        /// <param name="session">QuickFix.SessionID session</param>
        public void OnMessage(BusinessMessageReject message, SessionID session)
        {
            try
            {
                KeyValuePair<string, TOOrderSession> item;
                TOOrderSession to = null;
                int msgSeqNum;
                msgSeqNum = message.RefSeqNum.getValue();
                    
                // Buscar a sessao a partir do message sequence number 
                item = _orderMngr.GetOrderBySeqNum(msgSeqNum);
                
                if (item.Key != null)
                {
                    string chave = item.Key;
                    to = item.Value;
                    _orderRules.VerifyOrderSituationBMRandR(to.Sessao, _orderMngr, chave);
                }

                if (to != null)
                {
                    // Conversao do BusinessMessageReject para Mensagem ExecutionReport / OrderCancelReject em caso de alteracao / cancelamento
                    
                    QuickFix.Message origMsg = to.MensagemQF;
                    ExecutionReport er = null;
                    OrderCancelReject ocr = null;
                    if (message.RefMsgType.getValue() == MsgType.ORDER_CANCEL_REPLACE_REQUEST ||
                        message.RefMsgType.getValue() == MsgType.ORDER_CANCEL_REQUEST)
                    {
                        ocr = Fix44Translator.Fix44Reject2OrderCancelReject(message, origMsg);
                        // Compor os PartyIDs originais da mensagem
                        int lenPIDAcceptor = to.PartyIDs.Count;
                        this._partyIDsAssemble(_lenPartyIDs, lenPIDAcceptor, ocr, to.PartyIDs);
                    }
                    else
                    {
                        er = Fix44Translator.Fix44Rejection2ExecutionReport(message, origMsg);
                        // Compor os PartyIDs originais da mensagem
                        int lenPIDAcceptor = to.PartyIDs.Count;
                        this._partyIDsAssemble(_lenPartyIDs, lenPIDAcceptor, er, to.PartyIDs);
                    }

                    // Fix44Translator.Fix44Rejection2ExecutionReport(message, origMsg);
                    
                    
                    TOMessage xx = new TOMessage();
                    if (null != er)
                        xx.MensagemQF = er;
                    else
                        xx.MensagemQF = ocr;
                    xx.Sessao = to.Sessao;
                    SessionAcceptor ssAux = null;
                    _dicSessionAcceptor.TryGetValue(to.Sessao, out ssAux);
                    ssAux.AddMessage(xx);
                    xx = null;
                }
                to = null;
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(BusinessMessageReject): " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Trata mensagem de relatorio de execucao de ordem (acatamento,negocio, modificacao)
        /// </summary>
        /// <param name="message">QuickFix44.ExecutionReport message</param>
        /// <param name="session">QuickFix.SessionID session</param>
        /// Ponto de retorno para a sessao acceptor correspondente
        public void OnMessage(ExecutionReport message, QuickFix.SessionID session)
        {
            try
            {
                char cOrdStatus = message.GetChar(Tags.OrdStatus);
                char cExecType = message.GetChar(Tags.ExecType);
                string chave = message.ClOrdID.ToString() + "-" + message.Account.ToString() + "-" + message.Symbol.ToString();
                TOOrderSession to = null;

                // Teoricamente aqui sempre retornara, pois é referente a mensagem de requisicao 
                int retGetOrder = _orderMngr.GetOrder(chave, out to,"", 1, _config.Bolsa);
                
                if (to != null)
                {
                    TOOrderSession toOrder = null;
                    if (retGetOrder != FindType.DB)
                        _orderRules.VerifyOrderSituationER(message, cExecType, cOrdStatus, to.Sessao, _orderMngr, chave, out toOrder);
                    
                    //toOrder.ExchangeSeqNum = _seqExchangeNum;
                    TOMessage xx = new TOMessage();
                    int lenPIDAcceptor = to.PartyIDs.Count;
                    this._partyIDsAssemble(_lenPartyIDs, lenPIDAcceptor, message, to.PartyIDs);
                    // Recompor os PartyIds para ser enviado ao acceptor
                    //int len = to.PartyIDs.Count;
                    //for (int i = len; i >= 1; i--)
                    //{
                    //    message.RemoveGroup(i, Tags.NoPartyIDs);
                    //}
                    //for (int i = 0; i < len; i++)
                    //{
                    //    message.AddGroup(to.PartyIDs[i]); 
                    //}
                    xx.MensagemQF = message;
                    xx.Sessao = to.Sessao;

                    xx.Order = null == toOrder ? null: toOrder.Order; 
                    xx.TipoLimite = null == toOrder? TipoLimiteEnum.INDEFINIDO: toOrder.TipoLimite;
                    //toOrder.SecondaryOrderID = message.IsSetSecondaryOrderID() ? message.SecondaryOrderID.getValue() : string.Empty;
                    //toOrder.TradeDate = message.IsSetTradeDate() ? message.TradeDate.getValue() : DateTime.Now.ToString("yyyyMMdd");
                    SessionAcceptor ssAux = null;
                    _dicSessionAcceptor.TryGetValue(to.Sessao, out ssAux);
                    if (null != ssAux)
                        ssAux.AddMessage(xx);
                    else
                        logger.InfoFormat("Nao foi possivel entregar mensagem para sessao [{0}]. Desconectado ou inexistente. Msg: [{1}]", to.Sessao.ToString(), xx.MensagemQF.ToString());
                    xx = null;
                }
                to = null;
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(ExecutionReport): " + ex.Message, ex);
            }
        }

        public void OnMessage(Heartbeat message, SessionID session)
        {
            _bConectadoBolsa = true;
        }

        public void OnMessage(Logon message, SessionID session){ }

        public void OnMessage(Logout message, SessionID session) { }
        

        // New order cross will be sent directely from _processMessage(TOMessage)
        //public void OnMessage(NewOrderCross order, SessionID session)
        //{
           
        //}
        // New order single will be sent directly from _processMessage(TOMessage)
        //public void OnMessage (NewOrderSingle order, SessionID session)
        //{
            
        //}
        // Ponto de retorno para a sessao acceptor correspondente
        public void OnMessage(OrderCancelReject message, SessionID session)
        {

            try
            {
                // Somente repassar, nao sera apagado, pois houve rejeicao do cancelamento 
                // e necessita manter na collection
                KeyValuePair<string, TOOrderSession> item = new KeyValuePair<string,TOOrderSession>();
                TOOrderSession to = null;
                string chave = string.Empty;
                string chaveExch = string.Empty;
                // Montar a chave com cl + account + symbol

                if (message.IsSetField(Tags.Account) && message.IsSetField(Tags.Symbol))
                {
                    chave = message.ClOrdID.getValue() + "-" + message.Account.getValue() + "-" + message.Symbol.getValue();
                    if (message.GetField(Tags.OrderID)!="NONE")
                        chaveExch = message.OrderID.getValue() + "-" + message.Account.getValue() + "-" + message.Symbol.getValue();
                    _orderMngr.GetOrder(chave, out to, chaveExch);    
                }
                else
                    item = _orderMngr.GetOrderByClAndOrigCl(message.ClOrdID.ToString(), message.OrigClOrdID.ToString());
                
                if (null != item.Key)
                {
                    to = item.Value;
                }
                
                if (null != to)
                {
                    
                    _orderRules.VerifyOrderSituationOCR(message, to.Sessao, _orderMngr, chave, chaveExch);
                    TOMessage xx = new TOMessage();
                    // Recompor os PartyIds para ser enviado ao acceptor
                    int lenPIDAcceptor = to.PartyIDs.Count;
                    this._partyIDsAssemble(_lenPartyIDs, lenPIDAcceptor, message, to.PartyIDs);

                    xx.MensagemQF = message;
                    xx.Sessao = to.Sessao;
                    SessionAcceptor ssAux = null;
                    _dicSessionAcceptor.TryGetValue(to.Sessao, out ssAux);
                    ssAux.AddMessage(xx);
                    xx = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("OnMessage(OrderCancelReject): " + ex.Message, ex);
            }
        }

        // Order cancel replace request will be sent directly from _processMessage()
        //public void OnMessage(OrderCancelReplaceRequest order, SessionID session)
        //{
            
        //}
        // Order cancel request will be sent directly from _processMessage()
        //public void OnMessage(OrderCancelRequest order, SessionID session)
        //{
            
        //}

        /// <summary>
        /// Trata mensagem de rejeição de ordem
        /// </summary>
        /// <param name="message">QuickFix44.Reject message</param>
        /// <param name="session">QuickFix.SessionID session</param>
        
        // Ponto de retorno para a sessao acceptor correspondente
        public void OnMessage(Reject message, SessionID session)
        {
            try
            {
                KeyValuePair<string, TOOrderSession> item;
                TOOrderSession to = null;
                int msgSeqNum;
                Text text = message.Text;
                msgSeqNum = message.RefSeqNum.getValue();
                
                // Buscar a sessao a partir do message sequence number 
                //lock (_dicMsgs)
                //{
                item = _orderMngr.GetOrderBySeqNum(msgSeqNum);
                //}
                if (item.Key != null)
                {
                    string chave = item.Key;
                    to = item.Value;
                    _orderRules.VerifyOrderSituationBMRandR(to.Sessao, _orderMngr, chave);
                }

                if (to != null)
                {
                    // Conversao do Reject para mensagem ExecutionReport
                    QuickFix.Message origMsg = to.MensagemQF;
                    ExecutionReport er = Fix44Translator.Fix44Rejection2ExecutionReport(message, origMsg);
                    // Compor os PartyIDs originais da mensagem
                    int lenPIDAcceptor = to.PartyIDs.Count;
                    this._partyIDsAssemble(_lenPartyIDs, lenPIDAcceptor, er, to.PartyIDs);

                    TOMessage xx = new TOMessage();
                    xx.MensagemQF = er;
                    xx.Sessao = to.Sessao;
                    SessionAcceptor ssAux = null;
                    _dicSessionAcceptor.TryGetValue(to.Sessao, out ssAux);
                    ssAux.AddMessage(xx);
                    xx = null;
                }
                to = null;

            }
            catch (Exception ex)
            {
                logger.Error("onMessage(Reject): " + ex.Message, ex);
            }
        }

        #endregion

        #region Messages Management
        public void AddMessage(TOMessage msg)
        {
            try
            {
#if _CQUEUE
                _queueToExchange.Enqueue(msg);
                lock (_queueToExchange)
                    Monitor.Pulse(_queueToExchange);

                

#else                
                lock (_queueToExchange)
                {
                    _queueToExchange.Enqueue(msg);
                    Monitor.Pulse(_queueToExchange);
                }
#endif
            }
            catch (Exception ex)
            {
                logger.Error("AddMessage(): " + ex.Message, ex);
            }
        }

        private void SendToExchange()
        {
            try
            {
                while (_running)
                {
                    TOMessage msg = null;

#if _CQUEUE
                    if (!_queueToExchange.TryDequeue(out msg))
                    {
                        lock (_queueToExchange)
                            Monitor.Wait(_queueToExchange, 50);
                    }
#else  
                    lock (_queueToExchange)
                    {
                        if (_queueToExchange.Count > 0)
                        {
                            msg = _queueToExchange.Dequeue();
                        }
                        else
                        {
                            Monitor.Wait(_queueToExchange,5);
                            continue;
                        }

                    }
#endif
                    if (null != msg)
                    {
                        this._processMessage(msg);
                        msg = null;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("SendToExchange(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Process message and send to exchange
        /// </summary>
        /// <param name="toMsg"></param>
        private void _processMessage(TOMessage toMsg)
        {
            QuickFix.Message msgQF = toMsg.MensagemQF;
            string chave = string.Empty;
            string chave1 = string.Empty;
            string chave2 = string.Empty;

            SessionID ssID = toMsg.Sessao;
            TOOrderSession toOrder = new TOOrderSession();
            try
            {
                // Se a mensagem for order cross, entao a mesma se desdobrara em dois registros no dicionario
                int lenPIDAcceptor = 0;
                string msgType = msgQF.Header.GetField(Tags.MsgType);

                switch (msgType)
                {
                    case MsgType.NEW_ORDER_CROSS:
                        {

                            TOOrderSession toOrder1 = new TOOrderSession();
                            TOOrderSession toOrder2 = new TOOrderSession();
                            Group grpNoSides = msgQF.GetGroup(1, Tags.NoSides);
                            chave1 = grpNoSides.GetField(Tags.ClOrdID) + "-" + grpNoSides.GetField(Tags.Account) + "-" + msgQF.GetField(Tags.Symbol);
                            toOrder1.Sessao = toMsg.Sessao;
                            // New Order Cross nao possui TimeInForce / ExpireDate
                            toOrder1.ClOrdID = grpNoSides.IsSetField(Tags.ClOrdID) ? grpNoSides.GetField(Tags.ClOrdID) : string.Empty;
                            toOrder1.Account = grpNoSides.IsSetField(Tags.Account) ? Convert.ToInt32(grpNoSides.GetField(Tags.Account)) : -1;
                            // Armazenar o PartyID para montagem posterior
                            lenPIDAcceptor = grpNoSides.GetInt(Tags.NoPartyIDs);
                            for (int x = 1; x <= lenPIDAcceptor; x += 1)
                            {
                                Group noPartyIds = new Group(grpNoSides.GetGroup(x, Tags.NoPartyIDs));
                                toOrder1.PartyIDs.Add(noPartyIds);
                            }

                            this._partyIDsAssembleNewOrderCross(lenPIDAcceptor, _lenPartyIDs, grpNoSides, _lstPIDInitiator); 

                            grpNoSides = msgQF.GetGroup(2, Tags.NoSides);
                            toOrder1.TipoLimite = toMsg.TipoLimite;
                            toOrder1.Order = toMsg.Order;

                            chave2 = grpNoSides.GetField(Tags.ClOrdID) + "-" + grpNoSides.GetField(Tags.Account) + "-" + msgQF.GetField(Tags.Symbol);
                            toOrder2.Sessao = toMsg.Sessao;
                            // New Order Cross nao possui TimeInForce / ExpireDate
                            toOrder2.ClOrdID = grpNoSides.IsSetField(Tags.ClOrdID) ? grpNoSides.GetField(Tags.ClOrdID) : string.Empty;
                            toOrder2.Account = grpNoSides.IsSetField(Tags.Account) ? Convert.ToInt32(grpNoSides.GetField(Tags.Account)) : -1;
                            // Armazenar o PartyID para montagem posterior
                            lenPIDAcceptor = grpNoSides.GetInt(Tags.NoPartyIDs);
                            for (int x = 1; x <= lenPIDAcceptor; x += 1)
                            {
                                Group noPartyIds = new Group(grpNoSides.GetGroup(x, Tags.NoPartyIDs));
                                toOrder2.PartyIDs.Add(noPartyIds);
                            }
                            toOrder2.TipoLimite = toMsg.TipoLimite;
                            toOrder2.Order = toMsg.Order;
                            _orderMngr.AddOrder(chave1, toOrder1);
                            _orderMngr.AddOrder(chave2, toOrder2);

                            this._partyIDsAssembleNewOrderCross(lenPIDAcceptor, _lenPartyIDs, grpNoSides, _lstPIDInitiator);

                            // toOrder.MensagemQF = new QuickFix.Message(msgQF);
                            Session.SendToTarget(msgQF, _session);
                            toOrder1.MsgSeqNum = Convert.ToInt32(msgQF.Header.GetField(Tags.MsgSeqNum));
                            toOrder1.MensagemQF = new QuickFix.Message(msgQF);
                            toOrder2.MsgSeqNum = Convert.ToInt32(msgQF.Header.GetField(Tags.MsgSeqNum));
                            toOrder2.MensagemQF = new QuickFix.Message(msgQF);
                            msgQF.Clear();
                            msgQF = null;
                            toOrder1 = null;
                            toOrder2 = null;
                        } break;
                    default:
                        {
                            chave = msgQF.GetField(Tags.ClOrdID) + "-" + msgQF.GetField(Tags.Account) + "-" + msgQF.GetField(Tags.Symbol);
                            toOrder.Sessao = toMsg.Sessao;
                            toOrder.TipoExpiracao = msgQF.IsSetField(Tags.TimeInForce) ? msgQF.GetField(Tags.TimeInForce) : string.Empty;
                            toOrder.DataExpiracao = msgQF.IsSetField(Tags.ExpireDate) ? msgQF.GetField(Tags.ExpireDate) : string.Empty;
                            toOrder.DataEnvio = msgQF.Header.IsSetField(Tags.SendingTime) ? msgQF.Header.GetField(Tags.SendingTime) : string.Empty;
                            toOrder.OrigClOrdID = msgQF.IsSetField(Tags.OrigClOrdID) ? msgQF.GetField(Tags.OrigClOrdID) : string.Empty;
                            toOrder.ClOrdID = msgQF.IsSetField(Tags.ClOrdID) ? msgQF.GetField(Tags.ClOrdID) : string.Empty;
                            toOrder.Account = msgQF.IsSetField(Tags.Account) ? Convert.ToInt32(msgQF.GetField(Tags.Account)) : -1;
                            // Armazenar o PartyID para montagem posterior
                            lenPIDAcceptor = msgQF.IsSetField(Tags.NoPartyIDs)? msgQF.GetInt(Tags.NoPartyIDs): 0;
                            for (int x = 1; x <= lenPIDAcceptor; x += 1)
                            {
                                Group noPartyIds = new Group(msgQF.GetGroup(x, Tags.NoPartyIDs));
                                toOrder.PartyIDs.Add(noPartyIds);
                            }
                            
                            toOrder.TipoLimite = toMsg.TipoLimite;
                            toOrder.Order = toMsg.Order;
                            
                            _seqExchangeNum++;
                            toOrder.ExchangeSeqNum = _seqExchangeNum;
                            toOrder.ChaveDicionario = chave;
                            // Verificar se a tag 37 (OrderID) está na mensagem
                            // Caso sim, utilizar o mesmo para atualizar a chave ExchangeNumberID (para alteracao e cancelamento)
                            if (msgQF.IsSetField(Tags.OrderID))
                            {
                                string exchKey = msgQF.GetField(Tags.OrderID) + "-" + msgQF.GetField(Tags.Account) + "-" + msgQF.GetField(Tags.Symbol);
                                toOrder.ExchangeNumberID = exchKey;
                            }
                            _orderMngr.AddOrder(chave, toOrder);
                    
                            this._partyIDsAssemble(lenPIDAcceptor, _lenPartyIDs, msgQF, _lstPIDInitiator);
                            this._securityInfoAssemble(msgQF);
                            // toOrder.MensagemQF = new QuickFix.Message(msgQF);
                            Session.SendToTarget(msgQF, _session);
                            toOrder.MsgSeqNum = Convert.ToInt32(msgQF.Header.GetField(Tags.MsgSeqNum));
                            toOrder.MensagemQF = new QuickFix.Message(msgQF);
                            msgQF.Clear();
                            msgQF = null;
                            toOrder = null;
                        } break;
                }


                
            }
            catch (Exception ex)
            {
                logger.Error("_processMessage(): " + ex.Message, ex);
                QuickFix.FIX44.Reject rej = new QuickFix.FIX44.Reject();
                rej.Set(new RefMsgType(msgQF.Header.GetField(Tags.MsgType)));
                rej.Set(new RefSeqNum(msgQF.Header.GetInt(Tags.MsgSeqNum)));
                string xpto = msgQF.IsSetField(Tags.ClOrdID)? msgQF.GetField(Tags.ClOrdID) : string.Empty;
                rej.Set(new Text("Message ID already used by this session. ID: " + xpto));
                Session.SendToTarget(rej, ssID);
                //rej = null;
            }
        }

        /// <summary>
        /// Buscar informacoes da ordem. Para calculo de limites
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TOOrderSession GetOrigTOOrderSession(string key, string chExch)
        {
            try
            {
                TOOrderSession ret = null;
                lock (_orderMngr)
                {
                    _orderMngr.GetOrder(key, out ret, chExch, KeyType.ORIGCLORDID);
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas ao buscar o TOOrderSession " + ex.Message, ex);
                return null;
            }
        }

        private void _partyIDsAssemble(int lenPidAcceptor, int lenPidInitiator, QuickFix.Message msgQf, List<NewOrderSingle.NoPartyIDsGroup> lstGrp)
        {
            List<Group> aux = new List<Group>();
            int len = lstGrp.Count;
            aux.AddRange(lstGrp.ToList<Group>());
            this._partyIDsAssemble(lenPidAcceptor, lenPidInitiator, msgQf, aux);
        }

        private void _partyIDsAssemble(int lenPidAcceptor, int lenPidInitiator, QuickFix.Message msgQf, List<Group> lstGrp)
        {
            try
            {
                // Update PartyIDs to be sent to Initiator Session
                int diff = lenPidAcceptor - lenPidInitiator;
                if (diff == 0)
                {
                    for (int x = 0; x < lenPidInitiator; x++)
                    {
                        msgQf.ReplaceGroup(x + 1, Tags.NoPartyIDs, lstGrp[x]);
                    }
                }
                if (diff > 0)
                {
                    for (int x = 0; x < lenPidInitiator; x++)
                    {
                        msgQf.ReplaceGroup(x + 1, Tags.NoPartyIDs, lstGrp[x]);
                    }
                    // Apagar os adicionais
                    for (int x = lenPidAcceptor; x > lenPidInitiator; x--)
                    {
                        msgQf.RemoveGroup(x, Tags.NoPartyIDs);
                        msgQf.SetField(new NoPartyIDs(lenPidInitiator), true);
                    }
                }
                if (diff < 0)
                {
                    for (int x = 0; x < lenPidAcceptor; x++)
                    {
                        msgQf.ReplaceGroup(x + 1, Tags.NoPartyIDs, lstGrp[x]);
                    }
                    // Incluir os que faltam
                    for (int x = lenPidAcceptor; x < lenPidInitiator; x++)
                    {
                        msgQf.AddGroup(lstGrp[x]);
                        msgQf.SetField(new NoPartyIDs(lenPidInitiator), true);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na composicao dos PartyIDs das mensagens: " + ex.Message, ex);
            }
        }

        private void _partyIDsAssembleNewOrderCross(int lenPidAcceptor, int lenPidInitiator, Group grp, List<NewOrderSingle.NoPartyIDsGroup> lstGrp)
        {
            List<Group> aux = new List<Group>();
            int len = lstGrp.Count;
            aux.AddRange(lstGrp.ToList<Group>());
            this._partyIDsAssembleNewOrderCross(lenPidAcceptor, lenPidInitiator, grp, aux);
        }

        private void _partyIDsAssembleNewOrderCross(int lenPidAcceptor, int lenPidInitiator, Group grp, List<Group> lstGrp)
        {
            try
            {
                // Update PartyIDs to be sent to Initiator Session
                int diff = lenPidAcceptor - lenPidInitiator;
                if (diff == 0)
                {
                    for (int x = 0; x < lenPidInitiator; x++)
                    {
                        grp.ReplaceGroup(x + 1, Tags.NoPartyIDs, lstGrp[x]);
                    }
                }
                if (diff > 0)
                {
                    for (int x = 0; x < lenPidInitiator; x++)
                    {
                        grp.ReplaceGroup(x + 1, Tags.NoPartyIDs, lstGrp[x]);
                    }
                    // Apagar os adicionais
                    for (int x = lenPidAcceptor; x > lenPidInitiator; x--)
                    {
                        grp.RemoveGroup(x, Tags.NoPartyIDs);
                        grp.SetField(new NoPartyIDs(lenPidInitiator), true);
                    }
                }
                if (diff < 0)
                {
                    for (int x = 0; x < lenPidAcceptor; x++)
                    {
                        grp.ReplaceGroup(x + 1, Tags.NoPartyIDs, lstGrp[x]);
                    }
                    // Incluir os que faltam
                    for (int x = lenPidAcceptor; x < lenPidInitiator; x++)
                    {
                        grp.AddGroup(lstGrp[x]);
                        grp.SetField(new NoPartyIDs(lenPidInitiator), true);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na composicao dos PartyIDs da mensagem de NewOrderCross: " + ex.Message, ex);
            }
        }

        public List<PartyIDItem> GetPartyIDs()
        {
            return _config.PartyIDs;
        }

        private void _securityInfoAssemble(QuickFix.Message msg)
        {
            try
            {
                // string msgType = msg.Header.GetField(Tags.MsgType);

                // Supondo que exista simbolo, atribuir os campos SecurityID, SecurityIDSource e SecurityExchange
                if (msg.IsSetField(Tags.Symbol))
                {
                    msg.SetField(new SecurityID(msg.GetField(Tags.Symbol)), true);
                    msg.SetField(new SecurityIDSource(SecurityIDSource.EXCHANGE_SYMBOL), true);
                    msg.SetField(new SecurityExchange("BVMF"), true);
                }

                /*
                switch (msgType)
                {
                    case MsgType.NEWORDERSINGLE:
                    case MsgType.ORDER_CANCEL_REPLACE_REQUEST:
                    case MsgType.ORDER_CANCEL_REQUEST:
                    case MsgType.ORDER
                        break;
                }
                 */
                

            }
            catch (Exception ex)
            {
                logger.Error("Problemas na composicao do SecurityInfo da mensagem: " + ex.Message, ex);
            }
        }

        #endregion

        public void AddSessionAcceptor(SessionID sID, SessionAcceptor ss)
        {
            try
            {
                _dicSessionAcceptor.Add(sID, ss);
            }
            catch (Exception ex)
            {
                logger.Error("Erro na insercao do registro SessionAcceptor: " + ex.Message, ex); 
            }
        }

        #region Serialization Management
        /// <summary>
        /// Open _fileBackup and save dictionary
        /// </summary>
        /// <param name="regs"></param>
        /// 
        /*
        private void SaveSessionIDS(Dictionary<string, TOOrderSession> regs, string filename)
        {
            try
            {
                // Serialize Dictionary to dat file
                List<TOMessageBackup> lst = new List<TOMessageBackup>();
                if (!_serializing)
                {
                    _serializing = true;
                    FileStream fs = File.Open(filename, FileMode.Create, FileAccess.Write);
                    foreach (KeyValuePair<string, TOOrderSession> item in regs)
                    {
                        TOMessageBackup to = new TOMessageBackup();
                        
                        to.Key = item.Key;
                        to.BeginString = item.Value.Sessao.BeginString;
                        to.SenderCompID = item.Value.Sessao.SenderCompID;
                        to.TargetCompID = item.Value.Sessao.TargetCompID;
                        to.TipoExpiracao = item.Value.TipoExpiracao;
                        to.DataExpiracao = item.Value.DataExpiracao;
                        to.DataEnvio = item.Value.DataEnvio;
                        to.MsgSeqNum = item.Value.MsgSeqNum.ToString();
                        to.ClOrdID = item.Value.ClOrdID;
                        to.OrigClOrdID = item.Value.OrigClOrdID;
                        to.Account = item.Value.Account.ToString();
                        int lenPid = item.Value.PartyIDs.Count;
                        for (int i = 0; i < lenPid; i++)
                        {
                            PartyIDBackup pId = new PartyIDBackup();
                            pId.PartyID = item.Value.PartyIDs[i].GetField(Tags.PartyID);
                            pId.PartyIDSource = item.Value.PartyIDs[i].GetChar(Tags.PartyIDSource);
                            pId.PartyRole = item.Value.PartyIDs[i].GetInt(Tags.PartyRole);
                            to.PartyIDs.Add(pId);
                        }
                        // to.MensagemQF = item.Value.MensagemQF.ToString();
                        to.TipoLimite = (int)item.Value.TipoLimite;
                        to.Order = item.Value.Order;
                        to.MensagemQF = to.MensagemQF.ToString();
                        to.ExchangeNumberID = item.Value.ExchangeNumberID;
                        to.ExchangeSeqNum = item.Value.ExchangeSeqNum;
                        lst.Add(to);
                        to = null;
                    }

                    BinaryFormatter bs = new BinaryFormatter();
                    bs.Serialize(fs, lst);
                    bs = null;
                    logger.InfoFormat("SaveSessionIDS(): Registros serializados: [{0}] [{1}]", lst.Count, filename);
                    // Efetuar limpeza da lista
                    int len = lst.Count;
                    for (int i = 0; i < len; i++)
                    {
                        TOMessageBackup aux = lst[i];
                        aux = null;
                    }

                    lst.Clear();
                    lst = null;
                    fs.Close();
                    fs = null;
                    _serializing = false;
                }
                else
                {
                    if (_serializing)
                        logger.Debug("SaveSessionIDS(): Processo de serializacao já em execucao!!!");
                }
            }
            catch (Exception ex)
            {
                logger.Error("SaveSessionIDS(): Erro na serializacao dos registros do dicionario: " + ex.Message, ex);
                _serializing = false; // mudar para false para tentar backupear no proximo "ciclo"
            }
        }
         */
        /// <summary>
        /// Load messages x session ids and compose the _dicMsgs Dictionary
        /// </summary>
        /// 
        /*
        private void LoadSessionIDs(Dictionary <string, TOOrderSession> dic,  string fileName)
        {
            
            try
            {
                if (File.Exists(fileName))
                {
                    List<TOMessageBackup> lst = new List<TOMessageBackup>();
                    FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    lst = (List<TOMessageBackup>)bformatter.Deserialize(fs);
                    int length = lst.Count;
                    if (lst.Count > 0)
                    {
                        lock (dic)
                        {
                            for (int i = 0; i < length; i++)
                            {
                                TOMessageBackup to = lst[i];
                                TOOrderSession toOrder= new TOOrderSession();
                                SessionID ssID = new SessionID(to.BeginString, to.SenderCompID, to.TargetCompID);
                                toOrder.Sessao = ssID;
                                toOrder.TipoExpiracao = to.TipoExpiracao;
                                toOrder.DataExpiracao = to.DataExpiracao;
                                toOrder.DataEnvio = to.DataEnvio;
                                toOrder.MsgSeqNum = Convert.ToInt32(to.MsgSeqNum);
                                toOrder.ClOrdID = to.ClOrdID;
                                toOrder.OrigClOrdID = to.OrigClOrdID;
                                toOrder.Account = Convert.ToInt32(to.Account);
                                int len = to.PartyIDs.Count;
                                for (int j = 0; j< len; j++)
                                {
                                    QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup grp = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
                                    grp.Set(new PartyID(to.PartyIDs[j].PartyID));
                                    grp.Set(new PartyIDSource(to.PartyIDs[j].PartyIDSource));
                                    grp.Set(new PartyRole(to.PartyIDs[j].PartyRole));
                                    toOrder.PartyIDs.Add(grp);
                                }
                                
                                toOrder.TipoLimite = (TipoLimiteEnum) to.TipoLimite;
                                toOrder.Order = to.Order;
                                toOrder.MensagemQF = new QuickFix.Message(to.MensagemQF);
                                toOrder.ExchangeNumberID = to.ExchangeNumberID;
                                toOrder.ExchangeSeqNum = to.ExchangeSeqNum;
                                dic.Add(to.Key, toOrder);
                            }
                        }
                        logger.Info("LoadSessionIDs(): Registros recuperados: " + lst.Count);
                        lst.Clear();
                        lst = null;
                    }
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("LoadSessionIDs(): Erro na deserializacao dos registros do dicionario: " + ex.Message, ex);
            }
        }
        */
        public void SerializeMsgs()
        {
            try
            {
                if (null != _orderMngr)
                    //_orderMngr.SaveSessionIDS(_fileBackupName);
                    _orderMngr.SaveOrderSessionIDsToDB(_orderSessionID);
            }
            catch (Exception ex)
            {
                logger.Error("SerializeMsgs(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Routine to clear expired order from _dicMsgs
        /// Carefull to frequency execution
        /// </summary>
        
        public void ExpireOrderSessions()
        {
            if (null != _orderMngr)
                _orderMngr.ExpireOrderSessions();
        }
        #endregion


        #region Intranet Operations
        public void CancelOrderFromIntranet(string account, string origcl, string exchangenumber, string symbol)
        {
            try
            {
                // Buscar informacoes da ordem
                OrderDbInfo order = _dbFix.BuscarOrdem(origcl,Convert.ToInt32(account), symbol);
                QuickFix.FIX44.OrderCancelRequest ocr = null; 
                
                string acc;
                if (this._config.Bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                    acc = GeneralFunctions.CalcularCodigoCliente(227, Convert.ToInt32(account)).ToString();
                else
                    acc = account;

                if (null != order)
                {
                    // Gerar OrderCancelRequest
                    ocr = this._composeOrderCancelRequest(order, acc);
                }
                // Buscar a sessao para enviar para 
                string chave = origcl + "-" + acc.ToString() + "-" + symbol.ToUpper();
                TOOrderSession toOrder = null;
                _orderMngr.GetOrder(chave, out toOrder);
                if (null != toOrder)
                {
                    // Gerar TOMessage
                    TOMessage to = new TOMessage();
                    to.Sessao = toOrder.Sessao;
                    to.TipoLimite = toOrder.TipoLimite;
                    to.MensagemQF = ocr;
                    this.AddMessage(to);
                    to = null;
                }
                else
                {
                    throw new Exception("Ordem Original nao encontrada!");
                }
            }
            catch (Exception ex)
            {
                string strAux = string.Format("CancelOrderFromIntranet(): Problemas no cancelamento da ordem. OrigClOrdID: [{0}] Account: [{1}], Channel: [{2}]. Exception: [{3}]",
                                               origcl, account, this.Canal, ex.Message);
                logger.Error(strAux, ex);
                throw ex;
            }
        }

        private QuickFix.FIX44.OrderCancelRequest _composeOrderCancelRequest(OrderDbInfo orderCancelInfo, string accountWithDigitOrNot)
        {
            try
            {
             
                //Cria a mensagem FIX de OrderCancelRequest
                QuickFix.FIX44.OrderCancelRequest orderCancel = new OrderCancelRequest();

                if (!string.IsNullOrEmpty(accountWithDigitOrNot))
                    orderCancel.Set(new Account(accountWithDigitOrNot));

                orderCancel.Set(new OrigClOrdID(orderCancelInfo.ClOrdID));
                orderCancel.Set(new ClOrdID(this._newClOrdID()));
                if (!string.IsNullOrEmpty(orderCancelInfo.ExchangeNumberID))
                {
                    orderCancel.Set(new OrderID(orderCancelInfo.ExchangeNumberID));
                }


                // Instrument Identification Block
                orderCancel.Set(new Symbol(orderCancelInfo.Symbol));
                orderCancel.Set(new SecurityID(orderCancelInfo.Symbol));
                orderCancel.Set(new SecurityIDSource(SecurityIDSource.EXCHANGE_SYMBOL));

                if (orderCancelInfo.Side == 2)
                    orderCancel.Set(new Side(Side.SELL));
                else
                    orderCancel.Set(new Side(Side.BUY));
                orderCancel.Set(new TransactTime(DateTime.Now));

                //Qtde de contratos/papeis a serem cancelados
                orderCancel.Set(new OrderQty(orderCancelInfo.OrderQty));

                // Nao efetua a composicao dos partyIDs, pois sera composto no envio da mensagem


                /*
                // Cliente
                QuickFix.FIX44.OrderCancelRequest. NoPartyIDs PartyIDGroup1 = new QuickFix44.OrderCancelRequest.NoPartyIDs();
                PartyIDGroup1.set(new PartyID(orderCancelInfo.ExecBroker));
                PartyIDGroup1.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup1.set(new PartyRole(PartyRole.ENTERING_TRADER));
                
                // Corretora
                QuickFix44.OrderCancelRequest.NoPartyIDs PartyIDGroup2 = new QuickFix44.OrderCancelRequest.NoPartyIDs();
                PartyIDGroup2.set(new PartyID(_config.PartyID));
                PartyIDGroup2.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup2.set(new PartyRole(PartyRole.ENTERING_FIRM));

                QuickFix44.OrderCancelRequest.NoPartyIDs PartyIDGroup3 = new QuickFix44.OrderCancelRequest.NoPartyIDs();
                if (orderCancelInfo.SenderLocation != null && orderCancelInfo.SenderLocation.Length > 0)
                    PartyIDGroup3.set(new PartyID(orderCancelInfo.SenderLocation));
                else
                    PartyIDGroup3.set(new PartyID(this._config.SenderLocationID));
                PartyIDGroup3.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                PartyIDGroup3.set(new PartyRole(54));

                orderCancel.addGroup(PartyIDGroup1);
                orderCancel.addGroup(PartyIDGroup2);
                orderCancel.addGroup(PartyIDGroup3);
                */
                /*
                //BEI - 2012-Nov-14

                if (orderCancelInfo.ForeignFirm != null && orderCancelInfo.ForeignFirm.Length > 0)
                {
                    QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup4 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                    PartyIDGroup4.set(new PartyID(orderCancelInfo.ForeignFirm));
                    PartyIDGroup4.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                    PartyIDGroup4.set(new PartyRole(PartyRole.FOREIGN_FIRM));

                    orderCancel.addGroup(PartyIDGroup4);
                }

                if (orderCancelInfo.ExecutingTrader != null && orderCancelInfo.ExecutingTrader.Length > 0)
                {
                    QuickFix44.NewOrderSingle.NoPartyIDs PartyIDGroup7 = new QuickFix44.NewOrderSingle.NoPartyIDs();
                    PartyIDGroup7.set(new PartyID(orderCancelInfo.ExecutingTrader));
                    PartyIDGroup7.set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
                    PartyIDGroup7.set(new PartyRole(PartyRole.EXECUTING_TRADER));

                    orderCancel.addGroup(PartyIDGroup7);
                }
                */
                // Memo Field
                if(!string.IsNullOrEmpty(orderCancelInfo.Memo))
                {
                    orderCancel.SetField(new Memo(orderCancelInfo.Memo));
                }
                return orderCancel;
            }
            catch (Exception ex)
            {
                logger.Error("_composeOrderCancelRequest(): Problemas na composicao de order message request: " + ex.Message, ex);
                return null;
            }
        }

        private string _newClOrdID()
        {
            return string.Format("{0}{1}{2}",
                        DateTime.Now.ToString("ddMMyyyyhhmmss"),"-", new Random().Next(0, 999999999).ToString());
        }
        #endregion
    }
}
