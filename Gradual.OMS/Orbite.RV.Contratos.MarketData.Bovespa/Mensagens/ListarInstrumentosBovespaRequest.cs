using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.RV.Contratos.MarketData.Bovespa.Dados;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de lista de instrumentos Bovespa
    /// </summary>
    public class ListarInstrumentosBovespaRequest : MensagemRequestBase
    {
        /// <summary>
        /// Tipo da lista desejada
        /// </summary>
        public ListarInstrumentosBovespaTipoListaEnum TipoLista { get; set; }
        
        /// <summary>
        /// Data de referencia para a lista de instrumentos.
        /// </summary>
        public DateTime? DataReferencia { get; set; }

        /// <summary>
        /// Permite filtro por tipo de instrumentos
        /// </summary>
        public InstrumentoBovespaTipoEnum? InstrumentoTipo { get; set; }

        /// <summary>
        /// Permite filtro por características do instrumento
        /// </summary>
        public InstrumentoBovespaInfo Instrumento { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ListarInstrumentosBovespaRequest()
        {
            this.TipoLista = ListarInstrumentosBovespaTipoListaEnum.Padrao;
        }
    }
}
