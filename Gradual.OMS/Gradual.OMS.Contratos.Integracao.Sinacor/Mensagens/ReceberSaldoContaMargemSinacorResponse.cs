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
    /// margem sinacor
    /// </summary>
    public class ReceberSaldoContaMargemSinacorResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de saldos de conta margem do sinacor
        /// </summary>
        public List<SaldoContaMargemSinacorInfo> SaldosContaMargemSinacor { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberSaldoContaMargemSinacorResponse()
        {
            this.SaldosContaMargemSinacor = new List<SaldoContaMargemSinacorInfo>();
        }
    }
}
