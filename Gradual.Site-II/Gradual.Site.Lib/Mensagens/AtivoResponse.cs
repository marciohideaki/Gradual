using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.Lib.Dados.MinhaConta;

namespace Gradual.Site.Lib.Mensagens
{
  
    [Serializable]
    [DataContract]
    public class AtivoResponse : MensagemResponseBase
    {
        [DataMember]
        public List<AtivoInfo> ListaAtivo { get; set; }

        [DataMember]
        public AtivoInfo Ativo { get; set; }
    }
}
