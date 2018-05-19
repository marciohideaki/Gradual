using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de remoção de usuário
    /// </summary>
    [Serializable]
    public class RemoverUsuarioRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do usuário a remover
        /// </summary>
        public string CodigoUsuario { get; set; }
    }
}
