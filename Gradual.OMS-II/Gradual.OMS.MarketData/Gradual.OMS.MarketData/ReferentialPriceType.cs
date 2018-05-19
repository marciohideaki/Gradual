using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.MarketData
{
    public class ReferentialPriceType : QuickFix.IntField
    {
        public const int FIELD = 6934;
        public const int SETTLEMENT_PRICE = 0;
        public const int REFERENCE_PRICE = 1;
        public const int OPERATIONAL_TUNNEL_UPPER_LIMIT = 2;
        public const int OPERATIONAL_TUNNEL_LOWER_LIMIT = 3;
        public const int NO_REFERENTIAL_PRICE_AVAILABLE = 4;
        public const int PREVIOUS_DAY_SETTLEMENT_PRICE = 5;

        public ReferentialPriceType() : base (6934) {}

        public ReferentialPriceType(int data): base(6934,data) {}
    }
}
