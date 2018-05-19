using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.MarketData
{
    public class UniqueTradeID : QuickFix.StringField
    {
        public const int FIELD = 6032;

        public UniqueTradeID() : base(6032) { }
        public UniqueTradeID(string data) : base(6032, data) { }
    }
}
