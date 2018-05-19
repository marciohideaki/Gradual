using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gradual.Ordens.Template
{
    public  class TOrdem
    {
        /// <summary>
        /// tipo de Mercado ( Bovespa / Bmf )
        /// </summary>
        public int id_mercado_tipo { set; get; }

        /// <summary>
        /// Identificador de StopStart
        /// </summary>
        public int id_stopstart { set; get; }

        /// <summary>
        /// Status da Ordem Stop ex.Aberto, Pendente de Disparo etc
        /// ( verificar tabela tb_stopstart_status )
        /// </summary>        
        public int id_stopstart_status { set; get; }

        /// <summary>
        /// Id da bolsa que a ordem esta sendo enviada. ( Bovespa / Bmf / CME etc...).   
        /// </summary>
        public int id_bolsa { set; get; }

        /// <summary>
        /// Tipo do gatilho ( Stop, Start, Stop Movel etc, verificar tabela (tb_stopstart_tipo)
        /// </summary>
        public int id_stopstart_tipo { set; get; }

        /// <summary>
        /// Código do Cliente ( BOVESPA )
        /// </summary>
        public int id_cliente { set; get; }

        /// <summary>
        /// Código do Instrumento
        /// </summary>
        public string instrumento { set; get; }

        /// <summary>
        /// Data de validade do Stop/Start
        /// </summary>
        public Nullable<DateTime> data_validade { set; get; }

        /// <summary>
        /// Data do envio da ordem.
        /// </summary>
        public Nullable<DateTime> data_ordem_envio { set; get; }

        /// <summary>
        /// Data do disparo da ordem.
        /// </summary>
        public Nullable<DateTime> data_disparo_ordem { set; get; }

        /// <summary>
        /// Data de execucao da ordem.
        /// </summary>
        public Nullable<DateTime> data_execucao { set; get; }

        /// <summary>
        /// Data de execucao da ordem.
        /// </summary>
        public int quantidade { set; get; }

        /// <summary>
        /// Preco Gain do Stop
        /// </summary>
        public Nullable<decimal> preco_gain { set; get; }


        /// <summary>
        /// Preco de envio do start da ordem
        /// </summary>
        public Nullable<decimal> preco_envio_start { set; get; }

        /// <summary>
        /// Preco de envio do gain
        /// </summary>
        public Nullable<decimal> preco_envio_gain { set; get; }

        /// <summary>
        /// Preco loss do stop
        /// </summary>
        public Nullable<decimal> preco_loss { set; get; }

        /// <summary>
        /// Preco de envio do loss
        /// </summary>
        public Nullable<decimal> preco_envio_loss { set; get; }

        /// <summary>
        /// Preco do start
        /// </summary>
        public Nullable<decimal> preco_start { set; get; }

        /// <summary>
        /// Verifica se o Stop/Start esta ativo ou inativo.
        /// </summary>
        public Nullable<char> st_ativo { set; get; }

        /// <summary>
        /// Valor de Inicio do stop movel
        /// </summary>
        public Nullable<decimal> valor_inicio_movel { set; get; }

        /// <summary>
        /// Valor de ajuste do movel
        /// </summary>
        public Nullable<decimal> valor_ajuste_movel { set; get; }
    }
}
