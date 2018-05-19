using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Monitores.Compliance.Lib;
using Gradual.Monitores.Persistencia;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using log4net;
using System.ServiceModel;
using System.Configuration;

namespace Gradual.Monitores.Compliance
{
    /// <summary>
    /// Classe responsável pelo serviço de Compliance
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServerMonitorCompliance : IServicoMonitorCompliance,IServicoControlavel
    {
        /// <summary>
        /// Objeto de log para ser usado na classe
        /// </summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Notificador de segmento dos eventos de carga de suitability e de carga de churning
        /// </summary>
        private AutoResetEvent lThreadEvent = new AutoResetEvent(false);

        /// <summary>
        /// ThreadReset que inicializa e gerencia os métodos de verificação da carga de  churning
        /// </summary>
        private WaitOrTimerCallback ThreadResetLoadChurning    = null;

        /// <summary>
        /// Threadreset que inicializa e gerencia a thread de verificação de carga de suitability
        /// </summary>
        private WaitOrTimerCallback ThreadResetLoadSuitability = null;

        /// <summary>
        /// Thread que inicializa a carga de busca no sinacor 
        /// Foi necessário a implementação dessa thread para o serviço inicializar sem travamentos, já que essas cargas trazem um número 
        /// alto de registros.
        /// A thread registra o método ThreadCarregarMonitorMemoria que por sua vez chama os métodos carga de:
        /// Ordens alteradas
        /// Estatistica daytrade
        /// Negocios diretos
        /// </summary>
        private Thread thThreadCarregarMonitorMemoria;

        /// <summary>
        /// Temporizador das threads de churnig e de Load suitability.
        /// Key: TemporizadorIntervaloVerificacao
        /// </summary>
        private int TemporizadorIntervaloVerificacao
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["TemporizadorIntervaloVerificacao"]);
            }
        }

        /// <summary>
        /// Construtor vazio da classe
        /// </summary>
        public ServerMonitorCompliance()
        {
            //log4net.Config.XmlConfigurator.Configure();      
        }

        /// <summary>
        /// Lista usadas para armanezar listagens de ordens alteradas daytrade na memória
        /// </summary>
        private List<OrdensAlteradasDayTradeInfo>    _ListaOrdensAlteradasDayTrade = new List<OrdensAlteradasDayTradeInfo>();

        /// <summary>
        /// Lista usadas para armanezar listagens de Estatisticas daytrade na memória
        /// </summary>
        private List<EstatisticaDayTradeBovespaInfo> _ListaEstatisticaDayTradeBovespa = new List<EstatisticaDayTradeBovespaInfo>();

        /// <summary>
        /// Lista usadas para armanezar listagens de Negocios Diretos na memória
        /// </summary>
        private List<NegociosDiretosInfo>            _ListaNegociosDiretos = new List<NegociosDiretosInfo>();

        /// <summary>
        /// Status do serviço
        /// </summary>
        ServicoStatus _ServicoStatus { set; get; }
        
        /// <summary>
        /// Método de Load churning avulso usado para teste.
        /// Útil para carregar algumas datas que estão faltando no banco de dados
        /// </summary>
        /// <param name="pData">Data o programador pretende carregar no banco</param>
        public void StartLoadComplianceChurningAvulso (DateTime pData)
        {
            var lDb = new PersistenciaCompliance();

            List<ChurningIntradayInfo> lLista = lDb.ImportarChurningIntradayAvulso(pData);
        }

        /// <summary>
        /// Método que efetua a carga de dados do sinacor para o banco sql da gradual.
        /// Primeiro verifica se está dentro do horário programado (dentro .config ou appsettings) para efetuar a a carga, 
        /// e depois faz a busca no sinacor, e insere os dados dentro do sql gradualOMS
        /// </summary>
        /// <param name="value">(Não está sendo usado)</param>
        /// <param name="signaled">Se está sinalizado ou não(não está sendo usado)</param>
        public void StartLoadComplianceChurning(object value, bool signaled)
        {
            try
            {
                string[] lHoraVerif = ConfigurationManager.AppSettings["HoraVerificacaoChurning"].ToString().Split(';');

                List<string> lHorasVerif = lHoraVerif.ToList<string>();

                if (!lHorasVerif.Contains(DateTime.Now.ToString("HH:mm")))
                    return;

                var lDb = new PersistenciaCompliance();

                List<ChurningIntradayInfo> lLista = lDb.ImportarChurningIntraday();
            }
            catch (Exception ex)
            {
                logger.Error("Erro encontrado no método StartLoadComplianceChurning - ", ex);
            }
        }

        /// <summary>
        /// Método que verifica se é o horário certo de gerar os arquivos de fora perfil e o 
        /// arquivo de sauitability da fato.
        /// </summary>
        /// <param name="value">(Não está sendo usado)</param>
        /// <param name="signaled">(Não está sendo usado)</param>
        public void StartLoadSuitability(object value, bool signaled)
        {
            try
            {
                string[] lHoraVerif = ConfigurationManager.AppSettings["HoraVerificacaoSuitability"].ToString().Split(';');

                List<string> lHorasVerif = lHoraVerif.ToList<string>();

                if (!lHorasVerif.Contains(DateTime.Now.ToString("HH:mm")))
                    return;
                else
                {
                    if ((DateTime.Now.DayOfWeek != DayOfWeek.Sunday && DateTime.Now.DayOfWeek != DayOfWeek.Saturday))
                    {
                        var lDb = new PersistenciaCompliance();

                        logger.Info("Entrou na rotina para gerar o arquivo Fora perfil");

                        lDb.GeraListaExecutaBatchSuitability();

                        logger.Info("Entrou na rotina para gerar o arquivo Suitability Fato");

                        lDb.GeraListaSuitabilityFato();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro encontrado no método StartLoadSuitability - ", ex);
            }
        }

        /// <summary>
        /// Método que faz a chamada para os outros métodos que efetuam chamadas de carga para monitoração
        /// de ordens alteradas
        /// de MOnitor de estatistica daytrade bovespa
        /// de Negócios diretos
        /// </summary>
        /// <param name="value">(Não está sendo usado)</param>
        public void CarregarMonitoresMemoria(object value)
        {
            logger.Info("Thread responsavel por carregar os monitores em memoria inicializada");
            logger.Info("Tempo de processamento [aproximadamente 10 minutos]");
            logger.Info("Inicializando consultas.");

            logger.Info("Inicia monitor de Ordens alteradas.");
            this.PrepararMonitorOrdensAlteradas();
            logger.Info("Monitor de ordens alteradas carregado com sucesso.");

            logger.Info("Inicia monitor de estatisticas de daytrade");
            this.PrepararMonitorEstatisticaBovespa();
            logger.Info("Monitor de estatisticas de daytrade carregado com sucesso.");

            logger.Info("Inicia monitor de negocios diretos");
            this.PrepararMonitorNegociosDiretos();
            logger.Info("Monitor de negocios diretos carregados com sucesso.");

            logger.Info("Monitores carregados em memoria com sucesso");
            logger.Info("Aguardando solicitação de consulta ... ... ...");
        }

        /// <summary>
        /// Método que busca do sinacor a relação de Ordens alteradas e
        /// aloca na propriedade para ser acessado posteriormente na memória
        /// </summary>
        public void PrepararMonitorOrdensAlteradas()
        {        
            List<OrdensAlteradasDayTradeInfo> _lstOrdensAlteradas = new List<OrdensAlteradasDayTradeInfo>();

            try{

                List<OrdensAlteradasCabecalhoInfo> lstCabecalho = new PersistenciaCompliance().ObterCabecalhoOrdensAlteradasDayTrade();
                List<OrdensAlteradasInfo> lstCorpo = new PersistenciaCompliance().ObterOrdensAlteradasIntraday();


                OrdensAlteradasDayTradeInfo _tradeInfo = null;

                for (int i = 0; i <= lstCabecalho.Count - 1; i++){

                    _tradeInfo = new OrdensAlteradasDayTradeInfo();

                    OrdensAlteradasCabecalhoInfo _info = (OrdensAlteradasCabecalhoInfo)(lstCabecalho[i]);

                    var itemCorpo = from p in lstCorpo
                                    where p.NumeroSeqOrdem == _info.NumeroSeqOrdem
                                    select p;

                    if (itemCorpo.Count() > 0)
                    {
                        foreach (OrdensAlteradasInfo linha in itemCorpo)
                        {
                            _tradeInfo.NumeroSeqOrdem = lstCabecalho[i].NumeroSeqOrdem;
                            _tradeInfo.Cabecalho = lstCabecalho[i];
                            _tradeInfo.DataOperacao = lstCabecalho[i].DataHoraOrdem;
                            _tradeInfo.Corpo.Add(linha);
                        }
                        _lstOrdensAlteradas.Add(_tradeInfo);
                    }
                }

            }
            catch (Exception ex){
                logger.Error("Ocorreu um erro ao chamar o método PrepararMonitorOrdensAlteradas", ex);
            }        

            _ListaOrdensAlteradasDayTrade = _lstOrdensAlteradas;


        }

        /// <summary>
        /// Método que busca do sinacor a relação de estatistica daytrade bovespa e
        /// aloca na propriedade para ser acessado posteriormente na memória
        /// </summary>
        public void PrepararMonitorEstatisticaBovespa()
        {
            try{
                _ListaEstatisticaDayTradeBovespa 
                    = new PersistenciaCompliance().ObterEstatisticaDayTradeBovespa();
            }
            catch (Exception ex){
                logger.Error("Ocorreu um erro ao chamar o método PrepararMonitorEstatisticaBovespa.", ex);
            }
        }

        /// <summary>
        /// Método que busca do sinacor a relação de negocios diretos e aloca 
        /// na propriedade para ser acessado posteriormente na memória
        /// </summary>
        public void PrepararMonitorNegociosDiretos()
        {
            try
            {
                _ListaNegociosDiretos
                    = new PersistenciaCompliance().ObterNegociosDiretos();
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao chamar o método PrepararMonitorNegociosDiretos.", ex);
            }
        }

        #region IServicoControlavel Members

        /// <summary>
        /// Método que chama a função de carga na memória dos monitores
        /// </summary>
        public void ThreadCarregarMonitorMemoria()
        {
            try
            {
                this.CarregarMonitoresMemoria(null);
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao Iniciar o Servico de Compliance.", ex);
            }
        }

        /// <summary>
        /// Inicia o serviço de monitor de lucro prejuízo chamando o método StartMonitor
        /// Esse é um método padrão da Gradual.OMS.Library e é acionado sempre que o 
        /// serviço inicializa 
        /// </summary>
        public void IniciarServico()
        {

            try
            {
                logger.Info("Iniciando o servico de Monitoramento de Compliance");

                logger.Info("Inicia Thread responsavel por armazenar o monitor em memoria.");

                ThreadResetLoadSuitability = new WaitOrTimerCallback(StartLoadSuitability);

                ThreadPool.RegisterWaitForSingleObject(lThreadEvent, ThreadResetLoadSuitability, null, this.TemporizadorIntervaloVerificacao, false);

                //ThreadResetPosicao = new WaitOrTimerCallback(StartMonitorComplianceEvent);

                //ThreadPool.RegisterWaitForSingleObject(lThreadEvent, ThreadResetPosicao, null, this.TemporizadorIntervaloVerificacao, false);

                ThreadResetLoadChurning = new WaitOrTimerCallback(StartLoadComplianceChurning);

                ThreadPool.RegisterWaitForSingleObject(lThreadEvent, ThreadResetLoadChurning, null, this.TemporizadorIntervaloVerificacao, false);

                //ThreadPool.QueueUserWorkItem(new WaitCallback(CarregarMonitoresMemoria), null);

                thThreadCarregarMonitorMemoria = new Thread(new ThreadStart(ThreadCarregarMonitorMemoria));
                thThreadCarregarMonitorMemoria.Name = "ThreadCarregarMonitorMemoria";
                thThreadCarregarMonitorMemoria.Start();

                _ServicoStatus = ServicoStatus.EmExecucao;
                
                logger.Info("Processo inicializado com sucesso.");
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao Iniciar o Servico de Compliance.", ex);
            }
        }

        /// <summary>
        /// Método que para o serviço de monitor lucro prejuízo 
        /// Método padrão da Dll Gradual.OMS.Library que é acionado quando o serviço Está sendo parado.
        /// </summary>
        public void PararServico()
        {
            try
            {
                logger.Info("Parando o servico de Monitoramento de Compliance");
                _ServicoStatus = ServicoStatus.Parado;
                logger.Info("Servico parado com sucesso.");

                while (thThreadCarregarMonitorMemoria.IsAlive)
                {
                    logger.Info("Aguardando finalizar thThreadCarregarMonitorMemoria");
                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao parar o Servico de Compliance.", ex);
            }
        }

        /// <summary>
        /// Método que retorna o status do serviço
        /// </summary>
        /// <returns>Retorna o status do serviço de compliance</returns>
        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion

        #region IServicoMonitorCompliance Members
        /// <summary>
        /// Método que efetua a busca na prorpiedade na lista de alteração daytrade e efetua o
        /// filtro com o objeto de request
        /// </summary>
        /// <param name="pRequest">Objeto de request para efetuar o filtro na listagem na memória</param>
        /// <returns>Retorna o objeto response com os dados de ordens alteradas daytrade já filtrados</returns>
        public OrdensAlteradasDayTradeResponse ObterAlteracaoDayTrade(OrdensAlteradasDayTradeRequest pRequest)
        {

            logger.Info("Solicitação de consulta método [ObterAlteracaoDayTrade] ");
            logger.Info("Processando requisição");
            OrdensAlteradasDayTradeResponse _response = new OrdensAlteradasDayTradeResponse();

            try
            {

                if ((pRequest.DataDe != DateTime.MinValue) && (pRequest.DataAte != DateTime.MinValue))
                {

                    List<OrdensAlteradasDayTradeInfo> listaOrdens = new List<OrdensAlteradasDayTradeInfo>();

                    var lstOrdens = from p in _ListaOrdensAlteradasDayTrade
                                    where p.DataOperacao >= pRequest.DataDe
                                    && p.DataOperacao <= pRequest.DataAte
                                    select p;

                    if (lstOrdens.Count() > 0)
                    {
                        foreach (var item in lstOrdens)
                        {
                            listaOrdens.Add(item);
                        }
                    }

                    _response.lstAlteracaoDayTrade = listaOrdens;

                }
                else
                {
                    _response.lstAlteracaoDayTrade = _ListaOrdensAlteradasDayTrade;
                }

            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao calcular ao acessar o método ObterAlteracaoDayTrade. ", ex);
            }

            logger.Info("Consulta processada e retornada com sucesso. + Numero de ocorrencias = " + _response.lstAlteracaoDayTrade.Count.ToString());
            return _response;
        }

        /// <summary>
        /// Método efetua a busca na propriedade na lista de Estatísticas daytrade e e efetua o 
        /// filtro com o objeto de request
        /// </summary>
        /// <param name="pRequest">Objeto de request para efetuar o filtro na listagem na memória</param>
        /// <returns>Retorna o objeto de response com os dados das estatísticas de daytrade já filtrados</returns>
        public EstatisticaDayTradeResponse ObterEstatisticaDayTradeBovespa(EstatisticaDayTradeRequest pRequest)
        {
            logger.Info("Solicitação de consulta método [ObterEstatisticaDayTradeBovespa] ");
            logger.Info("Processando requisição");
            EstatisticaDayTradeResponse _response = new EstatisticaDayTradeResponse();

            try
            {
                if (pRequest.TipoBolsa == EnumBolsaDayTrade.TODOS)
                {
                    if ((pRequest.Assessor == 0) && (pRequest.CodigoCliente != 0))
                    {
                        List<EstatisticaDayTradeBovespaInfo> listaOrdens = new List<EstatisticaDayTradeBovespaInfo>();

                        var lstOrdens = from p in _ListaEstatisticaDayTradeBovespa
                                        where p.CodigoCliente == pRequest.CodigoCliente
                                        

                                        select p;

                        if (lstOrdens.Count() > 0)
                        {
                            foreach (var item in lstOrdens)
                            {
                                listaOrdens.Add(item);
                            }
                        }

                        _response.ListaEstatisticaBovespa = listaOrdens;

                    }
                    else if ((pRequest.Assessor != 0) && (pRequest.CodigoCliente == 0))
                    {
                        List<EstatisticaDayTradeBovespaInfo> listaOrdens = new List<EstatisticaDayTradeBovespaInfo>();

                        var lstOrdens = from p in _ListaEstatisticaDayTradeBovespa
                                        where p.CodigoAssessor == pRequest.Assessor
                                          

                                        select p;

                        if (lstOrdens.Count() > 0)
                        {
                            foreach (var item in lstOrdens)
                            {
                                listaOrdens.Add(item);
                            }
                        }

                        _response.ListaEstatisticaBovespa = listaOrdens;

                    }
                    else if ((pRequest.Assessor != 0) && (pRequest.CodigoCliente != 0))
                    {
                        List<EstatisticaDayTradeBovespaInfo> listaOrdens = new List<EstatisticaDayTradeBovespaInfo>();

                        var lstOrdens = from p in _ListaEstatisticaDayTradeBovespa
                                        where p.CodigoCliente == pRequest.CodigoCliente
                                        && p.CodigoAssessor == pRequest.Assessor

                                        select p;

                        if (lstOrdens.Count() > 0)
                        {
                            foreach (var item in lstOrdens)
                            {
                                listaOrdens.Add(item);
                            }
                        }

                        _response.ListaEstatisticaBovespa = listaOrdens;

                    }
                    else if ((pRequest.Assessor == 0) && (pRequest.CodigoCliente == 0))
                    {
                        List<EstatisticaDayTradeBovespaInfo> listaOrdens = new List<EstatisticaDayTradeBovespaInfo>();

                        var lstOrdens = from p in _ListaEstatisticaDayTradeBovespa
                                        select p;

                        if (lstOrdens.Count() > 0)
                        {
                            foreach (var item in lstOrdens)
                            {
                                listaOrdens.Add(item);
                            }
                        }

                        _response.ListaEstatisticaBovespa = listaOrdens;
                    }
                }
                else
                if (pRequest.TipoBolsa == EnumBolsaDayTrade.BOVESPA)
                {

                    if ((pRequest.Assessor == 0) && (pRequest.CodigoCliente != 0))
                    {
                        List<EstatisticaDayTradeBovespaInfo> listaOrdens = new List<EstatisticaDayTradeBovespaInfo>();

                        var lstOrdens = from p in _ListaEstatisticaDayTradeBovespa
                                        where p.CodigoCliente == pRequest.CodigoCliente
                                        && p.TipoBolsa == EnumBolsaDayTrade.BOVESPA

                                        select p;

                        if (lstOrdens.Count() > 0)
                        {
                            foreach (var item in lstOrdens)
                            {
                                listaOrdens.Add(item);
                            }
                        }

                        _response.ListaEstatisticaBovespa = listaOrdens;

                    }
                    else if ((pRequest.Assessor != 0) && (pRequest.CodigoCliente == 0))
                    {
                        List<EstatisticaDayTradeBovespaInfo> listaOrdens = new List<EstatisticaDayTradeBovespaInfo>();

                        var lstOrdens = from p in _ListaEstatisticaDayTradeBovespa
                                        where p.CodigoAssessor == pRequest.Assessor
                                          && p.TipoBolsa == EnumBolsaDayTrade.BOVESPA

                                        select p;

                        if (lstOrdens.Count() > 0)
                        {
                            foreach (var item in lstOrdens)
                            {
                                listaOrdens.Add(item);
                            }
                        }

                        _response.ListaEstatisticaBovespa = listaOrdens;

                    }
                    else if ((pRequest.Assessor != 0) && (pRequest.CodigoCliente != 0))
                    {
                        List<EstatisticaDayTradeBovespaInfo> listaOrdens = new List<EstatisticaDayTradeBovespaInfo>();

                        var lstOrdens = from p in _ListaEstatisticaDayTradeBovespa
                                        where p.CodigoCliente == pRequest.CodigoCliente
                                        && p.CodigoAssessor == pRequest.Assessor
                                        && p.TipoBolsa == EnumBolsaDayTrade.BOVESPA

                                        select p;

                        if (lstOrdens.Count() > 0)
                        {
                            foreach (var item in lstOrdens)
                            {
                                listaOrdens.Add(item);
                            }
                        }

                        _response.ListaEstatisticaBovespa = listaOrdens;

                    }
                    else if ((pRequest.Assessor == 0) && (pRequest.CodigoCliente == 0))
                    {
                        List<EstatisticaDayTradeBovespaInfo> listaOrdens = new List<EstatisticaDayTradeBovespaInfo>();

                        var lstOrdens = from p in _ListaEstatisticaDayTradeBovespa
                                        where p.TipoBolsa == EnumBolsaDayTrade.BOVESPA

                                        select p;

                        if (lstOrdens.Count() > 0)
                        {
                            foreach (var item in lstOrdens)
                            {
                                listaOrdens.Add(item);
                            }
                        }

                        _response.ListaEstatisticaBovespa = listaOrdens;
                    }
                }
                else
                {
                    //BMF SECTION

                    if ((pRequest.Assessor != 0) && (pRequest.CodigoCliente != 0))
                    {
                        List<EstatisticaDayTradeBovespaInfo> listaOrdens = new List<EstatisticaDayTradeBovespaInfo>();

                        var lstOrdens = from p in _ListaEstatisticaDayTradeBovespa
                                        where p.CodigoAssessor == pRequest.Assessor
                                        && p.CodigoCliente == pRequest.CodigoCliente
                                        && p.TipoBolsa == EnumBolsaDayTrade.BMF

                                        select p;

                        if (lstOrdens.Count() > 0)
                        {
                            foreach (var item in lstOrdens)
                            {
                                listaOrdens.Add(item);
                            }
                        }

                        _response.ListaEstatisticaBovespa = listaOrdens;

                    }
                    else if ((pRequest.Assessor != 0) && (pRequest.CodigoCliente == 0))
                    {
                        List<EstatisticaDayTradeBovespaInfo> listaOrdens = new List<EstatisticaDayTradeBovespaInfo>();

                        var lstOrdens = from p in _ListaEstatisticaDayTradeBovespa
                                        where p.CodigoAssessor == pRequest.Assessor
                                              && p.TipoBolsa == EnumBolsaDayTrade.BMF

                                        select p;

                        if (lstOrdens.Count() > 0)
                        {
                            foreach (var item in lstOrdens)
                            {
                                listaOrdens.Add(item);
                            }
                        }

                        _response.ListaEstatisticaBovespa = listaOrdens;

                    }
                    else if ((pRequest.Assessor == 0) && (pRequest.CodigoCliente != 0))
                    {
                        List<EstatisticaDayTradeBovespaInfo> listaOrdens = new List<EstatisticaDayTradeBovespaInfo>();

                        var lstOrdens = from p in _ListaEstatisticaDayTradeBovespa
                                        where p.CodigoCliente == pRequest.CodigoCliente
                                        && p.TipoBolsa == EnumBolsaDayTrade.BMF
                                        select p;

                        if (lstOrdens.Count() > 0)
                        {
                            foreach (var item in lstOrdens)
                            {
                                listaOrdens.Add(item);
                            }
                        }

                        _response.ListaEstatisticaBovespa = listaOrdens;

                    }
                    else if ((pRequest.Assessor == 0) && (pRequest.CodigoCliente == 0))
                    {
                        List<EstatisticaDayTradeBovespaInfo> listaOrdens = new List<EstatisticaDayTradeBovespaInfo>();

                        var lstOrdens = from p in _ListaEstatisticaDayTradeBovespa
                                        where p.TipoBolsa == EnumBolsaDayTrade.BMF

                                        select p;

                        if (lstOrdens.Count() > 0)
                        {
                            foreach (var item in lstOrdens)
                            {
                                listaOrdens.Add(item);
                            }
                        }

                        _response.ListaEstatisticaBovespa = listaOrdens;
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao calcular ao acessar o método ObterEstatisticaDayTradeBovespa. ", ex);
            }

            logger.Info("Consulta processada e retornada com sucesso. + Numero de ocorrencias = " + _response.ListaEstatisticaBovespa.Count.ToString());
            return _response;
        }

        /// <summary>
        /// Método efetua a busca na propriedade na lista de Negocios Diretos e e efetua o 
        /// filtro com o objeto de request
        /// </summary>
        /// <param name="pRequest">Objeto de request para efetuar o filtro na listagem na memória</param>
        /// <returns>Retorna o bojeto de response com os dados de negócios direto</returns>
        public NegociosDiretosResponse ObterNegociosDiretos(NegociosDiretosRequest pRequest)
        {
            logger.Info("Solicitação de consulta método [ObterNegociosDiretos] ");
            logger.Info("Processando requisição");
            NegociosDiretosResponse _response = new NegociosDiretosResponse();

            try
            {
                List<NegociosDiretosInfo> listaOrdens = new List<NegociosDiretosInfo>();

                //if ((pRequest.DataDe != DateTime.MinValue) && (pRequest.DataAte != DateTime.MinValue))
                //{

                //    var lstOrdens = from p in _ListaNegociosDiretos
                //                    where (p.DataNegocio >= pRequest.DataDe
                //                    && p.DataNegocio <= pRequest.DataAte) && p.Sentido == pRequest.Sentido
                //                    select p;

                //    if (pRequest.CodigoCliente != 0)
                //    {
                //        lstOrdens = from p in lstOrdens
                //                    where p.CodigoCliente == pRequest.CodigoCliente
                //                    select p;
                //    }

                //    _response.lstNegociosDiretos.Clear();

                //    listaOrdens.Clear();

                //    if (lstOrdens.Count() > 0)
                //    {

                //        foreach (var item in lstOrdens)
                //        {
                //            listaOrdens.Add(item);
                //        }
                //    }

                //    _response.lstNegociosDiretos = listaOrdens;
                //}
                //else
                //{
                
                var lstOrdens = from p in _ListaNegociosDiretos where p.DataNegocio.ToString("dd/MM/yyyy") == pRequest.Data.ToString("dd/MM/yyyy") select p;

                //if (pRequest.CodigoCliente != 0)
                //{
                //    lstOrdens = from p in lstOrdens where p.CodigoCliente == pRequest.CodigoCliente select p;
                //}

                listaOrdens.Clear();

                _response.lstNegociosDiretos.Clear();

                listaOrdens.AddRange(lstOrdens);

                _response.lstNegociosDiretos = listaOrdens;
                //}

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao inovar o método ObterNegociosDiretos.", ex);
            }

            logger.Info("Consulta processada e retornada com sucesso. + Numero de ocorrencias = " + _response.lstNegociosDiretos.Count.ToString());
            return _response;            
        }
        #endregion

    }
}
