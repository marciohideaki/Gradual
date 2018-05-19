using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Spider.Www
{
    public partial class Spider : System.Web.UI.MasterPage
    {
        public string RaizDoSite
        {
            get
            {
                return ConfiguracoesValidadas.RaizDoSite;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var lBase = (PaginaBase)this.Page;

            if (lBase.UsuarioLogado != null)
            {
                this.lblNomeUsuario.Text = lBase.UsuarioLogado.Nome;
            }  
        }
    }
}