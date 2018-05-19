using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de detalhe de usuário
    /// </summary>
    [Serializable]
    public class ReceberUsuarioResponse : MensagemResponseBase
    {
        /// <summary>
        /// Usuario encontrado
        /// </summary>
        public UsuarioInfo Usuario { get; set; }
    }
}
