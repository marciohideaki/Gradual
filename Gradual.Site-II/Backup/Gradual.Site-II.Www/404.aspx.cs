using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www
{
    public partial class _404 : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string lURL = Request["url"];

            if (!string.IsNullOrEmpty(lURL))
            {
                lblMensagem.Text = "Não existe página cadastrada para a URL " + lURL + ".";
            }
            else
            {
                lblMensagem.Text = "Página não encontrada.";
            }
        }
    }
}