using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using Gradual.MDS.Eventos.Lib.EventsArgs;
using log4net;
using System.Net.Sockets;
using Gradual.MDS.Eventos.Lib;
using Gradual.MDS.Core.Lib;

namespace Gradual.MDS.Core.HomeBrokerHandlers
{
    public class HomeBrokerHandlerLivroOfertaAgregado : HomeBrokerHandlerBase
    {
        protected bool bKeepRunning = true;
        protected Thread myThread = null;
        protected string myThreadName;
        protected ConcurrentQueue<string> queueToHomeBroker = new ConcurrentQueue<string>();
        protected object syncObj = new object();
        private HBLivroOfertasAgregadoEventHandler eventHandlerLOA;

        public HomeBrokerHandlerLivroOfertaAgregado(int clientNumber, Socket clientSocket)
            : base(clientNumber, clientSocket)
        {
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("HomeBrokerHandlerLivroOfertaAgregado-", logger.Logger);
        }

        public void Start()
        {
            myThread = new Thread(new ThreadStart(queueProcessor));
            myThread.Name = myThreadName;
            myThread.Start();
        }

        public void Stop()
        {
            EventQueueManager.Instance.OnEventoHBLivroOfertasAgregado -= eventHandlerLOA;

            bKeepRunning = false;

            while (myThread != null && myThread.IsAlive)
            {
                Thread.Sleep(250);
            }

            logger.Info(myThreadName + " Finalizada");
        }

        protected void queueProcessor()
        {
            logger.Info("Iniciando processamento da fila de envio home broker de livro de ofertas agregado");
            long lastLogTicks = 0;
            while (bKeepRunning)
            {
                try
                {
                    string msgLOA = null;
                    if (queueToHomeBroker.TryDequeue(out msgLOA))
                    {
                        SocketPackage.SendData(msgLOA, ClientSocket);

                        if (MDSUtils.shouldLog(lastLogTicks))
                        {
                            lastLogTicks = DateTime.UtcNow.Ticks;
                            logger.Info("Fila de eventos : " + queueToHomeBroker.Count);
                        }

                        continue;
                    }

                    lock (syncObj)
                    {
                        Monitor.Wait(syncObj, 50);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("queueProcessor(): " + ex.Message, ex);
                }
            }

            logger.Info("Finalizando processamento da fila de envio home broker de livro de ofertas");
        }


        protected override void listenEvents()
        {
            eventHandlerLOA = new HBLivroOfertasAgregadoEventHandler(despacharEventosLOA);

            EventQueueManager.Instance.OnEventoHBLivroOfertasAgregado += eventHandlerLOA;
        }

        private void despacharEventosLOA(object sender, HBLivroOfertasAgregadoEventArgs args)
        {
            try
            {
                string mensagem = null;

                EventoHBLivroOfertasAgregado eventoHB = args.Evento;
                mensagem = eventoHB.mensagem;

                if (!String.IsNullOrEmpty(mensagem))
                {
                    bool sinaliza = queueToHomeBroker.IsEmpty;
                    queueToHomeBroker.Enqueue(mensagem);
                    if (sinaliza)
                    {
                        lock (syncObj)
                        {
                            Monitor.Pulse(syncObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("despacharEventos(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void enviarSnapshotHB()
        {
            try
            {
                logger.Info("Gerando Snapshot de Livro de Ofertas Agregados");
                
                List<string> snapshotAgregado = ContainerManager.Instance.LivroOfertasConsumer.SnapshotHomeBrokerLivroAgregado();

                logger.Info("Snapshot de LOA com " + snapshotAgregado.Count + " itens");

                foreach (string mensagem in snapshotAgregado)
                {
                    bool sinaliza = queueToHomeBroker.IsEmpty;
                    queueToHomeBroker.Enqueue(mensagem);
                    if (sinaliza)
                    {
                        lock (syncObj)
                        {
                            Monitor.Pulse(syncObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("enviarSnapshotHB(): " + ex.Message, ex);
            }
        }
    }
}
