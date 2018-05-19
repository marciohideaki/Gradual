using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;

namespace Gradual.Integracao.Itau.EdiFundos
{
    public class Utils
    {
        public static string MD5HashFile(string sPath)
        {
            StreamReader sr = new StreamReader(sPath);
            MD5CryptoServiceProvider md5h = new MD5CryptoServiceProvider();

            string sHash = "";

            sHash = BitConverter.ToString(md5h.ComputeHash(sr.BaseStream));

            sr.Close();

            return sHash.ToUpperInvariant();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barray"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] barray)
        {
            byte[] buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, barray);
            string tempString = Encoding.UTF8.GetString(buf, 0, barray.Length);

            return tempString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barray"></param>
        /// <returns></returns>
        public static Decimal ByteArrayToDecimal(byte[] barray, int decimalplaces)
        {
            byte[] buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, barray);
            string tempString = Encoding.UTF8.GetString(buf, 0, barray.Length);
            tempString = tempString.Insert(tempString.Length - decimalplaces, ".");

            return Convert.ToDecimal(tempString,CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="datearr"></param>
        /// <param name="timearr"></param>
        /// <param name="parseformat"></param>
        /// <returns></returns>
        public static DateTime ByteArrayToDateTime(byte[] datearr, byte[] timearr, string parseformat)
        {
            StringBuilder tempString = new StringBuilder();

            byte[] buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, datearr);
            tempString.Append(Encoding.UTF8.GetString(buf, 0, datearr.Length));

            buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, timearr);
            tempString.Append(Encoding.UTF8.GetString(buf, 0, timearr.Length));

            DateTime ret = DateTime.ParseExact(tempString.ToString(), parseformat, CultureInfo.InvariantCulture);

            return ret;
        }
    }
}
