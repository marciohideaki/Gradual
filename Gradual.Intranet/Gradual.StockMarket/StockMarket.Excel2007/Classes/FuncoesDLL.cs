using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using StockMarket.Excel2007.RTD;

namespace StockMarket.Excel2007
{
    [Guid("ACC7E8E3-BA60-3244-84FD-17159E5E64DB")]
    [ProgId("StockMarket.RTD")]
    [ComVisible(true)]
    public partial class Funcoes : IRTDServer
    {
        private const String rtdName = "StockMarket.RTD";
    }
}
