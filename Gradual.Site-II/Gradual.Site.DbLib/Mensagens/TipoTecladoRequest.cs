using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Mensagens
{
    [Serializable]
    [DataContract]
    public class TipoTecladoRequest: Gradual.OMS.Library.MensagemRequestBase
    {
        [DataMember]
        public int? CodigoCliente { get; set; }
        [DataMember]
        public int? CodigoLogin { get; set; }
        [DataMember]
        public String Email { get; set; }
    }
}
