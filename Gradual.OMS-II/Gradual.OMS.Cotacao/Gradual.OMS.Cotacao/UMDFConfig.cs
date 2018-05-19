using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Cotacao
{
    public class UMDFConfig
    {
        [XmlElement("Porta")]
        public List<string> Portas { get; set; }
    }
}
