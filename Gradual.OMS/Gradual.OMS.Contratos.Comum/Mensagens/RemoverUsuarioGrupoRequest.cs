using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de remoção de usuário grupo
    /// </summary>
    [Serializable]
    public class RemoverUsuarioGrupoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do usuário grupo a remover
        /// </summary>
        public string CodigoUsuarioGrupo { get; set; }
    }
}
