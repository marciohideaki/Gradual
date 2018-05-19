using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using Gradual.Core.OMS.DropCopy.Lib;
using Gradual.Core.OMS.DropCopy.Lib.Dados;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using log4net;
using Gradual.Core.OMS.DropCopy.Lib.Util;
using System.ServiceModel.Channels;

namespace Gradual.Core.OMS.DropCopyProcessor
{
    public class DropCopyCallbackManager
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private List<IDropCopyCallback> _lstDropCopySubscribers = new List<IDropCopyCallback>();
        private static DropCopyCallbackManager _me = null;
        private bool _bKeepRunning = false;
        private Queue<OrdemInfo> qOrdemInfo = new Queue<OrdemInfo>();
        private Queue<OrderDbInfo> qDBOrdemInfoConvert = new Queue<OrderDbInfo>();
        private Thread _thHeartbeat = null;
        private Thread _thEnvioCallbackReports = null;
        private Thread _thInfoConverter = null;


        public static DropCopyCallbackManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new DropCopyCallbackManager();
                }

                return _me;
            }
        }

        public void Start()
        {
            _bKeepRunning = true;

            try
            {
                _thEnvioCallbackReports = new Thread(new ThreadStart(envioCallbackReportsProc));
                _thEnvioCallbackReports.Name = "thEnvioCallbackReports";
                _thEnvioCallbackReports.Start();

                _thHeartbeat = new Thread(new ThreadStart(heartBeatProc));
                _thHeartbeat.Name = "thHeartbeat";
                _thHeartbeat.Start();

                _thInfoConverter = new Thread(new ThreadStart(dbOrdemInfoConverter));
                _thInfoConverter.Name = "dbOrdemInfoConverter";
                _thInfoConverter.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Start(): " + ex.Message, ex);
            }
        }

        public void Stop()
        {
            logger.Info("Finalizando threads de tratamento dos assinantes");

            _bKeepRunning = false;

            int i=0;
            while (_thInfoConverter.IsAlive && i < 240)
            {
                logger.Info("Aguardando finalizar thread de conversao");
                Thread.Sleep(250);
            }

            i = 0;
            while (_thEnvioCallbackReports.IsAlive && i < 240)
            {
                logger.Info("Aguardando finalizar thread de envio de execution reports");
                Thread.Sleep(250);
            }

            i = 0;
            while (_thHeartbeat.IsAlive && i < 240)
            {
                logger.Info("Aguardando finalizar thread de heartbeat");
                Thread.Sleep(250);
            }


            logger.Info("Finalizando DropCopyCallbackManager");

        }

        /// <summary>
        /// 
        /// </summary>
        private void heartBeatProc()
        {
            long lastHeartBeat = DateTime.MinValue.Ticks;
            while (_bKeepRunning)
            {
                List<IDropCopyCallback> toDelete = new List<IDropCopyCallback>();
                TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - lastHeartBeat);
                if (ts.TotalMilliseconds >= 30000)
                {
                    lastHeartBeat = DateTime.Now.Ticks;
                    logger.Info("Enviando Heartbeat para " + _lstDropCopySubscribers.Count + " subscribers");

                    foreach (IDropCopyCallback subscriber in _lstDropCopySubscribers)
                    {
                        if (Ativador.IsValidChannel(subscriber))
                        {
                            try
                            {
                                subscriber.HeartBeat();
                                
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex);

                                logger.Info("Abortando channel para assinante: " + subscriber.ToString());
                                Ativador.AbortChannel(subscriber);

                                logger.Info("Removendo assinante da lista: " + subscriber.ToString());
                                toDelete.Add(subscriber);
                            }
                        }
                        else
                        {
                            logger.Info("Removendo assinante da lista: " + subscriber.ToString());
                            toDelete.Add(subscriber);
                        }
                    }

                    // Remove os assinantes abandonados/falhos da lista
                    foreach (IDropCopyCallback subscriber in toDelete)
                    {
                        _lstDropCopySubscribers.Remove(subscriber);
                    }

                    toDelete.Clear();

                }
                Thread.Sleep(250);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void envioCallbackReportsProc() 
        {
            List<IDropCopyCallback> toDelete = new List<IDropCopyCallback>();

            while (_bKeepRunning)
            {
                List<OrdemInfo> lstOrdems = null;
                lock (qOrdemInfo)
                {
                    if (qOrdemInfo.Count > 0 )
                    lstOrdems = qOrdemInfo.ToList();
                    qOrdemInfo.Clear();
                }

                if (lstOrdems != null)
                {
                    foreach (OrdemInfo info in lstOrdems)
                    {
                        foreach (IDropCopyCallback subscriber in _lstDropCopySubscribers)
                        {
                            if (Ativador.IsValidChannel(subscriber))
                            {
                                try
                                {
                                    subscriber.OrdemAlterada(info);
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex);

                                    logger.Info("Abortando channel para assinante: " + subscriber.ToString());
                                    Ativador.AbortChannel(subscriber);

                                    logger.Info("Removendo assinante da lista: " + subscriber.ToString());
                                    toDelete.Add(subscriber);
                                }
                            }
                            else
                            {
                                logger.Info("Removendo assinante da lista: " + subscriber.ToString());
                                toDelete.Add(subscriber);
                            }
                        }
                    }

                    // Remove os assinantes abandonados/falhos da lista
                    foreach (IDropCopyCallback subscriber in toDelete)
                    {
                        _lstDropCopySubscribers.Remove(subscriber);
                    }

                    toDelete.Clear();
                }
                else
                {
                    Thread.Sleep(100);
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void dbOrdemInfoConverter()
        {
            while (_bKeepRunning)
            {
                try
                {
                    if (qDBOrdemInfoConvert.Count > 0)
                    {
                        OrderDbInfo dbInfo = null;
                        lock (qDBOrdemInfoConvert)
                        {
                            dbInfo = qDBOrdemInfoConvert.Dequeue();
                        }

                        if (dbInfo != null)
                        {
                            OrdemInfo converted = Conversions.OrderDBInfo2OrdemInfo(dbInfo);

                            lock (qOrdemInfo)
                            {
                                qOrdemInfo.Enqueue(converted);
                            }
                        }
                    }
                    else
                        Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Error("dbOrdemInfoConverter:" + ex.Message, ex);
                    Thread.Sleep(100);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void AssinarDropCopyCallback()
        {
            IDropCopyCallback subscriber = Ativador.GetCallback<IDropCopyCallback>();

            logger.Info("Recebeu pedido de assinatura de callback de dropycopy: " + ((IContextChannel)subscriber).RemoteAddress.ToString());

            // Guarda a referencia do assinante na lista interna de
            // assinante
            lock (_lstDropCopySubscribers)
            {
                if (subscriber != null)
                {
                    _lstDropCopySubscribers.Add(subscriber);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public void EnqueueCallback(OrderDbInfo info)
        {
            try
            {
                lock (qDBOrdemInfoConvert)
                {
                    this.qDBOrdemInfoConvert.Enqueue(info);
                }
            }
            catch (Exception ex)
            {
                logger.Error("EnqueueCallback: " + ex.Message, ex);
            }
        }


    }
}
