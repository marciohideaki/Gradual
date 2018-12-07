using System;
using System.Collections.Generic;
using Gradual.OMS.Library;
using Gradual.Site.Lib.Dados.MinhaConta.Suitability;

namespace Gradual.Site.Lib.Mensagens
{
    [Serializable]
    [DataContract]
    public class FichaPerfilResponse : MensagemResponseBase
    {
        [DataMember]
        public List<FichaPerfilInfo> Retorno { get; set; }

        public FichaPerfilResponse()
        {
            this.Retorno = new List<FichaPerfilInfo>();
        }
    }
}
