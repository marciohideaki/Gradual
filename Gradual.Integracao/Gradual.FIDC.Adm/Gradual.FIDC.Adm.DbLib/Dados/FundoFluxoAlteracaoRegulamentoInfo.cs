using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Dados
{
    public class FundoFluxoAlteracaoRegulamentoInfo
    {
        public int IdFundoFluxoAlteracaoRegulamento { get; set; }
        public int IdFundoCadastro { get; set; }
        public int IdFluxoAlteracaoRegulamentoGrupoEtapa { get; set; }
        public int IdFluxoAlteracaoRegulamentoStatus { get; set; }
        public string DtInicio { get; set; }
        public string DtConclusao { get; set; }
        public string UsuarioResponsavel { get; set; }
    }
}
