using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using log4net;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Globalization;
using Gradual.OMS.CotacaoAdm.Lib;

namespace Gradual.OMS.Cotacao
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoCotacao : IServicoCotacao, IServicoControlavel, IServicoCotacaoAdm
    {
        #region Globais

        protected Timer gTimer;
        protected static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected CultureInfo ciBR = CultureInfo.CreateSpecificCulture("pt-BR");
        protected bool bKeepRunning = true;
        protected Thread thrMonitorConexao = null;
        MDSPackageSocket lSocket = null;
        //protected ComposicaoIndice composicaoIndice = null;
        protected IndiceGradual indiceGradual = null;
        protected IndiceGradualQuantidadeTeorica lIndiceGradualQuantidadeTeorica = null;
        protected double MaxDifHorarioBolsa;
        protected double TimeoutMDS;
        protected bool bEnviaAlertas = true;
        protected bool _filtraIndiceCheio = false;

        
        #endregion

        #region Propriedades

        protected ServicoStatus Status {set;get;}

        #endregion

        #region IServicoCotacao Members

        /// <summary>
        /// Metodo responsável por gerar o livro de ofertas (10 melhores ofertas para cada sentido)
        /// </summary>
        /// <param name="pInstrumento">Código do Instrumento</param>
        /// <returns>Melhores ofertas de compra e venda</returns>
        public string ReceberLivroOferta(string pInstrumento)
        {
            try
            {
                string strBook = string.Empty;

                if (pInstrumento.IndexOf(',') == -1)
                {
                    return MemoriaCotacao.ReceberBook(pInstrumento);
                }
                else
                {
                    string[] arrInstrumentos = pInstrumento.Split(',');
                    for (int i = 0; i <= arrInstrumentos.Length - 1; i++)
                    {
                        strBook += MemoriaCotacao.ReceberBook(arrInstrumentos[i]);

                        if (i != arrInstrumentos.Length - 1)
                        {
                            strBook += "|";
                        }
                    }

                }

                return strBook;
            }
            catch (Exception ex)
            {
                logger.Error("ReceberLivroOferta(): " + ex.Message, ex);
            }

            return String.Empty;
        }

        /// <summary>
        /// Metodo responsável por gerar o livro de negócios (10 posições por requisição)
        /// </summary>
        /// <param name="pInstrumento">Código do Instrumento (exemplo 'PETR4' ou 'PETR4,VALE5') </param>
        /// <returns>Livro de negocios</returns>
        public string ReceberLivroNegocios(string pInstrumento)
        {
            string strBussines = string.Empty;

            try
            {
                List<string> Cotacoes;

                if (MemoriaCotacao.hstCotacoes.TryGetValue(pInstrumento, out Cotacoes) )
                {
                    for (int i = 1; i < Cotacoes.Count; i++)
                        strBussines += Cotacoes[i].ToString() + "|";

                    if (strBussines.Length > 0)
                        strBussines.Remove(strBussines.Length - 1);
                }

                return strBussines;
            }
            catch (KeyNotFoundException knf)
            {
                logger.Error("ReceberLivroNegocios(KeyNotFoundException): " + knf.Message, knf);
                throw new KeyNotFoundException(string.Format("{0}{1}", "Ocorreu um erro ao procurar um item na coleção hstCotacoes. ", knf.Message));
            }
            catch (Exception ex)
            {
                logger.Error("ReceberLivroNegocios(): " + ex.Message, ex);
                throw new Exception(string.Format("{0}{1}", "Ocorreu um erro ao acessar o método ReceberLivroNegocios: ", ex.Message));
            }
        }

        /// <summary>
        /// Metodo responsável por retornar um ticker de cotação (Último negócio realizado ).
        /// </summary>
        /// <param name="pInstrumento">Código do Instrumento (exemplo 'PETR4' ou 'PETR4,VALE5' </param>
        /// <returns>Ticker com o último negócio realizado para o papel</returns>
        public string ReceberTickerCotacao(string pInstrumento)
        {
            string strBussines = string.Empty;

            try
            {
                if (pInstrumento.IndexOf(',') == -1)
                {
                    return _TickerCotacao(pInstrumento);
                }
                else
                {
                    string[] arrInstrumentos = pInstrumento.Split(',');
                    int instEncontrados = 0;

                    for (int i = 0; i < arrInstrumentos.Length; i++)
                    {
                        string instrumento = arrInstrumentos[i];

                        string abrefecha = _TickerCotacao(instrumento);
                        if (abrefecha != null && abrefecha.Length > 0)
                        {
                            if (instEncontrados > 0)
                                strBussines += "|";

                            strBussines += abrefecha;

                            instEncontrados++;
                        }
                    }
                }

                return strBussines;
            }
            catch (Exception ex)
            {
                logger.Error("ReceberTickerCotacao(" + pInstrumento +  "): " + ex.Message, ex);
                return String.Empty;
            }
        }

        /// <summary>
        /// Metodo responsável por retornar um ticker de cotação (Último negócio realizado ),
        /// obtendo as informações nos containers de sinal com atraso.
        /// </summary>
        /// <param name="pInstrumento">Código do Instrumento (exemplo 'PETR4' ou 'PETR4,VALE5' </param>
        /// <returns>Ticker com o último negócio realizado para o papel</returns>
        public string ReceberTickerCotacaoComAtraso(string pInstrumento)
        {
            string strBussines = string.Empty;

            try
            {
                if (pInstrumento.IndexOf(',') == -1)
                {
                    return _TickerCotacaoComAtraso(pInstrumento);
                }
                else
                {
                    string[] arrInstrumentos = pInstrumento.Split(',');
                    int instEncontrados = 0;

                    for (int i = 0; i < arrInstrumentos.Length; i++)
                    {
                        string instrumento = arrInstrumentos[i];

                        string abrefecha = _TickerCotacaoComAtraso(instrumento);
                        if (abrefecha != null && abrefecha.Length > 0)
                        {
                            if (instEncontrados > 0)
                                strBussines += "|";

                            strBussines += abrefecha;

                            instEncontrados++;
                        }
                    }
                }

                return strBussines;
            }
            catch (Exception ex)
            {
                logger.Error("ReceberTickerCotacaoComAtraso(" + pInstrumento + "): " + ex.Message, ex);
                return String.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        /// <returns></returns>
        private string _TickerCotacao( string instrumento )
        {
            string ret = "";
            List<string> Cotacoes = null;

            if (MemoriaCotacao.hstCotacoes.TryGetValue(instrumento, out Cotacoes) )
            {

                //int ItemsVetor = Cotacoes.Count - 1;

                string Cotacao = Cotacoes[0].ToString();

                string Compra = "C000000000000000000,00000000000000";
                string Venda = "V000000000000000000,00000000000000";
                string LivroOferta = Compra + Venda;

                string livroferta;
                if (MemoriaCotacao.hstLivroOferta.TryGetValue(instrumento, out livroferta))
                {
                    LivroOferta = livroferta.Remove(0, 41);
                }
                else
                {
                    logger.Info("Nao existe dados de LOF para [" + instrumento + "]");
                }

                int index = 0;
                if (LivroOferta.IndexOf("C") >= 0)
                {
                    Compra = LivroOferta.Substring(0, 34);
                }

                if (LivroOferta.IndexOf('V') >= 0)
                {
                    index = LivroOferta.IndexOf('V');
                    Venda = LivroOferta.Remove(0, index).Substring(0, 34);
                }

                string vAbertFech = "0000000000.000000000000.00";
                string cot;

                if (MemoriaCotacao.hstDadosPapel.TryGetValue(instrumento, out cot))
                {
                    if (cot != null && cot.Length > 0)
                        vAbertFech = cot;
                }
                else
                    logger.Error("Nao encontrou dados de abertura e fechamento do papel: [" + instrumento + "]");

                // X - Indicador de opcao
                // 00000.000000000000  - preco de exercicio
                // DDDDMMYY - data de exercicio
                string dadosopcao = "X000000.00000000000000000000";

                if (MemoriaCotacao.hstDadosOpcoes.ContainsKey(instrumento))
                {
                    MemoriaCotacao.hstDadosOpcoes.TryGetValue(instrumento, out dadosopcao);
                }

                ret = string.Format("{0}{1}{2}{3}{4}", Cotacao, vAbertFech, Compra, Venda, dadosopcao);
            }

            return ret;
        }

        /// <summary>
        /// Obtém informação de ticker de cotação a partir dos containers de sinal com atraso.
        /// </summary>
        /// <param name="instrumento">Nome do instrumento para o qual se solicita o ticker</param>
        /// <returns>string contendo o ticker com atraso</returns>
        private string _TickerCotacaoComAtraso(string instrumento)
        {
            string ret = "";
            List<string> Cotacoes = null;

            lock (MemoriaCotacaoDelay.GetInstance().hstCotacoes)
            {
                Cotacoes = (List<string>)(MemoriaCotacaoDelay.GetInstance().hstCotacoes[instrumento]);
            }

            if (Cotacoes != null)
            {

                int ItemsVetor = Cotacoes.Count - 1;

                string Cotacao = Cotacoes[ItemsVetor].ToString();

                string Compra = "C000000000000000000,00000000000000";
                string Venda = "V000000000000000000,00000000000000";
                string LivroOferta = Compra + Venda;

                string livroferta;
                if (MemoriaCotacao.hstLivroOferta.TryGetValue(instrumento, out livroferta))
                {
                    LivroOferta = livroferta.Remove(0, 41);
                }
                else
                {
                    logger.Info("Nao existe dados de LOF para [" + instrumento + "]");
                }

                int index = 0;
                if (LivroOferta.IndexOf("C") >= 0)
                {
                    Compra = LivroOferta.Substring(0, 34);
                }

                if (LivroOferta.IndexOf('V') >= 0)
                {
                    index = LivroOferta.IndexOf('V');
                    Venda = LivroOferta.Remove(0, index).Substring(0, 34);
                }

                string vAbertFech = "0000000000.000000000000.00";
                lock (MemoriaCotacaoDelay.GetInstance().hstDadosPapel)
                {
                    if (MemoriaCotacaoDelay.GetInstance().hstDadosPapel.ContainsKey(instrumento))
                    {
                        string cot = MemoriaCotacaoDelay.GetInstance().hstDadosPapel[instrumento].ToString();

                        if (cot != null && cot.Length > 0)
                            vAbertFech = cot;
                    }
                    else
                        logger.Error("Nao encontrou dados de abertura e fechamento do papel: [" + instrumento + "]");
                }

                // X - Indicador de opcao
                // 00000.000000000000  - preco de exercicio
                // DDDDMMYY - data de exercicio
                string dadosopcao = "X000000.00000000000000000000";

                if (MemoriaCotacao.hstDadosOpcoes.ContainsKey(instrumento))
                {
                    MemoriaCotacao.hstDadosOpcoes.TryGetValue(instrumento, out dadosopcao);
                }

                ret = string.Format("{0}{1}{2}{3}{4}", Cotacao, vAbertFech, Compra, Venda, dadosopcao);
            }

            return ret;
        }

        /// <summary>
        /// Metodo responsável por retornar um ticker resumido de cotação
        /// </summary>
        /// <param name="pInstrumento">Código do Instrumento  </param>
        /// <returns>Ticker resumido com o último negócio realizado para o papel</returns>
        public string ReceberTickerResumido(string pInstrumento)
        {
            string strBussiness = string.Empty;

            try
            {
                List<string> Cotacoes;

                if (MemoriaCotacao.hstCotacoes.TryGetValue(pInstrumento, out Cotacoes))
                {
                    int ItemsVetor = Cotacoes.Count - 1;
                    string Cotacao = Cotacoes[ItemsVetor].ToString();
                    Cotacao = Cotacao.Remove(0, 74);

                    string preco = Cotacao.Substring(0, 13);
                    string variacao = Cotacao.Substring(73, 8);

                    strBussiness = string.Format("{0}{1}", preco, variacao);
                }

                return strBussiness;
            }
            catch (KeyNotFoundException knf)
            {
                logger.Error("ReceberTickerResumido(KeyNotFoundException): " + knf.Message, knf);

                throw new KeyNotFoundException(string.Format("{0}{1}", "Ocorreu um erro ao procurar um item na coleção hstCotacoes. ", knf.Message));
            }
            catch (Exception ex)
            {
                logger.Error("ReceberTickerResumido(): " + ex.Message, ex);

                throw new Exception(string.Format("{0}{1}", "Ocorreu um erro ao acessar o método ReceberTickerResumido: ", ex.Message));
            }
        }

        /// <summary>
        /// Metodo responsável por retornar os destaques do Ibovespa
        /// </summary>        
        /// <returns>Ticker com os destaques do indice bovespa</returns>
        public string ReceberTickerDestaques()
        {
            try
            {
                return MemoriaCotacao.ReceberDestaques();
            }
            catch (Exception ex)
            {
                logger.Error("ReceberTickerDestaques(): " + ex.Message, ex);
                throw new Exception(string.Format("{0}{1}", "Ocorreu um erro ao acessar o método ReceberTickerDestaques: ", ex.Message));
            }
        }

        /// <summary>
        /// Metodo responsável por retornar o rank de corretoras de um determinado Instrumento
        /// </summary>
        /// <param name="pInstrumento">Código do Instrumento  </param>
        /// <returns>rank de corretora de um determinado Instrumento</returns>
        public string ReceberRankCorretora(string pInstrumento)
        {
            try
            {
                return MemoriaCotacao.ReceberRankCorretora(pInstrumento);
            }
            catch (Exception ex)
            {
                logger.Error("ReceberRankCorretora(" + pInstrumento + "): " + ex.Message, ex);
                throw new Exception(string.Format("{0}{1}", "Ocorreu um erro ao acessar o método ReceberTickerResumido: ", ex.Message));
            }
            
        }

        /// <summary>
        /// Metodo responsável por gerar o livro de ofertas agregado (10 melhores precos e qtdes para cada sentido)
        /// </summary>
        /// <param name="pInstrumento">Código do Instrumento</param>
        /// <returns>Melhores precos e qtdes de ofertas de compra e venda</returns>
        public string ReceberLivroAgregado(string pInstrumento)
        {
            try
            {
                StringBuilder strAgregado = new StringBuilder();

                if (pInstrumento.IndexOf(',') == -1)
                {
                    return MemoriaCotacao.ReceberAgregado(pInstrumento);
                }
                else
                {
                    string[] arrInstrumentos = pInstrumento.Split(',');
                    for (int i = 0; i <= arrInstrumentos.Length - 1; i++)
                    {
                        strAgregado.Append(MemoriaCotacao.ReceberAgregado(arrInstrumentos[i]));

                        if (i != arrInstrumentos.Length - 1)
                        {
                            strAgregado.Append("|");
                        }
                    }

                }

                return strAgregado.ToString();
            }
            catch (Exception ex)
            {
                logger.Error("ReceberLivroAgregado(): " + ex.Message, ex);
            }

            return String.Empty;
        }

        #endregion
    

        #region IServicoControlavel Members

        public virtual void IniciarServico()
        {
            //composicaoIndice = new ComposicaoIndice();
            indiceGradual = new IndiceGradual();
            lIndiceGradualQuantidadeTeorica = new IndiceGradualQuantidadeTeorica();

            // Verifica se deve ser ativado o tratamento de delay da cotação
            MemoriaCotacaoDelay.GetInstance().DelayTickerOn = false;
            if (ConfigurationManager.AppSettings["DelayTicker"] != null &&
                ConfigurationManager.AppSettings["DelayTicker"].ToString().ToUpper().Equals("TRUE"))
            {
                MemoriaCotacaoDelay.GetInstance().DelayTickerOn = true;

                // Obtem o valor do Delay do sinal de cotação, em minutos
                MemoriaCotacaoDelay.GetInstance().DelayTickerAmount = 15 * 60 * 1000;
                if (ConfigurationManager.AppSettings["DelayTickerAmount"] != null)
                {
                    int delayAmount = Convert.ToInt32(ConfigurationManager.AppSettings["DelayTickerAmount"].ToString());
                    MemoriaCotacaoDelay.GetInstance().DelayTickerAmount = delayAmount * 60 * 1000;
                }

                MemoriaCotacaoDelay.GetInstance().StartProcessamento();
            }

            if (ConfigurationManager.AppSettings["FiltraIndiceCheio"] != null &&
                ConfigurationManager.AppSettings["FiltraIndiceCheio"].ToString().ToUpper().Equals("TRUE"))
            {
                _filtraIndiceCheio = true;
            }

            QueueManager.Instance.Start();
            //QueueManager.Instance.ComposicaoIndice = composicaoIndice;
            QueueManager.Instance.IndiceGradual = indiceGradual;

            logger.Info("MDS modelo tradicional");
            lSocket = new MDSPackageSocket();
            lSocket.FiltraIndiceCheio = _filtraIndiceCheio;
            lSocket.IpAddr = ConfigurationManager.AppSettings["ASConnMDSIp"].ToString();
            lSocket.Port = ConfigurationManager.AppSettings["ASConnMDSPort"].ToString();
            //lSocket.setComposicaoIndice(composicaoIndice);
            lSocket.setIndiceGradual(indiceGradual);

            lSocket.OpenConnection();

            _sendMDSLoginMSG(lSocket);

            thrMonitorConexao = new Thread(new ThreadStart(MonitorConexaoMDS));
            thrMonitorConexao.Start();

            gTimer = new Timer(new TimerCallback(IniciarThread), null, 0, 5000);


            // Obtem o parametro de maxima diferenca de horario da ultima mensagem com a bolsa
            // para envio de alertas
            MaxDifHorarioBolsa = 75;
            if ( ConfigurationManager.AppSettings["MaxDifHorarioBolsa"] != null )
            {
                MaxDifHorarioBolsa = Convert.ToDouble(ConfigurationManager.AppSettings["MaxDifHorarioBolsa"].ToString());
            }
            MaxDifHorarioBolsa *= 1000;

            // Obtem o timeout de mensagem com o MDS, em segundos
            TimeoutMDS = 300;
            if (ConfigurationManager.AppSettings["TimeoutMDS"] != null)
            {
                TimeoutMDS = Convert.ToDouble(ConfigurationManager.AppSettings["TimeoutMDS"].ToString());
            }
            TimeoutMDS *= 1000;

            logger.Info("Servico cotacao iniciado");

            this.Status = ServicoStatus.EmExecucao;
        }

        public virtual void  PararServico()
        {
            logger.Info("Finalizando servico");

            bKeepRunning = false;

            MemoriaCotacaoDelay.GetInstance().StopProcessamento();

            if (thrMonitorConexao != null)
            {
                while (thrMonitorConexao.IsAlive)
                {
                    logger.Info("Aguardando finalizacao do monitor de conexao com MDS");
                    Thread.Sleep(250);
                }
            }

            QueueManager.Instance.Stop();

            logger.Info("Servico cotacao finalizado");

            this.Status = ServicoStatus.Parado;
        }

        public virtual ServicoStatus ReceberStatusServico()
        {
            return this.Status;
        }

        #endregion //IServicoControlavel Members

        #region Metodos de apoio


        protected void IniciarThread(object state)
        {
            this.ObterDadosPapel();
        }

        protected void ObterDadosPapel()
        {
            DCotacoes _DCotacoes = new DCotacoes();

            Dictionary<string, string> papeisCadastro = _DCotacoes.ObterHashDadosCotacao();

            foreach( KeyValuePair<string, string> item in papeisCadastro )
            {
                MemoriaCotacao.hstDadosPapel.AddOrUpdate(item.Key, item.Value, (key, oldvalue) => item.Value);
            }

            //Hashtable listaIndices = composicaoIndice.GetListaIndices();

            foreach ( string instrumento in papeisCadastro.Keys )
            {
                if ( !MemoriaCotacao.hstCotacoes.ContainsKey(instrumento) )
                {
                    string fakemsg = "";

                    /*
                    if (listaIndices.Contains(instrumento))
                    {
                        logger.Info("Inicializando dados de cotacao para indice [" + instrumento + "]");

                        ComposicaoIndice.ItemIndice indice = (ComposicaoIndice.ItemIndice)listaIndices[instrumento];

                        fakemsg = "NEBV";                               // Tipo msg + bolsa
                        fakemsg += "00000000000000000";                 // Dthr
                        fakemsg += instrumento.PadRight(20);            // Instrumento
                        fakemsg += indice.dataCotacao.ToString("yyyyMMdd");
                                                                        // Data
                        fakemsg += indice.dataCotacao.ToString("HHmmss") + "000"; 
                                                                        // Hora
                        fakemsg += "0".PadLeft(8, '0');                 // Corretora compradora
                        fakemsg += "0".PadLeft(8, '0');                 // Corretora vendedora
                        fakemsg += indice.valor.ToString("0.00", ciBR).PadLeft(13, '0'); 
                                                                        // Preco
                        fakemsg += "0".PadLeft(12, '0');                // Quantidade
                        fakemsg += "0".PadLeft(9, '0') + ",000";        // Max 
                        fakemsg += "0".PadLeft(9, '0') + ",000";        // Min
                        fakemsg += "0".PadLeft(9, '0') + ",000";        // Volume
                        fakemsg += "1".PadLeft(8, '0');                 // Num Neg
                        fakemsg += (indice.oscilacao < 0) ? "-" : " ";  // Indic. Variacao
                        fakemsg += Math.Abs(indice.oscilacao).ToString("0.00", ciBR).PadLeft(8, '0');  
                                                                        // Variacao
                        fakemsg += "2";                                 // "2" - negociado
                        fakemsg += "0".PadLeft(12, '0');                // Qtde compra
                        fakemsg += "0".PadLeft(12, '0');                // Qtde venda
                    }
                    else
                    {
                    */
                        logger.Info("Inicializando dados de cotacao para [" + instrumento + "]");

                        fakemsg = "NEB-";                       // Tipo msg + bolsa
                        fakemsg += "00000000000000000";         // Dthr
                        fakemsg += instrumento.PadRight(20);    // Instrumento
                        fakemsg += "0".PadLeft(8,'0');          // Data
                        fakemsg += "0".PadLeft(9,'0');          // Hora
                        fakemsg += "0".PadLeft(8,'0');          // Corretora compradora
                        fakemsg += "0".PadLeft(8,'0');          // Corretora vendedora
                        fakemsg += "0".PadLeft(9,'0') + ",000"; // Preco
                        fakemsg += "0".PadLeft(12,'0');
                        fakemsg += "0".PadLeft(9,'0') + ",000"; // Max 
                        fakemsg += "0".PadLeft(9,'0') + ",000"; // Min
                        fakemsg += "0".PadLeft(9,'0') + ",000"; // Volume
                        fakemsg += "0".PadLeft(8,'0');          // Num Neg
                        fakemsg += " ";                         // Indic. Variacao
                        fakemsg += "0".PadLeft(5,'0') + ",00";  // Variacao
                        fakemsg += "0";                         // "0" - nao negociado
                        fakemsg += "0".PadLeft(12, '0');        // Qtde compra
                        fakemsg += "0".PadLeft(12, '0');        // Qtde venda
                    //}

                    List<string> valores = MemoriaCotacao.hstCotacoes.GetOrAdd(instrumento, new List<string>());
                    valores.Add(fakemsg);
                }
            }

            Dictionary<string, string> dadosOpcoes = _DCotacoes.ObterHashDadosOpcoes();
            foreach( KeyValuePair<string, string> item in dadosOpcoes)
            {
                MemoriaCotacao.hstDadosOpcoes.AddOrUpdate(item.Key, item.Value, (key, oldvalue) => item.Value);
            }
        }

        protected virtual void MonitorConexaoMDS()
        {
            int i = 0;
            int iTrialInterval = 600;

            logger.Info("Iniciando thread de monitoracao de conexao com MDS");
            while (bKeepRunning)
            {
                try
                {
                    // Reconecta a cada 5 min
                    if (!lSocket.IsConectado())
                    {
                        if (i > iTrialInterval)
                        {
                            _enviaAlertaDesconexao(lSocket.IpAddr, lSocket.Port);

                            logger.Info("Reabrindo conexao com MDS...");
                            lSocket.OpenConnection();

                            _sendMDSLoginMSG(lSocket);

                            i = 0;
                        }
                        else
                        {
                            i++;
                            // Configura intervalos de 1 minuto durante o dia ou 
                            // 5 minutos 
                            if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 21)
                                iTrialInterval = 600;
                            else
                                iTrialInterval = 3000;

                        }
                    }
                    else
                    {
                        if (i > 600)
                        {
                            logger.Info("Conexao com MDS ativa " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " LastPkt: " + lSocket.LastPacket.ToString("dd/MM/yyyy HH:mm:ss.ffff") + " [" + lSocket.LastMsg + "]");
                            logger.Info("*** Ultimos pacotes: ");
                            logger.Info("Neg =[" + MemoriaCotacao.LastNegocioPacket.ToString("dd/MM/yyyy HH:mm:ss.ffff") + "] [" + MemoriaCotacao.LastNegocioMsg + "]");
                            logger.Info("Lof =[" + MemoriaCotacao.LastLofPacket.ToString("dd/MM/yyyy HH:mm:ss.ffff") + "] [" + MemoriaCotacao.LastLofMsg + "]");
                            logger.Info("Dest=[" + MemoriaCotacao.LastDestaquePacket.ToString("dd/MM/yyyy HH:mm:ss.ffff") + "] [" + MemoriaCotacao.LastDestaqueMsg + "]");
                            logger.Info("Rank=[" + MemoriaCotacao.LastRankingPacket.ToString("dd/MM/yyyy HH:mm:ss.ffff") + "] [" + MemoriaCotacao.LastRankingMsg + "]");
                            logger.Info("***");

                            // Verifica dessincronizacao de sinal 
                            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday &&
                                DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                            {
                                if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour < 18)
                                {
                                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - MemoriaCotacao.HorarioUltimaSonda.Ticks);
                                    if (ts.TotalMilliseconds > MaxDifHorarioBolsa)
                                    {
                                        _enviaAlertaAtraso(lSocket.IpAddr, lSocket.Port);
                                    }
                                }
                            }

                            // Verifica ultima comunicacao com MDS
                            TimeSpan tslastpkt = new TimeSpan(DateTime.Now.Ticks - lSocket.LastPacket.Ticks);
                            if (tslastpkt.TotalMilliseconds > TimeoutMDS)
                            {
                                logger.Warn("Finalizando conexao com MDS por timeout!!!");
                                lSocket.CloseConnection();
                            }

                            i = 0;
                        }
                        else
                            i++;
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    logger.Fatal("MonitorCotacaoMDS(): " + ex.Message, ex);
                    Thread.Sleep(1000);
                }
            }

            logger.Info("Thread de monitoracao de conexao com MDS finalizacao");
        }

        protected void _sendMDSLoginMSG(MDSPackageSocket mdsSocket)
        {
            string msg = "QLPP";

            try
            {

                if (ConfigurationManager.AppSettings["EfetuarLogonMDS"] == null)
                {
                    logger.Warn("Chave 'EfetuarLogonMDS' nao declarada no appsettings. Nao efetua login");
                    return;
                }

                if (!ConfigurationManager.AppSettings["EfetuarLogonMDS"].ToString().Equals("true"))
                {
                    logger.Warn("Nao efetua login no MDS, EfetuarLogonMDS=false.");
                    return;
                }

                msg += DateTime.Now.ToString("yyyyMMddHHmmssfff");
                msg += System.Environment.MachineName.PadRight(20);

                logger.Info("Efetuando login no MDS [" + msg + "]");

                if (mdsSocket != null && mdsSocket.IsConectado())
                {
                    mdsSocket.SendData(msg, true);
                }

                logger.Info("Mensagem de login enviada ao MDS");
            }
            catch (Exception ex)
            {
                logger.Info("_sendMDSLoginMSG():" + ex.Message, ex);
            }
        }


        /// <summary>
        /// Envia mensagem de alerta
        /// </summary>
        protected void _enviaAlertaAtraso(string server, string porta)
        {
            try
            {
                string body = "";

                string subject = System.Environment.MachineName + " Alerta: Atraso de sinal";
                body += System.Environment.MachineName + " Alerta: ";
                body += "\r\n";
                body += "Horario do server: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                body += "\r\n";
                body += "MDS: " + server + ":" + porta;
                body += "\r\n";
                body += "Ultimo sinal recebido: " + MemoriaCotacao.HorarioUltimaSonda.ToString("yyyy/MM/dd HH:mm:ss");

                if (_enviarEmail(subject, body))
                {
                    logger.InfoFormat("Email de alerta de atraso enviado com sucesso");
                }
            }
            catch (Exception ex)
            {
                logger.Error("_enviaAlertaAtraso(): " + ex.Message, ex);
            }

        }
        
        /// <summary>
        /// Envia mensagem de alerta
        /// </summary>
        protected void _enviaAlertaDesconexao(string server, string porta)
        {
            try
            {
                string body = "";

                string subject = System.Environment.MachineName + " Alerta: Desconectado do MDS";
                body += System.Environment.MachineName + " Alerta: ";
                body += "\r\n";
                body += "MDS: " + server + ":" + porta;
                body += "\r\n";
                body += "Desconectado do MDS: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                _enviarEmail(subject, body);
            }
            catch (Exception ex)
            {
                logger.Error("_enviaAlertaDesconexao(): " + ex.Message, ex);
            }

        }

        /// <summary>
        /// Envia mensagem de alerta
        /// </summary>
        private bool _enviarEmail(string subject, string body)
        {
            try
            {
                string[] destinatarios;

                if (bEnviaAlertas == false)
                {
                    logger.Error("Envio de alertas desabilitado!!!");
                    return false;
                }

                if (ConfigurationManager.AppSettings["EmailAlertaDestinatarios"] == null)
                {
                    logger.Error("Settings 'EmailAlertaDestinatarios' nao definido. Nao eh possivel enviar alerta");
                    return false;
                }

                char[] seps = { ';' };
                destinatarios = ConfigurationManager.AppSettings["EmailAlertaDestinatarios"].ToString().Split(seps);

                var lMensagem = new MailMessage("Gradual.OMS.Cotacao@gradualinvestimentos.com.br", destinatarios[0]);

                for (int i = 1; i < destinatarios.Length; i++)
                {
                    lMensagem.To.Add(destinatarios[i]);
                }


                lMensagem.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["EmailAlertaReplyTo"].ToString()));
                lMensagem.Body = body;
                lMensagem.Subject = subject;

                new SmtpClient(ConfigurationManager.AppSettings["EmailAlertaHost"].ToString()).Send(lMensagem);

                logger.InfoFormat("Email enviado com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("_enviarEmail(): " + ex.Message, ex);
                return false;
            }

            return true;
        }


        #endregion //Metodos de apoio

        #region IServicoCotacao Members

        public Hashtable ReceberDadosAberturaFechamento()
        {
            Hashtable hstCotacoes = new Hashtable();

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["OMS"]);

            try
            {
                conn.Open();

                SqlCommand Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = conn;
                Command.CommandText = "prc_TB_ATIVO_sel";

                DataTable dtPapel = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter(Command);

                adapter.Fill(dtPapel);

                if (dtPapel.Rows.Count > 0)
                {

                    for (int i = 0; i <= dtPapel.Rows.Count - 1; i++)
                    {
                        string Instrumento = dtPapel.Rows[i]["cd_negociacao"].ToString();
                        string Mensagem = dtPapel.Rows[i]["Mensagem"].ToString();

                        lock (hstCotacoes)
                        {
                            hstCotacoes.Remove(Instrumento);
                            hstCotacoes.Add(Instrumento, Mensagem);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.Error(string.Format("Ocorreu um erro ao acessar o banco de dados: ", ex.Message));
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return hstCotacoes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pInstrumento"></param>
        /// <returns></returns>
        public string ReceberAberturaFechamentoInstrumento(string pInstrumento)
        {
            //                      1         2
            //            01234567890123456789012345
            string ret = "0000000000.000000000000.00";

            try
            {
                string mensagem;
                if (MemoriaCotacao.hstDadosPapel.TryGetValue(pInstrumento, out mensagem))
                {
                    if (mensagem != null && mensagem.Length > 0)
                        ret = mensagem;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao obter dados de abertura e fechamento para [" + pInstrumento + "]:" + ex.Message, ex);
            }

            return ret;
        }

        #endregion //IServicoCotacao Members

        #region IServicoCotacaoAdm Members
        public void ConectarMDS()
        {
            logger.Info("Reabrindo conexao com MDS por solicitacao via ADM...");

            lSocket.OpenConnection();

            _sendMDSLoginMSG(lSocket);
        }

        public void DesconectarMDS()
        {
            logger.Warn("Finalizando conexao com MDS por solicitacao via ADM!");

            lSocket.CloseConnection();
        }

        public void TrocarServidorMDS(string host)
        {
            logger.Warn("Trocando servidor para [" + host + "] via ADM");

            // Remove a nova senha, no arquivo de configuracao inclusive
            Configuration stmconfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            stmconfig.AppSettings.Settings.Remove("ASConnMDSIp");
            stmconfig.AppSettings.Settings.Add("ASConnMDSIp", host);

            stmconfig.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }


        public void LigarEnvioAlertas()
        {
            bEnviaAlertas = true;
        }

        public void DesligarEnvioAlertas()
        {
            bEnviaAlertas = false;
        }

        public string ObterServidorMDS()
        {
            string ret = ConfigurationManager.AppSettings["ASConnMDSIp"].ToString();

            return ret;
        }

        public bool IsConectado()
        {
            return lSocket.IsConectado();
        }
        
        public DateTime LastPacket()
        {
            return lSocket.LastPacket;
        }

        public DateTime LastNegocioPacket()
        {
            return lSocket.LastNegocioPacket;
        }

        public DateTime LastLofPacket()
        {
            return lSocket.LastLofPacket;
        }

        public DateTime LastDestaquePacket()
        {
            return lSocket.LastDestaquePacket;
        }

        public DateTime LastRankingPacket()
        {
            return lSocket.LastLofPacket;
        }

        public DateTime LastSonda()
        {
            return MemoriaCotacao.HorarioUltimaSonda;
        }

        public DateTime LastAgregadoPacket()
        {
            return lSocket.LastAgregadoPacket;
        }

        #endregion //IServicoCotacaoAdm Members
    }
}
