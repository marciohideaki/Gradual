using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.Intranet.Buscas
{
    public partial class Monitoramento_Termos : PaginaBaseAutenticada
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            this.PopularControleComListaDoSinacor(eInformacao.Assessor, rptAssessor);
        }
    }
}