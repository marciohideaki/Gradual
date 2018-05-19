using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Automacao.Ordens.Mensagens
{
    [Serializable]
    public class CancelarStartStopOrdensRequest : MensagemResponseClienteBase
    {
        public string Instrument { get; set; }
        public int IdStopStart { get; set; }
        public int IdStopStartStatus { get; set; }
    }
}
