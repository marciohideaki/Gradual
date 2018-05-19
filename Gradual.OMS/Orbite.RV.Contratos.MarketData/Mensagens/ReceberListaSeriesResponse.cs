using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Contratos.MarketData.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de séries
    /// </summary>
    public class ReceberListaSeriesResponse
    {
        /// <summary>
        /// Lista de séries
        /// </summary>
        public List<SerieDescricaoInfo> SeriesDescricao { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberListaSeriesResponse()
        {
            this.SeriesDescricao = new List<SerieDescricaoInfo>();
        }
    }
}
