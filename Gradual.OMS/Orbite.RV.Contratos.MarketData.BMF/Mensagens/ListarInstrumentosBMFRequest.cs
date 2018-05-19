using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.RV.Contratos.MarketData.BMF.Dados;

namespace Orbite.RV.Contratos.MarketData.BMF.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de lista de instrumentos BMF
    /// </summary>
    public class ListarInstrumentosBMFRequest : MensagemRequestBase
    {
        /// <summary>
        /// Permite filtro por tipo de instrumentos
        /// </summary>
        public InstrumentoBMFTipoMercadoEnum FiltroTipoMercado { get; set; }

        /// <summary>
        /// Permite filtro por código de mercadoria
        /// </summary>
        public string FiltroCodigoMercadoria { get; set; }
    }
}
