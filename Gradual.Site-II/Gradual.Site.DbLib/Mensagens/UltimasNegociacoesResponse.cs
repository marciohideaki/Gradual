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
    public class UltimasNegociacoesResponse : MensagemResponseBase
    {
        [DataMember]
        public UltimasNegociacoesInfo UltimasNegociacoes { get; set; }

        [DataMember]
        public List<UltimasNegociacoesInfo> ListaUltimasNegociacoes { get; set; }
    }
}
