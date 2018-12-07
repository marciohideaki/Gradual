using System;
using Gradual.OMS.Library;
using Gradual.Site.Lib.Dados;

namespace Gradual.Site.Lib.Mensagens
{

    [Serializable]
    [DataContract]
    public class EstruturaRequest : MensagemRequestBase
    {
        [DataMember]
        public EstruturaInfo Estrutura { get; set; }
    }
}
