using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Risco.Lib;
using Gradual.OMS.Risco.Lib.Mensageria;
using Gradual.OMS.Seguranca.Lib;
using Gradual.Spider.PositionClient.DbLib;
using Gradual.Spider.PositionClient.Lib.Messages;
using Gradual.Spider.PostTradingClientEngine.App_Codigo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using appCodigo = Gradual.Spider.PostTradingClientEngine.App_Codigo;
using Gradual.Spider.PositionClient.Monitor.Lib;
using Gradual.Spider.PositionClient.Monitor.Lib.Message;
using Gradual.Spider.PositionClient.Monitor.Lib.Dados;
using Gradual.Spider.PostTradingClientEngine.App_Codigo.TransporteJSon;
using System.Data;

namespace Gradual.Spider.PostTradingClientEngine
{
    /// <summary>
    /// Página para consulta de risco de posção do cliente online
    /// </summary>
    public partial class PositionClient : PaginaBase
    {
        #region Atributos
        
        #endregion

        #region Propriedades
        /// <summary>
        /// Código de cliente para filtro
        /// </summary>
        public int CodigoCliente
        {
            get
            {
                int lRetorno = default(int);

                int.TryParse(this.Request["CodigoCliente"], out lRetorno);

                return lRetorno;
            }
        }

        /// <summary>
        /// Código de Instrumento para filtro
        /// </summary>
        public string CodigoInstrumento
        {
            get
            {
                var lRetorno = default(string);

                if (this.Request["CodigoInstrumento"] != null)
                {
                    lRetorno = this.Request["CodigoInstrumento"].ToString();
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Market Todos os Mercados
        /// </summary>
        public bool OpcaoMarketTodosMercados
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoMarketTodosMercados"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoMarketTodosMercados"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Market A Vista
        /// </summary>
        public bool OpcaoMarketAVista
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoMarketAVista"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoMarketAVista"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Market Futuros
        /// </summary>
        public bool OpcaoMarketFuturos
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoMarketFuturos"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoMarketFuturos"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Opção Market Opcao
        /// </summary>
        public bool OpcaoMarketOpcao
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoMarketOpcao"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoMarketOpcao"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Parametro Intraday Ofertas Pedra
        /// </summary>
        public bool OpcaoParametroIntradayOfertasPedra
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoParametroIntradayOfertasPedra"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoParametroIntradayOfertasPedra"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Parametro Intraday Net Negativo
        /// </summary>
        public bool OpcaoParametroIntradayNetNegativo
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoParametroIntradayNetNegativo"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoParametroIntradayNetNegativo"]);
                }

                return lRetorno;
            }
        }

        /// <summary>
        /// Request Parametro Intraday PL Negativo
        /// </summary>
        public bool OpcaoParametroIntradayPLNegativo
        {
            get
            {
                var lRetorno = default(bool);

                if (this.Request["OpcaoParametroIntradayPLNegativo"] != null)
                {
                    lRetorno = Convert.ToBoolean(this.Request["OpcaoParametroIntradayPLNegativo"]);
                }

                return lRetorno;
            }
        }



        #endregion

        #region PropriedadesAcompanhamento

        

        private static List<Objects.Ordem> gListaOrdens;

        private List<Objects.Ordem> SessionUltimoResultadoDeBuscaAcompanhamento
        {
            get
            {
                return gListaOrdens != null ? gListaOrdens : null;
            }
            set
            {
                gListaOrdens = value;
            }
        }

        

        private static List<Objects.VendaDescoberta> gListaVendasDescobertas;

        private List<Objects.VendaDescoberta> SessionUltimoResultadoDeBuscaVendasDescobertas
        {
            get
            {
                return gListaVendasDescobertas != null ? gListaVendasDescobertas : null;
            }
            set
            {
                gListaVendasDescobertas = value;
        }
        }




        #endregion

        List<ExecBroker> lResposta = new List<ExecBroker>();
        DataTable lData;

        #region Eventos

        /// <summary>
        /// Evento de load da pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                RegistrarRespostasAjax
                    (
                        new string[] 
                            {
                "CarregarOperacoesCliente"
                            }
                            , new ResponderAcaoAjaxDelegate[] 
            {
                ResponderCarregarOperacoesCliente
                            }
                    );

                this.hddConnectionSocketOperacoesIntraday.Value = ConfigurationManager.AppSettings["AddrWebSocketOperacoesIntraday"];
                this.hddConnectionSocketRiscoResumido.Value     = ConfigurationManager.AppSettings["AddrWebSocketConsolidatedRisk"];
                this.hddConnectionRESTRiscoResumido.Value       = ConfigurationManager.AppSettings["AddrWebRESTRiscoResumido"];
                this.hddConnectionRESTOperacoesIntraday.Value   = ConfigurationManager.AppSettings["AddrWebRESTOperacoesIntraday"]; 

                if (!this.IsPostBack)
                {
                    this.CarregarDadosIniciais();
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no Load da positionClient->", ex);
                throw;
            }
        }

        /// <summary>
        /// Evento de Load Complete da página
        /// </summary>
        /// <param name="sender">Não está sendo usado</param>
        /// <param name="e">Não está sendo usado</param>
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Literal litJavascript = (Literal)this.FindControl("litJavascriptOnLoad");

            //if (litJavascript == null && this.PaginaMaster != null)
            //    litJavascript = (Literal)this.PaginaMaster.FindControl("litJavascriptOnLoad");

            if (litJavascript != null)
                litJavascript.Text = this.JavascriptParaRodarOnLoad;
        }

        protected new void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (Application["LogIniciado"] == null)
                {
                    log4net.Config.XmlConfigurator.Configure();

                    Application["LogIniciado"] = true;
                }

                this.CodigoSessao = Request.QueryString["guid"];

                if (Request.QueryString["guid"] != null && Request.QueryString["guid"].Length > 0)
                {
                    this.CodigoSessao = Request.QueryString["guid"];
                }

                if (base.UsuarioLogado == null && (this.CodigoSessao != null && this.CodigoSessao != string.Empty))
                {
                    gLogger.InfoFormat("Iniciando sessão [{0}]", this.CodigoSessao);

                    IServicoSeguranca lServico = this.InstanciarServico<IServicoSeguranca>();

                    ReceberSessaoResponse lSessaoResponse;
                    ReceberSessaoRequest lSessaoRequest;

                    lSessaoRequest = new ReceberSessaoRequest();

                    lSessaoRequest.CodigoSessao = this.CodigoSessao;
                    lSessaoRequest.CodigoSessaoARetornar = this.CodigoSessao;

                    gLogger.DebugFormat("Receber sessão [{0}]", this.CodigoSessao);

                    lSessaoResponse = lServico.ReceberSessao(lSessaoRequest);

                    gLogger.DebugFormat("Resposta: [{0}] [{1}]", lSessaoResponse.StatusResposta, lSessaoResponse.DescricaoResposta);

                    if (lSessaoResponse.Usuario != null)
                    {
                        gLogger.DebugFormat("Usuário: [{0}] [{1}]", lSessaoResponse.Usuario.Nome, lSessaoResponse.Usuario.CodigoUsuario);
                    }
                    else
                    {
                        throw new Exception("Não foi recebido usuário do serviço de segurança!");
                    }

                    IServicoRisco lServicoRisco = Ativador.Get<IServicoRisco>();

                    ContextoOMSInfo lContexto;

                    base.UsuarioLogado = new appCodigo.Usuario();

                    base.UsuarioLogado.CodigoDaSessao = this.CodigoSessao;

                    gLogger.DebugFormat("Buscando contexto [{0}]", this.CodigoSessao);

                    lContexto = lSessaoResponse.Usuario.Complementos.ReceberItem<ContextoOMSInfo>();

                    if (lContexto != null)
                    {
                        base.UsuarioLogado.IdDoUsuario = lContexto.CodigoCBLC;

                        base.UsuarioLogado.AssumirIdentificador(Request.ServerVariables["REMOTE_ADDR"], Request.UserAgent);

                        gLogger.WarnFormat("Verificando presença do ID [{0}], Identificador [{1}], acessando via IP: [{2}]"
                                            , base.UsuarioLogado.IdDoUsuarioTipoInt
                                            , base.UsuarioLogado.IdentificadorDeSessao
                                            , Request.ServerVariables["REMOTE_ADDR"]);

                        if (!UsuarioJaLogado(base.UsuarioLogado.IdDoUsuarioTipoInt, base.UsuarioLogado.IdentificadorDeSessao))
                        {
                            gLogger.WarnFormat("Verificação da presença OK.");

                            base.UsuarioLogado.Nome = lSessaoResponse.Usuario.Nome;

                            //base.UsuarioLogado.CodBovespa = lContexto.CodigoBMF;      //A propriedade "CodigoBMF" na classe ContextoOMSInfo está com nomenclatura incorreta, é mesmo o código Bovespa do usuário (segundo Rafael em 6/1/2011) - Luciano

                            base.UsuarioLogado.CodBovespa = base.UsuarioLogado.IdDoUsuario;

                            ClienteAtividadeBmfRequest lRequestBmf = new ClienteAtividadeBmfRequest();

                            gLogger.DebugFormat("Requisitando código BMF para o código Bovespa [{0}]", base.UsuarioLogado.IdDoUsuario);

                            lRequestBmf.CodigoBase = base.UsuarioLogado.IdDoUsuarioTipoInt;

                            ClienteAtividadeBmfResponse lResponseBmf = lServicoRisco.ObterCodigoClienteAtividadeBmf(lRequestBmf);

                            gLogger.DebugFormat("Recebido código BMF [{0}]", lResponseBmf.CodigoBmf);

                            base.UsuarioLogado.CodBmf = lResponseBmf.CodigoBmf;

                            gLogger.InfoFormat("Logado usuário> Código Bovespa: [{0}], Código BMF: [{1}], IdDoUsuario: [{2}], Nome: [{3}], Sessão: [{4}]"
                                                , UsuarioLogado.CodBovespa
                                                , UsuarioLogado.CodBmf
                                                , UsuarioLogado.IdDoUsuario
                                                , UsuarioLogado.Nome
                                                , UsuarioLogado.CodigoDaSessao);
                        }
                        else
                        {
                            gLogger.WarnFormat("Presença inválida! ID [{0}] fez login a partir do Identificador [{1}]"
                                                , base.UsuarioLogado.IdDoUsuarioTipoInt
                                                , this.UsuariosLogados[base.UsuarioLogado.IdDoUsuarioTipoInt]);

                            Session.Clear();

                            Server.Transfer("JaLogado.aspx");
                        }
                    }
                    else
                    {
                        gLogger.ErrorFormat("Não foi possível buscar o contexto para o usuário na sessão [{0}]", this.CodigoSessao);
                    }

                    DisporServico(lServico);

                    DisporServico(lServicoRisco);
                }
            }
            catch (Exception ex)
            {
                gLogger.Error(string.Format("Erro: {0}\r\n{1}", ex.Message, ex.StackTrace));
            }

            //this.BuscarUltimasDatasNegociacao();

            try
            {
                base.Page_Init(sender, e);
            }
            catch (Exception ex)
            {
                if (ex.Message == CONST_MENSAGEM_SEM_USUARIO_LOGADO)
                {
                    this.UsuarioLogado = new appCodigo.Usuario()
                    {
                        IdDoUsuario = "237",
                        //IdDoUsuario = "1230",
                        Nome = "Login Não Implementado"
                    };
                }
                else
                {
                    gLogger.Error(string.Format("Erro: {0}\r\n{1}", ex.Message, ex.StackTrace));
                }
            }

        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método que efetua de busca de operações no banco de dados
        /// </summary>
        /// <returns>Retorna uma string serializado json</returns>
        public string ResponderCarregarOperacoesCliente()
        {
            string lRetorno = string.Empty;

            try
            {
                var lServico = Ativador.Get<IServicoPositionClientMonitor>();

                var lRequest = new BuscarOperacoesIntradayRequest();

                lRequest.CodigoCliente = CodigoCliente;

                lRequest.Ativo = this.CodigoInstrumento;

                if (this.OpcaoMarketTodosMercados)
                {
                    lRequest.OpcaoMarket = OpcaoMarket.TodosMercados;
                }else 
                {
                    if (this.OpcaoMarketOpcao)
                        lRequest.OpcaoMarket = OpcaoMarket.Opcoes;

                    if ( this.OpcaoMarketAVista )
                        lRequest.OpcaoMarket |= OpcaoMarket.Avista;

                    if ( this.OpcaoMarketFuturos )
                        lRequest.OpcaoMarket |= OpcaoMarket.Futuros;
                }

                if (this.OpcaoParametroIntradayOfertasPedra )
                {
                    lRequest.OpcaoParametrosIntraday |= OpcaoParametrosIntraday.OfertasPedra;
                }

                if ( this.OpcaoParametroIntradayNetNegativo )
                {
                    lRequest.OpcaoParametrosIntraday |= OpcaoParametrosIntraday.NetIntradayNegativo;
                }

                if ( this.OpcaoParametroIntradayPLNegativo )
                {
                    lRequest.OpcaoParametrosIntraday |= OpcaoParametrosIntraday.PLNegativo;
                }
                
                var lResponse = lServico.BuscarOperacoesIntraday(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lResponse.ListOperacoesIntraday, "Foram encontrados [{0}] operacoes" + lResponse.ListOperacoesIntraday.Count, lResponse.ListOperacoesIntraday.Count);
                }
                else
                {
                    lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao deserializar objeto JSON [{0}]", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que carrega os dados iniciais da página necessários 
        /// para funções básicas como carregamento de combos e inicializar contextos javascript.
        /// </summary>
        public void CarregarDadosIniciais()
        {
            string lScript = "PositionClient_Operacoes_Intraday_Grid();";
            lScript += "PositionClient_Risco_Resumido_Grid();";
            //lScript += "PositionClient_Risco_Acompanhamento_Grid();";
            //lScript += "PostTradingClient_Daily_Activity_PerAssetClass_Futures();";
            //lScript += "PostTradingClient_Daily_Activity_BuysSells_Buy();";
            //lScript += "PostTradingClient_Daily_Activity_BuysSells_Sell();";
            //lScript += "PostTradingClient_Daily_Activity_TradeByTrade();";
            //lScript += "PostTradingClient_Daily_Activity_PerAssetClass_Equities();";
            lScript += "ConnectSocketServerOperacoesIntraday();";
            lScript += "ConnectSocketServerRiscoResumido();";
            base.RodarJavascriptOnLoad(lScript);
        }
        #endregion

        public void CarregarExecBroker()
        {
            Gradual.Generico.Dados.AcessaDados lDados = null;
            System.Data.DataTable lTable = null;
            System.Data.Common.DbCommand lCommand = null;

            try
            {
                lDados = new Generico.Dados.AcessaDados();
                lDados.ConnectionStringName = "GradualSpider";

                lCommand = lDados.CreateCommand(System.Data.CommandType.StoredProcedure, "[prc_acompanhamento_buscar_execbroker]");

                lTable = lDados.ExecuteDbDataTable(lCommand);

                List<Objects.Detalhe> lListaOrdens = new List<Objects.Detalhe>();
                lData = lTable;
                foreach (System.Data.DataRow dr in lTable.Rows)
                {
                    lResposta.Add
                        (
                            new ExecBroker
                            {
                                Codigo = dr["ExecBroker"].DBToChar(),
                            }
                        );
                }

                //rptExecBroker.DataSource = lResposta;
                //rptExecBroker.DataBind();
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método CarregarExecBroker ",ex);
                // Not implemented
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }
        }
    }

    public class ExecBroker
    {
        public System.Char Codigo { get; set; }
    }
}