using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.BMF.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Orbite.RV.Contratos.MarketData.BMF.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de detalhe de instrumentos BMF
    /// </summary>
    public class ReceberDetalheInstrumentoBMFRequest : MensagemRequestBase
    {
        /// <summary>
        /// Codigo Negociacao para localizar o instrumento
        /// </summary>
        public string CodigoNegociacao { get; set; }
    }
}
