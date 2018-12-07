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
    public class UltimasNegociacoesResponse : MensagemResponseBase
    {
        [DataMember]
        public UltimasNegociacoesInfo UltimasNegociacoes { get; set; }

        [DataMember]
        public List<UltimasNegociacoesInfo> ListaUltimasNegociacoes { get; set; }
    }
}
