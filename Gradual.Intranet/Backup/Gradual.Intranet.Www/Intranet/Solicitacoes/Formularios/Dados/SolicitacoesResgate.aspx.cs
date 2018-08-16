using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Intranet.Www.Intranet.Solicitacoes.Formularios.Dados
{
    public partial class SolicitacoesResgate : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados" },
            new ResponderAcaoAjaxDelegate[] { ResponderCarregarHtmlComDados });
        }

        private string ResponderCarregarHtmlComDados()
        {
            return "";
        }
    }
}