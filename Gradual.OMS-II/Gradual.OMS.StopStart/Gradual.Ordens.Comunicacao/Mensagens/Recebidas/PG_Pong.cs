using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Gradual.OMS.Ordens.Comunicacao.Mensagens.Recebidas
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class PG_Pong
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string pStrTipoMensagem;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string pStrTipoBolsa;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string pStrDateTime;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string pStrInstrument;
    }
}
