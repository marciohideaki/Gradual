using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace Gradual.Utils.Hardware
{
    public class WMIReader
    {
        public static IList<string> GetPropertyValues(Connection WMIConnection,
                                                      string SelectQuery,
                                                      string className)
        {
            ManagementScope connectionScope = WMIConnection.GetConnectionScope;
            List<string> alProperties = new List<string>();
            SelectQuery msQuery = new SelectQuery(SelectQuery);
            ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(connectionScope, msQuery);

            try
            {
                foreach (ManagementObject item in searchProcedure.Get())
                {
                    foreach (string property in XMLConfig.GetSettings(className))
                    {
                        try { alProperties.Add(property + ": " + item[property].ToString()); }
                        catch (SystemException) { /* ignore error */ }
                    }
                }
            }
            catch (ManagementException e)
            {
                /* Do Nothing */
            }
            
            return alProperties;
        }
    }
}
