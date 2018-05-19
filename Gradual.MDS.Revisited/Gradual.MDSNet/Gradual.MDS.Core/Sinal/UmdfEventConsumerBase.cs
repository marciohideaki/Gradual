using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Eventos.Lib;
using Gradual.MDS.Core.Lib;
using System.Threading;
using log4net;
using System.Collections.Concurrent;

namespace Gradual.MDS.Core.Sinal
{
    public abstract class UmdfEventConsumerBase
    {
        protected ILog logger;
        protected ConcurrentQueue<EventoUmdf> queueEventos = new ConcurrentQueue<EventoUmdf>();
        protected ConcurrentQueue<EventoFIX> queueFIX = new ConcurrentQueue<EventoFIX>();
        protected bool bKeepRunning = false;
        protected Thread thProc = null;
        protected Thread thProcFix = null;
        protected string myThreadName;
        protected long lastEventTicks = 0;
        protected Object SyncObj = new Object();
        protected Object SyncObjFix = new Object();
        protected Dictionary<string, EventoUmdfWorkerState> poolWorkers = new Dictionary<string, EventoUmdfWorkerState>();
        protected Dictionary<string, FIXMessageWorkerState> poolFixWorkers = new Dictionary<string, FIXMessageWorkerState>();

        protected const string FIX_DEFAULT_WORKER = "DEFAULT_WORKER";

        protected virtual void beforeStart(){}
        protected virtual void afterStart(){}
        protected virtual void beforeStop(){}
        protected virtual void afterStop(){}



        protected class EventoUmdfWorkerState
        {
            public ConcurrentQueue<EventoUmdf> QueueEventos { get; set; }
            public Thread Thread { get; set; }
            public string ChannelID { get; set; }
            public Object SyncObj { get; set; }
            public long lastEnqueueLog { get; set; }

            public EventoUmdfWorkerState()
            {
                QueueEventos = new ConcurrentQueue<EventoUmdf>();
                SyncObj = new Object();
                lastEnqueueLog = 0;
            }
        }

        protected class FIXMessageWorkerState
        {
            public ConcurrentQueue<EventoFIX> QueueMensagens { get; set; }
            public Thread Thread { get; set; }
            public string ChannelID { get; set; }
            public Object SyncObjFIX { get; set; }
            public long lastEnqueueLog { get; set; }

            public FIXMessageWorkerState()
            {
                QueueMensagens = new ConcurrentQueue<EventoFIX>();
                SyncObjFIX = new Object();
                lastEnqueueLog = 0;
            }
        }

        public void Start()
        {
            logger.Info("Iniciando Consumer " + myThreadName);

            beforeStart();

            bKeepRunning = true;

            thProc = new Thread(new ThreadStart(procRun));
            thProc.Name = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            thProc.Start();

            thProcFix = new Thread(new ThreadStart(procRunFix));
            thProcFix.Name = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
            thProcFix.Start();

            afterStart();

            logger.Info("Consumer " + myThreadName + " iniciado");
        }

        public void Stop()
        {
            logger.Info("Finalizando " + myThreadName );

            beforeStop();

            bKeepRunning = false;
            while (thProc != null && thProc.IsAlive)
            {
                Thread.Sleep(100);
            }

            afterStop();

            logger.Info(myThreadName + " finalizado");
        }


        public virtual void EnqueueEventoUmdf(EventoUmdf e)
        {
                try
                {
                    //bool sinaliza = queueEventos.IsEmpty;
                    queueEventos.Enqueue(e);

                    //if (sinaliza)
                    //{
                    //    lock (SyncObj)
                    //    {
                    //        Monitor.Pulse(SyncObj);
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    logger.Error("EnqueueEventoUmdf(): " + ex.Message, ex);
                }
        }

        #region UMDF Handling
        protected virtual void procRun()
        {
            logger.Info(myThreadName + " - iniciando procRun()");
            long threadWatchDog = 0;
            while (bKeepRunning)
            {
                if (MDSUtils.shouldLog(threadWatchDog, 30))
                {
                    logger.Info("procRun() ativo");
                    threadWatchDog = DateTime.UtcNow.Ticks;
                }

                try
                {
                    EventoUmdf evento;
                    if ( queueEventos.TryDequeue(out evento)) 
                    {
                        //logger.InfoFormat("Distribuindo mensagem UMDF [{0}]", evento.MsgSeqNum);

                        processaEventoUmdf(evento);

                        if (MDSUtils.shouldLog(lastEventTicks))
                        {
                            logger.Info("EnqueueEventoUmdf - Mensagens na fila principal: " + queueEventos.Count);
                            lastEventTicks = DateTime.UtcNow.Ticks;
                        }

                        continue;
                    }

                    lock (SyncObj)
                    {
                        Monitor.Wait(SyncObj, 50);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("procRun(): " + ex.Message, ex);
                }
            }

            logger.Info(myThreadName + " End of procRun()");
        }

        protected virtual void processaEventoUmdf(EventoUmdf evento)
        {
            // Se nao existe thread para tratar o evento, cria
            EventoUmdfWorkerState workerState = null;

            if (evento == null)
            {
                logger.Error("EventoUmdf nao pode ser nulo");
                return;
            }

            if (String.IsNullOrEmpty(evento.ChannelID))
            {
                logger.Error("Eita porra....channelID nulo");
                return;
            }

            if (!poolWorkers.TryGetValue(evento.ChannelID, out workerState))
            {
                logger.Info("Criando worker para  channel [" + evento.ChannelID + "]");
                workerState = new EventoUmdfWorkerState();
                workerState.ChannelID = evento.ChannelID;
                workerState.Thread = new Thread(new ParameterizedThreadStart(eventoUmdfThreadWork));
                workerState.Thread.Name = myThreadName + "-Worker-" + workerState.ChannelID;
                workerState.Thread.Start(workerState);

                poolWorkers.Add(evento.ChannelID, workerState);
            }

            //bool sinaliza = workerState.QueueEventos.IsEmpty;
            workerState.QueueEventos.Enqueue(evento);
            if (MDSUtils.shouldLog(workerState.lastEnqueueLog, 30))
            {
                logger.Info("processaEventoUmdf para channelID [" + workerState.ChannelID + "] fila:" + workerState.QueueEventos.Count);
                workerState.lastEnqueueLog = DateTime.UtcNow.Ticks;
            }

            //if (sinaliza)
            //{
            //    lock (workerState.SyncObj)
            //    {
            //        Monitor.Pulse(workerState.SyncObj);
            //    }
            //}
        }

        protected void eventoUmdfThreadWork(object param)
        {
            EventoUmdfWorkerState workerstate = param as EventoUmdfWorkerState;
            long lastTick = 0;
            long threadWatchDog = 0;

            if (workerstate != null)
            {
                logger.Info("Iniciando eventoUmdfThreadWork para channelID [" + workerstate.ChannelID + "]");

                while (bKeepRunning)
                {
                    if (MDSUtils.shouldLog(threadWatchDog, 30))
                    {
                        logger.Info("eventoUmdfThreadWork(" + workerstate.ChannelID + ") ativo");
                        threadWatchDog = DateTime.UtcNow.Ticks;
                    }

                    try
                    {
                        EventoUmdf evento;
                        if ( workerstate.QueueEventos.TryDequeue(out evento) )
                        {
                            //logger.InfoFormat("Processando mensagem UMDF [{0}]", evento.MsgSeqNum);

                            trataEventoUmdf(evento);

                            if (MDSUtils.shouldLog(lastTick))
                            {
                                logger.Info("eventoUmdfThreadWork(" + workerstate.ChannelID + ") msgs na fila: " + workerstate.QueueEventos.Count);
                                lastTick = DateTime.UtcNow.Ticks;
                            }

                            continue;
                        }

                        lock (workerstate.SyncObj)
                        {
                            Monitor.Wait(workerstate.SyncObj,50);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("eventoUmdfThreadWork(" + workerstate.ChannelID + "): " + ex.Message, ex);
                    }
                }

                logger.Info("Finalizando eventoUmdfThreadWork para channelID [" + workerstate.ChannelID + "]");
            }
        }

        protected abstract void trataEventoUmdf(EventoUmdf evento);
        #endregion // UMDF Handling

        #region FIX Handling
        public virtual void EnqueueFIX(EventoFIX e)
        {
            try
            {
                bool sinaliza = queueFIX.IsEmpty;
                queueFIX.Enqueue(e);

                if (sinaliza)
                {
                    lock (SyncObjFix)
                    {
                        Monitor.Pulse(SyncObjFix);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("EnqueueFIX(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void procRunFix()
        {
            logger.Info(myThreadName + " - iniciando procRunFix()");
            
            long threadWatchDog = 0;
            
            while (bKeepRunning)
            {
                if (MDSUtils.shouldLog(threadWatchDog, 30))
                {
                    logger.Info("procRunFix() ativo");
                    threadWatchDog = DateTime.UtcNow.Ticks;
                }

                try
                {
                    EventoFIX evento;
                    if (queueFIX.TryDequeue(out evento))
                    {
                        processaMensagemFIX(evento);

                        if (MDSUtils.shouldLog(lastEventTicks))
                        {
                            logger.Info("procRunFix - Mensagens na fila principal: " + queueFIX.Count);
                            lastEventTicks = DateTime.UtcNow.Ticks;
                        }

                        continue;
                    }

                    lock (SyncObjFix)
                    {
                        Monitor.Wait(SyncObjFix, 50);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("procRunFix(): " + ex.Message, ex);
                }
            }

            logger.Info(myThreadName + " End of procRunFix()");
        }


        protected virtual void processaMensagemFIX(EventoFIX evento)
        {
            // Se nao existe thread para tratar o evento, cria
            FIXMessageWorkerState workerState = null;

            if (evento == null)
            {
                logger.Error("Mensagem FIX nao pode ser nula");
                return;
            }

            if (String.IsNullOrEmpty(evento.ChannelID))
            {
                logger.Error("Eita porra....channelID nulo");
                return;
            }

            if (!poolFixWorkers.TryGetValue(evento.ChannelID, out workerState))
            {
                logger.Info("Criando worker para  channel [" + evento.ChannelID + "]");
                workerState = new FIXMessageWorkerState();
                workerState.ChannelID = evento.ChannelID;
                workerState.Thread = new Thread(new ParameterizedThreadStart(messageFixThreadWork));
                workerState.Thread.Name = myThreadName + "-Worker-" + workerState.ChannelID;
                workerState.Thread.Start(workerState);

                poolFixWorkers.Add(evento.ChannelID, workerState);
            }

            bool bsinaliza = workerState.QueueMensagens.IsEmpty;
            workerState.QueueMensagens.Enqueue(evento);
            if (MDSUtils.shouldLog(workerState.lastEnqueueLog, 30))
            {
                logger.Info("processaMensagemFIX para channelID [" + workerState.ChannelID + "] fila:" + workerState.QueueMensagens.Count);
                workerState.lastEnqueueLog = DateTime.UtcNow.Ticks;
            }

            //if (bsinaliza)
            //{
            //    lock (workerState.SyncObjFIX)
            //    {
            //        Monitor.Pulse(workerState.SyncObjFIX);
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        protected void messageFixThreadWork(object param)
        {
            FIXMessageWorkerState workerstate = param as FIXMessageWorkerState;
            long lastTick = 0;
            long threadWatchDog = 0;

            if (workerstate != null)
            {
                logger.Info("Iniciando messageFixThreadWork para channelID [" + workerstate.ChannelID + "]");

                while (bKeepRunning)
                {
                    if (MDSUtils.shouldLog(threadWatchDog, 30))
                    {
                        logger.Info("messageFixThreadWork(" + workerstate.ChannelID + ") ativo");
                        threadWatchDog = DateTime.UtcNow.Ticks;
                    }

                    try
                    {
                        EventoFIX evento;
                        if (workerstate.QueueMensagens.TryDequeue(out evento))
                        {
                            trataMensagemFIX(evento);

                            if (MDSUtils.shouldLog(lastTick))
                            {
                                logger.Info("messageFixThreadWork(" + workerstate.ChannelID + ") msgs na fila: " + workerstate.QueueMensagens.Count);
                                lastTick = DateTime.UtcNow.Ticks;
                            }

                            continue;
                        }

                        lock (workerstate.SyncObjFIX)
                        {
                            Monitor.Wait(workerstate.SyncObjFIX, 50);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("messageFixThreadWork(" + workerstate.ChannelID + "): " + ex.Message, ex);
                    }
                }

                logger.Info("Finalizando messageFixThreadWork para channelID [" + workerstate.ChannelID + "]");
            }
        }

        protected abstract void trataMensagemFIX(EventoFIX evento);

        #endregion //FIX Handling

    }
}
