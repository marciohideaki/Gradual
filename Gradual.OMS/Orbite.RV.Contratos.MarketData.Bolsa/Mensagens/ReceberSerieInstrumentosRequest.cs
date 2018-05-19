using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Bolsa.Dados;
using Orbite.RV.Contratos.MarketData.Mensagens;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de lista de instrumentos 
    /// </summary>
    [SerieMarketData(
        CodigoSerie = "A856B9B7-1C99-47c7-8ABE-94FD1417F5B8",
        NomeSerie = "Lista de Instrumentos",
        DescricaoSerie = "Lista de Instrumentos do Canal",
        TipoMensagemResponse = typeof(ReceberSerieInstrumentosResponse))]
    public class ReceberSerieInstrumentosRequest : ReceberSerieItensRequest
    {
        /// <summary>
        /// Tipo da lista desejada
        /// </summary>
        public ReceberSerieInstrumentosTipoListaEnum TipoLista { get; set; }
        
        /// <summary>
        /// Data de referencia para a lista de instrumentos.
        /// </summary>
        public DateTime? DataReferencia { get; set; }

        /// <summary>
        /// Permite filtro por tipo de instrumentos
        /// </summary>
        public InstrumentoTipoEnum? InstrumentoTipo { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberSerieInstrumentosRequest()
        {
            this.TipoLista = ReceberSerieInstrumentosTipoListaEnum.Padrao;
        }
    }
}
