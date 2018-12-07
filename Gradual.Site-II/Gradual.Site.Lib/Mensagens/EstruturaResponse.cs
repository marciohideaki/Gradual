using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.Lib.Dados;

namespace Gradual.Site.Lib.Mensagens
{
    [Serializable]
    [DataContract]
    public class EstruturaResponse : MensagemResponseBase
    {
        [DataMember]
        public EstruturaInfo Estrutura { get; set; }

        [DataMember]
        public List<EstruturaInfo> ListaEstrutura { get; set; }
    }
}
