using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta;

namespace Gradual.Site.DbLib.Mensagens
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
