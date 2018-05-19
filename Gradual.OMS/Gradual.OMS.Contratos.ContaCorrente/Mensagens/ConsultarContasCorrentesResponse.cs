using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente.Dados;

namespace Gradual.OMS.Contratos.ContaCorrente.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de consulta de conta corrente
    /// </summary>
    public class ConsultarContasCorrentesResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de contas correntes encontradas
        /// </summary>
        public List<ContaCorrenteInfo> ContasCorrentes { get; set; }
    }
}
