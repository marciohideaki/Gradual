using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de validação de mensagem
    /// </summary>
    public class ValidarMensagemResponse : MensagemResponseBase
    {
        /// <summary>
        /// Informa o contexto utilizado na validação da mensagem.
        /// </summary>
        public ContextoValidacaoInfo ContextoValidacao { get; set; }
    }
}
