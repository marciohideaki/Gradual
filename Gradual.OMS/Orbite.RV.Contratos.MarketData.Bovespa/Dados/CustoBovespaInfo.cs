using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Dados
{
    /// <summary>
    /// Contem informações sobre os custos de bolsa
    /// </summary>
    [Serializable]
    public class CustoBovespaInfo 
    {
        /// <summary>
        /// Data de referencia
        /// </summary>
        public DateTime DataReferencia { get; set; }

        /// <summary>
        /// Bolsa a que se referem os custos
        /// </summary>
        public string Bolsa { get; set; }

        /// <summary>
        /// Custo dos emolumentos para ações
        /// </summary>
        public double CustoEmolumentosAcao { get; set; }

        /// <summary>
        /// Custo dos emolumentos para opções
        /// </summary>
        public double CustoEmoulmentosOpcao { get; set; }

        /// <summary>
        /// Custo de emolumentos para daytrade com ações
        /// </summary>
        public double CustoEmolumentosDayTradeAcao { get; set; }

        /// <summary>
        /// Custo de emolumentos para daytrade com opções
        /// </summary>
        public double CustoEmolumentosDayTradeOpcao { get; set; }
    }
}
