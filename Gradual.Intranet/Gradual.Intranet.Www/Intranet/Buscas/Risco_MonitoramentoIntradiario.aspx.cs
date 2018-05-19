using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Newtonsoft.Json;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.Intranet.Buscas
{
    public partial class Risco_MonitoramentoIntradiario : PaginaBase
    {
        public string gIdAsessorLogado { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.PopularControleComListaDoSinacor(eInformacao.AssessorPadronizado, this.rptBM_FiltroRelatorio_CodAssessor);
        }
    }
}