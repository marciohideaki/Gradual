using System;
using Gradual.OMS.Library;
using Gradual.Site.Lib.Dados.MinhaConta.Suitability;

namespace Gradual.Site.Lib.Mensagens
{
    [Serializable]
    [DataContract]
    public class FichaPerfilRequest : MensagemRequestBase
    {
        [DataMember]
        public FichaPerfilInfo Objeto { get; set; }

        public FichaPerfilRequest()
        {
            this.Objeto = new FichaPerfilInfo();
        }
    }
}
