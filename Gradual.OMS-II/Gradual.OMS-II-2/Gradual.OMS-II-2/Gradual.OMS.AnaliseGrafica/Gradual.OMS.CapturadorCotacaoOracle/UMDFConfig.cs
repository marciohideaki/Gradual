using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.CapturadorCotacaoOracle
{
    public class UMDFConfig
    {
        [XmlElement("Porta")]
        public List<int> Portas { get; set; }
    }
}
