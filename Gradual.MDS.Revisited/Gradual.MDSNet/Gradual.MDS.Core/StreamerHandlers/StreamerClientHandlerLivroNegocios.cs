using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Threading;
using Gradual.MDS.Core.Lib;
using Newtonsoft.Json;
using Gradual.MDS.Eventos.Lib;
using System.Net.Sockets;
using Gradual.MDS.Core.Sinal;
using Gradual.MDS.Eventos.Lib.EventsArgs;
using System.Collections.Concurrent;

namespace Gradual.MDS.Core.StreamerHandlers
{
    public class StreamerClientHandlerLivroNegocios : StreamerClientHandlerBase
    {
        protected ConcurrentQueue<string> queueToStreamer = new ConcurrentQueue<string>();
        private HttpLivroNegociosEventHandler eventHandler = null;
        private Object objLockSnapshot = new Object();

        public StreamerClientHandlerLivroNegocios(int clientNumber, Socket clientSocket)
            : base(clientNumber, clientSocket)
        {
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("StreamerClientHandlerLivroNegocios-", logger.Logger);
        }

        protected override void trataAssinatura(string tipo, string instrumento, string sessionID)
        {
            string mensagem = "";
            NegociosConsumerBase negociosConsumer = ContainerManager.Instance.NegociosConsumer;
            Dictionary<string, string> cabecalho = MDSUtils.montaCabecalhoStreamer(tipo, null, ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_COMPLETO, instrumento, negociosConsumer.RetornaCasasDecimais(instrumento), sessionID);

            logger.DebugFormat("{0} assinatura de {1} de {2}", sessionID, tipo, instrumento);

            // Aqui tem o pulo do gato
            // interrompe o processamento dos eventos ate a chegada do snapshot
            // para nao quebrar a sequencia do sinal do livro
            lock (objLockSnapshot)
            {
                EventoHttpLivroNegocios httpLNG;
                httpLNG = negociosConsumer.SnapshotStreamerLivroNegocios(instrumento);
                httpLNG.cabecalho = cabecalho;

                logger.Debug("Snapshot LNG de " + instrumento + ": " + httpLNG.negocio.Count + " items");

                mensagem = JsonConvert.SerializeObject(httpLNG);
                mensagem = MDSUtils.montaMensagemHttp(ConstantesMDS.TIPO_REQUISICAO_LIVRO_NEGOCIOS, instrumento, null, mensagem);

                queueToStreamer.Enqueue(mensagem);
            }
        }


        protected override void listenEvents()
        {
            eventHandler = new HttpLivroNegociosEventHandler(despacharEventos);
            EventQueueManager.Instance.OnEventoHttpLivroNegocios += eventHandler;
        }

        protected override void unlistenEvents()
        {
            if (eventHandler != null)
                EventQueueManager.Instance.OnEventoHttpLivroNegocios -= eventHandler;
        }

        private void despacharEventos(object sender, HttpLivroNegociosEventArgs args )
        {
            try
            {
                string mensagem = null;

                EventoHttpLivroNegocios httpLNG = args.Evento;

                if (dctSessions.ContainsKey(httpLNG.instrumento))
                {
                    mensagem = JsonConvert.SerializeObject(httpLNG);
                    mensagem = MDSUtils.montaMensagemHttp(ConstantesMDS.TIPO_REQUISICAO_LIVRO_NEGOCIOS, httpLNG.instrumento, null, mensagem);

                    if (!String.IsNullOrEmpty(mensagem))
                    {
                        queueToStreamer.Enqueue(mensagem);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("despacharEventos(): " + ex.Message, ex);
            }

        }

        protected override void queueProcessor()
        {
            logger.Info("Iniciando processamento da fila de envio streamer de Livro de Negocios");
            long lastLogTicks = 0;

            while (bKeepRunning)
            {
                try
                {
                    string msgLNG = null;
                    lock (objLockSnapshot)
                    {
                        if (queueToStreamer.TryDequeue(out msgLNG))
                        {
                            if (!String.IsNullOrEmpty(msgLNG))
                            {
                                SocketPackage.SendData(msgLNG, ClientSocket);
                            }

                            if (MDSUtils.shouldLog(lastLogTicks))
                            {
                                lastLogTicks = DateTime.UtcNow.Ticks;
                                logger.Info("Mensagens na fila: " + queueToStreamer.Count);
                            }

                            continue;
                        }
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Error("queueProcessor(): " + ex.Message, ex);
                }

            }

            logger.Info("Finalizando processamento da fila de envio streamer de Livro de Negocios");
        }
    }
}
