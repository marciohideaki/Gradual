using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de detalhe de um ticket de risco
    /// </summary>
    public class ReceberTicketRiscoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do ticket de risco desejado
        /// </summary>
        public string CodigoTicketRisco { get; set; }
    }
}
