using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Gradual.SaldoDevedor.WinApp.Classes
{
    class AssemblyHelper
    {
        /// <summary> 
        /// The company name of the calling assembly. 
        /// </summary> 
        public static string CompanyName
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        /// <summary> 
        /// The copyright holder of the calling assembly. 
        /// </summary> 
        public static string CopyrightHolder
        {
            get
            {
                object[] attributes = System.Reflection.Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary> 
        /// The description of the calling assembly. 
        /// </summary> 
        public static string Description
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <summary> 
        /// The product name of the calling assembly. 
        /// </summary> 
        public static string ProductName
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary> 
        /// The title of the calling assembly. 
        /// </summary> 
        public static string Title
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title.Length > 0)
                        return titleAttribute.Title;
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        /// <summary> 
        /// The version of the calling assembly. 
        /// </summary> 
        public static Version Version
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Version;
            }
        }

    }
}
