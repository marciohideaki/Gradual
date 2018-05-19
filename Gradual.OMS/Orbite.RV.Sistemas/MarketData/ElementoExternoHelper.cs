using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Sistemas.MarketData
{
    /// <summary>
    /// Helper para representar um elemento externo ao sistema
    /// fazer a conversão de valores externos para internos e vice-versa
    /// </summary>
    public class ElementoExternoHelper<T>
    {
        /// <summary>
        /// Representação interna do tipo da série
        /// </summary>
        public T ElementoInterno { get; set; }

        /// <summary>
        /// Representação externa do tipo da série
        /// </summary>
        public string ElementoExterno { get; set; }
    }
}
