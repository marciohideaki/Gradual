using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteClienteParametroGrupo
    {
        public int IdCliente { get; set; }

        public int IdParametro { get; set; }

        public List<int> ListaGrupos { get; set; }

        public TransporteClienteParametroGrupo() { }

    }
}