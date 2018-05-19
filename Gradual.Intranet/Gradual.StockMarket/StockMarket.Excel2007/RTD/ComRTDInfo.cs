using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace StockMarket.Excel2007.RTD
{
    [Guid("A7E0376C-B5A2-4C81-B30A-567F271A14A2")] [ComVisible(true)]
    public class ComRTDInfo : IComRTDInfo
    {
        public DateTime ultimaAtualizacao { get; set; }
        public bool usuarioLogado { get; set; }
        public string usuario { get; set; }
        public string acao { get; set; }
        public string parametro { get; set; }
        public int qtdRegistrosPosicaoNet { get; set; }
    }
}
