using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação para salvar perfil
    /// </summary>
    [Serializable]
    public class SalvarPerfilRequest : MensagemRequestBase
    {
        /// <summary>
        /// Perfil a ser salvo
        /// </summary>
        public PerfilInfo Perfil { get; set; }
    }
}
