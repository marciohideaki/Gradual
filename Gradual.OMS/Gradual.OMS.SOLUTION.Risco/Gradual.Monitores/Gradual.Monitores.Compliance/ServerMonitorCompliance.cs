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

namespace Gradual.Monitores.Compliance
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServerMonitorCompliance : IServicoMonitorCompliance,IServicoControlavel
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ServerMonitorCompliance()
        {
            log4net.Config.XmlConfigurator.Configure();      
        }

        #region declarações

        // Coleções responsaveis por armazenar as consultas do sinacor em memoria

        private List<OrdensAlteradasDayTradeInfo>    _ListaOrdensAlteradasDayTrade = new List<OrdensAlteradasDayTradeInfo>();
        private List<EstatisticaDayTradeBovespaInfo> _ListaEstatisticaDayTradeBovespa = new List<EstatisticaDayTradeBovespaInfo>();
        private List<NegociosDiretosInfo>            _ListaNegociosDiretos = new List<NegociosDiretosInfo>();
        #endregion
 

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

        ServicoStatus _ServicoStatus { set; get; }
    
        public void IniciarServico(){

            try
            {
                logger.Info("Iniciando o servico de Monitoramento de Compliance");

                logger.Info("Inicia Thread responsavel por armazenar o monitor em memoria.");
                ThreadPool.QueueUserWorkItem(new WaitCallback(CarregarMonitoresMemoria), null);
                _ServicoStatus = ServicoStatus.EmExecucao;
                logger.Info("Processo inicializado com sucesso.");
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao Iniciar o Servico de Compliance.", ex);
            }
        }

        public void PararServico(){
            try
            {
                logger.Info("Parando o servico de Monitoramento de Compliance");
                _ServicoStatus = ServicoStatus.Parado;
                logger.Info("Servico parado com sucesso.");
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao parar o Servico de Compliance.", ex);
            }
        }

        public ServicoStatus ReceberStatusServico(){
            return _ServicoStatus;
        }

        #endregion

        #region IServicoMonitorCompliance Members

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

        public EstatisticaDayTradeResponse ObterEstatisticaDayTradeBovespa(EstatisticaDayTradeRequest pRequest)
        {
            logger.Info("Solicitação de consulta método [ObterEstatisticaDayTradeBovespa] ");
            logger.Info("Processando requisição");
            EstatisticaDayTradeResponse _response = new EstatisticaDayTradeResponse();

            try
            {

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
                                        where  p.TipoBolsa == EnumBolsaDayTrade.BMF

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

        public NegociosDiretosResponse ObterNegociosDiretos(NegociosDiretosRequest pRequest)
        {
            logger.Info("Solicitação de consulta método [ObterNegociosDiretos] ");
            logger.Info("Processando requisição");
            NegociosDiretosResponse _response = new NegociosDiretosResponse();

            try
            {

                if ((pRequest.DataDe != DateTime.MinValue) && (pRequest.DataAte != DateTime.MinValue))
                {
                    List<NegociosDiretosInfo> listaOrdens = new List<NegociosDiretosInfo>();

                    var lstOrdens = from p in _ListaNegociosDiretos
                                    where p.DataNegocio >= pRequest.DataDe
                                    && p.DataNegocio <= pRequest.DataAte
                                    select p;

                    if (lstOrdens.Count() > 0)
                    {

                        foreach (var item in lstOrdens)
                        {
                            listaOrdens.Add(item);
                        }
                    }

                    _response.lstNegociosDiretos = listaOrdens;
                }
                else{
                    _response.lstNegociosDiretos = _ListaNegociosDiretos;
                }

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
