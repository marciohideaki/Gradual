using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Gradual.OMS.AutomacaoOrdensTeste
{
    public static class Extensions
    {
        public static string ByteArrayToString(this byte[] barray)
        {
            if (barray != null && barray.Length > 0)
            {
                byte[] buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, barray);
                string tempString = Encoding.UTF8.GetString(buf, 0, barray.Length);

                return tempString;
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datearr"></param>
        /// <param name="timearr"></param>
        /// <param name="parseformat"></param>
        /// <returns></returns>
        public static DateTime ByteArrayToDate(this byte[] datearr, string parseformat)
        {
            StringBuilder tempString = new StringBuilder();

            byte[] buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, datearr);
            tempString.Append(Encoding.UTF8.GetString(buf, 0, datearr.Length));
            tempString.Append("000000");

            DateTime ret = DateTime.ParseExact(tempString.ToString(), parseformat, CultureInfo.InvariantCulture);

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barray"></param>
        /// <returns></returns>
        public static Decimal ByteArrayToDecimal(this byte[] barray, int decimalplaces)
        {
            byte[] buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, barray);
            string tempString = Encoding.UTF8.GetString(buf, 0, barray.Length);
            tempString = tempString.Insert(tempString.Length - decimalplaces, ".");

            return Convert.ToDecimal(tempString, CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="barray"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this string value, int decimalplaces)
        {
            string tempString = value;
            tempString = tempString.Insert(tempString.Length - decimalplaces, ".");

            return Convert.ToDecimal(tempString, CultureInfo.InvariantCulture);
        }

    }
}
