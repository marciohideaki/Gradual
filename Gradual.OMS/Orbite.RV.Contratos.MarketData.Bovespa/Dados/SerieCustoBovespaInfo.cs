using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Dados
{
    /// <summary>
    /// Representa uma série de custos de bolsa
    /// </summary>
    [Serializable]
    public class SerieCustoBolsaInfo : ICodigoEntidade
    {
        /// <summary>
        /// Código da série
        /// </summary>
        public string CodigoSerieCustoBovespa { get; set; }

        /// <summary>
        /// Contem os itens da série
        /// </summary>
        public List<CustoBovespaInfo> Itens { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public SerieCustoBolsaInfo()
        {
            this.CodigoSerieCustoBovespa = Guid.NewGuid().ToString();
            this.Itens = new List<CustoBovespaInfo>();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoSerieCustoBovespa;
        }

        #endregion
    }
}
