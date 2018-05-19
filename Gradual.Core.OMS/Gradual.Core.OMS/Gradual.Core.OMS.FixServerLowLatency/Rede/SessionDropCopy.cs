using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Threading;

using log4net;

using QuickFix;
using QuickFix.FIX44;
using QuickFix.Fields;

using Gradual.Core.OMS.FixServerLowLatency.Util;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

using Cortex.OMS.FixUtilities.Lib;
using Cortex.OMS.ServidorFIX;

using Gradual.Core.OMS.LimiteManager.Streamer;
using Gradual.Core.OMS.LimiteManager.Lib.Dados;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using System.Collections.Concurrent;


namespace Gradual.Core.OMS.FixServerLowLatency.Rede
{
    public class SessionDropCopy
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        #region Private Variables
#if _CQUEUE
        ConcurrentQueue<QuickFix.Message> _queueMsg;
        
#else
        Queue<QuickFix.Message> _queueMsg;
#endif
        
        Thread _thQueue = null;
        bool _running;
        #endregion

//        public FixInitiator Initiator
//        {
//            get;
//            set;
//        }

        public FixSessionItem Config
        {
            get;
            set;
        }
        
        public SessionID Sessao
        {
            get;
            internal set;
        }

        public bool Conectado
        {
            get;
            set;
        }

        public SessionDropCopy()
        {
            //this.Initiator = null;
            this.Config = null;
            
            // this.LimitControl = null;
            _running = false;
            this.Conectado = false;
            
        }


        ~SessionDropCopy()
        {
            Stop();
        }
        #region Thread Controls
        /// <summary>
        /// Iniciar a thread de "listening" da queue
        /// </summary>
        public void Start()
        {
            try
            {
#if _CQUEUE
                _queueMsg = new ConcurrentQueue<QuickFix.Message>();
#else
                _queueMsg = new Queue<QuickFix.Message>();
#endif
                _running = true;
                _thQueue = new Thread(new ThreadStart(SendToClient));
                _thQueue.Priority = ThreadPriority.AboveNormal;
                _thQueue.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Start(): Problemas no start da thread. " + ex.Message, ex);
                throw ex;
            }
        }

        public void Stop()
        {
            if (!_running)
                return;
            _running = false;
            /*
            logger.Info("Parando gerenciador de limites para sessao: " + this.Config.SenderCompID);
            if (null != LimitControl)
            {
                LimitControl.Stop();
                LimitControl = null;
            }
            */

            if (_thQueue.IsAlive)
            {
                Thread.Sleep(100);
                _thQueue.Abort();
                _thQueue = null;
            }

#if _CQUEUE
            if (null != _queueMsg)
            {
                _queueMsg = null;
            }
#else
            if (null != _queueMsg)
            {
                _queueMsg.Clear();
                _queueMsg = null;
            }
#endif
        }

        #endregion

        #region Queue Controls
        public void AddMessage(QuickFix.Message msg)
        {
            try
            {
#if _CQUEUE
                _queueMsg.Enqueue(msg);
                lock (_queueMsg)
                    Monitor.Pulse(_queueMsg);
#else                
                lock (_queueMsg)
                {
                    _queueMsg.Enqueue(msg);
                    Monitor.Pulse(_queueMsg);
                }
#endif
            }
            catch (Exception ex)
            {
                logger.Error("AddMessage() - Erro na adicao da mensagem da fila: " + ex.Message, ex);
            }
        }

        private void SendToClient()
        {
            try
            {
                while (_running)
                {
                    try
                    {
                        QuickFix.Message msg = null;
#if _CQUEUE
                        if (!_queueMsg.TryDequeue(out msg))
                        {
                            lock (_queueMsg)
                                Monitor.Wait(_queueMsg, 50);
                        }
#else
                        lock (_queueMsg)
                        {
                            if (_queueMsg.Count > 0)
                            {
                                msg = _queueMsg.Dequeue();
                            }
                            else
                            {
                                Monitor.Wait(_queueMsg, 5);
                                continue;
                            }
                        }
#endif
                        if (null != msg)
                        {
                            this._processMessage(msg);
                            msg.Clear();
                        }
                        msg = null;
                        
                    }
                    catch (Exception ex)
                    {
                        logger.Error("erro no envio pro acceptor:" + ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("SendToClient() - Erro no envio para client: " + ex.Message, ex);
            }

        }

        private void _processMessage(QuickFix.Message msg)
        {
            try
            {
                if (null != msg)
                {
                    Session.SendToTarget(msg, this.Sessao);
                    // msg = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("_processMessage(): " + ex.Message, ex);
            }
        }



        #endregion
    }
}
