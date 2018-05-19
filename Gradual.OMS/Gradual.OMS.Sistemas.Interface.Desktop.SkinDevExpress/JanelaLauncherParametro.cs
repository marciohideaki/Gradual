using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Sistemas.Interface.Desktop.SkinDevExpress
{
    [Serializable]
    public class JanelaLauncherParametro : JanelaParametro
    {
        public string SkinName { get; set; }
        public bool MostrarComoMDI { get; set; }
        public string DesktopInicial { get; set; }
    }
}
