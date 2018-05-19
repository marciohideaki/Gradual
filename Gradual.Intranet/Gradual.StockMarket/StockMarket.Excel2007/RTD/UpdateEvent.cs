using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StockMarket.Excel2007.RTD
{
    public partial class UpdateEvent : IRTDUpdateEvent
    {
        public int HeartbeatInterval { get; set; }

        public UpdateEvent()
        {
            HeartbeatInterval = -1;
        }

        public void Disconnect()
        {
        }

        public void UpdateNotify()
        {
        }
    }
}
