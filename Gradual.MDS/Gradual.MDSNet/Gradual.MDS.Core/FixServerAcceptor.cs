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
    /// Classe Acceptor de comunicacao FIX para o Servico TCPReplay
    /// </summary>
    public class FixServerAcceptor : QuickFix.MessageCracker, QuickFix.IApplication
    {
        private log4net.ILog logger;

        private ThreadedSocketAcceptor _acceptor;
        private TCPReplayConfig _tcpReplayConfig;
        private Dictionary<string, SessionID> _dctSessionsFixClients;
        private Dictionary<string, SessionID> _dctSessionsFixChannels;
        private Context context;
        private TemplateRegistry registry = null;

        public FixServerAcceptor(
            TCPReplayConfig tcpReplayConfig,
            Dictionary<string, SessionID> dctSessionsFixClients,
            Dictionary<string, SessionID> dctSessionsFixChannels)
        {
            _tcpReplayConfig = tcpReplayConfig;
            _dctSessionsFixClients = dctSessionsFixClients;
            _dctSessionsFixChannels = dctSessionsFixChannels;

            logger = LogManager.GetLogger("FixServerAcceptor");

            MDSUtils.AddAppender("FixServerAcceptor", logger.Logger);

            XMLMessageTemplateLoader loader = new XMLMessageTemplateLoader();
            registry = UmdfUtils.loadTemplates(tcpReplayConfig.TemplateFile);
            context = new Context();
            context.TemplateRegistry = registry;

            try
            {
                // Cria dicionario da configuracao 
                QuickFix.Dictionary mainDic = new QuickFix.Dictionary();
                mainDic.SetLong("SocketAcceptPort", tcpReplayConfig.SocketAcceptPort);
                mainDic.SetBool("ResetOnLogon", tcpReplayConfig.ResetOnLogon);
                mainDic.SetBool("ResetOnDisconnect", tcpReplayConfig.ResetOnDisconnect);
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

                IEnumerable<int> rangeSenderSubID = Enumerable.Range(
                    tcpReplayConfig.SubIDStartSeq, 
                    tcpReplayConfig.SubIDEndSeq);
                foreach (int item in rangeSenderSubID)
                {
                    string subID = tcpReplayConfig.SubIDPrefix + item.ToString("D3");

                    // Cria sessao FIX
                    SessionID sessionID = new QuickFix.SessionID(
                        tcpReplayConfig.BeginString,
                        tcpReplayConfig.SenderCompID,
                        subID,
                        tcpReplayConfig.TargetCompID,
                        subID);

                    sessDic.SetString("SenderSubID", subID);
                    sessDic.SetString("TargetSubID", subID);
                    settings.Set(sessionID, sessDic);
                }

                logger.InfoFormat("Start(): iniciando FIX ACCEPTOR na porta {0}...", tcpReplayConfig.SocketAcceptPort);
                _acceptor = new ThreadedSocketAcceptor(this, store, settings, log, message);
                _acceptor.Start();
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
                _acceptor.Stop();
                _acceptor = null;

                Dictionary<string, SessionID>.Enumerator itens = _dctSessionsFixClients.GetEnumerator();
                while (itens.MoveNext())
                {
                    KeyValuePair<string, SessionID> item = itens.Current;
                    SessionID session = item.Value;
                    session = null;
                }
                _dctSessionsFixClients.Clear();
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

                _dctSessionsFixClients.Add(session.ToString(), session);
            }
            catch (Exception ex)
            {
                logger.Error("OnCreate(): " + ex.Message, ex);
            }
        }

        public virtual void OnLogon(QuickFix.SessionID session)
        {
            logger.InfoFormat("OnLogon() sessionID[{0}]", session.ToString());
        }

        public virtual void OnLogout(QuickFix.SessionID session)
        {
            logger.InfoFormat("OnLogout() sessionID[{0}]", session.ToString());
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
                string applReqID = message.ApplReqID.ToString();
                logger.InfoFormat("SessionID[{0}]: ApplicationMessageRequest ApplReqID[{1}]", sessionID, applReqID);

                // Acrescenta o sessionID do cliente para controle
                applReqID = applReqID + "|" + sessionID;
                message.Set(new QuickFix.Fields.ApplReqID(applReqID));

                string[] quebraApplReqID = applReqID.Split("-".ToCharArray());
                string channelID = quebraApplReqID[1];

                logger.DebugFormat("SessionID[{0}]: ApplicationMessageRequest enviando para ChannelID[{1}] msg[{2}]", 
                    sessionID, channelID, message.ToString());
                bool bRet = Session.SendToTarget(message, _dctSessionsFixChannels[channelID]);
                if (!bRet)
                {
                    logger.ErrorFormat("SessionID[{0}]: Falha ApplicationMessageRequest msg[{1}]", sessionID, message.ToString());
                }
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
                logger.DebugFormat("ApplicationMessageRequestAck ApplReqID[{0}] msg[{1}]", message.ApplReqID.ToString(), message.ToString());
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
                logger.DebugFormat("ApplicationMessageReport ApplReqID[{0}] msg[{1}]", message.ApplReqID.ToString(), message.ToString());
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
                logger.DebugFormat("ApplicationRawDataReporting ApplReqID[{0}] msg[{1}]", message.ApplReqID.ToString(), message.ToString());
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
                logger.DebugFormat("UnsupportedMessageType msg[{1}]", message.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("onMessage(UnsupportedMessageType): " + ex.Message, ex);
            }
        }
        #endregion // Quickfix Application Members
    }
}
