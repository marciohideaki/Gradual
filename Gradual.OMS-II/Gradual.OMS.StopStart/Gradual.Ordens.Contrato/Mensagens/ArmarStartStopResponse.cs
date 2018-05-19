using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.OMS.Ordens.StartStop.Lib;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    [DataContract(Namespace="http://gradual")]
    public class ArmarStartStopResponse : MensagemResponseBase
    {
        [DataMember]
        public int IdStopStart { get; set; }

        [DataMember]
        public AutomacaoOrdensInfo _AutomacaoOrdensInfo { get; set; }
    }
}
