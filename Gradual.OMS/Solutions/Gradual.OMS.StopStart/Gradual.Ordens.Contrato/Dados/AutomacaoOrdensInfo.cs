using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.Ordens.Contrato.Dados.Enum;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Dados
{
    public class AutomacaoOrdensInfo
    {
        /*
        /// <summary>
        /// tipo de Mercado ( Bovespa / Bmf )
        /// </summary>
        public int IdMercadoTipo { set; get; }

        /// <summary>
        /// Identificador de StopStart
        /// </summary>
        public int IdStopStart { set; get; }

        /// <summary>
        /// Status da Ordem Stop ex.Aberto, Pendente de Disparo etc
        /// ( verificar tabela tb_stopstart_status )
        /// </summary>        
        public int IdStopstartStatus { set; get; }

        /// <summary>
        /// Id da bolsa que a ordem esta sendo enviada. ( Bovespa / Bmf / CME etc...).   
        /// </summary>
        public int IdBolsa { set; get; }

        /// <summary>
        /// Tipo do gatilho ( Stop, Start, Stop Movel etc, verificar tabela (tb_stopstart_tipo)
        /// </summary>
        public StopStartTipoEnum IdStopStartTipo { set; get; }

        /// <summary>
        /// Código do Cliente ( BOVESPA )
        /// </summary>
        public int IdCliente { set; get; }

        /// <summary>
        /// Código do Instrumento
        /// </summary>
        public string Instrumento { set; get; }

        /// <summary>
        /// Data de validade do Stop/Start
        /// </summary>
        public Nullable<DateTime> DataValidade { set; get; }

        /// <summary>
        /// Data do envio da ordem.
        /// </summary>
        public Nullable<DateTime> DataOrdemEnvio { set; get; }

        /// <summary>
        /// Data do disparo da ordem.
        /// </summary>
        public Nullable<DateTime> DataDisparoOrdem { set; get; }

        /// <summary>
        /// Data de execucao da ordem.
        /// </summary>
        public Nullable<DateTime> DataExecucao { set; get; }

        /// <summary>
        /// Data de execucao da ordem.
        /// </summary>
        public int Quantidade { set; get; }

        /// <summary>
        /// Preco Gain do Stop
        /// </summary>
        public Nullable<decimal> PrecoGain { set; get; }

        /// <summary>
        /// Preco de envio do start da ordem
        /// </summary>
        public Nullable<decimal> PrecoEnvioStart { set; get; }

        /// <summary>
        /// Preco de envio do gain
        /// </summary>
        public Nullable<decimal> PrecoEnvioGain { set; get; }

        /// <summary>
        /// Preco loss do stop
        /// </summary>
        public Nullable<decimal> PrecoLoss { set; get; }

        /// <summary>
        /// Preco de envio do loss
        /// </summary>
        public Nullable<decimal> PrecoEnvioLoss { set; get; }

        /// <summary>
        /// Preco do start
        /// </summary>
        public Nullable<decimal> PrecoStart { set; get; }

        /// <summary>
        /// Verifica se o Stop/Start esta ativo ou inativo.
        /// </summary>
        public Nullable<char> Ativo { set; get; }

        /// <summary>
        /// Valor de Inicio do stop movel
        /// </summary>
        public Nullable<decimal> ValorInicioMovel { set; get; }

        /// <summary>
        /// Valor de ajuste do movel
        /// </summary>
        public Nullable<decimal> ValorAjusteMovel { set; get; }
        
         */
        /*
         * Novas propriedades
         */

        /// <summary>
        /// StopStartID - Identificador de StopStart
        /// </summary>
        public int StopStartID { get; set; }

        /// <summary>
        /// OrdTypeID - Id do tipo da ordem
        /// </summary>
        public char OrdTypeID { get; set; }

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
        public int IdBolsa { set; get; }

        /// <summary>
        /// Tipo do gatilho ( Stop, Start, Stop Movel etc, verificar tabela (tb_stopstart_tipo)
        /// </summary>
        public StopStartTipoEnum IdStopStartTipo { set; get; }
    }
}
