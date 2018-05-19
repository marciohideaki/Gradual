using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.Carteira.Dados
{
	/// <summary>
	/// O movimento é a menor entidade de uma operação, responsável pelas alterações no
	/// resultado, ou na conta, etc.
	/// Uma operação pode resultar em diversos movimentos. Por exemplo, uma operação de
	/// direto resulta em dois movimentos, um de débito em uma carteira e outro de
	/// crédito em outra carteira.
	/// </summary>
    public class MovimentoInfo
    {
		/// <summary>
		/// Chave primária do movimento.
		/// </summary>
		public string CodigoMovimento { get; set; }

		/// <summary>
		/// Indica a data em que o movimento entrou no sistema.
		/// </summary>
        public DateTime DataBoletagem { get; set; }

		/// <summary>
		/// Indica a data a que o movimento se refere.
		/// </summary>
        public DateTime DataReferencia { get; set; }
    }
}
