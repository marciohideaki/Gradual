using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de lista de custos Bovespa
    /// </summary>
    public class ReceberCustosBovespaRequest : MensagemRequestBase
    {
        /// <summary>
        /// Indica a data inicial de referencia
        /// </summary>
        public DateTime DataInicial { get; set; }

        /// <summary>
        /// Indica a data final de referencia
        /// </summary>
        public DateTime DataFinal { get; set; }
    }
}
