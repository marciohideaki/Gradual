using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.IntegracaoRocket
{
    public partial class IntegracaoRocket : System.Web.UI.MasterPage
    {
        public string VersaoDoSite
        {
            get
            {
                return ConfiguracoesValidadas.VersaoDoSite;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}