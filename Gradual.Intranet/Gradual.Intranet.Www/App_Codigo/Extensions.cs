using System;
using System.Globalization;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using System.Text;

namespace Gradual.Intranet.Www.App_Codigo
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

        public static Nullable<Int32> DBToInt32(this object pObject, eIntNull eInt)
        {
            Nullable<int> lReturn = new Nullable<int>();

            if (pObject == null || pObject == DBNull.Value || "null".Equals(pObject))
            {
                lReturn = eInt.Equals(eIntNull.Permite) ? new Nullable<int>() : 0;
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

        public static bool DBToBoolean(this object pObject)
        {
            if ("S".Equals(DBToString(pObject).ToUpper()))
                return true;
            else if ("N".Equals(DBToString(pObject).ToUpper()))
                return false;

            var lRetorno = default(bool);

            bool.TryParse(DBToString(pObject), out lRetorno);

            return lRetorno;
        }

        public static DateTime DBToDateTime(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return DateTime.MinValue;

            var lReturn = DateTime.MinValue;

            if (DateTime.TryParse(pObject.ToString(), System.Globalization.CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out lReturn))
            {
                return lReturn;
            }
            else
            {
                return DateTime.MinValue;
            }
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

            var lReturn = DateTime.MinValue;

            if (DateTime.TryParse(DBToString(pObject), System.Globalization.CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out lReturn))
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

        public static string ToStringFormatoNome(this object pObject)
        {
            if (null == pObject)
                return string.Empty;

            var lRetorno = new StringBuilder(DBToString(pObject).Trim().ToLower());

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

        public static string ToCpfString(this double pNumero)
        {
            string lRetorno = pNumero.ToString().PadLeft(11, '0');

            lRetorno = lRetorno.Insert(9, "-").Insert(6, "-").Insert(3, ".");

            return lRetorno;
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
            string lRetorno;

            if (string.IsNullOrWhiteSpace(pNumero))
                return string.Empty;

            pNumero = pNumero.ToCpfCnpjSemPontuacao();

            if (pNumero.Length <= 11)
            {
                //tratando como CPF
                // XXXYYYZZZWW
                // XXX.YYY.ZZZ-WW
                lRetorno = pNumero.PadLeft(11, '0').Insert(9, "-").Insert(6, ".").Insert(3, ".");
            }
            else
            {
                //tratando como CNPJ
                // XXYYYZZZWWWWQQ
                // XX.YYY.ZZZ.WWWW-QQ
                lRetorno = pNumero.PadLeft(14, '0').Insert(12, "-").Insert(8, "/").Insert(5, ".").Insert(2, ".");
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

        /// <summary>
        /// Insere o caractere '-' no retorno.
        /// </summary>
        public static string ToTelefoneString(this object pObject)
        {
            if (pObject != null)
            {
                var lTelefone = pObject.DBToString();

                if (lTelefone.Length > 9) //--> Colocando o DDD entre parênteres (caso haja DD) e traço no telefone.
                    lTelefone = lTelefone.Insert(lTelefone.Length - 4, "-").Insert(0, "(").Insert(3, ")");

                else if (lTelefone.Length > 7) //--> Colocando traço no telefone.
                    lTelefone = lTelefone.Insert(lTelefone.Length - 4, "-");

                return lTelefone;
            }

            return string.Empty;
        }

        public static decimal ToValorDecimalConvertido(this string pString)
        {
            if (string.IsNullOrEmpty(pString)) return 0;

            return decimal.Parse(pString.Replace(".", "").Replace(",", "."));
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

            return string.Format("{0}-{1}", pObject.ToString().PadLeft(5, '0'), lDigito);
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


namespace Gradual.Intranet.Www
{
    public static class Extensions
    {
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

            return string.Format("{0}-{1}", pObject.ToString().PadLeft(5, '0'), lDigito);
        }
    }
}
