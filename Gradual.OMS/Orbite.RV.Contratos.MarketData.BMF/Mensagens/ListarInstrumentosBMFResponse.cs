using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.BMF.Dados;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Orbite.RV.Contratos.MarketData.BMF.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de lista de instrumentos
    /// </summary>
    public class ListarInstrumentosBMFResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de instrumentos
        /// </summary>
        public List<InstrumentoBMFInfo> Instrumentos { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ListarInstrumentosBMFResponse()
        {
            this.Instrumentos = new List<InstrumentoBMFInfo>();
        }
    }
}
