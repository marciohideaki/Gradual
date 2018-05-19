using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.Carteira.Dados
{
	/// <summary>
	/// Representa uma informação de preço ou valor.
	/// Usado para carregar a indicação da moeda, juntamente com o valor.
	/// </summary>
    public class PrecoInfo
    {
        #region Propriedades 

        /// <summary>
		/// Indica a data de referência utilizado na representação da moeda. Caso tenha
		/// sido feita uma conversão, este valor indica a data mais próxima para
		/// representar o valor na nova moeda.
		/// </summary>
        public DateTime DataReferencia { get; set; }

		/// <summary>
		/// Caso tenha sido feita uma conversão, este campo mantem a data de referencia do
		/// valor original, enquanto que a data de referência pode indicar o valor mais
		/// próximo à data de referencia original encontrado para representar em outra
		/// moeda.
		/// </summary>
        public DateTime DataReferenciaOriginal { get; set; }

		/// <summary>
		/// Em casos de conversão, indica o fator de conversão.
		/// </summary>
        public double FatorConversao { get; set; }

		/// <summary>
		/// Indica a moeda em que o valor está representado.
		/// </summary>
        public string Moeda { get; set; }

		/// <summary>
		/// Em casos de conversão, indica a moeda original
		/// </summary>
        public string MoedaOriginal { get; set; }

		/// <summary>
		/// Indica o valor
		/// </summary>
        public double Valor { get; set; }

		/// <summary>
		/// Em casos de conversão, indica o valor original
		/// </summary>
        public double ValorOriginal { get; set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public PrecoInfo()
        {
            // ** Inicialização temporária - tem que vir de arquivo de configuração
            this.Moeda = "BRL";
        }

        /// <summary>
        /// Construtor. Recebe o valor para a moeda default.
        /// </summary>
        /// <param name="valor"></param>
        public PrecoInfo(double valor) : this()
        {
            this.Valor = valor;
        }

        /// <summary>
        /// Construtor. Recebe o valor e a moeda.
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="modeda"></param>
        public PrecoInfo(double valor, string modeda)
        {
            this.Valor = valor;
            this.Moeda = Moeda;
        }

        #endregion

        #region Operadores

        /// <summary>
        /// Operador de soma
        /// </summary>
        /// <param name="preco1"></param>
        /// <param name="preco2"></param>
        /// <returns></returns>
        public static PrecoInfo operator +(PrecoInfo preco1, PrecoInfo preco2)
        {
            // ** Por enquanto faz a soma simples, sem conversao de valores
            return new PrecoInfo(preco1.Valor + preco2.Valor, preco1.Moeda);
        }

        /// <summary>
        /// Operador de subtração
        /// </summary>
        /// <param name="preco1"></param>
        /// <param name="preco2"></param>
        /// <returns></returns>
        public static PrecoInfo operator -(PrecoInfo preco1, PrecoInfo preco2)
        {
            // ** Por enquanto faz a soma simples, sem conversao de valores
            return new PrecoInfo(preco1.Valor - preco2.Valor, preco1.Moeda);
        }

        /// <summary>
        /// Operador de multiplicação
        /// </summary>
        /// <param name="preco1"></param>
        /// <param name="preco2"></param>
        /// <returns></returns>
        public static PrecoInfo operator *(PrecoInfo preco1, PrecoInfo preco2)
        {
            // ** Por enquanto faz a soma simples, sem conversao de valores
            return new PrecoInfo(preco1.Valor * preco2.Valor, preco1.Moeda);
        }

        /// <summary>
        /// Operador de divisão
        /// </summary>
        /// <param name="preco1"></param>
        /// <param name="preco2"></param>
        /// <returns></returns>
        public static PrecoInfo operator /(PrecoInfo preco1, PrecoInfo preco2)
        {
            // ** Por enquanto faz a soma simples, sem conversao de valores
            return new PrecoInfo(preco1.Valor / preco2.Valor, preco1.Moeda);
        }

        #endregion
    }
}
