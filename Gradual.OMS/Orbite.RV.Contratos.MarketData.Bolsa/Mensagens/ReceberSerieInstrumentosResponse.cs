using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Bolsa.Dados;
using Orbite.RV.Contratos.MarketData.Mensagens;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de instrumentos
    /// </summary>
    public class ReceberSerieInstrumentosResponse : ReceberSerieItensResponse
    {
        /// <summary>
        /// Lista de instrumentos
        /// </summary>
        public List<InstrumentoInfo> Instrumentos { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberSerieInstrumentosResponse()
        {
            this.Instrumentos = new List<InstrumentoInfo>();
        }
    }
}
