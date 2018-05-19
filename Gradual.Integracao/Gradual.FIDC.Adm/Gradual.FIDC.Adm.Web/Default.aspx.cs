using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.FIDC.Adm.Web
{
    public partial class Default :PaginaBase
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                base.TituloDaPagina = "Página inicial";
            }
        }
    }
}