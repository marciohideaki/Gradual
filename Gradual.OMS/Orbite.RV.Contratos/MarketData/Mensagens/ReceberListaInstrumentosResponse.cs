using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Contratos.MarketData.Mensagens
{
    public class ReceberListaInstrumentosResponse
    {
        public List<InstrumentoInfo> Instrumentos { get; set; }

        public ReceberListaInstrumentosResponse()
        {
            this.Instrumentos = new List<InstrumentoInfo>();
        }
    }
}
