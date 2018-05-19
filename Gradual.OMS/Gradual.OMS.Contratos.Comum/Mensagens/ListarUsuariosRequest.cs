using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de consulta de usuários
    /// </summary>
    [Serializable]
    public class ListarUsuariosRequest : MensagemRequestBase
    {
        /// <summary>
        /// Permite filtrar pelo nome ou email
        /// </summary>
        public string FiltroNomeOuEmail { get; set; }

        /// <summary>
        /// Filtro por status do usuário
        /// </summary>
        public UsuarioStatusEnum? FiltroStatus { get; set; }

        /// <summary>
        /// Filtro por código do usuário
        /// </summary>
        public string FiltroCodigoUsuario { get; set; }

        /// <summary>
        /// Filtro por grupo de usuários
        /// </summary>
        public string FiltroCodigoUsuarioGrupo { get; set; }

        /// <summary>
        /// Filtro por perfil de usuário
        /// </summary>
        public string FiltroCodigoPerfil { get; set; }
    }
}
