using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados;

namespace Gradual.Site.DbLib.Mensagens
{
    [Serializable]
    [DataContract]
    public class PaginaRequest : MensagemRequestBase
    {
        [DataMember]
        public PaginaInfo Pagina { get; set; }

        /// <summary>
        /// Tipo de usuário para manter quando virando de página segmentada para página única
        /// </summary>
        [DataMember]
        public int MergeFrom { get; set; }

        [DataMember]
        public string VersaoDaEstrutura { get; set; }
    }
}
