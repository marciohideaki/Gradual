using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma consulta de perfis
    /// </summary>
    [Serializable]
    public class ListarPerfisResponse : MensagemResponseBase
    {
        /// <summary>
        /// Perfis encontrados
        /// </summary>
        public List<PerfilInfo> Perfis { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ListarPerfisResponse()
        {
            this.Perfis = new List<PerfilInfo>();
        }
    }
}
