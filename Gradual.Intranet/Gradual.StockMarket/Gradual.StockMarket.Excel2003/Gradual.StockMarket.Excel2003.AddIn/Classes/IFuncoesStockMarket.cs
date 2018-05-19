using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace Gradual.StockMarket
{
    public interface IFuncoesStockMarket
    {
        // Caro programador: Antes de mexer por aqui, ler os seguintes artigos:
        //   http://blogs.msdn.com/b/gabhan_berry/archive/2008/04/07/writing-custom-excel-worksheet-functions-in-c_2d00_sharp.aspx
        //   http://msdn.microsoft.com/en-us/library/ms173189%28VS.80%29.aspx 

        object[] SM_COTACAO(string Instrumento);

        object[,] SM_TICKER(string Instrumento);

        object[,] SM_LIVROOFERTAS(string Instrumento);

        object SM_ELEMENTO_COTACAO(object pElemento, string pInstrumento);
    }
}