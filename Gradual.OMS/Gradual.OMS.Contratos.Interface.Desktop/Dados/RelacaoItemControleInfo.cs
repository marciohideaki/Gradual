using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gradual.OMS.Contratos.Interface.Desktop.Dados
{
    [Serializable]
    public class RelacaoItemControleInfo
    {
        [XmlAttribute]
        public ItemTipoEnum ItemTipo { get; set; }

        [XmlAttribute]
        public string TipoObjetoDestino { get; set; }
    }
}
