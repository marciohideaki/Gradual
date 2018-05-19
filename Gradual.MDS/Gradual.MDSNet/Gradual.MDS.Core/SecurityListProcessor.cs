using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Core.Lib;
using OpenFAST;
using Gradual.MDS.Eventos.Lib.ControlEvents;
using System.Configuration;
using System.Threading;
using log4net;
using Gradual.MDS.Eventos.Lib;

namespace Gradual.MDS.Core
{
    public class SecurityListProcessor : AsyncUdpClient
    {
        private ChannelUMDFState channelState;
        private ChannelUMDFConfig channelConfig;
        private string[] listaSecurityListTemplateID;
        private MonitorConfig monitorConfig;

        #region ctor
        public SecurityListProcessor(ChannelUMDFState state, ChannelUMDFConfig config, MonitorConfig monitorConfig) :
            base(config.SecurityListHost, config.SecurityListPorta, config.SecurityListHostSec, 
                config.SecurityListPortaSec, config.TemplateFile, config.ChannelID,config.LocalInterfaceAddress)
        {
            this.channelConfig = config;
            this.channelState = state;
            this.processorType = ProcessorType.SecurityList;
            this.monitorConfig = monitorConfig;

            logger = LogManager.GetLogger("SecurityList-" + config.ChannelID);

            MDSUtils.AddAppender("SecurityList-" + config.ChannelID, logger.Logger );
        }
        #endregion ctor

        #region Event handling
        public delegate void OnSecurityListCompletedHandler(object sender, SecurityListCompletedEventArgs args);
        public event OnSecurityListCompletedHandler OnSecurityListCompleted;
        #endregion

        protected override void umdfPacketProcessor()
        {
        }

        protected override void umdfMessageProcessor()
        {
            logger.Debug("Templates carregados, processando pacotes");

            String securityListTemplateID = this.channelConfig.SecurityListTemplateID;
            listaSecurityListTemplateID = securityListTemplateID.Split(",".ToCharArray());
            long lastLogTicks = 0;

            bool ativaLeitura = false;
            while (bKeepRunning)
            {
                try
                {
                    Message message;
                    if (!queueToProcessor.TryDequeue(out message))
                    {
                        Thread.Sleep(50);
                        continue;
                    }

                    int seqNum = message.GetInt("MsgSeqNum");

                    // Despreza todas as mensagens ate reiniciar o SeqNum
                    if (!ativaLeitura)
                    {
                        if (seqNum == 1)
                            ativaLeitura = true;
                        else
                        {
                            logger.Debug("seqNum[" + seqNum + "] Desprezado");
                            continue;
                        }
                    }

                    if ("10".Equals(message.Template.Id))
                    {
                        logger.Debug("Sequence Reset");
                        continue;
                    }

                    logger.Debug(message);

                    if (MDSUtils.shouldLog(lastLogTicks))
                    {
                        lastLogTicks = DateTime.UtcNow.Ticks;

                        string msgTruncado = (message.ToString().Length < 200 ? message.ToString() : message.ToString().Substring(0, 200));
                        monitorConfig.channels[channelConfig.ChannelID].dateTimeStartedStopped =
                            "SecList - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + msgTruncado;
                        monitorConfig.channels[channelConfig.ChannelID].AddDetails(
                            "1) Security List", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), msgTruncado);
                    }

                    if (listaSecurityListTemplateID.Any(message.Template.Id.Contains))
                    {
                        logger.Debug("Fetching symbols");
                        fetchSymbols(message);


                        if (message.IsDefined("LastFragment"))
                        {
                            String isLast = message.GetString("LastFragment");

                            if (isLast != null)
                            {
                                if (isLast.ToLowerInvariant().Equals("y") ||
                                        isLast.Equals("1"))
                                {
                                    logger.Debug("Fim da security list! isLast: " + isLast);
                                    bKeepRunning = false;

                                    monitorConfig.channels[channelConfig.ChannelID].dateTimeStartedStopped =
                                        "SecList - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": Security List completo!";

                                    monitorConfig.channels[channelConfig.ChannelID].AddDetails(
                                        "1) Security List", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                        "Security List completo! Ultimo SeqNum[" + message.GetInt("MsgSeqNum") + "]");

                                    if (OnSecurityListCompleted != null)
                                    {
                                        SecurityListCompletedEventArgs args = new SecurityListCompletedEventArgs();
                                        OnSecurityListCompleted(this, args);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Damn, comecar tudo de novo: " + ex.Message, ex);
                    monitorConfig.channels[channelConfig.ChannelID].RemoveDetails("1) Security List");
                    break;
                    //bKeepRunning = false;
                }
            }

            Stop();

            logger.Info("Fim");
        }


        private void fetchSymbols(Message message)
        {
            try
            {
                SequenceValue sequence;

                if (message.IsDefined("RelatedSymbols"))
                    sequence = message.GetSequence("RelatedSymbols");
                else
                    sequence = message.GetSequence("RelatedSym");

                int ocorrencias = sequence.Length;

                logger.Debug("LISTA_INSTRUMENTO Sequencial[" +
                        message.GetInt("MsgSeqNum") +
                        "] Ocorrencias[" + ocorrencias + "]");

                List<Message> mensagens = UmdfUtils.splitMessage(message, channelConfig.MarketDepth, ConstantesUMDF.FAST_MSGTYPE_SECURITYLIST_SINGLE);

                foreach (Message newMessage in mensagens)
                {
                    MDSUtils.EnqueueEventoUmdf(newMessage,
                        this.channelConfig.SecurityListTemplateID,
                        this.channelConfig.ChannelID,
                        this.channelConfig.Segment,
                        ConstantesUMDF.FAST_MSGTYPE_SECURITYLIST_SINGLE,
                        channelConfig.MarketDepth,
                        StreamTypeEnum.STREAM_TYPE_SECURITY_DEFINITION);
                }
            }
            catch (Exception e)
            {
                logger.Error("Campo nao encontrado na mensagem: " + e.Message, e);
            }
        }

    }
}
