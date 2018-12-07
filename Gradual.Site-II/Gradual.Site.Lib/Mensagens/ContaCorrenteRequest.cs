using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.Lib.Dados.MinhaConta;
using Gradual.OMS.ContaCorrente.Lib;

namespace Gradual.Site.Lib.Mensagens
{
    [Serializable]
    [DataContract]
    public class ContaCorrenteRequest : MensagemRequestBase
    {
        [DataMember]
        public ContaCorrenteInfo ContaCorrente { get; set; }
    }
}
