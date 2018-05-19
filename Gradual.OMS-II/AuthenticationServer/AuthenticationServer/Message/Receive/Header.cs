using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AS.Messages
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class Header
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string pStrTipoMensagem;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string pStrTipoBolsa;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
        public string pStrDateTime;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string pStrInstrument;
    }
}
