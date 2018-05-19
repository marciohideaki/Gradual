using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Gradual.OMS.Contratos.Interface.Desktop.Dados
{
    [Serializable]
    public class ControleInfo : ItemInfoBase
    {
        [XmlAttribute]
        public string Titulo { get; set; }

        public ControleInfo()
        {
            this.DockStyle = DockStyle.Fill;
            this.ItemTipo = ItemTipoEnum.Controle;
        }
    }
}
