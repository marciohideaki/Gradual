using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.RV.Contratos.MarketData.BMF.Dados;

namespace Orbite.RV.Contratos.MarketData.BMF.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de detalhe de instrumento BMF
    /// </summary>
    public class ReceberDetalheInstrumentoBMFResponse : MensagemResponseBase
    {
        /// <summary>
        /// Instrumento encontrado
        /// </summary>
        public InstrumentoBMFInfo Instrumento { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ReceberDetalheInstrumentoBMFResponse()
        {
        }
    }
}
