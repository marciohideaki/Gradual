using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class PainelDeBusca : System.Web.UI.UserControl
    {
        #region Propriedades
        
        public string HostERaiz
        {
            get
            {
                return string.Format("{0}{1}", ConfiguracoesValidadas.HostDoSite, ConfiguracoesValidadas.RaizDoSite);
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}