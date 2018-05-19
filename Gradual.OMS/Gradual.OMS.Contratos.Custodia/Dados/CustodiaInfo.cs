using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Custodia.Dados
{
    /// <summary>
    /// Representa uma custódia, ou seja, um grupo de posições de custódia.
    /// </summary>
    [Serializable]
    public class CustodiaInfo : ICodigoEntidade
    {
        /// <summary>
        /// Chave primária da custódia
        /// </summary>
        public string CodigoCustodia { get; set; }

        /// <summary>
        /// Data da abertura das posições de custódia
        /// </summary>
        public DateTime DataAbertura { get; set; }

        /// <summary>
        /// Data em que foi feita a última integração com o sistema de custódia
        /// </summary>
        public DateTime DataIntegracao { get; set; }

        /// <summary>
        /// Data da última movimentação na custódia
        /// </summary>
        public DateTime DataUltimaMovimentacao { get; set; }

        /// <summary>
        /// Lista de posições de custódia
        /// </summary>
        public List<CustodiaPosicaoInfo> Posicoes { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public CustodiaInfo()
        {
            this.CodigoCustodia = Guid.NewGuid().ToString();
            this.Posicoes = new List<CustodiaPosicaoInfo>();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoCustodia;
        }

        #endregion
    }
}
