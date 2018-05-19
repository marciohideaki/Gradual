using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de liberação de cliente
    /// </summary>
    public class LiberarClienteRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código de usuário do cliente que se deseja fazer a liberação
        /// </summary>
        public string CodigoUsuario { get; set; }
    }
}
