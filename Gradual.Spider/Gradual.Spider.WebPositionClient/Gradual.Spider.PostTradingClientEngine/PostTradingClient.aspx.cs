using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Spider.PostTradingClientEngine.App_Codigo;
using System.Configuration;
using Gradual.Spider.PositionClient.DbLib;
using Gradual.Spider.PositionClient.Lib.Messages;

namespace Gradual.Spider.PostTradingClientEngine
{
    /// <summary>
    /// Página para efetuar consultas de dados de position client
    /// e é usada para efetuar as filtragem de informações de posições de clientes
    /// </summary>
    public partial class PostTradingClient : PaginaBase
    {

        #region Popriedades
        /// <summary>
        /// Request de data "DE" de filtro
        /// </summary>
        public DateTime DataDe
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataDe"], out lRetorno);

                return lRetorno;
            }
        }

        /// <summary>
        /// Request de Data "ATE" de filtro
        /// </summary>
        public DateTime DataAte
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataAte"], out lRetorno);

                return lRetorno;
            }
        }

        /// <summary>
        /// Request de hora "De" de filtro
        /// </summary>
        public DateTime HoraDe
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["HoraDe"], out lRetorno);

                return lRetorno;
            }
        }

        /// <summary>
        /// Request de hora "Ate" de filtro
        /// </summary>
        public DateTime HoraAte
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["HoraAte"], out lRetorno);

                return lRetorno;
            }
        }

        public int CodigoCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CodigoCliente"], out lRetorno);

                return lRetorno;
            }
        }
        #endregion

        #region eventos
        /// <summary>
        /// Evento de load da pagina
        /// </summary>
        /// <param name="sender">Não está sendo usado ainda</param>
        /// <param name="e">Não está sendo usado ainda</param>
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] {
                "CarregarOperacoesCliente"
            }, new ResponderAcaoAjaxDelegate[] 
            {
                ResponderCarregarOperacoesCliente
            });

            string lWebScoket = ConfigurationManager.AppSettings["AddrWebSocket"];

            this.hddConnectionSocket.Value = lWebScoket;

            if (!this.IsPostBack)
            {
                this.CarregarDadosIniciais();
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
        #endregion

        #region Métodos
        /// <summary>
        /// Método que carrega os dados iniciais da página necessários 
        /// para funções básicas como carregamento de combos e inicializar contextos javascript.
        /// </summary>
        public void CarregarDadosIniciais()
        {
            string lScript = "";// "PostTradingClient_Daily_Activity_GeneralView();";
            //lScript += "PostTradingClient_Daily_Activity_PerAssetClass_Equities();";
            ///lScript += "PostTradingClient_Daily_Activity_PerAssetClass_Futures();";
            //lScript += "PostTradingClient_Daily_Activity_BuysSells_Buy();";
            //lScript += "PostTradingClient_Daily_Activity_BuysSells_Sell();";
            //lScript += "PostTradingClient_Daily_Activity_TradeByTrade();";
            lScript += "ConnectSocketServer();";

            base.RodarJavascriptOnLoad(lScript);
        }

        /// <summary>
        /// Método que efetua de busca de operações no banco de dados
        /// </summary>
        /// <returns>Retorna uma string serializado com json</returns>
        public string ResponderCarregarOperacoesCliente()
        {
            string lRetorno = string.Empty;

            try
            {
                var lServico = new PositionClientDbLib();

                var lRequest = new TradeByTradeRequest();

                lRequest.Account = CodigoCliente;

                DateTime lDataDe = new DateTime(DataDe.Year, DataDe.Month, DataDe.Day, HoraDe.Hour, HoraDe.Minute, HoraDe.Second);

                DateTime lDataAte = new DateTime(DataAte.Year, DataAte.Month, DataAte.Day, HoraAte.Hour, HoraAte.Minute, HoraAte.Second);

                lRequest.De = lDataDe;

                lRequest.Ate = lDataAte;

                var lResponse = PositionClientDbLib.BuscarTradeByTrade(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lRetorno = RetornarSucessoAjax(lResponse.ListaTradeByTrade, "Foram encontrados [{0}] operacoes" + lResponse.ListaTradeByTrade.Count);
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
        #endregion
    }
}