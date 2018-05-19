using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using System.Runtime.Serialization;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    [DataContract(Namespace="http://gradual")]
    public class CancelarStartStopOrdensRequest 
    {
        [DataMember]
        public string Instrument { get; set; }
        
        [DataMember]
        public int IdStopStart { get; set; }

        [DataMember]
        public int IdStopStartStatus { get; set; }
    }
}
