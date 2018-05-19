using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gradual.OMS.AnaliseGrafica.Lib;
using System.Globalization;
using log4net;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using Gradual.OMS.Library;

namespace Gradual.OMS.GravadorNegociosLog
{
    public class ProcessadorCotacao : IServicoControlavel
    {
        private ServicoStatus _srvstatus = ServicoStatus.Parado;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CultureInfo ciBR = CultureInfo.CreateSpecificCulture("pt-BR");
        private string _diretorioLogs = null;
        private string _nomeLogPadrao = null;
        
        ANGPersistenciaDB _db = null;
        ProcessadorCotacaoConfig _config = null;
        Thread _threadCotacao = null;

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
                string status = pMensagem.Substring(155, 1);

                if (dataHoraNegocio.StartsWith("0000") == false)
                {
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
                    cotacao.St = Convert.ToInt16(status, ciBR);

                    GravarNegocio(cotacao, id);
                }
            }
            catch (Exception ex)
            {
                logger.Error("ProcessarMensagem: " + ex.Message, ex);
            }
        }

        public void Run()
        {
            DateTime dataAtual = DateTime.Now;
            dataAtual = dataAtual.AddDays(-1);

            string arquivoLeitura = _diretorioLogs + "\\" + _nomeLogPadrao +
                dataAtual.Year.ToString("0000") +
                dataAtual.Month.ToString("00") +
                dataAtual.Day.ToString("00");

            try
            {
                int cont = 0;
                using (StreamReader stream = new StreamReader(arquivoLeitura))
                {
                    logger.Info(String.Format("Iniciado leitura do log [{0}]", arquivoLeitura));
                    String linha;
                    while ((linha = stream.ReadLine()) != null)
                    {
                        if (linha.Contains("negocio  [NE]"))
                        {
                            cont++;
                            int posicaoInicial = linha.LastIndexOf('[') + 1;
                            int tamanho = linha.LastIndexOf(']') - posicaoInicial;
                            string mensagem = linha.Substring(posicaoInicial, tamanho);

                            ProcessarMensagemCotacao(mensagem);
                        }
                    }
                }
                logger.Info(String.Format("Leitura finalizada! [{0}] mensagens processadas", cont));
            }
            catch (Exception ex)
            {
                logger.Error("Falha: " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cotacao"></param>
        public void GravarNegocio(CotacaoANG negocio, string id)
        {
            string dump = string.Format("Gravar: Ativo[{0}] DataCotacao[{1}] Id[{2}] Preco[{3}]",
                negocio.A, negocio.Dt, id, negocio.Pr);
            logger.Debug(dump);

            _db.GravaNegocio(negocio, id);
        }

        #region IServicoControlavel Members
        /// <summary>
        /// Invocado pelo framework ao iniciar o serviço
        /// </summary>
        public void IniciarServico()
        {
            logger.Info("*** Iniciando Gravador de Negocios ***");

            _config = GerenciadorConfig.ReceberConfig<ProcessadorCotacaoConfig>();
            _diretorioLogs = _config.DiretorioLogs;
            _nomeLogPadrao = _config.NomeLogPadrao;

            _db = new ANGPersistenciaDB();

            _threadCotacao = new Thread(new ThreadStart(Run));
            _threadCotacao.Start();

            _srvstatus = ServicoStatus.EmExecucao;
        }

        /// <summary>
        /// Invocado pelo framework ao parar o servico
        /// </summary>

        public void PararServico()
        {
            logger.Info("*** Finalizando Gravador de Negocios ***");

            while (_threadCotacao.IsAlive)
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
