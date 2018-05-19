using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    /// <summary>
    /// Classe de info utilizada para gerencias dados referentes aos anexos do processo do fluxo de aprovação de um fundo
    /// </summary>
    public class FundoFluxoAprovacaoAnexoInfo
    {
        public int IdFundoFluxoAprovacao { get; set; }
        public string CaminhoAnexo { get; set; }
    }
}
