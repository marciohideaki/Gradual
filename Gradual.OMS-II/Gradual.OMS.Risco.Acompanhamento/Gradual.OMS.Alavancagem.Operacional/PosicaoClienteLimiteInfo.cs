using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Alavancagem.Operacional
{
    public class PosicaoClienteLimiteInfo
    {
        public string Instrumento { set; get; }

        public int Quantidade { set; get; }

        public Nullable<decimal> Preco { set; get; }

        public Nullable<decimal> Volume { set; get; }
       
    }
}
