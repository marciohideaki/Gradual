using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Utils
{
    public static class MethodHelper
    {
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]

        public static string GetCurrentMethod(int pPad, char pCharacter)
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame sf = st.GetFrame(1);
            return sf.GetMethod().Name.PadRight(pPad, pCharacter);
        }

        public static string GetCurrentMethod()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame sf = st.GetFrame(1);
            return sf.GetMethod().Name;
        }

        public static string GetCurrentType()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame sf = st.GetFrame(1);
            return sf.GetMethod().DeclaringType.Name;
        }
    }
}
