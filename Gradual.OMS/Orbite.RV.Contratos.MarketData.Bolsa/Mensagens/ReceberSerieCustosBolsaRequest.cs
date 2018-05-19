using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.Comum;
using Orbite.RV.Contratos.MarketData.Mensagens;
using Orbite.RV.Contratos.MarketData.Bolsa.Dados;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Mensagens
{
    /// <summary>
    /// Mensagem de request da série de custos de bolsa
    /// </summary>
    [SerieMarketData(
        CodigoSerie = "8687D55F-E708-403b-A733-0A4068703F51",
        NomeSerie = "Custos de Bolsa",
        DescricaoSerie = "Série com informações de custos de bolsa (emolumentos, etc)",
        TipoMensagemResponse = typeof(ReceberSerieCotacaoResponse))]
    public class ReceberSerieCustosBolsaRequest : ReceberSerieItensRequest
    {
        /// <summary>
        /// Indica a data inicial de referencia
        /// </summary>
        public DateTime DataInicial { get; set; }

        /// <summary>
        /// Indica a data final de referencia
        /// </summary>
        public DateTime DataFinal { get; set; }

        /// <summary>
        /// Indica a bolsa que se deseja receber as informações
        /// </summary>
        public string Bolsa { get; set; }
    }
}
