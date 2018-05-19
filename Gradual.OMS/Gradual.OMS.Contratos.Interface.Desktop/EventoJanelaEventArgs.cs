using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop.Dados;

namespace Gradual.OMS.Contratos.Interface.Desktop
{
    public class EventoJanelaEventArgs : EventArgs
    {
        public JanelaInfo JanelaInfo { get; set; }
    }
}
