using System;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta.Suitability;

namespace Gradual.Site.DbLib.Mensagens
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
