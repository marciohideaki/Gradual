using Gradual.OMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    public class FundoFluxoAlteracaoRegulamentoAnexoRequest : MensagemRequestBase
    {
        public int IdFundoFluxoAlteracaoRegulamento { get; set; }
        public string CaminhoAnexo { get; set; }
    }
}
