using System;
using System.Runtime.InteropServices;

namespace Gradual.OMS.Cotacao.Mensagens.Recebidas
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class A4_ResponseSignIn
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
        public string pStrIdCliente;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string pStrIdSistema;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string pStrStatusRequest;



    }
}
