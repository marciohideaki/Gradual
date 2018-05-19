using Gradual.OMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    public class ConsultaFundosConstituicaoRequest : MensagemRequestBase
    {
        public bool SelecionarPendentes { get; set; }
        public bool SelecionarConcluídos { get; set; }

        public DateTime? DtDe { get; set; }
        public DateTime? DtAte { get; set; }

        public int IdFundoCadastro { get; set; }
        public int IdFundoFluxoGrupo { get; set; }
    }
}
