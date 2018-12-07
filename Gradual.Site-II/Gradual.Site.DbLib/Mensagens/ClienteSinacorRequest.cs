using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta;

namespace Gradual.Site.DbLib.Mensagens
{
    /// <summary>
    /// Classe de Request de informações de cliente do sinacor
    /// </summary>
    [Serializable]
    [DataContract]
    public class ClienteSinacorRequest : MensagemRequestBase
    {
        /// <summary>
        /// Propriedade de request da info do sinacor de cliente
        /// </summary>
        [DataMember]
        public ClienteSinacorInfo ClienteSinacor { get; set; }
    }
}
