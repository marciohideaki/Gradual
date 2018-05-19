using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Alavancagem.Operacional
{

    public class LimitePosicaoInfo
    {
        public int IdClienteParametro { set; get; }
        public int IdClienteParametroValor { set; get; }
        public decimal? VlMovimento { set; get; }
        public decimal? VlAlocado { set; get; }
        public decimal? VlDisponivel { set; get; }
    }

}
