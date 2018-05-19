using System;
using System.Runtime.InteropServices;

namespace Gradual.OMS.Cotacao.Mensagens.Recebidas
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class NE_Negocio
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string pStrTipoMensagem;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string pStrTipoBolsa;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string pStrDateTime;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string pStrInstrument;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string pStrDateTimeBvsp;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string pStrCorrBuySide;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string pStrCorrSelSide;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string pStrPrice;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string pStrQtty;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string pStrMaxDay;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string pStrMinDay;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string pStrVolAcum;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string pStrNumeroNegocios;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string pStrIndicadorVariacao;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string pStrVariacaoInstrumento;

    }
}
