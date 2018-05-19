using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de tickets
    /// </summary>
    public class ListarTicketsRiscoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de tickets encontrado
        /// </summary>
        public List<TicketRiscoInfo> Resultado { get; set; }
    }
}
