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
    public class ClienteFundoResponse :MensagemResponseBase
    {
        [DataMember]
        public List<ClienteFundosInfo> ListaFundos { get; set; }

        [DataMember]
        public ClienteFundosInfo Fundo { get; set; }

        public ClienteFundoResponse()
        {
            ListaFundos = new List<ClienteFundosInfo>();
        }
    }
}
