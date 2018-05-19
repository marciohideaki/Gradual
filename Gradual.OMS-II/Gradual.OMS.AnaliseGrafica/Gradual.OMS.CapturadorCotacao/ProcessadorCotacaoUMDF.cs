using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using Gradual.OMS.AnaliseGrafica.Lib;
using Gradual.OMS.Library;
using log4net;

namespace Gradual.OMS.CapturadorCotacao
{
    public class ProcessadorCotacaoUMDF : ProcessadorCotacao
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MDSPackageSocket[] _umdfSockets;

        public override void IniciarServico()
        {
            logger.Info("*** Iniciando Processador de Cotacao ***");

            _bKeepRunning = true;

            _config = GerenciadorConfig.ReceberConfig<ProcessadorCotacaoConfig>();

            UMDFConfig _umdfconfig = GerenciadorConfig.ReceberConfig<UMDFConfig>();

            _umdfSockets = new MDSPackageSocket[_umdfconfig.Portas.Count];
            int i = 0;
            foreach (int porta in _umdfconfig.Portas)
            {
                _umdfSockets[i] = new MDSPackageSocket();

                _umdfSockets[i].IpAddr = _config.MDSAddress;
                _umdfSockets[i].Port = porta;

                _umdfSockets[i].OnFastQuoteReceived += new MDSMessageReceivedHandler(OnCotacao);
                _umdfSockets[i].OnSerieHistoricaReceived += new MDSMessageReceivedHandler(OnSerieHistorica);

                i++;
            }

            _db = new ANGPersistenciaDB();
            _db.ConnectionString = _config.ConnectionString;
            _db.MDSConnectionString = _config.MDSConnectionString;

            _threadCotacao = new Thread(new ThreadStart(Run));
            _threadCotacao.Start();

            _threadSerieHistorica = new Thread(new ThreadStart(SerieHistoricaRun));
            _threadSerieHistorica.Start();

            _srvstatus = ServicoStatus.EmExecucao;
        }


        public override void Run()
        {
            bool bWait = false;
            while (_bKeepRunning)
            {
                foreach (MDSPackageSocket mdsSocket in _umdfSockets)
                {
                    if (mdsSocket != null && mdsSocket.IsConectado() == false)
                    {
                        logger.InfoFormat("Abrindo conexao com MDS [{0}:{1}]", mdsSocket.IpAddr, mdsSocket.Port);
                        mdsSocket.OpenConnection();
                    }
                }

                List<CotacaoANG> tmpQueue = new List<CotacaoANG>();

                lock (queueCotacao)
                {
                    tmpQueue = queueCotacao.ToList();
                    queueCotacao.Clear();
                }

                foreach (CotacaoANG cotacao in tmpQueue)
                {
                    ProcessaCotacao(cotacao);
                }

                lock (queueCotacao)
                {
                    if (queueCotacao.Count == 0)
                        bWait = true;
                    else
                        bWait = false;
                }

                if (bWait)
                    Thread.Sleep(250);
            }
        }

    }
}
