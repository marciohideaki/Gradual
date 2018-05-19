using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para receber uma sessão
    /// </summary>
    [Serializable]
    public class ReceberSessaoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Codigo da sessão desejada
        /// </summary>
        public string CodigoSessaoARetornar { get; set; }

        /// <summary>
        /// Indica se deve incluir informações do usuário na resposta
        /// </summary>
        public bool RetornarUsuario { get; set; }
    }
}
