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
    public class FundoRequest : MensagemRequestBase
    {
        [DataMember]
        public string CpfDoCliente { get; set; }

        [DataMember]
        public FundoInfo Fundo { get; set; }
    }
}
