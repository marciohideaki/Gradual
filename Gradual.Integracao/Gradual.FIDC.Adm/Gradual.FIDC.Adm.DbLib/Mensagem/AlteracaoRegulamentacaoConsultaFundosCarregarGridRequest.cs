using Gradual.OMS.Library;
using System;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    public class AlteracaoRegulamentacaoConsultaFundosCarregarGridRequest : MensagemRequestBase
    {
        public bool SelecionarPendentes { get; set; }
        public bool SelecionarConcluídos { get; set; }

        public DateTime? DtDe { get; set; }
        public DateTime? DtAte { get; set; }

        public int IdFundoCadastro { get; set; }
        public int IdFluxoAlteracaoRegulamentoGrupo { get; set; }
    }
}
