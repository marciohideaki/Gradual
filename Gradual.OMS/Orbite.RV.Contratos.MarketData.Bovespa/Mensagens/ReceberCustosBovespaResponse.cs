using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.RV.Contratos.MarketData.Bovespa.Dados;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de custos Bovespa
    /// </summary>
    public class ReceberCustosBovespaResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de custos de bolsa solicitados
        /// </summary>
        public List<CustoBovespaInfo> CustosBolsa { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberCustosBovespaResponse()
        {
            this.CustosBolsa = new List<CustoBovespaInfo>();
        }
    }
}
