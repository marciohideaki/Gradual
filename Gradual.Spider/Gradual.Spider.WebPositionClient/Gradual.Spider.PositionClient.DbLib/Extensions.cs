using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.Spider.PositionClient.DbLib
{
    /// <summary>
    /// Classe de extensão para converts de tipos de classes
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converte para Int32
        /// </summary>
        /// <param name="pObject">Objeto a ser convertido</param>
        /// <returns>Retorna um Int32</returns>
        public static Int32 DBToInt32(this object pObject)
        {
            int lReturn;

            if (pObject == DBNull.Value) lReturn = 0;

            if (pObject == null) lReturn = 0;

            if (!int.TryParse(pObject.ToString(), out lReturn)) lReturn = 0;

            return lReturn;
        }

        /// <summary>
        /// Converte para Int64
        /// </summary>
        /// <param name="pObject">Objeto a ser convertido</param>
        /// <returns>Retorna um Int64</returns>
        public static Int64 DBToInt64(this object pObject)
        {
            Int64 lReturn;

            if (pObject == DBNull.Value) lReturn = 0;

            if (pObject == null) lReturn = 0;

            if (!Int64.TryParse(pObject.ToString(), out lReturn)) lReturn = 0;

            return lReturn;
        }

        /// <summary>
        /// Converte para Decimal
        /// </summary>
        /// <param name="pObject">Objeto a ser convertido</param>
        /// <returns>Retorna um Decimal</returns>
        public static decimal DBToDecimal(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return 0;

            decimal lReturn;

            if (pObject.GetType() == typeof(decimal))
            {
                return (decimal)pObject;
            }

            string lValor = pObject.DBToString();

            CultureInfo lInfo = new CultureInfo("pt-BR");

            if (lValor.Length > 3 && lValor[lValor.Length - 3] == '.')
                lInfo = new CultureInfo("en-US");

            if (Decimal.TryParse(lValor.PadLeft(4, '0'), NumberStyles.Any, lInfo, out lReturn))
            {
                return lReturn;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Converte para UInt16
        /// </summary>
        /// <param name="pObject">Objeto a ser convertido</param>
        /// <returns>Retorna um UInt16</returns>
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

        /// <summary>
        /// Converte para Datetime
        /// </summary>
        /// <param name="pObject">Objeto a ser convertido</param>
        /// <returns>Retorna um Datetime</returns>
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

        /// <summary>
        /// Convert um número Int64 para o formato de cpf ou cnpj
        /// </summary>
        /// <param name="pNumero">Um Número Int64</param>
        /// <returns>Retorna uma string com formato de cpf ou cnpj</returns>
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

        /// <summary>
        /// Convert uma string para o formato de cpf ou cnpj
        /// </summary>
        /// <param name="pNumero">Um número em stirng a ser formatado </param>
        /// <returns>Retorna uma string formatada em cpf ou cnpj</returns>
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

        /// <summary>
        /// Método que recebe um objeto com pontuação e formato de cpf ou cnpj e retira todas as pontuações
        /// </summary>
        /// <param name="pObject">Objeto a ser tratado</param>
        /// <returns>Retorna um Cpf ou cnpj em string sem pontuações e formatações</returns>
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
        /// Converte para Nullable<DataTime>
        /// </summary>
        /// <param name="pObject">Objeto a ser convertido</param>
        /// <param name="eDate">Enum de verificação que permite, ou não, da data ser convertida para nullable</param>
        /// <returns>Retorna um Nullable<DateTime></returns>
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

        /// <summary>
        /// Converte para uma string
        /// </summary>
        /// <param name="pObject">Um object que irá ser convertido</param>
        /// <returns>Retorna uma string</returns>
        public static string DBToString(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return "";

            return pObject.ToString();
        }

        /// <summary>
        /// Converte em um Boolean
        /// </summary>
        /// <param name="pObject">Obejto a ser convertido</param>
        /// <returns>Retorna um Bollean</returns>
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

        /// <summary>
        /// Converte em char
        /// </summary>
        /// <param name="pObject">OBjeto a ser covnertido</param>
        /// <returns>Retorna um char</returns>
        public static char DBToChar(this object pObject)
        {
            var lRetorno = default(char);

            char.TryParse(DBToString(pObject), out lRetorno);

            return lRetorno;
        }

        /// <summary>
        /// Converte para uma string com conta digito de conta bancária.
        /// </summary>
        /// <param name="pCdBanco">Código do banco</param>
        /// <param name="pCdAgencia">Código de agencia</param>
        /// <param name="pRequerimentoDigito">Digito da conta</param>
        /// <returns>Retorna uma string com o ´dígito da conta.</returns>
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
