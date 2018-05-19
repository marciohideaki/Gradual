using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Gradual.Spider
{
    public static class Extensions
    {
        private static CultureInfo gCultura
        {
            get { return new CultureInfo("pt-BR"); }
        }

        public static Int32 DBToInt32(this object pObject)
        {
            int lReturn;

            if (pObject == DBNull.Value) lReturn = 0;

            if (pObject == null) lReturn = 0;

            if (!int.TryParse(pObject.ToString(), NumberStyles.Any, gCultura, out lReturn)) lReturn = 0;

            return lReturn;
        }

        public static Int64 DBToInt64(this object pObject)
        {
            Int64 lReturn;

            if (pObject == DBNull.Value) lReturn = 0;

            if (pObject == null) lReturn = 0;

            if (!Int64.TryParse(pObject.ToString(), NumberStyles.Any, gCultura, out lReturn)) lReturn = 0;

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

            if (Decimal.TryParse(pObject.ToString(), System.Globalization.NumberStyles.Any, gCultura, out lReturn))
            {
                return lReturn;
            }
            else
            {
                return 0;
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

            if (UInt16.TryParse(pObject.ToString(), System.Globalization.NumberStyles.Any, gCultura, out lReturn))
            {
                return lReturn;
            }
            else
            {
                return 0;
            }
        }

        public static DateTime DBToDateTime(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return DateTime.MinValue;

            DateTime lReturn;

            if (DateTime.TryParse(pObject.ToString(), out lReturn))
            {
                return lReturn;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static string DBToString(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return "";

            return pObject.ToString();
        }

        public static string ToNumeroAbreviado(this int pObject)
        {
            return Convert.ToDouble(pObject).ToNumeroAbreviado();
        }
        
        public static string ToNumeroAbreviado(this double pObject)
        {
            double lDecPlaces = Math.Pow(10.0, 2);
            double lNumero = pObject;

            if (lNumero < 0)
                lNumero = lNumero * (-1);

            char[] lAbreviacoes = "KMBT".ToCharArray();

            string lFormato = string.Empty;

            for (int i = lAbreviacoes.Length - 1; i >= 0; i--) 
            {
                var lSize = Math.Pow(10, (i + 1) * 3);

                if (lSize <= lNumero) 
                {
                     lNumero = Math.Round(pObject * lDecPlaces / lSize) / lDecPlaces;

                     lFormato = lAbreviacoes[i].ToString();

                     break;
                }
            }

            return lNumero.ToString(new CultureInfo("pt-BR")) + lFormato;
        }

        public static bool EntreHoras(this DateTime pData, string pHoraDe, string pHoraAte)
        {
            try
            {
                DateTime lDe  = DateTime.ParseExact(string.Format("{0:dd/MM/yyyy} {1}", pData, pHoraDe), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime lAte = DateTime.ParseExact(string.Format("{0:dd/MM/yyyy} {1}", pData, pHoraAte), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                return (lDe <= pData && pData <= lAte);
            }
            catch { return false; }
        }
    }
}
