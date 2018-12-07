using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta;

namespace Gradual.Site.DbLib.Mensagens
{
    [Serializable]
    [DataContract]
    public class FundoResponse : MensagemResponseBase
    {
        [DataMember]
        public List<FundoInfo> ListaFundo { get; set; }

        [DataMember]
        public FundoInfo Fundo { get; set; }
    }
}
