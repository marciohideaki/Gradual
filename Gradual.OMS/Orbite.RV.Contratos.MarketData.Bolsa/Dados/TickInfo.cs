using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Dados
{
    [Serializable]
    public struct TickInfo 
    {
        public double Abertura { get; set; }
        public DateTime Data { get; set; }
        public double Fechamento { get; set; }
        public double Maximo { get; set; }
        public double Minimo { get; set; }
        public double Quantidade { get; set; }
        public double Volume { get; set; }
    }
}
