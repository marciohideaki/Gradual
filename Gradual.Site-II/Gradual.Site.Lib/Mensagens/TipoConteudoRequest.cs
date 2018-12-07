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
    public class TipoConteudoRequest : MensagemRequestBase
    {
        [DataMember]
        public TipoDeConteudoInfo TipoConteudo { get; set; }
    }
}
