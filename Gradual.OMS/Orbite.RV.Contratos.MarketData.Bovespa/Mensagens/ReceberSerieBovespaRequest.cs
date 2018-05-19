using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.RV.Contratos.MarketData.Bovespa.Dados;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de séries bovespa
    /// </summary>
    public class ReceberSerieBovespaRequest : MensagemRequestBase
    {
        /// <summary>
        /// Tipo dos eventos a serem listados
        /// </summary>
        public List<SerieBovespaTipoEnum> TiposEventos { get; set; }

        /// <summary>
        /// Informações do instrumento no qual se deseja recuperar os eventos
        /// </summary>
        public InstrumentoBovespaInfo Instrumento { get; set; }

        /// <summary>
        /// Data inicial do período a listar
        /// </summary>
        public DateTime? DataInicial { get; set; }

        /// <summary>
        /// Data final do período a listar
        /// </summary>
        public DateTime? DataFinal { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberSerieBovespaRequest()
        {
            this.TiposEventos = new List<SerieBovespaTipoEnum>();
        }
    }
}
