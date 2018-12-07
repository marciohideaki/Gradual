using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.DbLib;
using Gradual.Site.DbLib.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class RodapeDoSite : UserControlBase
    {
        #region Propriedades
        
        public string HostERaiz
        {
            get
            {
                var lHost = ConfiguracoesValidadas.HostDoSite;

                if (Request.ServerVariables["HTTPS"] == "on")
                {
                    lHost = lHost.Replace("http://", "https://");
                }

                return string.Format("{0}{1}", lHost, ConfiguracoesValidadas.RaizDoSite);
            }
        }

        #endregion

        #region Métodos Private

        private void ConfigurarRodape()
        {
            CarregarBanners();

            ConfigurarDisclaimer();
        }

        private void CarregarBanners()
        {
            /*
            BuscarItensDaListaRequest lRequest = new BuscarItensDaListaRequest();
            BuscarItensDaListaResponse lResponse;

            lRequest.IdDaLista = ConfiguracoesValidadas.IdDaLista_BannersProRodape;

            lResponse = this.PaginaBase.ServicoPersistenciaSite.BuscarItensDaLista(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                List<TransporteConteudoInfo_BannerDoRodape> lLista = lResponse.Itens.ParaListaTipada<TransporteConteudoInfo_BannerDoRodape>();

                rptBannersRodape.DataSource = lLista;
                rptBannersRodape.DataBind();
            }
            else
            {
                throw new Exception(string.Format("Resposta do serviço com erro em CarregarBanners() no Rodapé: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta));
            }
             * */
        }

        private void ConfigurarDisclaimer()
        {
            /*
            string lURL = Request.Url.OriginalString.ToLower();

            pnlDisclaimer_AnaliseMercado.Visible = 
            pnlDisclaimer_Ouvidoria.Visible = 
            pnlDisclaimer_Padrao.Visible = 
            pnlDisclaimer_ProdutosTipos.Visible = false;

            if (lURL.Contains("analisesemercado/"))
            {
                pnlDisclaimer_AnaliseMercado.Visible = true;
            }
            else if (lURL.Contains("ouvidoria.aspx"))
            {
                pnlDisclaimer_Ouvidoria.Visible = true;
            }
            else if (lURL.Contains("produtos/") || lURL.Contains("investimentos/"))
            {
                pnlDisclaimer_ProdutosTipos.Visible = true;
            }
            else
            {
                pnlDisclaimer_Padrao.Visible = true;
            }
            */
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigurarRodape();
        }

        #endregion
    }
}