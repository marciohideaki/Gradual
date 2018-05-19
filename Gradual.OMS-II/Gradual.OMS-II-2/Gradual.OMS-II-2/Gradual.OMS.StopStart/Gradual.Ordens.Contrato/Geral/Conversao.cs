using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Gradual.OMS.Ordens.StartStop.Geral
{
    public class Conversao
    {
        #region Conversão Numérica

        public static int? ToInt(object valor)
        {
            int retorno = default(int);

            if (valor != null && int.TryParse(valor.ToString(), out retorno))
                return retorno;
            else
                return null;
        }

        public static string ToCurrency(object valor)
        {
            if ((Conversao.ToString(valor) == string.Empty) || (Conversao.ToString(valor) == "0"))
            {
                return "0,00";
            }
            else
            {
                string _decimal = valor.ToString().Replace(".", ",");

                string inteiro = _decimal.Split(',')[0];

                int ix = default(int);

                while (ix + 3 < inteiro.Length)
                {
                    ix += 3;
                    inteiro = inteiro.Insert(inteiro.Length - ix, ".");
                    ix++;
                }

                if (!_decimal.Contains(","))
                    return string.Concat(inteiro, ",00");
                else
                    return string.Format("{0},{1}", inteiro, _decimal.Split(',')[1].PadLeft(1, '0'));
            }
        }

        /// <summary>
        /// Converte o valor informado para Int32.
        /// </summary>
        /// <param name="Valor">Valor a ser convertido.</param>
        /// <returns>Int32 do valor informado caso seja um inteiro válido. Do contrário retorna 0.</returns>
        public static int ToInt32(object valor)
        {
            int retorno = default(int);

            if (valor != null && int.TryParse(valor.ToString(), out retorno))
                return retorno;

            return retorno;
        }

        public static double ToInt64(object valor)
        {
            double retorno = default(double);

            if (null != valor && double.TryParse(valor.ToString(), out retorno))
                return retorno;

            return retorno;
        }

        public static decimal? ToDecimal(object valor)
        {
            decimal retorno = default(decimal);

            if (valor != null && decimal.TryParse(valor.ToString(), out retorno))
                return retorno;
            else
                return null;
        }

        public static string ToDecimalOracle(decimal? valor)
        {
            if (valor != null)
                return valor.ToString().Replace(',', '.');
            else
                return "0.00";
        }

        /// <summary>
        /// Conversão com validação o valor informado para Int32.
        /// </summary>
        /// <param name="Valor">Valor a ser convertido.</param>
        /// <param name="textoExcecao">Texto a ser exibido caso não seja possível a conversão.</param>
        /// <returns>Int32 do valor informado caso seja um inteiro válido. Do contrário retorna uma exceção.</returns>     
        public static int ToInt32(object valor, string textoExcecao)
        {
            int retorno = default(int);

            if (valor != null && int.TryParse(valor.ToString(), out retorno))
                return retorno;
            else
                throw new Exception(textoExcecao);
        }

        #endregion

        #region Conversão para string

        public static char? ToChar(object valor)
        {
            if (valor != null && valor.ToString().Length != 0)
                return Convert.ToChar(valor);
            else
                return null;
        }

        public static string ToString(object valor)
        {
            if (valor != null && valor.ToString().Length != 0)
                return valor.ToString();
            else
                return string.Empty;
        }

        public static string ToString(DateTime? valor)
        {
            if (valor != null)
            {
                return valor.Value.ToShortDateString();
            }
            else
                return string.Empty;
        }

        #endregion

        #region Conversão para data

        public static DateTime ToDate(string valor, string nomeCampo)
        {
            DateTime retorno;

            if (DateTime.TryParse(valor, out retorno))
                return retorno;
            else throw new Exception(string.Format("Informe uma data válida para o campo {0}. Use o padrão {1}", nomeCampo, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
        }

        public static DateTime? ToDateTime(object valor)
        {
            DateTime retorno;
            if (valor != null && !Convert.IsDBNull(valor) && DateTime.TryParse(valor.ToString(), out retorno))
                return retorno;
            else
                return null;
        }

        public static string ToDateOracle(DateTime? valor)
        {
            if (valor != null)
                return "TO_DATE('" + ((DateTime)valor).ToString("dd-MM-yyyy") + "','dd-MM-yyyy')";
            else
                return "NULL";
        }

        public static string ToDateTimeOracle(DateTime? valor)
        {
            if (valor != null)
                return "TO_DATE('" + ((DateTime)valor).ToString("dd-MM-yyyy hh:mm:ss") + "','DD/MM/YYYY HH24:MI:SS')";
            else
                return "NULL";
        }

        #endregion

        public static Boolean? ToBoolean(object valor)
        {
            if (valor != null && valor.ToString().Length != 0)
            {
                if (valor.ToString().ToUpper() == "S" || valor.ToString().ToUpper() == "1")
                    return true;
                else
                    return false;
            }
            else
                return null;
        }

        public static Boolean? CharToBool(object valor)
        {
            bool retorno = false;

            if (valor != null)
            {
                char Char;
                char.TryParse(valor.ToString(), out Char);
                retorno = Char == '1';
            }

            return retorno;
        }
    }
}
