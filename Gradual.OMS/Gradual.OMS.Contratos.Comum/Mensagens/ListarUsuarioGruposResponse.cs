using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de consulta de usuários grupo
    /// </summary>
    [Serializable]
    public class ListarUsuarioGruposResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de usuários grupo encontrada
        /// </summary>
        public List<UsuarioGrupoInfo> UsuarioGrupos { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ListarUsuarioGruposResponse()
        {
            this.UsuarioGrupos = new List<UsuarioGrupoInfo>();
        }
    }
}
