using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Contratos.MarketData.Mensagens
{
    public class ReceberListaSeriesResponse
    {
        public List<SerieInfo> Series { get; set; }

        public ReceberListaSeriesResponse()
        {
            this.Series = new List<SerieInfo>();
        }
    }
}
