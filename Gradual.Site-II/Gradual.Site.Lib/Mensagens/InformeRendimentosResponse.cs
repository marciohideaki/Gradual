using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.Lib.Dados.MinhaConta;

namespace Gradual.Site.Lib.Mensagens
{
    [Serializable]
    [DataContract]
    public class InformeRendimentosResponse : MensagemResponseBase
    {
        [DataMember]
        public List<InformeRendimentosInfo> ListaInformeRendimentos { get; set; }

        [DataMember]
        public InformeRendimentosInfo InformeRendimento { get; set; }
    }
}
