using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de consulta de saldo de conta margem do sinacor
    /// corrente sinacor
    /// </summary>
    public class ReceberSaldoContaMargemSinacorRequest : MensagemRequestBase
    {
        /// <summary>
        /// Codigo do cliente CBLC a ser consultado
        /// </summary>
        public string CodigoClienteCBLC { get; set; }

        /// <summary>
        /// Indica se deve retornar o histórico de utilização de conta margem
        /// </summary>
        public bool RetornarHistorico { get; set; }
    }
}
