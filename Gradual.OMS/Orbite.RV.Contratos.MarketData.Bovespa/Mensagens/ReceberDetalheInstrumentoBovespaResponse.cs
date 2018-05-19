using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.RV.Contratos.MarketData.Bovespa.Dados;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de detalhe de instrumentos Bovespa
    /// </summary>
    public class ReceberDetalheInstrumentoBovespaResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de instrumentos encontrados que satisfazem as condições informadas
        /// </summary>
        public List<InstrumentoBovespaInfo> Instrumentos { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberDetalheInstrumentoBovespaResponse()
        {
            this.Instrumentos = new List<InstrumentoBovespaInfo>();
        }
    }
}
