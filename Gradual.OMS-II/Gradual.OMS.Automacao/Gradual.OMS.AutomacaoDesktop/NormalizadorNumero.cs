using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.AutomacaoDesktop
{
    public class NormalizadorNumero
    {

        public string normaliza(char formato, string valor)
	    {
		    int numDigitosFracionarios = 
			    (formato >= '0' && formato <='9') ?
				    formato - '0' :
				    0;
		
		    int posFimInteiraValor = TAM_VALOR_INFORMADO - numDigitosFracionarios -  1;
		    int posInicioFracionariaValor = posFimInteiraValor + 1;
		
		    /* 
		    todo o número será uma string com 23 caracteres, com o separador decimal na 
		    posição 14 (índice 13)

		    para cada valor numérico informado pela Bovespa:

		    determina número de casas decimais, e consequentemente o início da parte 
		    decimal e o fim da parte inteira

		    do início da parte fracionária até o fim do valor, copiar; do fim do valor até o 
		    fim fixo (pos 23, índice 22), completar com zeros ("à direita")

		    do fim da parte inteira até o começo do valor, copiar; do começo do valor até o 
		    início fixo (pos 1, índice 0), completar com zeros ("à esquerda")

		    */
		
		    char[] numNormalizado = new char[TAM_NUM_NORMALIZADO];
		    numNormalizado[13] = ',';

		    for(int i = 0; i < TAM_PARTE_FRACIONARIA; i++)
			    numNormalizado[POS_INICIO_FRACIONARIA + i] = 
				    ((posInicioFracionariaValor + i) >= TAM_VALOR_INFORMADO) ?
					    '0' : valor[posInicioFracionariaValor + i];

		    for(int i = 0; i <= POS_FIM_INTEIRA; i++)
			    numNormalizado[POS_FIM_INTEIRA - i] =
				    ((posFimInteiraValor - i) >= 0) ?
					    numNormalizado[POS_FIM_INTEIRA - i] = valor[posFimInteiraValor - i] :
					    '0';
			
		    return new String(numNormalizado).ToString();
		
	    }
	
	    // Atualmente, o maior valor que a Bovespa envia utiliza 13 dígitos,
	    // e o maior número de dígitos para a parte decimal é 9. Dessa forma,
	    // o maior número normalizado terá 13 dígitos para a parte inteira e 
	    // 9 dígitos para a parte fracionária, mais o separador:
	    // 0000000000000,000000000
	    // \----- -----/|\--- ---/
	    //       |      |    |
	    //       V      V    V
	    //      13  +   1 +  9 = 23
	    public const int TAM_NUM_NORMALIZADO = 23;
	    public const int TAM_PARTE_INTEIRA = 13;
	    public const int TAM_PARTE_FRACIONARIA = 9;
	    public const int POS_SEPARADOR_FRACIONARIO = 13;
	    public const int POS_FIM_INTEIRA = 12;
	    public const int POS_INICIO_FRACIONARIA = 14;
	    public const int POS_FIM_NUM_NORMALIZADO = 22;
	
	    public const int TAM_VALOR_INFORMADO = 13;
    }
}
