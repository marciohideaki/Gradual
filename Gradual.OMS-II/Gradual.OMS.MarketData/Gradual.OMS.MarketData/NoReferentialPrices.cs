using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.MarketData
{
    public static class NoReferentialPrices : QuickFix.Group
    {

        public NoReferentialPrices() : base(6932, 6934, new int[] { 6934, 6933, 60, 0 }) { }

        public void set(Gradual.OMS.MarketData.ReferentialPriceType value)
        {
            setField(value);
        }

        public Gradual.OMS.MarketData.ReferentialPriceType get(
                Gradual.OMS.MarketData.ReferentialPriceType value)
        {
            getField(value);

            return value;
        }

        public Gradual.OMS.MarketData.ReferentialPriceType getReferentialPriceType()
        {
            Gradual.OMS.MarketData.ReferentialPriceType value =
                new Gradual.OMS.MarketData.ReferentialPriceType();

            getField(value);

            return value;
        }

        public bool isSet(Gradual.OMS.MarketData.ReferentialPriceType field)
        {
            return isSetField(field);
        }

        public bool isSetReferentialPriceType()
        {
            return isSetField(6934);
        }

        public void set(Gradual.OMS.MarketData.ReferentialPx value)
        {
            setField(value);
        }

        public Gradual.OMS.MarketData.ReferentialPx get(
                Gradual.OMS.MarketData.ReferentialPx value)
        {
            getField(value);

            return value;
        }

        public Gradual.OMS.MarketData.ReferentialPx getReferentialPx()
        {
            Gradual.OMS.MarketData.ReferentialPx value =
                new Gradual.OMS.MarketData.ReferentialPx();
            getField(value);

            return value;
        }

        public bool isSet(Gradual.OMS.MarketData.ReferentialPx field)
        {
            return isSetField(field);
        }

        public bool isSetReferentialPx()
        {
            return isSetField(6933);
        }

        public void set(QuickFix.TransactTime value)
        {
            setField(value);
        }

        public QuickFix.TransactTime get(
                QuickFix.TransactTime value)
        {
            getField(value);

            return value;
        }

        public QuickFix.TransactTime getTransactTime()
        {
            QuickFix.TransactTime value = new QuickFix.TransactTime();
            getField(value);

            return value;
        }

        public bool isSet(
                QuickFix.TransactTime field)
        {
            return isSetField(field);
        }

        public bool isSetTransactTime()
        {
            return isSetField(60);
        }
    }
}
