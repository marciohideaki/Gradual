using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using com.espertech.esper.client;
using com.espertech.esper.compat.collections;
using Gradual.OMS.AutomacaoDesktop.Events;
using System.Threading;

namespace Gradual.OMS.AutomacaoDesktop.Adapters
{
    public class GeradorEventosBovespa
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EPServiceProvider epService = null;
        private AutomacaoConfig parametros = null;
        string ultimoMsgId = "";
        private bool _bKeepRunning = false;
        private Thread _me = null;
        private BovespaClientSinal bovcli = null;


        public GeradorEventosBovespa(DadosGlobais dadosGlobais)
	    {
		    this.parametros = dadosGlobais.Parametros;
		    this.epService = dadosGlobais.EpService;
            this.ultimoMsgId = dadosGlobais.LastMdgIDBov;
		    return;
	    }

        public void Start()
        {
            _bKeepRunning = true;
            _me = new Thread(new ThreadStart(Run));
            _me.Start();
        }

        public void Run()
        {
            int lastTrial = 600;

            while (_bKeepRunning)
            {
                if (bovcli == null && lastTrial > 600)
                {
                    bovcli = new BovespaClientSinal();
                    bovcli.Debug = false;

                    if (this.ultimoMsgId != null && this.ultimoMsgId.Length > 0)
                        bovcli.LastMsg = this.ultimoMsgId;
                    else
                        bovcli.LastMsg = DateTime.Now.ToString("yyyyMMddA0006000002");

                    bovcli.OnConnect += new BovespaOnConnectEventHandler(OnBovespaConnect);
                    bovcli.OnDataReceived += new BovespaDataReceivedEventHandler(OnBovespaDataReceived);
                    bovcli.OnDisconnect += new BovespaOnDisconnectEventHandler(OnBovespaDisconnect);
                    bovcli.OnError += new BovespaOnErrorEventHandler(OnBovespaError);

                    bovcli.Connect(parametros.ServidorProxyDiff, Convert.ToString(parametros.PortaProxyDiff));

                    lastTrial = 0;
                }
                else
                    lastTrial++;

                Thread.Sleep(100);
            }
        }



        public void Stop()
        {
            _bKeepRunning = false;
            while (_me != null && _me.IsAlive)
            {
                Thread.Sleep(250);
            }
        }

        #region Handlers Eventos Bovespa
        public void OnBovespaConnect()
        {
            logger.Info("OnBovespaConnect()");
        }

        public void OnBovespaError(int error, string msg, string description)
        {
            logger.ErrorFormat("OnBovespaError({0},{1},{2})", error, msg, description);

            if (bovcli.IsConectado)
                bovcli.Disconnect();

            bovcli = null;
        }

        public void OnBovespaDisconnect(string description)
        {
            logger.Info("OnBovespaDisconnect(" + description + ")");

            bovcli = null;
        }

        /// <summary>
        /// Handler do evento de recebimento da mensagem
        /// </summary>
        /// <param name="LastMsgId"></param>
        /// <param name="SPF_Header"></param>
        /// <param name="DataPtr"></param>
        /// <param name="DataSize"></param>
	    public void OnBovespaDataReceived(string LastMsgId, string SPF_Header, string DataPtr, int DataSize)
	    {
		    string cabecalho;
		    string corpo;
		    string tipo;
		    string instrumento;
		    EventoBovespa msgEvent;
		    long antesSendEvent;
		    long depoisSendEvent;
		    int tamanhoCabecalho = EventoBovespa.BOV_CABECALHO_HORA_EVENTO_FIM;

            try
            {
                if (DataPtr.Length < tamanhoCabecalho)
                {
                    logger.Error("Tamanho da mensagem invalida [" + DataPtr + "]");
                }
                cabecalho = DataPtr.Substring(
                        EventoBovespa.BOV_CABECALHO_RESERVADO1_INI,
                        EventoBovespa.BOV_CABECALHO_HORA_EVENTO_FIM - EventoBovespa.BOV_CABECALHO_RESERVADO1_INI);
                corpo = DataPtr.Substring(tamanhoCabecalho);
                tipo = cabecalho.Substring(
                        EventoBovespa.BOV_CABECALHO_TIPO_MENSAGEM_INI,
                        EventoBovespa.BOV_CABECALHO_TIPO_MENSAGEM_FIM - EventoBovespa.BOV_CABECALHO_TIPO_MENSAGEM_INI);
                instrumento = cabecalho.Substring(
                        EventoBovespa.BOV_CABECALHO_CODIGO_PAPEL_INI,
                        EventoBovespa.BOV_CABECALHO_CODIGO_PAPEL_FIM - EventoBovespa.BOV_CABECALHO_CODIGO_PAPEL_INI).Trim();

                msgEvent = new EventoBovespa(LastMsgId, tipo, cabecalho, corpo, instrumento);

                antesSendEvent = DateTime.Now.Ticks;
                epService.EPRuntime.SendEvent(msgEvent);
                depoisSendEvent = DateTime.Now.Ticks;

                TimeSpan duracaoSendEvent = new TimeSpan(depoisSendEvent - antesSendEvent);
                logger.Debug(cabecalho + "-sendEvent em " + duracaoSendEvent.TotalMilliseconds + " ms");
            }
            catch (Exception ex)
            {
                logger.Error("Erro em OnDataReceived():" + ex.Message);
                logger.Error(ex);
            }
        }

        #endregion //Handlers Eventos Bovespa
    }
}
