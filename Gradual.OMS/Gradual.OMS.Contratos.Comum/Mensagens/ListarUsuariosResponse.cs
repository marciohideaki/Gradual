using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de consulta de usuários
    /// </summary>
    [Serializable]
    public class ListarUsuariosResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de usuarios encontrados
        /// </summary>
        public List<UsuarioInfo> Usuarios { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ListarUsuariosResponse()
        {
            this.Usuarios = new List<UsuarioInfo>();
        }
    }
}
