using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.Intranet.Buscas
{
    public partial class Compliance_EstatisticaDayTrade : PaginaBaseAutenticada
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.PopularControleComListaDoSinacor(eInformacao.AssessorPadronizado, this.rptBM_FiltroRelatorio_CodAssessor);
        }
    }
}