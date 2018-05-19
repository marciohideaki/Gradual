using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace StockMarket.Excel2007.RTD
{
    [Guid("D07F722E-748B-40AF-88E2-7FB545B1E852")] [ComVisible(true)]
    public interface IComRTDInfo
    {
        DateTime ultimaAtualizacao { get; set; }
        bool usuarioLogado { get; set; }
        string usuario { get; set; }
        string acao { get; set; }
        string parametro { get; set; }
        int qtdRegistrosPosicaoNet { get; set; }
    }
}
