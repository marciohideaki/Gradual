using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Core.Lib;
using OpenFAST;
using Gradual.MDS.Eventos.Lib.ControlEvents;
using Gradual.MDS.Eventos.Lib;
using System.Threading;
using log4net;

namespace Gradual.MDS.Core
{
    public class MarketRecoveryProcessor : AsyncUdpClient
    {
        private ChannelUMDFState channelState;
        private ChannelUMDFConfig channelConfig;
        private bool bCanProcess = false;
        private string[] listaRecoveryTemplateID;
        private MonitorConfig monitorConfig;

        #region ctor
        public MarketRecoveryProcessor(ChannelUMDFState state, ChannelUMDFConfig config, MonitorConfig monitorConfig) :
            base(config.MDRecoveryHost, config.MDRecoveryPorta, config.MDRecoveryHostSec, 
                config.MDRecoveryPortaSec, config.TemplateFile, config.ChannelID, config.LocalInterfaceAddress)
        {
            this.channelConfig = config;
            this.channelState = state;
            this.processorType = ProcessorType.MarketRecovery;
            this.monitorConfig = monitorConfig;

            logger = LogManager.GetLogger("MarketRecoveryProcessor-" + config.ChannelID);

            MDSUtils.AddAppender("MarketRecoveryProcessor-" + config.ChannelID, logger.Logger);

        }
        #endregion //ctor

        #region Event handling
        public delegate void OnRecoveryCompletedHandler(object sender, RecoveryCompletedEventArgs args);
        public event OnRecoveryCompletedHandler OnRecoveryCompleted;

        public delegate void OnRecoveryStartedHandler(object sender, RecoveryStartedEventArgs args);
        public event OnRecoveryStartedHandler OnRecoveryStarted;
        #endregion

        #region AsyncUdpClient Overrides
        protected override void umdfPacketProcessor() { }

        protected override void umdfMessageProcessor()
        {
		    try
		    {
			    logger.Debug("Processando mensagens de recovery");
			
			    string recoveryTemplateID = channelConfig.MDRecoveryTemplateID.ToString();
                listaRecoveryTemplateID = recoveryTemplateID.Split(",".ToCharArray());
                long lastLogTicks = 0;

                while (bKeepRunning)
			    {

                    try
                    {
                        //Message message = this.filaMensagensUMDF.take();
                        Message message;
                        if (!queueToProcessor.TryDequeue(out message))
                        {
                            Thread.Sleep(50);
                            continue;
                        }

                        // Notifica o fim do snapshot para inicio do tratamento do incremental
                        if (listaRecoveryTemplateID.Any(message.Template.Id.Contains))
                        {
                            List<Message> mensagens = UmdfUtils.splitMessage(message, channelConfig.MarketDepth, ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE);

                            foreach (Message newMessage in mensagens)
                            {
                                MDSUtils.EnqueueEventoUmdf(newMessage,
                                    this.channelConfig.MDRecoveryTemplateID,
                                    this.channelConfig.ChannelID,
                                    this.channelConfig.Segment,
                                    ConstantesUMDF.FAST_MSGTYPE_SNAPSHOT_SINGLE,
                                    channelConfig.MarketDepth,
                                    StreamTypeEnum.STREAM_TYPE_MARKET_RECOVERY);

                                if (MDSUtils.shouldLog(lastLogTicks))
                                {
                                    lastLogTicks = DateTime.UtcNow.Ticks;

                                    string msgTruncado = (message.ToString().Length < 200 ? message.ToString() : message.ToString().Substring(0, 200));
                                    monitorConfig.channels[channelConfig.ChannelID].dateTimeStartedStopped =
                                        "Snapshot - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + msgTruncado;
                                    monitorConfig.channels[channelConfig.ChannelID].AddDetails(
                                        "2) Snapshot", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), msgTruncado);
                                }
                            }

                            if (message.IsDefined("MsgSeqNum") && message.IsDefined("TotNumReports"))
                            {
                                int seqNum = message.GetInt("MsgSeqNum"); //34
                                int totNumReports = message.GetInt("TotNumReports"); //911

                                if (seqNum == 1)
                                {
                                    int LastMsgSeqNumProcessed = message.GetInt("LastMsgSeqNumProcessed");
                                    bCanProcess = true;

                                    logger.Info("Inicio do processamento do snapshot: " + totNumReports + " msgs");
                                    logger.Info("Notificando observers, LastMsgSeqNumProcessed:" + LastMsgSeqNumProcessed);

                                    RecoveryStartedEventArgs e = new RecoveryStartedEventArgs();
                                    e.LastMsgSeqNumProcessed = message.GetInt("LastMsgSeqNumProcessed");
                                    if (this.OnRecoveryStarted != null)
                                    {
                                        OnRecoveryStarted(this, e);
                                    }
                                }

                                if (seqNum == totNumReports && bCanProcess)
                                {
                                    int LastMsgSeqNumProcessed = message.GetInt("LastMsgSeqNumProcessed");

                                    logger.Info("Fim do processamento do snapshot: " + totNumReports + " msgs");
                                    logger.Info("Notificando observers, LastMsgSeqNumProcessed:" + LastMsgSeqNumProcessed);

                                    RecoveryCompletedEventArgs e = new RecoveryCompletedEventArgs();
                                    e.LastMsgSeqNumProcessed = message.GetInt("LastMsgSeqNumProcessed");

                                    monitorConfig.channels[channelConfig.ChannelID].dateTimeStartedStopped =
                                        "Snapshot - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": Snapshot completo!";

                                    monitorConfig.channels[channelConfig.ChannelID].AddDetails(
                                        "2) Snapshot", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                        "Snapshot completo! Ultimo SeqNum[" + e.LastMsgSeqNumProcessed + "]");

                                    if (this.OnRecoveryCompleted != null)
                                    {
                                        OnRecoveryCompleted(this, e);
                                    }
                                    bKeepRunning = false;
                                    break;
                                }
                            }
                            else
                            {
                                logger.Error("Template ID nao reconhecido [" + message.Template.Id + "]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("umdfMessageProcessor: " + ex.Message, ex);
                        bCanProcess = false;
                    }
			    }

                Stop();
		
			    logger.Debug("Fim");
		    }
		    catch(Exception ex)
		    {
			    logger.Error("Damn: " + ex.Message, ex);
                monitorConfig.channels[channelConfig.ChannelID].RemoveDetails("2) Snapshot");
                //bKeepRunning = false;
		    }
        }
        #endregion // AsyncUdpClient Overrides
    }
}
