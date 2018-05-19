using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de regras
    /// </summary>
    public class ListarRegraRiscoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de regras de risco encontradas
        /// </summary>
        public List<RegraRiscoInfo> Resultado { get; set; }
    }
}
