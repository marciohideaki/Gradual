using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de Perfils
    /// </summary>
    public class ListarPerfisRiscoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de Perfils encontrado
        /// </summary>
        public List<PerfilRiscoInfo> Resultado { get; set; }
    }
}
