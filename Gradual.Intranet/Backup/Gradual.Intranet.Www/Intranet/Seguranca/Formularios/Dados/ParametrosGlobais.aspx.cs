using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados
{
    public partial class ParametrosGlobais : PaginaBaseAutenticada
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados",
                                                  },
                new ResponderAcaoAjaxDelegate[] { ResponderCarregarHtmlComDados,
                                                  });
        }

        public string ResponderCarregarHtmlComDados()
        {
            return string.Empty;
        }
    }
}