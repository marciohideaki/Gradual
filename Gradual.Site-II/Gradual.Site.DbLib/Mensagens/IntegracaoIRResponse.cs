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
    public class IntegracaoIRResponse : MensagemResponseBase
    {
        [DataMember]
        public List<IntegracaoIRInfo> ListaIntegracaoIR { get; set; }

        [DataMember]
        public IntegracaoIRInfo IntegracaoIR { get; set; }
    }
}
