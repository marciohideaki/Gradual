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
    public class FundoFluxoGrupoEtapaInfo
    {
        public int IdFundoFluxoGrupoEtapa { get; set; }
        public int IdFundoFluxoGrupo { get; set; }
        public string DsFundoFluxoGrupoEtapa { get; set; }
    }
}
