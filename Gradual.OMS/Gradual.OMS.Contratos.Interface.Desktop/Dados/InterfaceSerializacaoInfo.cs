using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Interface.Desktop.Dados
{
    [Serializable]
    public class InterfaceSerializacaoInfo
    {
        public List<DesktopInfo> Desktops { get; set; }
        public List<JanelaSerializacaoInfo> Janelas { get; set; }
        public JanelaSerializacaoInfo JanelaLauncher { get; set; }
        public string IdDesktopDefault { get; set; }

        public InterfaceSerializacaoInfo()
        {
            this.Desktops = new List<DesktopInfo>();
            this.Janelas = new List<JanelaSerializacaoInfo>();
        }
    }
}
