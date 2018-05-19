using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Gradual.OMS.ConectorSTM
{
    public class STMUtilities
    {
        public static CultureInfo ciPtBR = CultureInfo.CreateSpecificCulture("pt-Br");
        public static CultureInfo ciEn = CultureInfo.CreateSpecificCulture("en-us");

        public static string saidaFormatada(char formato, String conteudo,
            bool formatoDinheiro,
			bool sinal, 
			bool zerosAEsquerda, 
			int tamanho)
        {
		
            int fracaoDeDigitos = 0;
            char sinalNegativo = ' ';
		
	        if (formato >= '0' && formato <= '9')
	        {
		        fracaoDeDigitos = formato - '0';
		        sinalNegativo = ' ';
	        }
	        else if (formato >= 'A' && formato <= 'J')
	        {
		        fracaoDeDigitos = formato - 'A';
		        sinalNegativo = '-';
	        }

	        // adiciona parte inteira
	        int tamanhoConteudo = conteudo.Length;
	        int primeiraPosicaoDaFracao = tamanhoConteudo - fracaoDeDigitos;
	        int tamanhoDaParteInteira = primeiraPosicaoDaFracao;
            StringBuilder saida = new StringBuilder();

	        int posicao = 0;
	        while (posicao < tamanhoDaParteInteira)
	        {
		        if (conteudo[posicao] == '0')
			        posicao++;
		        else
			        break;
	        }

	        if (posicao == tamanhoDaParteInteira)
	        {
		        saida.Append("0,");
	        }
	        else
	        {
		        saida.Append(conteudo.Substring(posicao, primeiraPosicaoDaFracao-posicao));
		        saida.Append(',');
	        }

	        // adiciona parte fracionaria
	        saida.Append(conteudo.Substring(primeiraPosicaoDaFracao));

	        if ( formatoDinheiro )
	        {
		        Double valor = Convert.ToDouble( saida.ToString().Replace(',', '.') );
		        String valorArredondado = String.Format("%.2f", valor);
		        saida.Clear();
		        saida.Append( valorArredondado.Replace('.', ',') );
	        }

            StringBuilder preencheZeros = new StringBuilder();
	        if ( zerosAEsquerda )
	        {
		        for ( int i = tamanho; i > ( sinal ? saida.Length+1 : saida.Length ); i-- )
			        preencheZeros.Append('0');
	        }

	        if ( sinal )
		        return (sinalNegativo + preencheZeros.ToString() + saida.ToString()).Trim();

	        return preencheZeros.ToString() + saida.ToString();
        }

        public static Decimal formataPreco( string formato, string preco )
        {
		    Decimal fracaoDeDigitos = 1;
		    Decimal sinal = 1;
            char cformato = formato.ToCharArray()[0];

            Decimal valor = Convert.ToDecimal(preco);

			if (cformato >= '0' && cformato <= '9')
			{
				fracaoDeDigitos = 10^(cformato - '0');
				sinal = 1;
			}
			else if (cformato >= 'A' && cformato <= 'J')
			{
				fracaoDeDigitos = 10^(cformato - 'A');
                sinal = -1;
			}

            valor = valor / fracaoDeDigitos * sinal;

            return valor;
	    }

        public static string RetornarNumeros(string toNormalize)
        {
            List<char> numbers = new List<char>("0123456789");
            StringBuilder toReturn = new StringBuilder(toNormalize.Length);
            CharEnumerator enumerator = toNormalize.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (numbers.Contains(enumerator.Current))
                    toReturn.Append(enumerator.Current);
            }

            return toReturn.ToString();
        }
    }
}
