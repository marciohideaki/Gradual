using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.RV.Contratos.MarketData.Bovespa.Dados;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de detalhe de instrumentos Bovespa
    /// </summary>
    public class ReceberDetalheInstrumentoBovespaRequest : MensagemRequestBase
    {
        /// <summary>
        /// Contém informações para localizar o instrumento
        /// </summary>
        public InstrumentoBovespaInfo Instrumento { get; set; }

        /// <summary>
        /// Indica a data de referencia a considerar no retorno das informações de instrumentos
        /// </summary>
        public DateTime? DataReferencia { get; set; }
    }
}
