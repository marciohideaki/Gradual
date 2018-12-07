using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Dados.MinhaConta;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens
{
    [Serializable]
    [DataContract]
    public class ClienteClubeRequest : MensagemRequestBase
    {
        [DataMember]
        public int IdCliente { get; set; }
    }
}
