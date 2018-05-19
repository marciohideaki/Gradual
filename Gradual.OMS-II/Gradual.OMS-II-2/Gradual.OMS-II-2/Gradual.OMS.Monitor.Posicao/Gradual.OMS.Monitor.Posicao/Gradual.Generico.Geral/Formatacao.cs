using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Generico.Geral
{
    public static class CFormatacao
    {     

        public static string FormataPrimeiraLetraMaiuscula(string txt)
        {
            if (txt.Length > 0)
                return txt.Substring(0, 1).ToUpper() + txt.Substring(1, txt.Length - 1).ToLower();
            else
                return "";
        }


        public static string TiraAcento(string pTexto)
        {
            return TiraAcento(pTexto, true);
        }

        public static string TiraAcento(object Texto, Boolean tiraPonto)
        {
            string pTexto;
            if (null == Texto)
                return "Null";
            else
                pTexto = Texto.ToString();
            if (pTexto != null)
            {
                pTexto = pTexto.Replace("á", "a");

                pTexto = pTexto.Replace("é", "e");

                pTexto = pTexto.Replace("í", "i");

                pTexto = pTexto.Replace("ó", "o");

                pTexto = pTexto.Replace("ú", "u");

                pTexto = pTexto.Replace("à", "a");

                pTexto = pTexto.Replace("è", "e");

                pTexto = pTexto.Replace("ì", "i");

                pTexto = pTexto.Replace("ò", "o");

                pTexto = pTexto.Replace("ù", "u");

                pTexto = pTexto.Replace("â", "a");

                pTexto = pTexto.Replace("ê", "e");

                pTexto = pTexto.Replace("î", "i");

                pTexto = pTexto.Replace("ô", "o");

                pTexto = pTexto.Replace("û", "u");

                pTexto = pTexto.Replace("ä", "a");

                pTexto = pTexto.Replace("ë", "e");

                pTexto = pTexto.Replace("ï", "i");

                pTexto = pTexto.Replace("ö", "o");

                pTexto = pTexto.Replace("ü", "u");

                pTexto = pTexto.Replace("ã", "a");

                pTexto = pTexto.Replace("õ", "o");

                pTexto = pTexto.Replace("ñ", "n");

                pTexto = pTexto.Replace("ç", "c");

                pTexto = pTexto.Replace("Á", "A");

                pTexto = pTexto.Replace("É", "E");

                pTexto = pTexto.Replace("Í", "I");

                pTexto = pTexto.Replace("Ó", "O");

                pTexto = pTexto.Replace("Ú", "U");

                pTexto = pTexto.Replace("À", "A");

                pTexto = pTexto.Replace("È", "E");

                pTexto = pTexto.Replace("Ì", "I");

                pTexto = pTexto.Replace("Ò", "O");

                pTexto = pTexto.Replace("Ù", "U");

                pTexto = pTexto.Replace("Â", "A");

                pTexto = pTexto.Replace("Ê", "E");

                pTexto = pTexto.Replace("Î", "I");

                pTexto = pTexto.Replace("Ô", "O");

                pTexto = pTexto.Replace("Û", "U");

                pTexto = pTexto.Replace("Ä", "A");

                pTexto = pTexto.Replace("Ë", "E");

                pTexto = pTexto.Replace("Ï", "I");

                pTexto = pTexto.Replace("Ö", "O");

                pTexto = pTexto.Replace("Ü", "U");

                pTexto = pTexto.Replace("Ã", "A");

                pTexto = pTexto.Replace("Õ", "O");

                pTexto = pTexto.Replace("Ñ", "N");

                pTexto = pTexto.Replace("Ç", "C");

                pTexto = pTexto.Replace("'", " ");

                if (tiraPonto)
                    pTexto = pTexto.Replace(".", "");

                return pTexto.ToUpper();
            }
            else
                return pTexto;
        }

        public static void IsNullOrEmpty(string Value, string Name, string Message)
        {
            if (string.IsNullOrEmpty(Message))
            {
                IsNullOrEmpty(Value, Name);
            }
            else
            {
                if (string.IsNullOrEmpty(Value))
                {
                    throw new Exception(string.Format(Message, Name));
                }
            }
        }

        public static void IsNullOrEmpty(string Value, string Name)
        {
            if (string.IsNullOrEmpty(Value))
                throw new Exception(string.Format("O campo {0} não pode estar vazio", Name));
        }

        public static void ComparaCampo(string ValueX, string ValueY, string NameX, string NameY)
        {
            if (!string.Compare(ValueX, ValueY).Equals(0))
                throw new Exception(string.Format("As informações digitadas no campo {0} não conferem com as digitadas no campo {1}", NameX, NameY));
        }

        public static void IsDecimal(string Value, string Name)
        {
            decimal retorno = default(decimal);

            if (!decimal.TryParse(Value, out retorno))
                throw new Exception(string.Format("As informações digitada no campo {0} deve ser do tipo decimal", Name));
        }

        public static void IsInteger(string Value, string Name)
        {
            int retorno = default(int);

            if (!int.TryParse(Value, out retorno))
                throw new Exception(string.Format("As informações digitada no campo {0} deve ser um tipo inteiro", Name));
        }

        public static bool IsEmail(string Value)
        {
            bool isValid = (Value != null) && Value.Contains("@") && Value.Contains(".");

            return isValid;
        }

        public static bool ValidaCPF(string cpf_cnpj)
        {
            bool retorno = false;
            retorno = VerificaCPF(cpf_cnpj);
            if (retorno)
                return true;
            else
            {
                retorno = VerificaCNPJ(cpf_cnpj);
                return retorno;
            }
        }

        private static string GeraDigitoCNPJ(string cnpj)
        {
            int pPeso = 2;
            int pSoma = 0;

            for (int i = cnpj.Length - 1; i >= 0; i--)
            {
                pSoma += pPeso * Convert.ToInt32(cnpj[i].ToString());
                pPeso++;

                if (pPeso == 10)
                    pPeso = 2;
            }

            int pNumero = (11 - (pSoma % 11));
            if (pNumero > 9)
                pNumero = 0;

            return pNumero.ToString();
        }

        private static bool VerificaCNPJ(string cnpj)
        {
            cnpj = RetiraString(cnpj);
            if (cnpj.Length < 14)
            {
                cnpj = cnpj.PadLeft(14, '0');
            }

            string aux = cnpj;

            // Guardo os dígitos para compará-lo no final
            string pDigito = aux.Substring(12, 2);
            aux = aux.Substring(0, 12);

            //Calculo do 1º digito
            aux += GeraDigitoCNPJ(aux);

            //Calculo do 2º digito
            aux += GeraDigitoCNPJ(aux);


            //Comparo os dígitos calculadoscom os guardados anteriormente

            return pDigito == aux.Substring(12, 2);
        }

        private static bool VerificaCPF(string cpf)
        {
            cpf = RetiraString(cpf);
            if (cpf.Length < 11)
            {
                cpf = cpf.PadLeft(11, '0');
            }

            int soma1 = 0;
            int soma2 = 0;
            int resto = 0;
            int digito1 = 0;
            int digito2 = 0;
            int conta = 0;



            soma1 = (int.Parse(cpf[0].ToString()) * 10) +
                (int.Parse(cpf[1].ToString()) * 9) +
                (int.Parse(cpf[2].ToString()) * 8) +
                (int.Parse(cpf[3].ToString()) * 7) +
                (int.Parse(cpf[4].ToString()) * 6) +
                (int.Parse(cpf[5].ToString()) * 5) +
                (int.Parse(cpf[6].ToString()) * 4) +
                (int.Parse(cpf[7].ToString()) * 3) +
                (int.Parse(cpf[8].ToString()) * 2);
            resto = soma1 % 11;
            digito1 = resto < 2 ? 0 : 11 - resto;

            soma2 = (int.Parse(cpf[0].ToString()) * 11) +
                (int.Parse(cpf[1].ToString()) * 10) +
                (int.Parse(cpf[2].ToString()) * 9) +
                (int.Parse(cpf[3].ToString()) * 8) +
                (int.Parse(cpf[4].ToString()) * 7) +
                (int.Parse(cpf[5].ToString()) * 6) +
                (int.Parse(cpf[6].ToString()) * 5) +
                (int.Parse(cpf[7].ToString()) * 4) +
                (int.Parse(cpf[8].ToString()) * 3) +
                (int.Parse(cpf[9].ToString()) * 2);
            resto = soma2 % 11;
            digito2 = resto < 2 ? 0 : 11 - resto;

            for (int i = 0; i < cpf.Length - 1; i++)
                if (cpf[0] == cpf[i])
                    conta++;

            if (conta >= 9) return false;
            if (int.Parse(cpf[9].ToString()) != digito1) return false;
            if (int.Parse(cpf[10].ToString()) != digito2) return false;

            return true;
        }
        /*
        /// <summary>
        /// Valida se o item selecionado está na posição -1.
        /// </summary>
        /// <param name="_DDLData">Objeto DropDownList a ser analidado.</param>
        /// <param name="NomeCampo">Nome do campo analisado.</param>
        public static void ValidaDropDownListSemSelecao(DropDownList _DDLData, string NomeCampo)
        {
            if (0.CompareTo(_DDLData.SelectedIndex).Equals(1))
                throw new Exception(string.Concat("Selecione um item para: ", NomeCampo));
        }
        */
        /*
        /// <summary>
        /// Valida se o item selecionado está na posição zero ou menor.
        /// </summary>
        /// <param name="_DDLData">Objeto DropDownList a ser analidado.</param>
        /// <param name="NomeCampo">Nome do campo analisado.</param>
        public static void ValidaDropDownListIndiceZero(DropDownList _DDLData, string NomeCampo)
        {
            if (1.CompareTo(_DDLData.SelectedIndex).Equals(1))
                throw new Exception(string.Concat("Selecione um item para: ", NomeCampo));
        }
        */
        /*
        public static string ValidaIsTexto(string Value, string campo)
        {
            if (string.IsNullOrEmpty(Value))
                throw new Exception(string.Format("Informe preencha o campo {0} ou informe um valor válido.", campo));

            else return HttpUtility.HtmlEncode(Value);
        }
        */
        public static DateTime ValidaIsData(string Value, string campo)
        {
            DateTime retorno;

            if (DateTime.TryParse(Value, out retorno))
                return retorno;
            else throw new Exception(string.Format("Informe uma data válida para o campo {0}", campo));
        }

        public static int ValidaIsInteiro(string Value, string campo)
        {
            int retorno = default(int);

            if (int.TryParse(Value, out retorno))
                return retorno;

            else throw new Exception(string.Format("Informe um número válido para o campo {0}", campo));
        }


        public static int isInteger (string Value, string campo)
        {
            int retorno = default(int);

            if (int.TryParse(Value, out retorno))
                return retorno;

            else throw new Exception(string.Format("Informe um número válido para o campo {0}", campo));
        }


        public static string RetiraString(string Value)
        {
            string Digitos = "0123456789";
            string temp = "";
            string digito = "";

            for (int i = 0; i < Value.Length; i++)
            {
                digito = Value[i].ToString();
                if (Digitos.IndexOf(digito) >= 0)
                    temp += digito;
            }
            return temp;
        }

        public static void ValidaTamanhoCampo(int Length, string Value, string Name, string Message)
        {
            if (string.IsNullOrEmpty(Message))
            {
                ValidaTamanhoCampo(Length, Value, Name);
            }
            else
            {
                if (Value.Length < Length)
                {
                    throw new Exception(string.Format(Message, Name, Length.ToString()));
                }
            }
        }

        public static void ValidaTamanhoCampo(int Length, string Value, string Name)
        {
            if (Value.Length < Length)
                throw new Exception(string.Format("O campo {0} precisa ter pelo menos {1} caracteres.", Name, Length.ToString()));
        }

        public static void isDatetime(string Value, string Name)
        {
            DateTime retorno = default(DateTime);

            if (!DateTime.TryParse(Value, out retorno))
                throw new Exception(string.Format("A data informada no campo {0} não é uma data valida", Name));
        }

        public static void ValidaEmail(string Value, string Name, string Message)
        {
            if (string.IsNullOrEmpty(Message))
            {
                ValidaEmail(Value, Name);
            }
            else
            {
                if (!(Value != null && Value.Contains("@") && Value.Contains(".")))
                {
                    throw new Exception(string.Format(Message, Name));
                }
            }
        }

        public static void ValidaEmail(string Value, string Name)
        {
            if (!(Value != null && Value.Contains("@") && Value.Contains(".")))
                throw new Exception(string.Format("O campo {0} não foi inserido corretamente. Informe um endereço de e-mail com formato válido. (ex. usuario@dominio.com)", Name));
        }
        /*
        public static void PosicionaDropDownList(DropDownList _DDLData, string Value)
        {
            _DDLData.SelectedIndex = _DDLData.Items.IndexOf(_DDLData.Items.FindByValue(Value));
        }

        public static void AdicionaItemDropDownList(DropDownList _DDLData, string Value)
        {
            _DDLData.Items.Insert(0, Value);
        }
        */
        /*
        public static void CarregaDropDownList(string itemValue, string itemText, DropDownList DropDown, BindingList<object> list)
        {
            DropDown.DataSource = list;
            DropDown.DataTextField = itemText;
            DropDown.DataValueField = itemValue;
            DropDown.DataBind();
        }*/

        public static void ValidaCampo(string Value, string Name, string Message)
        {
            if (string.IsNullOrEmpty(Message))
            {
                ValidaCampo(Value, Name);
            }
            else
            {
                if (string.IsNullOrEmpty(Value))
                {
                    throw new Exception(string.Format(Message, Name));
                }
            }
        }

        public static void ValidaCampo(string Value, string Name)
        {
            if (string.IsNullOrEmpty(Value.Trim()))
                throw new Exception(string.Format("O campo {0} não pode estar vazio", Name));
        }

        public static void ValidaData(string Value, string name)
        {
            DateTime retorno = default(DateTime);
            if (!DateTime.TryParse(Value, out retorno))
                throw new Exception(string.Format("Informe uma data válida no campo {0}. Use o formato: {1}"
                    , name.Replace(":", string.Empty), System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
        }

        public static string FormataTextoString(string entrada, int caracteresPorLinha)
        {
            System.Text.StringBuilder retorno = new System.Text.StringBuilder();

            int startIndex = caracteresPorLinha;
            int imin = 0;
            int imax = entrada.IndexOf(' ', caracteresPorLinha);
            bool limite = default(bool);

            while (true)
            {
                retorno.Append(entrada.Substring(imin, imax));
                retorno.Append(Environment.NewLine);

                if (limite) break;

                imin = ++imax;
                startIndex = startIndex + caracteresPorLinha;
                imax = entrada.IndexOf(' ', startIndex);
                if (imax > entrada.Length - imin) { imax = entrada.Length - imin; limite = true; }
            }

            return retorno.ToString();
        }

        public static string FormataTextoHTML(string entrada, int caracteresPorLinha)
        {
            System.Text.StringBuilder retorno = new System.Text.StringBuilder();


            int startIndex = caracteresPorLinha;
            int imin = 0;
            int imax = default(int);
            bool limite = default(bool);

            if (caracteresPorLinha > entrada.Length)
            {
                imax = entrada.Length;
                limite = true;
            }
            else if (entrada.IndexOf(" ", caracteresPorLinha) < 0)
            {
                imax = entrada.Length;
                limite = true;
            }
            else imax = entrada.IndexOf(" ", caracteresPorLinha);

            while (true)
            {
                retorno.Append(entrada.Substring(imin, imax));
                retorno.Append("<BR />");

                if (limite) break;

                imin = ++imax;
                startIndex = startIndex + caracteresPorLinha;
                imax = entrada.IndexOf(' ', imin);
                if (imax > entrada.Length - imin || imax < 0) { imax = entrada.Length - imin; limite = true; }
            }

            return retorno.ToString();
        }

        public static string EncodeURL(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            else return path.Replace("&", "$");
        }

        public static string DecodeURL(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            else return path.Replace("$", "&");
        }
    }
}
