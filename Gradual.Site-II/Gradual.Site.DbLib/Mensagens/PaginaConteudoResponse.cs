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
    public class PaginaConteudoResponse : MensagemResponseBase
    {
        [DataMember]
        public PaginaConteudoInfo PaginaConteudo { get; set; }

        [DataMember]
        public List<PaginaConteudoInfo> ListaPaginaConteudo { get; set; }
    }
}
