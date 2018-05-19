using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.BMF.Dados;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Orbite.RV.Contratos.MarketData.BMF.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de última cotação BMF
    /// </summary>
    public class ReceberUltimaCotacaoBMFResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista das cotações solicitadas
        /// </summary>
        public List<CotacaoBMFInfo> Cotacoes { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberUltimaCotacaoBMFResponse()
        {
            this.Cotacoes = new List<CotacaoBMFInfo>();
        }
    }
}
