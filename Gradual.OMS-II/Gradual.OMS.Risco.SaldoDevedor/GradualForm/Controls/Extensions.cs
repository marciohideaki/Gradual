using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GradualForm.Controls
{
    public static class Extensions
    {
        private static System.Globalization.CultureInfo gCultura
        {
            get { return new System.Globalization.CultureInfo("pt-BR"); }
        }

        public static Int32 ToInt32(this object pObject)
        {
            int lReturn;

            if (pObject == DBNull.Value) return lReturn = 0;

            if (pObject == null) return lReturn = 0;

            if (!int.TryParse(pObject.ToString(), System.Globalization.NumberStyles.Any, gCultura, out lReturn)) lReturn = 0;

            return lReturn;
        }

        public static Int64 ToInt64(this object pObject)
        {
            Int64 lReturn;

            if (pObject == DBNull.Value) lReturn = 0;

            if (pObject == null) lReturn = 0;

            if (!Int64.TryParse(pObject.ToString(), System.Globalization.NumberStyles.Any, gCultura, out lReturn)) lReturn = 0;

            return lReturn;
        }

        public static decimal ToDecimal(this object pObject)
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

        public static decimal ToDecimalEnglishToPortuguese(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return 0;

            decimal lReturn;

            if (pObject.GetType() == typeof(decimal))
            {
                return (decimal)pObject;
            }

            if (Decimal.TryParse(pObject.ToString(), System.Globalization.NumberStyles.Any, new System.Globalization.CultureInfo("en-US"), out lReturn))
            {
                return Convert.ToDecimal(lReturn, gCultura);
            }
            else
            {
                return 0;
            }
        }

        public static string ToCurrencyEnglishToPortuguese(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return "0";

            decimal lReturn;

            //if (pObject.GetType() == typeof(decimal))
            //{
            //    return (decimal)pObject;
            //}

            if (Decimal.TryParse(pObject.ToString(), System.Globalization.NumberStyles.Any, new System.Globalization.CultureInfo("pt-BR"), out lReturn))
            {
                return lReturn.ToString("C2", gCultura);
            }
            else
            {
                return "0";
            }
        }

        public static ushort ToUInt16(this object pObject)
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

        public static DateTime ToDateTime(this object pObject)
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

        public static string ToString(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return "";

            return pObject.ToString();
        }

        public static double ToDouble(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return 0;

            double lReturn;

            if (pObject.GetType() == typeof(decimal))
            {
                return (double)pObject;
            }

            //if (Double.TryParse(pObject.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out lReturn))
            if (Double.TryParse(pObject.ToString(), System.Globalization.NumberStyles.Any, gCultura, out lReturn))
            {
                return lReturn;
            }
            else
            {
                return 0;
            }
        }

        public static bool IsNumeric(this string strToCheck)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strToCheck, @"\d+(\,\d{1,2})?");//"^\\d+(\\,\\d+)?$"
        }


        public static string ToNumeroAbreviado(this int pObject)
        {
            return Convert.ToDouble(pObject).ToNumeroAbreviado();
        }

        // Sobrecarga do método ToNumeroAbreviado para permitir números maiores que 2Bi oriundos da conversão de Int64
        public static string ToNumeroAbreviado(this long pObject)
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

        public static string ToNumeroAbreviado(this decimal pObject)
        {
            decimal lDecPlaces = Convert.ToDecimal(Math.Pow(10.0, 2));
            decimal lNumero = pObject;

            if (lNumero < 0)
                lNumero = lNumero * (-1);

            char[] lAbreviacoes = "KMBT".ToCharArray();

            string lFormato = string.Empty;

            for (int i = lAbreviacoes.Length - 1; i >= 0; i--)
            {
                var lSize = Convert.ToDecimal(Math.Pow(10, (i + 1) * 3));

                if (lSize <= lNumero)
                {
                    lNumero = Math.Round(pObject * lDecPlaces / lSize) / lDecPlaces;

                    lFormato = lAbreviacoes[i].ToString();

                    break;
                }
            }

            return lNumero.ToString(new CultureInfo("pt-BR")) + lFormato;
        }

        public static Int64 ToNumeroNormal(this string pObject)
        {
            char[] lAbreviacoes = "KMBT".ToCharArray();

            double lnumero = 0;
            double.TryParse(pObject, out lnumero);

            for (int i = lAbreviacoes.Length - 1; i >= 0; i--)
            {
                if (pObject.IndexOf(lAbreviacoes[i]) > 0)
                {
                    var lSize = Math.Pow(10, (i + 1) * 3);
                    lnumero = double.Parse(pObject.Substring(0, pObject.IndexOf(lAbreviacoes[i])));
                    lnumero = lnumero * lSize;
                }
            }
            return Convert.ToInt64(lnumero);
        }

        public static decimal ToNumeroDecimal(this string pObject)
        {
            char[] lAbreviacoes = "KMBT".ToCharArray();

            double lnumero = 0;
            double.TryParse(pObject, out lnumero);

            for (int i = lAbreviacoes.Length - 1; i >= 0; i--)
            {
                if (pObject.IndexOf(lAbreviacoes[i]) > 0)
                {
                    var lSize = Math.Pow(10, (i + 1) * 3);
                    lnumero = double.Parse(pObject.Substring(0, pObject.IndexOf(lAbreviacoes[i])));
                    lnumero = lnumero * lSize;
                }
            }
            return (decimal)lnumero;
        }

        public static int ToNumeroNormalInt(this string pObject)
        {
            char[] lAbreviacoes = "KMBT".ToCharArray();

            double lnumero = 0;
            double.TryParse(pObject, out lnumero);

            for (int i = lAbreviacoes.Length - 1; i >= 0; i--)
            {
                if (pObject.IndexOf(lAbreviacoes[i]) > 0)
                {
                    var lSize = Math.Pow(10, (i + 1) * 3);
                    lnumero = double.Parse(pObject.Substring(0, pObject.IndexOf(lAbreviacoes[i])));
                    lnumero = lnumero * lSize;
                }
            }
            return Convert.ToInt32(lnumero);
        }

        /// <summary>
        /// Gera o digito do cliente nas atividades Bovespa e BMF e concatena com a conta informada
        /// </summary>
        /// <param name="LCodigoCorretora">Código da Corretora</param>
        /// <param name="CodigoCliente">Código do cliente na corretora ( BOVESPA/BMF)</param>
        /// <returns></returns>
        public static string ToCodigoClienteFormatado(this object pObject)
        {
            int lDigito = 0;

            int lCodigoCorretora = 227;

            lDigito = (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(1 - 1, 1)) * 5)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(2 - 1, 1)) * 4)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(1 - 1, 1)) * 3)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(2 - 1, 1)) * 2)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(3 - 1, 1)) * 9)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(4 - 1, 1)) * 8)
                    + (int.Parse(lCodigoCorretora.ToString().PadLeft(5, '0').Substring(5 - 1, 1)) * 7)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(3 - 1, 1)) * 6)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(4 - 1, 1)) * 5)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(5 - 1, 1)) * 4)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(6 - 1, 1)) * 3)
                    + (int.Parse(pObject.ToString().PadLeft(7, '0').Substring(7 - 1, 1)) * 2);

            lDigito = lDigito % 11;

            if (lDigito == 0 || lDigito == 1)
            {
                lDigito = 0;
            }

            else
            {
                lDigito = 11 - lDigito;
            }

            return string.Format("{0}-{1}", pObject.ToString(), lDigito);
        }

        public static string ToStringFormatoNome(this object pObject)
        {
            if (null == pObject)
                return string.Empty;

            var lRetorno = new StringBuilder(ToString(pObject).Trim().ToLower());

            try
            {
                var lArrayTexto = lRetorno.ToString().Split(' ');

                lRetorno.Clear();

                for (int i = 0; i < lArrayTexto.Length; i++)
                    lRetorno.AppendFormat("{0} ", lArrayTexto[i].Insert(0, lArrayTexto[i].Substring(0, 1).ToUpper()).Remove(1, 1));
            }
            catch (Exception ex) { var lalala = ex.ToString(); }

            return lRetorno.ToString().Trim();
        }
    }
}
