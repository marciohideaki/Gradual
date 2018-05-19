using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.ContaCorrente.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de detalhe de conta corrente
    /// </summary>
    public class ReceberContaCorrenteRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código da conta corrente a recuperar
        /// </summary>
        public string CodigoContaCorrente { get; set; }
    }
}
