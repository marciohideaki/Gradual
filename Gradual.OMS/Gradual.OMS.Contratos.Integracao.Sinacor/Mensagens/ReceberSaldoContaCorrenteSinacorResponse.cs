using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.Dados;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.Mensagens
{
    /// <summary>
    /// Mensagem resposta a uma solicitação de consulta de saldo de conta
    /// corrente sinacor
    /// </summary>
    public class ReceberSaldoContaCorrenteSinacorResponse : MensagemResponseBase
    {
        /// <summary>
        /// Informações do saldo de conta corrente do sinacor
        /// </summary>
        public SaldoContaCorrenteSinacorInfo SaldoContaCorrenteSinacor { get; set; }
    }
}
