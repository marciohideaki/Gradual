using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de detalhe de usuario grupo
    /// </summary>
    [Serializable]
    public class ReceberUsuarioGrupoResponse : MensagemResponseBase
    {
        /// <summary>
        /// UsuarioGrupo solicitado
        /// </summary>
        public UsuarioGrupoInfo UsuarioGrupo { get; set; }
    }
}
