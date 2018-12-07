using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens
{
    [Serializable]
    [DataContract]
    public class BuscarHtmlDaPaginaResponse : MensagemResponseBase
    {
        public string URL { get; set; }

        public string HTML { get; set; }

        public bool UtilizadoCache { get; set; }
    }
}
