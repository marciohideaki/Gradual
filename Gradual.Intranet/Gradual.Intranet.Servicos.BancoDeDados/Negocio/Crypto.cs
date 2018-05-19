using System;
using System.Security.Cryptography;
using System.Text;

namespace Gradual.Intranet.Servicos.BancoDeDados.Negocio
{
    public static class Crypto
    {
        public static string CalculateMD5Hash(string input)
        {
            try
            {
                //Primeiro passo, validar o input
                if (string.IsNullOrEmpty(input))
                    input = "gradual123*";

                // Segundo passo, calcular o MD5 hash a partir da string
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

                byte[] hash = md5.ComputeHash(inputBytes);

                // Terceiro passo, converter o array de bytes em uma string haxadecimal
                StringBuilder _HashBuilder = new StringBuilder();

                for (int intX = 0; intX < hash.Length; intX++)
                {
                    _HashBuilder.Append(hash[intX].ToString("X2"));
                }
                return _HashBuilder.ToString();

            }

            catch
            {
                throw new Exception("Ocorreu um erro ao Criptografar a senha.");

            }
        }
    }
}
