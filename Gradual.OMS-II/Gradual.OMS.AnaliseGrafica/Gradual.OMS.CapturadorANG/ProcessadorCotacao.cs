using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.AnaliseGrafica.Lib;
using System.Globalization;
using log4net;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using Gradual.OMS.Library;

namespace Gradual.OMS.CapturadorANG
{
    public class ProcessadorCotacao : IServicoControlavel
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected ServicoStatus _srvstatus = ServicoStatus.Parado;
        CultureInfo ciBR = CultureInfo.CreateSpecificCulture("pt-BR");
        protected Queue<CotacaoANG> queueCotacao = new Queue<CotacaoANG>();
        protected Queue<CotacaoANG> queueSerieHistorica = new Queue<CotacaoANG>();
        protected Dictionary<string, CotacaoANG> dctCotacao = new Dictionary<string, CotacaoANG>();
        protected bool _bKeepRunning = false;
        protected double _volumeIbovespa = 0;
        
        protected ANGPersistenciaDB _db = null;
        private MDSPackageSocket _mds = null;
        protected ProcessadorCotacaoConfig _config = null;

        protected Thread _threadCotacao = null;
        protected Thread _threadSerieHistorica = null;

        public void ProcessarMensagemSerieHistorica(string pMensagem)
        {
            try
            {
                CotacaoANG mensagem = new CotacaoANG();

                // Layout da mensagem de Serie Historica
                //
                // Nome	                    Tipo(Tamanho)   Observação
                // Tipo de Mensagem         X(2)            'SH'
                // Tipo de Bolsa            X(2)            Bovespa = BV, BM&F = BF
                // Data                     N(8)            Formato AAAAMMDD
                // Hora                     N(9)            Formato HHMMSSmmm (mmm = milisegundos)
                // Código do Instrumento    X(20)
                // 
                // Data da Cotação          N(8)            Formato AAAAMMDD
                // Hora da Cotação          N(9)            Formato HHMMSSmmm (mmm = milisegundos)
                // Preço Abertura           N(13)
                // Preço Fechamento         N(13)
                // Preço Médio              N(13)
                // Preço Máximo             N(13)
                // Preço Mínimo             N(13)
                // Ind Oscilação            X(1)            positiva: “ “ (espaço em branco), Variação negativa: “-“
                // Percentual Oscilação     N(8)
                // Melhor Oferta de Compra  N(13)
                // Melhor Oferta de Venda   N(13)
                // Quantidade de Negócios   N(8)
                // Quantidade de Papéis     N(12)
                // Volume Acumulado         N(13)
                // Preço de Ajuste          N(13)

                mensagem.A = pMensagem.Substring(21, 20).Trim();

                // Obtem o tipo da bolsa
                if (pMensagem.Substring(2, 2).Equals("BV"))
                    mensagem.Bo = "BOV";
                else
                    mensagem.Bo = "BMF";

                mensagem.Ab = Convert.ToDouble(pMensagem.Substring(58, 13), ciBR);
                mensagem.Mx = Convert.ToDouble(pMensagem.Substring(97, 13), ciBR);
                mensagem.Mi = Convert.ToDouble(pMensagem.Substring(110, 13), ciBR);
                mensagem.Me = Convert.ToDouble(pMensagem.Substring(84, 13), ciBR);
                mensagem.Fe = Convert.ToDouble(pMensagem.Substring(71, 13), ciBR);
                mensagem.Os = Convert.ToDouble(pMensagem.Substring(123, 9).Trim(), ciBR);
                mensagem.OfC = Convert.ToDouble(pMensagem.Substring(132, 13), ciBR);
                mensagem.OfV = Convert.ToDouble(pMensagem.Substring(145, 13), ciBR);
                mensagem.To = Convert.ToDouble(pMensagem.Substring(158, 8), ciBR);
                mensagem.Qt = Convert.ToDouble(pMensagem.Substring(166, 12), ciBR);
                mensagem.Vl = Convert.ToDouble(pMensagem.Substring(178, 13), ciBR);
                mensagem.Aj = Convert.ToDouble(pMensagem.Substring(191, 13), ciBR);

                String data = pMensagem.Substring(41, 8) + "000000";
                mensagem.Dt = DateTime.ParseExact(data, "yyyyMMddHHmmss", ciBR);

                lock (queueSerieHistorica)
                {
                    queueSerieHistorica.Enqueue(mensagem);
                }
            }
            catch (Exception ex)
            {
                logger.Error("ProcessarMensagem: " + ex.Message, ex);
            }
        }

        protected  void OnSerieHistorica(object sender, MDSMessageEventArgs args)
        {
            ProcessarMensagemSerieHistorica(args.Message);
        }

        public void SerieHistoricaRun()
        {
            bool bWait = false;
            while (_bKeepRunning)
            {
                List<CotacaoANG> tmpQueue = new List<CotacaoANG>();

                lock (queueSerieHistorica)
                {
                    tmpQueue = queueSerieHistorica.ToList();
                    queueSerieHistorica.Clear();
                }

                foreach (CotacaoANG mensagem in tmpQueue)
                {
                    ProcessaSerieHistorica(mensagem);
                }

                lock (queueSerieHistorica)
                {
                    if (queueSerieHistorica.Count == 0)
                        bWait = true;
                    else
                        bWait = false;
                }

                if (bWait)
                    Thread.Sleep(250);
            }
        }

        /// <summary>
        /// ProcessaSerieHistorica - 
        /// Essa funcao trata as mensagens de Serie Historica,
        /// efetuando a gravacao quando necessario no banco de dados
        /// </summary>
        /// <param name="cotacao"></param>
        public void ProcessaSerieHistorica(CotacaoANG cotacao)
        {
            try
            {
                GravarSerieHistorica(cotacao);
            }
            catch (Exception ex)
            {
                logger.Debug("ProcessaSerieHistorica: " + ex.Message);
            }
        }
        
        public void ProcessarMensagemCotacao(string pMensagem)
        {
            try
            {
                CotacaoANG cotacao = new CotacaoANG();

                // Layout da mensagem de negocio
                // Header
                // Nome	Tipo	Tamanho	Observação
                // Tipo de Mensagem X(2)
                // Tipo de Bolsa	X(2) Espaços, ou:
                //    Bovespa = BV
                //    BM&F = BF
                // Data	N(8) - Formato AAAAMMDD
                // Hora	N(9) - Formato HHMMSSmmm (mmm = milisegundos)
                // Código do Instrumento	X(20)

                // Tipo de Mensagem = NE
                // Nome	Tipo	Tamanho	Observação
                // Data	N(8) - Formato AAAAMMDD
                // Hora	N(9) - Formato HHMMSSmmm (mmm = milisegundos)
                // Corretora Compradora	N(8)
                // Corretora Vendedora	N(8)
                // Preço	N(13)
                // Quantidade N(12)
                // Máxima do dia	N(13)
                // Mínima do dia	N(13)
                // Volume Acumulado	N(13)
                // Número de Negócios	N(8)
                // Indicador de Variação em Relação ao Fechamento do Dia Anterior	X(1)
                //     Variação positiva: “ “ (espaço em branco)
                //     Variação negativa: “-“
                // Percentual de var. em relação ao Fechamento do Dia Anterior	N(8)
                // Estado do Papel	N(1)
                //    0 – Papel não negociado
                //    1 – Papel em leilão
                //    2 – Papel em negociação
                //    3 – Papel suspenso
                //    4 – Papel congelado
                //    5 – Papel inibido
                // Estado do Instrumento para Análise Gráfica N(1)
                //    0 - Inicial
                //    1 - Congela data/hora
                //    2 - Fechamento do dia
                //    3 - Despreza mensagens


                // Exemplos de mensagens:
                //<---              header             --->data    hora     comprad vend    preco        quantidade  max dia      min dia      vol          nneg    VvFech   E
                //0 2 4       12       21                  41      49       58      66      74           87          99           112          125          138      147     155           169                   191          204          217     225          238
                //NEBV20100702162034355PETR4               201007021618470000000013100000027000000026,800000000000200000000026,860000000026,5200209283409,0000010513 00001,3220000000029.900000000026.45C000000950000000026,79000000002800V000000270000000026,80000000000200
                //NEBV20100702162238788PETR4               201007021620510000000002700000131000000026,850000000000200000000026,870000000026,5200211588300,0000010621 00001,5120000000029.900000000026.45C000000270000000026,85000000000200V000000080000000026,86000000000200

                //try { this.HeaderTipoMensagem = pMensagem.Substring(0, 2); }

                string bolsa = pMensagem.Substring(2, 2);
                string ativo = pMensagem.Substring(21, 20).Trim();
                string dataNegocio = pMensagem.Substring(41, 8);
                string horaNegocio = pMensagem.Substring(49, 6);
                string preco = pMensagem.Substring(74, 13);
                string quantidade = pMensagem.Substring(87, 12);
                string oscilacao = pMensagem.Substring(146, 9).Trim();
                string compradora = pMensagem.Substring(58, 8);
                string estado = pMensagem.Substring(155, 1);
                string estadoAnaliseGrafica = pMensagem.Substring(156, 1);

                // Ignora mensagens que não estiverem no estado "Papel em negociação"
                if ( !estado.Equals("2") )
                    return;

                // Ignora mensagens que estiverem no estado "Despreza Mensagens"
                if ( estadoAnaliseGrafica.Equals("3") )
                    return;

                // Obtem o tipo da bolsa
                if ( bolsa.Equals("BV") )
                    cotacao.Bo = "BOV";
                else
                    cotacao.Bo = "BMF";

                cotacao.A = ativo;

                string dataHoraNegocio = dataNegocio + horaNegocio;

                if (dataHoraNegocio.StartsWith("0000") == false)
                {
                    cotacao.Pr = Convert.ToDouble(preco, ciBR);

                    // Ignora mensagens com preço zerado
                    if (cotacao.Pr == 0)
                        return;

                    // Despreza a data/hora das mensagens que estiverem no estado "Congela DataHora"
                    if (estadoAnaliseGrafica.Equals("1"))
                        cotacao.Dt = DateTime.MinValue;
                    else
                        cotacao.Dt = DateTime.ParseExact(dataHoraNegocio, "yyyyMMddHHmmss", ciBR);

                    cotacao.Qt = Convert.ToDouble(quantidade, ciBR);
                    cotacao.Vl = cotacao.Pr * cotacao.Qt;
                    cotacao.Os = Convert.ToDouble(oscilacao, ciBR);
                    cotacao.Cp = Convert.ToInt32(compradora);
                    cotacao.St = Convert.ToInt32(estadoAnaliseGrafica);

                    lock (queueCotacao)
                    {
                        queueCotacao.Enqueue(cotacao);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("ProcessarMensagem: " + ex.Message, ex);
            }
        }

        protected  void OnCotacao(object sender, MDSMessageEventArgs args)
        {
            ProcessarMensagemCotacao(args.Message);
        }

        public virtual void Run()
        {
            bool bWait = false;
            while (_bKeepRunning)
            {
                if (_mds == null || _mds.IsConectado() == false)
                {
                    _mds = new MDSPackageSocket();

                    _mds.IpAddr = _config.MDSAddress;
                    _mds.Port = _config.MDSPort;

                    _mds.OnFastQuoteReceived +=new MDSMessageReceivedHandler(OnCotacao);
                    _mds.OnSerieHistoricaReceived += new MDSMessageReceivedHandler(OnSerieHistorica);
                    _mds.OpenConnection();
                }

                List<CotacaoANG> tmpQueue = new List<CotacaoANG>();

                lock (queueCotacao)
                {
                    tmpQueue = queueCotacao.ToList();
                    queueCotacao.Clear();
                }

                foreach (CotacaoANG cotacao in tmpQueue)
                {
                    //ProcessaCotacao(cotacao);
                    AtualizarCotacao(cotacao);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cotacao"></param>
        public void GravarCotacao(CotacaoANG cotacao)
        {
            string dump = string.Format("Gravar ponto: {0} D:{1} A:{2} F:{3} m:{4} M:{5} O:{6} P:{7}",
                cotacao.A, 
                cotacao.Dt,
                cotacao.Ab,
                cotacao.Fe,
                cotacao.Mi,
                cotacao.Mx,
                cotacao.Os,
                cotacao.Pr);

            logger.Debug(dump);

            // Primeiro remove as cotacoes de dias anteriores, depois grava
            //_db.LimparCotacao(cotacao, _config.DiasRetencaoCotacao);

            _db.GravaAtivoCotacao(cotacao);
        }

        public void AtualizarCotacao(CotacaoANG cotacao)
        {
            try
            {
                if (!cotacao.A.Equals("IBOV"))
                {
                    if (cotacao.Cp != 0)
                        _volumeIbovespa += cotacao.Vl;
                    _db.AtualizaCotacao(cotacao);
                }
                else
                {
                    CotacaoANG cotacaoIbovespa = cotacao.Clone() as CotacaoANG;
                    cotacaoIbovespa.Vl = _volumeIbovespa;
                    _volumeIbovespa = 0;
                    _db.AtualizaCotacao(cotacaoIbovespa);
                }
            }
            catch (Exception ex)
            {
                logger.Debug("AtualizarCotacao: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cotacao"></param>
        public void GravarSerieHistorica(CotacaoANG cotacao)
        {
            string dump = string.Format("Gravar serie historica: {0} D:{1} A:{2} F:{3} m:{4} M:{5} O:{6}",
                cotacao.A,
                cotacao.Dt,
                cotacao.Ab,
                cotacao.Fe,
                cotacao.Mi,
                cotacao.Mx,
                cotacao.Os);

            logger.Debug(dump);

            _db.GravaSerieHistorica(cotacao);
        }

        #region IServicoControlavel Members
        /// <summary>
        /// Invocado pelo framework ao iniciar o serviço
        /// </summary>
        public virtual void IniciarServico()
        {
            logger.Info("*** Iniciando Processador de Cotacao ***");

            _bKeepRunning = true;

            _config = GerenciadorConfig.ReceberConfig<ProcessadorCotacaoConfig>();

            _db = new ANGPersistenciaDB();
            _db.ConnectionString = _config.ConnectionString;
            _db.MDSConnectionString = _config.MDSConnectionString;

            _threadCotacao = new Thread(new ThreadStart(Run));
            _threadCotacao.Start();

            _threadSerieHistorica = new Thread(new ThreadStart(SerieHistoricaRun));
            _threadSerieHistorica.Start();

            _srvstatus = ServicoStatus.EmExecucao;
        }

        /// <summary>
        /// Invocado pelo framework ao parar o servico
        /// </summary>
        public void PararServico()
        {
            logger.Info("*** Finalizando  Processador de Cotacao ***");

            _bKeepRunning = false;

            while (_threadCotacao.IsAlive)
            {
                Thread.Sleep(250);
            }

            while (_threadSerieHistorica.IsAlive)
            {
                Thread.Sleep(250);
            }

            _srvstatus = ServicoStatus.Parado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServicoStatus ReceberStatusServico()
        {
            return _srvstatus;
        }
        #endregion // IServicoControlavel Members
    }
}
