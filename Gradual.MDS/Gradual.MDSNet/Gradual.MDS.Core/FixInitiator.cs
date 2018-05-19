using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Classe Base da comunicacao Fix com o Servico TCPReplay
    /// </summary>
    public class FixInitiator : QuickFix.MessageCracker, QuickFix.IApplication
    {
        protected log4net.ILog logger;

        protected bool _logonEfetuado = false;
        protected bool _bConectadoBolsa = false;
        protected ChannelUMDFConfig _channelUmdfConfig;
        protected MarketIncrementalProcessor _mktIncProc;
        protected Queue<UdpPacket> _qUdpPkt;
        protected QuickFix.SessionID _session;
        protected QuickFix.Transport.SocketInitiator _initiator;
        protected object _lockObject;
        protected Context context;
        protected TemplateRegistry registry = null;
        protected Dictionary<string, ListChannelQueues> listaChannelQueues = new Dictionary<string, ListChannelQueues>();
        private Dictionary<string, QuickFix.FIX44.ApplicationMessageRequest> dctReqSent = new Dictionary<string, QuickFix.FIX44.ApplicationMessageRequest>();

        public class ListChannelQueues
        {
            public Queue<UdpPacket> qUdpPkt { get; set; }
            public Object replayLockObject { get; set; }

            public ListChannelQueues(Queue<UdpPacket> qUdpPkt, Object replayLockObject)
            {
                this.qUdpPkt = qUdpPkt;
                this.replayLockObject = replayLockObject;
            }
        }

        public FixInitiator(MarketIncrementalProcessor mktIncProc, ChannelUMDFConfig channelUmdfConfig, string templateFile, Queue<UdpPacket> qUdpPkt, Object replayLockObject)
        {
            logger = LogManager.GetLogger("FixInitiator-" + channelUmdfConfig.ChannelID);

            MDSUtils.AddAppender("FixInitiator-" + channelUmdfConfig.ChannelID, logger.Logger);

            XMLMessageTemplateLoader loader = new XMLMessageTemplateLoader();
            registry = UmdfUtils.loadTemplates(templateFile);
            context = new Context();
            context.TemplateRegistry = registry;

            _mktIncProc = mktIncProc;
            _channelUmdfConfig = channelUmdfConfig;

            if (!listaChannelQueues.ContainsKey(channelUmdfConfig.ChannelID))
                listaChannelQueues.Add(channelUmdfConfig.ChannelID, new ListChannelQueues(qUdpPkt, replayLockObject));

            logger.Info("Start(): iniciando sessao FIX...");
            try
            {
                // Cria sessao FIX
                _session = new QuickFix.SessionID(
                    _channelUmdfConfig.TCPConfig.BeginString,
                    _channelUmdfConfig.TCPReplayConfig.SenderCompID,
                    _channelUmdfConfig.TCPReplayConfig.SubID,
                    _channelUmdfConfig.TCPReplayConfig.TargetCompID,
                    _channelUmdfConfig.TCPReplayConfig.SubID);

                // Cria dicionario da configuracao 
                QuickFix.Dictionary mainDic = new QuickFix.Dictionary();
                mainDic.SetLong("SocketConnectPort", _channelUmdfConfig.TCPReplayConfig.SocketConnectPort);
                mainDic.SetLong("HeartBtInt", _channelUmdfConfig.TCPConfig.HeartBtInt);
                mainDic.SetLong("ReconnectInterval", _channelUmdfConfig.TCPConfig.ReconnectInterval);
                mainDic.SetBool("ResetOnLogon", _channelUmdfConfig.TCPConfig.ResetOnLogon);
                mainDic.SetBool("ResetOnLogout", _channelUmdfConfig.TCPConfig.ResetOnLogout);
                mainDic.SetBool("ResetOnDisconnect", _channelUmdfConfig.TCPConfig.ResetOnDisconnect);
                mainDic.SetBool("PersistMessages", _channelUmdfConfig.TCPConfig.PersistMessages);
                mainDic.SetString("ConnectionType", _channelUmdfConfig.TCPConfig.ConnectionType);
                mainDic.SetString("SocketConnectHost", _channelUmdfConfig.TCPReplayConfig.SocketConnectHost);
                mainDic.SetString("FileStorePath", _channelUmdfConfig.TCPConfig.FileStorePath);
                mainDic.SetString("FileLogPath", _channelUmdfConfig.TCPConfig.FileLogPath);
                mainDic.SetString("StartTime", _channelUmdfConfig.TCPReplayConfig.StartTime);
                mainDic.SetString("EndTime", _channelUmdfConfig.TCPReplayConfig.EndTime);

                QuickFix.Dictionary sessDic = new QuickFix.Dictionary();
                sessDic.SetString("BeginString", _channelUmdfConfig.TCPConfig.BeginString);
                sessDic.SetString("SenderCompID", _channelUmdfConfig.TCPReplayConfig.SenderCompID);
                sessDic.SetString("SenderSubID", _channelUmdfConfig.TCPReplayConfig.SubID);
                sessDic.SetString("TargetCompID", _channelUmdfConfig.TCPReplayConfig.TargetCompID);
                sessDic.SetString("TargetSubID", _channelUmdfConfig.TCPReplayConfig.SubID);
                sessDic.SetString("DataDictionary", _channelUmdfConfig.TCPConfig.DataDictionary);
                sessDic.SetBool("UseDataDictionary", true);

                // Configure the session settings
                QuickFix.SessionSettings settings = new QuickFix.SessionSettings();

                settings.Set(mainDic);
                settings.Set(_session, sessDic);

                MemoryStoreFactory store = new MemoryStoreFactory();
                FileLogFactory log = new FileLogFactory(settings);
                IMessageFactory message = new DefaultMessageFactory();

                // Cria o socket
                _initiator = new QuickFix.Transport.SocketInitiator(this, store, settings, log, message);
                _initiator.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Start():" + ex.Message, ex);
            }

            logger.Info("Start(): Sessao FIX iniciado!");
        }

        public virtual void Stop()
        {
            try
            {
                _initiator.Stop();
                _initiator = null;
                _session = null;
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

        public virtual void OnMessage(QuickFix.FIX44.ApplicationMessageRequestAck message, SessionID session)
        {
            try
            {
                logger.Info("*** Iniciando recepcao da Retransmissao");
                logger.Info("ApplReqID........: " + message.GetString(QuickFix.Fields.Tags.ApplReqID));
                logger.Info("ApplReqType......: " + message.GetString(QuickFix.Fields.Tags.ApplReqType));
                logger.Info("ApplResponseID...: " + message.GetString(QuickFix.Fields.Tags.ApplRespID));
                logger.Info("ApplResponseType.: " + message.GetString(QuickFix.Fields.Tags.ApplRespType));

                int applRespType = Convert.ToInt32(message.ApplRespType.ToString());

                if (message.IsSetField(QuickFix.Fields.Tags.NoApplIDs))
                {
                    int numApplIDs = message.GetInt(QuickFix.Fields.Tags.NoApplIDs);
                    logger.Info("ApplIDs..........: " + numApplIDs);

                    for (int numApplID = 1; numApplID <= numApplIDs; numApplID++)
                    {
                        QuickFix.FIX44.ApplicationMessageRequestAck.NoApplIDsGroup groupApplID = new QuickFix.FIX44.ApplicationMessageRequestAck.NoApplIDsGroup();
                        message.GetGroup(numApplID, groupApplID);

                        logger.Info("RefApplID[" + numApplID + "].....:" + groupApplID.GetString(QuickFix.Fields.Tags.RefApplID));
                        int beginSeqNum = 0;
                        int endSeqNum = 0;
                        if (groupApplID.IsSetField(QuickFix.Fields.Tags.ApplBegSeqNum))
                        {
                            beginSeqNum = groupApplID.GetInt(QuickFix.Fields.Tags.ApplBegSeqNum);
                            logger.Info("ApplBegSeqNum[" + numApplID + "].:" + beginSeqNum);
                        }
                        if (groupApplID.IsSetField(QuickFix.Fields.Tags.ApplEndSeqNum))
                        {
                            endSeqNum = groupApplID.GetInt(QuickFix.Fields.Tags.ApplEndSeqNum);
                            logger.Info("ApplEndSeqNum[" + numApplID + "].:" + endSeqNum);
                        }
                        if (groupApplID.IsSetField(QuickFix.Fields.Tags.ApplRespError))
                            logger.Info("ApplResponseError[" + numApplID + "].:" + groupApplID.GetString(QuickFix.Fields.Tags.ApplRespError));

                        // Se applRespType == 3, o server estava ocupado atendendo outra requisicao
                        // reenvia
                        if (applRespType == 3)
                        {
                            SendMessageRequest(beginSeqNum, endSeqNum);
                        }
                    }
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
                logger.Info("*** Relatorio de Retransmissao");
                logger.Info("ApplReportID.....: " + message.GetString(QuickFix.Fields.Tags.ApplReportID));
                logger.Info("ApplReportType...: " + message.GetString(QuickFix.Fields.Tags.ApplReportType));

                int applReportType = Convert.ToInt32(message.ApplReportType.ToString());

                string lastSeqNum = "";
                if (message.IsSetField(QuickFix.Fields.Tags.NoApplIDs))
                {
                    int numApplIDs = message.GetInt(QuickFix.Fields.Tags.NoApplIDs);
                    logger.Info("ApplIDs..........: " + numApplIDs);
                    for (int numApplID = 1; numApplID <= numApplIDs; numApplID++)
                    {
                        QuickFix.FIX44.ApplicationMessageReport.NoApplIDsGroup groupApplID = new QuickFix.FIX44.ApplicationMessageReport.NoApplIDsGroup();
                        message.GetGroup(numApplID, groupApplID);

                        logger.Info("RefApplID[" + numApplID + "].....:" + groupApplID.GetString(QuickFix.Fields.Tags.RefApplID));
                        if (groupApplID.IsSetField(QuickFix.Fields.Tags.RefApplLastSeqNum))
                        {
                            lastSeqNum = groupApplID.GetString(QuickFix.Fields.Tags.RefApplLastSeqNum);
                            logger.Info("RefApplLastSeqNum[" + numApplID + "].:" + lastSeqNum);
                        }

                        if (groupApplID.IsSetField(QuickFix.Fields.Tags.ApplRespError))
                            logger.Info("ApplResponseError[" + numApplID + "].:" + groupApplID.GetString(QuickFix.Fields.Tags.ApplRespError));
                    }
                }
                string[] quebraApplReqID = message.GetString(QuickFix.Fields.Tags.ApplReqID).Split("-".ToCharArray());
                string channelID = quebraApplReqID[1];

                if (applReportType == 3)
                {
                    logger.Info("Libera ChannelID: [" + channelID + "]");
                    lock (listaChannelQueues[channelID].replayLockObject)
                    {
                        Monitor.Pulse(listaChannelQueues[channelID].replayLockObject);
                    }
                }
                else
                {
                    logger.Warn("Application Resend Error, nao libera channelID [" + channelID + "] devera reiniciar por timeout");

                    if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday && DateTime.Now.DayOfWeek != DayOfWeek.Saturday)
                    {
                        if (DateTime.Now.Hour >= ConstantesMDS.HORARIO_NOTIFICACAO_EMAIL_INICIO &&
                            DateTime.Now.Hour < ConstantesMDS.HORARIO_NOTIFICACAO_EMAIL_FIM)
                        {

                            MDSUtils.EnviarEmail("Erro em recovery [" + channelID + "]", "Application Resend Error, nao libera channelID [" + channelID + "] devera reiniciar por timeout");
                        }
                    }
                }

                logger.Info("Final da Retransmissao ***");
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
                string applReqID = message.GetString(QuickFix.Fields.Tags.ApplReqID);
                logger.Info("*** Recebeu pacote RAW ***");
                logger.Info("ApplReqID .......: " + applReqID);
                logger.Info("ApplRespID ......: " + message.GetString(QuickFix.Fields.Tags.ApplRespID));
                logger.Info("ApplID ..........: " + message.GetString(QuickFix.Fields.Tags.ApplID));
                logger.Info("ApplResendFlag ..: " + message.GetString(QuickFix.Fields.Tags.ApplResendFlag));
                logger.Info("RawDataLength ...: " + message.GetString(QuickFix.Fields.Tags.RawDataLength));
                logger.Info("TotNumReports ...: " + message.GetString(QuickFix.Fields.Tags.TotNumReports));

                byte[] rawData = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(message.RawData.getValue());
                string[] quebraApplReqID = applReqID.Split("-".ToCharArray());
                string channelID = quebraApplReqID[1];

                logger.Debug("rawData.Length: " + rawData.Length + " (ChannelID:" + channelID + ")");
                //dumpFastData(rawData, 0, message.RawDataLength.getValue());

                int numApplSeqNums = message.GetInt(QuickFix.Fields.Tags.NoApplSeqNums);
                for (int i = 1; i <= numApplSeqNums; i++)
                {
                    try
                    {
                        QuickFix.FIX44.ApplicationRawDataReporting.NoApplSeqNumsGroup groupApplSeqNum = new QuickFix.FIX44.ApplicationRawDataReporting.NoApplSeqNumsGroup();
                        message.GetGroup(i, groupApplSeqNum);

                        int applSeqNum = groupApplSeqNum.GetInt(QuickFix.Fields.Tags.ApplSeqNum);
                        int rawDataOffSet = groupApplSeqNum.GetInt(QuickFix.Fields.Tags.RawDataOffset);
                        int rawDataLength = groupApplSeqNum.GetInt(QuickFix.Fields.Tags.RawDataLength);
                        int lastSeqNum = groupApplSeqNum.GetInt(QuickFix.Fields.Tags.ApplLastSeqNum);

                        logger.Info("ApplSeqNum[" + i + "]......: " + applSeqNum);
                        logger.Info("ApplLastSeqNum[" + i + "]..: " + lastSeqNum);
                        logger.Info("RawDataOffSet[" + i + "]...: " + rawDataOffSet);
                        logger.Info("RawDataLength[" + i + "]...: " + rawDataLength);

                        //dumpFastData(rawData, rawDataOffSet, rawDataLength);

                        MemoryStream byteIn = new MemoryStream(rawData, rawDataOffSet, rawDataLength);
                        //FastDecoder decoder = new FastDecoder(context, byteIn);
                        //OpenFAST.Message mensagem = decoder.ReadMessage();

                        byte[] seqNum = new byte[4];
                        byte[] chunk = new byte[2];
                        byte[] msgLength = new byte[2];

                        seqNum[0] = (byte)((applSeqNum & 0xFF000000) >> 24);
                        seqNum[1] = (byte)((applSeqNum & 0x00FF0000) >> 16);
                        seqNum[2] = (byte)((applSeqNum & 0x0000FF00) >> 8);
                        seqNum[3] = (byte)(applSeqNum & 0x000000FF);

                        int numChunk = 1;
                        chunk[0] = (byte)((numChunk & 0x0000FF00) >> 8);
                        chunk[1] = (byte)(numChunk & 0x000000FF);

                        msgLength[0] = (byte)((rawDataLength & 0x0000FF00) >> 8);
                        msgLength[1] = (byte)(rawDataLength & 0x000000FF);

                        byte[] umdfPacketBuffer = new byte[10 + rawDataLength];
                        System.Array.Copy(seqNum, 0, umdfPacketBuffer, 0, 4);
                        System.Array.Copy(chunk, 0, umdfPacketBuffer, 4, 2);
                        System.Array.Copy(chunk, 0, umdfPacketBuffer, 6, 2);
                        System.Array.Copy(msgLength, 0, umdfPacketBuffer, 8, 2);
                        System.Array.Copy(rawData, rawDataOffSet, umdfPacketBuffer, 10, rawDataLength);

                        UdpPacket packet = new UdpPacket();
                        packet.byteData = umdfPacketBuffer;
                        packet.pktLength = rawDataLength + 10;
                        packet.pktTimestamp = DateTime.Now.Ticks;
                        packet.feeder = UdpPacket.FEEDER_REPLAY;

                        dumpFastData(umdfPacketBuffer, 0, rawDataLength + 10);

                        lock (listaChannelQueues[channelID].qUdpPkt)
                        {
                            listaChannelQueues[channelID].qUdpPkt.Enqueue(packet);
                            Monitor.Pulse(listaChannelQueues[channelID].qUdpPkt);
                        }

                    }
                    catch (Exception ex)
                    {
                        logger.Error("onMessage(ApplicationRawDataReporting): " + ex.Message, ex);
                    }

                    lock (listaChannelQueues[channelID].replayLockObject)
                    {
                        Monitor.Pulse(listaChannelQueues[channelID].replayLockObject);
                    }
                }
                logger.Info("*** Fim do pacote RAW ***");
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

        private void dumpFastData(byte[] encodedData, int rawDataOffSet, int rawDataLength)
        {
            StringBuilder buffer = new StringBuilder();

            logger.Debug("*** Encoded Data");
            for (int i = 1; i < rawDataLength; i++)
            {
                buffer.AppendFormat("{0:x2}", encodedData[rawDataOffSet + i - 1]);
                buffer.Append(" ");
                if ((i % 10) == 0 || (i == rawDataLength - 1))
                {
                    logger.Debug("[" + buffer.ToString() + "]");
                    buffer.Clear();
                }
            }
            logger.Debug("*** End of Encoded Data");
        }

        public virtual void SendMessageRequest(int applBegSeqNum, int applEndSeqNum)
        {
            QuickFix.FIX44.ApplicationMessageRequest request = new QuickFix.FIX44.ApplicationMessageRequest();
            try
            {
                //_mktIncProc.HabilitarProcessamentoMensagens = false;

                StringBuilder applReqID = new StringBuilder();
                applReqID.Append(DateTime.Now.ToString("yyyyMMddHHmmss"));
                applReqID.Append("-");
                applReqID.Append(_channelUmdfConfig.ChannelID);
                applReqID.Append("-");
                applReqID.Append(applBegSeqNum.ToString());
                applReqID.Append("-");
                applReqID.Append(applEndSeqNum.ToString());

                logger.Info("Solicitando replay. ApplReqID[" + applReqID.ToString() + "]");

                request.Set(new QuickFix.Fields.ApplReqID(applReqID.ToString()));
                request.Set(new QuickFix.Fields.ApplReqType(0));

                QuickFix.FIX44.ApplicationMessageRequest.NoApplIDsGroup groupApplIDs = new QuickFix.FIX44.ApplicationMessageRequest.NoApplIDsGroup();
                groupApplIDs.Set(new QuickFix.Fields.RefApplID(_channelUmdfConfig.ChannelID));
                groupApplIDs.Set(new QuickFix.Fields.ApplBegSeqNum(applBegSeqNum));
                groupApplIDs.Set(new QuickFix.Fields.ApplEndSeqNum(applEndSeqNum));

                request.AddGroup(groupApplIDs);

                bool bRet = Session.SendToTarget(request, _session);
                if (!bRet)
                {
                    logger.Error("Falha na solicitacao do replay. ApplReqID[" + applReqID.ToString() + "]");
                }
            }
            catch (Exception ex)
            {
                logger.Error("SendMessageRequest(): " + ex.Message, ex);
            }
        }
    }
}
