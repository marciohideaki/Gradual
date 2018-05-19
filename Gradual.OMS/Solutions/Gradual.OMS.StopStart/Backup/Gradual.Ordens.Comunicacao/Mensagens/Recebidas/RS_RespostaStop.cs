using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Recebidas
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class RS_RespostaStop
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string pStrTipoMensagem;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string pStrTipoBolsa;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string pStrDateTime;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string pStrInstrument;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string pStrIdStopstart;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string pStrIdTipoOrdem;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string pStrStatus;

    }
}
