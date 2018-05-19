using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Interface.Desktop.Dados
{
    [Serializable]
    public class ComandoInfo : ItemInfoBase
    {
        [XmlAttribute]
        public string Titulo { get; set; }

        [XmlAttribute]
        public string NomeToolbar { get; set; }

        [XmlAttribute]
        public string NomeMenu { get; set; }

        public Image Imagem { get; set; }

        [XmlAttribute]
        public string ImagemArquivo { get; set; }

        [XmlAttribute]
        public string ImagemResource { get; set; }

        [XmlAttribute]
        public bool RegistrarEmToolbar { get; set; }

        [XmlAttribute]
        public bool RegistrarEmMenu { get; set; }
    }
}
