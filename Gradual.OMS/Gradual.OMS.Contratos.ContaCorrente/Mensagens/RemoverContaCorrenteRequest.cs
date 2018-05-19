using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.ContaCorrente.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de remoção de conta corrente
    /// </summary>
    public class RemoverContaCorrenteRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código da conta corrente a remover
        /// </summary>
        public string CodigoContaCorrente { get; set; }
    }
}
