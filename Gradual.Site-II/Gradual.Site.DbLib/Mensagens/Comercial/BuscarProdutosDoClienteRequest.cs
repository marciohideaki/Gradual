using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens
{
    [Serializable]
    [DataContract]
    public class BuscarProdutosDoClienteRequest: MensagemRequestBase
    {
        [DataMember]
        public string CpfCnpj { get; set; }
    }
}
