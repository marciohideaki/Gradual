using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using log4net;
using Gradual.MDS.Core.Lib;
using Gradual.MDS.Eventos.Lib;
using Gradual.MDS.Eventos.Lib.EventsArgs;
using System.Collections.Concurrent;

namespace Gradual.MDS.Core.HomeBrokerHandlers
{
    public class HomeBrokerHandlerNegocios: HomeBrokerHandlerBase
    {
        protected bool bKeepRunning = true;
        protected Thread myThread = null;
        protected string myThreadName;
        protected ConcurrentQueue<string> queueToHomeBroker = new ConcurrentQueue<string>();
        protected object syncObj = new object();
        private HBNegociosEventHandler eventHandler = null;


        public HomeBrokerHandlerNegocios(int clientNumber, Socket clientSocket)
            : base(clientNumber, clientSocket)
        {
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("HomeBrokerHandlerNegocios-", logger.Logger);
        }

        public void Start()
        {
            myThread = new Thread(new ThreadStart(queueProcessor));
            myThread.Name = myThreadName;
            myThread.Start();
        }

        public void Stop()
        {
            EventQueueManager.Instance.OnEventoHBNegocios -= eventHandler;
            bKeepRunning = false;
            while (myThread != null && myThread.IsAlive)
            {
                Thread.Sleep(250);
            }

            logger.Info(myThreadName + " Finalizada");
        }

        protected void queueProcessor()
        {
            logger.Info("Iniciando processamento da fila de envio home broker de Negocios");
            long lastLogTicks = 0;
            while (bKeepRunning)
            {
                try
                {
                    string msgNEG = null;
                    if (queueToHomeBroker.TryDequeue(out msgNEG))
                    {
                        SocketPackage.SendData(msgNEG, ClientSocket);

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

            logger.Info("Finalizando processamento da fila de envio home broker de Negocios");
        }

        protected override void listenEvents()
        {
            eventHandler = new HBNegociosEventHandler(despacharEventos);
            EventQueueManager.Instance.OnEventoHBNegocios += eventHandler;
        }

        private void despacharEventos(object sender, HBNegocioEventArgs args)
        {
            try
            {
                string mensagem = null;

                EventoHBNegocio eventoHB = args.Evento;

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

        protected override void enviarSnapshotHB()
        {
            try
            {
                logger.Debug("Gerando Snapshot de negocios");

                List<string> snapshot = ContainerManager.Instance.NegociosConsumer.SnapshotHB();

                logger.Debug("Snapshot com " + snapshot.Count + " itens");

                foreach (string mensagem in snapshot)
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
