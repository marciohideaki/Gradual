using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.Carteira.Dados
{
    	/// <summary>
	/// Representa um grupo de valores no resultado.
	/// O grupo pode representar, por exemplo, o grupo de valores da posição atual, da
	/// posição de carregamento, da posição do dia, etc.
	/// </summary>
    public class GrupoValoresInfo
    {
        #region Propriedades

        /// <summary>
        /// Indica o fator aplicado como custo de carregamento. O fator representa o 
        /// custo do periodo anterior ao periodo atual.
        /// </summary>
        public double CustoFator { get; set; }

        /// <summary>
        /// Representa o valor do custo de carregamento adicionado.
        /// </summary>
        public PrecoInfo CustoValor { get; set; }

        /// <summary>
        /// Representa o quanto foi gasto desde o início do carregamento.
        /// ** Precisa lembrar se este campo subtrai as vendas.
        /// </summary>
        public PrecoInfo Desembolso { get; set; }

        /// <summary>
        /// Posição de resultado. Quantidade x PrecoFechamento - Custos.
        /// </summary>
        public PrecoInfo Posicao { get; set; }

        /// <summary>
        /// Preço de fechamento do instrumento da posição.
        /// </summary>
        public PrecoInfo PrecoFechamento { get; set; }

        /// <summary>
        /// Preço médio negociado no instrumento no período em questão.
        /// </summary>
        public PrecoInfo PrecoMedio { get; set; }

        /// <summary>
        /// Quantidade de carregamento na posição.
        /// </summary>
        public double Quantidade { get; set; }

        /// <summary>
        /// Resultado Bruto da negociação do instrumento. Não considera os custos.
        /// </summary>
        public PrecoInfo ResultadoBruto { get; set; }

        /// <summary>
        /// Resultado Líquido da negociação do instrumento. Considera os custos.
        /// </summary>
        public PrecoInfo ResultadoLiquido { get; set; }

        /// <summary>
        /// ** Precisa relembrar o que é este campo.
        /// </summary>
        public PrecoInfo Valor { get; set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default.
        /// </summary>
        public GrupoValoresInfo()
        {
            // Inicializa os objetos de valor
            this.CustoValor = new PrecoInfo();
            this.Desembolso = new PrecoInfo();
            this.Posicao = new PrecoInfo();
            this.PrecoFechamento = new PrecoInfo();
            this.PrecoMedio = new PrecoInfo();
            this.ResultadoBruto = new PrecoInfo();
            this.ResultadoLiquido = new PrecoInfo();
            this.Valor = new PrecoInfo();
        }

        #endregion
    }
}
