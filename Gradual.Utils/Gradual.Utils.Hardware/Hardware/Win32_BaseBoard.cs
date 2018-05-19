using System;
using System.Management;
using System.Collections.Generic;
using System.Text;

namespace Gradual.Utils.Hardware
{
    public class Win32_BaseBoard : IWMI 
    {
        Connection WMIConnection;

        public Win32_BaseBoard(Connection WMIConnection)
        {
            this.WMIConnection = WMIConnection;
        }

        public IList<string> GetPropertyValues()
        {
            string className = System.Text.RegularExpressions.Regex.Match(
                                  this.GetType().ToString(), "Win32_.*").Value;

            return WMIReader.GetPropertyValues(WMIConnection,
                                               "SELECT * FROM " + className,
                                               className);
        }
    }
}
