using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Resposta a uma solicitação de recebimento de mensagem
    /// </summary>
    public class ReceberMensagemResponse : MensagemResponseBase
    {
        /// <summary>
        /// Mensagem recuperada
        /// </summary>
        public MensagemBase Mensagem { get; set; }
    }
}
