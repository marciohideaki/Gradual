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
    public class HomeBrokerHandlerLivroOfertas: HomeBrokerHandlerBase
    {
        protected bool bKeepRunning = true;
        protected Thread myThread = null;
        protected string myThreadName;
        protected ConcurrentQueue<string> queueToHomeBroker = new ConcurrentQueue<string>();
        protected object syncObj = new object();
        private HBLivroOfertasEventHandler eventHandlerLOF;

        public HomeBrokerHandlerLivroOfertas(int clientNumber, Socket clientSocket)
            : base(clientNumber, clientSocket)
        {
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            myThreadName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            MDSUtils.AddAppender("HomeBrokerHandlerLivroOfertas-", logger.Logger);
        }

        public void Start()
        {
            myThread = new Thread(new ThreadStart(queueProcessor));
            myThread.Name = myThreadName;
            myThread.Start();
        }

        public void Stop()
        {
            bKeepRunning = false;
            EventQueueManager.Instance.OnEventoHBLivroOfertas -= eventHandlerLOF;

            while (myThread != null && myThread.IsAlive)
            {
                Thread.Sleep(250);
            }

            logger.Info(myThreadName + " Finalizada");
        }

        protected void queueProcessor()
        {
            logger.Info("Iniciando processamento da fila de envio home broker de livro de ofertas");
            long lastLogTicks = 0;
            while (bKeepRunning)
            {
                try
                {
                    string msgLOF = null;
                    if (queueToHomeBroker.TryDequeue(out msgLOF))
                    {
                        SocketPackage.SendData(msgLOF, ClientSocket);

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
            eventHandlerLOF = new HBLivroOfertasEventHandler(despacharEventosLOF);
            EventQueueManager.Instance.OnEventoHBLivroOfertas += eventHandlerLOF;
        }

        private void despacharEventosLOF(object sender, HBLivroOfertasEventArgs args)
        {
            try
            {
                string mensagem = null;

                EventoHBLivroOfertas eventoHB = args.Evento;
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
                logger.Error("despacharEventosLOF(): " + ex.Message, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void enviarSnapshotHB()
        {
            try
            {
                logger.Info("Gerando Snapshot de Livro de Ofertas");

                List<string> snapshot = ContainerManager.Instance.LivroOfertasConsumer.SnapshotHomeBrokerLivroOfertas();

                logger.Info("Snapshot de LOF com " + snapshot.Count + " itens");

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
