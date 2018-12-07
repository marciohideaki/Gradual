using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.MinhaConta
{
    public partial class ucMinhaContaBase : System.Web.UI.UserControl
    {
        PaginaMasterBase lBase = new PaginaMasterBase();

        protected void Page_Load(object sender, EventArgs e) { }

        public System.String RaizDoSite
        {
            get
            {
                string lRetorno = ConfiguracoesValidadas.RaizDoSite;

                if (!string.IsNullOrEmpty(lRetorno))
                    lRetorno = "/" + lRetorno;

                return lRetorno;
            }
        }

        public System.String SkinEmUso
        {
            get
            {
                if (HttpContext.Current.Session["SkinEmUso"] == null)
                {
                    HttpContext.Current.Session["SkinEmUso"] = ConfiguracoesValidadas.SkinPadrao;
                }

                return (string)HttpContext.Current.Session["SkinEmUso"];
            }
        }
        
    }
}