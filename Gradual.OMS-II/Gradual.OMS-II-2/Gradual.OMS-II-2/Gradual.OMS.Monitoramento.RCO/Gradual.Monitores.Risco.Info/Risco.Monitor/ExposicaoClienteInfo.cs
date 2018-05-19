using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Gradual.Monitores.Risco.Enum;

namespace Gradual.Monitores.Risco.Lib
{
    /// <summary>
    /// Exposição do cliente  - Classe responsável pelo armazenamento das informações de resumo do cliente.
    /// Lista de Garantias, Lista de operações intraday BMF e BOVESPA, Limites operacionais, 
    /// </summary>
    [Serializable]
    [DataContract]
    public class ExposicaoClienteInfo
    {
        /// <summary>
        /// Nome do Cliente
        /// </summary>
        [DataMember]
        public string Cliente { set; get; }

        /// <summary>
        /// Codigo do assessor
        /// </summary>
        [DataMember]
        public string Assessor { set; get; }

        /// <summary>
        /// Nome do Assessor
        /// </summary>
        [DataMember]
        public string NomeAssessor { set; get; }

        /// <summary>
        /// Codigo Bovespa
        /// </summary>
        [DataMember]
        public int CodigoBovespa { set; get; }

        /// <summary>
        /// Codigo BMF
        /// </summary>
        [DataMember]
        public int CodigoBMF { set; get; }

        /// <summary>
        /// Saldo Conta Margem
        /// </summary>
        [DataMember]
        public decimal SaldoContaMargem { set; get; }

        /// <summary>
        /// Somatória total de garantias
        /// </summary>
        [DataMember]
        public decimal TotalGarantias { set; get; }

        /// <summary>
        /// Saldo de Lucro Prejuízo BMF
        /// </summary>
        [DataMember]
        public decimal SaldoBMF { set; get; }

        /// <summary>
        /// Saldo de conta corrente de abertura 
        /// </summary>
        [DataMember]
        public decimal ContaCorrenteAbertura { set; get; }

        /// <summary>
        /// Saldo da somatória da custódia (Quantidade * Cotação Abertura)
        /// (IMPORTANTE) - Não soma custódia da carteira de BTC
        /// </summary>
        [DataMember]
        public decimal CustodiaAbertura { set; get; }

        /// <summary>
        /// PL de Abertura Bovespa é a soma da Conta Corrente de Abertura + Custódia de Abertura + 
        /// a somatória de Garantias de Bovespa
        /// e é utilizada para expor quanto o cliente abriu o dia
        /// </summary>
        [DataMember]
        public decimal PLAberturaBovespa { set; get; }

        /// <summary>
        /// PL de Abertura BMF é a soma do Lucro Prejuízo Compra abertura + Lucro Prejuízo Venda abertura + 
        /// a somatória de Garantias de BMF
        /// </summary>
        [DataMember]
        public decimal PLAberturaBMF { set; get; }

        /// <summary>
        /// Lucro Prejuízo termo é a somatória do Lucro Prejuízo de termo
        /// </summary>
        [DataMember]
        public decimal LucroPrejuizoTermo { set; get; }

        /// <summary>
        /// Valor em bens que o cliente têm no sinacor
        /// </summary>
        [DataMember]
        public decimal SituacaoFinanceiraPatrimonial { set; get; }

        /// <summary>
        /// Situação financeira patrimonial L1 é um valor operacional que fica na tabela TSCCLIBOL do sinacor 
        /// e pode alterar durante o dia
        /// </summary>
        [DataMember]
        public decimal SFPL1 { get; set; }

        /// <summary>
        /// Total de fundos é a somatória dos saldos de fundos que o cliente têm
        /// </summary>
        [DataMember]
        public decimal TotalFundos { set; get; }

        /// <summary>
        /// Total de Clubes é a somatória dos saldos de clubes que o cliente têm. 
        /// Esse item ainda não é utilizado e deverá ser usado quando for implementada a solução de clubes.
        /// </summary>
        [DataMember]
        public decimal TotalClubes { set; get; }

        /// <summary>
        /// Listagem com a posição de fundos que o cliente têm
        /// </summary>
        [DataMember]
        public List<ClienteFundoInfo> PosicaoFundos { set; get; }

        /// <summary>
        /// Listagem com a posição de clubes que o cliente têm
        /// </summary>
        [DataMember]
        public List<ClienteClubeInfo> PosicaoClubes { set; get; }

        /// <summary>
        /// Lucro Prejuízo Bmf é composto dos seguintes itens:
        /// Lucro Prejuízo Compra (Intraday) + 
        /// Lucro Prejuízo Venda  (Instraday) +
        /// Posição de Abertura do Dia - >  (Lucro Prejuízo Compra (Abertura) + Lucro Prejuízo Venda (Abertura))
        /// </summary>
        [DataMember]
        public decimal LucroPrejuizoBMF { set; get; }

        /// <summary>
        /// Lucro prejuízo Bovespa é a somatória do Lucro Prejuízo das Ordens executadas intraday
        /// </summary>
        [DataMember]
        public decimal LucroPrejuizoBOVESPA { set; get; }

        /// <summary>
        /// Lucro Prejuízo total é composto por Lucro Prejuízo BMF + Lucro Prejuízo BOVESPA
        /// </summary>
        [DataMember]
        public decimal LucroPrejuizoTOTAL { set; get; }

        /// <summary>
        /// Total conta corrente em tempo real é composto pelo Conta corrente abertura + 
        /// (Custodia valorizada venda intraday - Custodia valorizada compra intraday)
        /// (Observações importantes) As Custódias valorizadas Intraday são as operações executadas intraday
        /// </summary>
        [DataMember]
        public decimal TotalContaCorrenteTempoReal { set; get; }

        /// <summary>
        /// Patrimonio Liquido em tempo real é a soma dos seguintes itens:
        /// Projetado onLine  -> soma das conta corrrente D0 + D1 + D2 + D3
        /// Custodia Valorizada Abertura -> Saldo da somatória da custódia (Quantidade * Cotação Abertura) (IMPORTANTE) - Não soma custódia da carteira de BTC
        /// (Custodia valorizada venda intraday - Custodia valorizada compra intraday) (Observações importantes) As Custódias valorizadas Intraday são as operações executadas intraday
        /// Lucro Prejuízo BMF -> Lucro Prejuízo Compra (Intraday) + Lucro Prejuízo Venda  (Instraday) + Posição de Abertura do Dia - >  (Lucro Prejuízo Compra (Abertura) + Lucro Prejuízo Venda (Abertura))
        /// Saldo BMF -> Saldo BMF é quanto o cliente tem de garantias na tabela TMFGARMAR, coluna VL_TOTMAR
        /// </summary>
        [DataMember]
        public decimal PatrimonioLiquidoTempoReal { set; get; }

        /// <summary>
        /// Net de operações Bovespa é diferença entre o financeiro comprado e o financeiro vendido
        /// o Financeiro é a somatória dos totais as negociações do cliente no intraday
        /// </summary>
        [DataMember]
        public decimal NetOperacoes { set; get; }

        /// <summary>
        /// Lista de limites operacionais 
        /// Compra à Vista, Venda à Vista, 
        /// Compra Opção , Venda Opção 
        /// </summary>
        [DataMember]
        public LimitesInfo LimitesOperacionais { set; get; }

        /// <summary>
        /// Listagem de Operações BTC - Aluguel de Ações
        /// </summary>
        [DataMember]
        public List<BTCInfo> OrdensBTC { set; get; }

        /// <summary>
        /// Listagem de ordens de termo
        /// </summary>
        [DataMember]
        public List<PosicaoTermoInfo> OrdensTermo { set; get; }

        /// <summary>
        /// Listagem de ordens Bovespa executadas
        /// </summary>
        [DataMember]
        public List<OperacoesInfo> OrdensExecutadas { set; get; }

        /// <summary>
        /// Listagem de ordens bmf do cliente
        /// </summary>
        [DataMember]
        public List<PosicaoBmfInfo> OrdensBMF { set; get; }

        /// <summary>
        /// Data do ultimo calculo da posição do cliente
        /// </summary>
        [DataMember]
        public DateTime DtAtualizacao { set; get; }

        /// <summary>
        /// Percentual de prejuízo em relação ao patrimonio liquido em tempo real que o cliente tem
        /// LucroPrejuízoTotal /(dividido) patrimonio liquido tempo real
        /// </summary>
        [DataMember]
        public decimal PercentualPrejuizo { set; get; }

        /// <summary>
        /// Listagem de negociações com risco resumido por operação
        /// </summary>
        [DataMember]
        public List<ClienteRiscoResumo> Operacoes { set; get; }

        /// <summary>
        /// Semaforo é a prorpiedade que informa se a posição de risco do cliente está em criticidade
        /// setando em Vermelho se o percentual de prejuízo é maior que 70%, 
        /// Amarelo se o percentual é maior que 20% e menor que 70% 
        /// verde se o percentual de prejuízo é menor que 20%
        /// O percentual de prejuízo é calculado com o LucroPrejuízoTotal /(dividido) patrimonio liquido tempo real
        /// </summary>
        [DataMember]
        public EnumSemaforo Semaforo { set; get; }

        /// <summary>
        /// Proporção do Lucro Prejuízo 
        /// </summary>
        [DataMember]
        public EnumProporcaoPrejuizo ProporcaoLucroPrejuizo { set; get; }

        /// <summary>
        /// Volume total financeiro Bovespa é a somatoria do finaceiro compra bovespa + o financeiro vendido bovespa do cliente.
        /// o Financeiro é a somatória do valor dos negócios de bovespa do cliente
        /// </summary>
        [DataMember]
        public decimal VolumeTotalFinanceiroBov { set; get; }

        /// <summary>
        /// Volume total financeiro BMF é a somatoria do finaceiro compra BMF + o financeiro vendido BMF do cliente.
        /// o Financeiro é a somatória do valor dos negócios de BMF do cliente
        /// </summary>
        [DataMember]
        public decimal VolumeTotalFinanceiroBmf { set; get; }
    }
}
