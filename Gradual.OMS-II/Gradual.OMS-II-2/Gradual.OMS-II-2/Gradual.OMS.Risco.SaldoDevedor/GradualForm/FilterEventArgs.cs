using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradualForm
{
    public class FilterEventArgs: EventArgs
    {
        public List<string> ListaClientes { get; set; }
        public bool Ascendente { get; set; }
    }

    public class FiltrarEventArgs : FilterEventArgs { }
}
