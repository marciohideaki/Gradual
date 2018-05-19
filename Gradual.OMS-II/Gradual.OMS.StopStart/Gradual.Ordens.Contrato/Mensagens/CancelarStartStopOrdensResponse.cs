using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using System.Runtime.Serialization;
using Gradual.OMS.Ordens.StartStop.Lib;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    [DataContract(Namespace="http://gradual")]
    public class CancelarStartStopOrdensResponse : MensagemResponseBase
    {
        [DataMember]
        int IDStopStart { get; set; }

        [DataMember]
        int CodCliente { get; set;}

        [DataMember]
        int OrderQtdy {get; set;}
            
        [DataMember]
        string Symbol {get; set;}

        [DataMember]
        AutomacaoOrdensInfo _AutomacaoOrdensInfo { get; set; }
    }
}
