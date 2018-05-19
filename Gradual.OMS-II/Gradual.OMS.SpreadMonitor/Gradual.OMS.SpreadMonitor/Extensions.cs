using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Gradual.OMS.SpreadMonitor
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

        public static Int32 DBToInt32(this object pObject)
        {
            int lReturn;

            if (pObject == DBNull.Value) lReturn = 0;

            if (pObject == null) lReturn = 0;

            if (!int.TryParse(pObject.ToString(), out lReturn)) lReturn = 0;

            return lReturn;
        }

        public static Nullable<Int32> DBToInt32Nullable(this object pObject)
        {
            Nullable<int> lReturn = new Nullable<int>();

            if (pObject == null || pObject == DBNull.Value || "null".Equals(pObject))
            {
                lReturn = null;
            }
            else
            {
                lReturn = int.Parse(pObject.ToString());
            }

            return lReturn;
        }

        public static Int64 DBToInt64(this object pObject)
        {
            return pObject.DBToInt64(false);
        }

        public static Int64 DBToInt64(this object pObject, bool pRemoverCaracteresSeparadores)
        {
            Int64 lReturn;

            if (pObject == DBNull.Value) lReturn = 0;

            if (pObject == null) lReturn = 0;

            if (pObject.GetType() == typeof(string) && pRemoverCaracteresSeparadores)
            {
                string lValor = (string)pObject;

                foreach (char lChar in ".,-/\\".ToCharArray())
                {
                    lValor = lValor.Replace(lChar.ToString(), "");
                }

                if (!Int64.TryParse(lValor, out lReturn)) lReturn = 0;
            }
            else
            {
                if (!Int64.TryParse(pObject.ToString(), out lReturn)) lReturn = 0;
            }

            return lReturn;
        }

        public static decimal DBToDecimal(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return 0;

            decimal lReturn;

            if (pObject.GetType() == typeof(decimal))
            {
                return (decimal)pObject;
            }

            if (Decimal.TryParse(pObject.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out lReturn))
            {
                return lReturn;
            }
            else
            {
                return 0;
            }
        }

        public static Nullable<double> DBToDouble(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return null;

            double lReturn;

            if (pObject.GetType() == typeof(double))
            {
                return (double)pObject;
            }

            if (double.TryParse(pObject.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out lReturn))
            {
                return lReturn;
            }
            else
            {
                return null;
            }
        }

        public static ushort DBToUInt16(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return 0;

            UInt16 lReturn;

            if (pObject.GetType() == typeof(UInt16))
            {
                return (UInt16)pObject;
            }

            if (UInt16.TryParse(pObject.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out lReturn))
            {
                return lReturn;
            }
            else
            {
                return 0;
            }
        }

        public static bool DBToBoolean(this object pObject)
        {
            var lRetorno = default(bool);

            bool.TryParse(DBToString(pObject), out lRetorno);

            return lRetorno;
        }

        public static DateTime DBToDateTime(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return DateTime.MinValue;

            var lReturn = DateTime.MinValue;

            string lFormat = "dd/MM/yyyy HH:mm:ss.fff";

            if (DateTime.TryParse(Convert.ToDateTime(pObject).ToString(lFormat), System.Globalization.CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out lReturn))
            {
                return lReturn;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static Nullable<DateTime> DBToDateTimeNullable(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value)
            {
                return null;
            }

            var lReturn = DateTime.MinValue;

            string lFormat = "dd/MM/yyyy hh:mm:sszzz";

            if (DateTime.TryParse(Convert.ToDateTime(pObject).ToString(lFormat), System.Globalization.CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out lReturn))
            {
                return lReturn;
            }
            else
            {
                return null;
            }
        }

        public static string DBToString(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return "";

            return pObject.ToString();
        }

    }
}
