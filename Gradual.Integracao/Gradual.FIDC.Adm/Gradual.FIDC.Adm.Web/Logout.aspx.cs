using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.FIDC.Adm.Web
{
    public partial class Logout : PaginaBase
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (UsuarioLogado != null)
            {
                UsuariosLogados.Remove(UsuarioLogado.CodBovespaTipoInt);
            }

            Session.Clear();

            Response.Redirect("~/Default.aspx");

        }
    }
}