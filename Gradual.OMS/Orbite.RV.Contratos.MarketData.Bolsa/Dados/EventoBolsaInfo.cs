using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Dados
{
    [Serializable]
    public class EventoBolsaInfo
    {
        public DateTime Data { get; set; }
        public double Valor { get; set; }
        public SerieBolsaTipoEnum TipoEvento { get; set; }
    }
}
