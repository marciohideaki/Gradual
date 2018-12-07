using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados;

namespace Gradual.Site.DbLib.Mensagens
{
    [Serializable]
    [DataContract]
    public class ConteudoResponse : MensagemResponseBase
    {
        [DataMember]
        public ConteudoInfo Conteudo { get; set; }

        [DataMember]
        public List<ConteudoInfo> ListaConteudo { get; set; }
    }
}
