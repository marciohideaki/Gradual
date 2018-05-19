using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    [Serializable]
    public class AtualizaOrdemStartStopRequest : MensagemRequestBase
    {
        public string IdStartStop { get; set; }
        public int IdStopStartStatus { get; set; }
        public decimal PrecoReferencia { get; set; }
    }
}
