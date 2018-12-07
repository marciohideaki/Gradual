using System;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados;

namespace Gradual.Site.DbLib.Mensagens
{

    [Serializable]
    [DataContract]
    public class EstruturaRequest : MensagemRequestBase
    {
        [DataMember]
        public EstruturaInfo Estrutura { get; set; }
    }
}
