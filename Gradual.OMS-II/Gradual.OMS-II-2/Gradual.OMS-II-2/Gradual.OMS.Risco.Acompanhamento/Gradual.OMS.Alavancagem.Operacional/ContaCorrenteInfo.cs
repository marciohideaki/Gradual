using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Alavancagem.Operacional;

namespace Gradual.OMS.Alavancagem.Operacional
{
    public class ContaCorrenteInfo
    {
        public int IdCliente { set; get; }
        public decimal? SaldoD0 { set; get; }
        public decimal? SaldoD1 { set; get; }
        public decimal? SaldoD2 { set; get; }
        public decimal? Liquidacoes { set; get; }
    }
}
