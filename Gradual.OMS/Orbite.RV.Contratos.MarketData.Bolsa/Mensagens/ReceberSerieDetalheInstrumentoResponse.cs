using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.RV.Contratos.MarketData.Bolsa.Dados;
using Orbite.RV.Contratos.MarketData.Mensagens;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de detalhe de ativo
    /// </summary>
    public class ReceberSerieDetalheInstrumentoResponse : ReceberSerieItensResponse
    {
        /// <summary>
        /// Lista de instrumentos encontrados que satisfazem as condições informadas
        /// </summary>
        public List<InstrumentoInfo> Instrumentos { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberSerieDetalheInstrumentoResponse()
        {
            this.Instrumentos = new List<InstrumentoInfo>();
        }
    }
}
