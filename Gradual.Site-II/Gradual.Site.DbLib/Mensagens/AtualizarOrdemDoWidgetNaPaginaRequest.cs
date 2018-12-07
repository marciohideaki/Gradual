using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens
{
    [Serializable]
    [DataContract]
    public class AtualizarOrdemDoWidgetNaPaginaRequest : MensagemRequestBase
    {
        [DataMember]
        public int IdDaEstrutura { get; set; }
        
        [DataMember]
        public List<int> OrdemDeWidgets { get; set; }
    }
}
