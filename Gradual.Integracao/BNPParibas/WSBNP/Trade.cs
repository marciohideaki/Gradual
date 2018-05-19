using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace WSBNPParibas
{
    [Serializable]
    public class Trade
    {
        private CultureInfo ciPtBR = CultureInfo.CreateSpecificCulture("pt-BR");
        private CultureInfo ciEn = CultureInfo.CreateSpecificCulture("en-US");

        #region Elementos Obrigatorios
        /// <summary>
        /// Numero sequencial do trade
        /// </summary>
        public long Sequencial { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RecordType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long NumOrdem { get; set; }

        /// <summary>
        /// Nome do operador
        /// </summary>
        public string NomeOperador { get; set; }

        /// <summary>
        /// Data da operacao
        /// </summary>
        public DateTime DataOperacao { get; set; }

        /// <summary>
        /// Codigo do papel ou instrumento
        /// </summary>
        public string NomeAtivo { get; set; }

        public string Commod { get; set; }
        public string Serie { get; set; }
        public string Mercado { get; set; }

        /// <summary>
        /// Compra/Venda ?
        /// </summary>
        public string TipoOperacao { get; set; }

        /// <summary>
        /// Qtde de papeis ou contratos negociados
        /// </summary>
        public long Quantidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Decimal Preco { get; set; }


        /// <summary>
        /// Flag indicativo de que o trade ocorreu em after
        /// </summary>
        public string AfterHour { get; set; }

        /// <summary>
        /// Codigo de corretora originaria na BMF
        /// </summary>
        public long CodCorretoraBMFOrigem { get; set; }

        /// <summary>
        /// Nome ou razao social da corretora originaria
        /// </summary>
        public string NomeCorretoraBMFOrigem { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ExercicioOpcaoBMF { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CnpjCliente { get; set; }

        public string NomeCliente { get; set; }

        public string Externa { get; set; }

        public string Brokeragem { get; set; }

        /// <summary>
        /// Codigo de corretora originaria na BMF
        /// </summary>
        public long CodCorretoraBMFDestino { get; set; }

        /// <summary>
        /// Nome ou razao social da corretora originaria
        /// </summary>
        public string NomeCorretoraBMFDestino { get; set; }


        public string LeilaoTermo { get; set; }


        /// <summary>
        /// Data vencimento do termo
        /// </summary>
        public DateTime DataVencimento { get; set; }


        /// <summary>
        /// Cod do operador
        /// </summary>
        public string TraderID { get; set; }

        /// <summary>
        /// Nome do Operador
        /// </summary>
        public string TraderName { get; set; }


        public string NumContaCliente { get; set; }


        public string Observacao { get; set; }


        public string Delta { get;set;}

        
        public string ValorFuturoLongo { get; set; }


        public string ValorFuturoCurto { get; set; }

        public DateTime TradeTimestamp { get; set; }

        public string Isin { get; set; }

        #endregion //Elementos Obrigatorios

        public Trade()
        {
            Sequencial=0;
            RecordType="";
            NumOrdem=0;
            NomeOperador="";
            DataOperacao = DateTime.MinValue;
            NomeAtivo="";
            TipoOperacao="";
            Quantidade=0;
            Preco=Decimal.Zero;
            AfterHour="";
            CodCorretoraBMFOrigem=0;
            NomeCorretoraBMFOrigem="";
            ExercicioOpcaoBMF="";
            CnpjCliente="";
            NomeCliente="";
            Externa="";
            Brokeragem="";
            CodCorretoraBMFDestino=0;
            NomeCorretoraBMFDestino="";
            LeilaoTermo="";
            DataVencimento=DateTime.MinValue;
            TraderID="";
            TraderName="";
            NumContaCliente="";
            Observacao="";
            Delta = "";
            ValorFuturoLongo="";
            ValorFuturoCurto="";
            TradeTimestamp = DateTime.MinValue;
        }


        /// <summary>
        /// Gera um subset XML completo com as informacoes deste 
        /// </summary>
        /// <returns></returns>
        public string ToXML()
        {
            string response = "";
            string xml = "<trade>";

            xml += "<SEQUENCIAL>" + this.Sequencial + "</SEQUENCIAL>";
            xml += "<TIPO_BOLETAGEM>" + this.RecordType + "</TIPO_BOLETAGEM>";
            xml += "<CHAVE_ESTRANGEIRA>" + this.Sequencial + "</CHAVE_ESTRANGEIRA>";
            xml += "<NOME_OPERADOR>" + this.NomeOperador + "</NOME_OPERADOR>";
            xml += "<DATA_OPERACAO>" + this.DataOperacao.ToString("yyyy-MM-dd") + "</DATA_OPERACAO>";
            xml += "<NOME_ATIVO>" + this.NomeAtivo.Trim() + "</NOME_ATIVO>";
            xml += "<TIPO_DE_OPERACAO>" + this.TipoOperacao + "</TIPO_DE_OPERACAO>";
            xml += "<QUANTIDADE>" + this.Quantidade + "</QUANTIDADE>";
            xml += "<PRECO>" + this.Preco.ToString(ciEn) +  "</PRECO>";
            xml += "<AFTER_HOUR>" + this.AfterHour + "</AFTER_HOUR>";
            xml += "<COD_CORRETORA_BMF_ORIGEM>" + this.CodCorretoraBMFOrigem + "</COD_CORRETORA_BMF_ORIGEM>";
            xml += "<NOME_CORRETORA_ORIGEM>" + this.NomeCorretoraBMFOrigem + "</NOME_CORRETORA_ORIGEM>";
            xml += "<EXERCICIO_OPCAO_BMF>" + this.ExercicioOpcaoBMF + "</EXERCICIO_OPCAO_BMF>";
            xml += "<CNPJ_CLIENTE>" + this.CnpjCliente + "</CNPJ_CLIENTE>";
            xml += "<NOME_CLIENTE>" + this.NomeCliente + "</NOME_CLIENTE>";
            xml += "<EXTERNA>" + this.Externa + "</EXTERNA>";
            xml += "<BROKERAGEM>" + this.Brokeragem + "</BROKERAGEM>";
            xml += "<COD_CORRETORA_BMF_DESTINO>" + this.CodCorretoraBMFDestino + "</COD_CORRETORA_BMF_DESTINO>";
            xml += "<NOME_CORRETORA_DESTINO>" + this.NomeCorretoraBMFDestino + "</NOME_CORRETORA_DESTINO>";
            xml += "<LEILAO_TERMO>" + this.LeilaoTermo + "</LEILAO_TERMO>";
            xml += "<DATA_TERMO>" + this.DataVencimento.ToString("yyyy-MM-dd") + "</DATA_TERMO>";
            xml += "<TRADERID>" + this.TraderID + "</TRADERID>";
            xml += "<TRADERNAME>" + this.TraderName + "</TRADERNAME>";
            xml += "<NUM_CONTA_CLIENTE>" + this.NumContaCliente + "</NUM_CONTA_CLIENTE>";
            xml += "<OBSERVACAO>" + this.Observacao + "</OBSERVACAO>";
            xml += "<DELTA>" + this.Delta + "</DELTA>";
            xml += "<TX_JUROS_LONGO>" + this.ValorFuturoLongo + "</TX_JUROS_LONGO>";
            xml += "<TX_JUROS_CURTO>" +  this.ValorFuturoCurto + "</TX_JUROS_CURTO>";
            xml += "<TRADE_TIMESTAMP>" + TradeTimestamp.ToString("yyyy-MM-ddTHH:mm:ss") + "</TRADE_TIMESTAMP>";


            response += xml;
            response += "</trade>";

            return response;
        }


        public long NumNegocio { get; set; }
    }
}
