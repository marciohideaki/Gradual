using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.Spider.CommServer
{
    [Serializable]
    public class ProviderDescription
    {
        [XmlElement("Tipo")]
        public List<string> Tipo { get; set; }

        public string ServiceIP { get; set; }

        public int ServicePort { get; set; }
    }

    [Serializable]
    public class ProviderManagerConfig
    {
        public List<ProviderDescription> Providers { get; set; }
    }
}
