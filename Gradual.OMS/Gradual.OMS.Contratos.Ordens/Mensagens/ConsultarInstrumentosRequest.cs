using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de consulta de instrumentos
    /// </summary>
    [Mensagem(TipoMensagemResponse = typeof(ConsultarInstrumentosResponse))]
    public class ConsultarInstrumentosRequest : MensagemRequestBase
    {
        /// <summary>
        /// Codigo da bolsa onde se deseja consultar o instrumento
        /// </summary>
        public string CodigoBolsa { get; set; }

        /// <summary>
        /// Consulta por symbol
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Consulta por security ID
        /// </summary>
        public string SecurityID { get; set; }
    }
}
