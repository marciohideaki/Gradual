using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Sistemas.MarketData
{
    /// <summary>
    /// Classe de configuração para o serviço de MarketData.
    /// </summary>
    public class ServicoMarketDataConfig
    {
        /// <summary>
        /// Lista de canais de market data a serem carregados
        /// </summary>
        public List<CanalMarketDataInfo> Canais { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public ServicoMarketDataConfig()
        {
            this.Canais = new List<CanalMarketDataInfo>();
        }
    }
}
