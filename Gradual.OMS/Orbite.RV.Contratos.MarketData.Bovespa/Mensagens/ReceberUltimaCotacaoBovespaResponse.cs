using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.RV.Contratos.MarketData.Bovespa.Dados;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de ultima cotação de ativos bovespa
    /// </summary>
    public class ReceberUltimaCotacaoBovespaResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista das cotações solicitadas
        /// </summary>
        public List<CotacaoBovespaInfo> Cotacoes { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberUltimaCotacaoBovespaResponse()
        {
            this.Cotacoes = new List<CotacaoBovespaInfo>();
        }
    }
}
