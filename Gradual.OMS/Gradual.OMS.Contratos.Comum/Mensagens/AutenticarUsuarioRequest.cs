using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de autenticação de usuário
    /// </summary>
    [Serializable]
    public class AutenticarUsuarioRequest : MensagemRequestBase
    {
        /// <summary>
        /// Codigo do usuário a autenticar
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Email do usuário a autenticar.
        /// Opcional, pode ser informado o código ou email para autenticação
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Senha a ser validada
        /// </summary>
        public string Senha { get; set; }

        /// <summary>
        /// Codigo do sistema cliente em que o usuário está realizando o login
        /// </summary>
        public string CodigoSistemaCliente { get; set; }
    }
}
