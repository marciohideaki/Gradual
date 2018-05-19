using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.Comum;
using Orbite.RV.Contratos.MarketData.BMF.Dados;

namespace Orbite.RV.Contratos.MarketData.BMF.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de histórico de cotação BMF
    /// </summary>
    public class ReceberHistoricoCotacaoBMFResponse : MensagemResponseBase
    {
        /// <summary>
        /// Indica a data final dos elementos carregados.
        /// </summary>
        public DateTime DataFinal { get; set; }

        /// <summary>
        /// Indica a data inicial dos elementos carregados.
        /// </summary>
        public DateTime DataInicial { get; set; }

        /// <summary>
        /// Resultado encontrado
        /// </summary>
        public List<CotacaoBMFInfo> Resultado { get; set; }

        /// <summary>
        /// Indica o período que os elementos estão sumarizados
        /// </summary>
        public PeriodoEnum Periodo { get; set; }

        /// <summary>
        /// Caso o período seja personalizado, indica o intervalo período.
        /// </summary>
        public TimeSpan PeriodoPersonalizado { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberHistoricoCotacaoBMFResponse()
        {
            this.Resultado = new List<CotacaoBMFInfo>();
        }
    }
}
