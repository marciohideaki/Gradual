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
    /// Mensagem de solicitação de detalhe de instrumento
    /// </summary>
    [SerieMarketData(
        CodigoSerie = "E2F356CE-8ADF-4145-BA89-F52EC0B19569",
        NomeSerie = "Detalhe de Instrumento",
        DescricaoSerie = "Série com informações de detalhes de instrumentos",
        TipoMensagemResponse = typeof(ReceberSerieDetalheInstrumentoResponse))]
    public class ReceberSerieDetalheInstrumentoRequest : ReceberSerieItensRequest
    {
        /// <summary>
        /// Contém informações para localizar o instrumento
        /// </summary>
        public InstrumentoInfo InstrumentoInfo { get; set; }

        /// <summary>
        /// Indica a data de referencia a considerar no retorno das informações de instrumentos
        /// </summary>
        public DateTime? DataReferencia { get; set; }
    }
}
