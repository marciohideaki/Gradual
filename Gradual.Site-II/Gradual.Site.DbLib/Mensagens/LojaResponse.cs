using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Mensagens
{
    [Serializable]
    [DataContract]
    public class LojaResponse : Gradual.OMS.Library.MensagemResponseBase
    {
        [DataMember]
        public List<Gradual.Site.DbLib.Dados.LojaInfo> ListaLojas { get; set; }
    }
}
