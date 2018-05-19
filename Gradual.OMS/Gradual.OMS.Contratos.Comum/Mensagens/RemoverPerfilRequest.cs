using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de requisição de remoção de perfil
    /// </summary>
    [Serializable]
    public class RemoverPerfilRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do perfil a ser removido
        /// </summary>
        public string CodigoPerfil { get; set; }
    }
}
