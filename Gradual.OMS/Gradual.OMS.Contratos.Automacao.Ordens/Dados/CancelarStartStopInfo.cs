using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Dados
{
    [Serializable]
    [DataContract]
    public class CancelarStartStopInfo
    {
        [DataMember]
        public string Instrument { get; set; }
        [DataMember]
        public string IdStopStart { get; set; }
    }
}
