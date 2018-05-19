using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.Comum;

namespace Orbite.RV.Contratos.Carteira.Dados
{
	/// <summary>
	/// Representa um resultado.
	/// O resultado é referente a uma faixa de datas, com um determinado período de
	/// valorização (hora, dia, mês, personalizado). Ele contém uma lista de posições,
	/// que são os cálculos feitos de acordo com as operações realizadas e as quebras
	/// solicitadas (carteira, periodo, etc).
	/// </summary>
    public class ResultadoInfo
    {
        /// <summary>
        /// Identificador do resultado
        /// </summary>
        public string CodigoResultado { get; set; }

        /// <summary>
        /// Data inicial a que o resultado se refere.
        /// </summary>
        public DateTime DataFinal { get; set; }

        /// <summary>
        /// Data final a que o resultado se refere
        /// </summary>
        public DateTime DataInicial { get; set; }

        /// <summary>
        /// Indica o periodo das posicoes do resultado. Pode ser por hora, diário, semanal, etc.
        /// </summary>
        public PeriodoEnum Periodo { get; set; }

        /// <summary>
        /// Caso o período seja personalizado, indica o timespan do periodo
        /// </summary>
        public TimeSpan PeriodoPersonalizado { get; set; }

		/// <summary>
		/// Contem a lista de posições resultado. Como as posições podem ter posições
		/// dentro delas, esta lista é o primeiro nível das posições.
		/// </summary>
        public List<PosicaoInfo> Posicoes { get; set; }

		/// <summary>
		/// Indica o resultado que deu origem a este resultado.
		/// Quando presente, fornece o ponto de partida para a valorização das operações.
		/// </summary>
        public ResultadoInfo ResultadoBase { get; set; }

        /// <summary>
        /// Construtor.
        /// </summary>
        public ResultadoInfo()
        {
            this.Posicoes = new List<PosicaoInfo>();
        }
    }
}
