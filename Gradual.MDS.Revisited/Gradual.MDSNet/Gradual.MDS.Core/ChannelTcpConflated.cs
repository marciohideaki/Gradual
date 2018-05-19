using System;
using System.Collections.Generic;
using log4net;
using Gradual.MDS.Core.Lib;
using QuickFix;
using QuickFix.FIX44;
using System.Threading;
using System.Collections.Concurrent;
using Gradual.MDS.Eventos.Lib;

namespace Gradual.MDS.Core
{
    public class ChannelTcpConflated: QuickFix.MessageCracker, QuickFix.IApplication, QuickFix.ILogFactory, QuickFix.ILog
    {
        protected log4net.ILog logger;
        protected bool _logonEfetuado = false;
        protected bool _bConectadoBolsa = false;
        protected QuickFix.SessionID _session;
        protected QuickFix.Transport.SocketInitiator _initiator;
        protected TCPConflatedConfig _channelUmdfConfig;
        protected int _secListMarketRequested = 0;
        protected int _marketDataRequested = 0;
        protected ConcurrentQueue<QuickFix.FIX44.Message> queueToSplit = new ConcurrentQueue<QuickFix.FIX44.Message>();
        protected ConcurrentQueue<QuickFix.FIX44.Message> queueToProcess = new ConcurrentQueue<QuickFix.FIX44.Message>();
        protected ConcurrentDictionary<string, string> dctChannel = new ConcurrentDictionary<string, string>();
        protected MachineGunFixSplitter machineGun = null;
        private Thread thSplitProc = null;
        private Thread thQueueProc = null;
        private bool _bKeepRunning = false;
        private object syncObjSplit = new object();
        private object syncObjProc = new object();
        protected const string FIX_DEFAULT_WORKER = "DEFAULT_WORKER";
        private bool bGetAllSecurityList = false;
        private bool bSubscribeAllMD = false;

        protected class FIXMessageWorkerState
        {
            public ConcurrentQueue<QuickFix.FIX44.Message> QueueMensagens { get; set; }
            public Thread Thread { get; set; }
            public string SecurityType { get; set; }
            public Object SyncObjFIX { get; set; }
            public long lastEnqueueLog { get; set; }

            public FIXMessageWorkerState()
            {
                QueueMensagens = new ConcurrentQueue<QuickFix.FIX44.Message>();
                SyncObjFIX = new Object();
                lastEnqueueLog = 0;
            }
        }

        public ChannelTcpConflated(TCPConflatedConfig conflatedConfig)
        {
            logger = LogManager.GetLogger("ChannelTcpConflated-" + conflatedConfig.ChannelID);

            MDSUtils.AddAppender("ChannelTcpConflated-" + conflatedConfig.ChannelID, logger.Logger);

            _bKeepRunning = true;

            machineGun = new MachineGunFixSplitter();
            machineGun.UnderFIXMessageFire += new FIXMachineGunEventHandler(machineGun_UnderFIXMessageFire);
            machineGun.Start();

            thQueueProc = new Thread(new ThreadStart(queueProc));
            thQueueProc.Start();

            thSplitProc = new Thread(new ThreadStart(splitterThreadWork));
            thSplitProc.Start();

            _channelUmdfConfig = conflatedConfig;

            //if (!listaChannelQueues.ContainsKey(conflatedConfig.ChannelID))
            //    listaChannelQueues.Add(conflatedConfig.ChannelID, new ListChannelQueues(qUdpPkt, replayLockObject));

            logger.Info("Start(): iniciando sessao FIX...");
            try
            {
                // Cria sessao FIX
                _session = new QuickFix.SessionID(
                    conflatedConfig.BeginString,
                    conflatedConfig.SenderCompID,
                    conflatedConfig.TargetCompID);

                // Cria dicionario da configuracao 
                QuickFix.Dictionary mainDic = new QuickFix.Dictionary();
                mainDic.SetLong("SocketConnectPort", conflatedConfig.ProxyPort);
                mainDic.SetLong("HeartBtInt", conflatedConfig.HeartBtInt);
                mainDic.SetLong("ReconnectInterval", conflatedConfig.ReconnectInterval);
                mainDic.SetBool("ResetOnLogon", conflatedConfig.ResetOnLogon);
                mainDic.SetBool("ResetOnLogout", conflatedConfig.ResetOnLogout);
                mainDic.SetBool("ResetOnDisconnect", conflatedConfig.ResetOnDisconnect);
                mainDic.SetBool("PersistMessages", conflatedConfig.PersistMessages);
                mainDic.SetString("ConnectionType", conflatedConfig.ConnectionType);
                mainDic.SetString("SocketConnectHost", conflatedConfig.ProxyHost);
                mainDic.SetString("FileStorePath", conflatedConfig.FileStorePath);
                mainDic.SetString("FileLogPath", conflatedConfig.FileLogPath);
                mainDic.SetString("StartTime", conflatedConfig.StartTime);
                mainDic.SetString("EndTime", conflatedConfig.EndTime);

                QuickFix.Dictionary sessDic = new QuickFix.Dictionary();
                sessDic.SetString("BeginString", conflatedConfig.BeginString);
                sessDic.SetString("SenderCompID", conflatedConfig.SenderCompID);

                sessDic.SetString("TargetCompID", conflatedConfig.TargetCompID);
                sessDic.SetString("DataDictionary", conflatedConfig.DataDictionary);
                sessDic.SetBool("CheckLatency", false);
                sessDic.SetBool("UseDataDictionary", true);
                sessDic.SetLong("SocketReceiveBufferSize", conflatedConfig.SocketReceiveBufferSize);

                // Configure the session settings
                QuickFix.SessionSettings settings = new QuickFix.SessionSettings();

                settings.Set(mainDic);
                settings.Set(_session, sessDic);

                MemoryStoreFactory store = new MemoryStoreFactory();
                FileLogFactory log = new FileLogFactory(settings);
                IMessageFactory message = new DefaultMessageFactory();

                // Cria o socket
                _initiator = new QuickFix.Transport.SocketInitiator(this, store, settings, this, message);
                _initiator.Start();

                QuickFix.Session mySession = QuickFix.Session.LookupSession(_session);
                QuickFix.Session.LookupSession(_session).ValidateLengthAndChecksum = false;

            }
            catch (Exception ex)
            {
                logger.Error("Start():" + ex.Message, ex);
            }

            logger.Info("Start(): Sessao FIX iniciado!");
        }

        void machineGun_UnderFIXMessageFire(object sender, FIXMachineGunEventEventArgs args)
        {
            enqueueToProcess(args.Message);
        }

        public virtual void Stop()
        {
            try
            {
                _bKeepRunning = false;

                _initiator.Stop();
                _initiator = null;
                _session = null;

                while (thSplitProc.IsAlive)
                {
                    logger.Info("Aguardando finalizar thread splitter");
                    Thread.Sleep(250);
                }

                while (thQueueProc.IsAlive)
                {
                    logger.Info("Aguardando finalizar thread de processamento da fila de mensagens FIX");
                    Thread.Sleep(250);
                }

                if (machineGun != null)
                {
                    machineGun.Stop();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Stop():" + ex.Message, ex);
            }

            _logonEfetuado = false;

            logger.Info("Stop(): Sessao FIX finalizado!");
        }


        #region Quickfix Application Members
        public void FromAdmin(QuickFix.Message message, QuickFix.SessionID session)
        {
            try
            {
                logger.Debug("RCV(Admin) <-- type[" + message.GetType().ToString() + "] msg[" + message.ToString() + "]");
            }
            catch (Exception ex)
            {
                logger.Error("FromAdmin(): " + ex.Message, ex);
            }
        }

        public virtual void FromApp(QuickFix.Message message, QuickFix.SessionID session)
        {
            try
            {
                if ( logger.IsDebugEnabled )
                    logger.Debug("RCV(App) <-- type[" + message.GetType().ToString() + "] msg[" + message.ToString() + "]");

                this.Crack(message, session);
            }
            catch (Exception ex)
            {
                logger.Error("FromApp(): " + ex.Message, ex);
            }
        }

        public virtual void OnCreate(QuickFix.SessionID session)
        {
            try
            {
                logger.Info("OnCreate() id[" + session.ToString() + "]");
            }
            catch (Exception ex)
            {
                logger.Error("OnCreate(): " + ex.Message, ex);
            }
        }

        public virtual void OnLogon(QuickFix.SessionID session)
        {
            logger.Info("OnLogon()");
            _bConectadoBolsa = true;

            _secListMarketRequested = 0;
            _marketDataRequested = 0;
            machineGun.Reset();
            ThreadPool.QueueUserWorkItem(new WaitCallback(RequestSecurityList), session);
        }

        public virtual void OnLogout(QuickFix.SessionID session)
        {
            try
            {
                logger.Info("OnLogout()");

                string msg = String.Format("Logout da sessao: {0}-{1}-{2}-{3}", session.SenderCompID,
                    session.SenderSubID,
                    session.TargetCompID,
                    session.TargetSubID);

                if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday && DateTime.Now.DayOfWeek != DayOfWeek.Saturday)
                {
                    if (DateTime.Now.Hour >= ConstantesMDS.HORARIO_NOTIFICACAO_EMAIL_INICIO &&
                        DateTime.Now.Hour < ConstantesMDS.HORARIO_NOTIFICACAO_EMAIL_FIM)
                    {
                        MDSUtils.EnviarEmail("FIX Logout", msg);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("OnLogout(): " + ex.Message, ex);
            }

            _bConectadoBolsa = false;
        }


        public virtual void ToAdmin(QuickFix.Message message, QuickFix.SessionID session)
        {
            try
            {
                logger.Debug("SND(Admin) --> type[" + message.GetType().ToString() + "] msg[" + message.ToString() + "]");
                //if (message.Header.GetField(QuickFix.Fields.Tags.MsgType) != QuickFix.Fields.MsgType.HEARTBEAT)
                //    this.Crack(message, session);
                // Complementa a mensagem de logon com a senha 
                if (message.GetType() == typeof(Logon))
                {
                    Logon message2 = (Logon)message;
                    if (_channelUmdfConfig.LogonPassword != "")
                    {
                        message2.Set(new QuickFix.Fields.RawData(_channelUmdfConfig.LogonPassword));
                        message2.Set(new QuickFix.Fields.RawDataLength(_channelUmdfConfig.LogonPassword.Length));
                        if (_channelUmdfConfig.NewPassword != null && _channelUmdfConfig.NewPassword.Length > 0)
                        {
                            QuickFix.Fields.StringField newPassword = new QuickFix.Fields.StringField(925, _channelUmdfConfig.NewPassword.Trim());
                            message2.SetField(newPassword);
                        }
                    }

                    message2.Set(new QuickFix.Fields.HeartBtInt(_channelUmdfConfig.HeartBtInt));
                    message2.Set(new QuickFix.Fields.EncryptMethod(0));
                    message2.Set(new QuickFix.Fields.ResetSeqNumFlag(_channelUmdfConfig.ResetOnLogon));
                }

                logger.Debug("ToAdmin(). Session id : " + session.ToString() + " Msg: " + message.GetType().ToString());

                //if (message.getHeader().getField(MsgType.FIELD) != QuickFix.MsgType.Heartbeat)
                //    this.crack(message, session);
            }
            catch (Exception ex)
            {
                logger.Error("ToAdmin(): " + ex.Message, ex);
            }
        }

        public virtual void ToApp(QuickFix.Message message, QuickFix.SessionID session)
        {
            try
            {
                if ( logger.IsDebugEnabled ) 
                    logger.Debug("SND(App) --> type[" + message.GetType().ToString() + "] msg[" + message.ToString() + "]");

                //this.Crack(message, session);
            }
            catch (Exception ex)
            {
                logger.Error("ToApp(): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.Heartbeat message, SessionID session)
        {
            try
            {
                logger.Info("OnMessage(Heartbeat)");
            }
            catch (Exception ex)
            {
                logger.Error("OnMessage(Heartbeat): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.Reject message, SessionID session)
        {
            try
            {
                string sessionRejectReason = message.IsSetSessionRejectReason() ? message.SessionRejectReason.ToString() : "0";
                string refSeqNum = message.IsSetRefSeqNum() ? message.RefSeqNum.ToString() : "no-ref-seq-num";
                string refTagID = message.IsSetRefTagID() ? message.RefTagID.ToString() : "no-tag-id";
                string refMsgType = message.IsSetRefMsgType() ? message.RefMsgType.ToString() : "no-ref-msg-type";
                string text = message.IsSetText() ? message.Text.ToString() : "no-text";

                logger.Error("onMessage(Reject): onMessage(Reject) SessionID: " + session.ToString());
                logger.Error("onMessage(Reject): Reason =[" + sessionRejectReason + "]");
                logger.Error("onMessage(Reject): RefSeqNum=[" + refSeqNum + "]");
                logger.Error("onMessage(Reject): RefTagID=[" + refTagID + "]");
                logger.Error("onMessage(Reject): RefMsgType=[" + refMsgType + "]");
                logger.Error("onMessage(Reject): Error=[" + text + "]");
            }
            catch (QuickFix.UnsupportedMessageType uex)
            {
                logger.Error("onMessage(Reject): " + uex.Message + "\r\n Data: " + uex.Data, uex);
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(Reject): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.News message, SessionID session)
        {
            try
            {
                logger.Info("Recebeu NEWS, descartando....");
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(QuickFix.FIX44.News): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.UnsupportedMessageType message, SessionID session)
        {
            try
            {

            }
            catch (Exception ex)
            {
                logger.Error("onMessage(UnsupportedMessageType): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.SecurityList message, SessionID session)
        {
            try
            {
                EventoFIX evento = new EventoFIX();

                if (message.IsSetField(QuickFix.Fields.Tags.SecurityRequestResult) &&
                    message.IsSetField(QuickFix.Fields.Tags.SecurityReqID))
                {
                    string requestResult = message.GetString(QuickFix.Fields.Tags.SecurityRequestResult);

                    if (!requestResult.Equals("0"))
                    {
                        logger.WarnFormat("Request [{0}] nao pode ser atendido result=[{1}]",
                            message.GetString(QuickFix.Fields.Tags.SecurityReqID),
                            message.GetString(QuickFix.Fields.Tags.SecurityRequestResult));
                    }
                }

                if (message.IsSetSecurityReqID())
                {
                    int noRelatedSym = message.GroupCount(QuickFix.Fields.Tags.NoRelatedSym);

                    for (int i = 1; i <= noRelatedSym; i++)
                    {
                        QuickFix.Group relatedSyms = message.GetGroup(i, QuickFix.Fields.Tags.NoRelatedSym);

                        string securityID = relatedSyms.GetString(QuickFix.Fields.Tags.SecurityID);
                        string securityType = relatedSyms.GetString(QuickFix.Fields.Tags.SecurityType);

                        dctChannel.AddOrUpdate(securityID, securityType, (key, oldValue) => securityType);
                    }
                }

                logger.Debug("OnMessage(QuickFix.FIX44.SecurityList) Enqueue to split [" + message.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum) +"]");
                enqueueToSplit(message);
                logger.Debug("OnMessage(QuickFix.FIX44.SecurityList) Enqueud [" + message.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum) +"]");

                // Requisita a proxima lista de papeis 
                if (message.IsSetLastFragment() && message.LastFragment.ToString().ToUpper().Equals("Y"))
                {
                    if (message.IsSetSecurityReqID())
                    {
                        logger.Info("Final da security list ID [" + message.SecurityReqID.ToString() + "]");

                        logger.Info("Solicitando snapshots");
                        ThreadPool.QueueUserWorkItem(new WaitCallback(RequestMarketData), session);

                        if (_secListMarketRequested < _channelUmdfConfig.Markets.Count)
                            ThreadPool.QueueUserWorkItem(new WaitCallback(RequestSecurityList), session);
                        else
                        {
                            logger.Info("Final das requisicoes de securitylist");
                            //logger.Info("Solicitando snapshots");
                            //ThreadPool.QueueUserWorkItem(new WaitCallback(RequestMarketData), session);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(SecurityList): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.MarketDataSnapshotFullRefresh message, SessionID session)
        {
            try
            {
                enqueueToSplit(message);

                // Requisita a proxima lista de papeis 
                if (message.IsSetLastFragment() && message.LastFragment.ToString().ToUpper().Equals("Y"))
                {
                    if (message.IsSetMDReqID())
                        logger.Info("Final do snapshot ID [" + message.MDReqID.ToString() + "]");

                    if ( _marketDataRequested >= _channelUmdfConfig.Markets.Count)
                    {
                        logger.Info("Final das requisicoes de snapshot");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(MarketDataSnapshotFullRefresh): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.MarketDataIncrementalRefresh message, SessionID session)
        {
            try
            {
                enqueueToSplit(message);
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(MarketDataIncrementalRefresh): " + ex.Message, ex);
            }
        }


        public virtual void OnMessage(QuickFix.FIX44.MarketDataRequestReject message, SessionID session)
        {
            try
            {
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(MarketDataRequestReject): " + ex.Message, ex);
            }
        }


        public virtual void OnMessage(QuickFix.FIX44.SecurityStatus message, SessionID session)
        {
            try
            {
                enqueueToSplit(message);
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(SecurityStatus): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.TradeHistoryResponse message, SessionID session)
        {
            try
            {
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(TradeHistoryResponse): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.MarketTotalsResponse message, SessionID session)
        {
            try
            {
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(MarketTotalsResponse): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.MarketTotalsComposition message, SessionID session)
        {
            try
            {
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(MarketTotalsComposition): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.MarketTotalsBroadcast message, SessionID session)
        {
            try
            {
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(MarketTotalsBroadcast): " + ex.Message, ex);
            }
        }



        #endregion //Quickfix Application Members

        #region Private Methods
        public void RequestSecurityList(object state)
        {
            try
            {
                // So pra teste, aguarda 5 segundo antes de enviar request
                Thread.Sleep(15000);
                if (_secListMarketRequested < _channelUmdfConfig.Markets.Count)
                {
                    QuickFix.SessionID session = (QuickFix.SessionID)state;

                    ConflatedSecurityList secList = _channelUmdfConfig.Markets[_secListMarketRequested];

                    logger.Info("Requisitando SecurityList Market [" + secList.Product + "] Type [" + secList.SecurityType + "]");

                    QuickFix.FIX44.SecurityListRequest secListReq = new SecurityListRequest();

                    secListReq.Product = new QuickFix.Fields.Product(Convert.ToInt32(secList.Product));  //EQUITIES
                    secListReq.SubscriptionRequestType = new QuickFix.Fields.SubscriptionRequestType('1'); // Subscribe + update
                    secListReq.SecurityReqID = new QuickFix.Fields.SecurityReqID(secList.SecurityReqIDPrefix + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    secListReq.SecurityType = new QuickFix.Fields.SecurityType(secList.SecurityType);

                    Session.SendToTarget(secListReq, session);

                    _secListMarketRequested++;
                }
                
            }
            catch (Exception ex)
            {
                logger.Error("RequestSecurityList: " + ex.Message, ex);
            }
        }


        public void RequestMarketData(object state)
        {
            try
            {
                // So pra teste, aguarda 5 segundo antes de enviar request
                Thread.Sleep(15000);

                if (_marketDataRequested < _channelUmdfConfig.Markets.Count)
                {
                    QuickFix.SessionID session = (QuickFix.SessionID)state;

                    ConflatedSecurityList secList = _channelUmdfConfig.Markets[_marketDataRequested];

                    logger.Info("Requisitando snapshot MD [" + secList.Product + "] Type [" + secList.SecurityType + "]");

                    QuickFix.FIX44.MarketDataRequest mktDataReq = new MarketDataRequest();

                    mktDataReq.Product = new QuickFix.Fields.Product(Convert.ToInt32(secList.Product));  //EQUITIES
                    mktDataReq.SubscriptionRequestType = new QuickFix.Fields.SubscriptionRequestType('1'); // Subscribe + update
                    mktDataReq.MDReqID = new QuickFix.Fields.MDReqID(secList.SecurityReqIDPrefix + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    mktDataReq.SecurityType = new QuickFix.Fields.SecurityType(secList.SecurityType);
                    mktDataReq.MDBookType = new QuickFix.Fields.MDBookType(3); //MBO

                    Session.SendToTarget(mktDataReq, session);

                    _marketDataRequested++;
                }
            }
            catch (Exception ex)
            {
                logger.Error("RequestMarketData: " + ex.Message, ex);
            }
        }

        private void enqueueToProcess(QuickFix.FIX44.Message message)
        {
            try
            {
                queueToProcess.Enqueue(message);
            }
            catch (Exception ex)
            {
                logger.Error("enqueueToProcess: " + ex.Message, ex);
            }
        }

        private void enqueueToSplit(QuickFix.FIX44.Message message)
        {
            try
            {
                machineGun.EnqueueMessage(message);

                //queueToSplit.Enqueue(message);

                //if (queueToSplit.Count == 1)
                //{
                //    lock (syncObjSplit)
                //    {
                //        Monitor.Pulse(syncObjSplit);
                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.Error("enqueueToSplit: " + ex.Message, ex);
            }
        }


        private void splitterThreadWork()
        {
            logger.Info("Iniciando thread de splitter processor");

            long lastLog = 0;
            while (_bKeepRunning)
            {
                try
                {
                    QuickFix.FIX44.Message message = null;

                    if (queueToSplit.TryDequeue(out message))
                    {
                        logger.Debug("Splitting Message [" + message.Header.GetInt(34) + "] type [" + message.Header.GetString(35) + "]");

                        List<QuickFix.FIX44.Message> lstSplitted = FIXUtils.splitMessage(message, 0, null);

                        foreach (QuickFix.FIX44.Message splittedmsg in lstSplitted)
                        {
                            enqueueToProcess(splittedmsg);
                        }

                        if (MDSUtils.shouldLog(lastLog))
                        {
                            logger.Info("Mensagens para splitting FIX na fila: " + queueToSplit.Count);
                            lastLog = DateTime.UtcNow.Ticks;
                        }

                        continue;
                    }

                    Thread.Sleep(25);
                }
                catch (Exception ex)
                {
                    logger.Error("splitterProc: " + ex.Message, ex);
                }
            }

            logger.Info("Thread de splitter finalizada");
        }

        private void queueProc()
        {
            logger.Info("Iniciando thread de processamento da fila de mensagens FIX");
            long lastLog = 0;
            while (_bKeepRunning)
            {
                try
                {
                    QuickFix.FIX44.Message message = null;

                    if (queueToProcess.TryDequeue(out message))
                    {
                        if ( logger.IsDebugEnabled )
                            logger.Debug("Processing Message [" + message.Header.GetInt(34) + "] type [" + message.Header.GetString(35) + "]");

                        string channelID;
                        string msgtype = message.Header.GetString(QuickFix.Fields.Tags.MsgType);
                        switch (msgtype)
                        {
                            case QuickFix.FIX44.MarketDataIncrementalRefresh.MsgType:
                                if (message.GroupCount(QuickFix.Fields.Tags.NoMDEntries) > 0)
                                {
                                    QuickFix.Group MDEntry = message.GetGroup(1, QuickFix.Fields.Tags.NoMDEntries);
                                    string securityID = MDEntry.GetString(QuickFix.Fields.Tags.SecurityID);
                                    channelID = dctChannel.GetOrAdd(securityID, FIX_DEFAULT_WORKER);
                                }
                                else
                                    channelID = FIX_DEFAULT_WORKER;
                                break;
                            case QuickFix.FIX44.MarketDataSnapshotFullRefresh.MsgType:
                                if (message.IsSetField(QuickFix.Fields.Tags.SecurityID))
                                {
                                    string securityID = message.GetString(QuickFix.Fields.Tags.SecurityID);
                                    channelID = dctChannel.GetOrAdd(securityID, FIX_DEFAULT_WORKER);
                                }
                                else
                                    channelID = FIX_DEFAULT_WORKER;
                                break;
                            case QuickFix.FIX44.SecurityList.MsgType:
                                if (message.GroupCount(QuickFix.Fields.Tags.NoRelatedSym) > 0)
                                {
                                    QuickFix.Group MDEntry = message.GetGroup(1, QuickFix.Fields.Tags.NoRelatedSym);
                                    string securityID = MDEntry.GetString(QuickFix.Fields.Tags.SecurityID);
                                    string securityType = MDEntry.GetString(QuickFix.Fields.Tags.SecurityType);
                                    channelID = dctChannel.GetOrAdd(securityID, securityType);
                                }
                                else
                                    channelID = FIX_DEFAULT_WORKER;
                                break;
                            case QuickFix.FIX44.SecurityStatus.MsgType:
                                if (message.IsSetField(QuickFix.Fields.Tags.SecurityID))
                                {
                                    string securityID = message.GetString(QuickFix.Fields.Tags.SecurityID);
                                    channelID = dctChannel.GetOrAdd(securityID, FIX_DEFAULT_WORKER);
                                }
                                else
                                    channelID = FIX_DEFAULT_WORKER;
                                break;
                            default:
                                channelID = FIX_DEFAULT_WORKER;
                                break;
                        }

                        EventoFIX eventoFIX = new EventoFIX();
                        eventoFIX.ChannelID = channelID;
                        eventoFIX.MarketDepth = ConstantesUMDF.UMDF_MARKETDEPTH_MARKET_BY_ORDER;
                        eventoFIX.Message = message;
                        eventoFIX.MsgSeqNum = message.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum);
                        eventoFIX.MsgType = msgtype;
                        eventoFIX.StreamType = StreamTypeEnum.STREAM_TYPE_TCP_CONFLATED;


                        ContainerManager.Instance.EnqueueFIX(eventoFIX);

                        if (MDSUtils.shouldLog(lastLog))
                        {
                            logger.Info("Mensagens FIX para processar na fila: " + queueToProcess.Count);
                            lastLog = DateTime.UtcNow.Ticks;
                        }

                        continue;
                    }

                    Thread.Sleep(25);
                }
                catch (Exception ex)
                {
                    logger.Error("queueProc: " + ex.Message, ex);
                }
            }

            logger.Info("Thread de fila de mensagens FIX finalizada");
        }

        #endregion //Private Methods

        internal void Start()
        {
            throw new NotImplementedException();
        }

        public string EnviarAssinaturaSecurityListConflated(string securityType, string product, string cfiCode)
        {
            string seqlistReqID = "SECLST";

            try
            {
                logger.InfoFormat("Requisitando SecurityList Product [{0}] Type [{1}] CFI[{2}]",
                    product, securityType, cfiCode);

                QuickFix.FIX44.SecurityListRequest secListReq = new SecurityListRequest();

                if (!String.IsNullOrEmpty(product) && !product.Equals("0"))
                {
                    secListReq.Product = new QuickFix.Fields.Product(Convert.ToInt32(product));
                    seqlistReqID += "-" + product;
                }

                if (!String.IsNullOrEmpty(securityType))
                {
                    secListReq.SecurityType = new QuickFix.Fields.SecurityType(securityType);
                    seqlistReqID += "-" + securityType;
                }

                if (!String.IsNullOrEmpty(cfiCode))
                {
                    secListReq.CFICode = new QuickFix.Fields.CFICode(cfiCode);
                    seqlistReqID += "-" + cfiCode;
                }

                seqlistReqID += "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                secListReq.SecurityReqID = new QuickFix.Fields.SecurityReqID(seqlistReqID);

                secListReq.SubscriptionRequestType = new QuickFix.Fields.SubscriptionRequestType('1'); // Subscribe + update

                Session.SendToTarget(secListReq, _session);
            }
            catch (Exception ex)
            {
                logger.Error("EnviarAssinaturaSecurityListConflated: " + ex.Message, ex);
            }

            return seqlistReqID;
        }


        public void CancelarAssinaturaSecurityListConflated(string securityType, string product, string cfiCode, string securityReqID)
        {
            try
            {
                logger.InfoFormat("Cancelando SecurityList Product [{0}] Type [{1}] CFI[{2}] ReqID [{3}]",
                    product, securityType, cfiCode, securityReqID);

                QuickFix.FIX44.SecurityListRequest secListReq = new SecurityListRequest();

                if (!String.IsNullOrEmpty(product))
                    secListReq.Product = new QuickFix.Fields.Product(Convert.ToInt32(product));

                if (!String.IsNullOrEmpty(securityType))
                    secListReq.SecurityType = new QuickFix.Fields.SecurityType(securityType);

                if (!String.IsNullOrEmpty(cfiCode))
                    secListReq.CFICode = new QuickFix.Fields.CFICode(cfiCode);

                secListReq.SecurityReqID = new QuickFix.Fields.SecurityReqID(securityReqID);
                secListReq.SubscriptionRequestType = new QuickFix.Fields.SubscriptionRequestType(QuickFix.Fields.SubscriptionRequestType.DISABLE_PREVIOUS); // unsubscribe
                
                Session.SendToTarget(secListReq, _session);
            }
            catch (Exception ex)
            {
                logger.Error("CancelarAssinaturaSecurityListConflated: " + ex.Message, ex);
            }
        }

        public string EnviarAssinaturaMarketDataConflated(string instrumento, string securityType, string product, string cfiCode)
        {
            string mdReqID = "MDREQ";
            try
            {
                logger.InfoFormat("Requisitando snapshot MD Instrumento[{0}] Produto [{1}] Type [{2}] CFI[{3}]",
                    instrumento,
                    product,
                    securityType,
                    cfiCode);

                QuickFix.FIX44.MarketDataRequest mktDataReq = new MarketDataRequest();

                if (!String.IsNullOrEmpty(instrumento))
                {
                    QuickFix.FIX44.MarketDataRequest.NoRelatedSymGroup noRelatedSym = new MarketDataRequest.NoRelatedSymGroup();

                    noRelatedSym.SecurityExchange = new QuickFix.Fields.SecurityExchange("BVMF");
                    noRelatedSym.SecurityIDSource = new QuickFix.Fields.SecurityIDSource("8");
                    noRelatedSym.SecurityID = new QuickFix.Fields.SecurityID(instrumento);

                    mktDataReq.AddGroup(noRelatedSym);

                    mdReqID += "-" + instrumento;
                }

                if (!String.IsNullOrEmpty(product) && !product.Equals("0"))
                {
                    mktDataReq.Product = new QuickFix.Fields.Product(Convert.ToInt32(product));
                    mdReqID += "-" + product;
                }

                if ( !String.IsNullOrEmpty(securityType)) 
                {
                    mktDataReq.SecurityType = new QuickFix.Fields.SecurityType(securityType);
                }

                if (!String.IsNullOrEmpty(cfiCode))
                {
                    mktDataReq.CFICode = new QuickFix.Fields.CFICode(cfiCode);
                }

                mdReqID += "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                mktDataReq.MDReqID = new QuickFix.Fields.MDReqID(mdReqID);
                mktDataReq.SubscriptionRequestType = new QuickFix.Fields.SubscriptionRequestType(QuickFix.Fields.SubscriptionRequestType.SNAPSHOT_PLUS_UPDATES); // Subscribe + update
                mktDataReq.MDBookType = new QuickFix.Fields.MDBookType(QuickFix.Fields.MDBookType.ORDER_DEPTH); //MBO

                Session.SendToTarget(mktDataReq, _session);
            }
            catch (Exception ex)
            {
                logger.Error("RequestMarketData: " + ex.Message, ex);
            }

            return mdReqID;
        }

        public  void CancelarAssinaturaMarketDataConflated(string instrumento, string securityType, string product, string cfiCode, string mdReqID)
        {
            try
            {
                logger.InfoFormat("CancelandoAssinatura MD Instrumento[{0}] Produto [{1}] Type [{2}] CFI[{3}] ReqID[{4}]",
                    instrumento,
                    product,
                    securityType,
                    cfiCode,
                    mdReqID);

                QuickFix.FIX44.MarketDataRequest mktDataReq = new MarketDataRequest();

                if (!String.IsNullOrEmpty(instrumento))
                {
                    QuickFix.FIX44.MarketDataRequest.NoRelatedSymGroup noRelatedSym = new MarketDataRequest.NoRelatedSymGroup();

                    noRelatedSym.SecurityExchange = new QuickFix.Fields.SecurityExchange("BVMF");
                    noRelatedSym.SecurityIDSource = new QuickFix.Fields.SecurityIDSource("8");
                    noRelatedSym.SecurityID = new QuickFix.Fields.SecurityID(instrumento);

                    mktDataReq.AddGroup(noRelatedSym);
                }

                if (!String.IsNullOrEmpty(product) && !product.Equals ("0") )
                    mktDataReq.Product = new QuickFix.Fields.Product(Convert.ToInt32(product));

                if (!String.IsNullOrEmpty(securityType))
                    mktDataReq.SecurityType = new QuickFix.Fields.SecurityType(securityType);

                if (!String.IsNullOrEmpty(cfiCode))
                {
                    mktDataReq.CFICode = new QuickFix.Fields.CFICode(cfiCode);
                }

                mktDataReq.MDReqID = new QuickFix.Fields.MDReqID(mdReqID);
                mktDataReq.SubscriptionRequestType = new QuickFix.Fields.SubscriptionRequestType(QuickFix.Fields.SubscriptionRequestType.DISABLE_PREVIOUS); // Subscribe + update
                mktDataReq.MDBookType = new QuickFix.Fields.MDBookType(QuickFix.Fields.MDBookType.ORDER_DEPTH); //MBO

                Session.SendToTarget(mktDataReq, _session);
            }
            catch (Exception ex)
            {
                logger.Error("CancelarAssinaturaMarketDataConflated: " + ex.Message, ex);
            }
        }

        internal bool EnviarResendRequestConflated(int seqNoIni, int seqNoFim)
        {
            try
            {
                QuickFix.FIX44.ResendRequest ressReq = new ResendRequest();

                ressReq.BeginSeqNo = new QuickFix.Fields.BeginSeqNo(seqNoIni);
                ressReq.EndSeqNo = new QuickFix.Fields.EndSeqNo(seqNoFim);

                Session.SendToTarget(ressReq, _session);
            }
            catch (Exception ex)
            {
                logger.Error("RequestMarketData: " + ex.Message, ex);
                return false;
            }

            return true;
        }

        internal bool EnviarSequenceResetConflated(int newSeqNo)
        {
            try
            {
                QuickFix.FIX44.SequenceReset seqReset = new SequenceReset();
                seqReset.NewSeqNo = new QuickFix.Fields.NewSeqNo(newSeqNo);

                Session.SendToTarget(seqReset, _session);
            }
            catch (Exception ex)
            {
                logger.Error("RequestMarketData: " + ex.Message, ex);
                return false;
            }

            return true;
        }

        public QuickFix.ILog Create(SessionID sessionID)
        {
            return this;
        }

        void QuickFix.ILog.Clear()
        {
        }

        void QuickFix.ILog.OnEvent(string s)
        {
            logger.Info("Fix Event [" + s + "]");
        }

        void QuickFix.ILog.OnIncoming(string msg)
        {
        }

        void QuickFix.ILog.OnOutgoing(string msg)
        {
        }

        void IDisposable.Dispose()
        {
        }

    }
}
