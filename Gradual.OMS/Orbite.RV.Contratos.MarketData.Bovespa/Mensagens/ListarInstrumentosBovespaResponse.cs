using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.RV.Contratos.MarketData.Bovespa.Dados;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de instrumentos
    /// </summary>
    public class ListarInstrumentosBovespaResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de instrumentos
        /// </summary>
        public List<InstrumentoBovespaInfo> Instrumentos { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ListarInstrumentosBovespaResponse()
        {
            this.Instrumentos = new List<InstrumentoBovespaInfo>();
        }
    }
}
