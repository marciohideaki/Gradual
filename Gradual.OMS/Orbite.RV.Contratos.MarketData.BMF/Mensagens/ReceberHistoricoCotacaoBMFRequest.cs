using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;

using Orbite.Comum;
using Orbite.RV.Contratos.MarketData.BMF.Dados;

namespace Orbite.RV.Contratos.MarketData.BMF.Mensagens
{
    /// <summary>
    /// Mensagem de solicitação de histórico de cotações BMF
    /// </summary>
    public class ReceberHistoricoCotacaoBMFRequest : MensagemRequestBase
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
        public InstrumentoBMFInfo Instrumento { get; set; }

        /// <summary>
        /// Indica que se não for encontrado nenhuma cotação para o período informado,
        /// retorna a cotação encontrada imediatamente anterior à data inicial
        /// </summary>
        public bool SeNaoEncontradoTrazerUltimo { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public ReceberHistoricoCotacaoBMFRequest()
        {
            this.Periodo = PeriodoEnum.Natural;
        }
   }
}
