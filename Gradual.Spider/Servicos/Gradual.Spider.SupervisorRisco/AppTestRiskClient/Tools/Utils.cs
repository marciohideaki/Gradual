using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AppTestRiskClient.Tools
{
    public class Utils
    {

        public static T FromBinaryString<T>(String msg)
        {
            byte[] buff = Encoding.UTF8.GetBytes(msg);
            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
            T s = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return s;
        }

    }
}
