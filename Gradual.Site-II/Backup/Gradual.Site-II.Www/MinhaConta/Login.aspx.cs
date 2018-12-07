using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.Www;

namespace Gradual.Site.Www.MinhaConta
{
    public partial class Login : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["redir"]))
            {
                Session["RedirecionamentoPorFaltaDeLogin"] = Request["redir"];
            }
        }
    }
}