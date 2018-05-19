using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;

namespace Gradual.OMS.Contratos.Ordens.Mensagens
{
    /// <summary>
    /// Resposta a uma solicitação de detalhamento de ordem
    /// </summary>
    public class ReceberOrdemResponse : MensagemOrdemResponseBase
    {
        public OrdemInfo OrdemInfo { get; set; }
    }
}
