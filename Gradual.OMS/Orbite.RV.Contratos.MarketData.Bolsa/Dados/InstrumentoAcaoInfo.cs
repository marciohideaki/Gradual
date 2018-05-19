using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Dados
{
    /// <summary>
    /// Complemento de informações de ativos para Ações.
    /// </summary>
    [Serializable]
    public class InstrumentoAcaoInfo : InstrumentoInfo
    {
        /// <summary>
        /// Construtor default
        /// </summary>
        public InstrumentoAcaoInfo()
        {
            this.Tipo = InstrumentoTipoEnum.Acao;
        }
    }
}
