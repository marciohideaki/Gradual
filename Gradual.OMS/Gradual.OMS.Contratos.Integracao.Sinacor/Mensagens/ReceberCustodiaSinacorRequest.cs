using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de custodia do sinacor
    /// </summary>
    public class ReceberCustodiaSinacorRequest : MensagemRequestBase
    {
        /// <summary>
        /// Código do cliente do sinacor que deseja receber a custodia
        /// </summary>
        public string CodigoClienteCBLC { get; set; }
    }
}
