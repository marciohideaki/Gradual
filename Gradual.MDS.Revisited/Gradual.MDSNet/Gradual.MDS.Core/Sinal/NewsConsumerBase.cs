using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Eventos.Lib;
using System.Configuration;
using log4net;
using Gradual.MDS.Core.Lib;

namespace Gradual.MDS.Core.Sinal
{
    public class NewsConsumerBase :UmdfEventConsumerBase
    {
        private Dictionary<string, ChannelUDMF> dicCanais;
        private SinalLastTimestamp timestampControl = new SinalLastTimestamp();

        public NewsConsumerBase(Dictionary<string, ChannelUDMF> dicCanais)
        {
            this.dicCanais = dicCanais;

            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("NewsConsumerBase-", logger.Logger);
        }

        protected override void trataEventoUmdf(EventoUmdf evento)
        {
            try
            {
                String msgType = evento.MsgType;
                OpenFAST.Message umdfMessage = evento.MsgBody;
                String instrumento = "";
                String msgID = evento.MsgSeqNum.ToString();
                String channelId = evento.ChannelID;
                bool isPuma = dicCanais[channelId].channelConfig.IsPuma;
                bool isBMF = (dicCanais[channelId].channelConfig.Segment.ToUpper().Equals(ConstantesMDS.CHANNEL_UMDF_SEGMENT_BMF) ? true : false);
                //bool isPuma20 = dicCanais[channelId].channelConfig.IsPuma20;
                int marketDepth = evento.MarketDepth;
                bool sendToStreamer = true;

                if (logger.IsDebugEnabled)
                {
                    logger.DebugFormat("channelID ....: {0} (isPuma={1})", channelId, isPuma.ToString());
                    logger.DebugFormat("msgID  .......: {0}", msgID);
                    logger.DebugFormat("Message ......: {0}", umdfMessage.ToString());
                }

                if (umdfMessage.IsDefined("Headline"))
                    logger.Info("Head [" + umdfMessage.GetString("Headline") + "]");

                if (umdfMessage.IsDefined("Text"))
                    logger.Info("Text: [" + umdfMessage.GetString("Text") + "]");

                if (umdfMessage.IsDefined("URLLink"))
                    logger.Info("URL: [" + umdfMessage.GetString("URLLink") + "]");


            }
            catch (Exception ex)
            {
                logger.Error("NewsConsumerBase.TrataEventoUmdf(): " + ex.Message, ex);
            }
        }

        protected override void trataMensagemFIX(EventoFIX evento)
        {
        }
    }
}
