using System;

namespace Gradual.Intranet.Servicos.BancoDeDados.Negocio
{
    public class GeradorSenhaAleatoria
    {
        public static String GerarSenha()
        {
            //string caracMinusculo = "abcdefhijkmnopqrstuvxwyz";
            //string caracMaiusculo = "ABCDEFHIJKMNOPQRSTUVXWYZ";
            //string caracNumeros   = "0123456789";

            //char[] caracterMinusculo = caracMinusculo.ToCharArray();
            //char[] caracterMaiusculo = caracMaiusculo.ToCharArray();
            //char[] caracterNumeros   = caracNumeros.ToCharArray();

            //embaralhar(ref caracterMinusculo, ref caracterMaiusculo, ref caracterNumeros, 3);

            //string novaSenha = string.Empty;

            //for (int i = 0; i < 9; i++)
            //{
            //    novaSenha += caracterMinusculo[i];
            //    i++;
                
            //    novaSenha += caracterMaiusculo[i];
            //    i++;

            //    if (caracterNumeros.Length > i)
            //    {
            //        novaSenha += caracterNumeros[i];
            //    }

            //    i++;
            //}

            //return novaSenha;

            string lRetorno = String.Empty;
            Random random = new Random();

            for (int i = 0; i <= 5; i++)
            {
                lRetorno += random.Next(10);
            }

            return lRetorno;
            
        }

        private static void embaralhar(ref char[] arrayMinusculo, ref char[] arrayMaiusculo, ref char[] arrayNumeros ,int qtd)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < qtd; i++)
            {
                for (int j = 0; j <= arrayMinusculo.Length; j++)
                {
                    swap(ref arrayMinusculo[random.Next(0, arrayMinusculo.Length)], ref arrayMinusculo[random.Next(0, arrayMinusculo.Length)]);
                }

                for (int j = 0; j <= arrayMaiusculo.Length; j++)
                {
                    swap(ref arrayMaiusculo[random.Next(0, arrayMaiusculo.Length)], ref arrayMaiusculo[random.Next(0, arrayMaiusculo.Length)]);
                }

                for (int j = 0; j <= arrayNumeros.Length; j++)
                {
                    swap(ref arrayNumeros[random.Next(0, arrayNumeros.Length)], ref arrayNumeros[random.Next(0, arrayNumeros.Length)]);
                }
            }
        }

        private static void swap(ref char arg1, ref char arg2)
        {
            char temp = arg1; arg1 = arg2; arg2 = temp;
        }
    }
}
