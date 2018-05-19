using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.Carteira.Dados
{
	/// <summary>
	/// Representa a posição de um determinado instrumento em determinadas condições.
	/// Um agrupamento de resultado referente ao instrumento, data, carteira, etc.
	/// </summary>
    public class PosicaoInfo
    {
		/// <summary>
		/// Indica a carteira a que se refere esta posição.
		/// </summary>
        public string CodigoCarteira { get; set; }

		/// <summary>
		/// Indica o código da posição.
		/// </summary>
        public string CodigoPosicao { get; set; }

		/// <summary>
		/// Indica a data de referência a que se refere esta posição.
		/// </summary>
        public DateTime DataReferencia { get; set; }

		/// <summary>
		/// Indica a data referencia fim a que se refere esta posição.
		/// Juntamente com a DataReferencia, indica o período a que esta posição se refere.
		/// Como a classe PosicaoInfo não tem referência reversa ao Resultado, esta
		/// propriedade oferece uma forma de se saber o período da posição.
		/// </summary>
        public DateTime DataReferenciaFim { get; set; }

		/// <summary>
		/// Lista de operações que estão contabilizadas nesta Posição.
		/// </summary>
        public List<OperacaoInfo> Operacoes { get; set; }

		/// <summary>
		/// Faz referencia à posição pai, caso esta seja uma posição derivada.
		/// </summary>
        public PosicaoInfo PosicaoPai { get; set; }

		/// <summary>
		/// Contém a lista de posições filhas desta posição. As posições filhas são aquelas
		/// que contribuíram para os valores desta posição.
		/// </summary>
        public List<PosicaoInfo> PosicoesFilhas { get; set; }

        /// <summary>
        /// Construtor.
        /// </summary>
        public PosicaoInfo()
        {
            this.Operacoes = new List<OperacaoInfo>();
            this.PosicoesFilhas = new List<PosicaoInfo>();
        }
    }
}
