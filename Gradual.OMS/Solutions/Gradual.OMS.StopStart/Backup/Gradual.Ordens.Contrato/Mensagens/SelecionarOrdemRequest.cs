using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    [Serializable]
    public class SelecionarOrdemRequest : MensagemResponseClienteBase
    {
        public int IdStopStart { get; set; }
    }
}
