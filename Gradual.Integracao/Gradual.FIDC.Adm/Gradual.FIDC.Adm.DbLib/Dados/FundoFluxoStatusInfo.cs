using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    /// <summary>
    /// Classe de info utilizada para gerencias dados referentes aos status das etapas do fluxo de aprovação
    /// </summary>
    public class FundoFluxoStatusInfo
    {
        public int IdFundoFluxoStatus { get; set; }
        public string DsFundoFluxoStatus { get; set; }
    }
}
