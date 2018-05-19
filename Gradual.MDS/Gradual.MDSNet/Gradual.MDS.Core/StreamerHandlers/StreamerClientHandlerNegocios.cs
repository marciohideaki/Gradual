using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Gradual.MDS.Core.Lib;
using Gradual.MDS.Core.Sinal;
using Gradual.MDS.Eventos.Lib;
using Gradual.MDS.Eventos.Lib.EventsArgs;
using log4net;
using Newtonsoft.Json;

namespace Gradual.MDS.Core.StreamerHandlers
{
    public class StreamerClientHandlerNegocios : StreamerClientHandlerBase
    {
        protected ConcurrentQueue<string> queueToStreamer = new ConcurrentQueue<string>();
        private object syncQueueToStreamer = new object();
        private HttpNegocioEventHander eventHandler;

        public StreamerClientHandlerNegocios(int clientNumber, Socket clientSocket)
            : base(clientNumber, clientSocket)
        {
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("StreamerClientHandlerNegocios-", logger.Logger);
        }

        protected override void trataAssinatura(string tipo, string instrumento, string sessionID)
        {
            string mensagem = "";
            NegociosConsumerBase negociosConsumer = ContainerManager.Instance.NegociosConsumer;

            logger.DebugFormat("{0} assinatura de {1} de {2}", sessionID, tipo, instrumento);

            EventoHttpNegocio httpNEG = negociosConsumer.SnapshotStreamerNegocio(instrumento);
            if (httpNEG != null)
            {
                Dictionary<string, string> cabecalho = MDSUtils.montaCabecalhoStreamer(tipo, null, ConstantesMDS.HTTP_OFERTAS_TIPO_ACAO_COMPLETO, instrumento, negociosConsumer.RetornaCasasDecimais(instrumento), sessionID);

                httpNEG.cabecalho = cabecalho;

                mensagem = JsonConvert.SerializeObject(httpNEG);
                mensagem = MDSUtils.montaMensagemHttp(ConstantesMDS.TIPO_REQUISICAO_NEGOCIOS, instrumento, null, mensagem);

                logger.Debug("SNAP Negocio[" + httpNEG.instrumento + "]: " + mensagem);

                bool sinaliza = queueToStreamer.IsEmpty;
                queueToStreamer.Enqueue(mensagem);

                if (sinaliza)
                {
                    lock (syncQueueToStreamer)
                    {
                        Monitor.Pulse(syncQueueToStreamer);
                    }
                }
            }
        }


        protected override void listenEvents()
        {
            eventHandler = new HttpNegocioEventHander(despacharEventos);
            EventQueueManager.Instance.OnEventoHttpNegocios += eventHandler;
        }

        protected override void unlistenEvents()
        {
            if ( eventHandler != null )
                EventQueueManager.Instance.OnEventoHttpNegocios -= eventHandler;
        }

        private void despacharEventos(object sender, HttpNegocioEventArgs args)
        {
            try
            {
                string mensagem = null;
                EventoHttpNegocio httpNEG = args.Evento;


                if (dctSessions.ContainsKey(httpNEG.instrumento))
                {
                    mensagem = JsonConvert.SerializeObject(httpNEG);
                    mensagem = MDSUtils.montaMensagemHttp(ConstantesMDS.TIPO_REQUISICAO_NEGOCIOS, httpNEG.instrumento, null, mensagem);

                    logger.Debug("INCR Negocio[" + httpNEG.instrumento + "]: " + mensagem);

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

        //protected override EPStatement createStatement(string tipo, string instrumento)
        //{
        //    try
        //    {
        //        string consultaEsper = "";

        //        consultaEsper = "select * from EventoHttpNegocio where instrumento='" + instrumento + "'";

        //        consultaEsper += " output all every 1 msec";

        //        EPStatement comandoEsper = NesperManager.Instance.epService.EPAdministrator.CreateEPL(consultaEsper);
        //        comandoEsper.Events += new UpdateEventHandler(eventUpdateHandler);

        //        logger.Debug("Consulta [" + consultaEsper + "] cadastrada no ESPER!");

        //        return comandoEsper;
        //    }
        //    catch (EPException epex)
        //    {
        //        logger.Error("Exception in createEPL - " + epex.Message, epex);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Error in CreateStatement() " + ex.Message, ex);
        //    }

        //    return null;
        //}


        protected override void queueProcessor()
        {
            logger.Info("Iniciando processamento da fila de envio streamer de Negocios");
            long lastLogTicks = 0;
            while (bKeepRunning)
            {
                try
                {
                    string msgneg = null;

                    if (queueToStreamer.TryDequeue(out msgneg))
                    {
                        if (!String.IsNullOrEmpty(msgneg))
                        {
                            SocketPackage.SendData(msgneg, ClientSocket);
                        }

                        if (MDSUtils.shouldLog(lastLogTicks))
                        {
                            lastLogTicks = DateTime.UtcNow.Ticks;
                            logger.Info("Mensagens na fila: " + queueToStreamer.Count);
                        }

                        continue;
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

            logger.Info("Finalizando processamento da fila de envio streamer de Negocios");
        }

    }
}
