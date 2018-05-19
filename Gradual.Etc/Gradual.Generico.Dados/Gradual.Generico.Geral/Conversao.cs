using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Generico.Geral
{
    public class Conversao
    {
        public static char? ToChar(object valor)
        {
            if (valor != null && valor.ToString().Length != 0)
                return Convert.ToChar(valor);
            else
                return null;
        }

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

        public static decimal? ToDecimal(object valor)
        {
            if (valor != null && valor.ToString().Length != 0)
                return Convert.ToDecimal(valor);
            else
                return null;
        }

        public static string ToString(object valor)
        {
            if (valor != null && valor.ToString().Length != 0)
                return valor.ToString();
            else
                return "";
        }

        public static DateTime? ToDateTime(object valor)
        {
            if (valor != null && valor.ToString().Length != 0)
                return Convert.ToDateTime(valor);
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

        public static int? ToInt(object valor)
        {
            int number = default(int);

            if (int.TryParse(valor.ToString(), out number))
                return number;
            else
                return null;
        }

        public static int ToInt32(object valor)
        {
            int number = default(int);

            if (int.TryParse(valor.ToString(), out number))
                return number;
            else
                return int.MinValue;
        }

        public static string ToDecimalOracle(decimal? valor)
        {
            if (valor != null)
                return valor.ToString().Replace(',', '.');
            else
                return "0.00";
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
    }
}
