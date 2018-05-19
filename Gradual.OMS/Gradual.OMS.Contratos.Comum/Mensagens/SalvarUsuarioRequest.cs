using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para salvar usuário
    /// </summary>
    [Serializable]
    public class SalvarUsuarioRequest : MensagemRequestBase
    {
        /// <summary>
        /// Usuário a ser salvo
        /// </summary>
        public UsuarioInfo Usuario { get; set; }
    }
}
