using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de detalhe de ticket de risco
    /// </summary>
    public class ReceberTicketRiscoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Ticket de risco encontrado
        /// </summary>
        public TicketRiscoInfo TicketRiscoInfo { get; set; }
    }
}
