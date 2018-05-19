using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using log4net;
using System.Configuration;
using Gradual.OMS.Library;
using System.Runtime.InteropServices;
using Gradual.OMS.SpreadMonitor.Lib.Dados;
using Gradual.OMS.SpreadMonitor.Lib.Eventos;

namespace Gradual.OMS.SpreadMonitor
{
    public class ThreadPoolManager
    {
        private struct SinalStruct
        {
            public string Instrumento { get; set; }
            public string Mensagem { get; set; }
        }

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ThreadPoolManager _me = null;
        private Thread _thProc = null;
        private bool _bKeepRunning;
        private ConcurrentQueue<SinalStruct> queueSinal = new ConcurrentQueue<SinalStruct>();

        private List<ThreadWorkerBase> poolThreadSpread = new List<ThreadWorkerBase>();
        private List<ThreadWorkerBase> poolThreadDiferencial = new List<ThreadWorkerBase>();
        private List<ThreadWorkerBase> poolThreadFinanceiro = new List<ThreadWorkerBase>();

        #region Events
        public event HttpAlgoritmoEventHander OnAlgoritmoEvent;
        #endregion // Events

        public static ThreadPoolManager Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new ThreadPoolManager();
                }
                return _me;
            }
        }

        public ThreadPoolManager()
        {
        }

        public void Start()
        {
            logger.Info("Iniciando processador da fila de sinais");

            _bKeepRunning = true;
            _thProc = new Thread(new ThreadStart(procSinal));
            _thProc.Name = "procSinal";
            _thProc.Start();
        }

        public void Stop()
        {
            logger.Info("Finalizando processador da fila de sinais");
            _bKeepRunning = false;

            while (_thProc != null && _thProc.IsAlive)
            {
                Thread.Sleep(100);
            }

            logger.Info("Processador da fila de sinais finalizado");
        }



        public void EnqueueSinal(string instrumento, string mensagem)
        {
            try
            {
                SinalStruct sinal = new SinalStruct();
                sinal.Instrumento = instrumento;
                sinal.Mensagem = mensagem;

                queueSinal.Enqueue(sinal);
            }
            catch (Exception ex)
            {
                logger.Error("EnqueueSinal: " + ex.Message, ex);
            }

        }


        private void procSinal()
        {
            long lastLogTicks = 0;
            while (_bKeepRunning)
            {
                try
                {
                    SinalStruct sinal;

                    if (queueSinal.TryDequeue(out sinal))
                    {
                        MessageBroker(sinal.Instrumento, sinal.Mensagem);

                        if (shouldLog(lastLogTicks))
                        {
                            logger.Info("Fila de sinais a processar: " + queueSinal.Count);
                            lastLogTicks = DateTime.Now.Ticks;
                        }
                        continue;
                    }
                    
                    Thread.Sleep(100);
                }
                catch(Exception ex )
                {
                    logger.Error("procSinal:" + ex.Message, ex);
                }
            }
        }

        public static bool shouldLog(long lastEventTicks)
        {
            if ((DateTime.Now.Ticks - lastEventTicks) > TimeSpan.TicksPerSecond)
                return true;

            return false;
        }

        public bool AddAlgoritmo(AlgoStruct algo)
        {
            bool retorno = false;
            try
            {
                ThreadWorkerBase worker = null;
                if (algo.TipoAlgorito == AlgoritmoEnum.DIFERENCIAL)
                {
                    bool bAdded = false;
                    foreach (ThreadWorkerBase runningworker in poolThreadDiferencial)
                    {
                        if (runningworker.HasRoom())
                        {
                            runningworker.AddAlgoritmo(algo);
                            bAdded = true;
                            break;
                        }
                    }

                    if (!bAdded)
                    {
                        worker = new ThreadWorkerDiferencial();
                        worker.Start();
                        worker.AddAlgoritmo(algo);
                        poolThreadDiferencial.Add(worker);
                    }
                }

                if (algo.TipoAlgorito == AlgoritmoEnum.FINANCEIRO)
                {
                    bool bAdded = false;
                    foreach (ThreadWorkerBase runningworker in poolThreadFinanceiro)
                    {
                        if (runningworker.HasRoom())
                        {
                            runningworker.AddAlgoritmo(algo);
                            bAdded = true;
                            break;
                        }
                    }

                    if (!bAdded)
                    {
                        worker = new ThreadWorkerDiferencial();
                        worker.AddAlgoritmo(algo);
                        poolThreadFinanceiro.Add(worker);
                    }
                }

                if (algo.TipoAlgorito == AlgoritmoEnum.SPREAD)
                {
                    bool bAdded = false;
                    foreach (ThreadWorkerBase runningworker in poolThreadSpread)
                    {
                        if (runningworker.HasRoom())
                        {
                            runningworker.AddAlgoritmo(algo);
                            bAdded = true;
                            break;
                        }
                    }

                    if (!bAdded)
                    {
                        worker = new ThreadWorkerDiferencial();
                        worker.AddAlgoritmo(algo);
                        poolThreadSpread.Add(worker);
                    }
                }

                retorno = true;
            }
            catch (Exception ex)
            {
                logger.Error("AddAlgoritmo()" + ex.Message, ex);
            }
            return retorno;
        }

        private void MessageBroker(string Instrumento, string Mensagem)
        {
            try
            {
                int headerLen = Marshal.SizeOf(new Header_MDS());
                int agregadoLen = Marshal.SizeOf(new MSG_LivroOfertaAgregado_MDS());

                logger.Debug("[" + Instrumento + "] [" + Mensagem + "]");


                switch (Mensagem.ToString().Substring(0, 2))
                {
                    case ConstantesMDS.Negocio:
                        break;
                    case ConstantesMDS.LivroOferta:
                        break;
                    case ConstantesMDS.Destaques:
                        break;
                    case ConstantesMDS.RankCorretora:
                        break;
                    case ConstantesMDS.Sonda:
                        logger.Info("Recebeu Sonda: [" + Mensagem + "]");
                        break;
                    case ConstantesMDS.LIVRO_AGREGADO:
                        Header_MDS headerMds = Utilities.MarshalFromStringBlock<Header_MDS>(Mensagem.Substring(0,headerLen));

                        MSG_LivroOfertaAgregado_MDS agregadoCompra = Utilities.MarshalFromStringBlock<MSG_LivroOfertaAgregado_MDS>(Mensagem.Substring(headerLen,agregadoLen));
                        
                        int qtdeEntradas = (Mensagem.Length - headerLen) / agregadoLen;

                        int indexVenda = headerLen + (qtdeEntradas / 2 * agregadoLen);
                        MSG_LivroOfertaAgregado_MDS agregadoVenda = Utilities.MarshalFromStringBlock<MSG_LivroOfertaAgregado_MDS>(Mensagem.Substring(indexVenda,agregadoLen));

                        CotacaoInfo cotacao = new CotacaoInfo();
                        cotacao.Instrumento = headerMds.Instrumento.ByteArrayToString();
                        cotacao.QuantidadeBid = Convert.ToInt64(agregadoCompra.Quantidade.ByteArrayToString());
                        cotacao.TopbPriceBid = agregadoCompra.Preco.ByteArrayToDecimal(3);
                        cotacao.QuantidadeAsk = Convert.ToInt64(agregadoVenda.Quantidade.ByteArrayToString());
                        cotacao.TopbPriceAsk = agregadoVenda.Preco.ByteArrayToDecimal(3);

                        logger.DebugFormat("{0} BidQty={1} BidPrice={2} AskQty={3} Ask=Price={4}",
                            cotacao.Instrumento,
                            cotacao.QuantidadeBid,
                            cotacao.TopbPriceBid,
                            cotacao.QuantidadeAsk,
                            cotacao.TopbPriceAsk);

                        enqueueToThreads(cotacao);
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error("MessageBroker(): " + ex.Message, ex);

                if (!String.IsNullOrEmpty(Instrumento))
                    logger.Error("Instrumento [" + Instrumento + "]");

                if (!String.IsNullOrEmpty(Mensagem))
                    logger.Error("Mensagem [" + Mensagem + "]");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cotacao"></param>
        private void enqueueToThreads(CotacaoInfo cotacao)
        {
            foreach (ThreadWorkerBase worker in poolThreadDiferencial)
            {
                worker.EnqueueCotacao(cotacao);
            }

            foreach (ThreadWorkerBase worker in poolThreadSpread)
            {
                worker.EnqueueCotacao(cotacao);
            }

            foreach (ThreadWorkerBase worker in poolThreadFinanceiro)
            {
                worker.EnqueueCotacao(cotacao);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idlogin"></param>
        /// <returns></returns>
        public List<Dictionary<string,string>> SnapshotStreamer(string idlogin)
        {
            List<AlgoStruct> snapshot = new List<AlgoStruct>();

            foreach (ThreadWorkerBase worker in poolThreadDiferencial)
            {
                snapshot.AddRange(worker.SnapshotStreamer(idlogin));
            }

            foreach (ThreadWorkerBase worker in poolThreadSpread)
            {
                snapshot.AddRange(worker.SnapshotStreamer(idlogin));
            }

            foreach (ThreadWorkerBase worker in poolThreadFinanceiro)
            {
                snapshot.AddRange(worker.SnapshotStreamer(idlogin));
            }

            List<Dictionary<string, string>> retorno = new List<Dictionary<string, string>>();

            foreach (AlgoStruct algo in snapshot)
            {
                retorno.Add(MDSUtils.montaMensagemStreamerAlgoritmo(ConstantesMDS.HTTP_ALGORITMOS_TIPO_ACAO_INCLUIR, algo, 2));
            }

            return retorno;
        }
    }
}
