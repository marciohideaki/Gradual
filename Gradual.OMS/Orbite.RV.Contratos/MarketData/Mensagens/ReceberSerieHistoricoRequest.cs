using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.Comum;
using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Contratos.MarketData.Mensagens
{
    /// <summary>
    /// Mensagem para requisição de histórico de uma série
    /// </summary>
    public class ReceberSerieHistoricoRequest
    {
        /// <summary>
        /// Indica a data inicial dos elementos da série a retornar.
        /// Caso não seja informado, indica desde o início da série.
        /// </summary>
        public DateTime? DataInicial { get; set; }

        /// <summary>
        /// Indica a data final dos elementos da série a retornar.
        /// Caso não seja informado, indica desde o fim da série.
        /// </summary>
        public DateTime? DataFinal { get; set; }

        /// <summary>
        /// Indica o período de retorno desejado
        /// </summary>
        public PeriodoEnum Periodo { get; set; }

        /// <summary>
        /// Contem informações para se localizar a série desejada.
        /// </summary>
        public SerieInfo Serie { get; set; }

        /// <summary>
        /// Opcionalmente, especifica o canal do qual deseja receber a série
        /// </summary>
        public string Canal { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public ReceberSerieHistoricoRequest()
        {
            this.Periodo = PeriodoEnum.Natural;
        }
    }
}
