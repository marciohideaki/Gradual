using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace StockMarket.Excel2007.RTD
{
    [Guid("A2B06D19-F8BF-4440-BCB0-4B73BAE5D9A5"), ProgId("StockMarket.Event"), ComVisible(true)]
    public partial class UpdateEvent : IRTDUpdateEvent
    {
        private const String eventName = "StockMarket.Event";
    }
}
