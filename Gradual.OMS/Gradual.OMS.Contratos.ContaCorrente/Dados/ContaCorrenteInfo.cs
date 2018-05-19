using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.ContaCorrente.Dados
{
    /// <summary>
    /// Contem informações de uma conta corrente
    /// </summary>
    [Serializable]
    public class ContaCorrenteInfo : ICodigoEntidade
    {
        /// <summary>
        /// Código da conta corrente.
        /// Chave primária do objeto
        /// </summary>
        public string CodigoContaCorrente { get; set; }

        #region Saldo ContaMargem

        /// <summary>
        /// ContaMargem: Data referencia do saldo, ou a data do pregão considerado
        /// </summary>
        public DateTime ContaMargemDataReferencia { get; set; }

        /// <summary>
        /// ContaMargem: Valor do limite
        /// </summary>
        public double ContaMargemValorLimite { get; set; }

        /// <summary>
        /// ContaMargem: Valor de depósito em conta corrente
        /// </summary>
        public double ContaMargemValorDepositoContaCorrente { get; set; }

        /// <summary>
        /// ContaMargem: Valor Financiado
        /// </summary>
        public double ContaMargemValorFinanciado { get; set; }

        /// <summary>
        /// ContaMargem: Valor do IOF
        /// </summary>
        public double ContaMargemValorIOF { get; set; }

        #endregion

        #region Saldo Regular

        /// <summary>
        /// SaldoRegular: Indica o saldo atual da conta
        /// </summary>
        public double SaldoRegularAtual { get; set; }

        /// <summary>
        /// SaldoRegular: Contem o saldo projetado.
        /// O índice da lista indica a quantidade de dias futuros da projeção.
        /// 0 = saldo em d+0 = saldo atual, 1 = saldo em d+1, 2 = saldo em d + 2, etc
        /// </summary>
        public List<double> SaldoRegularProjetado { get; set; }

        /// <summary>
        /// SaldoRegular: Data em que foi feita a última integração com o sistema de custódia
        /// </summary>
        public DateTime SaldoRegularDataIntegracao { get; set; }

        /// <summary>
        /// SaldoRegular: Data da última movimentação na custódia
        /// </summary>
        public DateTime SaldoRegularDataUltimaMovimentacao { get; set; }

        #endregion

        #region Saldo Investimento

        /// <summary>
        /// SaldoInvestimento: Indica o saldo atual da conta investimento
        /// </summary>
        public double SaldoInvestimentoAtual { get; set; }

        /// <summary>
        /// SaldoInvestimento: Contem o saldo de investimento projetado.
        /// O índice da lista indica a quantidade de dias futuros da projeção.
        /// 0 = saldo em d+0 = saldo atual, 1 = saldo em d+1, 2 = saldo em d + 2, etc
        /// </summary>
        public List<double> SaldoInvestimentoProjetado { get; set; }

        /// <summary>
        /// SaldoInvestimento: Data em que foi feita a última integração com o sinacor
        /// </summary>
        public DateTime SaldoInvestimentoDataIntegracao { get; set; }

        /// <summary>
        /// SaldoInvestimento: Data da última movimentação no sinacor
        /// </summary>
        public DateTime SaldoInvestimentoDataUltimaMovimentacao { get; set; }

        #endregion

        #region Saldo Bloqueado

        /// <summary>
        /// Indica o saldo bloqueado do cliente.
        /// Operações pendentes de execução são contabilizadas aqui.
        /// </summary>
        public double SaldoBloqueado { get; set; }

        /// <summary>
        /// Indica a composição do saldo bloqueado.
        /// Permite armazenar maiores informações sobre cada parcela do saldo bloqueado,
        /// para posteriormente poder retirar.
        /// </summary>
        public List<SaldoBloqueadoParcelaInfo> SaldoBloqueadoComposicao { get; set; }

        /// <summary>
        /// Data da última movimentação do saldo bloqueado
        /// </summary>
        public DateTime SaldoBloqueadoDataUltimaMovimentacao { get; set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public ContaCorrenteInfo()
        {
            this.CodigoContaCorrente = Guid.NewGuid().ToString();
            this.SaldoRegularProjetado = new List<double>();
            this.SaldoInvestimentoProjetado = new List<double>();
            this.SaldoBloqueadoComposicao = new List<SaldoBloqueadoParcelaInfo>();
        }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoContaCorrente;
        }

        #endregion
    }
}
