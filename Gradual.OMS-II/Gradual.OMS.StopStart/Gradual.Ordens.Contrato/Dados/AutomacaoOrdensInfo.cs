using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.OMS.Ordens.StartStop.Lib.Enum;

namespace Gradual.OMS.Ordens.StartStop.Lib
{

    public class AutomacaoOrdensInfo
    {
        /// <summary>
        /// StopStartID - Identificador de StopStart
        /// </summary>
        public int StopStartID { get; set; }

        /// <summary>
        /// OrdTypeID - Id do tipo da ordem
        /// </summary>
        public int OrdTypeID { get; set; }

        /// <summary>
        /// StopStartStatusID - Id do StopStatusID
        /// </summary>
        public int StopStartStatusID { get; set; }

        /// <summary>
        /// Symbol - 
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// OrderQty - Quantidade 
        /// </summary>
        public int OrderQty { get; set; }

        /// <summary>
        /// Account - Conta do cliente
        /// </summary>
        public int Account { get; set; }

        /// <summary>
        /// RegisterTime - Data e hora do registro
        /// </summary>
        public Nullable<DateTime> RegisterTime { get; set; }

        /// <summary>
        /// ExpireDate - Data e Hora de expiração
        /// </summary>
        public Nullable<DateTime> ExpireDate { get; set; }

        /// <summary>
        /// Data de execução da Ordem
        /// </summary>
        public Nullable<DateTime> ExecutionTime { get; set; }

        /// <summary>
        /// ReferencePrice - Preço de referencia
        /// </summary>
        public Nullable<decimal> ReferencePrice { get; set; }

        /// <summary>
        /// StartPriceValue - Preço do Start
        /// </summary>
        public Nullable<decimal> StartPriceValue { get; set; }

        /// <summary>
        /// Preço de start enviado
        /// </summary>
        public Nullable<decimal> SendStartPrice { get; set; }

        /// <summary>
        /// StartGainValuePrice - Preço de gain no start 
        /// </summary>
        public Nullable<decimal> StopGainValuePrice { get; set; }

        /// <summary>
        /// SendStartGainPrice - Preço enviado de gain start 
        /// </summary>
        public Nullable<decimal> SendStopGainPrice { get; set; }

        /// <summary>
        /// LossValuePrice - Valor de Loss 
        /// </summary>
        public Nullable<decimal> StopLossValuePrice { get; set; }

        /// <summary>
        /// SendLossValuePrice - Valor de Loss Enviado
        /// </summary>
        public Nullable<decimal> SendStopLossValuePrice { get; set; }

        /// <summary>
        /// InitialMovelPrice - 
        /// </summary>
        public Nullable<decimal> InitialMovelPrice { get; set; }

        /// <summary>
        /// AdjustmentMovelPrice - 
        /// </summary>
        public Nullable<decimal> AdjustmentMovelPrice { get; set; }

        /// <summary>
        /// tipo de Mercado ( Bovespa / Bmf )
        /// </summary>
        public Nullable<int> IdBolsa { set; get; }

        /// <summary>
        /// Tipo do gatilho ( Stop, Start, Stop Movel etc, verificar tabela (tb_stopstart_tipo)
        /// </summary>
        public StopStartTipoEnum IdStopStartTipo { set; get; }

        /// <summary>
        /// Porta de origem
        /// </summary>
        public string ControlePorta{ set; get; }
    }
}
