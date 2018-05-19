using Gradual.OMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe que armazena informações de request referentes aos status das etapas do fluxo de aprovacao
    /// </summary>
    public class FundoFluxoAprovacaoRequest : MensagemRequestBase
    {
        public int IdFundoFluxoAprovacao { get; set; }
        public int IdFundoCadastro { get; set; }
        public int IdFundoFluxoGrupoEtapa { get; set; }
        public int IdFundoFluxoStatus { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtConclusao { get; set; }
        public string UsuarioResponsavel { get; set; }
    }
}
