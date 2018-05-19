using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Dados
{
    /// <summary>
    /// Parâmetros para o serviço de market data bovespa que são atribuídas em 
    /// tempo de execução
    /// </summary>
    [Serializable]
    public class MarketDataBovespaParametrosInfo : ICodigoEntidade
    {
        /// <summary>
        /// Indica a data de referência da última requisição realizada à bolsa
        /// </summary>
        public DateTime DataReferenciaUltimaRequisicao { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public MarketDataBovespaParametrosInfo()
        {
            this.DataReferenciaUltimaRequisicao = DateTime.MinValue;
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            // Retorna um código fixo para o objeto
            return "A5AF7A73-0964-4458-BF5F-7AACED620295";
        }

        #endregion
    }
}
