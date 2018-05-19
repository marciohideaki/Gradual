using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenFAST;
using log4net;
using Gradual.MDS.Eventos.Lib;
using log4net.Core;
using System.Configuration;
using Gradual.MDS.Core.Lib;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Layout;

namespace Gradual.MDS.Core
{
    public class MDSUtils
    {
        protected static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static void EnqueueEventoUmdf(Message message,
            String templateId,
            String channelID,
            String segment,
            int marketDepth,
            StreamTypeEnum streamType)
        {
            string msgtype;

            if (message.IsDefined("MsgType"))
                msgtype = message.GetString("MsgType");
            else
                msgtype = message.GetString("MessageType");

            EnqueueEventoUmdf(message, templateId, channelID, segment, msgtype, marketDepth, streamType);
        }


        public static void EnqueueEventoUmdf(Message message,
            String templateId,
            String channelID,
            String segment,
            String msgType,
            int marketDepth,
            StreamTypeEnum streamType)
        {
            try
            {
                EventoUmdf evento = new EventoUmdf();
                evento.MsgSeqNum = message.GetInt("MsgSeqNum");
                evento.MsgType = msgType;
                evento.TemplateID = templateId;
                evento.MsgBody = message;
                evento.ChannelID = channelID;
                evento.UmdfSegment = segment;
                evento.MarketDepth = marketDepth;
                evento.StreamType = streamType;

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Enfileirando evento umdf " +
                        "MsgSeqNum=[" + evento.MsgSeqNum +
                        "] MsgType=[" + evento.MsgType +
                        "] TemplateId=[" + evento.TemplateID +
                        "] Segment=[" + evento.UmdfSegment +
                        "] Channel=[" + evento.ChannelID +
                        "] MarketDepth=[" + evento.MarketDepth.ToString() +
                        "] message=[" + evento.MsgBody.ToString() + "]");
                }

                ContainerManager.Instance.EnqueueEventoUmdf(evento);
            }
            catch (Exception ex)
            {
                logger.Error("Exception em EnqueueEventoUmdf - MsgSeqNum=[" + message.GetInt("MsgSeqNum") + "]");
                logger.Error(ex.Message, ex);
            }

            return;
        }

        public static void EnqueueEventoUmdfNews(Message message,
            String templateId,
            String channelID,
            String segment,
            String msgType,
            StreamTypeEnum streamType)
        {
            try
            {
                EventoUmdf evento = new EventoUmdf();
                evento.MsgSeqNum = message.GetInt("MsgSeqNum");
                evento.MsgType = msgType;
                evento.TemplateID = templateId;
                evento.MsgBody = message;
                evento.ChannelID = channelID;
                evento.UmdfSegment = segment;
                evento.StreamType = streamType;

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Enfileirando evento umdf noticias" +
                        "MsgSeqNum=[" + evento.MsgSeqNum +
                        "] MsgType=[" + evento.MsgType +
                        "] TemplateId=[" + evento.TemplateID +
                        "] Segment=[" + evento.UmdfSegment +
                        "] Channel=[" + evento.ChannelID +
                        "] Stream=[" + evento.StreamType.ToString() +
                        "] message=[" + evento.MsgBody.ToString() + "]");
                }

                ContainerManager.Instance.EnqueueNews(evento);
            }
            catch (Exception ex)
            {
                logger.Error("EnqueueEventoUmdfNews - MsgSeqNum=[" + message.GetInt("MsgSeqNum") + "]");
                logger.Error(ex.Message, ex);
            }

            return;
        }

        public static void GenerateEsperEvent(Message message,
            String templateId,
            String channelID,
            String segment,
            String msgType,
            int marketDepth,
            StreamTypeEnum streamType)
        {
            try
            {
                EventoUmdf evento = new EventoUmdf();
                evento.MsgSeqNum = message.GetInt("MsgSeqNum");
                evento.MsgType = msgType;
                evento.TemplateID = templateId;
                evento.MsgBody = message;
                evento.ChannelID = channelID;
                evento.UmdfSegment = segment;
                evento.MarketDepth = marketDepth;
                evento.StreamType = streamType;

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Gerando evento Esper " +
                        "MsgSeqNum=[" + evento.MsgSeqNum +
                        "] MsgType=[" + evento.MsgType +
                        "] TemplateId=[" + evento.TemplateID +
                        "] Segment=[" + evento.UmdfSegment +
                        "] Channel=[" + evento.ChannelID +
                        "] Stream=[" + evento.StreamType.ToString() +
                        "] MarketDepth=[" + evento.MarketDepth.ToString() +
                        "] message=[" + evento.MsgBody.ToString() + "]");
                }

                ContainerManager.Instance.EnqueueEventoUmdf(evento);
            }
            catch (Exception ex)
            {
                logger.Error("Exception em generateEsperEvent - MsgSeqNum=[" + message.GetInt("MsgSeqNum") + "]");
                logger.Error(ex.Message, ex);
            }

            return;
        }


        //public static void AddAppender(string appenderName, ILogger wLogger)
        //{
        //    log4net.Repository.Hierarchy.Logger l = (log4net.Repository.Hierarchy.Logger)wLogger;

        //    log4net.Appender.IAppender hasAppender = l.GetAppender(appenderName);
        //    if (hasAppender != null)
        //    {
        //        hasAppender..ActivateOptions();

        //        l.AddAppender(hasAppender);
        //    }
        //}

        public static void AddAppender(string appenderName, ILogger wLogger)
        {
            string filename = ConfigurationManager.AppSettings["LogDir"].ToString() + "\\" + appenderName + ".log";

            log4net.Repository.Hierarchy.Logger l = (log4net.Repository.Hierarchy.Logger)wLogger;

            log4net.Appender.IAppender hasAppender = l.GetAppender(appenderName);
            if (hasAppender == null)
            {
                log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();

                appender.DatePattern = "yyyyMMdd";
                appender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Date;
                appender.AppendToFile = true;
                appender.File = filename;
                appender.StaticLogFileName = true;
                appender.Name = appenderName;

                log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout();
                layout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
                layout.ActivateOptions();

                appender.Layout = layout;
                appender.ActivateOptions();


                l.AddAppender(appender);
            }
        }


        //public static ILog CreateAppender(string appenderName)
        //{
        //    string filename = ConfigurationManager.AppSettings["LogDir"].ToString() + "\\" + appenderName + ".log";

        //    Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
        //    RollingFileAppender roller = new RollingFileAppender();
        //    roller.LockingModel = new log4net.Appender.FileAppender.MinimalLock();
        //    roller.AppendToFile = true;
        //    roller.RollingStyle = RollingFileAppender.RollingMode.Composite;
        //    roller.MaxSizeRollBackups = 14;
        //    roller.MaximumFileSize = "15000KB";
        //    roller.DatePattern = "yyyyMMdd";
        //    roller.Layout = new log4net.Layout.PatternLayout();
        //    roller.File = filename;
        //    roller.StaticLogFileName = true;

        //    PatternLayout patternLayout = new PatternLayout();
        //    patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
        //    patternLayout.ActivateOptions();

        //    roller.Layout = patternLayout;
        //    roller.ActivateOptions();
        //    hierarchy.Root.AddAppender(roller);

        //    hierarchy.Root.Level = Level.All;
        //    hierarchy.Configured = true;

        //    //log4net.Core.

        //    //DummyLogger dummyILogger = new DummyLogger(appenderName);
        //    //dummyILogger.Hierarchy = hierarchy;
        //    //dummyILogger.Level = log4net.Core.Level.All;
        //    //dummyILogger.AddAppender(roller);

        //    //return new LogImpl(dummyILogger);
        //    //return new LogImpl(dummyILogger);
        //}

        //public static Dictionary<string, string> montaCabecalhoStreamer(string tipoMensagem, string tipoBolsa, int acao, string instrumento, string sessionID)
        //{
        //    return montaCabecalhoStreamer(tipoMensagem, tipoBolsa, acao, instrumento, 2, sessionID);
        //}

        public static Dictionary<string, string> montaCabecalhoStreamer(string tipoMensagem, string tipoBolsa, int acao, string instrumento, int numCasasDecimais, string sessionID)
        {
            Dictionary<String, String> cabecalho =  new Dictionary<String, String>();

            if ( !String.IsNullOrEmpty(tipoMensagem) )
                cabecalho.Add( ConstantesMDS.HTTP_CABECALHO_TIPO_MENSAGEM, tipoMensagem);

            cabecalho.Add( ConstantesMDS.HTTP_CABECALHO_ACAO, acao.ToString());

            cabecalho.Add(ConstantesMDS.HTTP_CABECALHO_CASAS_DECIMAIS, numCasasDecimais.ToString());

            if ( !String.IsNullOrEmpty(instrumento))
                cabecalho.Add( ConstantesMDS.HTTP_CABECALHO_INSTRUMENTO, instrumento);

            if (!String.IsNullOrEmpty(sessionID))
                cabecalho.Add( ConstantesMDS.HTTP_CABECALHO_SESSIONID, sessionID);

            if (!String.IsNullOrEmpty(tipoBolsa))
                cabecalho.Add( ConstantesMDS.HTTP_CABECALHO_TIPO_BOLSA, tipoBolsa);

            cabecalho.Add(ConstantesMDS.HTTP_CABECALHO_DATA, DateUtil.Now.ToString("yyyyMMdd"));
            cabecalho.Add(ConstantesMDS.HTTP_CABECALHO_HORA, DateUtil.Now.ToString("HHmmssfff"));

            return cabecalho;
        }

        public static string montaMensagemHttp(string tipoMensagem, string instrumento, string filtro, string mensagem)
        {
            StringBuilder builder = new StringBuilder();

            //builder.Append(tipoMensagem.Substring(0, 2));
            builder.Append(tipoMensagem);
            builder.Append(mensagem);
            return builder.ToString();
        }

        public static string montaHeaderHomeBroker(string instrumento, string tipo, string tipoBolsa)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(instrumento.PadRight(8).Substring(0, 8));

            // Cabecalho da mensagem MDS de Negocio
            if (String.IsNullOrEmpty(tipo))
                builder.Append("XX");
            else
                builder.Append(tipo);

            if ( String.IsNullOrEmpty(tipoBolsa))
                builder.Append("BB");
            else
                builder.Append(tipoBolsa);

            builder.Append(DateUtil.Now.ToString("yyyyMMddHHmmssfff"));

            builder.Append(instrumento.PadRight(20));

            return builder.ToString();
        }

        public static Decimal calcularVariacao(Decimal preco, Decimal precoFechamento)
        {
            Decimal variacao = Decimal.Zero;
            if (precoFechamento == Decimal.Zero)
                precoFechamento = preco;

            variacao = preco / precoFechamento;
            variacao -= Decimal.One;
            variacao *= 100;

            return variacao;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastEventTicks"></param>
        /// <returns></returns>
        public static bool shouldLog(long lastEventTicks, long intervalInSec=1)
        {
            if ((DateTime.UtcNow.Ticks - lastEventTicks) > TimeSpan.TicksPerSecond * intervalInSec)
                return true;

            return false;
        }

        public static void EnviarEmail(string subject, string msg)
        {
            string titulo = System.Environment.MachineName + " - " + subject;

            Gradual.OMS.Library.Utilities.EnviarEmail(titulo, msg);
        }
    }
}
