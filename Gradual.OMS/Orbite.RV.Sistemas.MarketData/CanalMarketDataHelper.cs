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
        public List<SerieDescricaoInfo> Series { get; set; }

        public CanalMarketDataHelper()
        {
            this.Series = new List<SerieDescricaoInfo>();
        }
    }
}
