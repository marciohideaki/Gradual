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
    public class SinacorEnderecoRequest : MensagemRequestBase
    {
        [DataMember]
        public SinacorEnderecoInfo SinacorEndereco { get; set; }
    }
}
