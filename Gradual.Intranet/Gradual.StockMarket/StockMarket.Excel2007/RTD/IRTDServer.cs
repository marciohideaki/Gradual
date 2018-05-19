using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace StockMarket.Excel2007.RTD
{
    // O GUID abaixo eh exatamente igual ao usado pela Microsoft, pois deve chamar a mesma interface que o Excel RTD chama.
    [ComImport, TypeLibType((short)0x1040), Guid("EC0E6191-DB51-11D3-8F3E-00C04F3651B8")]

    public interface IRTDServer
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(10)]
        int ServerStart([In, MarshalAs(UnmanagedType.Interface)] IRTDUpdateEvent callback);

        [return: MarshalAs(UnmanagedType.Struct)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)]
        object ConnectData([In] int topicId, [In, MarshalAs(UnmanagedType.SafeArray,
            SafeArraySubType = VarEnum.VT_VARIANT)] ref object[] parameters, [In, Out] ref bool newValue);

        [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(12)]
        Array RefreshData([In, Out] ref int topicCount);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(13)]
        void DisconnectData([In] int topicId);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(14)]
        int Heartbeat();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(15)]
        void ServerTerminate();
    }
}
