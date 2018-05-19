using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Dados
{
    /// <summary>
    /// Contém informações sobre cotações bovespa
    /// </summary>
    [Serializable]
    public struct CotacaoBovespaInfo : ICloneable
    {
        public string Ativo { get; set; }
        public DateTime Data { get; set; }

        public double Abertura { get; set; }
        public double Fechamento { get; set; }
        public double Maximo { get; set; }
        public double Minimo { get; set; }
        public double Quantidade { get; set; }
        public double Volume { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
