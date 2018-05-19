using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para salvar um ticket de risco
    /// </summary>
    public class SalvarTicketRiscoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Ticket de risco a ser salvo
        /// </summary>
        public TicketRiscoInfo TicketRiscoInfo { get; set; }
    }
}
