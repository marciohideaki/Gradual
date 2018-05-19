using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Contratos.Interface.Desktop.Dados
{
    [Serializable]
    public class JanelaInicializacaoInfo
    {
        [XmlAttribute]
        public string TipoJanelaAlvo { get; set; }
        public List<ComandoInfo> Comandos { get; set; }

        public JanelaInicializacaoInfo()
        {
            this.Comandos = new List<ComandoInfo>();
        }
    }
}
