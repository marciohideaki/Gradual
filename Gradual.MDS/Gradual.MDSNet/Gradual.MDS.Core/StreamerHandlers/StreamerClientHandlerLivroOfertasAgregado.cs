using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using Gradual.MDS.Core.Lib;
using Gradual.MDS.Core.Sinal;
using Gradual.MDS.Eventos.Lib;
using Gradual.MDS.Eventos.Lib.EventsArgs;
using log4net;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gradual.MDS.Core.StreamerHandlers
{
    public class StreamerClientHandlerLivroOfertasAgregado : StreamerClientHandlerBase
    {
        protected ConcurrentQueue<string> queueToStreamer = new ConcurrentQueue<string>();
		private ConcurrentQueue<EventoHttpLivroOfertasAgregado> queueEventosLOA = new ConcurrentQueue<EventoHttpLivroOfertasAgregado>();
        private HttpLivroOfertasAgregadoEventHandler eventHandlerLOA;
        private Object objLockSnapshot = new Object();
        private Object syncQueueEventosLOA = new Object();
        private Object syncQueueToStreamer = new Object();
		private Thread thOtimizadorEventos = null;

        public StreamerClientHandlerLivroOfertasAgregado(int clientNumber, Socket clientSocket)
            : base(clientNumber, clientSocket)
        {
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("StreamerClientHandlerLivroOfertasAgregado-", logger.Logger);
        }

        protected override void trataAssinatura(string tipo, string instrumento, string sessionID)
        {
            string mensagem = "";
            bool bsinaliza = false;
            LivroOfertasConsumerBase lofConsumer = ContainerManager.Instance.LivroOfertasConsumer;

            logger.DebugFormat("{0} assinatura de {1} de {2}", sessionID, tipo, instrumento);

            // Aqui tem o pulo do gato
            // interrompe o processamento dos eventos ate a chegada do snapshot
            // para nao quebrar a sequencia do sinal do livro
            lock (objLockSnapshot)
            {
                EventoHttpLivroOfertasAgregado httpLOA;
                httpLOA = lofConsumer.SnapshotStreamerLivroAgregado(instrumento, sessionID);

                logger.Debug("Snapshot LOA de " + instrumento + ": " + httpLOA.livroCompra.Count + "C/" + httpLOA.livroVenda.Count + "V items");

                mensagem = JsonConvert.SerializeObject(httpLOA);
                mensagem = MDSUtils.montaMensagemHttp(ConstantesMDS.TIPO_REQUISICAO_LIVRO_OFERTAS_AGREGADO, instrumento, null, mensagem);

                bsinaliza = queueToStreamer.IsEmpty;
                queueToStreamer.Enqueue(mensagem);
            }

            if (bsinaliza)
            {
                lock (syncQueueToStreamer)
                {
                    Monitor.Pulse(syncQueueToStreamer);
                }
            }
        }




        protected override void listenEvents()
        {
            eventHandlerLOA = new HttpLivroOfertasAgregadoEventHandler(despacharEventosLOA);
            EventQueueManager.Instance.OnEventoHttpLivroOfertasAgregado += eventHandlerLOA;
        }

        protected override void unlistenEvents()
        {
            if (eventHandlerLOA != null)
                EventQueueManager.Instance.OnEventoHttpLivroOfertasAgregado -= eventHandlerLOA;
        }

        private void despacharEventosLOA(object sender, HttpLivroOfertasAgregadoEventArgs args)
        {
            try
            {
                EventoHttpLivroOfertasAgregado httpLOA = args.Evento;

                if (dctSessions.ContainsKey(httpLOA.instrumento))
                {
                    bool sinaliza = queueEventosLOA.IsEmpty;
                    queueEventosLOA.Enqueue(httpLOA);
                    if (sinaliza)
                    {
                        lock (syncQueueEventosLOA)
                        {
                            Monitor.Pulse(syncQueueEventosLOA);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("despacharEventosLOA(): " + ex.Message, ex);
            }
        }



        private void otimizadorEventos()
        {
            logger.Info("Iniciando otimizacao de eventos");
            long lastLogTicks = 0;
            long lastOtimizationTicks = 0;
            string lastInstrumento = null;

            EventoHttpLivroOfertasAgregado evtCache = null;
            while (bKeepRunning)
            {
                try
                {
                    EventoHttpLivroOfertasAgregado evento;
                    if (queueEventosLOA.TryDequeue(out evento))
                    {
                        // Se houver cache, descarrega
                        if ( !String.IsNullOrEmpty(lastInstrumento) )
                        {
                            if ( !lastInstrumento.Equals(evento.instrumento))
                            {
                                flushEventCache(evtCache);
                                lastInstrumento = null;
                                evtCache = null;
                            }
                        }

                        // Se cache vazio, cria um novo cache
                        if (String.IsNullOrEmpty(lastInstrumento))
                        {
                            lastInstrumento = evento.instrumento;
                            evtCache = new EventoHttpLivroOfertasAgregado();
                            evtCache.cabecalho = evento.cabecalho;
                            evtCache.instrumento = evento.instrumento;
                            evtCache.livroCompra = new List<Dictionary<string, string>>();
                            evtCache.livroVenda = new List<Dictionary<string, string>>();
                        }

                        // Se for igual, acrescenta as operacoes de livro no final
                        if (lastInstrumento.Equals(evento.instrumento))
                        {
                            if (evento.livroCompra != null && evento.livroCompra.Count > 0)
                                evtCache.livroCompra.AddRange(evento.livroCompra);

                            if (evento.livroVenda != null && evento.livroVenda.Count > 0)
                                evtCache.livroVenda.AddRange(evento.livroVenda);
                        }

                        if (MDSUtils.shouldLog(lastLogTicks))
                        {
                            lastLogTicks = DateTime.UtcNow.Ticks;
                            logger.Info("Fila de eventos a serem otimizados: " + queueEventosLOA.Count);
                        }

                        continue;
                    }

                    // Se a fila estiver vazia e estourar o timeout, descarrega o cache
                    //if (!String.IsNullOrEmpty(lastInstrumento) && (DateTime.UtcNow.Ticks - lastOtimizationTicks) > TimeSpan.TicksPerMillisecond)
                    if (!String.IsNullOrEmpty(lastInstrumento) )
                    {
                        flushEventCache(evtCache);
                        lastInstrumento = null;
                        evtCache = null;

                        continue;
                    }

                    lock (syncQueueEventosLOA)
                    {
                        Monitor.Wait(syncQueueEventosLOA, 50);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("otimizadorEventos(): " + ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtCache"></param>
        private void flushEventCache(EventoHttpLivroOfertasAgregado evtCache)
        {
            string mensagem = null;
            try
            {
                mensagem = JsonConvert.SerializeObject(evtCache);
                mensagem = MDSUtils.montaMensagemHttp(ConstantesMDS.TIPO_REQUISICAO_LIVRO_OFERTAS_AGREGADO, evtCache.instrumento, null, mensagem);

                bool bsinaliza = false;
                lock (objLockSnapshot)
                {
                    if (!String.IsNullOrEmpty(mensagem))
                    {
                        bsinaliza = queueToStreamer.IsEmpty;
                        queueToStreamer.Enqueue(mensagem);
                    }
                }

                if (bsinaliza)
                {
                    lock (syncQueueToStreamer)
                    {
                        Monitor.Pulse(syncQueueToStreamer);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("flushEventCache(): " + ex.Message, ex);
            }
        }


        protected  override void queueProcessor()
        {
            logger.Info("Iniciando thread para otimizar eventos");

            thOtimizadorEventos = new Thread(new ThreadStart(otimizadorEventos));
            thOtimizadorEventos.Name = "thOtimizadorEventos-";
            thOtimizadorEventos.Start();

            logger.Info("Iniciando processamento da fila de envio streamer de LOA");
            long lastLogTicks = 0;
            while (bKeepRunning)
            {
                try
                {
                    string msglof = null;
                    lock (objLockSnapshot)
                    {
                        if ( queueToStreamer.TryDequeue(out msglof) )
                        {
                            if (!String.IsNullOrEmpty(msglof))
                            {
                                SocketPackage.SendData(msglof, ClientSocket);
                            }

                            if (MDSUtils.shouldLog(lastLogTicks))
                            {
                                lastLogTicks = DateTime.UtcNow.Ticks;
                                logger.Info("Mensagens na fila: " + queueToStreamer.Count);
                            }
                            continue;
                        }
                    }

                    lock (syncQueueToStreamer)
                    {
                        Monitor.Wait(syncQueueToStreamer, 50);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("queueProcessor(): " + ex.Message, ex);
                }
            }

            logger.Info("Finalizando processamento da fila de envio streamer de LOA");
        }

    }
}
