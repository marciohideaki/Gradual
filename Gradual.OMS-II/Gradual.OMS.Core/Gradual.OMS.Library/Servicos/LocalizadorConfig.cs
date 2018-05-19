using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Library.Servicos
{
    public class AllowedHosts
    {
        [XmlElement("AllowHost")]
        public List<string> AllowedHost { get; set; }

        public AllowedHosts()
        {
            AllowedHost = new List<string>();
        }
    }

    public class LocalizadorConfig
    {
        public AllowedHosts AllowedHosts { get; set; }
    }
}
