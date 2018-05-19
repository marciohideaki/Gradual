using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;

using log4net;
using QuickFix;
using QuickFix.Fields;

using Gradual.OMS.Library;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using System.Collections.Concurrent;





namespace Gradual.Core.OMS.FixServerLowLatency.Rede
{
    public class FixDropCopy: QuickFix.MessageCracker, QuickFix.IApplication
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        FixDropCopyConfig _config;
        List<FixSessionItem> _sessionsDropCopy;
        Dictionary<int, List<SessionDropCopy>> _dicSessionsDropCopy;
//        bool _bConectado;
        bool _finalizarSinalizado;
        bool _running = false;
        int _intCanal;
        ThreadedSocketAcceptor _socketDropCopy;
#if _CQUEUE
        ConcurrentQueue<TODropCopy> _queueDropCopy;
        
#else
        Queue<TODropCopy> _queueDropCopy;
#endif
        Thread _thDropCopy;
        #endregion

        #region Properties
//        public bool Conectado
//        {
//            get
//            {
//                return _bConectado && !_finalizarSinalizado;
//            }
//        }
        public int Canal
        {
            get
            {
                return _intCanal;
            }
            internal set { }
        }

        #endregion

        // Constructor / Destructor
        public FixDropCopy(List<FixSessionItem> config)
        {
            _sessionsDropCopy = config;
#if _CQUEUE
            _queueDropCopy = new ConcurrentQueue<TODropCopy>();
#else            
            _queueDropCopy = new Queue<TODropCopy>();
#endif
            _dicSessionsDropCopy = new Dictionary<int, List<SessionDropCopy>>();
        }

        #region Start/Stop Controls
        public void Start()
        {
            try
            {
                logger.Info("FixDropCopy Start(): Iniciando ThreadedSocketAcceptor....");
                _running = true;

                // Buscar Application Path para compor diretorios de dicionario e afins(Substring(6) para retirar 'file:\')
                string path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(FixAcceptor)).CodeBase).Substring(6);
                logger.Info("Application Path: " + path);
                _config = GerenciadorConfig.ReceberConfig<FixDropCopyConfig>();
                // Cria dicionario da configuracao 
                Dictionary mainDic = new Dictionary();

                if (_config.SocketAcceptPort > 0)
                    mainDic.SetLong("SocketAcceptPort", _config.SocketAcceptPort);

                mainDic.SetLong("HeartBtInt", _config.HeartBtInt);
                mainDic.SetLong("ReconnectInterval", _config.ReconnectInterval);
                mainDic.SetString("FileStorePath", _config.FileStorePath);
                logger.Debug("FileLogPath: " + _config.FileLogPath);
                mainDic.SetString("FileLogPath", _config.FileLogPath);
                logger.Debug("DebugFileLogPath: " + _config.FileLogPath);
                mainDic.SetString("DebugFileLogPath", _config.DebugFileLogPath);
                mainDic.SetString("StartTime", _config.StartTime);
                mainDic.SetString("EndTime", _config.EndTime);
                mainDic.SetString("ConnectionType", "acceptor");


                // Configure the session settings
                SessionSettings settings = new SessionSettings();

                settings.Set(mainDic);

                foreach (FixSessionItem info in _sessionsDropCopy)
                {
                    Dictionary sessDic = new Dictionary();
                    string strdictionary = path + Path.DirectorySeparatorChar + info.DataDictionary;

                    sessDic.SetString("DataDictionary", strdictionary);
                    sessDic.SetBool("UseDataDictionary", info.UseDataDictionary);
                    sessDic.SetBool("ResetOnLogon", info.ResetSeqNum);
                    sessDic.SetBool("PersistMessages", info.PersistMessages);

                    logger.InfoFormat("Criando sessao DROPCOPY S:[{0}] T:[{1}] UseDic:[{2}] Dic:[{3}] Rst:[{4}] Pers:[{5}] Begstr:[{6}] FinancialLimit:[{7}] Port:[{8}] IdClient[{9}]",
                        info.SenderCompID,
                        info.TargetCompID,
                        info.UseDataDictionary,
                        strdictionary,
                        info.ResetSeqNum,
                        info.PersistMessages,
                        info.BeginString,
                        info.FinancialLimit,
                        info.Operador,
                        info.IdCliente
                        );

                    // Adicionar no dicionario para validacao de logon
                    // Atribui respectivo Initiator para cada sessao
                    SessionDropCopy ssDrop = new SessionDropCopy();
                    //FixInitiator aux = null;
                    //foreach (FixInitiator init in _lstInitiator)
                    //{
                    //    if (init.Canal == info.Operador)
                    //    {
                    //        aux = init;
                    //        break;
                    //    }
                    //}
                    ssDrop.Config = info;
                    
                    //ssDrop.Initiator = aux;
                    //ssDrop.CalcularLimite = info.FinancialLimit;
                    /*
                     */
                    SessionID session = new SessionID(info.BeginString, info.TargetCompID, info.SenderCompID);
                    ssDrop.Sessao = session;
                    ssDrop.Start();
                    //_dicSessionsDropCopy.Add(session, ssDrop);

                    List<SessionDropCopy> lstDC = null;
                    if (_dicSessionsDropCopy.TryGetValue(info.Operador, out lstDC))
                    {
                        lstDC.Add(ssDrop);
                    }
                    else
                    {
                        List<SessionDropCopy> lst = new List<SessionDropCopy>();
                        lst.Add(ssDrop);
                        _dicSessionsDropCopy.Add(info.Operador, lst);
                    }
                    // Adicionar a sessao no initiator, para conseguir "referencia cruzada"
                    // Verificar se nao ocorre erros
                    //if (null != aux)
                    //{
                    //    ssAcceptor.Initiator.AddSessionAcceptor(session, ssAcceptor);
                    //}
                    settings.Set(session, sessDic);
                }

                //FileStoreFactory store = new FileStoreFactory(settings);
                MemoryStoreFactory store = new MemoryStoreFactory();

                FileLogFactory logs = new FileLogFactory(settings);
                IMessageFactory msgs = new DefaultMessageFactory();

                logger.Info("Iniciando ThreadedSocketAcceptor ...");
                _socketDropCopy = new ThreadedSocketAcceptor(this, store, settings, logs, msgs);
                _socketDropCopy.Start();
                logger.Info("Start(): ThreadedSocketAcceptor em execucao....");


                logger.Info("Start(): Iniciando thread de mensagens....");
                // Thread para receber as mensagens de acceptors
                _thDropCopy = new Thread(new ThreadStart(this.SendToDropCopy));
                _thDropCopy.Priority = ThreadPriority.Normal;
                _thDropCopy.Start();
                logger.Info("Start(): thread de mensagens iniciada....");

                logger.Info("FixDropCopy Start(): Fim de Inicializacao do ThreadedSocketAcceptor....");
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no start da sessao FixDropCopy: " + ex.Message, ex);
                throw ex;
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("Finalizando ThreadedSocketAcceptor - Sessao DropCopy");

                _running = false;
                _finalizarSinalizado = true;
                try
                {
                    if (_socketDropCopy != null)
                    {
                        _socketDropCopy.Stop();
                        _socketDropCopy = null;
                    }

                    logger.Info("Stop(): Finalizando Thread thDropCopy");
                    if (_thDropCopy.IsAlive)
                    {
                        _thDropCopy.Abort();
                        _thDropCopy = null;
                    }

                    foreach (KeyValuePair<int, List<SessionDropCopy>> pair in _dicSessionsDropCopy)
                    {
                        int len = pair.Value.Count;
                        for (int i = 0; i < len; i++)
                        {
                            pair.Value[i].Stop();
                            pair.Value[i].Config = null;
                        }
                    }
                    _dicSessionsDropCopy.Clear();
                    _dicSessionsDropCopy = null;

                    
                }
                catch (Exception ex)
                {
                    logger.Error("Erro em Finalizar SocketAcceptor():" + ex.Message, ex);
                }
                finally
                {
                }
                logger.Info("*** SocketAcceptor finalizado ***");
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no stop da sessao FixDropCopy: " + ex.Message, ex);
            }
        }

        #endregion



        #region QuickFix Members
        public void FromAdmin(QuickFix.Message msg, QuickFix.SessionID s)
        {
            string errormsg = "Error: ";
            try
            {
                string msgType = msg.Header.GetField(Tags.MsgType);
                if (msgType == MsgType.LOGON)
                {
                    QuickFix.FIX44.Logon logonMsg = msg as QuickFix.FIX44.Logon;
                    if (logonMsg != null)
                    {
                        // Validates sender & target compids
                        string sndCompID = s.SenderCompID;
                        string tgtCompID = s.TargetCompID;
                        string password = logonMsg.IsSetField(96)? logonMsg.GetString(96): string.Empty;

                        logger.DebugFormat("snd[{0}] tgt[{1}] pwd[{2}]", sndCompID, tgtCompID, password);
                        if (!_validateLogon(sndCompID, tgtCompID, password))
                        {
                            logger.Info("Não foi possivel autenticar sessao acceptor");
                            QuickFix.FIX44.Logout logout = new QuickFix.FIX44.Logout();
                            logout.Set(new Text("This 4.4 session was not authorized"));
                            Session.SendToTarget(logout, s);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errormsg += ex.Message;
                logger.Error("FromAdmin: " + ex.Message, ex);
            }
        }

        public void FromApp(QuickFix.Message msg, QuickFix.SessionID s)
        {
            Crack(msg, s);
        }
        public void OnCreate(QuickFix.SessionID s)
        { }

        public void OnLogon(QuickFix.SessionID s)
        {
            logger.Info("FixDropCopy Acceptor OnLogon() " + s.ToString());
            //_bConectado = true;
        }

        public void OnLogout(QuickFix.SessionID s)
        {
            logger.Info("FixDropCopy OnLogout(): " + s.ToString());
            this._validateLogout(s.SenderCompID, s.TargetCompID);
            //_bConectado = false;
        }
        
        public void ToAdmin(QuickFix.Message msg, QuickFix.SessionID s)
        {
        }

        public void ToApp(QuickFix.Message msg, QuickFix.SessionID s)
        {
        }

        #endregion

        #region Flow Control
        /// <summary>
        /// Validar logon da sessao fix
        /// </summary>
        /// <returns></returns>
        private bool _validateLogon(string sndCompID, string tgtCompID, string password)
        {
            foreach (KeyValuePair<int, List<SessionDropCopy>> pair in _dicSessionsDropCopy)
            {

                int len = pair.Value.Count;
                for (int i = 0; i < len; i++)
                {
                    // Deve-se inverter o sndCompID e tgtCompID para validacao, 
                    // pois a outra ponta eh initiator e o server eh acceptor
                    if (pair.Value[i].Config.SenderCompID.Equals(tgtCompID) &&
                        pair.Value[i].Config.TargetCompID.Equals(sndCompID) &&
                        pair.Value[i].Config.LogonPassword.Equals(password))
                    {
                        pair.Value[i].Conectado = true;
                        return true;
                    }
                }
            }
            return false;
        }


        private bool _validateLogout(string sndCompID, string tgtCompID)
        {
            foreach (KeyValuePair<int, List<SessionDropCopy>> pair in _dicSessionsDropCopy)
            {

                int len = pair.Value.Count;
                for (int i = 0; i < len; i++)
                {
                    // Deve-se inverter o sndCompID e tgtCompID para validacao, 
                    // pois a outra ponta eh initiator e o server eh acceptor
                    if (pair.Value[i].Config.SenderCompID.Equals(tgtCompID) &&
                        pair.Value[i].Config.TargetCompID.Equals(sndCompID))
                    {
                        pair.Value[i].Conectado = false;
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion


        #region Messages Management
        public void AddMessage(TODropCopy to)
        {
            try
            {
#if _CQUEUE
                _queueDropCopy.Enqueue(to);
                lock (_queueDropCopy)
                {
                    Monitor.Pulse(_queueDropCopy);
                }
#else                
                lock (_queueDropCopy)
                {
                    _queueDropCopy.Enqueue(to);
                    Monitor.Pulse(_queueDropCopy);
                }
#endif
            }
            catch (Exception ex)
            {
                logger.Error("AddMessage(): " + ex.Message, ex);
            }
        }
        
        private void SendToDropCopy()
        {
            try
            {
                while (_running)
                {
                    TODropCopy msg = null;
#if _CQUEUE
                    if (!_queueDropCopy.TryDequeue(out msg))
                    {
                        lock (_queueDropCopy)
                        {
                            Monitor.Wait(_queueDropCopy, 50);
                        }
                    }
#else
                    lock (_queueDropCopy)
                    {
                        if (_queueDropCopy.Count > 0)
                        {
                            msg = _queueDropCopy.Dequeue();
                        }
                        else
                        {
                            Monitor.Wait(_queueDropCopy, 5);
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

        private void _processMessage(TODropCopy to)
        {
            try
            {
                // Buscar quais sessoes postar a mensagem
                List<SessionDropCopy> lst =null;

                if (_dicSessionsDropCopy.TryGetValue(to.Canal, out lst))
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (lst[i].Conectado)
                        {
                            // QuickFix.Message qf = new QuickFix.Message(to.MensagemQF);
                            lst[i].AddMessage(to.MensagemQF);
                            //qf.Clear();
                            //qf = null;
                        }
                    }
                    to = null;
                }
                // lst.Clear();
                lst = null;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento da mensagem DropCopy: " + ex.Message, ex);
            }
        }
        #endregion


    }
}
