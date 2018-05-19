using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData;
using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Sistemas.MarketData
{
    /// <summary>
    /// Utilizada para armazenar o canal junto com outras informações (Instrumentos e Series)
    /// em uma coleção.
    /// </summary>
    public class CanalMarketDataHelper
    {
        public CanalMarketDataBase Canal { get; set; }
        public List<InstrumentoInfo> Instrumentos { get; set; }
        public List<SerieInfo> Series { get; set; }

        public CanalMarketDataHelper()
        {
            this.Instrumentos = new List<InstrumentoInfo>();
            this.Series = new List<SerieInfo>();
        }
    }
}
