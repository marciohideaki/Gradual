using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Dados
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
        public string CodigoSerieCustoBolsa { get; set; }

        /// <summary>
        /// Contem os itens da série
        /// </summary>
        public List<CustoBolsaInfo> Itens { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public SerieCustoBolsaInfo()
        {
            this.CodigoSerieCustoBolsa = Guid.NewGuid().ToString();
            this.Itens = new List<CustoBolsaInfo>();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoSerieCustoBolsa;
        }

        #endregion
    }
}
