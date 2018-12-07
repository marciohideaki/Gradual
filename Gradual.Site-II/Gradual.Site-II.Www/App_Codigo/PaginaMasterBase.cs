using log4net;

namespace Gradual.Site.Www
{
    public class PaginaMasterBase : System.Web.UI.MasterPage
    {
        #region Atributos

        private static bool gServicosCarregados = false;

        #endregion

        #region Propriedades

        public string HostDoSite
        {
            get
            {
                return ConfiguracoesValidadas.HostDoSite;
            }
        }

        public string RaizDoSite
        {
            get
            {
                string lRetorno = ConfiguracoesValidadas.RaizDoSite;

                if (!string.IsNullOrEmpty(lRetorno))
                    lRetorno = "/" + lRetorno;

                return lRetorno;
            }
        }

        public string HostERaiz
        {
            get
            {
                return string.Format("{0}{1}", this.HostDoSite, ConfiguracoesValidadas.RaizDoSite);
            }
        }

        public string HostERaizHttps
        {
            get
            {
                string lRetorno = string.Format("{0}{1}", this.HostDoSite, this.RaizDoSite);

                if (!lRetorno.ToLower().Contains("localhost"))
                {
                    lRetorno = lRetorno.Replace("http:", "https:");
                }

                return lRetorno;
            }
        }

        public string SkinEmUso
        {
            get
            {
                if (Session["SkinEmUso"] == null)
                {
                    Session["SkinEmUso"] = ConfiguracoesValidadas.SkinPadrao;
                }

                return (string)Session["SkinEmUso"];
            }

            set
            {
                Session["SkinEmUso"] = value;
            }
        }

        public string VersaoDoSite
        {
            get
            {
                return ConfiguracoesValidadas.VersaoDoSite;
            }
        }

        public TransporteSessaoClienteLogado SessaoClienteLogado
        {
            get { return (TransporteSessaoClienteLogado)this.Session["ClienteLogado"]; }
            set { this.Session["ClienteLogado"] = value; }
        }

        public int IdDaPagina { get; set; }

        public int IdDaEstrutura { get; set; }

        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #endregion

        #region Construtores

        public PaginaMasterBase()
        {
            if (!gServicosCarregados)
            {
                Gradual.OMS.Library.Servicos.ServicoHostColecao.Default.CarregarConfig("Desenvolvimento");
                gServicosCarregados = true;
            }
        }

        #endregion
    }
}