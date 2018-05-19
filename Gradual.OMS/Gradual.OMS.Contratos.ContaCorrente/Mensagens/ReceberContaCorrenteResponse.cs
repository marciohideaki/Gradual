using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente.Dados;

namespace Gradual.OMS.Contratos.ContaCorrente.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de detalhe de conta corrente
    /// </summary>
    public class ReceberContaCorrenteResponse : MensagemResponseBase
    {
        /// <summary>
        /// Conta corrente encontrada
        /// </summary>
        public ContaCorrenteInfo ContaCorrenteInfo { get; set; }
    }
}
