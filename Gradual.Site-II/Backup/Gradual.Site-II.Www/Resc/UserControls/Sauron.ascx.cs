using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class Sauron : UserControlBase
    {
        private string _NomeDaPagina = null;

        public string NomeDaPagina
        {
            get
            {
                if (string.IsNullOrEmpty(_NomeDaPagina) && this.PaginaBase != null)
                {
                    _NomeDaPagina = this.PaginaBase.NomeDaPagina;
                }

                return _NomeDaPagina;
            }

            set
            {
                _NomeDaPagina = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ConfiguracoesValidadas.AnalyticsHabilitado)
                this.Visible = false;
        }
    }
}