using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Risco.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de detalhe de uma regra de risco
    /// </summary>
    public class ReceberRegraRiscoRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código da regra de risco a retornar
        /// </summary>
        public string CodigoRegraRisco { get; set; }
    }
}
