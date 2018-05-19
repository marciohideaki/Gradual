using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Eventos.Lib;
using Gradual.MDS.Eventos.Lib.EventsArgs;
using System.Threading;
using System.Collections.Concurrent;

namespace Gradual.MDS.Core
{
    public class EventQueueManager
    {
        protected static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static EventQueueManager _me = null;

        #region filas eventos
        private ConcurrentQueue<EventoHttpNegocio> queueHttpNEG = new ConcurrentQueue<EventoHttpNegocio>();
        private ConcurrentQueue<EventoHttpLivroNegocios> queueHttpLNG = new ConcurrentQueue<EventoHttpLivroNegocios>();
        private ConcurrentQueue<EventoHttpLivroOfertas> queueHttpLOF = new ConcurrentQueue<EventoHttpLivroOfertas>();
        private ConcurrentQueue<EventoHttpLivroOfertasAgregado> queueHttpLOA = new ConcurrentQueue<EventoHttpLivroOfertasAgregado>();

        private ConcurrentQueue<EventoHBNegocio> queueHBNEG = new ConcurrentQueue<EventoHBNegocio>();
        private ConcurrentQueue<EventoHBLivroOfertas> queueHBLOF = new ConcurrentQueue<EventoHBLivroOfertas>();
        private ConcurrentQueue<EventoHBLivroOfertasAgregado> queueHBLOA = new ConcurrentQueue<EventoHBLivroOfertasAgregado>();

        private ConcurrentQueue<EventoNegocioANG> queueANG = new ConcurrentQueue<EventoNegocioANG>();

        private object syncQueueHttpNEG = new object();
        private object syncQueueHttpLNG = new object();
        private object syncQueueHttpLOF = new object();
        private object syncQueueHttpLOA = new object();

        private object syncQueueHBNEG = new object();
        private object syncQueueHBLOF = new object();
        private object syncQueueHBLOA = new object();

        private object syncQueueANG = new object();

        #endregion //filas eventos

        #region Events  
        public event AnaliseGraficaNegocioEventHandler OnEventoNegocioAnaliseGrafica;
        public event HBLivroOfertasAgregadoEventHandler OnEventoHBLivroOfertasAgregado;
        public event HBLivroOfertasEventHandler OnEventoHBLivroOfertas;
        public event HBNegociosEventHandler OnEventoHBNegocios;
        public event HttpLivroNegociosEventHandler OnEventoHttpLivroNegocios;
        public event HttpLivroOfertasAgregadoEventHandler OnEventoHttpLivroOfertasAgregado;
        public event HttpLivroOfertasEventHandler OnEventoHttpLivroOfertas;
        public event HttpNegocioEventHander OnEventoHttpNegocios;
        #endregion // Events

        private Thread thProcANGQueueNegocio = null;
        private Thread thProcHBQueueLOA = null;
        private Thread thProcHBQueueLOF = null;
        private Thread thProcHBQueueNEG = null;
        private Thread thProcHttpQueueLOA = null;
        private Thread thProcHttpQueueLOF = null;
        private Thread thProcHttpQueueLNG = null;
        private Thread thProcHttpQueueNEG = null;
        private bool bKeepRunning = false;

        private const int QUEUE_WAIT_TIMEOUT = 50;

        public static EventQueueManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new EventQueueManager();
                }

                return _me;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            try
            {

                logger.Info("Iniciando threads de distribuicao de eventos");
                bKeepRunning = true;

                thProcANGQueueNegocio = new Thread(new ThreadStart(this.procANGQueueNegocio));
                thProcANGQueueNegocio.Name = "thProcANGQueueNegocio";
                thProcANGQueueNegocio.Start();

                thProcHBQueueLOA = new Thread(new ThreadStart(this.procHBQueueLOA));
                thProcHBQueueLOA.Name = "thProcHBQueueLOA";
                thProcHBQueueLOA.Start();

                thProcHBQueueLOF = new Thread(new ThreadStart(this.procHBQueueLOF));
                thProcHBQueueLOF.Name = "thProcHBQueueLOF";
                thProcHBQueueLOF.Start();

                thProcHBQueueNEG = new Thread(new ThreadStart(this.procHBQueueNEG));
                thProcHBQueueNEG.Name = "thProcHBQueueNEG";
                thProcHBQueueNEG.Start();

                thProcHttpQueueLOA = new Thread(new ThreadStart(this.procHttpQueueLOA));
                thProcHttpQueueLOA.Name = "thProcHttpQueueLOA";
                thProcHttpQueueLOA.Start();

                thProcHttpQueueLOF = new Thread(new ThreadStart(this.procHttpQueueLOF));
                thProcHttpQueueLOF.Name = "thProcHttpQueueLOF";
                thProcHttpQueueLOF.Start();

                thProcHttpQueueLNG = new Thread(new ThreadStart(this.procHttpQueueLNG));
                thProcHttpQueueLNG.Name = "thProcHttpQueueLNG";
                thProcHttpQueueLNG.Start();

                thProcHttpQueueNEG = new Thread(new ThreadStart(this.procHttpQueueNEG));
                thProcHttpQueueNEG.Name = "thProcHttpQueueNEG";
                thProcHttpQueueNEG.Start();

                while (!thProcANGQueueNegocio.IsAlive &&
                        !thProcHBQueueLOA.IsAlive &&
                        !thProcHBQueueLOF.IsAlive &&
                        !thProcHBQueueNEG.IsAlive &&
                        !thProcHttpQueueLOA.IsAlive &&
                        !thProcHttpQueueLOF.IsAlive &&
                        !thProcHttpQueueLNG.IsAlive &&
                        !thProcHttpQueueNEG.IsAlive)
                {
                    logger.Info("Aguardando inicializacao das threads de processamento das filas");
                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Start():" + ex.Message, ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            try
            {

                logger.Info("Finalizando threads de distribuicao de eventos");
                bKeepRunning = false;

                while (thProcANGQueueNegocio.IsAlive &&
                        thProcHBQueueLOA.IsAlive &&
                        thProcHBQueueLOF.IsAlive &&
                        thProcHBQueueNEG.IsAlive &&
                        thProcHttpQueueLOA.IsAlive &&
                        thProcHttpQueueLOF.IsAlive &&
                        thProcHttpQueueLNG.IsAlive &&
                        thProcHttpQueueNEG.IsAlive)
                {
                    logger.Info("Aguardando Finalizacao das threads de processamento das filas");
                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Start():" + ex.Message, ex);
            }

        }

        #region EventEnqueuers
        public void SendEvent(EventoHBLivroOfertas e)
        {
            try
            {
                //bool bsinaliza = queueHBLOF.IsEmpty;
                queueHBLOF.Enqueue(e);
                //if (bsinaliza)
                //{
                //    lock (syncQueueHBLOF)
                //    {
                //        Monitor.Pulse(syncQueueHBLOF);
                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.Error("SendEvent(EventoHBLivroOfertas): " + ex.Message, ex);
            }
        }

        public void SendEvent(EventoHBLivroOfertasAgregado e)
        {
            try
            {
               // bool bsinaliza = queueHBLOA.IsEmpty;
                queueHBLOA.Enqueue(e);

                //if (bsinaliza)
                //{
                //    lock (syncQueueHBLOA)
                //    {
                //        Monitor.Pulse(syncQueueHBLOA);
                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.Error("SendEvent(EventoHBLivroOfertasAgregado): " + ex.Message, ex);
            }
        }

        public void SendEvent(EventoHBNegocio e)
        {
            try
            {
                //bool bsinaliza = queueHBNEG.IsEmpty;
                queueHBNEG.Enqueue(e);
                //if (bsinaliza)
                //{
                //    lock (syncQueueHBNEG)
                //    {
                //        Monitor.Pulse(syncQueueHBNEG);
                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.Error("SendEvent(EventoHBNegocio): " + ex.Message, ex);
            }
        }

        public void SendEvent(EventoHttpLivroNegocios e)
        {
            try
            {
                //bool bsinaliza = queueHttpLNG.IsEmpty;
                queueHttpLNG.Enqueue(e);
                //if (bsinaliza)
                //{
                //    lock (syncQueueHttpLNG)
                //    {
                //        Monitor.Pulse(syncQueueHttpLNG);
                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.Error("SendEvent(EventoHttpLivroNegocios): " + ex.Message, ex);
            }
        }

        public void SendEvent(EventoHttpLivroOfertas e)
        {
            try
            {
                //bool bsinaliza = queueHttpLOF.IsEmpty;
                queueHttpLOF.Enqueue(e);
                //if (bsinaliza)
                //{
                //    lock (syncQueueHttpLOF)
                //    {
                //        Monitor.Pulse(syncQueueHttpLOF);
                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.Error("SendEvent(EventoHttpLivroOfertas): " + ex.Message, ex);
            }
        }

        public void SendEvent(EventoHttpLivroOfertasAgregado e)
        {
            try
            {
                //bool bsinaliza = queueHttpLOA.IsEmpty;
                queueHttpLOA.Enqueue(e);
                //if (bsinaliza)
                //{
                //    lock (syncQueueHttpLOA)
                //    {
                //        Monitor.Pulse(syncQueueHttpLOA);
                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.Error("SendEvent(EventoHttpLivroOfertasAgregado): " + ex.Message, ex);
            }
        }

        public void SendEvent(EventoHttpNegocio e)
        {
            try
            {
                //bool bsinaliza = queueHttpNEG.IsEmpty;
                queueHttpNEG.Enqueue(e);
                //if (bsinaliza)
                //{
                //    lock (syncQueueHttpNEG)
                //    {
                //        Monitor.Pulse(syncQueueHttpNEG);
                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.Error("SendEvent(EventoHttpNegocio): " + ex.Message, ex);
            }
        }

        public void SendEvent(EventoNegocioANG e)
        {
            try
            {
                //bool bsinaliza = queueANG.IsEmpty;
                queueANG.Enqueue(e);
                //if (bsinaliza)
                //{
                //    lock (syncQueueANG)
                //    {
                //        Monitor.Pulse(syncQueueANG);
                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.Error("SendEvent(EventoNegocioANG): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        private void procANGQueueNegocio()
        {
            long lstEvent = 0;

            logger.Info("Inicializando thread do processamento da fila de eventos de Negocios ANG");

            while(bKeepRunning)
            {
                try
                {
                    EventoNegocioANG e;
                    if ( queueANG.TryDequeue( out e ) )
                    {
                        if (OnEventoNegocioAnaliseGrafica != null)
                        {
                            AnaliseGraficaNegocioEventArgs args = new AnaliseGraficaNegocioEventArgs();
                            args.Evento = e;
                            OnEventoNegocioAnaliseGrafica(this, args);
                        }

                        if ( MDSUtils.shouldLog(lstEvent) )
                        {
                            lstEvent = DateTime.UtcNow.Ticks;
                            logger.Info("Fila queueANG: " + queueANG.Count + " eventos.");
                        }

                        continue;
                    }

                    lock (syncQueueANG)
                    {
                        Monitor.Wait(syncQueueANG, QUEUE_WAIT_TIMEOUT);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("procANGQueueNegocio: " + ex.Message, ex);
                }
            }
        }
        
        private void procHBQueueLOA()
        {
            long lstEvent = 0;

            logger.Info("Inicializando thread do processamento da fila de eventos de Livro de Ofertas Agregado HomeBroker");

            while (bKeepRunning)
            {
                try
                {
                    EventoHBLivroOfertasAgregado e;
                    if (queueHBLOA.TryDequeue(out e))
                    {
                        if (OnEventoHBLivroOfertasAgregado != null )
                        {
                            HBLivroOfertasAgregadoEventArgs args = new HBLivroOfertasAgregadoEventArgs();
                            args.Evento = e;
                            OnEventoHBLivroOfertasAgregado(this, args);
                        }

                        if (MDSUtils.shouldLog(lstEvent))
                        {
                            lstEvent = DateTime.UtcNow.Ticks;
                            logger.Info("Fila queueHBLOA: " + queueHBLOA.Count + " eventos.");
                        }

                        continue;
                    }

                    lock (syncQueueHBLOA)
                    {
                        Monitor.Wait(syncQueueHBLOA, QUEUE_WAIT_TIMEOUT);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("procHBQueueLOA: " + ex.Message, ex);
                }
            }
        }

        private void procHBQueueLOF()
        {
            long lstEvent = 0;

            logger.Info("Inicializando thread do processamento da fila de eventos de Livro de Ofertas HomeBroker");

            while (bKeepRunning)
            {
                try
                {
                    EventoHBLivroOfertas e;
                    if (queueHBLOF.TryDequeue(out e))
                    {
                        if (OnEventoHBLivroOfertas != null)
                        {
                            HBLivroOfertasEventArgs args = new HBLivroOfertasEventArgs();
                            args.Evento = e;
                            OnEventoHBLivroOfertas(this, args);
                        }

                        if (MDSUtils.shouldLog(lstEvent))
                        {
                            lstEvent = DateTime.UtcNow.Ticks;
                            logger.Info("Fila queueHBLOF: " + queueHBLOF.Count + " eventos.");
                        }

                        continue;
                    }

                    lock (syncQueueHBLOF)
                    {
                        Monitor.Wait(syncQueueHBLOF, QUEUE_WAIT_TIMEOUT);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("procHBQueueLOF: " + ex.Message, ex);
                }
            }
        }

        private void procHBQueueNEG()
        {
            long lstEvent = 0;
            logger.Info("Inicializando thread do processamento da fila de eventos de Negocios HomeBroker");
            while (bKeepRunning)
            {
                try
                {
                    EventoHBNegocio e;
                    if ( queueHBNEG.TryDequeue(out e) )
                    {
                        if (OnEventoHBNegocios != null)
                        {
                            HBNegocioEventArgs args = new HBNegocioEventArgs();
                            args.Evento = e;
                            OnEventoHBNegocios(this, args);
                        }

                        if (MDSUtils.shouldLog(lstEvent))
                        {
                            lstEvent = DateTime.UtcNow.Ticks;
                            logger.Info("Fila queueHBNEG: " + queueHBNEG.Count + " eventos.");
                        }

                        continue;
                    }

                    lock (syncQueueHBNEG)
                    {
                        Monitor.Wait(syncQueueHBNEG, QUEUE_WAIT_TIMEOUT);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("procHBQueueNEG: " + ex.Message, ex);
                }
            }
        }

        private void procHttpQueueLOA()
        {
            long lstEvent = 0;
            logger.Info("Inicializando thread do processamento da fila de eventos de Livro de Ofertas Agregado Streamer");
            while (bKeepRunning)
            {
                try
                {
                    EventoHttpLivroOfertasAgregado e;
                    if ( queueHttpLOA.TryDequeue(out e) )
                    {
                        if (OnEventoHttpLivroOfertasAgregado != null )
                        {
                            HttpLivroOfertasAgregadoEventArgs args = new HttpLivroOfertasAgregadoEventArgs();
                            args.Evento = e;
                            OnEventoHttpLivroOfertasAgregado(this, args);
                        }

                        if (MDSUtils.shouldLog(lstEvent))
                        {
                            lstEvent = DateTime.UtcNow.Ticks;
                            logger.Info("Fila queueHttpLOA: " + queueHttpLOA.Count + " eventos.");
                        }

                        continue;
                    }

                    lock (syncQueueHttpLOA)
                    {
                        Monitor.Wait(syncQueueHttpLOA, QUEUE_WAIT_TIMEOUT);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("procHttpQueueLOA: " + ex.Message, ex);
                }
            }
        }

        private void procHttpQueueLOF()
        {
            long lstEvent = 0;
            logger.Info("Inicializando thread do processamento da fila de eventos de Livro de Ofertas Streamer");
            while (bKeepRunning)
            {
                try
                {
                    EventoHttpLivroOfertas e;
                    if ( queueHttpLOF.TryDequeue(out e) )
                    {
                        if (OnEventoHttpLivroOfertas != null)
                        {
                            HttpLivroOfertasEventArgs args = new HttpLivroOfertasEventArgs();
                            args.Evento = e;
                            OnEventoHttpLivroOfertas(this, args);
                        }

                        if (MDSUtils.shouldLog(lstEvent))
                        {
                            lstEvent = DateTime.UtcNow.Ticks;
                            logger.Info("Fila queueHttpLOF: " + queueHttpLOF.Count + " eventos.");
                        }

                        continue;
                    }

                    lock (syncQueueHttpLOF)
                    {
                        Monitor.Wait(syncQueueHttpLOF,QUEUE_WAIT_TIMEOUT);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("procHttpQueueLOF: " + ex.Message, ex);
                }
            }
        }

        private void procHttpQueueLNG()
        {
            long lstEvent = 0;
            logger.Info("Inicializando thread do processamento da fila de eventos de Livro de Negocios Streamer");
            while (bKeepRunning)
            {
                try
                {
                    EventoHttpLivroNegocios e;
                    if ( queueHttpLNG.TryDequeue(out e) )
                    {
                        if (OnEventoHttpLivroNegocios != null)
                        {
                            HttpLivroNegociosEventArgs args = new HttpLivroNegociosEventArgs();
                            args.Evento = e;
                            OnEventoHttpLivroNegocios(this, args);
                        }

                        if (MDSUtils.shouldLog(lstEvent))
                        {
                            lstEvent = DateTime.UtcNow.Ticks;
                            logger.Info("Fila queueHttpLNG: " + queueHttpLNG.Count + " eventos.");
                        }

                        continue;
                    }

                    lock (syncQueueHttpLNG)
                    {
                        Monitor.Wait(syncQueueHttpLNG, QUEUE_WAIT_TIMEOUT);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("procHttpQueueLNG: " + ex.Message, ex);
                }
            }
        }

        private void procHttpQueueNEG()
        {
            long lstEvent = 0;
            logger.Info("Inicializando thread do processamento da fila de eventos de Negocios Streamer");
            while (bKeepRunning)
            {
                try
                {
                    EventoHttpNegocio e;
                    if ( queueHttpNEG.TryDequeue(out e) )
                    {
                        if (OnEventoHttpNegocios != null)
                        {
                            HttpNegocioEventArgs args = new HttpNegocioEventArgs();
                            args.Evento = e;
                            OnEventoHttpNegocios(this, args);
                        }

                        if (MDSUtils.shouldLog(lstEvent))
                        {
                            lstEvent = DateTime.UtcNow.Ticks;
                            logger.Info("Fila queueHttpNEG: " + queueHttpNEG.Count + " eventos.");
                        }

                        continue;
                    }

                    lock (syncQueueHttpNEG)
                    {
                        Monitor.Wait(syncQueueHttpNEG, QUEUE_WAIT_TIMEOUT);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("procHttpQueueNEG: " + ex.Message, ex);
                }
            }
        }

        #endregion
    }
}
