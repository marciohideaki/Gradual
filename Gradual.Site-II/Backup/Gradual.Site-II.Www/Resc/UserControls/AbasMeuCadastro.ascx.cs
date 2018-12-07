using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class AbasMeuCadastro : UserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PaginaBase.SessaoClienteLogado != null)
            {
                if (PaginaBase.SessaoClienteLogado.JaPreencheuSuitability)
                {
                    liMeuPerfil.Attributes["class"] = null;
                }
            }

            var lURL = Request.Url.ToString().ToLower();

            if (lURL.Contains("meucadastro.aspx"))
            {
                liMeuCadastro.Attributes["class"] = "ativo";
            }
            else if (lURL.Contains("meuperfil.aspx"))
            {
                liMeuPerfil.Attributes["class"] = "ativo";
            }
            else
            {
                liSeguranca.Attributes["class"] = "ativo";
            }

        }
    }
}