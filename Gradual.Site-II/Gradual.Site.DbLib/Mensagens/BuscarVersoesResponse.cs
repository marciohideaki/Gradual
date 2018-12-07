using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados;

namespace Gradual.Site.DbLib.Mensagens
{
    public class BuscarVersoesResponse : MensagemResponseBase
    {
        [DataMember]
        public List<string> Versoes { get; set; }
    }
}
