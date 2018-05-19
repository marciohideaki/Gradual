using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Bolsa.Dados;
using Orbite.RV.Contratos.MarketData.Mensagens;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de custos de bolsa
    /// </summary>
    public class ReceberSerieCustosBolsaResponse : ReceberSerieItensResponse
    {
        /// <summary>
        /// Lista de custos de bolsa solicitados
        /// </summary>
        public List<CustoBolsaInfo> CustosBolsa { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberSerieCustosBolsaResponse()
        {
            this.CustosBolsa = new List<CustoBolsaInfo>();
        }
    }
}
