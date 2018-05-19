using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Bolsa.Dados;
using Orbite.RV.Contratos.MarketData.Mensagens;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de série de eventos 
    /// </summary>
    public class ReceberSerieEventoResponse : ReceberSerieItensResponse
    {
        /// <summary>
        /// Lista de eventos encontrados
        /// </summary>
        public List<EventoBolsaInfo> Resultado { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberSerieEventoResponse()
        {
            this.Resultado = new List<EventoBolsaInfo>();
        }
    }
}
