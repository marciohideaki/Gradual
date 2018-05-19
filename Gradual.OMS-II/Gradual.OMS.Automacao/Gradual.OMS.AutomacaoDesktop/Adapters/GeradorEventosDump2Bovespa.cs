using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.AutomacaoDesktop.Events;
using System.Threading;
using com.espertech.esper.client;
using log4net;
using System.IO;

namespace Gradual.OMS.AutomacaoDesktop.Adapters
{
    class GeradorEventosDump2Bovespa
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EPServiceProvider epService = null;
        private AutomacaoConfig parametros = null;
        string ultimoMsgId = "";
        private bool _bKeepRunning = false;
        private Thread _me = null;
        private BovespaClientSinal bovcli = null;
        DadosGlobais _dadosGlobais = null;


        public GeradorEventosDump2Bovespa(DadosGlobais dadosGlobais)
	    {
		    this.parametros = dadosGlobais.Parametros;
		    this.epService = dadosGlobais.EpService;
		    this.ultimoMsgId = dadosGlobais.LastMdgIDBov;
            this._dadosGlobais = dadosGlobais;
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
            int lastTrial = 0;
            long reglidos = 0;

            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            FileStream fs = File.Open(parametros.DiretorioDump + "\\" + parametros.ArquivoBinarioSimuladorBovespa,
                FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(fs, Encoding.UTF8);

            while (_bKeepRunning)
            {
                try
                {
                    byte[] size = new byte[4];


                    reader.Read(size, 0, 4);

                    int BufferSize = 0;

                    BufferSize += (((int)size[0]) & 0xFF) << 24;

                    BufferSize += (((int)size[1]) & 0xFF) << 16;

                    BufferSize += (((int)size[2]) & 0xFF) << 8;

                    BufferSize += (((int)size[3]) & 0xFF);

                    //int buflen = BitConverter.ToInt32(size, 0);
                    int buflen = BufferSize;

                    byte[] buf = new byte[buflen];

                    if (reader.Read(buf, 0, buflen) == 0)
                    {
                        logger.Info("End Of Dump File");
                        break;
                    }

                    string sinal = System.Text.ASCIIEncoding.ASCII.GetString(buf, 0, buflen);

                    int virgula = sinal.IndexOf(",");

                    if (virgula == -1)
                    {
                        logger.Error("Buffer invalido [" + buf + "]");
                        continue;
                    }


                    string msgId = sinal.Substring(0, virgula);
                    string dados = sinal.Substring(virgula + 1);

                    if ( msgId.CompareTo(_dadosGlobais.LastMdgIDBov) > 0 )
                        OnBovespaDataReceived(msgId, "", dados, dados.Length);

                    reglidos++;
                    if ( lastTrial==30)
                    {
                        Thread.Sleep(5);
                        lastTrial=0;
                        //logger.InfoFormat("Lidos {0} registros", reglidos);
                    }

                    lastTrial++;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }

            reader.Close();
            fs.Close();
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
