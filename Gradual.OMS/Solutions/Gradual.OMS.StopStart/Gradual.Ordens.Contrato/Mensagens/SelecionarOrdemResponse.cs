using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using System.Runtime.Serialization;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    [DataContract]    
    public class SelecionarOrdemResponse 
    {
        [DataMember]
        public AutomacaoOrdensInfo AutomacaoOrdem { get; set; }
    }
}
