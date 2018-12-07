using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens
{
    [Serializable]
    [DataContract]
    public class BuscarHtmlDaPaginaRequest : MensagemRequestBase
    {
        [DataMember]
        public int IdDaPagina { get; set; }

        [DataMember]
        public byte RenderizandoNaAba { get; set; }

        [DataMember]
        public bool ExisteClienteLogado { get; set; }

        [DataMember]
        public string HostERaiz { get; set; }

        [DataMember]
        public string Versao { get; set; }
    }
}
