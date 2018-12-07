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
    public class ContaCorrenteResponse : MensagemResponseBase
    {
        [DataMember]
        public List<Gradual.OMS.ContaCorrente.Lib.ContaCorrenteInfo> ListaContaCorrente { get; set; }

        [DataMember]
        public Gradual.OMS.ContaCorrente.Lib.ContaCorrenteInfo ContaCorrente { get; set; }
    }
}
