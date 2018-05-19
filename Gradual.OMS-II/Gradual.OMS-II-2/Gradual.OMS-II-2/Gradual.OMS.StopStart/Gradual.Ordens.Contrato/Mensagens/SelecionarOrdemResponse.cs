using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.OMS.Ordens.StartStop.Lib;

using System.Runtime.Serialization;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    [DataContract]    
    public class SelecionarOrdemResponse : MensagemResponseBase
    {
        [DataMember]
        public AutomacaoOrdensInfo AutomacaoOrdem { get; set; }
    }
}
