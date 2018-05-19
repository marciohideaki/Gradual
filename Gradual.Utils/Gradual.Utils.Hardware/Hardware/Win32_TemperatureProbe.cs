using System;
using System.Collections.Generic;
using System.Text;

namespace Gradual.Utils.Hardware
{
    public class Win32_TemperatureProbe : IWMI 
    {
             Connection WMIConnection;

        public Win32_TemperatureProbe(Connection WMIConnection)
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
