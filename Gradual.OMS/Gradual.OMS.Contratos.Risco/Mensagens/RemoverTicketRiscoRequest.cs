using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de remoção de um ticket de risco
    /// </summary>
    public class RemoverTicketRiscoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do ticket de risco a remover
        /// </summary>
        public string CodigoTicketRisco { get; set; }
    }
}
