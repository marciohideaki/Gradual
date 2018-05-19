using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Spider.GlobalOrderTracking
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
                object[] attributes = System.Reflection.Assembly.GetCallingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyCompanyAttribute), false);
                return attributes.Length == 0 ? "" : ((System.Reflection.AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        /// <summary> 
        /// The copyright holder of the calling assembly. 
        /// </summary> 
        public static string CopyrightHolder
        {
            get
            {
                object[] attributes = System.Reflection.Assembly.GetCallingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyCopyrightAttribute), false);
                return attributes.Length == 0 ? "" : ((System.Reflection.AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary> 
        /// The description of the calling assembly. 
        /// </summary> 
        public static string Description
        {
            get
            {
                object[] attributes = System.Reflection.Assembly.GetCallingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyDescriptionAttribute), false);
                return attributes.Length == 0 ? "" : ((System.Reflection.AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <summary> 
        /// The product name of the calling assembly. 
        /// </summary> 
        public static string ProductName
        {
            get
            {
                object[] attributes = System.Reflection.Assembly.GetCallingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyProductAttribute), false);
                return attributes.Length == 0 ? "" : ((System.Reflection.AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary> 
        /// The title of the calling assembly. 
        /// </summary> 
        public static string Title
        {
            get
            {
                object[] attributes = System.Reflection.Assembly.GetCallingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    System.Reflection.AssemblyTitleAttribute titleAttribute = (System.Reflection.AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title.Length > 0)
                        return titleAttribute.Title;
                }
                return System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        /// <summary> 
        /// The version of the calling assembly. 
        /// </summary> 
        public static Version Version
        {
            get
            {
                return System.Reflection.Assembly.GetCallingAssembly().GetName().Version;
            }
        }

    }
}
