using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Interface.Desktop.Dados
{
    [Serializable]
    public class JanelaInfo : ItemInfoBase
    {
        public string IdDesktop { get; set; }
        public string IdHost { get; set; }

        public JanelaInfo()
        {
            this.ItemTipo = ItemTipoEnum.JanelaForm;
        }
    }
}
