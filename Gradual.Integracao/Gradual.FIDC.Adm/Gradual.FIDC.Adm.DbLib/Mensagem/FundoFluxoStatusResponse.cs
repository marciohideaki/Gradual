using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gradual.OMS.Library;
using Gradual.FIDC.Adm.DbLib.Dados;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de response das etapas do fluxo de aprovação
    /// </summary>
    public class FundoFluxoStatusResponse : MensagemResponseBase
    {
        public List<FundoFluxoStatusInfo> ListaStatus { get; set; }
    }
}
