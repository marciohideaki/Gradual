using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Alavancagem.Operacional;

namespace Gradual.OMS.Alavancagem.Operacional
{
    public class CustodiaPapelInfo
    {
        public string Instrumento { set; get; }

        public int CodigoBovespa { set; get; }

        public decimal Preco { set; get; }

        public int Quantidade { set; get; }

        public string NatOperacao { set; get; }

        public int OrderStatusID { set; get; }

        public DateTime TransactTime { set; get; }

        public string Mercado { set; get; }

        public int OrderID { set; get; }

    }
}
