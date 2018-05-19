using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Informações sobre o endpoint wcf
    /// </summary>
    public class ServicoEndPointInfo
    {
        public string Endereco { get; set; }

        [XmlElement(IsNullable = true)]
        public string NomeBindingType { get; set; }
    }
}
