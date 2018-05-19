using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de lista de perfis
    /// </summary>
    [Serializable]
    public class ListarPerfisRequest : MensagemRequestBase
    {
        /// <summary>
        /// Permite filtrar pelo código do perfil
        /// </summary>
        public string FiltroCodigoPerfil { get; set; }

        /// <summary>
        /// Permite filtrar pelo nome do perfil
        /// </summary>
        public string FiltroNomePerfil { get; set; }
    }
}
