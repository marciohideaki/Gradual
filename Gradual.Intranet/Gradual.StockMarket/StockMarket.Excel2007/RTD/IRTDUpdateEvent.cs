using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace StockMarket.Excel2007.RTD
{
    // O GUID abaixo eh exatamente igual ao usado pela Microsoft, pois deve chamar a mesma interface do Excel RTD.
    [ComImport, TypeLibType((short)0x1040), Guid("A43788C1-D91B-11D3-8F39-00C04F3651B8")]
    public interface IRTDUpdateEvent
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(10), PreserveSig]
        void UpdateNotify();

        [DispId(11)]
        int HeartbeatInterval
        {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)]
            get;
            [param: In]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(12)]
        void Disconnect();
    }
}
