using Gradual.OMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe que armazena informações de request referentes aos status das etapas do fluxo de alteração de regulamento
    /// </summary>
    public class FundoFluxoAlteracaoRegulamentoRequest : MensagemRequestBase
    {
        public int IdFundoFluxoAlteracaoRegulamento { get; set; }
        public int IdFundoCadastro { get; set; }
        public int IdFluxoAlteracaoRegulamentoGrupoEtapa { get; set; }
        public int IdFluxoAlteracaoRegulamentoStatus { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtConclusao { get; set; }
        public string UsuarioResponsavel { get; set; }
    }
}
