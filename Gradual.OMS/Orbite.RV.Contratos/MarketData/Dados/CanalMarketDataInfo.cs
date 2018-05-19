using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Dados
{
    /// <summary>
    /// Contém informações sobre um canal de market data.
    /// </summary>
    public class CanalMarketDataInfo
    {
        /// <summary>
        /// Nome que identifica o canal.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Tipo do canal, no formato Tipo, Assembly
        /// </summary>
        public string TipoCanal { get; set; }
    }
}
