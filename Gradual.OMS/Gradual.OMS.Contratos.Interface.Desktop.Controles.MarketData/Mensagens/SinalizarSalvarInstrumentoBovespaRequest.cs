using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop.Mensagens;
using Orbite.RV.Contratos.MarketData.Bovespa.Dados;

namespace Gradual.OMS.Contratos.Interface.Desktop.Controles.MarketData.Mensagens
{
    /// <summary>
    /// Mensagem para informar salvar de instrumento bovespa
    /// </summary>
    public class SinalizarSalvarInstrumentoBovespaRequest : MensagemInterfaceRequestBase
    {
        /// <summary>
        /// Custodia sendo inicializada
        /// </summary>
        public InstrumentoBovespaInfo InstrumentoBovespa { get; set; }
    }
}
