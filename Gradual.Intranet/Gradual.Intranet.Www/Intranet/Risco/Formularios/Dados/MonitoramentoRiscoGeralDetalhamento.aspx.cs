using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Monitores.Risco.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Monitores.Risco.Info;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Newtonsoft.Json;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using log4net;
using Gradual.OMS.Cotacao.Lib;
using System.Globalization;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Contratos.Dados;
using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.OMS.Custodia.Lib.Enum;
using Gradual.OMS.ContaCorrente.Lib.Info.Enum;
using System.Threading;
using Gradual.OMS.Library;
using Gradual.OMS.Monitor.Custodia.Lib;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using Gradual.OMS.Monitor.Custodia.Lib.Mensageria;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;
using Gradual.Intranet.Www.App_Codigo;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class MonitoramentoRiscoGeralDetalhamento : PaginaBase
    {
        #region Atributos
        private const string DI            = "DI1";
        private const string DOLAR         = "DOL";
        private const string INDICE        = "IND";
        private const string MINIBOLSA     = "WIN";
        private const string MINIDOLAR     = "WDL";
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

        private CultureInfo gCultura = new CultureInfo("pt-BR");
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.LucrosPrejuizosInfo> gMonitoramentoLucrosPrejuizos;
        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesClienteInfo> gMonitoramentoOperacoesCliente;
        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo> gMonitoramentoOperacoesDetalhesCliente;
        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo> gMonitoramentoOperacoesDetalhesClienteBmf;
        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridInfo> gMonitoramentoOperacoesClienteBmf;
        
        //--Termo --//
        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesTermoClienteInfo> gMonitoramentoOperacoesTermoCliente;
        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesTermoClienteDetalhesInfo> gMonitoramentoOperacoesTermoClienteDetalhes;

        //-- BTC --//
        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesBTCClienteInfo> gMonitoramentoOperacoesBTCCliente;
        private static List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesBTCClienteDetalhesInfo> gMonitoramentoOperacoesBTCClienteDetalhes;

        //--Monitor--/
        //private static MonitorLucroPrejuizoResponse Monitor;

        
        #endregion

        #region Properties
        private string GetInstrumento
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["Instrumento"]))
                    return this.Request["Instrumento"];

                return this.Request["Instrumento"];
            }
        }

        private string GetFiltrarPor
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["sidx"]))
                    return this.Request["sidx"];

                return null;
            }
        }

        private int? GetCdCliente
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodigoCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }
        
        private int? GetCdClienteBmf
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodigoClienteBmf"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetCdAssessor
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodAssessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private string GetOrdenacao
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Request["sord"]))
                    return this.Request["sord"];

                return null;
            }
        }
        
        public string GetMercado
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["Mercado"]))
                    return null;

                return this.Request["Mercado"];
            }
        }
        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.LucrosPrejuizosInfo> SessaoUltimaConsulta
        {
            get { return gMonitoramentoLucrosPrejuizos != null ? gMonitoramentoLucrosPrejuizos : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.LucrosPrejuizosInfo>(); }
            set { gMonitoramentoLucrosPrejuizos = value; }
        }

        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesClienteInfo> SessaoUltimaConsultaOperacoesCliente
        {
            get { return gMonitoramentoOperacoesCliente != null ? gMonitoramentoOperacoesCliente : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesClienteInfo>(); }
            set { gMonitoramentoOperacoesCliente = value; }
        }
        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo> SessaoUltimaConsultaOperacoesClienteDetalhe
        {
            get { return gMonitoramentoOperacoesDetalhesCliente != null ? gMonitoramentoOperacoesDetalhesCliente : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo>(); }
            set { gMonitoramentoOperacoesDetalhesCliente = value; }
        }
        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo> SessaoUltimaConsultaOperacoesClienteDetalhesBmf
        {
            get { return gMonitoramentoOperacoesDetalhesClienteBmf != null ? gMonitoramentoOperacoesDetalhesClienteBmf : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo>(); }
            set { gMonitoramentoOperacoesDetalhesClienteBmf = value; }
        }

        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesTermoClienteInfo> SessaoUltimaConsultaOperacoesTermosCliente
        {
            get { return gMonitoramentoOperacoesTermoCliente != null ? gMonitoramentoOperacoesTermoCliente : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesTermoClienteInfo>(); }
            set { gMonitoramentoOperacoesTermoCliente = value; }
        }
        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesTermoClienteDetalhesInfo> SessaoUltimaConsultaOperacoesTermosClienteDetalhes
        {
            get { return gMonitoramentoOperacoesTermoClienteDetalhes != null ? gMonitoramentoOperacoesTermoClienteDetalhes : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesTermoClienteDetalhesInfo>(); }
            set { gMonitoramentoOperacoesTermoClienteDetalhes = value; }
        }

        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesBTCClienteInfo> SessaoUltimaConsultaOperacoesBTCCliente
        {
            get { return gMonitoramentoOperacoesBTCCliente != null ? gMonitoramentoOperacoesBTCCliente : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesBTCClienteInfo>(); }
            set { gMonitoramentoOperacoesBTCCliente = value; }
        }

        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesBTCClienteDetalhesInfo> SessaoUltimaConsultaOperacoesBTCClienteDetalhes
        {
            get { return gMonitoramentoOperacoesBTCClienteDetalhes != null ? gMonitoramentoOperacoesBTCClienteDetalhes : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesBTCClienteDetalhesInfo>(); }
            set { gMonitoramentoOperacoesBTCClienteDetalhes = value; }
        }

        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridInfo> SessaoUltimaConsultaOperacoesClienteBmf
        {
            get { return gMonitoramentoOperacoesClienteBmf != null ? gMonitoramentoOperacoesClienteBmf : new List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridInfo>(); }
            set { gMonitoramentoOperacoesClienteBmf = value; }
        }
        #endregion

        #region Eventos
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null) { return; }

            base.RegistrarRespostasAjax(new string[] { 
                                                      "BuscarCustodia"
                                                     , "BuscarValoresLabels"
                                                     , "BuscarCustodiaPosicao"
                                                     , "BuscarDadosCliente"
                                                     , "BuscarTotalLucroPrejuizoBovespa"
                                                     , "BuscarTotalLucroPrejuizoBmf"
                                                     , "BuscarOperacoesBovespa"
                                                     , "BuscarOperacoesBmf"
                                                     , "BuscarDetalhamentoOperacoesBovespa"
                                                     , "BuscarDetalhamentoOperacoeBmf"
                                                     , "BuscarCustodiaOperacoesTermo"
                                                     , "BuscarCustodiaOperacoesTermoDetalhes"
                                                     , "BuscarCustodiaOperacoesBTC"
                                                     , "BuscarCustodiaOperacoesBTCDetalhes"
                                                     , "BuscarCustodiaFundos"
                                                     , "BuscarCustodiaRendaFixa"
                                                     , "BuscarCustodiaSemCarteira"
                                                     
                                                     },
                     new ResponderAcaoAjaxDelegate[] { 
                                                       this.ResponderBuscarCustodia
                                                     , this.CarregarHtmlLables
                                                     , this.ResponderBuscarCustodiaPosicao
                                                     , this.ResponderBuscarDadosClientes
                                                     , this.ResponderBuscarTotalLucroPrejuizoBovespa
                                                     , this.ResponderBuscarTotalLucroPrejuizoBmf
                                                     , this.ResponderBuscarOperacoesBovespa
                                                     , this.ResponderBuscarOperacoesBMF
                                                     , this.ResponderBuscarDetalhamentoOperacoesBovespa
                                                     , this.ResponderBuscarDetalhamentoOperacoesBmf
                                                     , this.ResponderBuscarCustodiaTermo
                                                     , this.ResponderBuscarCustodiaTermoDetalhes
                                                     , this.ResponderBuscarCustodiaBTC
                                                     , this.ResponderBuscarCustodiaBTCDetalhes
                                                     , this.ResponderBuscarCustodiaFundos
                                                     , this.ResponderBuscarCustodiaRendaFixa
                                                     , this.ResponderBuscarCustodiaSemCarteira
                                                     
                                                     });
        }

        #endregion

        #region Termo e BTC
        private string ResponderBuscarCustodiaBTCDetalhes()
        {
            string lRetorno = string.Empty;

            string lInstrumento = string.Empty;

            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            //ObterPosicaoBtcRequest lRequest = new ObterPosicaoBtcRequest();

            MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            if (this.GetCdCliente != null)
            {
                //lRequest.Cliente = this.GetCdCliente.Value;
                lRequest.Cliente = this.GetCdCliente.Value;
            }

            if (this.GetInstrumento != null)
            {
                lInstrumento = this.GetInstrumento;
            }

            //var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);

            var lRetornoConsulta = Session["Monitor_" + lRequest.Cliente] as MonitorLucroPrejuizoResponse;

            if (lRetornoConsulta != null && lRetornoConsulta.Monitor != null && lRetornoConsulta.Monitor.Count > 0)
            //if (lRetornoConsulta != null && lRetornoConsulta.PosicaoBTC != null && lRetornoConsulta.PosicaoBTC.Count > 0)
            {
                this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensBTC, lInstrumento);

                this.ResponderOrdenarPorColunaOperacoesDetalhesBTC();
                //this.AplicarFiltroInstrumento(lInstrumento);

                lRetornoLista = new TransporteDeListaPaginada(this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = 0;
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao processar a requisição");
            }

            return lRetorno;
        }

        private string ResponderBuscarCustodiaBTC()
        {
            string lRetorno = string.Empty;

            string lInstrumento = string.Empty;

            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            //ObterPosicaoBtcRequest lRequest = new ObterPosicaoBtcRequest();
            MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            if (this.GetCdCliente != null)
            {
                lRequest.Cliente = this.GetCdCliente.Value;
            }

            if (this.GetInstrumento != null)
            {
                lInstrumento = this.GetInstrumento;
            }

            //var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest); // Session["Monitor"] as MonitorLucroPrejuizoResponse;

            var lRetornoConsulta = Session["Monitor_" + lRequest.Cliente] as MonitorLucroPrejuizoResponse;

            if (lRetornoConsulta != null && lRetornoConsulta.Monitor != null && lRetornoConsulta.Monitor.Count > 0)
            {
                this.SessaoUltimaConsultaOperacoesBTCCliente = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensBTC);

                this.ResponderOrdenarPorColunaOperacoesBTC();

                lRetornoLista = new TransporteDeListaPaginada(this.SessaoUltimaConsultaOperacoesBTCCliente);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = this.SessaoUltimaConsultaOperacoesTermosCliente.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = 0;
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao processar a requisição");
            }

            return lRetorno;
        }

        private string ResponderBuscarCustodiaTermoDetalhes()
        {
            string lRetorno = string.Empty;

            string lInstrumento = string.Empty;

            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            //ObterPosicaoTermoRequest lRequest = new ObterPosicaoTermoRequest();
            MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            if (this.GetCdCliente != null)
            {
                lRequest.Cliente = this.GetCdCliente.Value;
            }

            if (this.GetInstrumento != null)
            {
                lInstrumento = this.GetInstrumento;
            }

            //var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);

            var lRetornoConsulta = Session["Monitor_" + lRequest.Cliente] as MonitorLucroPrejuizoResponse;

            if (lRetornoConsulta != null && lRetornoConsulta.Monitor != null && lRetornoConsulta.Monitor.Count > 0)
            {
                this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensTermo, lInstrumento, this.ListaFeriados);

                this.ResponderOrdenarPorColunaOperacoesDetalhesTermo();

                lRetornoLista = new TransporteDeListaPaginada(this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = this.SessaoUltimaConsultaOperacoesTermosCliente.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = 0;
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao processar a requisição");
            }

            return lRetorno;
        }

        private string ResponderBuscarCustodiaTermo()
        {
            string lRetorno = string.Empty;

            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            //ObterPosicaoTermoRequest lRequest = new ObterPosicaoTermoRequest();

            MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            if (this.GetCdCliente != null)
            {
                //lRequest.CodigoCliente = this.GetCdCliente.Value;
                lRequest.Cliente = this.GetCdCliente.Value;
            }

            //var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);

            var lRetornoConsulta = Session["Monitor_" + lRequest.Cliente] as MonitorLucroPrejuizoResponse;

            if (lRetornoConsulta != null && lRetornoConsulta.Monitor.Count > 0)
            {
                this.SessaoUltimaConsultaOperacoesTermosCliente = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensTermo);

                this.ResponderOrdenarPorColunaOperacoesTermo();

                lRetornoLista = new TransporteDeListaPaginada(this.SessaoUltimaConsultaOperacoesTermosCliente);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = this.SessaoUltimaConsultaOperacoesTermosCliente.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = 0;
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao processar a requisição");
            }

            return lRetorno;
        }

        private string ResponderBuscarCustodiaRendaFixa()
        {
            string lRetorno = string.Empty;

            if (this.GetCdCliente.HasValue)
            {
                var lRequestRendaFixa = new ConsultarEntidadeCadastroRequest<RendaFixaInfo>();

                lRequestRendaFixa.EntidadeCadastro = new RendaFixaInfo();

                lRequestRendaFixa.EntidadeCadastro.CodigoCliente = Convert.ToInt32(this.GetCdCliente.Value);//.DBToInt32();

                ConsultarEntidadeCadastroResponse<RendaFixaInfo> lPosicaoRendaFixa = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RendaFixaInfo>(lRequestRendaFixa);

                var ListaRendaFixa = new TransporteRelatorioRendaFixa().TraduzirLista(lPosicaoRendaFixa.Resultado, null);

                var lRetornoLista = new TransporteDeListaPaginada(ListaRendaFixa);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                //lRetorno = base.RetornarSucessoAjax(ListaRendaFixa, "Dados carregados com sucesso.");
            }
            return lRetorno;
        }
        #endregion

        #region Métodos de Ordenação
        private string ResponderOrdenarPorColunaOperacoesDetalhesTermo()
        {
            switch (this.GetFiltrarPor)
            {
                case "CodigoCliente":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp1.CodigoCliente), int.Parse(lp2.CodigoCliente)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp2.CodigoCliente), int.Parse(lp1.CodigoCliente)));
                    }
                    break;

                case "Instrumento":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.Instrumento, lp2.Instrumento));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.Instrumento, lp1.Instrumento));
                    }
                    break;
                case "DataExecucao":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp1.DataExecucao), DateTime.Parse(lp2.DataExecucao)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp2.DataExecucao), DateTime.Parse(lp1.DataExecucao)));
                    }
                    break;

                case "DataVencimento":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp1.DataVencimento), DateTime.Parse(lp2.DataVencimento)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp2.DataVencimento), DateTime.Parse(lp1.DataVencimento)));
                    }
                    break;

                case "Quantidade":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<Int32>.Default.Compare(int.Parse(lp1.Quantidade), int.Parse(lp2.Quantidade)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<Int32>.Default.Compare(Int32.Parse(lp2.Quantidade), Int32.Parse(lp1.Quantidade)));
                    }
                    break;

                case "LucroPrejuizo":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.LucroPrejuizo), decimal.Parse(lp2.LucroPrejuizo)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.LucroPrejuizo), decimal.Parse(lp1.LucroPrejuizo)));
                    }
                    break;
                case "PrecoExecucao":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PrecoExecucao), decimal.Parse(lp2.PrecoExecucao)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PrecoExecucao), decimal.Parse(lp1.PrecoExecucao)));
                    }
                    break;
                case "PrecoMercado":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PrecoMercado), decimal.Parse(lp2.PrecoMercado)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PrecoMercado), decimal.Parse(lp1.PrecoMercado)));
                    }
                    break;
            }
            
            return base.RetornarSucessoAjax(this.SessaoUltimaConsultaOperacoesClienteBmf, "sucesso");
        }

        private string ResponderOrdenarPorColunaOperacoesDetalhesBTC()
        {

            switch (this.GetFiltrarPor)
            {
                case "Carteira":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.Carteira, lp2.Carteira));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.Carteira, lp1.Carteira));
                    }
                    break;

                case "CodigoCliente":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp1.CodigoCliente), int.Parse(lp2.CodigoCliente)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp2.CodigoCliente), int.Parse(lp1.CodigoCliente)));
                    }
                    break;

                case "DataAbertura":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp1.DataAbertura), DateTime.Parse(lp2.DataAbertura)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp2.DataAbertura), DateTime.Parse(lp1.DataAbertura)));
                    }
                    break;

                case "DataVencimento":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp1.DataVencimento), DateTime.Parse(lp2.DataVencimento)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp2.DataVencimento), DateTime.Parse(lp1.DataVencimento)));
                    }
                    break;
                case "Instrumento":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.Instrumento, lp2.Instrumento));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.Instrumento, lp1.Instrumento));
                    }
                    break;
                case "PrecoMedio":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PrecoMedio), decimal.Parse(lp2.PrecoMedio)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PrecoMedio), decimal.Parse(lp1.PrecoMedio)));
                    }
                    break;
                case "PrecoMercado":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PrecoMercado), decimal.Parse(lp2.PrecoMercado)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PrecoMercado), decimal.Parse(lp1.PrecoMercado)));
                    }
                    break;
                case "Quantidade":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.Quantidade), decimal.Parse(lp2.Quantidade)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.Quantidade), decimal.Parse(lp1.Quantidade)));
                    }
                    break;
                case "Remuneracao":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.Remuneracao), decimal.Parse(lp2.Remuneracao)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.Remuneracao), decimal.Parse(lp1.Remuneracao)));
                    }
                    break;
                case "Taxa":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.Taxa), decimal.Parse(lp2.Taxa)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.Taxa), decimal.Parse(lp1.Taxa)));
                    }
                    break;
                case "TipoContrato":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.TipoContrato, lp2.TipoContrato));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.TipoContrato, lp1.TipoContrato));
                    }
                    break;
            }

            return base.RetornarSucessoAjax(this.SessaoUltimaConsultaOperacoesBTCClienteDetalhes, "sucesso");
        }

        private string ResponderOrdenarPorColunaOperacoesBTC()
        {
            
            switch (this.GetFiltrarPor)
            {
                case "Carteira":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.Carteira, lp2.Carteira));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.Carteira, lp1.Carteira));
                    }
                    break;

                case "CodigoCliente":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp1.CodigoCliente), int.Parse(lp2.CodigoCliente)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp2.CodigoCliente), int.Parse(lp1.CodigoCliente)));
                    }
                    break;

                case "DataAbertura":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp1.DataAbertura), DateTime.Parse(lp2.DataAbertura)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp2.DataAbertura),DateTime.Parse(lp1.DataAbertura)));
                    }
                    break;

                case "DataVencimento":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp1.DataVencimento), DateTime.Parse(lp2.DataVencimento)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp2.DataVencimento), DateTime.Parse(lp1.DataVencimento)));
                    }
                    break;
                case "Instrumento":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.Instrumento, lp2.Instrumento));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.Instrumento, lp1.Instrumento));
                    }
                    break;
                case "PrecoMedio":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PrecoMedio), decimal.Parse(lp2.PrecoMedio)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PrecoMedio), decimal.Parse(lp1.PrecoMedio)));
                    }
                    break;
                case "PrecoMercado":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PrecoMercado), decimal.Parse( lp2.PrecoMercado)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PrecoMercado),decimal.Parse( lp1.PrecoMercado)));
                    }
                    break;
                case "Quantidade":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare( decimal.Parse(lp1.Quantidade), decimal.Parse( lp2.Quantidade)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare( decimal.Parse(lp2.Quantidade), decimal.Parse( lp1.Quantidade)));
                    }
                    break;
                case "Remuneracao":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.Remuneracao),decimal.Parse( lp2.Remuneracao)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.Remuneracao), decimal.Parse(lp1.Remuneracao)));
                    }
                    break;
                case "Taxa":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.Taxa), decimal.Parse(lp2.Taxa)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.Taxa), decimal.Parse(lp1.Taxa)));
                    }
                    break;
                case "TipoContrato":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.TipoContrato, lp2.TipoContrato));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.TipoContrato, lp1.TipoContrato));
                    }
                    break;
                
                case "SubtotalQuantidade":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SubtotalQuantidade), decimal.Parse(lp2.SubtotalQuantidade)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SubtotalQuantidade), decimal.Parse(lp1.SubtotalQuantidade)));
                    }
                    break;

                case "SubtotalValor":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SubtotalValor), decimal.Parse(lp2.SubtotalValor)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesBTCCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SubtotalValor), decimal.Parse(lp1.SubtotalValor)));
                    }
                    break;
            }

            return base.RetornarSucessoAjax(this.SessaoUltimaConsultaOperacoesClienteBmf, "sucesso");
        }

        private string ResponderOrdenarPorColunaOperacoesTermo()
        {
            switch (this.GetFiltrarPor)
            {
                case "CodigoCliente":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosCliente.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp1.CodigoCliente), int.Parse(lp2.CodigoCliente)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosCliente.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp2.CodigoCliente), int.Parse(lp1.CodigoCliente)));
                    }
                    break;

                case "Instrumento":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosCliente.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.Instrumento, lp2.Instrumento));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosCliente.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.Instrumento, lp1.Instrumento));
                    }
                    break;
                case "SubtotalQuantidade":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SubtotalQuantidade), decimal.Parse(lp2.SubtotalQuantidade)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SubtotalQuantidade), decimal.Parse(lp1.SubtotalQuantidade)));
                    }
                    break;
                case "SubtotalValor":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SubtotalValor), decimal.Parse(lp2.SubtotalValor)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SubtotalValor), decimal.Parse(lp1.SubtotalValor)));
                    }
                    break;
                case "SubtotalLucroPrejuizo":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesTermosCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SubtotalLucroPrejuizo), decimal.Parse(lp2.SubtotalLucroPrejuizo)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesTermosCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SubtotalLucroPrejuizo), decimal.Parse(lp1.SubtotalLucroPrejuizo)));
                    }
                    break;
            }

            return base.RetornarSucessoAjax(this.SessaoUltimaConsultaOperacoesClienteBmf, "sucesso");
        }

        private string ResponderOrdenarPorColunaOperacoesBmf()
        {
            switch (this.GetFiltrarPor)
            {
                case "Cliente":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp1.Cliente), int.Parse(lp2.Cliente)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp2.Cliente), int.Parse(lp1.Cliente)));
                    }
                    break;
                case "Contrato":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.Contrato, lp2.Contrato));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.Contrato, lp1.Contrato));
                    }
                    break;

                case "DiferencialPontos":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.DiferencialPontos), decimal.Parse(lp2.DiferencialPontos)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.DiferencialPontos), decimal.Parse(lp1.DiferencialPontos)));
                    }
                    break;
                case "FatorMultiplicador":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.FatorMultiplicador), decimal.Parse(lp2.FatorMultiplicador)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.FatorMultiplicador), decimal.Parse(lp1.FatorMultiplicador)));
                    }
                    break;
                case "LucroPrejuizoContrato":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.LucroPrejuizoContrato), decimal.Parse(lp2.LucroPrejuizoContrato)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.LucroPrejuizoContrato), decimal.Parse(lp1.LucroPrejuizoContrato)));
                    }
                    break;
                case "PrecoContatoMercado":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PrecoContatoMercado), decimal.Parse(lp2.PrecoContatoMercado)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PrecoContatoMercado), decimal.Parse(lp1.PrecoContatoMercado)));
                    }
                    break;
                case "QuantidadeContato":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.QuantidadeContato), decimal.Parse(lp2.QuantidadeContato)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.QuantidadeContato), decimal.Parse(lp1.QuantidadeContato)));
                    }
                    break;
                case "PrecoMedio":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PrecoMedio), decimal.Parse(lp2.PrecoMedio)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PrecoMedio), decimal.Parse(lp1.PrecoMedio)));
                    }
                    break;
                case "Count":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp1.Count), int.Parse(lp2.Count)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(int.Parse(lp2.Count), int.Parse(lp1.Count)));
                    }
                    break;
                case "SubtotalCompra":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SubtotalCompra), decimal.Parse(lp2.SubtotalCompra)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SubtotalCompra), decimal.Parse(lp1.SubtotalCompra)));
                    }
                    break;
                case "SubtotalVenda":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SubtotalVenda), decimal.Parse(lp2.SubtotalVenda)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesClienteBmf.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SubtotalVenda), decimal.Parse(lp1.SubtotalVenda)));
                    }
                    break;
            }

            return base.RetornarSucessoAjax(this.SessaoUltimaConsultaOperacoesClienteBmf, "sucesso");
        }

        private string ResponderOrdenarPorColunaOperacoesBovespa()
        {
            switch (this.GetFiltrarPor)
            {
                case "Instrumento":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.Cliente, lp2.Cliente));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp2.Cliente, lp1.Cliente));
                    }
                    break;
                case "Cliente":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp1.Cliente), int.Parse(lp2.Cliente)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<int>.Default.Compare(int.Parse(lp2.Cliente), int.Parse(lp1.Cliente)));
                    }
                    break;
                case "Cotacao":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.Cotacao), decimal.Parse(lp2.Cotacao)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.Cotacao), decimal.Parse(lp1.Cotacao)));
                    }
                    break;
                case "FinanceiroAbertura":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.FinanceiroAbertura), decimal.Parse(lp2.FinanceiroAbertura)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.FinanceiroAbertura), decimal.Parse(lp1.FinanceiroAbertura)));
                    }
                    break;
                case "FinanceiroComprado":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.FinanceiroComprado), decimal.Parse(lp2.FinanceiroComprado)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.FinanceiroComprado), decimal.Parse(lp1.FinanceiroComprado)));
                    }
                    break;
                case "FinanceiroVendido":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.FinanceiroVendido), decimal.Parse(lp2.FinanceiroVendido)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.FinanceiroVendido), decimal.Parse(lp1.FinanceiroVendido)));
                    }
                    break;
                case "LucroPrejuizo":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.LucroPrejuizo), decimal.Parse(lp2.LucroPrejuizo)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.LucroPrejuizo), decimal.Parse(lp1.LucroPrejuizo)));
                    }
                    break;
                case "NetOperacao":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.NetOperacao), decimal.Parse(lp2.NetOperacao)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.NetOperacao), decimal.Parse(lp1.NetOperacao)));
                    }
                    break;
                case "PrecoReversao":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.PrecoReversao), decimal.Parse(lp2.PrecoReversao)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.PrecoReversao), decimal.Parse(lp1.PrecoReversao)));
                    }
                    break;
                case "QtdeAber":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.QtdeAber), decimal.Parse(lp2.QtdeAber)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.QtdeAber), decimal.Parse(lp1.QtdeAber)));
                    }
                    break;
                case "QtdeAtual":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.QtdeAtual), decimal.Parse(lp2.QtdeAtual)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.QtdeAtual), decimal.Parse(lp1.QtdeAtual)));
                    }
                    break;
                case "QtdeComprada":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.QtdeComprada), decimal.Parse(lp2.QtdeComprada)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.QtdeComprada), decimal.Parse(lp1.QtdeComprada)));
                    }
                    break;
                case "QtdeVendida":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.QtdeVendida), decimal.Parse(lp2.QtdeVendida)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.QtdeVendida), decimal.Parse(lp1.QtdeVendida)));
                    }
                    break;
                case "QtReversao":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.QtReversao), decimal.Parse(lp2.QtReversao)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.QtReversao), decimal.Parse(lp1.QtReversao)));
                    }
                    break;
                case "TipoMercado":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.TipoMercado), decimal.Parse(lp2.TipoMercado)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.TipoMercado), decimal.Parse(lp1.TipoMercado)));
                    }
                    break;
                case "VLMercadoCompra":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.VLMercadoCompra), decimal.Parse(lp2.VLMercadoCompra)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.VLMercadoCompra), decimal.Parse(lp1.VLMercadoCompra)));
                    }
                    break;
                case "VLMercadoVenda":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.VLMercadoVenda), decimal.Parse(lp2.VLMercadoVenda)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.VLMercadoVenda), decimal.Parse(lp1.VLMercadoVenda)));
                    }
                    break;
                case "VLNegocioVenda":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.VLNegocioVenda), decimal.Parse(lp2.VLNegocioVenda)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.VLNegocioVenda), decimal.Parse(lp1.VLNegocioVenda)));
                    }
                    break;
                case "SubtotalCompra":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SubtotalCompra), decimal.Parse(lp2.SubtotalCompra)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SubtotalCompra), decimal.Parse(lp1.SubtotalCompra)));
                    }
                    break;
                case "SubtotalVenda":
                    if (this.GetOrdenacao == "asc")
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp1.SubtotalVenda), decimal.Parse(lp2.SubtotalVenda)));
                    }
                    else
                    {
                        this.SessaoUltimaConsultaOperacoesCliente.Sort((lp1, lp2) => Comparer<decimal>.Default.Compare(decimal.Parse(lp2.SubtotalVenda), decimal.Parse(lp1.SubtotalVenda)));
                    }
                    break;
            }

            return base.RetornarSucessoAjax(this.SessaoUltimaConsultaOperacoesCliente, "sucesso");
        }

        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo> AplicarOrdenacaoDetalhesOperacoesBmf(string lCliente, string lInstrumento)
        {
            List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo> lListaSession =
                Session["ExecBmf_" + lCliente] as List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo>; ;

            IEnumerable<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo> lLista = from a in lListaSession where a.Contrato == lInstrumento select a;

            lListaSession = lLista.ToList();

            lListaSession.Sort((lp1, lp2) => Comparer<string>.Default.Compare(lp1.Sentido, lp2.Sentido));

            return lListaSession;
        }

        private void AplicarOrdenacaoDetalhesOperacoesTermo()
        {
            this.SessaoUltimaConsultaOperacoesTermosClienteDetalhes.Sort((lp1, lp2) => Comparer<DateTime>.Default.Compare(DateTime.Parse(lp1.DataVencimento), DateTime.Parse(lp2.DataVencimento)));
        }

        #endregion

        #region Métodos de Resposta

        #region  métodos de Operações
        private string ResponderBuscarDetalhamentoOperacoesBmf()
        {
            string lRetorno = string.Empty;

            string lInstrumento = string.Empty;

            string lCliente = string.Empty;

            if (this.GetInstrumento != null)
            {
                lInstrumento = this.GetInstrumento;
            }

            if (this.GetCdCliente != null)
            {
                lCliente = this.GetCdCliente.Value.ToString();
            }

            List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo> lList = this.AplicarFiltroInstrumentoBmf(lInstrumento, lCliente);

            List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo> lListFormatada = new List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo>();

            lList = this.AplicarOrdenacaoDetalhesOperacoesBmf(lCliente, lInstrumento);

            lList.ForEach(operacao =>
            {
                operacao.PrecoContatoMercado    = Convert.ToDecimal(operacao.PrecoContatoMercado, gCultura).ToString("N2");
                operacao.LucroPrejuizoContrato  = Convert.ToDecimal(operacao.LucroPrejuizoContrato, gCultura).ToString("N2");
                operacao.PrecoAquisicaoContrato = Convert.ToDecimal(operacao.PrecoAquisicaoContrato, gCultura).ToString("N2");
                operacao.QuantidadeContato      = String.Format(gCultura, "{0:#,0}", int.Parse(operacao.QuantidadeContato.Replace(".", string.Empty)));

                lListFormatada.Add(operacao);
            });

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            lRetornoLista = new TransporteDeListaPaginada(lListFormatada);

            lRetorno = JsonConvert.SerializeObject(lRetornoLista);

            lRetornoLista.TotalDeItens = lList.Count;

            lRetornoLista.PaginaAtual = 1;

            lRetornoLista.TotalDePaginas = 0;

            return lRetorno;
        }

        private string ResponderBuscarDetalhamentoOperacoesBovespa()
        {
            string lRetorno = string.Empty;

            string lInstrumento = string.Empty;

            string lCliente = string.Empty;

            if (this.GetInstrumento != null)
            {
                lInstrumento = this.GetInstrumento;
            }

            if (this.GetCdCliente != null)
            {
                lCliente = this.GetCdCliente.Value.ToString();
            }

            List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo> lList = this.AplicarFiltroInstrumentoBovespa(lInstrumento, lCliente);

            List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo> lListFormatada = new List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo>();

            lList.ForEach(operacao => 
            {
                TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo lOperacao = operacao;
                lOperacao.PrecoMercado  = Convert.ToDecimal(operacao.PrecoMercado, gCultura).ToString("N2");
                lOperacao.LucroPrejuiso = Convert.ToDecimal(operacao.LucroPrejuiso, gCultura).ToString("N2");
                lOperacao.PrecoNegocio  = Convert.ToDecimal(operacao.PrecoNegocio, gCultura).ToString("N2");
                lOperacao.Quantidade    = String.Format(gCultura, "{0:#,0}", int.Parse(operacao.Quantidade.Replace(".",string.Empty), gCultura));

                lListFormatada.Add(lOperacao);
            });

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            lRetornoLista = new TransporteDeListaPaginada(lListFormatada);

            lRetorno = JsonConvert.SerializeObject(lRetornoLista);

            lRetornoLista.TotalDeItens = lList.Count;

            lRetornoLista.PaginaAtual = 1;

            lRetornoLista.TotalDePaginas = 0;

            return lRetorno;
        }

        private string ResponderBuscarOperacoesBMF()
        {
            string lRetorno = string.Empty;

            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

            List<ExposicaoClienteInfo> lstMonitor = new List<ExposicaoClienteInfo>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            if (null != this.GetCdClienteBmf)
            {
                lRequest.Cliente = this.GetCdClienteBmf.Value;
            }
            else
            {
                lRequest.Cliente = this.GetCdCliente.Value;
            }

            if (null != this.GetCdAssessor)
            {
                lRequest.Assessor = this.GetCdAssessor.Value;
            }

            //var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);

            var lRetornoConsulta = Session["Monitor_" + lRequest.Cliente] as MonitorLucroPrejuizoResponse;

            if (lRetornoConsulta != null && lRetornoConsulta.Monitor != null && lRetornoConsulta.Monitor.Count > 0 && lRetornoConsulta.Monitor[0].OrdensBMF != null)
            {
                this.SessaoUltimaConsultaOperacoesClienteBmf = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensBMF);
                
                if (this.GetCdClienteBmf.HasValue)
                {
                    Session["ExecBmf_" + this.GetCdClienteBmf.Value] = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensBMF, string.Empty);
                    Session["OperacoesBmf_" + this.GetCdClienteBmf.Value] = this.SessaoUltimaConsultaOperacoesClienteBmf;
                }

                this.ResponderOrdenarPorColunaOperacoesBmf();

                lRetornoLista = new TransporteDeListaPaginada(this.SessaoUltimaConsultaOperacoesClienteBmf);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = this.SessaoUltimaConsultaOperacoesClienteBmf.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = 0;
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao processar a requisição");
            }

            return lRetorno;
        }

        private string ResponderBuscarOperacoesBovespa()
        {
            string lRetorno = string.Empty;

            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

            List<ExposicaoClienteInfo> lstMonitor = new List<ExposicaoClienteInfo>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            if (null != this.GetCdCliente)
            {
                lRequest.Cliente = this.GetCdCliente.Value;
            }
            else
            {
                var ListaVazia = new List<ClienteRiscoResumo>();

                var ListavaziaOperacoes = new List<OperacoesInfo>();

                this.SessaoUltimaConsultaOperacoesCliente = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(ListaVazia, ListavaziaOperacoes);

                lRetornoLista = new TransporteDeListaPaginada(this.SessaoUltimaConsultaOperacoesCliente);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = this.SessaoUltimaConsultaOperacoesCliente.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = 0; 
            }

            if (null != this.GetCdAssessor)
            {
                lRequest.Assessor = this.GetCdAssessor.Value;
            }

            //var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);

            var lRetornoConsulta = Session["Monitor_" + lRequest.Cliente] as MonitorLucroPrejuizoResponse;

            if (null != lRetornoConsulta && lRetornoConsulta.Monitor != null && lRetornoConsulta.Monitor.Count > 0 && this.GetCdCliente.HasValue)
            {
                this.SessaoUltimaConsultaOperacoesCliente = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].Operacoes, lRetornoConsulta.Monitor[0].OrdensExecutadas);

                if (this.GetCdCliente.HasValue)
                {
                    Session["OperacoesBovespa_" + this.GetCdCliente.Value] = this.SessaoUltimaConsultaOperacoesCliente;
                    Session["ExecBov_" + this.GetCdCliente.Value] = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensExecutadas);
                }

                this.ResponderOrdenarPorColunaOperacoesBovespa();

                //this.AplicarFiltrosDePesquisa();

                lRetornoLista = new TransporteDeListaPaginada(this.SessaoUltimaConsultaOperacoesCliente);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = this.SessaoUltimaConsultaOperacoesCliente.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = 0;
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao processar a requisição");
            }

            return lRetorno;
        }

        private void BuscarOperacoesBovespa(MonitorCustodiaInfo lInfo)
        {
            string lRetorno = string.Empty;

            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

            List<ExposicaoClienteInfo> lstMonitor = new List<ExposicaoClienteInfo>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            if (lInfo.CodigoClienteBov.HasValue)
            {
                lRequest.Cliente = lInfo.CodigoClienteBov.Value;
            }

            //if (lInfo.CodigoAssessor.HasValue)
            //{
            //    lRequest.Assessor = lInfo.CodigoAssessor.Value;
            //}

            //var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);

            var lRetornoConsulta = Session["Monitor_" + lRequest.Cliente] as MonitorLucroPrejuizoResponse;

            if (lRetornoConsulta != null && lRetornoConsulta.Monitor != null && lRetornoConsulta.Monitor.Count > 0 && lRetornoConsulta.Monitor[0].OrdensBMF != null)
            {
                var ListavaziaOperacoes = new List<OperacoesInfo>();

                this.SessaoUltimaConsultaOperacoesCliente = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].Operacoes, ListavaziaOperacoes);

                Session["ExecBov_" + lInfo.CodigoClienteBov] = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensBMF, string.Empty);
                Session["OperacoesBovespa_" + lInfo.CodigoClienteBov] = this.SessaoUltimaConsultaOperacoesCliente;

            }
        }

        private void BuscarOperacoesBMF(MonitorCustodiaInfo lInfo)
        {
            string lRetorno = string.Empty;

            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

            List<ExposicaoClienteInfo> lstMonitor = new List<ExposicaoClienteInfo>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            if (lInfo.CodigoClienteBmf.HasValue)
            {
                lRequest.Cliente = lInfo.CodigoClienteBmf.Value;
            }

            //if (lInfo.CodigoAssessor.HasValue)
            //{
            //    lRequest.Assessor = lInfo.CodigoAssessor.Value;
            //}

            //var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);

            var lRetornoConsulta = Session["Monitor_" + lRequest.Cliente] as MonitorLucroPrejuizoResponse;

            if (lRetornoConsulta != null && lRetornoConsulta.Monitor != null && lRetornoConsulta.Monitor.Count > 0 && lRetornoConsulta.Monitor[0].OrdensBMF != null)
            {
                this.SessaoUltimaConsultaOperacoesClienteBmf = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensBMF);

                if (this.GetCdClienteBmf.HasValue)
                {
                    Session["ExecBmf_" + lInfo.CodigoClienteBmf.Value] = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensBMF, string.Empty);
                    Session["OperacoesBmf_" + lInfo.CodigoClienteBmf.Value] = this.SessaoUltimaConsultaOperacoesClienteBmf;
                }
            }
        }

        private void BuscarOperacoesBovComCodigoCliente(int CodigoClienteBov)
        {
            string lRetorno = string.Empty;

            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

            List<ExposicaoClienteInfo> lstMonitor = new List<ExposicaoClienteInfo>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            lRequest.Cliente = CodigoClienteBov;

            //var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);
            
            var lRetornoConsulta = Session["Monitor_" + lRequest.Cliente] as MonitorLucroPrejuizoResponse;
            
            if (lRetornoConsulta != null && lRetornoConsulta.Monitor != null && lRetornoConsulta.Monitor.Count > 0 && lRetornoConsulta.Monitor[0].OrdensExecutadas != null)
            {
                var ListavaziaOperacoes = new List<OperacoesInfo>();

                this.SessaoUltimaConsultaOperacoesCliente = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].Operacoes, ListavaziaOperacoes);

                if (this.GetCdCliente.HasValue)
                {
                    Session["ExecBov_" + CodigoClienteBov] = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensExecutadas);
                    Session["OperacoesBovespa_" + CodigoClienteBov] = this.SessaoUltimaConsultaOperacoesCliente;
                }
            }
        }

        private void BuscarOperacoesBMFComCodigoCliente(int CodigoClienteBMF)
        {
            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

            List<ExposicaoClienteInfo> lstMonitor = new List<ExposicaoClienteInfo>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            lRequest.Cliente = CodigoClienteBMF;

            //var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);

            var lRetornoConsulta = Session["Monitor_" + lRequest.Cliente] as MonitorLucroPrejuizoResponse;

            if (lRetornoConsulta != null && lRetornoConsulta.Monitor != null && lRetornoConsulta.Monitor.Count > 0 && lRetornoConsulta.Monitor[0].OrdensBMF != null)
            {
                this.SessaoUltimaConsultaOperacoesClienteBmf = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensBMF);

                if (this.GetCdClienteBmf.HasValue)
                {
                    Session["ExecBmf_" + CodigoClienteBMF] = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensBMF, string.Empty);
                    Session["OperacoesBmf_" + CodigoClienteBMF] = this.SessaoUltimaConsultaOperacoesClienteBmf;
                }
            }
        }

        private string ResponderBuscarMonitorRiscoDetalhamentoOperacoes()
        {
            string lRetorno = string.Empty;

            string lInstrumento = string.Empty;

            IServicoMonitorRisco lServico = Ativador.Get<IServicoMonitorRisco>();

            MonitorLucroPrejuizoRequest lRequest = new MonitorLucroPrejuizoRequest();

            List<ExposicaoClienteInfo> lstMonitor = new List<ExposicaoClienteInfo>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            if (this.GetCdCliente != null)
            {
                lRequest.Cliente = this.GetCdCliente.Value;
            }

            if (this.GetCdAssessor != null)
            {
                lRequest.Assessor = this.GetCdAssessor.Value;
            }

            if (this.GetInstrumento != null)
            {
                lInstrumento = this.GetInstrumento;
            }

            //var lRetornoConsulta = lServico.ObterMonitorLucroPrejuizo(lRequest);

            var lRetornoConsulta = Session["Monitor_" + lRequest.Cliente] as MonitorLucroPrejuizoResponse;

            if (null != lRetornoConsulta && null != lRetornoConsulta.Monitor)
            {
                this.SessaoUltimaConsultaOperacoesClienteDetalhe = new TransporteRiscoMonitoramentoLucrosPrejuizos().TraduzirLista(lRetornoConsulta.Monitor[0].OrdensExecutadas);

                this.AplicarFiltroInstrumento(lInstrumento);

                lRetornoLista = new TransporteDeListaPaginada(this.SessaoUltimaConsultaOperacoesClienteDetalhe);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = this.SessaoUltimaConsultaOperacoesClienteDetalhe.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = 0;
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Ocorreu um erro ao processar a requisição");
            }

            return lRetorno;
        }

        private string ResponderBuscarTotalLucroPrejuizoBovespa()
        {
            string lRetorno;

            decimal lTotalLucroPrejuizo = 0;

            decimal lPrecoMedio = 0;

            int lCont = 0;

            if (this.GetCdCliente.HasValue && this.GetCdCliente.Value != 0)
            {
                string lCliente = this.GetCdCliente.Value.ToString();

                lTotalLucroPrejuizo = BuscarTotalLucroPrejuizoBovespaTotal(lCliente);
            }

            TransporteRiscoMonitoramentoLucrosPrejuizos.TotalLucroPrejuizoOperacoes lTransporte
                = TransporteRiscoMonitoramentoLucrosPrejuizos.TraduzirLucroPrejuizo(lTotalLucroPrejuizo, lCont, lPrecoMedio);

            lRetorno = base.RetornarSucessoAjax(lTransporte, "Dados da página listados com sucesso!");

            return lRetorno;
        }

        private string ResponderBuscarTotalLucroPrejuizoBmf()
        {
            string lRetorno;

            decimal lTotalLucroPrejuizo = 0;

            decimal lPrecoMedio = 0;

            int lContratosCount = 0;

            if (this.GetCdClienteBmf.HasValue && this.GetCdClienteBmf.Value != 0)
            {
                string lCliente = this.GetCdClienteBmf.Value.ToString();

                lTotalLucroPrejuizo = BuscarTotalLucroPrejuizoBmfTotal(lCliente);
            }

            TransporteRiscoMonitoramentoLucrosPrejuizos.TotalLucroPrejuizoOperacoes lTransporte
                = TransporteRiscoMonitoramentoLucrosPrejuizos.TraduzirLucroPrejuizo(lTotalLucroPrejuizo, lContratosCount, lPrecoMedio);

            lRetorno = base.RetornarSucessoAjax(lTransporte, "Dados da página listados com sucesso!");

            return lRetorno;
        }

        public decimal BuscarTotalLucroPrejuizoBovespaTotal(string lCliente)
        {
            decimal lTotalLucroPrejuizo = 0;

            List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesClienteInfo> lListaSession =
                Session["OperacoesBovespa_" + lCliente] as List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesClienteInfo>;

            if (lListaSession == null)
            {
                return 0;
            }

            foreach (TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesClienteInfo operacao in lListaSession)
            {
                lTotalLucroPrejuizo += Convert.ToDecimal(operacao.LucroPrejuizo, gCultura);
            }

            return lTotalLucroPrejuizo;
        }

        public decimal BuscarTotalLucroPrejuizoBmfTotal(string lCliente)
        {
            decimal lTotalLucroPrejuizo = 0;

            List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridInfo> lListaSession =
                Session["OperacoesBmf_" + lCliente] as List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridInfo>;

            if (lListaSession == null)
            {
                return 0;
            }

            foreach (TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridInfo operacao in lListaSession)
            {
                lTotalLucroPrejuizo += Convert.ToDecimal(operacao.LucroPrejuizoContrato, gCultura);
            }

            return lTotalLucroPrejuizo;
        }

        public decimal BuscarTotalLucroPrejuizoBmf(string lCliente, string lInstrumento)
        {
            decimal lTotalLucroPrejuizo = 0;

            List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridInfo> lListaSession =
                Session["OperacoesBmf_" + lCliente] as List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridInfo>;

            if (lListaSession == null)
            {
                return 0;
            }

            TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridInfo lOperacao = lListaSession.Find(operacao => operacao.Contrato == lInstrumento);

            lTotalLucroPrejuizo = Convert.ToDecimal(lOperacao.LucroPrejuizoContrato, gCultura);

            return lTotalLucroPrejuizo;
        }
        #endregion

        private string ResponderBuscarDadosClientes()
        {
            string lRetorno = string.Empty;

            if (Session["Usuario"] == null) { return lRetorno; }

            MonitorCustodiaRequest lRequest = new MonitorCustodiaRequest();

            MonitorCustodiaResponse lResponse = new MonitorCustodiaResponse();

            MonitorLucroPrejuizoRequest lRequestMonitor = new MonitorLucroPrejuizoRequest();

            try
            {
                if (this.GetCdCliente != null)
                {
                    lRequest.CodigoCliente  = this.GetCdCliente.Value;
                    
                    lRequestMonitor.Cliente = this.GetCdCliente.Value;
                }

                if (this.CodigoAssessor.HasValue)
                {
                    lRequest.CodAssessor     = this.CodigoAssessor;
                    
                    lRequestMonitor.Assessor = this.CodigoAssessor.Value;
                }
                
                IServicoMonitorRisco lServicoMonitor = Ativador.Get<IServicoMonitorRisco>();
                
                IServicoMonitorCustodia gServicoCustodia = Ativador.Get<IServicoMonitorCustodia>();

                lResponse = gServicoCustodia.ObterMonitorCustodiaMemoria(lRequest);

                var lRetornoMonitor = lServicoMonitor.ObterMonitorLucroPrejuizo(lRequestMonitor);

                Session["Monitor_" + lRequestMonitor.Cliente] = lRetornoMonitor;

                if (lResponse != null && lResponse.MonitorCustodia != null)
                {
                    Session["MonitorCustodiaInfo_" + this.GetCdCliente.Value] = lResponse.MonitorCustodia;

                    if (lResponse.MonitorCustodia.CodigoClienteBmf.HasValue)
                    {
                        this.BuscarOperacoesBMF(lResponse.MonitorCustodia);
                    }

                    if (lResponse.MonitorCustodia.CodigoClienteBov.HasValue)
                    {
                        this.BuscarOperacoesBovespa(lResponse.MonitorCustodia);
                    }

                    try
                    {
                        this.BuscarFundosClubes(lResponse.MonitorCustodia);
                    }
                    catch (Exception ex)
                    {
                        gLogger.Error(string.Format("Erro: {0}\r\n{1}", ex.Message, ex.StackTrace));
                    }

                    lRetorno = base.RetornarSucessoAjax(lResponse, "Sucesso");
                }
                else
                {
                    lRetorno = base.RetornarSucessoAjax("Sucesso");
                }
                //ConsultarObjetosResponse<MonitorRiscoInfo> lResponseMonitor = new PersistenciaDbIntranet().ConsultarObjetos<MonitorRiscoInfo>(lRequest);

                //if (lResponseMonitor != null && lResponseMonitor.Resultado != null && lResponseMonitor.Resultado.Count > 0)
                //{
                //    if (lResponseMonitor.Resultado[0].CodigoClienteBmf.HasValue)
                //    {
                //        this.BuscarOperacoesBMF(lResponseMonitor.Resultado[0]);
                //    }

                //    if (lResponseMonitor.Resultado[0].CodigoCliente.HasValue)
                //    {
                //        this.BuscarOperacoesBovespa(lResponseMonitor.Resultado[0]);
                //    }

                //    this.BuscarFundosClubes(lResponseMonitor.Resultado[0]);

                //    lRetorno = base.RetornarSucessoAjax(lResponseMonitor, "Sucesso");
                //}
                //else
                //{
                //    lRetorno = base.RetornarSucessoAjax("Sucesso");
                //}

            }
            catch (Exception ex)
            {
                gLogger.Error(string.Format("Erro: {0}\r\n{1}", ex.Message, ex.StackTrace));
                //lRetorno = base.RetornarSucessoAjax(ex, "Erro");
            }
            return lRetorno;
        }

        private void BuscarFundosClubes(MonitorCustodiaInfo lInfo)
        {
            if (!lInfo.CodigoClienteBov.HasValue)
            {
                return;
            }

            List<Transporte_PosicaoCotista> lPosicao = base.PosicaoFundosSumarizado(lInfo.CodigoClienteBov.Value, lInfo.CpfCnpj);

            if (lPosicao.Count > 0)
            {
                if (lInfo.CodigoClienteBov.HasValue)
                {
                    Session["Fundos_Clubes_" + lInfo.CodigoClienteBov.Value] = lPosicao;
                }
                else if (lInfo.CodigoClienteBmf.HasValue)
                {
                    Session["Fundos_Clubes_" + lInfo.CodigoClienteBmf.Value] = lPosicao;
                }
            }
        }

        private string ResponderBuscarCustodiaFundos()
        {
            string lRetorno = string.Empty;

            try
            {
                int lCodigoCliente = 0;

                if (this.GetCdCliente.HasValue)
                {
                    lCodigoCliente = this.GetCdCliente.Value;
                }
                else if (this.GetCdClienteBmf.HasValue)
                {
                    lCodigoCliente = this.GetCdClienteBmf.Value;
                }

                var lPosicao = Session["Fundos_Clubes_" + lCodigoCliente] as List<Transporte_PosicaoCotista>;

                var lClubesEFundos = new TransporteRelatorioClubesEFundos();

                lClubesEFundos.ListaFundos = new TransporteRelatorioFundos().TraduzirListaParaTransporteRelatorioFundos(lPosicao);

                TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

                lRetornoLista = new TransporteDeListaPaginada(lClubesEFundos.ListaFundos);

                lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = lClubesEFundos.ListaFundos.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = 0;
                
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Format("Erro encontrado no método de retornar fundos: {0}\r\n{1}", ex.Message, ex.StackTrace));

                lRetorno = RetornarErroAjax("Erro encontrado no método de retornar fundos", ex);
            }

            return lRetorno;
        }

        private string ResponderBuscarCustodiaPosicao()
        {
            
            string lRetorno = string.Empty;

            if (Session["Usuario"] == null) { return lRetorno; }

            List<TransporteCustodiaInfo> lLista = new List<TransporteCustodiaInfo>();

            //IServicoRelatoriosFinanceiros lServico = Ativador.Get<IServicoRelatoriosFinanceiros>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            PosicaoCustodiaRequest lRequest = new PosicaoCustodiaRequest();

            if (this.GetCdClienteBmf.HasValue)
            {
                lRequest.ConsultaCdClienteBMF = this.GetCdClienteBmf;
            }

            if (this.GetCdCliente.HasValue)
            {
                lRequest.ConsultaCdClienteBovespa = this.GetCdCliente.Value;
            }

            MonitorCustodiaInfo lCustodia = new MonitorCustodiaInfo();

            lCustodia = Session["MonitorCustodiaInfo_" + this.GetCdCliente.Value] as MonitorCustodiaInfo;

            //PosicaoCustodiaResponse lResponse = lServico.ConsultarCustodia(lRequest);

            //ReceberEntidadeRequest<MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo> lRequestCustodia = new ReceberEntidadeRequest<MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo>();

            /*
            lRequestCustodia.Objeto = new MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo()
            {
                ConsultaCdClienteBMF = this.GetCdClienteBmf,
                ConsultaCdClienteBovespa = this.GetCdCliente
            };
            */
            
            //ReceberObjetoResponse<MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo> lResponse =new PersistenciaDbIntranet().ReceberObjeto<MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo>(lRequestCustodia);

            //lResponse.Objeto.ListaPosicaoDia = lResponse.Objeto.ListaPosicaoDia.FindAll(cci => "FUT".Equals(cci.TipoMercado.ToUpper()) || "OPF".Equals(cci.TipoMercado.ToUpper()));

            lLista = TransporteCustodiaInfo.TraduzirCustodiaInfo(lCustodia.ListaPosicaoDiaBMF);

            //this.RecuperarValoresUltimaCotacao(ref lLista);

            lRetornoLista = new TransporteDeListaPaginada(lLista);

            lRetorno = JsonConvert.SerializeObject(lRetornoLista);

            lRetornoLista.TotalDeItens = this.SessaoUltimaConsulta.Count;

            lRetornoLista.PaginaAtual = 1;

            lRetornoLista.TotalDePaginas = 0;

            return lRetorno;
        }

        private string ResponderBuscarCustodia()
        {
            string lRetorno = string.Empty;

            if (Session["Usuario"] == null) { return lRetorno; }

            List<TransporteCustodiaInfo> lLista = new List<TransporteCustodiaInfo>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            PosicaoCustodiaRequest lRequest = new PosicaoCustodiaRequest();

            MonitorCustodiaInfo lCustodia = new MonitorCustodiaInfo();
            
            lCustodia = Session["MonitorCustodiaInfo_" + this.GetCdCliente.Value] as MonitorCustodiaInfo;

            List<MonitorCustodiaInfo.CustodiaPosicao> lListaCustodia = new List<MonitorCustodiaInfo.CustodiaPosicao>();

            if (lCustodia.ListaCustodia != null && lCustodia.ListaCustodia.Count > 0)
            {
                switch (this.GetMercado)
                {
                    case "FUT":
                    case "OPF":
                    case "DIS":
                        lListaCustodia = lCustodia.ListaCustodia.FindAll(cci => "FUT".Equals(cci.TipoMercado.ToUpper()) || "OPF".Equals(cci.TipoMercado.ToUpper()) || "DIS".Equals(cci.TipoMercado.ToUpper()));
                        break;
                    case "OPC":
                    case "OPV":
                        lListaCustodia = lCustodia.ListaCustodia.FindAll(cci => "OPC".Equals(cci.TipoMercado.ToUpper()) || "OPV".Equals(cci.TipoMercado.ToUpper()));
                        break;
                    case "TER":
                        lListaCustodia = lCustodia.ListaCustodia.FindAll(cci => "TER".Equals(cci.TipoMercado.ToUpper()));
                        break;
                    case "VIS":
                        lListaCustodia = lCustodia.ListaCustodia.FindAll(cci => "VIS".Equals(cci.TipoMercado.ToUpper()) && !"TEDI".Equals(cci.TipoGrupo.ToUpper()));
                        break;
                    case "BTC":
                        lListaCustodia = lCustodia.ListaCustodia.FindAll(cci => "BTC".Equals(cci.TipoMercado.ToUpper()));
                        break;
                    case "TEDI":
                        lListaCustodia = lCustodia.ListaCustodia.FindAll(cci => cci.TipoGrupo != null && "TEDI".Equals(cci.TipoGrupo.ToUpper()) && cci.QtdeDisponivel != 0);
                        break;
                }
            }

            lLista = TransporteCustodiaInfo.TraduzirCustodiaInfo(lListaCustodia);

            lRetornoLista = new TransporteDeListaPaginada(lLista);

            lRetorno = JsonConvert.SerializeObject(lRetornoLista);

            lRetornoLista.TotalDeItens = this.SessaoUltimaConsulta.Count;

            lRetornoLista.PaginaAtual = 1;

            lRetornoLista.TotalDePaginas = 0;

            return lRetorno;
        }

        private string CarregarHtmlLables()
        {
            string lRetorno = string.Empty;
            
            if (Session["Usuario"] == null) { return lRetorno; }

            var lCultureInfo = new CultureInfo("pt-BR");

            double lSubTotalMercadoTermo      = 0D;
            double lSubTotalMercadoAVista     = 0D;
            double lSubTotalTesouroDireto     = 0D;
            double lSubTotalBTC               = 0D;
            double ldSubTotalMercadoFuturo    = 0D;
            double lSubTotalMercadoDeOpcoes   = 0D;
            double lTotal                     = 0D;
            double lSubTotalTemp              = 0D;
            double ldSubTotalMercadoFuturoD_1 = 0D;

            Thread.Sleep(1100);

            List<string> lstFatorCotacao1000 = new List<string>();

            lstFatorCotacao1000.Add("CEGR3");
            lstFatorCotacao1000.Add("CAFE3");
            lstFatorCotacao1000.Add("CAFE4");
            lstFatorCotacao1000.Add("CBEE3");
            lstFatorCotacao1000.Add("SGEN4");
            lstFatorCotacao1000.Add("PMET6");
            lstFatorCotacao1000.Add("EBTP3");
            lstFatorCotacao1000.Add("EBTP4");
            lstFatorCotacao1000.Add("TOYB3");
            lstFatorCotacao1000.Add("TOYB4");
            lstFatorCotacao1000.Add("FNAM11");
            lstFatorCotacao1000.Add("FNOR11");
            lstFatorCotacao1000.Add("NORD3");

            MonitorLucroPrejuizoRequest lRequestMonitor = new MonitorLucroPrejuizoRequest();

            IServicoMonitorRisco lServicoMonitor = Ativador.Get<IServicoMonitorRisco>();

            if (this.GetCdCliente != null)
            {
                lRequestMonitor.Cliente = this.GetCdCliente.Value;
            }
            else
                if (this.GetCdClienteBmf.HasValue)
                {
                    lRequestMonitor.Cliente = this.GetCdClienteBmf.Value;
                }

            if (this.GetCdAssessor != null)
            {
                lRequestMonitor.Assessor = this.GetCdAssessor.Value;
            }

            //MonitorLucroPrejuizoResponse lResponseMonitorLucroPrejuizo = lServicoMonitor.ObterMonitorLucroPrejuizo(lRequestMonitor);

            MonitorLucroPrejuizoResponse lResponseMonitorLucroPrejuizo = Session["Monitor_" + lRequestMonitor.Cliente] as MonitorLucroPrejuizoResponse;

            IServicoExtratos lServicoLiquidacao = Ativador.Get<IServicoExtratos>();

            var lRetornoObjeto = new TransporteRiscoMonitoramentoLucrosPrejuizos.MonitoramentoContaCorrenteLimiteGeral();

            MonitorCustodiaInfo lCustodia = new MonitorCustodiaInfo();

            lCustodia = Session["MonitorCustodiaInfo_" + this.GetCdCliente.Value] as MonitorCustodiaInfo;

            lRetornoObjeto.Assessor = lCustodia.CodigoAssessor; //this.GetCdAssessor.ToString();

            lRetornoObjeto.CdCliente = lCustodia.CodigoClienteBov.ToString(); //this.GetCdCliente.ToString();

            lRetornoObjeto.NmCliente = "";

            lRetornoObjeto.DataAtual = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            //Valores de Custódia
            if (lCustodia != null && lCustodia.ListaCustodia != null && lCustodia.ListaCustodia.Count > 0)
            {
                var lCustodias = TransporteCustodiaInfo.TraduzirCustodiaInfo(lCustodia.ListaCustodia);

                //this.RecuperarValoresUltimaCotacao(ref lCustodias);

                lCustodias.ForEach(delegate(TransporteCustodiaInfo info)
                {
                    if (info.Cotacao != "0,00")
                    {
                        if (info.TipoMercado.Equals("TER") || info.TipoMercado.Equals("BTC"))
                        {
                            lSubTotalTemp = (Convert.ToDouble(info.Cotacao) * Convert.ToInt32(info.QtdAtual.Replace(".", string.Empty)));
                        }
                    }
                    else if (info.TipoGrupo == "TEDI" && info.QtdAtual != "0,00")
                    {
                        lSubTotalTesouroDireto += Convert.ToDouble(info.ValorPosicao, new CultureInfo("pt-BR"));
                    }
                    else
                    {
                        lSubTotalTemp = 0;
                    }

                    //info.Resultado = lSubTotalTemp.ToString();

                    if (info.TipoMercado.Equals("VIS") && info.TipoGrupo != "TEDI")
                    {
                        lSubTotalMercadoAVista += Convert.ToDouble( info.Resultado, gCultura);
                    }
                    else if (info.TipoMercado.Equals("TER"))
                    {
                        lSubTotalMercadoTermo += lSubTotalTemp; 
                    }
                    else if (info.TipoMercado.Equals("OPC") || info.TipoMercado.Equals("OPV"))
                    {
                        lSubTotalMercadoDeOpcoes += Convert.ToDouble(info.Resultado, gCultura);
                    }
                    else if (info.TipoMercado.Equals("BTC"))
                    {
                        lSubTotalBTC += lSubTotalTemp;
                    }
                    else if ((info.TipoMercado.Equals("FUT") || info.TipoMercado.Equals("OPF")) || info.TipoMercado.Equals("DIS") && info.Cotacao != "n/d")
                    {
                        ldSubTotalMercadoFuturo += Convert.ToDouble(info.Resultado, gCultura);

                        ldSubTotalMercadoFuturoD_1 += Convert.ToDouble(info.Resultado, gCultura);
                    }

                    lTotal += lSubTotalTemp;
                });
            }

            if (this.GetCdClienteBmf.HasValue)
            {
                if (Session["OperacoesBmf_" + this.GetCdClienteBmf.Value] == null)
                {
                    BuscarOperacoesBMFComCodigoCliente(this.GetCdClienteBmf.Value);
                }

                decimal lTotalLucroPrejuizoBmf = BuscarTotalLucroPrejuizoBmfTotal(this.GetCdClienteBmf.Value.ToString());

                if (lTotalLucroPrejuizoBmf != 0)
                {
                    ldSubTotalMercadoFuturo += Convert.ToDouble(lTotalLucroPrejuizoBmf, gCultura);
                }
            }

            if (this.GetCdCliente.HasValue)
            {
                if (Session["OperacoesBov_" + this.GetCdCliente.Value] == null)
                {
                    BuscarOperacoesBovComCodigoCliente(this.GetCdCliente.Value);
                }

                List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo> lListaSession =
                Session["ExecBov_" + this.GetCdCliente.Value] as List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo>;

                if (lListaSession != null && lListaSession.Count > 0)
                {
                    List<string> lListaPortas = new List<string>();

                    lListaSession.ForEach(operacao =>
                    {
                        string lPorta = operacao.Porta.Substring(0, operacao.Porta.IndexOf(' '));
                        
                        if (!lListaPortas.Contains(lPorta))
                        {
                            lListaPortas.Add(lPorta);
                        }
                    });

                    lRetornoObjeto.ListaPortas = "Portas operadas (Bovespa) : ";

                    lListaPortas.ForEach(portas=> 
                        {
                            lRetornoObjeto.ListaPortas = string.Concat(lRetornoObjeto.ListaPortas, portas, " - ");
                        });

                    lRetornoObjeto.ListaPortas = lRetornoObjeto.ListaPortas.Remove(lRetornoObjeto.ListaPortas.LastIndexOf('-') - 1, 3);
                }
            }

            if (this.GetCdCliente.HasValue)
            {
                lRetornoObjeto.DigitoCodigoCliente = this.GetCdCliente.Value.ToCodigoClienteFormatado();
            }

            lRetornoObjeto.CustodiaSubTotalBmfD_1 = ldSubTotalMercadoFuturoD_1.ToString("N2", lCultureInfo);

            lRetornoObjeto.CustodiaSubTotalBmf = ldSubTotalMercadoFuturo.ToString("N2", lCultureInfo);

            lRetornoObjeto.CustodiaSubTotalOpcoes = lSubTotalMercadoDeOpcoes.ToString("N2", lCultureInfo);

            lRetornoObjeto.CustodiaSubTotalTermo = lSubTotalMercadoTermo.ToString("N2", lCultureInfo);

            lRetornoObjeto.CustodiaSubTotalAvista = lSubTotalMercadoAVista.ToString("N2", lCultureInfo);

            lRetornoObjeto.CustodiaSubTotalTesouroDireto = lSubTotalTesouroDireto.ToString("N2", lCultureInfo);

            lRetornoObjeto.CustodiaSubTotalBTC = lSubTotalBTC.ToString("N2", lCultureInfo);

            TransporteSaldoDeConta lRetornoSaldo = this.BuscarSaldoELimitesNoServico();

            lRetornoObjeto.ContaCorrenteD0 = lRetornoSaldo.Acoes_SaldoD0.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteD1 = lRetornoSaldo.Acoes_SaldoD1.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteD2 = lRetornoSaldo.Acoes_SaldoD2.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteD3 = lRetornoSaldo.Acoes_SaldoD3.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteDTotal = (lRetornoSaldo.Acoes_SaldoD0 + lRetornoSaldo.Acoes_SaldoD1 + lRetornoSaldo.Acoes_SaldoD2 + lRetornoSaldo.Acoes_SaldoD3).ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteDisponivelAvista = lRetornoSaldo.Limite_CompraAVista_Disponivel.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteDisponivelBmf = (lRetornoSaldo.BMF_DisponivelParaResgate).ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteGarantiaBmf = lRetornoSaldo.BMF_SaldoMargem.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteContaMargem = lRetornoSaldo.Acoes_SaldoContaMargem.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteDisponivelOpcoes = lRetornoSaldo.Limite_CompraOpcoes_Disponivel.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteCompraAvista = lRetornoSaldo.Limite_CompraAVista_Total.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteCompraOpcao = lRetornoSaldo.Limite_CompraOpcoes_Total.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteVendaAvista = lRetornoSaldo.Limite_VendaAVista_Total.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteVendaOpcao = lRetornoSaldo.Limite_VendaOpcoes_Total.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteProjetado = (lRetornoSaldo.Acoes_SaldoD0 +
                                                            lRetornoSaldo.Acoes_SaldoD1 +
                                                            lRetornoSaldo.Acoes_SaldoD2 +
                                                            lRetornoSaldo.Acoes_SaldoD3 +
                                                            lRetornoSaldo.Acoes_SaldoContaMargem).ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteMargemRequeriada = lCustodia.ValorMargemRequerida.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteGarantiaBovespa = lCustodia.ValorGarantiaDeposito.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteMargemRequeridaBovespa = lCustodia.ValorMargemRequeridaBovespa.ToString("N2", lCultureInfo);

            lRetornoObjeto.ContaCorrenteDisponivelBovespa = (Convert.ToDecimal(lRetornoObjeto.ContaCorrenteGarantiaBovespa, gCultura) - Convert.ToDecimal(lRetornoObjeto.ContaCorrenteMargemRequeridaBovespa, gCultura)).ToString("N2");

            lRetornoObjeto.Assessor = lCustodia.CodigoAssessor;

            lRetornoObjeto.NmCliente =lCustodia.NomeCliente;

            lRetornoObjeto.StatusBmf = lCustodia.StatusBmf;

            lRetornoObjeto.StatusBovespa = lCustodia.StatusBov;

            lRetornoObjeto.DataAtual = DateTime.Now.ToString("dd/MM/yyyy");

            lRetornoObjeto.OperacoesSubtotalTermo = "0,00";

            lRetornoObjeto.OperacoesSubtotalBTC = "0,00";

            lRetornoObjeto.ListaGarantiasBMF = new List<TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoFinanceiroGridInfo>();

            foreach (MonitorCustodiaInfo.CustodiaGarantiaBMFOuro lGarantia in lCustodia.ListaGarantiasBMFOuro)
            {
                TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoFinanceiroGridInfo lInfoGarantia = new TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoFinanceiroGridInfo();

                lInfoGarantia.DescricaoGarantia = lGarantia.DescricaoGarantia;

                lInfoGarantia.ValorGarantiaDeposito = lGarantia.ValorGarantiaDeposito.ToString("N2");

                lRetornoObjeto.ListaGarantiasBMF.Add(lInfoGarantia);
            }
            
            if (lRetornoObjeto.ListaGarantiasBMF == null)
            {
                lRetornoObjeto.ListaGarantiasBMF = new List<TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoFinanceiroGridInfo>();
            }

            foreach (MonitorCustodiaInfo.CustodiaGarantiaBMF lGarantia in lCustodia.ListaGarantias)
            {
                TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoFinanceiroGridInfo lInfoGarantia = new TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoFinanceiroGridInfo();

                lInfoGarantia.DescricaoGarantia = lGarantia.DescricaoGarantia;

                lInfoGarantia.ValorGarantiaDeposito = lGarantia.ValorGarantiaDeposito.ToString("N2");

                lRetornoObjeto.ListaGarantiasBMF.Add(lInfoGarantia);
            }
            
            lRetornoObjeto.ListaGarantiasBOV = new List<TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoFinanceiroGridInfo>();

            foreach (MonitorCustodiaInfo.CustodiaGarantiaBovespa lGarantia in lCustodia.ListaGarantiasBovespa)
            {
                TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoFinanceiroGridInfo lInfoGarantia = new TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoFinanceiroGridInfo();

                lInfoGarantia.DescricaoGarantia = lGarantia.DescricaoGarantia;

                lInfoGarantia.ValorGarantiaDeposito = lGarantia.ValorGarantiaDeposito.ToString("N2");

                lInfoGarantia.CodigoAtividade = lGarantia.CodigoAtividade;

                lInfoGarantia.CodigoDistribuicao = lGarantia.CodigoDistribuicao.ToString();

                lInfoGarantia.CodigoIsin = lGarantia.CodigoIsin;

                lInfoGarantia.Quantidade = lGarantia.Quantidade.ToString();

                lInfoGarantia.DtDeposito = lGarantia.DtDeposito.ToString("dd/MM/yyyy");

                lInfoGarantia.FinalidadeGarantia = lGarantia.FinalidadeGarantia;

                lInfoGarantia.NomeEmpresa = lGarantia.NomeEmpresa;

                lRetornoObjeto.ListaGarantiasBOV.Add(lInfoGarantia);
            }
            

            if (lResponseMonitorLucroPrejuizo != null && lResponseMonitorLucroPrejuizo.Monitor.Count > 0)
            {
                lRetornoObjeto.Financeiro_SFP = lResponseMonitorLucroPrejuizo.Monitor[0].SituacaoFinanceiraPatrimonial.ToString("N2", lCultureInfo);

                decimal lSubtotalTermo = 0.0M;

                decimal lSubtotalBTC = 0.0M;

                if (lResponseMonitorLucroPrejuizo.Monitor[0].OrdensTermo != null)
                {
                    foreach (PosicaoTermoInfo info in lResponseMonitorLucroPrejuizo.Monitor[0].OrdensTermo)
                    {
                        lSubtotalTermo += info.LucroPrejuizo;
                    }
                }

                if (lResponseMonitorLucroPrejuizo.Monitor[0].OrdensBTC != null)
                {
                    foreach (BTCInfo info in lResponseMonitorLucroPrejuizo.Monitor[0].OrdensBTC)
                    {
                        if (lstFatorCotacao1000.Contains(info.Instrumento))
                        {
                            lSubtotalBTC += ((info.PrecoMercado * info.Quantidade)/1000);
                        }
                        else 
                        {
                            lSubtotalBTC += (info.PrecoMercado * info.Quantidade);
                        }
                    }
                }

                lRetornoObjeto.OperacoesSubtotalTermo = lSubtotalTermo.ToString("N2");

                lRetornoObjeto.OperacoesSubtotalBTC = lSubtotalBTC.ToString("N2");
            }
            else
            {
                lRetornoObjeto.Financeiro_SFP = "0,00";
            }

            var lRespostaBusca = lServicoLiquidacao.ConsultarExtratoContaCorrente(
                new ContaCorrenteExtratoRequest()
                {
                    ConsultaTipoExtratoDeConta = EnumTipoExtradoDeConta.Liquidacao,
                    ConsultaCodigoCliente = this.GetCdCliente,
                    //ConsultaNomeCliente        = this.GetNomeCliente,
                    ConsultaDataInicio = DateTime.Now.AddDays(-4).Date,
                    ConsultaDataFim = DateTime.Now,
                });

            if (lRespostaBusca.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
            {
                TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoExtratoLiquidacao lExtrato =
                    new TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoExtratoLiquidacao();

                lExtrato.ListaExtratoMovimento = new List<TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoExtratoLiquidacaoMovimento>();

                foreach (ContaCorrenteMovimentoInfo extrato in lRespostaBusca.Relatorio.ListaContaCorrenteMovimento)
                {
                    lExtrato.ListaExtratoMovimento.Add(new TransporteRiscoMonitoramentoLucrosPrejuizos.MonitorRiscoExtratoLiquidacaoMovimento(extrato));
                }

                lExtrato.TotalCliente            = lRespostaBusca.Relatorio.SaldoTotal.ToString("N2");
                lExtrato.ValorDisponivel         = lRespostaBusca.Relatorio.SaldoDisponivel.ToString("N2");
                lExtrato.SaldoAnterior           = lRespostaBusca.Relatorio.SaldoAnterior.ToString("N2");
                lRetornoObjeto.ExtratoLiquidacao = lExtrato;
            }

            if (this.GetCdCliente.HasValue)
            {
                decimal lSubFundosClubes = 0.0M;

                if (Session["Fundos_Clubes_" + this.GetCdCliente.Value] != null)
                {
                    var lClubesEFundos = new TransporteRelatorioClubesEFundos();

                    var lListaFundos = Session["Fundos_Clubes_" + this.GetCdCliente.Value] as List<Transporte_PosicaoCotista>;

                    lClubesEFundos.ListaFundos = new TransporteRelatorioFundos().TraduzirListaParaTransporteRelatorioFundos(lListaFundos);
                    
                    

                     lClubesEFundos.ListaFundos.ForEach(fundos=>
                     {
                         lSubFundosClubes += Convert.ToDecimal(fundos.ValorLiquido, gCultura);
                     });
                }

                lRetornoObjeto.CustodiaSubTotalClubesFundos = lSubFundosClubes.ToString("N2");
            }

            decimal SubtotalRendaFixa = 0.0M;

            var lRequestRendaFixa = new ConsultarEntidadeCadastroRequest<RendaFixaInfo>();

            lRequestRendaFixa.EntidadeCadastro = new RendaFixaInfo();

            lRequestRendaFixa.EntidadeCadastro.CodigoCliente = this.GetCdCliente.HasValue ? Convert.ToInt32(this.GetCdCliente.Value) : 0;//.DBToInt32();

            ConsultarEntidadeCadastroResponse<RendaFixaInfo> lPosicaoRendaFixa = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RendaFixaInfo>(lRequestRendaFixa);

            if (lPosicaoRendaFixa != null && lPosicaoRendaFixa.Resultado.Count > 0)
            {
                foreach (RendaFixaInfo renda in lPosicaoRendaFixa.Resultado)
                {
                    SubtotalRendaFixa += renda.SaldoLiquido;
                }
            }

            lRetornoObjeto.CustodiaSubTotalRendaFixa = SubtotalRendaFixa.ToString("N2");

            lRetorno = base.RetornarSucessoAjax(lRetornoObjeto, "Sucesso");

            return lRetorno;
        }

        private decimal RetornaFatorMultiplicador(string pInstrumento, double? taxaOperada, double? taxaMercado)
        {
            decimal lRetorno = 0.0M;

            string ClassificacaoInstrumento = pInstrumento.Substring(0, 3);

            switch (ClassificacaoInstrumento)
            {
                case INDICE:
                    lRetorno = 1 ;
                    break;
                case DOLAR:
                    lRetorno = 50;
                    break;
                case DI:
                    {
                        double lRetornoTemp = base.CalcularTaxaDI(pInstrumento, taxaOperada.Value, taxaMercado.Value);

                        lRetorno = Convert.ToDecimal(lRetornoTemp, gCultura);
                    }
                    break;
                case CHEIOBOI:
                    lRetorno = 330;
                    break;
                case MINIDOLAR:
                case MINIDOLARFUT:
                    lRetorno = 10;
                    break;
                case MINIBOLSA:
                    lRetorno = 0.2M;
                    break;

                case MINIBOI:
                    lRetorno = 33;
                    break;
                case EURO:
                    lRetorno = 50;
                    break;
                case MINIEURO:
                    lRetorno = 10;
                    break;
                case CAFE:
                    lRetorno = base.CalcularTaxaPtax(100);
                    break;
                case MINICAFE:
                    lRetorno = base.CalcularTaxaPtax(10);
                    break;
                case FUTUROACUCAR:
                    lRetorno = base.CalcularTaxaPtax(270);
                    break;
                case ETANOL:
                    lRetorno = 30;
                    break;
                case ETANOLFISICO:
                    lRetorno = 30;
                    break;
                case MILHO:
                    lRetorno = 450;
                    break;
                case SOJA:
                    lRetorno = base.CalcularTaxaPtax(450);
                    break;
                case OURO:
                    lRetorno = 249.75M;
                    break;
                case ROLAGEMDOLAR:
                    lRetorno = 50;
                    break;
                case ROLAGEMINDICE:
                    lRetorno = 1;
                    break;
                case ROLAGEMBOI:
                    lRetorno = 330;
                    break;
                case ROLAGEMCAFE:
                    lRetorno = 100;
                    break;
                case ROLAGEMMILHO:
                    lRetorno = 450;
                    break;
                case ROLAGEMSOJA:
                    lRetorno = base.CalcularTaxaPtax(450);
                    break;
            }

            return lRetorno;
        }

        private string ResponderBuscarCustodiaSemCarteira()
        {
            string lRetorno = string.Empty;

            if (Session["Usuario"] == null) { return lRetorno; }

            List<TransporteCustodiaInfo> lLista = new List<TransporteCustodiaInfo>();

            TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

            PosicaoCustodiaRequest lRequest = new PosicaoCustodiaRequest();

            MonitorCustodiaInfo lCustodia = new MonitorCustodiaInfo();

            lCustodia = Session["MonitorCustodiaInfo_" + this.GetCdCliente.Value] as MonitorCustodiaInfo;

            List<MonitorCustodiaInfo.CustodiaPosicao> lListaCustodia = new List<MonitorCustodiaInfo.CustodiaPosicao>();

            if (lCustodia.ListaCustodiaSemCarteira != null && lCustodia.ListaCustodiaSemCarteira.Count > 0)
            {
                switch (this.GetMercado)
                {
                    case "FUT":
                    case "OPF":
                    case "DIS":
                        lListaCustodia = lCustodia.ListaCustodiaSemCarteira.FindAll(cci => "FUT".Equals(cci.TipoMercado.ToUpper()) || "OPF".Equals(cci.TipoMercado.ToUpper()) || "DIS".Equals(cci.TipoMercado.ToUpper()));
                        break;
                    case "OPC":
                    case "OPV":
                        lListaCustodia = lCustodia.ListaCustodiaSemCarteira.FindAll(cci => "OPC".Equals(cci.TipoMercado.ToUpper()) || "OPV".Equals(cci.TipoMercado.ToUpper()));
                        break;
                    case "TER":
                        lListaCustodia = lCustodia.ListaCustodiaSemCarteira.FindAll(cci => "TER".Equals(cci.TipoMercado.ToUpper()));
                        break;
                    case "VIS":
                        lListaCustodia = lCustodia.ListaCustodiaSemCarteira.FindAll(cci => "VIS".Equals(cci.TipoMercado.ToUpper()) && !"TEDI".Equals(cci.TipoGrupo.ToUpper()));
                        break;
                    case "BTC":
                        lListaCustodia = lCustodia.ListaCustodiaSemCarteira.FindAll(cci => "BTC".Equals(cci.TipoMercado.ToUpper()));
                        break;
                    case "TEDI":
                        lListaCustodia = lCustodia.ListaCustodia.FindAll(cci => cci.TipoGrupo != null && "TEDI".Equals(cci.TipoGrupo.ToUpper()) && cci.QtdeDisponivel != 0);
                        break;
                }
            }

            lLista = TransporteCustodiaInfo.TraduzirCustodiaInfo(lListaCustodia);

            lRetornoLista = new TransporteDeListaPaginada(lLista);

            lRetorno = JsonConvert.SerializeObject(lRetornoLista);

            lRetornoLista.TotalDeItens = this.SessaoUltimaConsulta.Count;

            lRetornoLista.PaginaAtual = 1;

            lRetornoLista.TotalDePaginas = 0;

            return lRetorno;
        }

        
        #endregion

        #region Métodos internos
        private TransporteSaldoDeConta BuscarSaldoEmContaNoServico()
        {
            TransporteSaldoDeConta lRetorno = new TransporteSaldoDeConta();

            try
            {
                IServicoContaCorrente servicoCC = this.InstanciarServico<IServicoContaCorrente>();

                SaldoContaCorrenteResponse<Gradual.OMS.ContaCorrente.Lib.ContaCorrenteInfo> resCC = servicoCC.ObterSaldoContaCorrente(new SaldoContaCorrenteRequest()
                {
                    IdCliente = this.GetCdCliente.Value
                });

                SaldoContaCorrenteResponse<ContaCorrenteBMFInfo> resBMF = servicoCC.ObterSaldoContaCorrenteBMF(new SaldoContaCorrenteRequest()
                {
                    IdCliente = this.GetCdClienteBmf.HasValue ? this.GetCdClienteBmf.Value : 0
                });

                if (resCC.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK && resBMF.StatusResposta != OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                {
                    lRetorno = new TransporteSaldoDeConta(resCC.Objeto);
                }
                else if (resCC.StatusResposta != OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK && resBMF.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                {
                    lRetorno = new TransporteSaldoDeConta(resBMF.Objeto);
                }
                else if (resCC.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK && resBMF.StatusResposta == OMS.ContaCorrente.Lib.Enum.CriticaMensagemEnum.OK)
                {
                    lRetorno = new TransporteSaldoDeConta(resCC.Objeto, resBMF.Objeto);
                }
                else
                {
                    gLogger.Error(string.Format("Erro: {0}\r\n{1}", resCC.StatusResposta, resCC.DescricaoResposta));
                }
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Format("Erro: {0}\r\n{1}", ex.Message, ex.StackTrace));
            }

            return lRetorno;
        }

        private TransporteSaldoDeConta BuscarSaldoELimitesNoServico()
        {
            TransporteSaldoDeConta lSaldo = this.BuscarSaldoEmContaNoServico();

            RiscoLimiteAlocadoInfo lEntidadeCadastro = new RiscoLimiteAlocadoInfo();

            if (this.GetCdCliente.HasValue)
            {
                lEntidadeCadastro.ConsultaIdCliente = this.GetCdCliente.Value;
            }
            else if (this.GetCdClienteBmf.HasValue)
            {
                lEntidadeCadastro.ConsultaIdCliente = this.GetCdClienteBmf.Value;
            }

            lEntidadeCadastro.NovoOMS = true;

            var lLimitesDoCliente = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoLimiteAlocadoInfo>(new ConsultarEntidadeCadastroRequest<RiscoLimiteAlocadoInfo>()
            {
                EntidadeCadastro = lEntidadeCadastro 
            });

            if (lLimitesDoCliente.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                lSaldo.CarregarDadosDeLimite(lLimitesDoCliente.Resultado);
            }
            //else
            //{
            //    //TODO: Erro!
            //}

            return lSaldo;
        }

        private void AplicarFiltroInstrumento(string lInstrumento)
        {
            IEnumerable<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo> lLista = from a in gMonitoramentoOperacoesDetalhesCliente where a.Instrumento == lInstrumento select a;

            this.SessaoUltimaConsultaOperacoesClienteDetalhe = lLista.ToList();
        }

        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo> AplicarFiltroInstrumentoBovespa(string lInstrumento, string lCliente)
        {
            List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo> lListaSession =
                Session["ExecBov_" + lCliente] as List<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo>;

            IEnumerable<TransporteRiscoMonitoramentoLucrosPrejuizos.OperacoesDetalhesClienteInfo> lLista = from a in lListaSession where a.Instrumento == lInstrumento select a;

            return lLista.ToList();
        }

        private List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo> AplicarFiltroInstrumentoBmf(string lInstrumento, string lCliente)
        {
            List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo> lListaSession =
                Session["ExecBmf_" + lCliente] as List<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo>;

            IEnumerable<TransporteRiscoMonitoramentoLucrosPrejuizos.OrdensExecultadasBmfGridDetalhesInfo> lLista = from a in lListaSession where a.Contrato == lInstrumento select a;

            return lLista.ToList();
        }

        private void RecuperarValoresUltimaCotacao(ref List<TransporteCustodiaInfo> pListaCustodias)
        {
            if (pListaCustodias != null)
            {
                Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteMensagemDeNegocio lCotacao = null;

                var lServico = this.InstanciarServico<IServicoCotacao>();

                foreach (TransporteCustodiaInfo lTransporteCustodia in pListaCustodias)
                {
                    try
                    {

                        if (!lTransporteCustodia.TipoGrupo.Equals("TEDI"))
                        {
                            lCotacao = new Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteMensagemDeNegocio(lServico.ReceberTickerCotacao(lTransporteCustodia.CodigoNegocio));

                            lTransporteCustodia.ValorDeFechamento = Convert.ToDecimal(lCotacao.ValorFechamento, gCultura).ToString("N2");

                            lTransporteCustodia.Cotacao = Convert.ToDecimal(lCotacao.Preco).ToString("N2");

                            lTransporteCustodia.Variacao = lCotacao.Variacao;

                            //lCotacao.PrecoDeExercico
                        }
                        

                        try
                        {
                            if (lTransporteCustodia.TipoGrupo.Equals("TEDI"))
                            {
                                lTransporteCustodia.Resultado = Convert.ToDecimal(lTransporteCustodia.ValorPosicao, gCultura).ToString("N2") ;
                            }
                            else if ((lTransporteCustodia.TipoMercado.Equals("FUT") || lTransporteCustodia.TipoMercado.Equals("OPF")) && lTransporteCustodia.Cotacao != "n/d")
                            {
                                if (!lTransporteCustodia.EhPosicao)
                                {
                                    decimal lQuantidadeAtual      = Convert.ToDecimal(Convert.ToInt32(lTransporteCustodia.QtdAtual).ToString(), gCultura);

                                    decimal lValorCotacao         = decimal.Parse(lTransporteCustodia.Cotacao, gCultura) ;

                                    decimal lFatorMultiplicador = RetornaFatorMultiplicador(lTransporteCustodia.CodigoNegocio, Convert.ToDouble(lTransporteCustodia.ValorDeFechamento, gCultura), Convert.ToDouble(lValorCotacao, gCultura));

                                    decimal lValorPosicao         = Convert.ToDecimal(lTransporteCustodia.ValorDeFechamento, gCultura);

                                    decimal lDiferenca            = Convert.ToDecimal((lValorCotacao - lValorPosicao), gCultura);

                                    //lTransporteCustodia.ValorDeFechamento = Convert.ToDecimal(lCotacao.ValorAbertura, gCultura).ToString("N2");

                                    lTransporteCustodia.Resultado = ((lDiferenca * lFatorMultiplicador) * lQuantidadeAtual).ToString("N2");
                                }
                                else
                                {
                                    if (Session["OperacoesBmf_" + lTransporteCustodia.CodigoCliente] == null)
                                    {
                                        this.BuscarOperacoesBMFComCodigoCliente(int.Parse(lTransporteCustodia.CodigoCliente));
                                    }

                                    decimal lTotalLucroPrejuzo = BuscarTotalLucroPrejuizoBmf(lTransporteCustodia.CodigoCliente, lTransporteCustodia.CodigoNegocio);

                                    lTransporteCustodia.ValorDeFechamento = Convert.ToDecimal(lCotacao.ValorAbertura, gCultura).ToString("N2");

                                    lTransporteCustodia.Resultado = lTotalLucroPrejuzo.ToString("N2");
                                }
                            }
                            else
                            {
                                lTransporteCustodia.Resultado = ( ((double)double.Parse(lCotacao.Preco) * double.Parse(lTransporteCustodia.QtdAtual)) / double.Parse( lTransporteCustodia.FatorCotacao)).ToString("n");
                            }
                        }
                        catch (Exception ex)
                        {
                            gLogger.Error("Erro na página Custodia.aspx", ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        gLogger.Error("Erro em RecuperarValoresUltimaCotacao() na página Custodia.aspx", ex);
                    }
                }
            }
        }
        #endregion
    }
}

