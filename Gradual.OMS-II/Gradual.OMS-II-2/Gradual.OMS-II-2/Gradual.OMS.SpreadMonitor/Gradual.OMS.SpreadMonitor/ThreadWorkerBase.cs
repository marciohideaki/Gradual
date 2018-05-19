using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.SpreadMonitor.Lib.Dados;
using System.Collections.Concurrent;
using System.Threading;
using log4net;

namespace Gradual.OMS.SpreadMonitor
{
    public abstract class ThreadWorkerBase
    {
        protected static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected Dictionary<string, AlgoStruct> dctRegistros = new Dictionary<string, AlgoStruct>();
        protected ConcurrentDictionary<string, HashSet<string>> dctInstrumentos1 = new ConcurrentDictionary<string, HashSet<string>>();
        protected ConcurrentDictionary<string, HashSet<string>> dctInstrumentos2 = new ConcurrentDictionary<string, HashSet<string>>();
        protected ConcurrentQueue<CotacaoInfo> queueCotacaoInbound = new ConcurrentQueue<CotacaoInfo>();
        protected ConcurrentQueue<AlgoStruct> qStreamerOutbound = new ConcurrentQueue<AlgoStruct>();

        protected Thread _thCalculationsProcessor = null;
        protected bool _bKeepRunning;
        protected Thread _thDispatcher = null;

        #region Abstract Functions
        protected abstract int getAlgoritmosPorThread();
        public abstract AlgoStruct DoAlgoritmo(AlgoStruct algo, CotacaoInfo cotacao);
        #endregion //Abstract Functions

        #region Virtual Functions

        public virtual bool HasRoom()
        {
            lock (dctRegistros)
            {
                if (dctRegistros.Count < getAlgoritmosPorThread())
                    return true;
            }

            return false;
        }

        public virtual void Start()
        {
            logger.Info("Iniciando Worker de processamento de algoritmo " + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _bKeepRunning = true;

            _thDispatcher = new Thread(new ThreadStart(procDispatcher));

            _thCalculationsProcessor = new Thread(new ThreadStart(procCalculations));
            _thCalculationsProcessor.Start();
        }

        public virtual void Stop()
        {
            _bKeepRunning = false;

            while (_thCalculationsProcessor != null && _thCalculationsProcessor.IsAlive)
                Thread.Sleep(250);

            while (_thDispatcher != null && _thDispatcher.IsAlive)
                Thread.Sleep(250);

            logger.Info("Worker finalizado");
        }

        public virtual void EnqueueCotacao(CotacaoInfo cotacao)
        {
            try
            {
                queueCotacaoInbound.Enqueue(cotacao);
            }
            catch (Exception ex)
            {
                logger.Error("EnqueueCotacao():" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void procDispatcher()
        {
            logger.Info("Iniciando thread de despacho de calculos para o streamer");
            while (_bKeepRunning)
            {
                try
                {
                    AlgoStruct registro;

                    if (qStreamerOutbound.TryDequeue(out registro))
                    {
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("procDispatcher(): " + ex.Message, ex);
                }
            }
            logger.Info("Thread de despacho de calculos para o streamer finalizado!");
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void procCalculations()
        {
            logger.Info("Iniciando thread de tratamento da fila de cotacoes/calculos");
            while (_bKeepRunning)
            {
                try
                {
                    CotacaoInfo info;

                    if (queueCotacaoInbound.TryDequeue(out info))
                    {
                        Calculate(info);
                        continue;
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Error("procDispatcher(): " + ex.Message, ex);
                }
            }
            logger.Info("Thread de tratamento da fila de cotacoes/calculos finalizado");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        /// <param name="topbPrice"></param>
        /// <param name="qtde"></param>
        public virtual void Calculate(CotacaoInfo info)
        {
            if (dctInstrumentos1.ContainsKey(info.Instrumento))
            {
                HashSet<string> algos = null;

                if (dctInstrumentos1.TryGetValue(info.Instrumento, out algos))
                {
                    foreach (string algoID in algos)
                    {
                        AlgoStruct registro;
                        bool bFound = false;
                        lock(dctRegistros)
                        {
                            registro = dctRegistros[algoID];
                            bFound = true;
                        }

                        if ( bFound )
                        {
                            registro = DoAlgoritmo(registro, info);
                            qStreamerOutbound.Enqueue(registro);
                        }
                    }
                }
            }


            if (dctInstrumentos2.ContainsKey(info.Instrumento))
            {
                HashSet<string> algos = null;

                if (dctInstrumentos2.TryGetValue(info.Instrumento, out algos))
                {
                    foreach (string algoID in algos)
                    {
                        AlgoStruct registro;
                        bool bFound = false;
                        lock (dctRegistros)
                        {
                            registro = dctRegistros[algoID];
                            bFound = true;
                        }

                        if (bFound)
                        {
                            registro = DoAlgoritmo(registro, info);
                            qStreamerOutbound.Enqueue(registro);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="algo"></param>
        public virtual void AddAlgoritmo(AlgoStruct algo)
        {
            lock (dctRegistros)
            {
                if (!dctRegistros.ContainsKey(algo.IDRegistro))
                {
                    dctRegistros.Add(algo.IDRegistro, algo);
                }

                // 1a perna
                dctInstrumentos1.GetOrAdd(algo.Instrumento1, new HashSet<string>()).Add(algo.IDRegistro);

                // 2a perna
                dctInstrumentos2.GetOrAdd(algo.Instrumento2, new HashSet<string>()).Add(algo.IDRegistro);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="algo"></param>
        public virtual void RemoveAlgoritmo(AlgoStruct algo)
        {
            lock(dctRegistros)
            {
                if ( dctRegistros.ContainsKey(algo.IDRegistro) )
                {
                    dctRegistros.Remove(algo.IDRegistro);
                }

                // Sanity check
                if (dctInstrumentos1.ContainsKey(algo.Instrumento1))
                {
                    dctInstrumentos1[algo.Instrumento1].Remove(algo.IDRegistro);

                    if (dctInstrumentos1[algo.Instrumento1].Count == 0)
                    {
                        HashSet<string> algos;
                        dctInstrumentos1.TryRemove(algo.Instrumento1, out algos);
                    }
                }

                // Sanity check
                if (dctInstrumentos2.ContainsKey(algo.Instrumento2))
                {
                    dctInstrumentos2[algo.Instrumento2].Remove(algo.IDRegistro);

                    if (dctInstrumentos2[algo.Instrumento2].Count == 0)
                    {
                        HashSet<string> algos;
                        dctInstrumentos2.TryRemove(algo.Instrumento2, out algos);
                    }
                }
            }
        }

        #endregion // Virtual Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idlogin"></param>
        /// <returns></returns>
        public List<AlgoStruct> SnapshotStreamer(string idlogin)
        {
            List<AlgoStruct> retorno = new List<AlgoStruct>();

            foreach (AlgoStruct algo in dctRegistros.Values)
            {
                if (algo.IDLogin.Equals(idlogin) )
                {
                    retorno.Add(algo);
                }
            }

            return retorno;
        }
    }
}
