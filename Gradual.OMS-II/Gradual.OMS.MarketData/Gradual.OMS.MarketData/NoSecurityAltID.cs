using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.MarketData
{
    public class NoSecurityAltID : QuickFix.Group
    {
        public NoSecurityAltID(): base(454, 455, new int[] { 455, 456, 0 }){}

        public void set(QuickFix.SecurityAltID value) {
            setField(value);
        }

        public QuickFix.SecurityAltID get(
            QuickFix.SecurityAltID value) {
            getField(value);

            return value;
        }

        public QuickFix.SecurityAltID getSecurityAltID() {
            QuickFix.SecurityAltID value = new QuickFix.SecurityAltID();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.SecurityAltID field) {
            return isSetField(field);
        }

        public bool isSetSecurityAltID() {
            return isSetField(455);
        }

        public void set(QuickFix.SecurityAltIDSource value) {
            setField(value);
        }

        public QuickFix.SecurityAltIDSource get( QuickFix.SecurityAltIDSource value)
        {
            getField(value);

            return value;
        }

        public QuickFix.SecurityAltIDSource getSecurityAltIDSource()
        {
            QuickFix.SecurityAltIDSource value = new QuickFix.SecurityAltIDSource();
            getField(value);

            return value;
        }

        public bool isSet(QuickFix.SecurityAltIDSource field) {
            return isSetField(field);
        }

        public bool isSetSecurityAltIDSource() {
            return isSetField(456);
        }
    }
}
