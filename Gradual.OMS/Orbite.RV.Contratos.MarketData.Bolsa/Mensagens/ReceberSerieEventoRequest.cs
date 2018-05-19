using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Bolsa.Dados;
using Orbite.RV.Contratos.MarketData.Mensagens;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de série de eventos 
    /// </summary>
    [SerieMarketData(
        CodigoSerie = "1D7A30DB-1249-45da-AFE8-67DF7A4E74D0",
        NomeSerie = "Eventos",
        DescricaoSerie = "Série de Eventos de Ativos",
        TipoMensagemResponse = typeof(ReceberSerieEventoResponse))]
    public class ReceberSerieEventoRequest : ReceberSerieItensRequest
    {
        /// <summary>
        /// Tipo dos eventos a serem listados
        /// </summary>
        public List<SerieBolsaTipoEnum> TiposEventos { get; set; }

        /// <summary>
        /// Informações do instrumento no qual se deseja recuperar os eventos
        /// </summary>
        public InstrumentoInfo Instrumento { get; set; }

        /// <summary>
        /// Data inicial do período a listar
        /// </summary>
        public DateTime DataInicial { get; set; }

        /// <summary>
        /// Data final do período a listar
        /// </summary>
        public DateTime DataFinal { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberSerieEventoRequest()
        {
            this.TiposEventos = new List<SerieBolsaTipoEnum>();
        }
    }
}
