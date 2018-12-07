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
    public class BuscarItensDaListaRequest : MensagemRequestBase
    {
        [DataMember]
        public int IdDaLista { get; set; }
    }
}
