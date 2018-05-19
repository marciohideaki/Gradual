using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using log4net;
using QuickFix;
using OpenFAST;
using OpenFAST.Codec;
using Gradual.MDS.Core.Lib;
using OpenFAST.Template.Loader;
using OpenFAST.Template;

namespace Gradual.MDS.Core
{
    /// <summary>
    /// Classe Initiator de comunicacao FIX para o Servico TCPReplay
    /// </summary>
    public class FixServerInitiator : QuickFix.MessageCracker, QuickFix.IApplication
    {
        private log4net.ILog logger;

        private SessionID _sessionID;
        private QuickFix.Transport.SocketInitiator _initiator;
        private TCPReplayConfig _tcpReplayConfig;
        private Dictionary<string, SessionID> _dctSessionsFixClients;
        private Dictionary<string, SessionID> _dctSessionsFixChannels;
        private Context context;
        private TemplateRegistry registry = null;

        public FixServerInitiator(
            string channelsID,
            TCPReplayConfig tcpReplayConfig,
            Dictionary<string, SessionID> dctSessionsFixClients,
            Dictionary<string, SessionID> dctSessionsFixChannels)
        {
            _tcpReplayConfig = tcpReplayConfig;
            _dctSessionsFixClients = dctSessionsFixClients;
            _dctSessionsFixChannels = dctSessionsFixChannels;

            logger = LogManager.GetLogger("FixServerInitiator-" + tcpReplayConfig.ChannelID);

            MDSUtils.AddAppender("FixServerInitiator-" + tcpReplayConfig.ChannelID, logger.Logger);

            XMLMessageTemplateLoader loader = new XMLMessageTemplateLoader();
            registry = UmdfUtils.loadTemplates(tcpReplayConfig.TemplateFile);
            context = new Context();
            context.TemplateRegistry = registry;

            try
            {
                // Cria dicionario da configuracao 
                QuickFix.Dictionary mainDic = new QuickFix.Dictionary();
                mainDic.SetString("SocketConnectHost", tcpReplayConfig.SocketConnectHost);
                mainDic.SetLong("SocketConnectPort", tcpReplayConfig.SocketConnectPort);
                if (!String.IsNullOrEmpty(tcpReplayConfig.SocketConnectHost1))
                {
                    mainDic.SetString("SocketConnectHost1", tcpReplayConfig.SocketConnectHost1);
                    mainDic.SetLong("SocketConnectPort1", tcpReplayConfig.SocketConnectPort1);
                }
                mainDic.SetLong("HeartBtInt", tcpReplayConfig.HeartBtInt);
                mainDic.SetLong("ReconnectInterval", tcpReplayConfig.ReconnectInterval);
                mainDic.SetBool("ResetOnLogon", tcpReplayConfig.ResetOnLogon);
                mainDic.SetBool("PersistMessages", tcpReplayConfig.PersistMessages);
                mainDic.SetString("ConnectionType", tcpReplayConfig.ConnectionType);
                mainDic.SetString("FileStorePath", tcpReplayConfig.FileStorePath);
                mainDic.SetString("FileLogPath", tcpReplayConfig.FileLogPath);
                mainDic.SetString("StartTime", tcpReplayConfig.StartTime);
                mainDic.SetString("EndTime", tcpReplayConfig.EndTime);

                QuickFix.Dictionary sessDic = new QuickFix.Dictionary();
                sessDic.SetString("BeginString", tcpReplayConfig.BeginString);
                sessDic.SetString("SenderCompID", tcpReplayConfig.SenderCompID);
                sessDic.SetString("TargetCompID", tcpReplayConfig.TargetCompID);
                sessDic.SetString("DataDictionary", tcpReplayConfig.DataDictionary);
                sessDic.SetBool("UseDataDictionary", true);

                // Configure the session settings
                QuickFix.SessionSettings settings = new QuickFix.SessionSettings();

                settings.Set(mainDic);

                MemoryStoreFactory store = new MemoryStoreFactory();
                FileLogFactory log = new FileLogFactory(settings);
                IMessageFactory message = new DefaultMessageFactory();

                // Cria sessao FIX
                _sessionID = new QuickFix.SessionID(
                    tcpReplayConfig.BeginString,
                    tcpReplayConfig.SenderCompID,
                    tcpReplayConfig.TargetCompID);

                settings.Set(_sessionID, sessDic);

                string[] quebraChannelsID = channelsID.Split(",".ToCharArray());
                foreach (string channel in quebraChannelsID)
                {
                    dctSessionsFixChannels.Add(channel, _sessionID);
                }

                logger.InfoFormat("Start(): iniciando FIX com sessionID[{0}]...", _sessionID.ToString());
                _initiator = new QuickFix.Transport.SocketInitiator(this, store, settings, log, message);
                _initiator.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Start():" + ex.Message, ex);
            }

            logger.Info("Start(): Sessao FIX iniciada!");
        }

        public void Stop()
        {
            try
            {
                _initiator.Stop();
                _initiator = null;
            }
            catch (Exception ex)
            {
                logger.Error("Stop():" + ex.Message, ex);
            }

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
        }

        public virtual void OnLogout(QuickFix.SessionID session)
        {
            logger.Info("OnLogout()");
        }


        public virtual void ToAdmin(QuickFix.Message message, QuickFix.SessionID session)
        {
            try
            {
                logger.Debug("SND(Admin) --> type[" + message.GetType().ToString() + "] msg[" + message.ToString() + "]");
                //if (message.Header.GetField(QuickFix.Fields.Tags.MsgType) != QuickFix.Fields.MsgType.HEARTBEAT)
                //    this.Crack(message, session);
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
                logger.Debug("SND(App) --> type[" + message.GetType().ToString() + "] msg[" + message.ToString() + "]");
                this.Crack(message, session);
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
                logger.Debug("OnMessage(Heartbeat)");
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

        public virtual void OnMessage(QuickFix.FIX44.ApplicationMessageRequest message, SessionID session)
        {
            try
            {
                string sessionID = session.ToString();
                logger.InfoFormat("SessionID[{0}]: ApplicationMessageRequest ApplReqID[{1}] msg[{2}]",
                    sessionID, message.ApplReqID.ToString(), message.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(ApplicationMessageRequest): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.ApplicationMessageRequestAck message, SessionID session)
        {
            try
            {
                string[] quebraApplReqID = message.ApplReqID.ToString().Split("|".ToCharArray());
                string applReqID = quebraApplReqID[0];
                string sessionIDResponse = quebraApplReqID[1];

                // Retorna o applReqID original
                message.Set(new QuickFix.Fields.ApplReqID(quebraApplReqID[0]));

                // Inverte origem e destino da mensagem, para devolver a resposta ao cliente Fix
                message.Header.SetField(new QuickFix.Fields.SenderCompID(_dctSessionsFixClients[sessionIDResponse].TargetCompID));
                message.Header.SetField(new QuickFix.Fields.SenderSubID(_dctSessionsFixClients[sessionIDResponse].TargetSubID));
                message.Header.SetField(new QuickFix.Fields.TargetCompID(_dctSessionsFixClients[sessionIDResponse].SenderCompID));
                message.Header.SetField(new QuickFix.Fields.TargetSubID(_dctSessionsFixClients[sessionIDResponse].SenderSubID));

                logger.InfoFormat("SessionID[{0}]: ApplicationMessageRequestAck enviando para sessionIDResponse[{1}] msg[{2}]",
                    session.ToString(), sessionIDResponse, message.ToString());
                bool bRet = Session.SendToTarget(message, _dctSessionsFixClients[sessionIDResponse]);
                if (!bRet)
                {
                    logger.ErrorFormat("SessionID[{0}]: Falha ApplicationMessageRequestAck sessionIDResponse[{1}] msg[{2}]",
                        session.ToString(), sessionIDResponse, message.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(ApplicationMessageRequestAck): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.ApplicationMessageReport message, SessionID session)
        {
            try
            {
                string[] quebraApplReqID = message.ApplReqID.ToString().Split("|".ToCharArray());
                string applReqID = quebraApplReqID[0];
                string sessionIDResponse = quebraApplReqID[1];

                // Retorna o applReqID original
                message.Set(new QuickFix.Fields.ApplReqID(quebraApplReqID[0]));

                // Inverte origem e destino da mensagem, para devolver a resposta ao cliente Fix
                message.Header.SetField(new QuickFix.Fields.SenderCompID(_dctSessionsFixClients[sessionIDResponse].TargetCompID));
                message.Header.SetField(new QuickFix.Fields.SenderSubID(_dctSessionsFixClients[sessionIDResponse].TargetSubID));
                message.Header.SetField(new QuickFix.Fields.TargetCompID(_dctSessionsFixClients[sessionIDResponse].SenderCompID));
                message.Header.SetField(new QuickFix.Fields.TargetSubID(_dctSessionsFixClients[sessionIDResponse].SenderSubID));

                logger.InfoFormat("SessionID[{0}]: ApplicationMessageReport enviando para sessionIDResponse[{1}] msg[{2}]",
                    session.ToString(), sessionIDResponse, message.ToString());
                bool bRet = Session.SendToTarget(message, _dctSessionsFixClients[sessionIDResponse]);
                if (!bRet)
                {
                    logger.ErrorFormat("SessionID[{0}]: Falha ApplicationMessageReport sessionIDResponse[{1}] msg[{2}]",
                        session.ToString(), sessionIDResponse, message.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(ApplicationMessageReport): " + ex.Message, ex);
            }
        }

        public virtual void OnMessage(QuickFix.FIX44.ApplicationRawDataReporting message, SessionID session)
        {
            try
            {
                //logger.InfoFormat("RawDataLength[{0}] getLength[{1}]", message.RawDataLength, message.RawData.getLength());
                UmdfUtils.dumpFastData(UmdfUtils.ENCODING.GetBytes(message.RawData.getValue()), 0, Int32.Parse(message.RawDataLength.ToString()));

                string[] quebraApplReqID = message.ApplReqID.ToString().Split("|".ToCharArray());
                string applReqID = quebraApplReqID[0];
                string sessionIDResponse = quebraApplReqID[1];

                // Retorna o applReqID original
                message.Set(new QuickFix.Fields.ApplReqID(quebraApplReqID[0]));

                // Inverte origem e destino da mensagem, para devolver a resposta ao cliente Fix
                message.Header.SetField(new QuickFix.Fields.SenderCompID(_dctSessionsFixClients[sessionIDResponse].TargetCompID));
                message.Header.SetField(new QuickFix.Fields.SenderSubID(_dctSessionsFixClients[sessionIDResponse].TargetSubID));
                message.Header.SetField(new QuickFix.Fields.TargetCompID(_dctSessionsFixClients[sessionIDResponse].SenderCompID));
                message.Header.SetField(new QuickFix.Fields.TargetSubID(_dctSessionsFixClients[sessionIDResponse].SenderSubID));

                logger.InfoFormat("SessionID[{0}]: ApplicationRawDataReporting enviando para sessionIDResponse[{1}] msg[{2}]",
                    session.ToString(), sessionIDResponse, message.ToString());
                bool bRet = Session.SendToTarget(message, _dctSessionsFixClients[sessionIDResponse]);
                if (!bRet)
                {
                    logger.ErrorFormat("SessionID[{0}]: Falha ApplicationRawDataReporting sessionIDResponse[{1}] msg[{2}]",
                        session.ToString(), sessionIDResponse, message.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(ApplicationRawDataReporting): " + ex.Message, ex);
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
        #endregion // Quickfix Application Members
    }
}
