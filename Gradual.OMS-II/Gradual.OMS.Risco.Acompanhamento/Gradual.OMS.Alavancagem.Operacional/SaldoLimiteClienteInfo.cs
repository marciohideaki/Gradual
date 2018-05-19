using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Alavancagem.Operacional
{
    public class SaldoLimiteClienteInfo
    {
        public int IdCliente { set; get; }
        public decimal ValorDisponivel { set; get; }
        public string TipoLimite { set; get; }

    }
}
