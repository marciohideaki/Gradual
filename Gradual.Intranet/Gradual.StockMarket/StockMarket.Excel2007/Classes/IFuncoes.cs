using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using StockMarket.Excel2007.RTD;

namespace StockMarket.Excel2007
{
    //[Guid("87C9C74B-70B7-47C2-A5FB-C50E18F87E6F")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IFuncoes
    {
        // Caro programador: Antes de mexer por aqui, ler os seguintes artigos:
        //   http://blogs.msdn.com/b/gabhan_berry/archive/2008/04/07/writing-custom-excel-worksheet-functions-in-c_2d00_sharp.aspx
        //   http://msdn.microsoft.com/en-us/library/ms173189%28VS.80%29.aspx 

        object SM_COTACAO(string ativoPropriedade);

        object SM_COTACAO_LINHA(string ativoPropriedade);

        object SM_COTACAORAPIDA(string ativoPropriedade);

        object SM_LIVROOFERTAS(string ativoPropriedadeSentidoOcorrencia);

        object SM_POSICAO(string clienteAtivoPropriedade);

        object SM_POSICAO_NET(string clientePropriedadeOcorrencia);
    }
}
