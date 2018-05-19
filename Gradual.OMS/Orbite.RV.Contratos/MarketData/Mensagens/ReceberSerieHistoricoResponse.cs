using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Contratos.MarketData.Mensagens
{
    /// <summary>
    /// Mensagem de resposta de requisição de histórico de uma série
    /// </summary>
    public class ReceberSerieHistoricoResponse
    {
        /// <summary>
        /// Contem a lista de elementos retornados
        /// </summary>
        public object SerieElementos { get; set; }
    }
}
