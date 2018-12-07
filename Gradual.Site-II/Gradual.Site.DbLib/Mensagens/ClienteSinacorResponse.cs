using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta;

namespace Gradual.Site.DbLib.Mensagens
{
    /// <summary>
    /// Classe de Response das informações de Cliente no sinacor
    /// </summary>
    [Serializable]
    [DataContract]
    public class ClienteSinacorResponse : MensagemResponseBase
    {
        /// <summary>
        /// Propriedade de lista de retorno do info de cliente do sinacor
        /// </summary>
        [DataMember]
        public List<ClienteSinacorInfo> ListaClienteSinacor { get; set; }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public ClienteSinacorResponse()
        {
            ListaClienteSinacor = new List<ClienteSinacorInfo>();
        }
    }
}
