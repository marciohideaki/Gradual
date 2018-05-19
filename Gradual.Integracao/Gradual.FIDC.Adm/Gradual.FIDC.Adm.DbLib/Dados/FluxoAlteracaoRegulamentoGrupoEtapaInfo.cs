using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    /// <summary>
    /// Classe de info utilizada para gerencias dados referentes a etapas do fluxo de aprovação
    /// </summary>
    public class FluxoAlteracaoRegulamentoGrupoEtapaInfo
    {
        public int IdFluxoAlteracaoRegulamentoGrupoEtapa { get; set; }
        public int IdFluxoAlteracaoRegulamentoGrupo { get; set; }
        public string DsFluxoAlteracaoRegulamentoGrupoEtapa { get; set; }
    }
}
