using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Gradual.Site.DbLib.Dados;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace Gradual.Site.DbLib.Widgets
{
    public static class Extensions
    {
        public static Int32 DBToInt32(this object pObject)
        {
            int lReturn;

            if (pObject == DBNull.Value) lReturn = 0;

            else if (pObject == null) lReturn = 0;

            else if (!int.TryParse(pObject.ToString(), out lReturn)) lReturn = 0;

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

            if (Decimal.TryParse(pObject.ToString(), System.Globalization.NumberStyles.Any, CultureInfo.CreateSpecificCulture("pt-BR"), out lReturn))
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
            if (DBToString(pObject).ToUpper() == "S")
            {
                return true;
            }
            else if (DBToString(pObject).ToUpper() == "N")
            {
                return false;
            }

            bool lRetorno;

            bool.TryParse(DBToString(pObject), out lRetorno);

            return lRetorno;
        }

        public static char DBToChar(this object pObject)
        {
            var lRetorno = default(char);

            char.TryParse(pObject.DBToString(), out lRetorno);

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

        public static string DBToString(this object pObject)
        {
            if (pObject == null || pObject == DBNull.Value) return "";

            return pObject.ToString();
        }

        public static string ToStringFormatoNome(this object pObject)
        {
            if (pObject == null)
                return string.Empty;

            StringBuilder lRetorno = new StringBuilder(DBToString(pObject).Trim().ToLower());

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

            pNumero = double.Parse(pNumero).DBToString();

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

        public static string ToNumeroFormatado(this string pString, bool pRemoverParteDecimal)
        {
            string lRetorno = pString.ToNumeroFormatado();

            if (pRemoverParteDecimal)
            {
                lRetorno = lRetorno.Substring(0, lRetorno.IndexOf(','));
            }

            return lRetorno;
        }

        public static string ToNumeroFormatado(this string pString)
        {
            CultureInfo lInfo = new CultureInfo("pt-BR");

            if (pString.Length > 0)
            {
                try
                {
                    return string.Format("{0:n}", decimal.Parse(pString, lInfo));
                }
                catch
                {
                    return "--err--";
                }
            }

            return "";
        }

        public static object ClonarTudo(object pObjeto) 
        {
            object lRetorno = null;

            using (MemoryStream  lStream = new MemoryStream())
            {
                BinaryFormatter  lFormatter =   new BinaryFormatter();

                lFormatter.Serialize(lStream, pObjeto);

                lStream.Position = 0;

                lRetorno = lFormatter.Deserialize(lStream);
            }

            return lRetorno;
        }

        public static List<T> ParaListaTipada<T>(this List<ConteudoInfo> pLista)
        {
            List<T> lRetorno = new List<T>();

            string lJsonComCodigo;

            foreach (ConteudoInfo lInfo in pLista)
            {
                lJsonComCodigo = lInfo.ConteudoJson;

                lJsonComCodigo = lJsonComCodigo.Substring(0, lJsonComCodigo.LastIndexOf('}'));

                lJsonComCodigo += ", \"CodigoConteudo\": " + lInfo.CodigoConteudo + "}";

                lRetorno.Add(JsonConvert.DeserializeObject<T>(lJsonComCodigo));
            }

            return lRetorno;
        }

        public static List<T> ParaListaTipada<T>(this List<PaginaConteudoInfo> pLista)
        {
            List<T> lRetorno = new List<T>();

            string lJsonConteudo, lConteudoHtml, lJsonWidget;

            foreach (PaginaConteudoInfo lInfo in pLista)
            {
                lJsonConteudo = lInfo.ConteudoJson;

                lConteudoHtml = lInfo.ConteudoHTML;

                lJsonWidget = lInfo.WidgetJson;

                if (lJsonConteudo.ToLower().Contains(lInfo.ConteudoTermo.ToLower()))
                {
                    lJsonConteudo = lJsonConteudo.Substring(0, lJsonConteudo.LastIndexOf('}'));

                    lJsonConteudo += ", \"NomePagina\": \"" + lInfo.NomePagina + "\", \"DescURL\": \"" + lInfo.DescURL + "\"}";

                    lRetorno.Add(JsonConvert.DeserializeObject<T>(lJsonConteudo));
                }

                if (lJsonWidget.ToLower().Contains(lInfo.ConteudoTermo.ToLower()))
                {
                    lJsonWidget = lJsonWidget.Substring(0, lJsonWidget.LastIndexOf('}'));

                    lJsonWidget += ", \"NomePagina\": \"" + lInfo.NomePagina + "\", \"DescURL\": \"" + lInfo.DescURL + "\"}";

                    lRetorno.Add(JsonConvert.DeserializeObject<T>(lJsonWidget));
                }
            }

            return lRetorno;
        }

        private static int MenorIndiceNaColecaoComPropriedade(Newtonsoft.Json.Linq.JObject pItem, List<Newtonsoft.Json.Linq.JObject> pLista, string pPropriedade)
        {
            int lRetorno = pLista.Count;

            string lValor, lValorAtual;

            DateTime lData, lDataAtual;

            CultureInfo lInfo = new CultureInfo("pt-BR");

            lValor = pItem.Property(pPropriedade).Value.ToString();

            if (pLista.Count > 0)
            {
                for (int a = pLista.Count - 1; a >= 0; a--)
                {
                    lValorAtual = pLista[a].Property(pPropriedade).Value.ToString();

                    if (pPropriedade.ToLower().StartsWith("data"))
                    {
                        lValor = lValor.Replace("\"", "").Replace("'", "");
                        lValorAtual = lValorAtual.Replace("\"", "").Replace("'", "");

                        lData      = DateTime.ParseExact(lValor, "dd/MM/yyyy", lInfo);
                        lDataAtual = DateTime.ParseExact(lValorAtual, "dd/MM/yyyy", lInfo);

                        if (lData.CompareTo(lDataAtual) == -1)
                        {
                            //o valor atual é menor, então o índice é menor:
                            lRetorno = a;
                        }
                    }
                    else
                    {
                        if (lValor.CompareTo(lValorAtual) == -1)
                        {
                            //o valor atual é menor, então o índice é menor:
                            lRetorno = a;
                        }
                    }
                }
            }

            return lRetorno;
        }
        
        private static int MaiorIndiceNaColecaoComPropriedade(Newtonsoft.Json.Linq.JObject pItem, List<Newtonsoft.Json.Linq.JObject> pLista, string pPropriedade)
        {
            int lRetorno = pLista.Count;

            string lValor, lValorAtual;

            DateTime lData, lDataAtual;

            CultureInfo lInfo = new CultureInfo("pt-BR");

            lValor = pItem.Property(pPropriedade).Value.ToString();

            if (pLista.Count > 0)
            {
                for (int a = pLista.Count - 1; a >= 0; a--)
                {
                    lValorAtual = pLista[a].Property(pPropriedade).Value.ToString();

                    if (pPropriedade.ToLower().StartsWith("data"))
                    {
                        lValor = lValor.Replace("\"", "").Replace("'", "");
                        lValorAtual = lValorAtual.Replace("\"", "").Replace("'", "");

                        lData      = DateTime.ParseExact(lValor, "dd/MM/yyyy", lInfo);
                        lDataAtual = DateTime.ParseExact(lValorAtual, "dd/MM/yyyy", lInfo);

                        if (lData.CompareTo(lDataAtual) == 1)
                        {
                            //o valor atual é menor, então o índice é menor:
                            lRetorno = a;
                        }
                    }
                    else
                    {
                        if (lValor.CompareTo(lValorAtual) == 1)
                        {
                            //o valor atual é menor, então o índice é menor:
                            lRetorno = a;
                        }
                    }
                }
            }


            return lRetorno;
        }

        public static void OrdenarEConverterParaListaJson(ref List<ConteudoInfo> pLista, ref List<Newtonsoft.Json.Linq.JObject> pListaJson, string pOrdenacao)
        {
            pListaJson = new List<Newtonsoft.Json.Linq.JObject>();

            List<ConteudoInfo> lNovaLista = new List<ConteudoInfo>();

            Newtonsoft.Json.Linq.JObject lItem;

            string lPropriedadeDeOrdenacao = "";
            string lDirecaoDeOrdenacao = "";

            if (!string.IsNullOrEmpty(pOrdenacao))
            {
                lPropriedadeDeOrdenacao = pOrdenacao.Substring(0, pOrdenacao.IndexOf(' '));
                lDirecaoDeOrdenacao     = pOrdenacao.Substring(pOrdenacao.IndexOf(' ') + 1, 1);
            }

            foreach (ConteudoInfo lConteudo in pLista)
            {
                lItem = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(lConteudo.ConteudoJsonComPropriedadesExtras);

                if (string.IsNullOrEmpty(pOrdenacao))
                {
                    pListaJson.Add(lItem);

                    lNovaLista.Add(lConteudo);
                }
                else
                {
                    int lIndice;

                    if (lDirecaoDeOrdenacao.ToUpper() == "D")
                    {
                        lIndice = MaiorIndiceNaColecaoComPropriedade(lItem, pListaJson, lPropriedadeDeOrdenacao);
                    }
                    else
                    {
                        lIndice = MenorIndiceNaColecaoComPropriedade(lItem, pListaJson, lPropriedadeDeOrdenacao);
                    }

                    pListaJson.Insert(lIndice, lItem);

                    lNovaLista.Insert(lIndice, lConteudo);
                }
            }

            pLista = lNovaLista;
        }

        public static string ToXml<T>(this T toSerialize)
        {
            var serializer = new XmlSerializer(typeof(T));

            var sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
                serializer.Serialize(writer, toSerialize);

            return sb.ToString();
        }

        public static T DeserializeXmlString<T>(this string xml)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(xml))
                return (T)serializer.Deserialize(reader);
        }

        public static bool EDataValida(this string pStringDeData, out DateTime pData)
        {
            DateTime lData;

            if(!DateTime.TryParseExact(pStringDeData, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out lData))
            {
                if(!DateTime.TryParseExact(pStringDeData, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out lData))
                {
                    if(!DateTime.TryParseExact(pStringDeData, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out lData))
                    {
                        pData = DateTime.MinValue;


                        return false;
                    }
                }
            }

            pData = lData;

            return true;
        }
        
        public static bool EDataValida(this string pStringDeData)
        {
            DateTime lData;

            return pStringDeData.EDataValida(out lData);
        }
    }
}
