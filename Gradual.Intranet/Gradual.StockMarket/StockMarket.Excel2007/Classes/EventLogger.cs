using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace StockMarket.Excel2007
{
    public static class EventLogger
    {
        public static void WriteFormat(EventLogEntryType pType, string pMessage, params object[] pParams)
        {
            if (!EventLog.SourceExists("StockMarket"))
            {
                EventLog.CreateEventSource("StockMarket", "StockMarketLog");
            }

            EventLog.WriteEntry("StockMarket", string.Format(pMessage, pParams), pType);
        }
    }
}
