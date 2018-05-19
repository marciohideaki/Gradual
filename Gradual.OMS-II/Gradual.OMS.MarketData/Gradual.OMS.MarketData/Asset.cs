using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.MarketData
{
    public class Asset: QuickFix.StringField
    {
        public const int FIELD = 6937;

        public Asset(): base (6937) {}

        public Asset(string data): base(6937, data) {}

    }
}
