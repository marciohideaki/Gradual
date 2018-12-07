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
    public class ClienteClubeResponse : MensagemResponseBase
    {
        [DataMember]
        public List<ClienteClubesInfo> ListaClube { get; set; }

        [DataMember]
        public ClienteClubesInfo Clube { get; set; }

        public ClienteClubeResponse()
        {
            ListaClube = new List<ClienteClubesInfo>();
        }
    }
}
