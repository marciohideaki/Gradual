using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.MarketData
{
    /// <summary>
    /// Classe de configuração do canal de MarketData Bovespa.
    /// </summary>
    public class CanalMarketDataBovespaConfig
    {
        public string Origem { get; set; }
        public string DiretorioArquivos { get; set; }
    }
}
