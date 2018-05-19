using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Custodia.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de remover custódia
    /// </summary>
    public class RemoverCustodiaRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código da custódia a remover
        /// </summary>
        public string CodigoCustodia { get; set; }
    }
}
