using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Dados
{
    [Serializable]
    [DataContract]
    public class AutomacaoOrdensInfo
    {
        /// <summary>
        /// Identificador de StopStart
        /// </summary>
        public string IdStopStart { set; get; }

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
        public int IdStopStartTipo { set; get; }

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
        /// Valor de Inicio do stop movel
        /// </summary>
        public Nullable<decimal> ValorInicioMovel { set; get; }

        /// <summary>
        /// Valor de ajuste do movel
        /// </summary>
        public Nullable<decimal> ValorAjusteMovel { set; get; }

        /// <summary>
        /// Prazo Validade:
        /// 1 = hoje; 
        /// 2 = 30 dias;
        /// </summary>
        public int PrazoValidade { get; set; }
    }
}
