using System;
using Newtonsoft.Json;

namespace Gradual.Site.Www
{
    public partial class PaginaInternaMaster : PaginaMasterBase
    {
        #region Propriedades

        public bool EPaginaDoMinhaConta
        {
            get
            {
                string lURL = Request.Url.AbsoluteUri.ToLower();

                return (lURL.Contains("/minhaconta"));
            }
        }

        public bool EPaginaDoMinhaContaMasNaoLogin
        {
            get
            {
                string lURL = Request.Url.AbsoluteUri.ToLower();

                return (lURL.Contains("/minhaconta") && !lURL.Contains("/login.aspx"));
            }
        }
        
        private string _Crumb1Text = "&nbsp;";
        private string _Crumb1Link = "#";
        private string _Crumb2Text = "&nbsp;";
        private string _Crumb2Link = "#";
        private string _Crumb3Text = "&nbsp;";
        private string _Crumb3Link = "#";

        public string Crumb1Text { get { return _Crumb1Text; } set { _Crumb1Text = value; if (!string.IsNullOrEmpty(_Crumb1Text)) this.Crumb1Visible = true; } }
        public string Crumb1Link { get { return _Crumb1Link; } set { _Crumb1Link = value; } }

        public string Crumb2Text { get { return _Crumb2Text; } set { _Crumb2Text = value; if (!string.IsNullOrEmpty(_Crumb2Text)) this.Crumb2Visible = true; } }
        public string Crumb2Link { get { return _Crumb2Link; } set { _Crumb2Link = value; } }

        public string Crumb3Text { get { return _Crumb3Text; } set { _Crumb3Text = value; if (!string.IsNullOrEmpty(_Crumb3Text)) this.Crumb3Visible = true; } }
        public string Crumb3Link { get { return _Crumb3Link; } set { _Crumb3Link = value; } }

        public bool Crumb1Visible { get; set; }
        public bool Crumb2Visible { get; set; }
        public bool Crumb3Visible { get; set; }

        public bool BreadCrumbVisible { get; set; }

        private string _RelatorioDePerformance;

        public string RelatorioDePerformance
        {
            get
            {
                return _RelatorioDePerformance;
            }

            set
            {
                _RelatorioDePerformance = value;

                if (string.IsNullOrEmpty(_RelatorioDePerformance))
                {
                    pnlRelatorioDePerformance.Visible = false;
                }
                else
                {
                    pnlRelatorioDePerformance.InnerHtml = this.RelatorioDePerformance;

                    pnlRelatorioDePerformance.Visible = true;
                }
            }
        }

        public bool ExibindoUmItem { get; set; }

        public bool CarrinhoVisivel
        {
            get
            {
                return Carrinho1.Visible;
            }

            set
            {
                Carrinho1.Visible = value;
            }
        }

        public string DadosDoCarrinho
        {
            get
            {
                if (SessaoClienteLogado != null && SessaoClienteLogado.DadosDoCarrinho != null)
                {
                    return JsonConvert.SerializeObject(SessaoClienteLogado.DadosDoCarrinho);
                }
                else
                {
                    return JsonConvert.SerializeObject(new TransporteDadosCarrinho());
                }
            }
        }

        #endregion

        #region Métodos Private

        private void ConfigurarTela()
        {
            ModuloCMS.Visible = false;

            if (SessaoClienteLogado != null && !this.EPaginaDoMinhaConta)
            {
                if (SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarCMS))
                {
                    ModuloCMS.Visible = true;
                }
                else
                {
                    string lURL = Request.Url.ToString().ToLower();

                    /*
                    if(lURL.Contains("analiseseconomicas.aspx") && SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseEconomica))
                        ModuloCMS.Visible = true;

                    if(lURL.Contains("analisesfundamentalistas.aspx") && SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseFundamentalista))
                        ModuloCMS.Visible = true;

                    if(lURL.Contains("analisesgraficas.aspx") && SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseGrafica))
                        ModuloCMS.Visible = true;

                    if(lURL.Contains("carteirasrecomendadas.aspx") && SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarCarteirasRecomendadas))
                        ModuloCMS.Visible = true;

                    if(lURL.Contains("nikkei") && SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarNikkei))
                        ModuloCMS.Visible = true;

                    if(lURL.Contains("gradiusgestao.aspx") && SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarGradiusGestao))
                        ModuloCMS.Visible = true;
                    */
                }
            }

            mnuWealth.Visible = false;

            //pnlBreadCrumb.Visible = this.BreadCrumbVisible;

            liCrumb1.Visible = this.Crumb1Visible;
            liCrumb2.Visible = this.Crumb2Visible;
            liCrumb3.Visible = this.Crumb3Visible;
            
            pnlBreadCrumb.Visible = false;  //estamos tirando o breadcrumb do site porque precisaria ser definido direito para onde ir os links

            /*
            navBreadCrumb.Visible = !this.EPaginaDoMinhaConta;
            navMinhaConta.Visible = this.EPaginaDoMinhaConta;

            if (navMinhaConta.Visible)
            {
                string lURL = Request.Url.AbsoluteUri.ToLower();

                if (lURL.Contains("cadastropf.aspx"))
                {
                    navMinhaConta.Attributes["class"] = "SemLinks CabecalhoCadastro";

                    this.SubSecao = new SecaoDoSite("AbraConta", "");
                }
                else if(lURL.Contains("login.aspx"))
                {
                    navMinhaConta.Attributes["class"] = "SemLinks CabecalhoLogin";

                    this.SubSecao = new SecaoDoSite("Login", "");
                }
            }

            pnlCotacaoRapida.Visible = this.EPaginaDoMinhaContaMasNaoLogin;

            */
            try
            {
                hidServerData.Value = string.Format("[{0}] [{1}] [{2}] [{3}]"
                                                    , Request.ServerVariables["SERVER_NAME"]
                                                    , Request.ServerVariables["LOCAL_ADDR"]
                                                    , Request.ServerVariables["APP_POOL_ID"]
                                                    , Request.ServerVariables["INSTANCE_ID"]);
            }
            catch{}
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (this.Secao != null)
            {
                //lnkBreadCrumb_Secao.Text = this.Secao.Nome;
                //lnkBreadCrumb_Secao.NavigateUrl = string.Format("~/{0}", this.Secao.URL);

                //lnkBreadCrumb_Secao.Visible = true;
            }
            else
            {
                //lnkBreadCrumb_Secao.Visible = false;
            }

            if (this.SubSecao != null)
            {
                //lnkBreadCrumb_SubSecao.Text = this.SubSecao.Nome;
                //lnkBreadCrumb_SubSecao.NavigateUrl = string.Format("~/{0}", this.SubSecao.URL);

                //lnkBreadCrumb_SubSecao.Visible = true;
            }
            else
            {
                //lnkBreadCrumb_SubSecao.Visible = false;
            }
            */

            this.ConfigurarTela();

        }

        #endregion

    }
}