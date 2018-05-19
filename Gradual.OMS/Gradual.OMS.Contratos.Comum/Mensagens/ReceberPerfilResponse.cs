using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de perfil
    /// </summary>
    [Serializable]
    public class ReceberPerfilResponse : MensagemResponseBase
    {
        /// <summary>
        /// Perfil encontrado
        /// </summary>
        public PerfilInfo Perfil { get; set; }
    }
}
