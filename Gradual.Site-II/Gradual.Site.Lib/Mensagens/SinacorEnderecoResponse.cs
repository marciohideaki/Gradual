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
    public class SinacorEnderecoResponse : MensagemResponseBase
    {
        [DataMember]
        public List<SinacorEnderecoInfo> ListaSinacorEndereco { get; set; }

        [DataMember]
        public SinacorEnderecoInfo SinacorEndereco { get; set; }
    }

}
