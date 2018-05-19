using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    /// <summary>
    /// Classe de info utilizada para gerencias dados referentes ao processo do fluxo de aprovação de um fundo
    /// </summary>
    public class FundoFluxoAprovacaoInfo
    {
        public int IdFundoFluxoAprovacao { get; set; }
        public int IdFundoCadastro { get; set; }
        public int IdFundoFluxoGrupoEtapa { get; set; }
        public int IdFundoFluxoStatus { get; set; }
        public string DtInicio { get; set; }
        public string DtConclusao { get; set; }
        public string UsuarioResponsavel { get; set; }
    }
}
