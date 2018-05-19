using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Interface.Desktop.Dados
{
    [Serializable]
    public class HostInfo : ItemInfoBase
    {
        public HostInicializacaoTipoEnum InicializacaoTipo { get; set; }

        public int AppDomainId { get; set; }

        public HostInfo()
        {
            this.InicializacaoTipo = HostInicializacaoTipoEnum.Normal;
        }
    }
}
