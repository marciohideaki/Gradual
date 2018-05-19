using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de inicialização de cliente
    /// </summary>
    public class InicializarClienteRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código de usuário do cliente a ser inicializado
        /// </summary>
        public string CodigoUsuario { get; set; }
    }
}
