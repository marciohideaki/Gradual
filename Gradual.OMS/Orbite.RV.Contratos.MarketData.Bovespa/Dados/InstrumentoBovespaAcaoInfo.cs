using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Dados
{
    /// <summary>
    /// Complemento de informações de ativos para Ações.
    /// </summary>
    [Serializable]
    public class InstrumentoBovespaAcaoInfo : InstrumentoBovespaInfo
    {
        /// <summary>
        /// Construtor default
        /// </summary>
        public InstrumentoBovespaAcaoInfo()
        {
            this.Tipo = InstrumentoBovespaTipoEnum.Acao;
        }
    }
}
