using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de alteração de senha
    /// </summary>
    public class AlterarSenhaRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do usuário a ter a senha alterada
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Senha atual para validação
        /// </summary>
        public string SenhaAtual { get; set; }

        /// <summary>
        /// Nova senha
        /// </summary>
        public string NovaSenha { get; set; }
    }
}
