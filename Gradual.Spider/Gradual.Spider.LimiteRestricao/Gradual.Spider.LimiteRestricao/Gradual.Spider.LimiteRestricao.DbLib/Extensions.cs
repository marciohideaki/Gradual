using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Spider.LimiteRestricao.Lib.Dados;
using Gradual.Spider.LimiteRestricao.Lib;

namespace Gradual.Spider.LimiteRestricao.DbLib
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

            string lValor = pObject.DBToString();

            if (Decimal.TryParse(pObject.DBToString().PadLeft(4, '0'), out lReturn))
            {
                return lReturn;
            }
            else
            {
                return 0;
            }
        }
        
        public static double DBToDouble(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return 0;

            var lRetorno = default(double);

            double.TryParse(pObject.ToString(), out lRetorno);

            return lRetorno;
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

        public static string ToCpfCnpjSemPontuacao(this object pObject)
        {
            var lRetorno = string.Empty;

            if (null != pObject)
            {
                lRetorno = pObject.DBToString();

                lRetorno = lRetorno.Replace(".", string.Empty)
                                   .Replace("-", string.Empty)
                                   .Replace("/", string.Empty);
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
            if ("0".Equals(DBToString(pObject)))
                return false;

            if ("1".Equals(DBToString(pObject)))
                return true;

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

        public static string RequisicaoContaDigito(string pCdBanco, string pCdAgencia, bool pRequerimentoDigito)
        {
            var lRetorno = string.Empty;
            var lCdAgencia = pCdAgencia.PadLeft(7, '0');

            switch (pCdBanco)
            {
                default:
                    if (!pRequerimentoDigito)
                        lRetorno = lCdAgencia.Substring(0, 5);
                    else
                        lRetorno = lCdAgencia.Substring(5, 2);
                    break;
            }

            return lRetorno;
        }
    }
}
