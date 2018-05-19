using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Servicos.BancoDeDados.Negocio
{
    public static class Validacao
        {


            /// <summary>
            /// Validação de CPF retornando Exception para inválido
            /// </summary>
            /// <param name="pCpfCnpj">Cpf ou CNPJ a ser validado. Clientes do Sinacor já estão tratados ex. 99000 + código e são válidos</param>
            /// <param name="pMensagemErro">Mensagem que retorna na Exception em caso de Inválido</param>
            public static void ValidaCpfCnpj(string pCpfCnpj, string pMensagemErro)
            {
                if (!ValidaCpfCnpj(pCpfCnpj))
                {
                    throw new Exception(pMensagemErro);
                }

            }


            /// <summary>
            /// Validação de CPF retornando False para Inválido
            /// </summary>
            /// <param name="pCpfCnpj">Cpf ou CNPJ a ser validado. Clientes do Sinacor já estão tratados ex. 99000 + código e são válidos</param>
            /// <returns>não retorna exception, Retorna False para inválido</returns>
            public static bool ValidaCpfCnpj(string pCpfCnpj)
            {
                if (CpfSinacor(pCpfCnpj)) return true;

                bool retorno = false;
                retorno = VerificaCPF(pCpfCnpj);
                if (retorno)
                    return true;
                else
                {
                    retorno = VerificaCNPJ(pCpfCnpj);
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

            private static string RetiraString(string Value)
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

            private static bool CpfSinacor(string cpf_cnpj)
            {
                if (cpf_cnpj.Length > 2)

                    if (cpf_cnpj.Substring(0, 5) == "99000" || cpf_cnpj.Substring(0, 4) == "88000" || cpf_cnpj.Substring(0, 5) == "44400")
                        return true;
                    else
                        return false;
                else
                    return false;
            }

    }
}
