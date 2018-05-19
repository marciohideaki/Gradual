using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.Comum;
using Orbite.RV.Contratos.MarketData.Bolsa.Dados;
using Orbite.RV.Contratos.MarketData.Mensagens;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Mensagens
{
    /// <summary>
    /// Retorna uma lista de ticks
    /// </summary>
    public class ReceberSerieCotacaoResponse : ReceberSerieItensResponse
    {
        /// <summary>
        /// Indica o canal preencheu esta coleção de elementos.
        /// </summary>
        public string Canal { get; set; }

        /// <summary>
        /// Indica a data final dos elementos carregados.
        /// </summary>
        public DateTime DataFinal { get; set; }

        /// <summary>
        /// Indica a data inicial dos elementos carregados.
        /// </summary>
        public DateTime DataInicial { get; set; }

        /// <summary>
        /// Resultado encontrado
        /// </summary>
        public List<TickInfo> Resultado { get; set; }

        /// <summary>
        /// Indica o período que os elementos estão sumarizados
        /// </summary>
        public PeriodoEnum Periodo { get; set; }

        /// <summary>
        /// Caso o período seja personalizado, indica o intervalo período.
        /// </summary>
        public TimeSpan PeriodoPersonalizado { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberSerieCotacaoResponse()
        {
            this.Resultado = new List<TickInfo>();
        }
    }
}
