using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using log4net;
using System.Net.Sockets;
using Gradual.MDS.Core.Lib;
using Gradual.MDS.Eventos.Lib;
using Gradual.MDS.Eventos.Lib.EventsArgs;
using System.Collections.Concurrent;

namespace Gradual.MDS.Core.AnaliseGrafica
{
    public class AnaliseGraficaClientHandler
    {
        protected ILog logger;
        protected bool bKeepRunning = true;
        protected Thread myThread = null;
        protected string myThreadName;
        protected ConcurrentQueue<string> queueToAnaliseGrafica = new ConcurrentQueue<string>();
        protected Socket ClientSocket;
        private AnaliseGraficaNegocioEventHandler eventHandler;
        private object syncQueueToAnaliseGrafica = new object();

        public AnaliseGraficaClientHandler(int clientNumber, Socket clientSocket)
        {
            this.ClientSocket = clientSocket;
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("AnaliseGraficaClientHandler", logger.Logger);
        }

        public void Start()
        {
            myThread = new Thread(new ThreadStart(queueProcessor));
            myThread.Name = myThreadName;
            myThread.Start();

            listenEvents();
        }

        public void Stop()
        {
            unlistenEvents();

            bKeepRunning = false;
            while (myThread != null && myThread.IsAlive)
            {
                Thread.Sleep(250);
            }

            logger.Info(myThreadName + " Finalizada");
        }

        protected void queueProcessor()
        {
            logger.Info("Iniciando processamento da fila de envio Analise Grafica");
            long lastLogTicks = 0;
            while (bKeepRunning)
            {
                try
                {
                    string msgNEG = null;
                    if (queueToAnaliseGrafica.TryDequeue(out msgNEG))
                    {
                        SocketPackage.SendData(msgNEG, ClientSocket);

                        if (MDSUtils.shouldLog(lastLogTicks))
                        {
                            lastLogTicks = DateTime.UtcNow.Ticks;
                            logger.Info("Fila de eventos : " + queueToAnaliseGrafica.Count);
                        }
                        logger.DebugFormat("Mensagem [{0}]", msgNEG);

                        continue;
                    }

                    lock (syncQueueToAnaliseGrafica)
                    {
                        Monitor.Wait(syncQueueToAnaliseGrafica, 50);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("queueProcessor(): " + ex.Message, ex);
                }

            }

            logger.Info("Finalizando processamento da fila de envio home broker de Negocios");
        }


        //protected EPStatement createStatement()
        //{
        //    try
        //    {
        //        string consultaEsper = "select * from EventoNegocioANG";

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


        //protected void eventUpdateHandler(object sender, com.espertech.esper.client.UpdateEventArgs args)
        //{
        //    try
        //    {
        //        if (args.OldEvents != null && args.OldEvents.Length > 0)
        //            despacharEventos(args.OldEvents);

        //        if (args.NewEvents != null && args.NewEvents.Length > 0)
        //            despacharEventos(args.NewEvents);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("eventUpdateHandler(): " + ex.Message, ex);
        //    }
        //}

        private void listenEvents()
        {
            eventHandler = new AnaliseGraficaNegocioEventHandler(despacharEventos);
            EventQueueManager.Instance.OnEventoNegocioAnaliseGrafica += eventHandler;
        }

        private void unlistenEvents()
        {
            if (eventHandler != null)
                EventQueueManager.Instance.OnEventoNegocioAnaliseGrafica -= eventHandler;
        }

        private void despacharEventos(object sender, AnaliseGraficaNegocioEventArgs args)
        {
            try
            {
                string mensagem = null;

                EventoNegocioANG eventoANG = args.Evento;

                mensagem = eventoANG.mensagem;

                if (!String.IsNullOrEmpty(mensagem))
                {
                    bool sinaliza = queueToAnaliseGrafica.IsEmpty;
                    queueToAnaliseGrafica.Enqueue(mensagem);
                    if (sinaliza)
                    {
                        lock (syncQueueToAnaliseGrafica)
                        {
                            Monitor.Pulse(syncQueueToAnaliseGrafica);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("despacharEventos(): " + ex.Message, ex);
            }
        }

        public void TratarConexaoANG( string serverName)
        {
            logger.Debug("Tratando conexao de  [" + serverName + "]");

            //ThreadPool.QueueUserWorkItem(
            //    new WaitCallback(
            //        delegate(object required)
            //        {
            //            try
            //            {
            //                listenEvents();
            //            }
            //            catch (Exception ex)
            //            {
            //                logger.Error("TratarConexaoANG(): " + ex.Message, ex);
            //            }
            //        }
            //    )
            //);

        }
    }
}
