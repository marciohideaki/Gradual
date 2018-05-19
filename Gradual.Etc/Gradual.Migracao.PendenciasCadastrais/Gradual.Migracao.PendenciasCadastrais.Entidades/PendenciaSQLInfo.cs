using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Migracao.PendenciasCadastrais.Entidades
{
    public class ClientePendenciaSQLInfo
    {
        public int IdTipoPendencia { get; set; }

        public int IdCliente { get; set; }

        public string DsPendencia { get; set; }

        public DateTime DtCadastroPendencia { get; set; }
    }
}
