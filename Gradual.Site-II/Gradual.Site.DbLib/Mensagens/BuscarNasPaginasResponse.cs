using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados;

namespace Gradual.Site.DbLib.Mensagens
{
    public class BuscarNasPaginasResponse : MensagemResponseBase
    {
        [DataMember]
        public List<PaginaBuscaInfo> Resultados { get; set; }
    }
}
