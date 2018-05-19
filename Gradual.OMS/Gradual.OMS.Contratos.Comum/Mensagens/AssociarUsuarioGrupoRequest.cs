using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de associação de grupo de usuário
    /// </summary>
    public class AssociarUsuarioGrupoRequest : MensagemRequestBase 
    {
        /// <summary>
        /// Código do grupo de usuário a ser associado
        /// </summary>
        public string CodigoUsuarioGrupo { get; set; }

        /// <summary>
        /// Código do usuário a receber a associação
        /// </summary>
        public string CodigoUsuario { get; set; }
    }
}
