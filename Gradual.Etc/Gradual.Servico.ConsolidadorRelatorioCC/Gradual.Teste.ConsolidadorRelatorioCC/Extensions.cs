using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Sistemas.Risco.Persistencia
{
    public static class Extensions
    {
        public static Int32 DBToInt32(this object pObject)
        {
            int lReturn;

            if (pObject == DBNull.Value) lReturn = 0;

            if (pObject == null) lReturn = 0;

            if (!int.TryParse(pObject.ToString(), out lReturn)) lReturn = 0;

            return lReturn;
        }

        public static Int64 DBToInt64(this object pObject)
        {
            Int64 lReturn;

            if (pObject == DBNull.Value) lReturn = 0;

            if (pObject == null) lReturn = 0;

            if (!Int64.TryParse(pObject.ToString(), out lReturn)) lReturn = 0;

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

        public static string ToCpfCnpjString(this Int64 pNumero)
        {
            string lRetorno = pNumero.ToString();

            if (lRetorno.Length <= 11)
            {
                //tratando como CPF
                // XXXYYYZZZWW
                // XXX.YYY.ZZZ-WW
                lRetorno = lRetorno.PadLeft(11, '0').Insert(9, "-").Insert(6, ".").Insert(3, ".");
            }
            else
            {
                //tratando como CNPJ
                // XXYYYZZZWWWWQQ
                // XX.YYY.ZZZ.WWWW-QQ
                lRetorno = lRetorno.PadLeft(14, '0').Insert(12, "-").Insert(8, "/").Insert(5, ".").Insert(2, ".");
            }

            return lRetorno;
        }

        public static string ToCpfCnpjString(this string pNumero)
        {
            string lRetorno = pNumero;

            if (lRetorno.Length <= 11)
            {
                //tratando como CPF
                // XXXYYYZZZWW
                // XXX.YYY.ZZZ-WW
                lRetorno = lRetorno.PadLeft(11, '0').Insert(9, "-").Insert(6, ".").Insert(3, ".");
            }
            else
            {
                //tratando como CNPJ
                // XXYYYZZZWWWWQQ
                // XX.YYY.ZZZ.WWWW-QQ
                lRetorno = lRetorno.PadLeft(14, '0').Insert(12, "-").Insert(8, "/").Insert(5, ".").Insert(2, ".");
            }

            return lRetorno;
        }

        public static Nullable<DateTime> DBToDateTime(this object pObject, eDateNull eDate)
        {
            if (pObject == null || pObject == DBNull.Value)
            {
                if (eDate.Equals(eDateNull.Permite))
                    return null;
                else
                    return DateTime.MinValue;
            }

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

        public static bool DBToBoolean(this object pObject)
        {
            var lRetorno = default(bool);

            bool.TryParse(DBToString(pObject), out lRetorno);

            return lRetorno;
        }

        public static char DBToChar(this object pObject)
        {
            var lRetorno = default(char);

            char.TryParse(DBToString(pObject), out lRetorno);

            return lRetorno;
        }

        public enum eDateNull
        {
            Permite,
            DataMinValue
        }
    }
}
