using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using System.Runtime.Serialization;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    [DataContract]
    public class ListarOrdensRequest 
    {
        [DataMember]
        public string TipoOrdem { get; set; }
    }
}
