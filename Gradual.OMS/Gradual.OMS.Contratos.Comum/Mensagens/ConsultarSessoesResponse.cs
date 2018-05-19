using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma consulta de sessoes
    /// </summary>
    [Serializable]
    public class ConsultarSessoesResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de sessoes encontradas
        /// </summary>
        public List<SessaoInfo> Sessoes { get; set; }
    }
}
