using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.RV.Contratos.MarketData.Bovespa.Dados;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de eventos bovespa
    /// </summary>
    public class ReceberSerieBovespaResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de eventos encontrados
        /// </summary>
        public List<EventoBovespaInfo> Resultado { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberSerieBovespaResponse()
        {
            this.Resultado = new List<EventoBovespaInfo>();
        }
    }
}
