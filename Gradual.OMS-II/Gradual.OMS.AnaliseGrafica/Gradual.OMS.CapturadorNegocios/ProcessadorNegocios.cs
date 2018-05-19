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

namespace Gradual.OMS.CapturadorNegocios
{
    public class ProcessadorNegocios : IServicoControlavel
    {
        private ServicoStatus _srvstatus = ServicoStatus.Parado;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CultureInfo ciBR = CultureInfo.CreateSpecificCulture("pt-BR");
        Queue<CotacaoANG> queueCotacao = new Queue<CotacaoANG>();
        Dictionary<string, CotacaoANG> dctCotacao = new Dictionary<string, CotacaoANG>();
        private bool _bKeepRunning = false;
        
        ANGPersistenciaDB _db = null;
        MDSPackageSocket _mds = null;
        ProcessadorNegociosConfig _config = null;

        Thread _threadNegocios = null;

        public void ProcessarMensagemNegocios(string pMensagem)
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


                // Exemplos de mensagens:
                //<---              header             --->data    hora     comprad vend    preco        quantidade  max dia      min dia      vol          nneg    VvFech   E
                //0 2 4       12       21                  41      49       58      66      74           87          99           112          125          138      147     155           169                   191          204          217     225          238
                //NEBV20100702162034355PETR4               201007021618470000000013100000027000000026,800000000000200000000026,860000000026,5200209283409,0000010513 00001,3220000000029.900000000026.45C000000950000000026,79000000002800V000000270000000026,80000000000200
                //NEBV20100702162238788PETR4               201007021620510000000002700000131000000026,850000000000200000000026,870000000026,5200211588300,0000010621 00001,5120000000029.900000000026.45C000000270000000026,85000000000200V000000080000000026,86000000000200

                //try { this.HeaderTipoMensagem = pMensagem.Substring(0, 2); }

                string id = pMensagem.Substring(4, 17);
                string ativo = pMensagem.Substring(21, 20).Trim();
                string bolsa = pMensagem.Substring(2, 2);
                string dataHoraNegocio = pMensagem.Substring(41, 14);
                string compradora = pMensagem.Substring(58, 8);
                string vendedora = pMensagem.Substring(66, 8);
                string preco = pMensagem.Substring(74, 13);
                string quantidade = pMensagem.Substring(87, 12);
                string maximo = pMensagem.Substring(99, 13);
                string minimo = pMensagem.Substring(112, 13);
                string volume = pMensagem.Substring(125, 13);
                string totalNegocios = pMensagem.Substring(138, 8);
                string oscilacao = pMensagem.Substring(146, 9).Trim();
                string estado = pMensagem.Substring(155, 1);

                if (bolsa.Equals("BV"))
                {
                    // Ignora mensagens que não estiverem no estado "Papel em negociação", para BOVESPA
                    if (!estado.Equals("2"))
                        return;
                }
                else
                {
                    // Ignora mensagens que não estiverem no estado "Papel em negociação" e "Em leilão", para BMF
                    if (!estado.Equals("2") && !estado.Equals("1"))
                        return;
                }

                if (dataHoraNegocio.StartsWith("0000") == false)
                {
                    cotacao.Id = id;
                    cotacao.A = ativo;
                    cotacao.Bo = bolsa;
                    cotacao.Dt = DateTime.ParseExact(dataHoraNegocio, "yyyyMMddHHmmss", ciBR);
                    cotacao.Cp = Convert.ToInt16(compradora, ciBR);
                    cotacao.Vd = Convert.ToInt16(vendedora, ciBR);
                    cotacao.Pr = Convert.ToDouble(preco, ciBR);
                    cotacao.Qt = Convert.ToDouble(quantidade, ciBR);
                    cotacao.Mx = Convert.ToDouble(maximo, ciBR);
                    cotacao.Mi = Convert.ToDouble(minimo, ciBR);
                    cotacao.Vl = Convert.ToDouble(volume, ciBR);
                    cotacao.To = Convert.ToDouble(totalNegocios, ciBR);
                    cotacao.Os = Convert.ToDouble(oscilacao, ciBR);
                    cotacao.St = Convert.ToInt16(estado, ciBR);

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

        private void OnCotacao(object sender, MDSMessageEventArgs args)
        {
            ProcessarMensagemNegocios(args.Message);
        }

        public void Run()
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
                    GravarNegocio(cotacao);
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

        public void GravarNegocio(CotacaoANG negocio)
        {
            string dump = string.Format("Gravar: Ativo[{0}] DataCotacao[{1}] Id[{2}] Preco[{3}]",
                negocio.A, negocio.Dt, negocio.Id, negocio.Pr);
            logger.Debug(dump);

            _db.GravaNegocio(negocio, negocio.Id);
        }

        #region IServicoControlavel Members
        /// <summary>
        /// Invocado pelo framework ao iniciar o serviço
        /// </summary>
        public void IniciarServico()
        {
            logger.Info("*** Iniciando Capturador de Negocios ***");

            _bKeepRunning = true;

            _config = GerenciadorConfig.ReceberConfig<ProcessadorNegociosConfig>();

            _db = new ANGPersistenciaDB();
            _db.ConnectionString = _config.ConnectionString;
            _db.MDSConnectionString = _config.MDSConnectionString;

            _threadNegocios = new Thread(new ThreadStart(Run));
            _threadNegocios.Start();

            _srvstatus = ServicoStatus.EmExecucao;
        }

        /// <summary>
        /// Invocado pelo framework ao parar o servico
        /// </summary>
        public void PararServico()
        {
            logger.Info("*** Finalizando Capturador de Negocios ***");

            _bKeepRunning = false;

            while (_threadNegocios.IsAlive)
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
