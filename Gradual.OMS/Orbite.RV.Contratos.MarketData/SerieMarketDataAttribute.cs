using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData
{
    /// <summary>
    /// Atributo para indicar uma série
    /// </summary>
    public class SerieMarketDataAttribute : Attribute
    {
        /// <summary>
        /// Código da série
        /// </summary>
        public string CodigoSerie { get; set; }
        
        /// <summary>
        /// Nome da série
        /// </summary>
        public string NomeSerie { get; set; }

        /// <summary>
        /// Descrição da série
        /// </summary>
        public string DescricaoSerie { get; set; }

        /// <summary>
        /// Tipo da mensagem de request.
        /// Opcional, pois se não estiver presente, assume-se o tipo que carrega
        /// o atributo
        /// </summary>
        public Type TipoMensagemRequest { get; set; }

        /// <summary>
        /// Tipo da mensagem de response
        /// </summary>
        public Type TipoMensagemResponse { get; set; }
    }
}
