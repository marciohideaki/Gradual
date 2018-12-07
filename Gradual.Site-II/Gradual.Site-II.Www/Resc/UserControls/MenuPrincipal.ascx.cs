using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Site.DbLib.Dados;

namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class MenuPrincipal : UserControlBase
    {
        #region Propriedades

        public TransporteSessaoClienteLogado SessaoClienteLogado
        {
            get { return (TransporteSessaoClienteLogado)this.Session["ClienteLogado"]; }
        }

        public string HostERaiz
        {
            get
            {
                return string.Format("{0}{1}", ConfiguracoesValidadas.HostDoSite, ConfiguracoesValidadas.RaizDoSite);
            }
        }

        public string HostERaizHttps
        {
            get
            {
                string lRetorno = string.Format("{0}{1}", ConfiguracoesValidadas.HostDoSite, ConfiguracoesValidadas.RaizDoSite);

                if (!lRetorno.ToLower().Contains("localhost"))
                {
                    lRetorno = lRetorno.Replace("http:", "https:");
                }

                return lRetorno;
            }
        }

        public string HtmlDoMenuPrincipal
        {
            get
            {
                if (Application["HtmlDoMenuPrincipal"] == null)
                {
                    CarregarMenuPrincipal();
                }

                return Convert.ToString(Application["HtmlDoMenuPrincipal"]);
            }

            set
            {
                Application["HtmlDoMenuPrincipal"] = value;
            }
        }

        #endregion

        #region Métodos Private

        private void CarregarMenuPrincipal()
        {
            ConteudoRequest lRequest = new ConteudoRequest();
            ConteudoResponse lResponse;

            lRequest = new ConteudoRequest();

            lRequest.Conteudo = new ConteudoInfo();

            lRequest.Conteudo.CodigoTipoConteudo = ConfiguracoesValidadas.IdDoTipo_MenuPrincipal;
            lRequest.Conteudo.ValorPropriedade1 = "S";

            lResponse = PaginaBase.ServicoPersistenciaSite.SelecionarConteudoPorPropriedade(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                if (lResponse.ListaConteudo.Count > 0)
                {
                    string lMenuPrincipal = lResponse.ListaConteudo[0].ConteudoHtml;

                    if(ConfiguracoesValidadas.RaizDoSite != "")
                    {
                        lMenuPrincipal = lMenuPrincipal.Replace("href=\"/", ("href=\"/" + ConfiguracoesValidadas.RaizDoSite + "/"));
                    }

                    this.HtmlDoMenuPrincipal = lMenuPrincipal;
                }
                else
                {
                    this.HtmlDoMenuPrincipal = "<!-- Nenhum menu principal retornado como ativo... -->";
                }
            }
            else
            {
                throw new Exception(string.Format("Erro ao carregar menu principal: [{0}]\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
            }
        }

        private void ConfigurarTela()
        {
            //if (this.SessaoClienteLogado == null)
            //{
            //    lnkAbraSuaConta.Attributes["style"] = "";
            //    pnlMinhaconta.Attributes["style"] = "display:none";
            //}
            //else
            //{
            //    lnkAbraSuaConta.Attributes["style"] = "display:none";
            //    pnlMinhaconta.Attributes["style"] = "";
            //}

            litMenuPrincipal.Text = this.HtmlDoMenuPrincipal;
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigurarTela();
        }

        #endregion
    }
}