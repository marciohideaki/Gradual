using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de associação de perfil
    /// </summary>
    public class AssociarPerfilRequest : MensagemRequestBase
    {
        /// <summary>
        /// Perfil a ser associado
        /// </summary>
        public string CodigoPerfil { get; set; }

        /// <summary>
        /// Código do usuário a associar o perfil
        /// </summary>
        public string CodigoUsuario { get; set; }

        /// <summary>
        /// Código do grupo de usuário a associar o perfil
        /// </summary>
        public string CodigoUsuarioGrupo { get; set; }
    }
}
