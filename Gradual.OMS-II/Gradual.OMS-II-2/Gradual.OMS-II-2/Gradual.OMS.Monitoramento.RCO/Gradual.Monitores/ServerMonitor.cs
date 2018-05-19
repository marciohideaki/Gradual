using Gradual.OMS.Library.Servicos;
using System.Threading;
using Gradual.Monitores.Risco.Lib;
using Gradual.Monitores.Persistencia;
using System.Collections;
using Gradual.Monitores.Risco.Info;
using System.ServiceModel;
using System.Configuration;
using Gradual.Monitores.Risco.Enum;
using System.Collections.Concurrent;
using System.Globalization;
using System;
using System.Collections.Generic;
using log4net;
using System.Linq;
namespace Gradual.Monitores.Risco
{
    /// <summary>
    /// Classe de Monitoração de Lucro/Prejuizo por cliente
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServerMonitor : IServicoMonitorRisco,IServicoControlavel
    {
        #region Atributos
        /// <summary>
        /// Objeto de log para ser usado na classe
        /// </summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Objeto para fazer o Formater da moeda americana
        /// </summary>
        private CultureInfo gCultura = new CultureInfo("en-US");
        
        /// <summary>
        /// Objeto com a Lista de cliente que são monitorados para calcular o risco/Prejuizo um por um.
        /// Têm a finalidade de guardar os clientes que irão ser calculados em cada rodada.
        /// </summary>
        private List<int> ClientesMonitor          = new List<int>();

        /// <summary>
        /// Lista de Operações de clientes com alertas de negociações
        /// </summary>
        private List<PLDOperacaoInfo> ClientesPLD  = new List<PLDOperacaoInfo>();

        /// <summary>
        /// Thread Principal de clientes que têm a finalidade de rodar o calculo das posições  para 
        /// definir o lucro prejuizo dos clientes
        /// </summary>
        private Thread thThreadClientes;

        /// <summary>
        /// Dictionary que guarda o Codigo Bovespa Principal do cliente e seu Mutex.
        /// IMPORTANTE - Têm a Finalidade de controle de como vai ser calculado, como intervalo de recalculo, status de processamento
        /// </summary>
        private ConcurrentDictionary<int, ClienteMutexInfo> htClientes = new ConcurrentDictionary<int, ClienteMutexInfo>();

        /// <summary>
        /// HashTable de que guarda na memória o Código de negócio e seu tipo de mercado, VIS, LEI, OPC, EOC, etc....
        /// </summary>
        private Hashtable MercadoInstumento       = new Hashtable();

        /// <summary>
        /// Lista de cotações de papeis no fechamento
        /// </summary>
        private Hashtable CotacaoFechamento       = new Hashtable();
        
        /// <summary>
        /// Lista de cotações de papeis no ajuste BMF
        /// </summary>
        private Hashtable CotacaoAjuste = new Hashtable();

        /// <summary>
        /// Lista de cotações de papeis na abertura
        /// </summary>
        private Hashtable CotacaoAbertura         = new Hashtable();

        /// <summary>
        /// Dictionary principal de Lucro Prejuízo com o código principal do cliente e o objeto de exposição para cada cliente.
        /// (IMPORTANTE)Têm a finalidade de armazenar e expor a posição (Lucro - Prejuízo ) do cliente
        /// </summary>
        private ConcurrentDictionary<int, ExposicaoClienteInfo> MonitorLucroPrejuizo = new ConcurrentDictionary<int, ExposicaoClienteInfo>();

        /// <summary>
        /// Hashtable com a lista de vencimentos de DI com Instrumento e data, especificamente.
        /// Têm a finalidade de armazenar a Listagem de instrumentos para consulta no calculo de Taxa de DI na função CalcularTaxaDI
        /// </summary>
        private Hashtable htVencimentoDI          = new Hashtable();

        /// <summary>
        /// Lista de datas de feriados usadas para calcular dias uteis na função ObterDiasUteis
        /// </summary>
        private List<DateTime> lsFeriadosDI       = new List<DateTime>();

        /// <summary>
        /// Flag que sinaliza que o serviço está rodando e está ativo.
        /// </summary>
        public bool _bKeepRunning = false;

        /// <summary>
        /// Status do serviço
        /// </summary>
        private ServicoStatus _ServicoStatus { set; get; }
        #endregion


        /// <summary>
        /// Lista de constantes que guardam os valores de BMF para o calculo identificar qual é o Fator Multiplicador de 
        /// cada conjunto de instrumento de BMF
        /// </summary>
        #region Constantes utilizadas nos monitores de risco

        private const string DI            = "DI1";
        private const string DOLAR         = "DOL";
        private const string INDICE        = "IND";
        private const string MINIBOLSA     = "WIN";
        private const string MINIDOLAR     = "WDO";
        private const string MINIDOLARFUT  = "WDO"; 
        private const string CHEIOBOI      = "BGI";
        private const string MINIBOI       = "WBG";
        private const string EURO          = "EUR";
        private const string MINIEURO      = "WEU";
        private const string CAFE          = "ICF";
        private const string MINICAFE      = "WCF";
        private const string FUTUROACUCAR  = "ISU";
        private const string ETANOL        = "ETH";
        private const string ETANOLFISICO  = "ETN";
        private const string MILHO         = "CCM";
        private const string SOJA          = "SFI";
        private const string OURO          = "OZ1";
        private const string ROLAGEMDOLAR  = "DR1";
        private const string ROLAGEMINDICE = "IR1";
        private const string ROLAGEMBOI    = "BR1";
        private const string ROLAGEMCAFE   = "CR1";
        private const string ROLAGEMMILHO  = "MR1";
        private const string ROLAGEMSOJA   = "SR1";

        /// <summary>
        /// Codigo fixo da carteira de BTC --->>> 22012
        /// </summary>
        private const int    CodigoCarteiraBTC = 22012;

        private const string AVISTA        = "VIS";
        private const string OPCAOCOMPRA   = "OPV";
        private const string OPCAOVENDA    = "OPV";
        private const string BMF           = "BMF";

        #endregion

        /// <summary>
        /// Lista de Condições do Monitor de PLD
        /// </summary>
        #region Constantes utilizadas nos monitores de PLD

        private const string PLDACEITO    = "ACEITO";        
        private const string PLDREJEITADO = "REJEITADO";
        private const string PLDANALISE   = "ANALISE";


        #endregion

        #region Propriedades e Configurações
        /// <summary>
        /// Retorna o fator multiplicador do Euro para calculo de posição bmf (EUR)
        /// </summary>
        private string FATORCALCULOEURO 
        {
            get
            {
                return ConfigurationManager.AppSettings["EUR"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do Mini do Euro para calculo de posição bmf (WEU)
        /// </summary>
        private string FATORCALCULOMINIEURO 
        { 
            get 
            {
                return ConfigurationManager.AppSettings["WEU"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do CAFÉ para calculo de posição bmf (ICF)
        /// </summary>
        private string FATORCALCULOCAFE 
        { 
            get 
            {
                return ConfigurationManager.AppSettings["ICF"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do MINI do CAFÉ  para calculo de posição bmf (WCF)
        /// </summary>
        private string FATORCALCULOMINICAFE 
        { 
            get 
            { 
                return ConfigurationManager.AppSettings["WCF"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do ACUCAR FUTURO para calculo de posição bmf (SIU)
        /// </summary>
        private string FATORCALCULOFUTUROACUCAR 
        { 
            get 
            {
                return ConfigurationManager.AppSettings["ISU"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do ETANOL para calculo de posição bmf (ETH)
        /// </summary>
        private string FATORCALCULOETANOL 
        { 
            get 
            {
                return ConfigurationManager.AppSettings["ETH"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do ETANOL FISICO para calculo de posição bmf (ETN)
        /// </summary>
        private string FATORCALCULOETANOLFISICO 
        { 
            get 
            {
                return ConfigurationManager.AppSettings["ETN"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do MILHO para calculo de posição bmf (CCM)
        /// </summary>
        private string FATORCALCULOMILHO 
        { 
            get 
            {
                return ConfigurationManager.AppSettings["CCM"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do SOJA para calculo de posição bmf (SFI)
        /// </summary>
        private string FATORCALCULOSOJA 
        { 
            get 
            {
                return ConfigurationManager.AppSettings["SFI"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do OURO para calculo de posição bmf (OZ1)
        /// </summary>
        private string FATORCALCULOOURO 
        { 
            get 
            {
                return ConfigurationManager.AppSettings["OZ1"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do ROLAGEM DE DOLAR para calculo de posição bmf (DR)
        /// </summary>
        private string FATORCALCULOROLAGEMDOLAR 
        { 
            get 
            {
                return ConfigurationManager.AppSettings["DR1"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do ROLAGEM DE INDICE para calculo de posição bmf (IR1)
        /// </summary>
        private string FATORCALCULOROLAGEMINDICE 
        { 
            get 
            { 
                return ConfigurationManager.AppSettings["IR1"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do ROLAGEM BOI para calculo de posição bmf (BR1)
        /// </summary>
        private string FATORCALCULOROLAGEMBOI 
        { 
            get 
            {
                return ConfigurationManager.AppSettings["BR1"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do ROLAGEM DE CAFE para calculo de posição bmf (CR1)
        /// </summary>
        private string FATORCALCULOROLAGEMCAFE 
        { 
            get 
            {
                return ConfigurationManager.AppSettings["CR1"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do ROLAGEM DE MILHO para calculo de posição bmf (MR1)
        /// </summary>
        private string FATORCALCULOROLAGEMMILHO 
        { 
            get 
            { 
                return ConfigurationManager.AppSettings["MR1"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do ROLAGEM DE SOJA para calculo de posição bmf (SR1)
        /// </summary>
        private string FATORCALCULOROLAGEMSOJA 
        { 
            get 
            { 
                return ConfigurationManager.AppSettings["SR1"].ToString(); 
            } 
        }

        /// <summary>
        /// Retorna o fator multiplicador do MINI CONTRATO DE INDICE para calculo de posição bmf (MINI INDICE)
        /// </summary>
        private string FATORCALCULOMINIINDICE
        {
            get
            {
                return ConfigurationManager.AppSettings["MINICONTRATO"].ToString();
            }
        }

        /// <summary>
        /// Retorna o fator multiplicador do MINI DOLAR para calculo de posição bmf (MINI DOLAR)
        /// </summary>
        private string FATORCALCULOMINIDOLAR
        {
            get
            {
                return ConfigurationManager.AppSettings["MINICONTRATODOLAR"].ToString();
            }
        }

        /// <summary>
        /// Retorna o fator multiplicador do INDICE para calculo de posição bmf (INDICE contrato cheio)
        /// </summary>
        private string FATORCALCULOINDICE
        {
            get
            {
                return ConfigurationManager.AppSettings["INDICE"].ToString();
            }
        }

        /// <summary>
        /// Retorna o fator multiplicador do DOLAR para calculo de posição bmf (contrato cheio DOLAR)
        /// </summary>
        private string FATORCALCULODOLAR
        {
            get
            {
                return ConfigurationManager.AppSettings["DOLAR"].ToString();
            }
        }

        /// <summary>
        /// Retorna o fator multiplicador do contrato cheio BOI para calculo de posição bmf (BGI)
        /// </summary>
        private string FATORCALCULOBOICHEIO
        {
            get
            {
                return ConfigurationManager.AppSettings["BGI"].ToString();
            }
        }

        /// <summary>
        /// Retorna o fator multiplicador do MINI contrato BOI para calculo de posição bmf (BGI)
        /// </summary>
        private string FATORCALCULOBOIMINI
        {
            get
            {
                return ConfigurationManager.AppSettings["WBG"].ToString();
            }
        }

        /// <summary>
        /// Retorna o fator multiplicador innicial de DI para calculo de posição bmf (DI)
        /// </summary>
        private string FATORCALCULODI
        {
            get
            {
                return ConfigurationManager.AppSettings["DI"].ToString();
            }
        }

        /// <summary>
        /// Retorna o tempo de expiração da lista de negociações de PLD que está no *.config
        /// </summary>
        private int PLDTEMPOTOTAL
        {
            get
            {
                return ConfigurationManager.AppSettings["TEMPOPLD"].DBToInt32();
            }
        }

        /// <summary>
        /// Retorna o pld critico, (15 minutos) no *.config.
        /// Passado desse tempo, os status de criticidade da listagem de pld se torna CRÍTICA
        /// </summary>
        private int PLDCRITICO
        {
            get
            {
                return ConfigurationManager.AppSettings["PLDCRITICO"].DBToInt32();
            }
        }

        /// <summary>
        /// Retorna o pld ALerta, (16 minutos) no *.config.
        /// Passado desse tempo, os status de criticidade da listagem de pld se torna ALERTA
        /// </summary>
        private int PLDALERTA
        {
            get
            {
                return ConfigurationManager.AppSettings["PLDALERTA"].DBToInt32();
            }
        }

        /// <summary>
        /// Retorna o pld FOLGA, (26 minutos) no *.config.
        /// Passado desse tempo, os status de criticidade da listagem de pld se torna FOLGA
        /// </summary>
        private int PLDFOLGA
        {
            get
            {
                return ConfigurationManager.AppSettings["PLDFOLGA"].DBToInt32();
            }
        }

        /// <summary>
        /// Retorna o Intervalo de calculo pronto (em milisegundos) para cada CLIENTE
        /// Têm a finalidade de assegurar que se não já nada configurado para o Intervalo de recalculo a propriedade 
        /// irá setar 01 minuto
        /// </summary>
        private int IntervaloRecalculo
        {
            get
            {
                int intervaloRecalculo = 60000;

                if (ConfigurationManager.AppSettings["IntervaloRecalculo"] != null)
                {
                    intervaloRecalculo = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloRecalculo"].ToString());
                    intervaloRecalculo *= 1000;
                }

                return intervaloRecalculo;
            }
        }

        /// <summary>
        /// Retorna o Intervalo de calculo Fora do Pregão pronto (em milisegundos) para ser comparado se está em horário de 
        /// Leilão ou quando acabou o pregão
        /// </summary>
        private int IntervaloRecalculoForaPregao
        {
            get
            {
                int intervaloRecalculo = 300000;

                if (ConfigurationManager.AppSettings["IntervaloRecalculoForaPregao"] != null)
                {
                    intervaloRecalculo = Convert.ToInt32(ConfigurationManager.AppSettings["IntervaloRecalculoForaPregao"].ToString());
                    intervaloRecalculo *= 1000;
                }

                return intervaloRecalculo;
            }
        }

        #endregion
        /// <summary>
        /// Construtor da Classe ServerMonitor
        /// </summary>
        public ServerMonitor()
        {
            //log4net.Config.XmlConfigurator.Configure();          
        }

        /// <summary>
        /// Função que Inicializa as funcionalidades principais do serviço de monitoração de Lucro Prejuizo, tais como:
        /// Obtem os vencimentos de DI
        /// Obtem os feriados para calculo de dias uteis
        /// Obtem lista de cotações de abertura
        /// Obtem lista de cotações de fechamento
        /// Obtem Lista de tipos de mercado
        /// Inicializa thread principal (ThreadClientes) deexecução de calculos de posições de clientes
        /// </summary>
        /// <param name="sender">Sender do tipo object para enviar </param>
        public void StartMonitor(object sender)
        {
            try
            {
                logger.Info("Iniciando Monitor de Risco");
                logger.Info("Inicializando Thread para obter cotações em tempo real");
                htVencimentoDI = new PersistenciaPLD().ObterVencimentosDI();

                lsFeriadosDI = new PersistenciaPLD().ObterFeriadosDI();

                logger.Info("Pausa de 5 segundos"); 
                Thread.Sleep(5000);

                logger.Info("Popula objetos de memoria do sistema (Fechamentos,aberturas e tipo de mercado.)");

                logger.Info("Inicianliza Hash de fechamentos");
                CotacaoFechamento    = ObterCotacaoFechamento();

                logger.Info("Inicializa Hash de aberturas");
                CotacaoAbertura      = ObterCotacaoAbertura();

                logger.Info("Inicializa Hash de tipo de mercado");
                MercadoInstumento    = ObterMercadoInstrumento();

                logger.Info("Inicializa Hash de Ajuste BMF");
                CotacaoAjuste = ObterCotacaoAjuste();

                logger.Info("Objetos carregados com sucesso");
                logger.Info("Inicia rotina principal do sistema");

                logger.Info("Pausa de 2 segundos");
                Thread.Sleep(2000);

                //logger.Info("Inicia Thread de calculo de posição de lucro e prej dos clientes a cada 5 segundos.");
                thThreadClientes      = new Thread(new ThreadStart(ThreadClientes));
                thThreadClientes.Name = "ThreadClientes";
                thThreadClientes.Start();

                logger.Info("Processo de inicialização finalizado");

                logger.Info("Aguardando Transações ...");
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o metodo StartMonitor.", ex);
            }
        }

        #region Controle PLD / REPASSE

        #region Sessão de PLD
        /// <summary>
        /// Efetua o calculo de dias uteis entre um range de datas
        /// </summary>
        /// <param name="dataInicial">Data Inicial do range</param>
        /// <param name="dataFinal">Data Final do Range</param>
        /// <returns>Retorna o número de dias uteis</returns>
        public int ObterDiasUteis(DateTime dataInicial, DateTime dataFinal)
        {
            int dias = 0;
            int ContadorDias = 0;

            dias = dataInicial.Date.Subtract(dataFinal).Days;

            DateTime lDataAtual = dataInicial.Date;

            if (dias < 0)
                dias = dias * -1;

            var lContFeriados = from feriado in lsFeriadosDI where feriado >= lDataAtual && feriado <= dataFinal select feriado;

            for (int i = 1; i <= dias; i++)
            {
                dataInicial = dataInicial.AddDays(1).Date;

                if ((dataInicial.DayOfWeek == DayOfWeek.Sunday || dataInicial.DayOfWeek == DayOfWeek.Saturday) && lContFeriados.Contains(dataInicial))
                {
                    ContadorDias++;
                }

                if (dataInicial.DayOfWeek != DayOfWeek.Sunday && dataInicial.DayOfWeek != DayOfWeek.Saturday)
                    ContadorDias++;
            }

            if (lContFeriados != null)
            {
                ContadorDias = ContadorDias - lContFeriados.Count();
            }

            return ContadorDias;
        }

        /// <summary>
        /// Efetua o Calculo da taxa PTax para ser usado no calculo de DI na função CalcularTaxaDI
        /// </summary>
        /// <param name="Instrumento">Insturmento a ser calculado</param>
        /// <param name="taxaOperada">Taxa Operada</param>
        /// <returns>Retorna o calculo da taxa pTax com a taxa operada</returns>
        private double CalcularTaxaPtax(string Instrumento, double taxaOperada)
        {
            double lRetorno = 0.0;

            double taxaMercado = new PersistenciaMonitorRisco().ObterCotacaoPtax();

            lRetorno = taxaOperada * taxaMercado;

            return lRetorno;
        }

        /// <summary>
        /// Efetua o calculo da Taxa DI com base nos exponencial dos dias uteis, A cotação do DI atual e o seu preço Unitário.
        /// </summary>
        /// <param name="Instrumento">Instrumento a ser calculado</param>
        /// <param name="taxaOperada">Taxa Operada do instrumento operada pelo cliente</param>
        /// <returns>Retorna a taxa de DI calculada para o requisitante</returns>
        private double CalcularTaxaDI(string Instrumento, double taxaOperada)
        {
            double Ajuste = 0;
            double Numerador = 100000;
            double NumeroDiasBase = 252;
            
            try
            {
                DateTime dtVencimento       = DateTime.Parse(htVencimentoDI[Instrumento].ToString());              
             
                double NumeroDiasCalculados = this.ObterDiasUteis(DateTime.Now, dtVencimento);
                double Exponencial          = (NumeroDiasCalculados / NumeroDiasBase);

                double taxaMercado = double.Parse(ObterCotacaoAtual(Instrumento).ToString());
 
                if (taxaOperada == taxaMercado)
                {
                    return 0;
                }

                #region CALCULO PU            

                double PUInicial = (Numerador / Math.Pow(((1 + (taxaOperada / 100))), Exponencial));
                double PUFinal   = (Numerador / Math.Pow(((1 + (taxaMercado / 100))), Exponencial));
                
                Ajuste           = (Math.Round(PUFinal,2) - Math.Round(PUInicial,2));

                #endregion


            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao calcular o Spreed do DI", ex);
            }

            return Math.Round(Ajuste,2);
           
        }

        /// <summary>
        /// Obtem Listagem de PLD via chamada da interface
        /// </summary>
        /// <param name="sender"></param>
        public void ObterRelacaoPLD(object sender)
        {
            try{
                List<PLDOperacaoInfo> Operacoes = new List<PLDOperacaoInfo>();      

                Operacoes = new PersistenciaPLD().ObterOperacoesPLD();

                lock (Operacoes)
                {
                    #region Monitor de PLD { Posição de PLD , Lucro e Prej e Criticidade dos PLDS } 

                    #region Calcula os fatores de multiplicação dos instrumentos BMF bem como o Lucro e Prejuiso dos derivativos

                    List<LucroPrejuisoPLDInfo> lstPLDLucroPrejuiso = new List<LucroPrejuisoPLDInfo>();

                    foreach (var item in Operacoes){

                        LucroPrejuisoPLDInfo Info = new LucroPrejuisoPLDInfo();

                        string ClassificacaoInstrumento = item.Intrumento.Substring(0, 3);

                        switch (ClassificacaoInstrumento)
                        {
                            case INDICE:
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOINDICE.ToString(), gCultura);
                                break;
                            case DOLAR:
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULODOLAR.ToString(), gCultura);
                                break;
                            case DI:
                                Info.FatorMultiplicador = decimal.Parse(CalcularTaxaDI(item.Intrumento, double.Parse(item.PrecoNegocio.ToString(), gCultura)).ToString(), gCultura);                            
                                break;
                            case MINIBOLSA:
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOMINIINDICE.ToString(), gCultura);
                                break;
                            case MINIDOLAR:
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOMINIDOLAR.ToString(), gCultura);
                                break;
                            case CHEIOBOI     :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOBOICHEIO.ToString(), gCultura);
                                break;
                            case MINIBOI      :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOBOIMINI.ToString(), gCultura);
                                break;
                            case EURO         :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOEURO.ToString(), gCultura);
                                break;
                            case MINIEURO     :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOMINIEURO.ToString(), gCultura);
                                break;
                            case CAFE        :
                                Info.FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(item.Intrumento, double.Parse(FATORCALCULOCAFE, gCultura)).ToString(), gCultura);
                                break;
                            case MINICAFE    :
                                Info.FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(item.Intrumento,  double.Parse(FATORCALCULOMINICAFE, gCultura)).ToString(), gCultura);
                                break;
                            case FUTUROACUCAR :
                                Info.FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(item.Intrumento,  double.Parse(FATORCALCULOFUTUROACUCAR, gCultura)).ToString(),gCultura);
                                break;
                            case ETANOL       :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOETANOL.ToString(),gCultura);
                                break;
                            case ETANOLFISICO :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOETANOLFISICO.ToString(),gCultura);
                                break;
                            case MILHO        :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOMILHO.ToString(),gCultura);
                                break;
                            case SOJA         :
                                Info.FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(item.Intrumento,  double.Parse(FATORCALCULOSOJA,gCultura)).ToString(),gCultura);
                                break;
                            case OURO         :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOOURO.ToString(),gCultura);
                                break;
                            case ROLAGEMDOLAR :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMDOLAR.ToString(),gCultura);
                                break;
                            case ROLAGEMINDICE:
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMINDICE.ToString(),gCultura);
                                break;
                            case ROLAGEMBOI   :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMBOI.ToString(),gCultura);
                                break;
                            case ROLAGEMCAFE  :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMCAFE.ToString(),gCultura);
                                break;
                            case ROLAGEMMILHO :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMMILHO.ToString(),gCultura);
                                break;
                            case ROLAGEMSOJA  :
                                Info.FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMSOJA.ToString(),gCultura);
                                break;
                        }
                        
                        Info.Instrumento    = item.Intrumento;
                        Info.PrecoNegocio   = item.PrecoNegocio;
                        Info.QtdeNegocio    = item.Quantidade;
                        Info.PrecoMercado   = ObterCotacaoAtual(Info.Instrumento);
                        Info.Sentido        = item.Sentido;
                        Info.Seq            = item.Seq;

                        if (Info.Sentido == "V"){

                            if (ClassificacaoInstrumento == DI)
                            {
                                Info.LucroPrejuiso = Math.Round((Info.QtdeNegocio * Info.FatorMultiplicador), 2);
                            }
                            else
                            {
                                Info.LucroPrejuiso = (((Info.PrecoNegocio - Info.PrecoMercado) * Info.QtdeNegocio) * Info.FatorMultiplicador);
                            }
                        }
                        else{
                            if (ClassificacaoInstrumento == DI)
                            {
                                Info.LucroPrejuiso = Math.Round((Info.QtdeNegocio * Info.FatorMultiplicador), 2);
                            }
                            else
                            {
                                Info.LucroPrejuiso = (((Info.PrecoMercado - Info.PrecoNegocio) * Info.QtdeNegocio) * Info.FatorMultiplicador);
                            }
                        }


                        lstPLDLucroPrejuiso.Add(Info);
                       
                    }

                    #endregion                           

                    for (int i = 0; i <= Operacoes.Count - 1; i++){

                        if (Operacoes.Count == lstPLDLucroPrejuiso.Count){
                            if (Operacoes[i].Seq == lstPLDLucroPrejuiso[i].Seq)
                            {
                                Operacoes[i].PrecoMercado = lstPLDLucroPrejuiso[i].PrecoMercado;
                                Operacoes[i].LucroPrejuiso = lstPLDLucroPrejuiso[i].LucroPrejuiso;

                                if (Operacoes[i].STATUS != PLDACEITO)
                                {
                                    TimeSpan MinutosRemanescentes = new TimeSpan();

                                    DateTime dataNegocio = (Operacoes[i].HR_NEGOCIO);
                                    DateTime dataExpiracaoNegocio = dataNegocio.AddMinutes(PLDTEMPOTOTAL);

                                    MinutosRemanescentes = (dataExpiracaoNegocio - DateTime.Now);
                                    Operacoes[i].MinutosRestantesPLD = MinutosRemanescentes;

                                    int MinutosRestantes = MinutosRemanescentes.Minutes;

                                    if (MinutosRestantes <= PLDCRITICO)
                                    {
                                        Operacoes[i].Criticidade = EnumCriticidadePLD.CRITICO;
                                        Operacoes[i].DescricaoCriticidade = "CRITICO";
                                    }
                                    else if ((MinutosRestantes >= PLDCRITICO) && (MinutosRestantes <= PLDALERTA))
                                    {
                                        Operacoes[i].Criticidade = EnumCriticidadePLD.ALERTA;
                                        Operacoes[i].DescricaoCriticidade = "ALERTA";
                                    }
                                    else
                                    {
                                        Operacoes[i].Criticidade = EnumCriticidadePLD.FOLGA;
                                        Operacoes[i].DescricaoCriticidade = "FOLGA";
                                    }
                                  
                                }
                            }
                        }
                    }
           
                    #endregion
                }

                lock (ClientesPLD)
                {
                    ClientesPLD.Clear();
                    ClientesPLD = Operacoes;
                }
            }
            catch (Exception ex){
                logger.Info("Ocorreu um erro ao carregar a relação PLD dos clientes. " + ex.Message);
            }
        }

        #endregion

        #endregion

        #region Monitor de Risco (L/P) 

        #region Gerenciamento de controle de threads
        /// <summary>
        /// Calcula o total de custodia de ativos de bovespa (sem Tesouro Direto), fazendo a somatoria da cotação x Quantidade atual do cliente 
        /// dvidido pelo Fator de Cotação
        /// </summary>
        /// <param name="CodigoBovespa">Codigo Bovespa</param>
        /// <param name="CodigoBmf">Codigo BMF</param>
        /// <returns>Retorna o valor total de custódia do cliente</returns>
        public decimal TotalCustodiaMonitorIntradiario(int CodigoBovespa, int CodigoBmf)
        {
            decimal lRetorno = 0;

            try
            {
                List<MonitorCustodiaInfo.CustodiaPosicao> lPosicao = PersistenciaMonitorRisco.ConsultarCustodiaNormalSql(CodigoBovespa,  CodigoBmf);

                foreach (MonitorCustodiaInfo.CustodiaPosicao posicao in lPosicao)
                {
                    if ((posicao.TipoMercado == "VIS" && posicao.TipoGrupo != "TEDI")|| 
                        posicao.TipoMercado == "OPC" || 
                        posicao.TipoMercado == "TER" || 
                        posicao.TipoMercado == "OPV" )
                    {
                        lRetorno += (posicao.Cotacao * posicao.QtdeAtual) / posicao.FatorCotacao;
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro na rotina de TotalCustodiaMonitorIntradiario ", ex);
            }

            return lRetorno;
        }
        
        /// <summary>
        /// Esse método recebe a posição do cliente para efetuar o calculo de Net X SFP (Situação financeira patrimonial) e insere dados no 
        /// sql no banco de Risco um resumo intradiário com algumas posições importantes do cliente, inclusive SFP
        /// L1 para consulta posterior em relatório na intranet...
        /// </summary>
        /// <param name="pInfo">Exposição do cliente para fazer o calculo de Net x SFP e inserir no banco a posição do cliente</param>
        public void AtualizaExposicaoCliente(ExposicaoClienteInfo pInfo)
        {
            var lDb = new PersistenciaMonitorRisco();

            var lIntradiario = new MonitoramentoIntradiarioInfo();

            try
            {
                lIntradiario.CodigoCliente    = pInfo.CodigoBovespa;
                lIntradiario.CodigoClienteBmf = pInfo.CodigoBMF;
                lIntradiario.CodigoAssessor   = int.Parse(pInfo.Assessor);
                lIntradiario.NomeAssessor     = pInfo.NomeAssessor;
                lIntradiario.NomeCliente      = pInfo.Cliente;

                decimal lTotalBtc      = 0;
                decimal lTotalBovespa  = 0;
                decimal lSaldoD0       = new PersistenciaMonitorRisco().ObterSaldoD0(pInfo.CodigoBovespa);
                decimal lTotalCustodia = this.TotalCustodiaMonitorIntradiario(pInfo.CodigoBovespa, pInfo.CodigoBMF);

                if (pInfo.OrdensBTC != null)
                    pInfo.OrdensBTC.ForEach(btc =>
                    {
                        lTotalBtc += (btc.Quantidade * btc.PrecoMercado);
                    });

                if (pInfo.Operacoes != null)
                    pInfo.Operacoes.ForEach(bov =>
                    {
                        lTotalBovespa += bov.LucroPrejuizo;
                    });

                lIntradiario.Posicao = lTotalCustodia - lTotalBtc + lSaldoD0;
                lIntradiario.Exposicao = lTotalBovespa;

                if (lIntradiario.Posicao != 0)
                {
                    if (((lIntradiario.Exposicao * 100) / lIntradiario.Posicao) > 100)
                    {
                        lIntradiario.EXPxPosicao = 100;
                    }
                    else
                    {
                        lIntradiario.EXPxPosicao = Decimal.Round(((lIntradiario.Exposicao * 100) / lIntradiario.Posicao),3);
                    }
                }
                else
                {
                    lIntradiario.EXPxPosicao = 100;
                }

                lIntradiario.Net = pInfo.NetOperacoes;
                lIntradiario.SFP = pInfo.SFPL1;

                if (pInfo.SFPL1 != 0)
                {
                    if (((pInfo.NetOperacoes * 100) / pInfo.SFPL1) > 100)
                    {
                        lIntradiario.NETxSFP = 100;
                    }
                    else
                    {
                        lIntradiario.NETxSFP = Decimal.Round(((pInfo.NetOperacoes * 100) / pInfo.SFPL1), 3);
                    }
                }
                else
                {
                    lIntradiario.NETxSFP = 100;
                }

                lIntradiario.LucroPrejuizo      = pInfo.LucroPrejuizoTOTAL;
                lIntradiario.PercentualPrejuizo = pInfo.PercentualPrejuizo;
                lIntradiario.VolumeBovespa      = pInfo.VolumeTotalFinanceiroBov;

                lDb.AtualizarPosicaoExposicaoCliente(lIntradiario);
            }
            catch (Exception ex)
            {
                logger.Error("Erro na rotina de Atualização da Exposição do Cliente ", ex);
            }
        }

        /// <summary>
        /// Thread em que os clientes são relacionados do banco de dados 
        /// para entrarem na relação de clientes monitorados.
        /// Depois de adicionados, é chamada a função de RunTimer
        /// </summary>
        private void ThreadClientes()
        {
            DateTime lastRun      = DateTime.MinValue;
            DateTime lastRun10Min = DateTime.MinValue;


            while (_bKeepRunning)
            {
                TimeSpan interval = DateTime.Now - lastRun;

                try
                {
                    List<int> RelacaoClientes = null;

                    List<int> RelacaoClientesRodada = null;

                    if (interval.TotalMilliseconds > 120000)
                    {
                        lastRun = DateTime.Now;

                        logger.Debug("Obtendo relacao de clientes que operaram no dia");

                        this.ClientesMonitor.Clear();

                        RelacaoClientes = new PersistenciaMonitorRisco().ObterClientesPosicaoDia();

                        RelacaoClientesRodada = new PersistenciaMonitorRisco().ObterClientesPosicaoDiaLimitada();

                        #region Clientes BMF AFTER

                        logger.Debug("Obtendo relacao de clientes com posicao AFTER BMF");

                        List<int> BMFAfter = new PersistenciaMonitorRisco().ObterClientesPosicaoBMFAfter();

                        foreach (int Cliente in BMFAfter)
                        {
                            if (!RelacaoClientesRodada.Contains(Cliente))
                            {
                                RelacaoClientesRodada.Add(Cliente);
                            }

                            if (!RelacaoClientes.Contains(Cliente))
                            {
                                RelacaoClientes.Add(Cliente);
                            }
                        }

                        #endregion

                        if (RelacaoClientes.Count > 0)
                        {
                            logger.Info("Relacao de clientes encontrados.[" + RelacaoClientesRodada.Count.ToString() + "].");
                        }
                        else
                        {
                            logger.Info("Não existe clientes para serem calculados nesta tentativa.");
                        }


                        if (RelacaoClientesRodada.Count > 0)
                        {
                            logger.Info("Relacao de clientes encontrados.[" + RelacaoClientesRodada.Count.ToString() + "].");
                        }
                        else
                        {
                            logger.Info("Não existe clientes para serem calculados nesta tentativa.");
                        }

                        lock (ClientesMonitor)
                        {
                            foreach (int Cliente in RelacaoClientesRodada)
                            {
                                if (!ClientesMonitor.Contains(Cliente))
                                {
                                    logger.Info("Acrescentando [" + Cliente + "] na lista de monitoracao");
                                    this.ClientesMonitor.Add(Cliente);
                                    this.RunTimer(Cliente);
                                    Thread.Sleep(250);
                                }
                            }
                        }
                    }

                    DateTime lNow = DateTime.Now;

                    DateTime lDataInicial = new DateTime(lNow.Year, lNow.Month, lNow.Day, 10, 30, 00);

                    DateTime lDataFinal = new DateTime(lNow.Year, lNow.Month, lNow.Day, 11, 30, 00);

                    bool EhHorarioProibido = (lDataInicial <= lNow && lDataFinal >= lNow);

                    TimeSpan interval10Min = DateTime.Now - lastRun10Min;

                    //logger.InfoFormat("EhHorarioProibido->[{0}]", EhHorarioProibido);

                    //logger.InfoFormat("interval10Min.TotalMilliseconds -> [{0}]", interval10Min.TotalMilliseconds);

                    if (interval10Min.TotalMilliseconds > 2400000 && !EhHorarioProibido) //--> de vinte em vinte Minutos
                    {
                        lastRun10Min = DateTime.Now;

                        logger.Info("Entrou na rotina de dez em dez minutos para clientes que operaram hoje mas não nos ultimos 2 minutos.");

                        var RelacaoParaAtualizar = new List<int>() ;

                        if (RelacaoClientes == null)
                        {
                            RelacaoClientes = new PersistenciaMonitorRisco().ObterClientesPosicaoDia();
                        }

                        foreach (int Cliente in RelacaoClientes)
                        {
                            if (!RelacaoParaAtualizar.Contains(Cliente))
                                RelacaoParaAtualizar.Add(Cliente);
                        }

                        foreach (int Cliente in RelacaoParaAtualizar)
                        {
                            this.RunTimer(Cliente);
                            Thread.Sleep(250);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Ocorreu um erro ao acessar o método ThreadClientes:" + ex.Message, ex);
                }

                Thread.Sleep(250);
            }

            //_bKeepRunning = false;
        }

        /// <summary>
        /// Método RunTimer é responsável por montar o Mutex do cliente e inseri-lo
        /// na lista de clientes monitorados
        /// </summary>
        /// <param name="CodigoCliente">Código principal do cliente</param>
        private void RunTimer(int CodigoCliente)
        {
            try
            {
                ClienteMutexInfo StateObj = new ClienteMutexInfo();

                StateObj.TimerCanceled     = false;
                StateObj.SomeValue         = 1;
                StateObj.IdCliente         = CodigoCliente;
                StateObj.StatusProcessando = EnumProcessamento.LIVRE;
                StateObj.FirstTimeProcessed = DateTime.Now;
                StateObj.Intervalo         = IntervaloRecalculo;

                System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(TimerTask);

                logger.Debug("Inicia timer para cliente [" + CodigoCliente + "] com [" + IntervaloRecalculo + "] ms");

                System.Threading.Timer TimerItem = new System.Threading.Timer(TimerDelegate, StateObj, 1000, IntervaloRecalculo);

                StateObj.TimerReference = TimerItem;

                if (htClientes.ContainsKey(CodigoCliente))
                {
                    htClientes.TryRemove(CodigoCliente, out StateObj);
                }

                htClientes.AddOrUpdate(CodigoCliente, StateObj, (key, oldValue) => StateObj);
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro no método RunTimer ", ex);
            }
        }

        /// <summary>
        /// Gera a lista de sufixos de contratos de BMFs
        /// </summary>
        /// <returns>Retorna lista de sufixos de contratos de bmf</returns>
        private List<string> ContratosAgricula()
        {
            List<string> lstAgricula = new List<string>();

            lstAgricula.Add("ICF");
            lstAgricula.Add("CR1");
            lstAgricula.Add("BGI");
            lstAgricula.Add("ETN");
            lstAgricula.Add("ETH");
            lstAgricula.Add("ISU");
            lstAgricula.Add("CCM");
            lstAgricula.Add("CTM");
            lstAgricula.Add("COP");
            lstAgricula.Add("CRV");
            lstAgricula.Add("CPG");
            lstAgricula.Add("MR1");
            lstAgricula.Add("SOJ");
            lstAgricula.Add("SR1");
            lstAgricula.Add("SFI");
            lstAgricula.Add("ISP");

            return lstAgricula;
        }
        
        /// <summary>
        /// Método de verificação de monitoração do cliente 
        /// Verifica s eo cliente não está em processamento e verifica se o cliente 
        /// pode ser recalculado para atualizar a exposição do mesmo.
        /// </summary>
        /// <param name="StateObj">Objeto cliente tipo object que irá posteriormente ser convertido para ClienteMutexInfo </param>
        private void TimerTask(object StateObj)
        {
            logger.Debug("Entrando no timertask [" + ((ClienteMutexInfo)StateObj).IdCliente + "]");
            try
            {
                ClienteMutexInfo State = (ClienteMutexInfo)StateObj;

                int Cliente = State.IdCliente;

                System.Threading.Interlocked.Increment(ref State.SomeValue);

                logger.Info("Thread disparada  " + DateTime.Now.ToString() + " Cliente: " + State.IdCliente.ToString());

                if (State.StatusProcessando == EnumProcessamento.LIVRE)
                {
                    State.StatusProcessando = EnumProcessamento.EMPROCESSAMENTO;

                    logger.Debug(" Cliente: " + State.IdCliente.ToString() + " - Aguardando Mutex");
                    State._Mutex.WaitOne();

                    //
                    logger.Info("INICIA CALCULO DE POSICAO  " + DateTime.Now.ToString() + " Cliente: " + State.IdCliente.ToString());

                    ExposicaoClienteInfo info = new ExposicaoClienteInfo();

                    //lock (MonitorLucroPrejuizo)
                    //{
                        info = this.CalcularPosicao(Cliente);

                        //if (MonitorLucroPrejuizo.ContainsKey(info.CodigoBovespa))
                        //    MonitorLucroPrejuizo[info.CodigoBovespa] = info;
                        //else
                    MonitorLucroPrejuizo.AddOrUpdate(info.CodigoBovespa, info, (key, oldValue) => info );
                    //.AddOrUpdate(instrumento, info, (key, oldValue) => info);
                    //}

                    logger.Info("POSICAO CALCULADA  " + DateTime.Now.ToString() + " Cliente: " + State.IdCliente.ToString());

                    this.AtualizaExposicaoCliente(info);
                    
                    State._Mutex.ReleaseMutex();
                    State.StatusProcessando = EnumProcessamento.LIVRE;
                }
                else
                {
                    logger.Info("processando  " + DateTime.Now.ToString() + " Cliente: " + State.IdCliente.ToString());
                }


                System.Threading.Timer myTimer = State.TimerReference;

                if (DateTime.Now.Hour <= 8 || DateTime.Now.Hour >= 20)
                {
                    if (State.Intervalo != IntervaloRecalculoForaPregao)
                    {
                        myTimer.Change(0, IntervaloRecalculoForaPregao);
                        State.Intervalo = IntervaloRecalculoForaPregao;
                        logger.Info("Alterando intervalo de recalculo do cliente [" + Cliente + "] para " +IntervaloRecalculoForaPregao + "ms");
                    }
                }
                else
                {
                    if (State.Intervalo != IntervaloRecalculo)
                    {
                        myTimer.Change(0, IntervaloRecalculo);
                        State.Intervalo = IntervaloRecalculo;
                        logger.Info("Alterando intervalo de recalculo do cliente [" + Cliente + "] para " + IntervaloRecalculo + "ms");
                    }
                }


                if (State.TimerReference != null)
                {
                    State.TimerReference.Dispose();
                    State.TimerReference = null;
                }
                

                if (State.TimerCanceled)
                {
                    if (State.TimerReference != null)
                    {
                        State.TimerReference.Dispose();
                    }

                    logger.Info("Done  " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao processar método TimerTask {requisição de processamento da rotina CalcularPosicao} " + ex.Message, ex);
            }
        }

        #endregion

        #region Rotinas de Calculo de Posicao de Lucro e Prej nos mercado de Bovespa e bmf.
        /// <summary>
        /// Método que gera uma listagem do sinacor de posição de BTC do cliente requisitado 
        /// </summary>
        /// <param name="idCliente">Codigo principal do cliente</param>
        /// <returns>Retorna Listagem de posição de Btc do cliente</returns>
        public List<BTCInfo> ObterPosicaoBTC(int idCliente)
        {
            List<BTCInfo> lstBtc = new PersistenciaMonitorRisco().ObterPosicaoBTC(idCliente);

            for (int i = 0; i <= lstBtc.Count - 1; i++)
            {
                string Instrumento = lstBtc[i].Instrumento;
                lstBtc[i].PrecoMercado = ObterCotacaoAtual(Instrumento);

            }

            return lstBtc;
        }

        /// <summary>
        /// Método que gera uma Listagem do sinacor de posição de clientes de Termo
        /// </summary>
        /// <param name="idCliente">Código principal do cliente</param>
        /// <returns>Retorna uma lista de Posição de Termo do cliente requisitado</returns>
        public List<PosicaoTermoInfo> ObterPosicaoTermo(int idCliente)
        {
            List<PosicaoTermoInfo> lstTermo = new PersistenciaMonitorRisco().ObterPosicaoTermo(idCliente);
            List<PosicaoTermoInfo> lstTermoAux = new List<PosicaoTermoInfo>();

            for (int i = 0; i <= lstTermo.Count - 1; i++)
            {
                string Instrumento = lstTermo[i].Instrumento;
                decimal LucroPrejuizo = 0;

                lstTermo[i].PrecoMercado = ObterCotacaoAtual(Instrumento);

                decimal diferencial = (lstTermo[i].PrecoMercado - lstTermo[i].PrecoExecucao);
                LucroPrejuizo = (lstTermo[i].Quantidade * diferencial);

                lstTermo[i].LucroPrejuizo = LucroPrejuizo;
            }

            var ListaTermo = from p in lstTermo
                             orderby p.Instrumento, p.DataVencimento
                             select p;


            foreach (var item in lstTermo)
            {
                lstTermoAux.Add(item);
            }

            return lstTermoAux;
        }

        /// <summary>
        /// Método que gera o valor da situação Financeira Patrimonial do cliente pelo sinacor
        /// </summary>
        /// <param name="IdCliente">Código principal do cliente </param>
        /// <returns>Retorna o valor de Situação Financeira Patrimonial</returns>
        public decimal ObterPosicaoFinanceiraPatrimonial(int IdCliente)
        {
            try
            {
                decimal SFP = new PersistenciaMonitorRisco().ObterSituacaoFinanceiraPatrimonial(IdCliente);
                return SFP;
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao calcular a sfp do cliente: " + IdCliente.ToString(), ex);
            }

            return 0;

        }

        /// <summary>
        /// Método que gera o valor da Posiçaõ de Situação Financeira Patrimonial L1 do cliente pelo sinacor
        /// </summary>
        /// <param name="IdCliente">Codigo principal do cliente</param>
        /// <returns>Retorna o valor da Posição FInanceira Patrimonial L1 da TSCCLIBOL</returns>
        public decimal ObterPosicaoFinanceiraPatrimonialL1(int IdCliente)
        {
            try
            {
                decimal SFP = new PersistenciaMonitorRisco().ObterSituacaoFinanceiraPatrimonialL1(IdCliente);
                return SFP;
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao calcular a sfp do cliente: " + IdCliente.ToString(), ex);
            }

            return 0;

        }

        /// <summary>
        /// Método que busca no sinacor e gera uma soma de das garantias de bovespa e garantias de bmf 
        /// sem margem
        /// </summary>
        /// <param name="IdClienteBov">Código Bovespa do cliente</param>
        /// <param name="IdClienteBmf">Código Bmf do cliente</param>
        /// <returns>Retorna a soma das garantias de bovespa e BMF para ser somada a propriedade de patrimonio líquido em tempo real</returns>
        public decimal ObterPosicaoGarantiaSemMargem(string IdClienteBov, string IdClienteBmf)
        {
            decimal lRetorno = 0.0M;

            try
            {
                decimal lGarantiaBmf = 0.0M;

                decimal lGarantiaBov = 0.0M;

                if (!string.IsNullOrEmpty(IdClienteBmf))
                {
                    lGarantiaBmf = new PersistenciaMonitorRisco().ObterSaldoGarantiaBmfSemMargem(int.Parse(IdClienteBmf));//.ObterPosicaoGarantiaBMF(int.Parse(IdClienteBmf));
                }

                if (!string.IsNullOrEmpty(IdClienteBov))
                {
                    lGarantiaBov = new PersistenciaMonitorRisco().ObterSaldoGarantiaBovespa(int.Parse(IdClienteBov));
                }

                lRetorno = lGarantiaBmf + lGarantiaBov ;
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao obter a posicao de Garantias do cliente: " + (!string.IsNullOrEmpty(IdClienteBov) ? IdClienteBov : IdClienteBmf).ToString(), ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que atribui valores as variáveis de garantias e margem requerida do cliente 
        /// em questão com os valores do sinacor e soma os valores de Garantia BMf + Garantia Bovespa + Margem Requerida BMF
        /// </summary>
        /// <param name="IdClienteBov">Código Bovespa do cliente</param>
        /// <param name="IdClienteBmf">Código Bmf do Cliente</param>
        /// <param name="GarantiaBov">Garantia Bovespa do Cliente</param>
        /// <param name="GarantiaBmf">Garantia Bmf do Cliente</param>
        /// <param name="MargemRequeridaBmf">Margem Requerida de BMF</param>
        /// <returns>Retorna Soma das garantias de Bovespa + Bmf + Margem Requerida BMF</returns>
        public decimal ObterPosicaoGarantias (string IdClienteBov, string IdClienteBmf, ref decimal GarantiaBov, ref decimal GarantiaBmf, ref decimal MargemRequeridaBmf)
        {
            decimal lRetorno = 0.0M;

            try
            {
                decimal lGarantiaBmf = 0.0M;

                decimal lGarantiaBov = 0.0M;

                decimal lMargemRequeridaBmf = 0.0M;

                if (!string.IsNullOrEmpty(IdClienteBmf))
                {
                    lGarantiaBmf = new PersistenciaMonitorRisco().ObterSaldoBMF(int.Parse(IdClienteBmf));//.ObterPosicaoGarantiaBMF(int.Parse(IdClienteBmf));

                    lMargemRequeridaBmf = new PersistenciaMonitorRisco().ObterPosicaoMargemRequeridaBMF(int.Parse(IdClienteBmf));

                    GarantiaBmf = lGarantiaBmf;

                    MargemRequeridaBmf = lMargemRequeridaBmf;
                }

                if (!string.IsNullOrEmpty(IdClienteBov))
                {
                    lGarantiaBov = new PersistenciaMonitorRisco().ObterSaldoGarantiaBovespa(int.Parse(IdClienteBov));

                    GarantiaBov = lGarantiaBov;
                }

                lRetorno = lGarantiaBmf + lGarantiaBov + lMargemRequeridaBmf;
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao obter a posicao de Garantias do cliente: " + (!string.IsNullOrEmpty(IdClienteBov) ? IdClienteBov: IdClienteBmf).ToString(), ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que gera Listagem de posição de Fundos do Cliente
        /// (IMPORTANTE) A Listagem é gerada somente a partir do SQL da posição só de ITAU.
        /// Futuramente é necessário efetuar a listagem também da Financial 
        /// para obter a posição completa dos clientes
        /// </summary>
        /// <param name="idCliente">Código principal do cliente</param>
        /// <returns>Retorna uma Listagem do Tipo ClienteFundoInfo (Fundos) </returns>
        public List<ClienteFundoInfo> ObterPosicaoFundos(int idCliente)
        {
            try
            {
                List<ClienteFundoInfo> lstFundos = new PersistenciaMonitorRisco().ObterPosicaoFundoCliente(idCliente);
                return lstFundos;
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao obter a posicao de fundos do cliente: " + idCliente.ToString(), ex);
            }

            return null;
        }

        /// <summary>
        /// Método que calcula e gera a posição completa de Monitor de 
        /// Risco de Lucro Prejuízo do Cliente.
        /// Esse método busca informações de banco de dados do sinacor e do GradualOMS (Risco) do sql 
        /// As informações da exposição do cliente são usadas para controle do risco e controle de operações, Garantias
        /// Situação Financeira Patrimonial Intraday atualizada do cliente
        /// </summary>
        /// <param name="CodigoCliente">Código principal do Cliente</param>
        /// <returns>Retorna A Exposição do cliente - Lista de Garantias, Lista de operações intraday BMF e BOVESPA, Limites operacionais,  </returns>
        private ExposicaoClienteInfo CalcularPosicao(object CodigoCliente)
        {
            ExposicaoClienteInfo      _MonitorExposicao   = new ExposicaoClienteInfo();
            List<ClienteRiscoResumo>  _ClienteResumoLista = new List<ClienteRiscoResumo>();
            ContaCorrenteInfo         _ContaCorrenteInfo = new ContaCorrenteInfo();

            decimal LucroPrejuizoAcumulado = 0;  

            decimal CustodiaValorizadaCompraIntraday = 0;
            decimal CustodiaValorizadaVendaIntraday = 0;
            decimal CustodiaValorizadaAbertura;

            try
            {
                //Obtem o saldo de conta corrente do cliente
                _ContaCorrenteInfo = new PersistenciaMonitorRisco().ObterSaldoContaCorrente(int.Parse(CodigoCliente.ToString()));

                #region Informações básicas sobre a conta do cliente ( codigos,nome,assessor etc )

                logger.Debug("Informações básicas sobre a conta do cliente [ " + CodigoCliente.ToString() + "]");

                int CodigoBovespa = int.Parse(CodigoCliente.ToString());

                _MonitorExposicao.OrdensExecutadas = new List<OperacoesInfo>();

                _MonitorExposicao.DtAtualizacao = DateTime.Now;

                DateTime dataIniDadosCli = DateTime.Now;

                //Obtem os dados de conta do cliente bovespa, codigo da conta bmf, 
                //Tipo de cliente, código de assessor, nome do cliente, nome do assessor
                //nome do assessor
                ClienteInfo _ClienteInfo = new PersistenciaMonitorRisco().ObterDadosCliente(CodigoBovespa);

                if ((_ClienteInfo.CodigoBMF == string.Empty) || (_ClienteInfo.CodigoBMF == null))
                {
                    int CodigoBmf = new PersistenciaMonitorRisco().ObterContaBMF(CodigoBovespa);

                    if (CodigoBmf > 0)
                    {
                        _ClienteInfo.CodigoBMF = CodigoBmf.ToString();
                    }
                }

                TimeSpan stampDadosCli = (DateTime.Now - dataIniDadosCli);

                _MonitorExposicao.CodigoBovespa = CodigoBovespa;
                _MonitorExposicao.Cliente       = _ClienteInfo.NomeCliente;
                _MonitorExposicao.Assessor      = _ClienteInfo.Assessor;
                _MonitorExposicao.NomeAssessor  = _ClienteInfo.NomeAssessor;

                #endregion

                #region Rotina de Calculo e processamento de bmf.
                logger.Debug("Rotina de Calculo e processamento de bmf. [ " + CodigoCliente.ToString() + "]");

                int QuantidadeContrato     = 0;
                decimal PosicaoAberturaDia = 0;

                if ((_ClienteInfo.CodigoBMF != string.Empty) && (_ClienteInfo.CodigoBMF != null))
                {
                    #region Abertura

                    //Posição de custódia de Abertura de bmf do dia
                    List<CustodiaAberturaInfo> PosicaoAberturaBMF = new PersistenciaMonitorRisco().ObterCustodiaAberturaBMF(int.Parse(_ClienteInfo.CodigoBMF));

                    if (PosicaoAberturaBMF.Count > 0)
                    {
                        decimal LucroPrejuizoCompraAbertura = 0;
                        decimal LucroPrejuizoVendaAbertura  = 0;

                        for (int i = 0; i <= PosicaoAberturaBMF.Count - 1; i++)
                        {
                            decimal FatorMultiplicador    = 0;
                            decimal PrecoContratoMercado  = 0;
                            decimal PrecoContratoAjuste = 0;
                            decimal DiferencialPontos     = 0;

                            //BGI: (PF – PI) * Q * 330
                            //WBG: (PF – PI) * Q * 33

                            CustodiaAberturaInfo _PosicaoAberturaBmf = (CustodiaAberturaInfo)(PosicaoAberturaBMF[i]);

                            string ClassificacaoInstrumento = _PosicaoAberturaBmf.Instrumento.Substring(0, 3);

                            switch (ClassificacaoInstrumento)
                            {
                                case INDICE:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOINDICE.ToString(), gCultura);
                                    break;
                                case DOLAR:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULODOLAR.ToString(), gCultura);
                                    break;
                                case DI:
                                    {
                                        decimal  lCotacaoDI = this.ObterAjusteBmf(PosicaoAberturaBMF[i].Instrumento);

                                        double lTaxa =  CalcularTaxaDI(PosicaoAberturaBMF[i].Instrumento, Convert.ToDouble(lCotacaoDI));

                                        FatorMultiplicador = Convert.ToDecimal(lTaxa);
                                    }
                                    break;
                                case MINIBOLSA:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOMINIINDICE.ToString(), gCultura);
                                    break;
                                case MINIDOLAR:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOMINIDOLAR.ToString(), gCultura);
                                    break;
                                case CHEIOBOI:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOBOICHEIO.ToString(), gCultura);
                                    break;
                                case MINIBOI:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOBOIMINI.ToString(), gCultura);
                                    break;
                                case EURO:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOEURO.ToString(), gCultura);
                                    break;
                                case MINIEURO:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOMINIEURO.ToString(), gCultura);
                                    break;
                                case CAFE:
                                    FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(_PosicaoAberturaBmf.Instrumento, double.Parse(FATORCALCULOCAFE,gCultura)).ToString());
                                    break;
                                case MINICAFE:
                                    FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(_PosicaoAberturaBmf.Instrumento, double.Parse(FATORCALCULOMINICAFE)).ToString());
                                    break;
                                case FUTUROACUCAR:
                                    FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(_PosicaoAberturaBmf.Instrumento, double.Parse(FATORCALCULOFUTUROACUCAR)).ToString());
                                    break;
                                case ETANOL:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOETANOL.ToString());
                                    break;
                                case ETANOLFISICO:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOETANOLFISICO.ToString());
                                    break;
                                case MILHO:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOMILHO.ToString());
                                    break;
                                case SOJA:
                                    FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(_PosicaoAberturaBmf.Instrumento, double.Parse(FATORCALCULOSOJA)).ToString());
                                    break;
                                case OURO:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOOURO.ToString());
                                    break;
                                case ROLAGEMDOLAR:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMDOLAR.ToString());
                                    break;
                                case ROLAGEMINDICE:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMINDICE.ToString());
                                    break;
                                case ROLAGEMBOI:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMBOI.ToString());
                                    break;
                                case ROLAGEMCAFE:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMCAFE.ToString());
                                    break;
                                case ROLAGEMMILHO:
                                    FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMMILHO.ToString());
                                    break;
                                case ROLAGEMSOJA:
                                    FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(_PosicaoAberturaBmf.Instrumento, double.Parse(FATORCALCULOROLAGEMSOJA)).ToString());
                                    break;

                            }

                            PrecoContratoMercado = ObterCotacaoAtual(_PosicaoAberturaBmf.Instrumento);
                            PrecoContratoAjuste = Convert.ToDecimal(CotacaoAjuste[_PosicaoAberturaBmf.Instrumento]); // ObterCotacaoAberturaInstrumento((_PosicaoAberturaBmf.Instrumento));

                            if (_PosicaoAberturaBmf.Quantidade < 0) // Posicao Vendida
                            {
                                DiferencialPontos = ((PrecoContratoMercado - PrecoContratoAjuste) * _PosicaoAberturaBmf.Quantidade);
                                LucroPrejuizoVendaAbertura += (DiferencialPontos * FatorMultiplicador);

                            }
                            else if (_PosicaoAberturaBmf.Quantidade > 0) // Posicao comprada
                            {
                                DiferencialPontos = ((PrecoContratoMercado - PrecoContratoAjuste) * _PosicaoAberturaBmf.Quantidade);
                                LucroPrejuizoCompraAbertura += (DiferencialPontos * FatorMultiplicador);
                            }

                        }

                        PosicaoAberturaDia = (LucroPrejuizoCompraAbertura + LucroPrejuizoVendaAbertura);
                        _MonitorExposicao.PLAberturaBMF = PosicaoAberturaDia;

                    }

                    #endregion

                    #region Lucro / Prej BMF

                    //Posição Intraday de BMF
                    List<PosicaoBmfInfo> PosicaoIntradayBMF = new PersistenciaMonitorRisco().ObtemPosicaoIntradayBMF(int.Parse(_ClienteInfo.CodigoBMF));

                    //Posição Intraday depois do after
                    List<PosicaoBmfInfo> PosicaoIntradayBMFAFTER = new PersistenciaMonitorRisco().ObtemPosicaoIntradayBMFAFTER(int.Parse(_ClienteInfo.CodigoBMF));

                    //Lista de contratos agricolas - Lista de contratos BMF
                    List<string> lstAgricula = this.ContratosAgricula();

                    foreach (string contrato in lstAgricula)
                    {

                        for (int i = 0; i <= PosicaoIntradayBMFAFTER.Count - 1; i++)
                        {

                            string ClassificacaoInstrumento =
                                PosicaoIntradayBMFAFTER[i].Contrato.Substring(0, 3);

                            if (lstAgricula.Contains(ClassificacaoInstrumento))
                            {

                                lock (PosicaoIntradayBMF)
                                {
                                    PosicaoIntradayBMF.Add(PosicaoIntradayBMFAFTER[i]);
                                }

                                lock (PosicaoIntradayBMFAFTER)
                                {
                                    PosicaoIntradayBMFAFTER.RemoveAt(i);
                                }
                            }
                        }
                    }

                    decimal LucroPrejuizoContratoCompra = 0;
                    decimal LucroPrejuizoContratoVenda = 0;

                    List<PosicaoBmfInfo> PosicaoBMFCalculada = new List<PosicaoBmfInfo>();

                    decimal lSubTotalCompraBmf = 0.0M;
                    decimal lSubTotalVendaBmf = 0.0M;

                    if (PosicaoIntradayBMF.Count > 0)
                    {
                        Dictionary<string, decimal> dicContratoValor = new Dictionary<string, decimal>();

                        for (int i = 0; i <= PosicaoIntradayBMF.Count - 1; i++)
                        {
                            PosicaoBmfInfo _PosicaoBmfInfo =
                                (PosicaoBmfInfo)(PosicaoIntradayBMF[i]);

                            string ClassificacaoInstrumento = _PosicaoBmfInfo.Contrato.Substring(0, 3);

                            switch (ClassificacaoInstrumento)
                            {
                                case INDICE:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOINDICE.ToString(), gCultura);
                                    break;
                                case DOLAR:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULODOLAR.ToString(), gCultura);
                                    break;
                                case DI:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(CalcularTaxaDI(_PosicaoBmfInfo.Contrato, double.Parse(_PosicaoBmfInfo.PrecoAquisicaoContrato.ToString())).ToString());
                                    break;
                                case MINIBOLSA:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOMINIINDICE.ToString(), gCultura);
                                    break;
                                case MINIDOLAR:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOMINIDOLAR.ToString(), gCultura);
                                    break;
                                case CHEIOBOI:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOBOICHEIO.ToString(), gCultura);
                                    break;
                                case MINIBOI:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOBOIMINI.ToString(), gCultura);
                                    break;
                                case EURO:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOEURO.ToString(), gCultura);
                                    break;
                                case MINIEURO:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOMINIEURO.ToString(), gCultura);
                                    break;
                                case CAFE:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(_PosicaoBmfInfo.Contrato, double.Parse(FATORCALCULOCAFE)).ToString());
                                    break;
                                case MINICAFE:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(_PosicaoBmfInfo.Contrato, double.Parse(FATORCALCULOMINICAFE)).ToString());
                                    break;
                                case FUTUROACUCAR:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(_PosicaoBmfInfo.Contrato, double.Parse(FATORCALCULOFUTUROACUCAR)).ToString());
                                    break;
                                case ETANOL:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOETANOL.ToString());
                                    break;
                                case ETANOLFISICO:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOETANOLFISICO.ToString());
                                    break;
                                case MILHO:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOMILHO.ToString());
                                    break;
                                case SOJA:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(_PosicaoBmfInfo.Contrato, double.Parse(FATORCALCULOSOJA)).ToString());
                                    break;
                                case OURO:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOOURO.ToString());
                                    break;
                                case ROLAGEMDOLAR:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMDOLAR.ToString());
                                    break;
                                case ROLAGEMINDICE:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMINDICE.ToString());
                                    break;
                                case ROLAGEMBOI:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMBOI.ToString());
                                    break;
                                case ROLAGEMCAFE:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMCAFE.ToString());
                                    break;
                                case ROLAGEMMILHO:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(FATORCALCULOROLAGEMMILHO.ToString());
                                    break;
                                case ROLAGEMSOJA:
                                    _PosicaoBmfInfo.FatorMultiplicador = decimal.Parse(CalcularTaxaPtax(_PosicaoBmfInfo.Contrato, double.Parse(FATORCALCULOROLAGEMSOJA)).ToString());
                                    break;

                            }

                            decimal ValorInstrumentoMercado = 0.0M;

                            decimal ValorInstrumentoMercadoReal = ObterCotacaoAtual(_PosicaoBmfInfo.Contrato);

                            if (!dicContratoValor.ContainsKey(_PosicaoBmfInfo.Contrato))
                            {
                                ValorInstrumentoMercado = ObterCotacaoAtual(_PosicaoBmfInfo.Contrato);

                                _PosicaoBmfInfo.PrecoContatoMercado = ValorInstrumentoMercado;

                                dicContratoValor.Add(_PosicaoBmfInfo.Contrato, ValorInstrumentoMercado);
                            }
                            else
                            {
                                _PosicaoBmfInfo.PrecoContatoMercado = dicContratoValor[_PosicaoBmfInfo.Contrato];
                            }

                            if (_PosicaoBmfInfo.Sentido == "V")
                            {
                                if (ClassificacaoInstrumento != DI)
                                {
                                    _PosicaoBmfInfo.DiferencialPontos = (_PosicaoBmfInfo.PrecoAquisicaoContrato - ValorInstrumentoMercadoReal);

                                    _PosicaoBmfInfo.LucroPrejuizoContrato = Math.Round(((_PosicaoBmfInfo.DiferencialPontos * _PosicaoBmfInfo.FatorMultiplicador) * _PosicaoBmfInfo.QuantidadeContato), 2);

                                    LucroPrejuizoContratoVenda += _PosicaoBmfInfo.LucroPrejuizoContrato;

                                    QuantidadeContrato -= _PosicaoBmfInfo.QuantidadeContato;

                                }
                                else
                                {
                                    _PosicaoBmfInfo.DiferencialPontos = (_PosicaoBmfInfo.PrecoAquisicaoContrato - ValorInstrumentoMercadoReal);
                                    
                                    //_PosicaoBmfInfo.LucroPrejuizoContrato = Math.Round((_PosicaoBmfInfo.DiferencialPontos * (_PosicaoBmfInfo.FatorMultiplicador * -1)), 2);
                                    
                                    _PosicaoBmfInfo.LucroPrejuizoContrato = (_PosicaoBmfInfo.QuantidadeContato * _PosicaoBmfInfo.FatorMultiplicador);
                                    
                                    _PosicaoBmfInfo.LucroPrejuizoContrato = Math.Round(_PosicaoBmfInfo.LucroPrejuizoContrato, 2);
                                    
                                    LucroPrejuizoContratoVenda += _PosicaoBmfInfo.LucroPrejuizoContrato;
                                    
                                    QuantidadeContrato -= _PosicaoBmfInfo.QuantidadeContato;
                                    
                                    _PosicaoBmfInfo.FatorMultiplicador = Math.Round(_PosicaoBmfInfo.FatorMultiplicador, 2);

                                }

                                lSubTotalVendaBmf += _PosicaoBmfInfo.LucroPrejuizoContrato;
                            }
                            if (_PosicaoBmfInfo.Sentido == "C")
                            {
                                if (ClassificacaoInstrumento != DI)
                                {
                                    _PosicaoBmfInfo.DiferencialPontos = (ValorInstrumentoMercadoReal - _PosicaoBmfInfo.PrecoAquisicaoContrato);

                                    _PosicaoBmfInfo.LucroPrejuizoContrato = Math.Round(((_PosicaoBmfInfo.DiferencialPontos * _PosicaoBmfInfo.FatorMultiplicador) * _PosicaoBmfInfo.QuantidadeContato), 2);
                                    
                                    LucroPrejuizoContratoCompra += _PosicaoBmfInfo.LucroPrejuizoContrato;
                                    
                                    QuantidadeContrato += _PosicaoBmfInfo.QuantidadeContato;

                                }
                                else
                                {
                                    _PosicaoBmfInfo.DiferencialPontos = (ValorInstrumentoMercadoReal - _PosicaoBmfInfo.PrecoAquisicaoContrato);
                                    
                                    //_PosicaoBmfInfo.LucroPrejuizoContrato = Math.Round((_PosicaoBmfInfo.DiferencialPontos * _PosicaoBmfInfo.FatorMultiplicador), 2);
                                    
                                    _PosicaoBmfInfo.LucroPrejuizoContrato = (_PosicaoBmfInfo.QuantidadeContato * _PosicaoBmfInfo.FatorMultiplicador) * -1;
                                    
                                    _PosicaoBmfInfo.LucroPrejuizoContrato = Math.Round(_PosicaoBmfInfo.LucroPrejuizoContrato, 2);

                                    LucroPrejuizoContratoCompra += _PosicaoBmfInfo.LucroPrejuizoContrato;

                                    QuantidadeContrato += _PosicaoBmfInfo.QuantidadeContato;
                                    
                                    _PosicaoBmfInfo.FatorMultiplicador = Math.Round(_PosicaoBmfInfo.FatorMultiplicador, 2);

                                }

                                lSubTotalCompraBmf += _PosicaoBmfInfo.LucroPrejuizoContrato;
                                
                            }

                            PosicaoBMFCalculada.Add(_PosicaoBmfInfo);
                        }

                    }

                    #endregion

                    _MonitorExposicao.LucroPrejuizoBMF = ((LucroPrejuizoContratoCompra + LucroPrejuizoContratoVenda)) + PosicaoAberturaDia;
                    _MonitorExposicao.OrdensBMF                = PosicaoBMFCalculada;
                    _MonitorExposicao.VolumeTotalFinanceiroBmf = (Math.Abs(lSubTotalCompraBmf) + Math.Abs(lSubTotalVendaBmf));
                }


                #endregion

                #region Situacao Financeira Patrimonial
                logger.Debug("Situacao Financeira Patrimonial [ " + CodigoCliente.ToString() + "]");

                _MonitorExposicao.SituacaoFinanceiraPatrimonial = this.ObterPosicaoFinanceiraPatrimonial(int.Parse(CodigoCliente.ToString()));
                _MonitorExposicao.SFPL1 = this.ObterPosicaoFinanceiraPatrimonialL1(int.Parse(CodigoCliente.ToString()));

                #endregion

                #region Posicao Fundos
                logger.Debug("Posicao Fundos [ " + CodigoCliente.ToString() + "]");
                List<ClienteFundoInfo> lstFundos = ObterPosicaoFundos(int.Parse(CodigoCliente.ToString()));
                

                if (lstFundos != null && lstFundos.Count > 0)
                {
                    _MonitorExposicao.PosicaoFundos = lstFundos;

                    foreach (var item in lstFundos)
                    {
                        _MonitorExposicao.TotalFundos += item.Saldo;
                    }
                }



                #endregion

                #region Posicao Tesouro Direto
                decimal lPosicaoTesouro = 0;

                lPosicaoTesouro = new PersistenciaMonitorRisco().ObterPosicaoTesouroDiretoCliente(int.Parse(CodigoCliente.ToString()));
                #endregion

                #region Saldo BMF
                logger.Debug("Saldo BMF [ " + CodigoCliente.ToString() + "]");
                
                decimal PosicaoBMF;

                if ((_ClienteInfo.CodigoBMF != string.Empty) && (_ClienteInfo.CodigoBMF != null))
                {
                    PosicaoBMF = new PersistenciaMonitorRisco().
                        ObterSaldoBMF(int.Parse(_ClienteInfo.CodigoBMF));

                    _MonitorExposicao.CodigoBMF = int.Parse(_ClienteInfo.CodigoBMF.ToString());
                    _MonitorExposicao.SaldoBMF = PosicaoBMF;

                }
                else
                {
                    PosicaoBMF = 0;
                    _MonitorExposicao.CodigoBMF = 0;
                    _MonitorExposicao.SaldoBMF = PosicaoBMF;
                }



                #endregion
                
                #region Rotina de calculo e processamento de bovespa
                logger.Debug("Rotina de calculo e processamento de bovespa [ " + CodigoCliente.ToString() + "]");

                #region Posicao de BTC

                _MonitorExposicao.OrdensBTC = this.ObterPosicaoBTC(int.Parse(CodigoCliente.ToString()));

                #endregion

                #region Obter Posicao Termo
                logger.Debug("Obter Posicao Termo [ " + CodigoCliente.ToString() + "]");

                _MonitorExposicao.OrdensTermo = this.ObterPosicaoTermo(int.Parse(CodigoCliente.ToString()));

                decimal LucroPrejuizoTermo = 0;

                foreach (var item in _MonitorExposicao.OrdensTermo)
                {
                    LucroPrejuizoTermo += item.LucroPrejuizo;
                }

                _MonitorExposicao.LucroPrejuizoTermo = LucroPrejuizoTermo;

                #endregion

                #region Posicoes de Abertura

                #region Conta Corrente na abertura do dia
                logger.Debug("Conta Corrente na abertura do dia [ " + CodigoCliente.ToString() + "]");

                DateTime dataIniCC = DateTime.Now;


                decimal PosicaoContaCorrenteAbertura = 0;
                PosicaoContaCorrenteAbertura = new PersistenciaMonitorRisco().ObterSaldoAbertura(CodigoBovespa);

                TimeSpan stampDadosCC = (DateTime.Now - dataIniCC);

                // CONTA CORRENTE DE ABERTURA + SALDO BMF 
                //_MonitorExposicao.ContaCorrenteAbertura = (PosicaoContaCorrenteAbertura + PosicaoBMF);
                _MonitorExposicao.ContaCorrenteAbertura = (PosicaoContaCorrenteAbertura);
                #endregion

                #region Posicao de Conta Margem na abertura do dia

                decimal PosicaoContaMargem = 0;
                PosicaoContaMargem = new PersistenciaMonitorRisco().ObterSaldoContaMargem(CodigoBovespa);
                _MonitorExposicao.SaldoContaMargem = PosicaoContaMargem;

                #endregion

                #region Posicao de custodia de abertura

                decimal PosicaoCustodiaAbertura = 0;

                DateTime dataIniCus = DateTime.Now;

                List<CustodiaAberturaInfo> _lstCustodiaAbertura = new PersistenciaMonitorRisco().ObterCustodiaAbertura(CodigoBovespa);

                TimeSpan stampDadosCus = (DateTime.Now - dataIniCus);

                // Calcula Patrimonio Liquido Abertura
                foreach (var CustodiaItem in _lstCustodiaAbertura)
                {

                    decimal CotacaoAtualAbert = ObterCotacaoAtual(CustodiaItem.Instrumento);

                    // SUBTRAIR O BTC DO PL.
                    if (CustodiaItem.TipoMercado != "BMF")
                    {
                        if (CustodiaItem.CodigoCarteira != CodigoCarteiraBTC)
                        {
                            PosicaoCustodiaAbertura += (CustodiaItem.Quantidade * CotacaoAtualAbert);
                        }
                        else
                        {
                            PosicaoCustodiaAbertura -= (CustodiaItem.Quantidade * CotacaoAtualAbert);
                        }
                    }
                }

                _MonitorExposicao.CustodiaAbertura = (PosicaoCustodiaAbertura);
                CustodiaValorizadaAbertura = PosicaoCustodiaAbertura;

                #endregion

                #endregion

                ///logger.Info("Limites operacionais [ " + CodigoCliente.ToString() + "]");

                #region LimitesOperacionais

                _MonitorExposicao.LimitesOperacionais = new PersistenciaMonitorRisco().ObterLimiteOperacional(CodigoBovespa);

                #endregion

                #region Resumo das posicoes Intraday

                DateTime dataIniIntra = DateTime.Now;
                List<string> lstPapeisOperados = new List<string>();

                List<PosicaoClienteIntradayInfo> PosicaoIntraday =
                    new PersistenciaMonitorRisco().ObtemPosicaoIntraday(CodigoBovespa.ToString());

                TimeSpan stampDadosIntra = (DateTime.Now - dataIniIntra);

                #region Popula o tipo de mercado dos instrumentos

                for (int i = 0; i <= PosicaoIntraday.Count - 1; i++)
                {
                    PosicaoIntraday[i].TipoMercado = this.ObterTipoMercadoInstrumento(PosicaoIntraday[i].Instrumento);
                }

                #endregion

                #region Papeis operados na Compra


                var _posicaoComprada = from p in PosicaoIntraday
                                       where p.SentidoOperacao == 'C'
                                       select p;

                lock (lstPapeisOperados)
                {
                    lstPapeisOperados.Clear();
                }

                foreach (var itemCompra in _posicaoComprada)
                {
                    string Instrumento = itemCompra.Instrumento;

                    if (itemCompra.Instrumento.ToUpper().Substring(itemCompra.Instrumento.Length - 1, 1) == "E")
                    {
                        if (!lstPapeisOperados.Contains(itemCompra.InstrumentoOpcao))
                        {
                            lstPapeisOperados.Add(itemCompra.InstrumentoOpcao);
                        }

                        if (!lstPapeisOperados.Contains(Instrumento))
                        {
                            lstPapeisOperados.Add(Instrumento);
                        }
                    }
                    else
                    {
                        if (!lstPapeisOperados.Contains(Instrumento))
                        {
                            lstPapeisOperados.Add(Instrumento);
                        }
                    }
                }


                #endregion

                #region Papeis operados na Venda

                var _posicaoVendida = from p in PosicaoIntraday
                                      where p.SentidoOperacao == 'V'
                                      select p;

                //lock (lstPapeisOperados)
                //{
                //    lstPapeisOperados.Clear();
                //}

                foreach (var itemVenda in _posicaoVendida)
                {
                    string Instrumento = itemVenda.Instrumento;

                    if (itemVenda.Instrumento.ToUpper().Substring(itemVenda.Instrumento.Length - 1, 1) == "E")
                    {
                        if (!lstPapeisOperados.Contains(itemVenda.InstrumentoOpcao))
                        {
                            lstPapeisOperados.Add(itemVenda.InstrumentoOpcao);
                        }

                        if (!lstPapeisOperados.Contains(Instrumento))
                        {
                            lstPapeisOperados.Add(Instrumento);
                        }
                    }
                    else
                    {
                        if (!lstPapeisOperados.Contains(Instrumento))
                        {
                            lstPapeisOperados.Add(Instrumento);
                        }
                    }

                }

                #endregion

                #endregion

                #region Lucro Prejuizo

                lstPapeisOperados = TratarListaInstrumentos(lstPapeisOperados);

                foreach (string item in lstPapeisOperados)
                {
                    ClienteRiscoResumo _ClienteRiscoResumo = new ClienteRiscoResumo();
                    decimal CotacaoAtual = ObterCotacaoAtual(item);

                    int QuantidadeAbertura = 0;
                    int QuantidadeTotal = 0;

                    decimal VolumeAbertura = 0;
                    decimal LucroPrejuizoCompras = 0;
                    decimal LucroPrejuizoVendas = 0;


                    //if ((item == "") || (item == "VALEE40") || (item == "BVMF3") || (item == "PETRE21"))
                    //{

                    #region Custodia Abertura


                    var PosicaoAbertura = from p in _lstCustodiaAbertura
                                          where p.Instrumento == item
                                          select p;

                    if (PosicaoAbertura.Count() > 0)
                    {
                        foreach (var ItemAbertura in PosicaoAbertura)
                        {
                            QuantidadeAbertura = ItemAbertura.Quantidade;
                            _ClienteRiscoResumo.QtdeAber = QuantidadeAbertura;
                        }

                        VolumeAbertura = (QuantidadeAbertura * ObterCotacaoAtual(item));
                    }

                    #endregion

                    #region Posicao Comprada Dia

                    var _PosicaoCompradaPapel = from p in _posicaoComprada
                                                where p.Instrumento == item
                                                select p;

                    if (_PosicaoCompradaPapel.Count() > 0)
                    {
                        OperacoesInfo _OrdemExecInfo = null;
                        
                        //decimal ValorInstrumentoMercado = 0.0M;

                        //ValorInstrumentoMercado = ObterCotacaoAtual(item);

                        foreach (var ItemComprado in _PosicaoCompradaPapel)
                        {
                            _OrdemExecInfo = new OperacoesInfo();

                            _OrdemExecInfo.Cliente       = _MonitorExposicao.CodigoBovespa.ToString();
                            _OrdemExecInfo.Instrumento   = ItemComprado.Instrumento;
                            _OrdemExecInfo.Sentido       = "C";
                            _OrdemExecInfo.Quantidade    = ItemComprado.Quantidade;
                            
                            if (ItemComprado.Instrumento.ToUpper().Substring(ItemComprado.Instrumento.Length - 1, 1) == "E")
                            {
                                _OrdemExecInfo.PrecoMercado = ObterCotacaoAtual(ItemComprado.InstrumentoOpcao);
                            }
                            else
                            {
                                _OrdemExecInfo.PrecoMercado = CotacaoAtual;
                            }

                            _OrdemExecInfo.PrecoNegocio  = ItemComprado.PrecoMedioNegocio;
                            _OrdemExecInfo.TotalNegocio  = (_OrdemExecInfo.Quantidade * _OrdemExecInfo.PrecoNegocio);
                            _OrdemExecInfo.TotalMercado  = (_OrdemExecInfo.Quantidade * _OrdemExecInfo.PrecoMercado);
                           
                            _OrdemExecInfo.LucroPrejuizo = (_OrdemExecInfo.TotalMercado - _OrdemExecInfo.TotalNegocio);
                            
                            _OrdemExecInfo.Porta         = ItemComprado.Porta;
                            LucroPrejuizoCompras        += _OrdemExecInfo.LucroPrejuizo;
                            
                            _MonitorExposicao.OrdensExecutadas.Add(_OrdemExecInfo);

                            _OrdemExecInfo = null;

                        }

                    }

                    #endregion

                    #region Posicao Vendida Dia

                    var _PosicaoVendidaPapel = from p in _posicaoVendida
                                               where p.Instrumento == item
                                               select p;

                    if (_PosicaoVendidaPapel.Count() > 0)
                    {
                        OperacoesInfo _OrdemExecInfo = null;

                        //decimal ValorInstrumentoMercado = 0.0M;

                        //ValorInstrumentoMercado = ObterCotacaoAtual(item);

                        foreach (var ItemVendido in _PosicaoVendidaPapel)
                        {
                            _OrdemExecInfo = new OperacoesInfo();

                            _OrdemExecInfo.Cliente       = _MonitorExposicao.CodigoBovespa.ToString();
                            _OrdemExecInfo.Instrumento   = ItemVendido.Instrumento;
                            _OrdemExecInfo.Sentido       = "V";
                            _OrdemExecInfo.Quantidade    = ItemVendido.Quantidade;
                            
                            //_OrdemExecInfo.PrecoMercado  = CotacaoAtual;

                            if (ItemVendido.Instrumento.ToUpper().Substring(ItemVendido.Instrumento.Length - 1, 1) == "E")
                            {
                                _OrdemExecInfo.PrecoMercado = ObterCotacaoAtual(ItemVendido.InstrumentoOpcao);
                            }
                            else
                            {
                                _OrdemExecInfo.PrecoMercado = CotacaoAtual;
                            }

                            _OrdemExecInfo.PrecoNegocio  = ItemVendido.PrecoMedioNegocio;
                            _OrdemExecInfo.TotalNegocio  = (_OrdemExecInfo.Quantidade * _OrdemExecInfo.PrecoNegocio);
                            _OrdemExecInfo.TotalMercado  = (_OrdemExecInfo.Quantidade * _OrdemExecInfo.PrecoMercado);
                            _OrdemExecInfo.LucroPrejuizo = (_OrdemExecInfo.TotalNegocio - _OrdemExecInfo.TotalMercado);
                            _OrdemExecInfo.Porta         = ItemVendido.Porta;
                            LucroPrejuizoVendas         += _OrdemExecInfo.LucroPrejuizo;

                            _MonitorExposicao.OrdensExecutadas.Add(_OrdemExecInfo);

                            _OrdemExecInfo = null;
                        }

                    }

                    #endregion

                    #region Totais de Compra (Somatoria)

                    var ExecucaoCompras = from p in _MonitorExposicao.OrdensExecutadas
                                          where p.Sentido == "C"
                                          && p.Instrumento == item
                                          select p;

                    if (ExecucaoCompras.Count() > 0)
                    {
                        foreach (var itemCompra in ExecucaoCompras)
                        {
                            _ClienteRiscoResumo.QtdeComprada += itemCompra.Quantidade;
                            _ClienteRiscoResumo.FinanceiroComprado += itemCompra.TotalNegocio;
                            _ClienteRiscoResumo.VLNegocioCompra += itemCompra.TotalNegocio;
                            _ClienteRiscoResumo.VLMercadoCompra += itemCompra.TotalMercado;


                            CustodiaValorizadaCompraIntraday += itemCompra.TotalMercado;
                        }
                    }

                    #endregion

                    #region Totais de Venda (Somatoria)

                    var ExecucaoVendas = from p in _MonitorExposicao.OrdensExecutadas
                                         where p.Sentido == "V"
                                          && p.Instrumento == item
                                         select p;

                    if (ExecucaoVendas.Count() > 0)
                    {
                        foreach (var itemVenda in ExecucaoVendas)
                        {
                            _ClienteRiscoResumo.QtdeVendida += itemVenda.Quantidade;
                            _ClienteRiscoResumo.FinanceiroVendido += itemVenda.TotalNegocio;
                            _ClienteRiscoResumo.VLNegocioVenda += itemVenda.TotalNegocio;
                            _ClienteRiscoResumo.VLMercadoVenda += itemVenda.TotalMercado;

                            CustodiaValorizadaVendaIntraday += itemVenda.TotalMercado;
                        }
                    }

                    #endregion

                    #region Lucro Prejuizo Acumulado

                    decimal _LucroPrejuizoAcum = (LucroPrejuizoCompras + LucroPrejuizoVendas);

                    QuantidadeTotal = (_ClienteRiscoResumo.QtdeComprada - _ClienteRiscoResumo.QtdeVendida);

                    _ClienteRiscoResumo.QtdeAtual = QuantidadeTotal;

                    decimal TotalCompraCalculado = (_ClienteRiscoResumo.VLMercadoCompra - _ClienteRiscoResumo.VLNegocioCompra);
                    decimal TotalVendaCalculado = (_ClienteRiscoResumo.VLNegocioVenda - _ClienteRiscoResumo.VLMercadoVenda);

                    if (QuantidadeTotal == 0)
                    {
                        _ClienteRiscoResumo.LucroPrejuizo = _LucroPrejuizoAcum;
                    }
                    else
                    {
                        _ClienteRiscoResumo.LucroPrejuizo = _LucroPrejuizoAcum; //(TotalVendaCalculado + TotalCompraCalculado);
                    }


                    #endregion

                    _ClienteRiscoResumo.Cotacao            = ObterCotacaoAtual(item);
                    _ClienteRiscoResumo.Cliente            = CodigoBovespa;
                    _ClienteRiscoResumo.Instrumento        = item;
                    _ClienteRiscoResumo.FinanceiroAbertura = VolumeAbertura;
                    _ClienteRiscoResumo.FinanceiroComprado = _ClienteRiscoResumo.VLNegocioCompra;
                    _ClienteRiscoResumo.FinanceiroVendido  = _ClienteRiscoResumo.VLNegocioVenda;
                    _ClienteRiscoResumo.NetOperacao        = (_ClienteRiscoResumo.FinanceiroVendido - _ClienteRiscoResumo.FinanceiroComprado);

                    _ClienteRiscoResumo.TipoMercado = this.ObterTipoMercadoInstrumento(_ClienteRiscoResumo.Instrumento);

                    if (_ClienteRiscoResumo.QtdeAtual != 0)
                    {
                        if (CotacaoAtual > 0)
                        {
                            _ClienteRiscoResumo.QtReversao = Math.Round((Math.Abs(_ClienteRiscoResumo.LucroPrejuizo) / CotacaoAtual), 0) + 1;

                            if (_ClienteRiscoResumo.QtReversao < 1)
                            {
                                _ClienteRiscoResumo.QtReversao = 1;
                            }
                            _ClienteRiscoResumo.PrecoReversao = CotacaoAtual;
                        }

                        if (_ClienteRiscoResumo.QtdeAtual > 0)
                        {
                            _ClienteRiscoResumo.QtReversao = (_ClienteRiscoResumo.LucroPrejuizo * -1);
                            if (_ClienteRiscoResumo.QtReversao < 1)
                            {
                                _ClienteRiscoResumo.QtReversao = 1;
                            }
                            _ClienteRiscoResumo.PrecoReversao = CotacaoAtual;
                        }
                    }
                    else
                    {
                        _ClienteRiscoResumo.QtReversao = 0;
                        _ClienteRiscoResumo.PrecoReversao = 0;
                    }

                    _MonitorExposicao.NetOperacoes += (_ClienteRiscoResumo.FinanceiroComprado - _ClienteRiscoResumo.FinanceiroVendido);

                    _MonitorExposicao.VolumeTotalFinanceiroBov += (_ClienteRiscoResumo.FinanceiroComprado + _ClienteRiscoResumo.FinanceiroVendido);

                    _ClienteResumoLista.Add(_ClienteRiscoResumo);
                    _ClienteRiscoResumo = null;

                    //}

                }// fim loop
                #endregion

                #region Lucro Prejuizo Consolidado

                _MonitorExposicao.Operacoes = _ClienteResumoLista;

                foreach (var item in _ClienteResumoLista)
                {
                    LucroPrejuizoAcumulado += item.LucroPrejuizo;
                }
                _MonitorExposicao.LucroPrejuizoBOVESPA = LucroPrejuizoAcumulado;
                //_MonitorExposicao.LucroPrejuizoBOVESPA += CustodiaValorizadaAbertura;

                #region Garantias Bovespa/Bmf
                {
                    decimal lGarantiaBmf = 0.0M;
                    decimal lGarantiaBov = 0.0M;
                    decimal lMargemRequeridaBmf = 0.0M;

                    _MonitorExposicao.TotalGarantias = ObterPosicaoGarantias(_ClienteInfo.CodigoBovespa, _ClienteInfo.CodigoBMF, ref  lGarantiaBov, ref lGarantiaBmf, ref lMargemRequeridaBmf);

                    _MonitorExposicao.PLAberturaBMF += (lGarantiaBmf); // Garantia só de dinheiro de Bmf
                    _MonitorExposicao.PLAberturaBovespa += (lGarantiaBov); // Garantia só de dinheiro de Bovespa

                    //_MonitorExposicao.PatrimonioLiquidoTempoReal += (lGarantiaBmf + lGarantiaBov);
                }
                #endregion

                _MonitorExposicao.LucroPrejuizoTOTAL = (_MonitorExposicao.LucroPrejuizoBOVESPA + (_MonitorExposicao.LucroPrejuizoBMF)); //_MonitorExposicao.PLAberturaBMF --> Cido pediu para retirar 

                #endregion

                #region PatrimonioLiquidoTempoReal

                decimal PLCustodia = ((CustodiaValorizadaVendaIntraday - CustodiaValorizadaCompraIntraday) + CustodiaValorizadaAbertura);
                decimal ContaCorrenteProjetado = (_MonitorExposicao.ContaCorrenteAbertura + (CustodiaValorizadaVendaIntraday - CustodiaValorizadaCompraIntraday));

                _MonitorExposicao.PLAberturaBovespa = (CustodiaValorizadaAbertura + _MonitorExposicao.ContaCorrenteAbertura);
                //_MonitorExposicao.PatrimonioLiquidoTempoReal = ((ContaCorrenteProjetado + (CustodiaValorizadaAbertura)) + (_MonitorExposicao.PLAberturaBMF + _MonitorExposicao.LucroPrejuizoBMF));

                decimal ProjetadoOnLine = (_ContaCorrenteInfo.SaldoD0 + _ContaCorrenteInfo.SaldoD1 + _ContaCorrenteInfo.SaldoD2 + _ContaCorrenteInfo.SaldoD3);
                decimal LucroPrej = (ProjetadoOnLine +
                                 CustodiaValorizadaAbertura +
                                 (CustodiaValorizadaCompraIntraday - CustodiaValorizadaVendaIntraday) +
                                 _MonitorExposicao.LucroPrejuizoBMF + 
                                 _MonitorExposicao.SaldoBMF);

                //_MonitorExposicao.LucroPrejuizoBMF += PosicaoAberturaDia ;

                _MonitorExposicao.SaldoBMF += _MonitorExposicao.LucroPrejuizoBMF;

                LucroPrej += _MonitorExposicao.TotalFundos + _MonitorExposicao.TotalClubes + PosicaoAberturaDia;

                #endregion

                #region Outras Informações consolidadas

                _MonitorExposicao.PLAberturaBovespa = (_MonitorExposicao.ContaCorrenteAbertura + _MonitorExposicao.CustodiaAbertura);
                _MonitorExposicao.TotalContaCorrenteTempoReal = ContaCorrenteProjetado;

                #endregion

                #region PatrimonioLiquidoTempoReal

                decimal lTotalGarantiasSemMargem = ObterPosicaoGarantiaSemMargem(_ClienteInfo.CodigoBovespa, _ClienteInfo.CodigoBMF);

                _MonitorExposicao.PatrimonioLiquidoTempoReal = ProjetadoOnLine +
                                _MonitorExposicao.TotalFundos +
                                lPosicaoTesouro +
                                 CustodiaValorizadaAbertura        +
                                 (CustodiaValorizadaCompraIntraday - CustodiaValorizadaVendaIntraday) + 
                                 _MonitorExposicao.LucroPrejuizoBMF + 
                                 lTotalGarantiasSemMargem;
                // --> comentado bug referente 2011 - Custodia Fundos + BVSP + BM&F (Posição DIA BMF) + TD – todos os prejuízos + saldo CC até D3 + garantias em $$. 

                #endregion

                #region Semaforo e proporcao de prejuiso

                decimal PercentualPrejuizo = 0;
                decimal ProporcaoPrejuizo = 0;



                if (_MonitorExposicao.CodigoBovespa !=0)
                {
                    new PersistenciaMonitorRisco().AtualizaPosicaoRiscoLucroPrejuizo(_MonitorExposicao.CodigoBovespa, _MonitorExposicao.LucroPrejuizoTOTAL, _MonitorExposicao.PatrimonioLiquidoTempoReal);
                }

                if (_MonitorExposicao.CodigoBMF != 0)
                {
                    new PersistenciaMonitorRisco().AtualizaPosicaoRiscoLucroPrejuizo(_MonitorExposicao.CodigoBMF, _MonitorExposicao.LucroPrejuizoTOTAL, _MonitorExposicao.PatrimonioLiquidoTempoReal);
                }

                if (_MonitorExposicao.LucroPrejuizoTOTAL < 0)
                {

                    PercentualPrejuizo = ((_MonitorExposicao.LucroPrejuizoTOTAL / _MonitorExposicao.PatrimonioLiquidoTempoReal) * 100);

                    if ((_MonitorExposicao.LucroPrejuizoTOTAL < 0) && (_MonitorExposicao.PatrimonioLiquidoTempoReal < 0))
                    {
                        PercentualPrejuizo = -100;
                        _MonitorExposicao.PercentualPrejuizo = PercentualPrejuizo;
                    }
                    else
                    {
                        _MonitorExposicao.PercentualPrejuizo = PercentualPrejuizo;
                    }

                    _MonitorExposicao.PercentualPrejuizo = Math.Round(_MonitorExposicao.PercentualPrejuizo, 2);

                    ProporcaoPrejuizo = Math.Round(_MonitorExposicao.LucroPrejuizoTOTAL, 2);

                    if (ProporcaoPrejuizo < 0)
                    {
                        decimal lTempProporcaoPrejuizo = Math.Abs(ProporcaoPrejuizo);

                        if (lTempProporcaoPrejuizo < 2000)
                        {
                            _MonitorExposicao.ProporcaoLucroPrejuizo = EnumProporcaoPrejuizo.ATE2K;
                        }
                        if (lTempProporcaoPrejuizo >= 2000)
                        {
                            _MonitorExposicao.ProporcaoLucroPrejuizo = EnumProporcaoPrejuizo.MAIORQUE2K;
                        }
                        if (lTempProporcaoPrejuizo >= 5000)
                        {
                            _MonitorExposicao.ProporcaoLucroPrejuizo = EnumProporcaoPrejuizo.MAIORQUE5K;
                        }
                        if (lTempProporcaoPrejuizo >= 10000)
                        {
                            _MonitorExposicao.ProporcaoLucroPrejuizo = EnumProporcaoPrejuizo.MAIORQUE10K;
                        }
                        if (lTempProporcaoPrejuizo >= 15000)
                        {
                            _MonitorExposicao.ProporcaoLucroPrejuizo = EnumProporcaoPrejuizo.MAIORQUE15K;
                        }
                        if (lTempProporcaoPrejuizo >= 20000)
                        {
                            _MonitorExposicao.ProporcaoLucroPrejuizo = EnumProporcaoPrejuizo.MAIORQUE20K;
                        }
                    }

                    if (PercentualPrejuizo < 0)
                    {
                        decimal lTempPercentualPrejuiso = Math.Abs(PercentualPrejuizo);

                        if (lTempPercentualPrejuiso < 20)
                        {
                            _MonitorExposicao.Semaforo = EnumSemaforo.VERDE;
                        }
                        if ((lTempPercentualPrejuiso >= 20) && (lTempPercentualPrejuiso < 70))
                        {
                            _MonitorExposicao.Semaforo = EnumSemaforo.AMARELO;
                        }
                        if (lTempPercentualPrejuiso > 70)
                        {
                            _MonitorExposicao.Semaforo = EnumSemaforo.VERMELHO;
                        }
                    }
                }
                else
                {
                    if (_MonitorExposicao.PatrimonioLiquidoTempoReal != 0)
                    {
                        PercentualPrejuizo = ((_MonitorExposicao.LucroPrejuizoTOTAL / _MonitorExposicao.PatrimonioLiquidoTempoReal) * 100);
                    }
                    else
                    {
                        PercentualPrejuizo = 100;
                    }

                    if (_MonitorExposicao.PatrimonioLiquidoTempoReal < 0)
                    {
                        PercentualPrejuizo = -100;
                        _MonitorExposicao.PercentualPrejuizo = PercentualPrejuizo;
                    }
                    else
                    {
                        _MonitorExposicao.PercentualPrejuizo = PercentualPrejuizo;
                    }

                    //_MonitorExposicao.PercentualPrejuiso = Math.Round(_MonitorExposicao.PercentualPrejuiso, 2);

                    //ProporcaoPrejuiso = Math.Round(_MonitorExposicao.LucroPrejuizoTOTAL, 2);

                    //if (ProporcaoPrejuiso < 0)
                    //{
                    //    decimal lTempProporcaoPrejuiso = Math.Abs(ProporcaoPrejuiso);

                    //    if (lTempProporcaoPrejuiso < 2000)
                    //    {
                    //        _MonitorExposicao.ProporcaoLucroPrejuiso = EnumProporcaoPrejuiso.ATE2K;
                    //    }
                    //    if (lTempProporcaoPrejuiso >= 2000)
                    //    {
                    //        _MonitorExposicao.ProporcaoLucroPrejuiso = EnumProporcaoPrejuiso.MAIORQUE2K;
                    //    }
                    //    if (lTempProporcaoPrejuiso >= 5000)
                    //    {
                    //        _MonitorExposicao.ProporcaoLucroPrejuiso = EnumProporcaoPrejuiso.MAIORQUE5K;
                    //    }
                    //    if (lTempProporcaoPrejuiso >= 10000)
                    //    {
                    //        _MonitorExposicao.ProporcaoLucroPrejuiso = EnumProporcaoPrejuiso.MAIORQUE10K;
                    //    }
                    //    if (lTempProporcaoPrejuiso >= 15000)
                    //    {
                    //        _MonitorExposicao.ProporcaoLucroPrejuiso = EnumProporcaoPrejuiso.MAIORQUE15K;
                    //    }
                    //    if (lTempProporcaoPrejuiso >= 20000)
                    //    {
                    //        _MonitorExposicao.ProporcaoLucroPrejuiso = EnumProporcaoPrejuiso.MAIORQUE20K;
                    //    }
                    //}

                    if (PercentualPrejuizo < 0)
                    {
                        decimal lTempPercentualPrejuiso = Math.Abs(PercentualPrejuizo);

                        if (lTempPercentualPrejuiso < 20)
                        {
                            _MonitorExposicao.Semaforo = EnumSemaforo.VERDE;
                        }
                        if ((lTempPercentualPrejuiso >= 20) && (lTempPercentualPrejuiso < 70))
                        {
                            _MonitorExposicao.Semaforo = EnumSemaforo.AMARELO;
                        }
                        if (lTempPercentualPrejuiso > 70)
                        {
                            _MonitorExposicao.Semaforo = EnumSemaforo.VERMELHO;
                        }
                    }
                    

                }

                #endregion

                #endregion
                logger.Debug("Recalculo Finalizado [ " + CodigoCliente.ToString() + "]");

                string TempoGasto = "Tempo gasto no processamento do calculo >> [" + CodigoCliente.ToString() + "] Dados Basicos:" + stampDadosCli.ToString() + " | Dados ContaCorrente: " + stampDadosCC.ToString() + " | Dados Custodia: " + stampDadosCus + " | Dados Intraday: " + stampDadosIntra.ToString();
                logger.Info(TempoGasto);
            }
            catch (Exception ex)
            {
                string stackTrade = ex.StackTrace;
                logger.Error("Ocorreu um erro ao recalcular a posição do cliente. " + CodigoCliente.ToString() + " - Erro: " + ex.Message, ex);
            }

            return _MonitorExposicao;
        }

        #endregion

        #region  Metodos de apoio

        /// <summary>
        /// Metodo responsavel por gerar codigos bovespa validos.
        /// </summary>
        /// <param name="BovespaRef"></param>
        /// <returns>Retorna um código de bovespa válido</returns>
        private string ConverterCodigoBovespa(string BovespaRef)
        {
            string Bovespa = string.Empty;
            long BovespaAux = long.Parse(BovespaRef);

            Bovespa =
                BovespaAux.ToString().Remove(BovespaAux.ToString().Length - 1, 1);

            return Bovespa;

        }

        /// <summary>
        /// Lista o clientes que estão sendo monitrados no momento para serem filtrados
        /// no request do ObterMonitorLucroPrejuizo do usuário na tela.
        /// Método para Listagem rápida dos clientes.
        /// </summary>
        /// <returns>Retorna o clientes que estão sendo monitorados pelo serviço </returns>
        public List<ExposicaoClienteInfo> ObterItemsMonitor()
        {
            List<ExposicaoClienteInfo> lstClientes = new List<ExposicaoClienteInfo>();
            IEnumerator denum;
            KeyValuePair<int, ExposicaoClienteInfo> dentry;

            //lock (MonitorLucroPrejuizo)
            //{
            denum = MonitorLucroPrejuizo.GetEnumerator();

            while (denum.MoveNext())
            {
                dentry = (KeyValuePair<int, ExposicaoClienteInfo>)denum.Current;
                lstClientes.Add((ExposicaoClienteInfo)(dentry.Value));
            }
            //}

            return lstClientes;
        }

        /// <summary>
        /// Método que trata a lista de instrumentos retirando o "F" dos papeis de fracionário
        /// A cotação do papel foi solicitada pelo setor de Risco da gradual 
        /// para conter apenas lote inteiros, sem o fracionário
        /// </summary>
        /// <param name="ListaInstrumentos">Lista de Instumentos para serem tratados</param>
        /// <returns>Retorna Lista de instrumentos para serem retirados os "F" no final do nome do papel fracionário</returns>
        private List<string> TratarListaInstrumentos(List<string> ListaInstrumentos)
        {
            List<string> ListaTratada = new List<string>();

            string InstrumentoAux;
            foreach (string item in ListaInstrumentos)
            {
                InstrumentoAux = item;

                if (item.Substring(item.Length - 1, 1) == "F")
                {
                    InstrumentoAux = item.Remove(item.Length - 1);
                }

                if (!ListaTratada.Contains(InstrumentoAux))
                {
                    lock (ListaTratada)
                    {
                        ListaTratada.Add(InstrumentoAux);
                    }
                }

            }

            return ListaTratada;

        }
        
        #region Cotação Atual


        /// <summary>
        /// Método responsavel por obter a ultima cotação de um determinado instrumento.
        /// </summary>
        /// <param name="Instument"></param>
        /// <returns></returns>
        private decimal ObterCotacaoAtual(string Instument)
        {
            //lock (CotacaoAtual)
            //{

                PersistenciaMonitorRisco.CotacaoValor lRetorno = new PersistenciaMonitorRisco.CotacaoValor();

                //if (CotacaoAtual.Contains(Instument))
                //{
                    lRetorno = new PersistenciaMonitorRisco().ObterCotacaoAtual(Instument);   

                    //return CotacaoAtual[Instument].DBToDecimal();

                    return lRetorno.ValorCotacao;
               // }
            //}

            //return 0;
        }

        /// <summary>
        /// Método responsável por obter o ajuste bmf de um determinado instrumento
        /// </summary>
        /// <param name="pInstrumento">Instrumento solicitado</param>
        /// <returns>Retorna do banco de dados o valor de ajuste de instrmento bmf</returns>
        private decimal ObterAjusteBmf(string pInstrumento)
        {
            PersistenciaMonitorRisco.CotacaoValor lRetorno = new PersistenciaMonitorRisco.CotacaoValor();
            
            lRetorno = new PersistenciaMonitorRisco().ObterCotacaoAtual(pInstrumento);

            return lRetorno.ValorAjuste;
        }

        #endregion

        #region Cotação Fechamento
        /// <summary>
        /// Método responsável por obter a cotação de abertura de um determinado instrumento.
        /// </summary>
        /// <param name="Instument">Instrumento para obter a cotação de abertura</param>
        /// <returns></returns>
        private decimal ObterCotacaoAberturaInstrumento(string Instument)
        {
            lock (CotacaoAbertura)
            {
                if (CotacaoAbertura.Contains(Instument))
                {
                    return CotacaoAbertura[Instument].DBToDecimal();
                }
            }

            return 0;
        }

        /// <summary>
        /// Método responsavel por obter a cotação de todos os fechamentos
        /// </summary>
        private Hashtable ObterCotacaoFechamento()
        {
            lock (CotacaoFechamento)
            {
                CotacaoFechamento.Clear();
                CotacaoFechamento = new PersistenciaMonitorRisco().ObtemCotacaoFechamento();
            }

            return CotacaoFechamento;

        }

        /// <summary>
        /// Método que busca na hashtable MercadoInstrumento o Tipo de mercado 
        /// do Instrumento: OPC, OPV, VIS, DIS, etc. A Hashtable foi previamente carregada na inicialização do serviço.
        /// </summary>
        /// <param name="Instumento">INstrumento para buscar na lista de tipo de mercado</param>
        /// <returns>Retorna o tipo de instrumento que está na hashtable na memória </returns>
        private string ObterTipoMercadoInstrumento(string Instumento)
        {
            lock (MercadoInstumento)
            {
                if (MercadoInstumento.Contains(Instumento))
                {
                    return MercadoInstumento[Instumento].DBToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Método que busca no banco de dados do Sinacor a cotação de abertura de todos os papéis
        /// </summary>
        /// <returns>Retorna uma hashtable com todos os papéis operados e seus devidos preços de abertura.</returns>
        public Hashtable ObterCotacaoAbertura()
        {
            lock (CotacaoAbertura)
            {
                CotacaoAbertura.Clear();
                CotacaoAbertura = new PersistenciaMonitorRisco().ObtemCotacaoAbertura();
            }

            return CotacaoAbertura;
        }

        /// <summary>
        /// Método que busca no banco de dados do SQL a os valores de Ajuste de bmf de todos os instrumentos 
        /// </summary>
        /// <returns>Retorna uma hashtable com todos os papéis operados e seus devidos preços de abertura.</returns>
        public Hashtable ObterCotacaoAjuste()
        {
            lock (CotacaoAjuste)
            {
                CotacaoAjuste.Clear();
                CotacaoAjuste = new PersistenciaMonitorRisco().ObtemCotacaoAjusteBmf();
            }

            return CotacaoAjuste;
        }


        /// <summary>
        /// Método que busca no banco dados do sinacor a lista de tipo 
        /// de mercado que 
        /// </summary>
        /// <returns>Retorna mercado de Instrumento</returns>
        private Hashtable ObterMercadoInstrumento()
        {
            lock (MercadoInstumento)
            {
                MercadoInstumento.Clear();
                MercadoInstumento = new PersistenciaMonitorRisco().ObtemTipoMercadoInstrumento();
            }

            return MercadoInstumento;

        }

        #endregion

        #endregion   

        #endregion

        #region Implementação das Interfaces de RISCO e PLD

        #region RISCO Members
        /// <summary>
        /// Método que avalia e gerencia o Mutex de cada cliente.
        /// Verifica se o cliente está na hash de clientes e 
        /// </summary>
        /// <param name="IdCliente">Codigo principal do cliente</param>
        private void AddRemoveClientRunTimerProcessed(int IdCliente)
        {
            ClienteMutexInfo StateObj = new ClienteMutexInfo();

            StateObj.TimerCanceled     = false;
            StateObj.SomeValue         = 1;
            StateObj.IdCliente         = IdCliente;
            StateObj.StatusProcessando = EnumProcessamento.LIVRE;
            //StateObj.FirstTimeProcessed = DateTime.Now;
            StateObj.Intervalo         = IntervaloRecalculo;

            //lock (htClientes)
            //{
                if (htClientes.ContainsKey(IdCliente))
                {
                    var ClienteMutex = new ClienteMutexInfo();

                    ClienteMutex = htClientes[IdCliente] as ClienteMutexInfo;

                    StateObj.SomeValue = ClienteMutex.SomeValue;
                    StateObj.FirstTimeProcessed = ClienteMutex.FirstTimeProcessed;

                    double lMinutes = (DateTime.Now - ClienteMutex.FirstTimeProcessed).TotalMinutes;

                    logger.InfoFormat("FirstTimeProcessed {0} - Datetime. Now {1}", ClienteMutex.FirstTimeProcessed, DateTime.Now);

                    logger.InfoFormat("Consulta do Cliente {1} - Ultimo acesso {0}", lMinutes, IdCliente);

                    if (lMinutes > 3)
                    {
                        logger.InfoFormat("Já passou 3 minutos da ultima consulta do Cliente {1} - Ultimo acesso {0}", lMinutes, IdCliente);

                        StateObj.FirstTimeProcessed = DateTime.Now;

                        //htClientes.TryRemove(IdCliente, out StateObj);

                        if (ClientesMonitor.Contains(IdCliente))
                        {
                            lock (ClientesMonitor)
                            {
                                ClientesMonitor.Remove(IdCliente);
                            }
                        }

                        //htClientes.TryRemove(IdCliente, out StateObj);
                        //lock (ClientesMonitor)
                        //{
                        //    if (MonitorLucroPrejuizo.ContainsKey(IdCliente) && !ClientesMonitor.Contains(IdCliente))
                        //    {
                        //        //MonitorLucroPrejuizo.Remove(IdCliente);
                        //    }
                        //}
                    }
                    else
                    {
                        if (!ClientesMonitor.Contains(IdCliente))
                        {
                            lock (ClientesMonitor)
                            {
                                ClientesMonitor.Add(IdCliente);
                            }
                        }
                        //StateObj.FirstTimeProcessed = DateTime.Now;
                        htClientes.TryRemove(IdCliente, out StateObj);
                    }
                }
                else
                {
                    StateObj.FirstTimeProcessed = DateTime.Now;
                }

                htClientes.AddOrUpdate(IdCliente, StateObj, (key, oldValue)=> StateObj);
            //}
        }

        /// <summary>
        /// Método principal que é usado para Obter e rever a exposição do cliente
        /// Garante que retorna a posição mesmo se o cliente não estiver sendo monitorado no momento do request.
        /// Se o cliente NÃO foi recalculado no monitoramento nos ultimos minutos configurados e estipulados no intervalo de tempo,
        /// o método garante que o cliente seja recalculado novamente
        /// </summary>
        /// <param name="pRequest">Objeto de Request do Monitor de Lucro Prejuízo</param>
        /// <returns>Retorna a exposição do cliente para poder </returns>
        public MonitorLucroPrejuizoResponse ObterMonitorLucroPrejuizo(MonitorLucroPrejuizoRequest pRequest)
        {
            logger.Debug("Solicitação de consulta [ ObterMonitorLucroPrejuizo ] requisitada. Cliente = " + pRequest.Cliente.ToString() + " Assessor = " + pRequest.Assessor.ToString());
            MonitorLucroPrejuizoResponse _response = new MonitorLucroPrejuizoResponse();
   
            try
            {
                #region Monitor Completo
                
                logger.Debug("Nao estava monitorando [" + pRequest.Cliente + "]");

                ClienteInfo _ClienteInfo = new PersistenciaMonitorRisco().ObterDadosCliente(pRequest.Cliente);

                if ((_ClienteInfo.CodigoBovespa != string.Empty)    || 
                    (_ClienteInfo.CodigoBMF != string.Empty)        ||
                     pRequest.Cliente != 0)
                {
                    this.AddRemoveClientRunTimerProcessed(pRequest.Cliente);

                    if (MonitorLucroPrejuizo.ContainsKey(pRequest.Cliente))
                    {
                        if (!ClientesMonitor.Contains(pRequest.Cliente))
                        {
                            logger.InfoFormat("O cliente [{0}] não estava sendo monitorado", pRequest.Cliente);

                            ExposicaoClienteInfo lInfoPosicao = this.CalcularPosicao(pRequest.Cliente);

                            MonitorLucroPrejuizo.AddOrUpdate(lInfoPosicao.CodigoBovespa, lInfoPosicao, (key, oldValue)=>  lInfoPosicao);
                        }
                        else
                        {
                            ExposicaoClienteInfo lInfoPosicao = MonitorLucroPrejuizo[pRequest.Cliente] as ExposicaoClienteInfo;

                            logger.InfoFormat("Pegou posicao [" + pRequest.Cliente + "] da memoria");
                        }
                    }
                    else
                    {
                        logger.Debug("Recalcular posicao [" + pRequest.Cliente + "] novamente");
                                
                        ExposicaoClienteInfo lInfoPosicao = this.CalcularPosicao(pRequest.Cliente);

                        MonitorLucroPrejuizo.AddOrUpdate(lInfoPosicao.CodigoBovespa, lInfoPosicao, (key, oldValue) => lInfoPosicao);
                        //lock (MonitorLucroPrejuizo)
                        //{
                        //    MonitorLucroPrejuizo.Add(lInfoPosicao.CodigoBovespa, lInfoPosicao);
                        //}
                    }

                    logger.InfoFormat("**************************************************************************************");
                    logger.InfoFormat("*******Total de calculos efetuados na memória [{0}]", MonitorLucroPrejuizo.Count);
                    logger.InfoFormat("**************************************************************************************");
                }

                List<ExposicaoClienteInfo> lstClientes = this.ObterItemsMonitor();

                if ((pRequest.Cliente == 0) && (pRequest.Assessor == 0))
                {
                    if (lstClientes.Count > 0)
                    {
                        #region Verifica Semaforo

                        if (pRequest.Semaforo != 0)
                        {
                            var Items = from p in lstClientes                                    
                                        where p.Semaforo == pRequest.Semaforo
                                        select p;

                            if (Items.Count() > 0)
                            {
                                foreach (ExposicaoClienteInfo info in Items)
                                {
                                    _response.Monitor.Add(info);
                                }
                            }
                        }

                        #endregion

                        #region Verifica Proporcao Lucro/Prejuiso

                        if (pRequest.ProporcaoPrejuizo != 0)
                        {
                            var Items = from p in lstClientes                                       
                                        where p.ProporcaoLucroPrejuizo == pRequest.ProporcaoPrejuizo
                                        select p;

                            if (Items.Count() > 0)
                            {
                                foreach (ExposicaoClienteInfo info in Items)
                                {
                                    _response.Monitor.Add(info);
                                }
                            }
                        }

                        #endregion

                        #region Completo

                        if ((pRequest.ProporcaoPrejuizo == 0) && (pRequest.Semaforo == 0))
                        {
                            foreach (var item in lstClientes)
                            {
                                _response.Monitor.Add(item);
                            }
                        }

                        #endregion
                    }

                    _response = this.PaginacaoObterMonitorLucroPrejuizo(pRequest, _response);

                    return _response;
                }

                #endregion

                #region Pesquisa por Cliente e Assessor

                if  ((pRequest.Cliente != 0) && (pRequest.Assessor != 0))
                {
                    List<string> lAssessoresVinculados = new PersistenciaMonitorRisco().ListarAssessoresVinculados(pRequest.Assessor, pRequest.CodigoLogin);

                    if (lstClientes.Count > 0)
                    {
                        #region Verifica Semaforo
                        if (pRequest.Semaforo != 0)
                        {
                            var Items = from p in lstClientes
                                        where p.Semaforo == pRequest.Semaforo
                                        && lAssessoresVinculados.Contains(p.Assessor)   //== pRequest.Assessor.ToString()
                                        && (p.CodigoBovespa == pRequest.Cliente || p.CodigoBMF == pRequest.Cliente) 
                                        select p;

                            if (Items.Count() > 0)
                            {
                                foreach (ExposicaoClienteInfo info in Items)
                                {
                                    _response.Monitor.Add(info);
                                }
                            }
                        }
                        #endregion

                        #region Verifica Proporcao Lucro/Prejuiso
                        if (pRequest.ProporcaoPrejuizo != 0)
                        {
                            var Items = from p in lstClientes
                                        where p.ProporcaoLucroPrejuizo == pRequest.ProporcaoPrejuizo
                                        && lAssessoresVinculados.Contains(p.Assessor) //p.Assessor == pRequest.Assessor.ToString()
                                        && (p.CodigoBovespa == pRequest.Cliente || p.CodigoBMF == pRequest.Cliente) 
                                        select p;

                            if (Items.Count() > 0)
                            {
                                foreach (ExposicaoClienteInfo info in Items)
                                {
                                    _response.Monitor.Add(info);
                                }
                            }
                        }
                        #endregion

                        #region cLiente & Assessor
                        if ((pRequest.ProporcaoPrejuizo == 0) && (pRequest.Semaforo == 0))
                        {

                            var Items = from p in lstClientes
                                        where lAssessoresVinculados.Contains(p.Assessor)// p.Assessor == pRequest.Assessor.ToString()
                                        && (p.CodigoBovespa == pRequest.Cliente || p.CodigoBMF == pRequest.Cliente) 
                                        select p;

                            if (Items.Count() > 0)
                            {
                                foreach (ExposicaoClienteInfo info in Items)
                                {
                                    _response.Monitor.Add(info);
                                }
                            }
                        }
                        #endregion
                    }

                    _response = this.PaginacaoObterMonitorLucroPrejuizo(pRequest, _response);

                    return _response;
                }
                #endregion

                #region Pesquisa por assessor

                if ((pRequest.Assessor != 0) && (pRequest.Cliente == 0))
                {
                    List<string> lAssessoresVinculados = new PersistenciaMonitorRisco().ListarAssessoresVinculados(pRequest.Assessor, pRequest.CodigoLogin);

                    if (lstClientes.Count > 0)
                    {
                        #region Verifica Semaforo

                        if (pRequest.Semaforo != 0)
                        {
                            var Items = from p in lstClientes
                                        where p.Semaforo == pRequest.Semaforo
                                        && lAssessoresVinculados.Contains(p.Assessor)// p.Assessor == pRequest.Assessor.ToString()                                   
                                        select p;

                            if (Items.Count() > 0)
                            {
                                foreach (ExposicaoClienteInfo info in Items)
                                {
                                    _response.Monitor.Add(info);
                                }
                            }
                        }

                        #endregion

                        #region Verifica Proporcao Lucro/Prejuiso

                        if (pRequest.ProporcaoPrejuizo != 0)
                        {
                            var Items = from p in lstClientes
                                        where p.ProporcaoLucroPrejuizo == pRequest.ProporcaoPrejuizo
                                        && lAssessoresVinculados.Contains(p.Assessor)//p.Assessor == pRequest.Assessor.ToString()                                    
                                        select p;

                            if (Items.Count() > 0)
                            {
                                foreach (ExposicaoClienteInfo info in Items)
                                {
                                    _response.Monitor.Add(info);
                                }
                            }
                        }

                        #endregion

                        #region Assessor
                        if ((pRequest.ProporcaoPrejuizo == 0) && (pRequest.Semaforo == 0))
                        {
                            var Items = from p in lstClientes
                                        where lAssessoresVinculados.Contains(p.Assessor)//p.Assessor == pRequest.Assessor.ToString()
                                        select p;

                            if (Items.Count() > 0)
                            {
                                foreach (ExposicaoClienteInfo info in Items)
                                {
                                    _response.Monitor.Add(info);
                                }
                            }
                        }
                        #endregion
                    }

                    _response = this.PaginacaoObterMonitorLucroPrejuizo(pRequest, _response);

                    return _response;
                }

                #endregion

                #region Pesquisa por Cliente e Assessor

                if ((pRequest.Assessor == 0) && (pRequest.Cliente != 0))
                {
                    if (lstClientes.Count > 0)
                    {
                        #region Verifica Semaforo

                        if (pRequest.Semaforo != 0)
                        {
                            var Items = from p in lstClientes
                                        where p.Semaforo == pRequest.Semaforo
                                        &&(p.CodigoBovespa == pRequest.Cliente || p.CodigoBMF == pRequest.Cliente)
                                        select p;

                            if (Items.Count() > 0)
                            {
                                foreach (ExposicaoClienteInfo info in Items)
                                {
                                    _response.Monitor.Add(info);
                                }
                            }
                        }

                        #endregion

                        #region Verifica Proporcao Lucro/Prejuiso

                        if (pRequest.ProporcaoPrejuizo != 0)
                        {
                            var Items = from p in lstClientes
                                        where p.ProporcaoLucroPrejuizo == pRequest.ProporcaoPrejuizo
                                        && (p.CodigoBovespa == pRequest.Cliente || p.CodigoBMF == pRequest.Cliente)

                                        select p;

                            if (Items.Count() > 0)
                            {
                                foreach (ExposicaoClienteInfo info in Items)
                                {
                                    _response.Monitor.Add(info);
                                }
                            }
                        }

                        #endregion

                        #region Consulta por Cliente

                        if ((pRequest.ProporcaoPrejuizo == 0) && (pRequest.Semaforo == 0))
                        {
                            var Items = from p in lstClientes
                                        where   (p.CodigoBovespa == pRequest.Cliente || p.CodigoBMF == pRequest.Cliente)
                                        select p;

                            if (Items.Count() > 0)
                            {
                                foreach (ExposicaoClienteInfo info in Items)
                                {
                                    _response.Monitor.Add(info);
                                }
                            }

                        }
                        #endregion
                    }
                    
                    _response = this.PaginacaoObterMonitorLucroPrejuizo(pRequest, _response);

                    return _response;
                }

                #endregion

            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao acessar o metodo ObterMonitorLucroPrejuizo.", ex);
            }
            return _response;              
        }

        /// <summary>
        /// Método que pagina os registros de exposição de todos os clientes sendo monitorados.
        /// Foi preciso paginar, pois o tamanho da mensagem que retorna de resposta para intranet gerava 
        /// um tamanho maior que o limite do wcf suporta
        /// </summary>
        /// <param name="pRequest">Objeto de Request do monitor de lucro prejuizo</param>
        /// <param name="pResponse">Objeto de Response do monitor de Lucro Prejuízo</param>
        /// <returns>Retorna o objeto de response já paginado em tamanho de 50 posições por página</returns>
        private MonitorLucroPrejuizoResponse PaginacaoObterMonitorLucroPrejuizo (MonitorLucroPrejuizoRequest pRequest,  MonitorLucroPrejuizoResponse pResponse)
        {
            MonitorLucroPrejuizoResponse lRetorno = new MonitorLucroPrejuizoResponse();

            lRetorno.Monitor = new List<ExposicaoClienteInfo>();

            int lPagina = 0;

            if (pRequest.NovoRange.HasValue)
            {
                lPagina = pRequest.NovoRange.Value * 50;
            }

            lRetorno.TotalRegistros = pResponse.Monitor.Count;

            logger.Info(" Encontrou um total de " + lRetorno.TotalRegistros + " registros na consulta");

            if (pResponse.Monitor != null && pResponse.Monitor.Count > 0 && lPagina != 0 && pResponse.Monitor.Count > lPagina)
            {
                lRetorno.Monitor = pResponse.Monitor.Skip(lPagina).Take(50).ToList();

                logger.Info("Número " + lPagina + " paginação da consulta");
            }
            else
            {
                lPagina = 50;

                lRetorno.Monitor = pResponse.Monitor.Take(lPagina).ToList();

                logger.Info("1º paginação da consulta");
            }

            return lRetorno;
        }

        #endregion

        #region PLD Members
        /// <summary>
        /// Método para listar Monitor de PLD (Prevenção de Lavagem de Dinheiro) que está na memória e 
        /// efetua o filtro de acrodo com o request solicitado
        /// </summary>
        /// <param name="pRequest">Request para o método efetuar o filtro dentro da List que está na memória</param>
        /// <returns>Retorna Lista Filtrada de PLDs</returns>
        public MonitorPLDResponse ObterMonitorPLD(MonitorPLDRequest pRequest)
        {
            MonitorPLDResponse _MonitorPLDResponse = new MonitorPLDResponse();
             List<PLDOperacaoInfo> lstPLD = new List<PLDOperacaoInfo>();

            lock(ClientesPLD){
                 lstPLD = ClientesPLD;
            }

            _MonitorPLDResponse.lstPLD.Clear();

            try
            {
                switch (pRequest.EnumStatusPLD)
                {
                    case EnumStatusPLD.COMPLETO:

                        #region Completo

                        if ((pRequest.Instrumento != string.Empty) && (pRequest.NumeroNegocio != 0))
                        {
                            var lstCompleto = from p in lstPLD
                                              where p.Intrumento == pRequest.Instrumento
                                              && p.STATUS == PLDANALISE
                                                || p.STATUS == PLDREJEITADO     
                                              select p;

                            if (lstCompleto.Count() > 0)
                            {
                                foreach (var item in lstCompleto)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em aprovados");
                            }
                        }

                        if ((pRequest.Instrumento != null) && (pRequest.NumeroNegocio != 0))
                        {
                            var lstCompleto = from p in lstPLD
                                              where p.Intrumento == pRequest.Instrumento
                                              && p.NumeroNegocio == pRequest.NumeroNegocio
                                              && p.STATUS == PLDANALISE
                                                || p.STATUS == PLDREJEITADO   
                                              select p;

                            if (lstCompleto.Count() > 0)
                            {
                                foreach (var item in lstCompleto)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em aprovados");
                            }
                        }

                        if ((pRequest.Instrumento == null) && (pRequest.NumeroNegocio == 0))
                        {
                            var lstPLDRetorno = from p in lstPLD
                                                where p.STATUS == PLDANALISE
                                                || p.STATUS == PLDREJEITADO
                                                select p;

                            if (lstPLDRetorno.Count() > 0)
                            {
                                foreach (var item in lstPLDRetorno)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em analise e reprovados");
                            }
                        }

                        if (pRequest.Instrumento != string.Empty)
                        {
                            var lstCompleto = from p in lstPLD
                                              where p.Intrumento == pRequest.Instrumento
                                              && (p.STATUS == PLDANALISE
                                                || p.STATUS == PLDREJEITADO)
                                              select p;

                            if (lstCompleto.Count() > 0)
                            {
                                foreach (var item in lstCompleto)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em aprovados");
                            }
                        }

                        #endregion

                        break;

                    case EnumStatusPLD.APROVADO:

                        #region Aprovado

                        if ((pRequest.Instrumento != null) && (pRequest.NumeroNegocio == 0))
                        {
                            var lstAceitos = from p in lstPLD
                                             where p.STATUS == PLDACEITO
                                             && p.Intrumento == pRequest.Instrumento
                                             select p;

                            if (lstAceitos.Count() > 0)
                            {
                                foreach (var item in lstAceitos)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em aprovados");
                            }

                        }

                        if ((pRequest.Instrumento != null) && (pRequest.NumeroNegocio != 0))
                        {
                            var lstAceitos = from p in lstPLD
                                             where p.STATUS == PLDACEITO
                                             && p.Intrumento == pRequest.Instrumento
                                             && p.NumeroNegocio == pRequest.NumeroNegocio
                                             select p;

                            if (lstAceitos.Count() > 0)
                            {
                                foreach (var item in lstAceitos)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em aprovados");
                            }
                        }

                        if ((pRequest.Instrumento == null) && (pRequest.NumeroNegocio == 0))
                        {
                            var lstAceitos = from p in lstPLD
                                             where p.STATUS == PLDACEITO
                                             select p;

                            if (lstAceitos.Count() > 0)
                            {
                                foreach (var item in lstAceitos)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em aprovados");
                            }



                        }

                        #endregion

                        break;

                    case EnumStatusPLD.REJEITADO:

                        #region [Rejeitado]

                        if ((pRequest.Instrumento != null) && (pRequest.NumeroNegocio == 0))
                        {
                            var lstRejeitado = from p in lstPLD
                                               where p.STATUS == PLDREJEITADO
                                               && p.Intrumento == pRequest.Instrumento
                                               select p;

                            if (lstRejeitado.Count() > 0)
                            {
                                foreach (var item in lstRejeitado)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em rejeitados");
                            }
                        }

                        if ((pRequest.Instrumento == null) && (pRequest.NumeroNegocio != 0))
                        {
                            var lstRejeitado = from p in lstPLD
                                               where p.STATUS == PLDREJEITADO
                                               && p.Intrumento == pRequest.Instrumento
                                               && p.NumeroNegocio == pRequest.NumeroNegocio
                                               select p;

                            if (lstRejeitado.Count() > 0)
                            {
                                foreach (var item in lstRejeitado)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em rejeitados");
                            }

                        }

                        if ((pRequest.Instrumento == null) && (pRequest.NumeroNegocio == 0))
                        {
                            var lstRejeitado = from p in lstPLD
                                               where p.STATUS == PLDREJEITADO
                                               select p;

                            if (lstRejeitado.Count() > 0)
                            {
                                foreach (var item in lstRejeitado)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em rejeitados");
                            }
                        }

                        #endregion

                        break;

                    case EnumStatusPLD.EMANALISE:

                        #region [ Analise ]

                        if ((pRequest.Instrumento != null) && (pRequest.NumeroNegocio == 0))
                        {

                            var lstAnalise = from p in lstPLD
                                             where p.STATUS == PLDANALISE
                                             && p.Intrumento == pRequest.Instrumento
                                             select p;

                            if (lstAnalise.Count() > 0)
                            {
                                foreach (var item in lstAnalise)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em rejeitados");
                            }
                        }

                        if ((pRequest.Instrumento != null) && (pRequest.NumeroNegocio != 0))
                        {
                            var lstAnalise = from p in lstPLD
                                             where p.STATUS == PLDANALISE
                                             && p.Intrumento == pRequest.Instrumento
                                             && p.NumeroNegocio == pRequest.NumeroNegocio
                                             select p;

                            if (lstAnalise.Count() > 0)
                            {
                                foreach (var item in lstAnalise)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(item);
                                }

                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em rejeitados");
                            }

                        }

                        if ((pRequest.Instrumento == null) && (pRequest.NumeroNegocio == 0))
                        {
                            for (int i = 0; i <= lstPLD.Count - 1; i++)
                            {
                                string status = lstPLD[i].STATUS;

                                if (status == PLDANALISE)
                                {
                                    _MonitorPLDResponse.lstPLD.Add(lstPLD[i]);
                                }
                                logger.Info("Foram encontrados: " + _MonitorPLDResponse.lstPLD.Count + " em analise");
                            }

                        }

                        #endregion

                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao acessar o metodo ObterMonitorPLD. ", ex);
            }

            return _MonitorPLDResponse;

        }

        #endregion

        #endregion

         
        #region IServicoControlavel Members
        /// <summary>
        /// Inicia o serviço de monitor de lucro prejuízo chamando o método StartMonitor
        /// Esse é um método padrão da Gradual.OMS.Library e é acionado sempre que o 
        /// serviço inicializa 
        /// </summary>
        public void IniciarServico()
        {
            try
            {
                _bKeepRunning = true;
                logger.Info("Inicializa o servico de Monitoramento de Risco e PLD");

                //ThreadResetPosicao = new WaitOrTimerCallback(StartMonitorEvent);

                //ThreadPool.RegisterWaitForSingleObject(lThreadEvent, ThreadResetPosicao, null, this.TemporizadorIntervaloVerificacao, false);


                logger.Info("Carrega os dados na Inicialização do Serviço.");
                StartMonitor(null);

                _ServicoStatus = ServicoStatus.EmExecucao;
                logger.Info("Inicializa o servico de Monitoramento de Risco e PLD");
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao iniciar o servico de monitoramento de risco e pld", ex);
                _ServicoStatus = ServicoStatus.Erro;
            }
        }

        /// <summary>
        /// Paraliza o serviço de monitor lucro prejuízo 
        /// Método padrão da Dll Gradual.OMS.Library que é acionado quando o serviço Está sendo parado.
        /// </summary>
        public void PararServico()
        {
            try
            {
                _bKeepRunning = false;

                logger.Info("Stop servido de monitoramento de risco e pld.");
                while (thThreadClientes.IsAlive)
                {
                    logger.Info("Aguardando finalizar ThreadClientes");
                    Thread.Sleep(250);
                }

                _ServicoStatus = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao iniciar o servico de monitoramento de risco" + ex.Message, ex);
                _ServicoStatus = ServicoStatus.Erro;
            }
        }


        /// <summary>
        /// Método que retorna o status do serviço
        /// </summary>
        /// <returns>Retorna o status do serviço de Lucro Prejuízo</returns>
        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion

        #region IServicoMonitorRisco Members

        /// <summary>
        /// Método usado para Buscar os dados do cliente no banco de dados (Sinacor)
        /// </summary>
        /// <param name="pRequest">Dados para Request</param>
        /// <returns>Retorna dados do cliente em questão com as informações do sinacor </returns>
        public ObterDadosClienteResponse ObterDadosCliente(ObterDadosClienteRequest pRequest)
        {
            ObterDadosClienteResponse lResponse = new ObterDadosClienteResponse();

            lResponse.ClienteInfos = new PersistenciaMonitorRisco().ObterDadosCliente(pRequest.CodigoCliente);

            if ((lResponse.ClienteInfos.CodigoBMF == string.Empty) || (lResponse.ClienteInfos.CodigoBMF == null))
            {

                int CodigoBmf = new PersistenciaMonitorRisco().ObterContaBMF(pRequest.CodigoCliente);

                if (CodigoBmf > 0)
                {
                    lResponse.ClienteInfos.CodigoBMF = CodigoBmf.ToString();
                }

            }

            return lResponse;
        }

        #endregion

        #region IServicoMonitorRisco Members

        /// <summary>
        /// Método que obtem a listagem de BTC do Cliente 
        /// </summary>
        /// <param name="pRequest">Request de posição de BTC</param>
        /// <returns>Retorna Lista de Posição de BTC do Cliente</returns>
        public ObterPosicaoBtcResponse ObterPosicaoBTC(ObterPosicaoBtcRequest pRequest)
        {
            ObterPosicaoBtcResponse lRetorno = new ObterPosicaoBtcResponse();

            lRetorno.PosicaoBTC = new List<BTCInfo>();

            List<BTCInfo> lstBtc = new PersistenciaMonitorRisco().ObterPosicaoBTC(pRequest.CodigoCliente);

            for (int i = 0; i <= lstBtc.Count - 1; i++)
            {
                string Instrumento = lstBtc[i].Instrumento;
                //lstBtc[i].PrecoMercado = ObterCotacaoAtual(Instrumento);

            }

            lRetorno.PosicaoBTC = lstBtc;

            return lRetorno;
        }

        /// <summary>
        /// Obtem Lista de Posição de Termo do Cliente
        /// </summary>
        /// <param name="pRequest">Objeto de Request para Posição de termo</param>
        /// <returns>Retorna uma lista de Posição do Termo do cliente</returns>
        public ObterPosicaoTermoResponse ObterPosicaoTermo(ObterPosicaoTermoRequest pRequest)
        {
            ObterPosicaoTermoResponse lRetorno = new ObterPosicaoTermoResponse();

            lRetorno.PosicaoTermo = new List<PosicaoTermoInfo>();

            List<PosicaoTermoInfo> lstTermo = new PersistenciaMonitorRisco().ObterPosicaoTermo(pRequest.CodigoCliente);

            List<PosicaoTermoInfo> lstTermoAux = new List<PosicaoTermoInfo>();

            for (int i = 0; i <= lstTermo.Count - 1; i++)
            {
                string Instrumento = lstTermo[i].Instrumento;
                decimal LucroPrejuizo = 0;

                lstTermo[i].PrecoMercado = ObterCotacaoAtual(Instrumento);

                decimal diferencial = (lstTermo[i].PrecoMercado - lstTermo[i].PrecoExecucao);
                LucroPrejuizo = (lstTermo[i].Quantidade * diferencial);

                lstTermo[i].LucroPrejuizo = LucroPrejuizo;
            }

            var ListaTermo = from p in lstTermo
                             orderby p.Instrumento, p.DataVencimento
                             select p;


            foreach (var item in lstTermo)
            {
                lstTermoAux.Add(item);
            }

            lRetorno.PosicaoTermo = lstTermoAux;

            return lRetorno;
        }

        #endregion
    }

}
  
