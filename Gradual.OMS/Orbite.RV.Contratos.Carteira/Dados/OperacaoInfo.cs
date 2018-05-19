using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.Carteira.Dados
{
	/// <summary>
	/// Contém informações base de uma operação.
	/// O intuito desta classe é comportar qualquer tipo de operação, por isso ela
	/// contém apenas o essencial de qualquer operação.
	/// </summary>
    public class OperacaoInfo
    {
		/// <summary>
		/// Código da operação.
		/// </summary>
        public string CodigoOperacao { get; set; }

		/// <summary>
		/// Indica a data em que o registro entrou no sistema
		/// </summary>
        public DateTime DataBoletagem { get; set; }

		/// <summary>
		/// Indica a data a que se refere a operação.
		/// </summary>
        public DateTime DataReferencia { get; set; }
    }
}
