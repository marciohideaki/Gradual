using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Contratos.Interface.Desktop.Dados
{
    [Serializable]
    public class SkinInfo
    {
        [XmlAttribute]
        public string Nome { get; set; }

        public List<RelacaoItemControleInfo> RelacaoItensControles { get; set; }
        public List<JanelaInicializacaoInfo> InicializacaoJanelas { get; set; }

        public SkinInfo()
        {
            this.RelacaoItensControles = new List<RelacaoItemControleInfo>();
            this.InicializacaoJanelas = new List<JanelaInicializacaoInfo>();
        }
    }
}
