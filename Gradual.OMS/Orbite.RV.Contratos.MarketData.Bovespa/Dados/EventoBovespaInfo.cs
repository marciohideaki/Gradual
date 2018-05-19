using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Dados
{
    [Serializable]
    public class EventoBovespaInfo
    {
        public DateTime Data { get; set; }
        public double Valor { get; set; }
        public SerieBovespaTipoEnum TipoEvento { get; set; }
    }
}
