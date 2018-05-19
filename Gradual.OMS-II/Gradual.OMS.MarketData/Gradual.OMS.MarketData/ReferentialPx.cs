using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.MarketData
{
    public class ReferentialPx : QuickFix.DoubleField
    {
        public const int FIELD = 6933;

        public ReferentialPx() : base(6933) { }

        public ReferentialPx(double data) : base(6933, data) { }
    }
}
