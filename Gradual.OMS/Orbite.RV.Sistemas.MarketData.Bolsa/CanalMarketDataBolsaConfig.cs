using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.MarketData.Bolsa
{
    /// <summary>
    /// Classe de configuração do canal de MarketData Bovespa.
    /// </summary>
    public class CanalMarketDataBolsaConfig
    {
        public string Origem { get; set; }
        public string DiretorioArquivosBovespa { get; set; }
    }
}
