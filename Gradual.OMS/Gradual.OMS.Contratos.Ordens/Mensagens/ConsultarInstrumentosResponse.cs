using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de consulta de instrumentos
    /// </summary>
    public class ConsultarInstrumentosResponse : MensagemResponseBase
    {
        /// <summary>
        /// Lista de instrumentos encontrados
        /// </summary>
        public List<InstrumentoInfo> Instrumentos { get; set; }
    }
}
