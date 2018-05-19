using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.BMF.Dados
{
    /// <summary>
    /// Informações de uma cotação BMF
    /// </summary>
    public class CotacaoBMFInfo : ICloneable
    {
        /// <summary>
        /// Data de referência da cotação
        /// </summary>
        public DateTime DataReferencia { get; set; }

        /// <summary>
        /// Código de negociação do ativo
        /// </summary>
        public string CodigoNegociacao { get; set; }

        /// <summary>
        /// Código de mercadoria. Exemplo: BGI, DOL, DI1, FRC, etc.
        /// </summary>
        public string CodigoMercadoria { get; set; }

        /// <summary>
        /// Indica o tipo do instrumento, se é uma ação, opção, futuro, etc.
        /// </summary>
        public InstrumentoBMFTipoMercadoEnum TipoMercado { get; set; }

        /// <summary>
        /// Tipo da série: opc compra, opc venda, futuro
        /// </summary>
        public CotacaoBMFTipoSerieEnum TipoSerie { get; set; }

        /// <summary>
        /// Série ou vencimento do instrumento. Exemplo: Z10, X10, V11, etc.
        /// </summary>
        public string SerieVencimento { get; set; }

        /// <summary>
        /// Data de vencimento do instrumento.
        /// Instrumentos com tipo de mercado = disponível não tem data de vencimento
        /// </summary>
        public DateTime? DataVencimento { get; set; }

        /// <summary>
        /// Preco de exercicio da opção
        /// </summary>
        public double PrecoExercicio { get; set; }

        /// <summary>
        /// Valor ou tamanho do contrato
        /// </summary>
        public double ValorOuTamanhoContrato { get; set; }

        /// <summary>
        /// Volume do dia em reais
        /// </summary>
        public double VolumeDiaEmReais { get; set; }

        /// <summary>
        /// Volume do dia em dolar
        /// </summary>
        public double VolumeDiaEmDolar { get; set; }

        /// <summary>
        /// Quantidade de contratos em aberto
        /// </summary>
        public int QuantidadeContratosAberto { get; set; }

        /// <summary>
        /// Quantidade de negocios do dia
        /// </summary>
        public int QuantidadeNegociosDia { get; set; }

        /// <summary>
        /// Quantidade de contratos negociados no dia
        /// </summary>
        public int QuantidadeContratosNegociadosDia { get; set; }

        /// <summary>
        /// Quantidade de contratos da ultima oferta de compra do dia
        /// </summary>
        public int QuantidadeContratosUltimaOfertaCompraDia { get; set; }

        /// <summary>
        /// Cotacao da ultima oferta de compra do dia
        /// </summary>
        public double CotacaoUltimaOfertaCompraDia { get; set; }

        /// <summary>
        /// Quantidade de contratos da ultima oferta de venda do dia
        /// </summary>
        public int QuantidadeContratosUltimaOfertaVendaDia { get; set; }

        /// <summary>
        /// Cotacao da ultima oferta de venda do dia
        /// </summary>
        public double CotacaoUltimaOfertaVendaDia { get; set; }

        /// <summary>
        /// Cotação do primeiro negócio do dia
        /// </summary>
        public double CotacaoPrimeiroNegocioDia { get; set; }

        /// <summary>
        /// Menor cotação negociada no dia
        /// </summary>
        public double CotacaoMenorNegocioDia { get; set; }

        /// <summary>
        /// Maior cotação negociada no dia
        /// </summary>
        public double CotacaoMaiorNegocioDia { get; set; }

        /// <summary>
        /// Cotação média dos negócios do dia
        /// </summary>
        public double CotacaoMediaNegociosDia { get; set; }

        /// <summary>
        /// Quantidade de contratos do ultimo negocio do dia
        /// </summary>
        public int QuantidadeContratosUltimoNegocioDia { get; set; }

        /// <summary>
        /// Cotacao do ultimo negócio do dia
        /// </summary>
        public double CotacaoUltimoNegocioDia { get; set; }

        /// <summary>
        /// Data/Hora do último negócio do dia
        /// </summary>
        public DateTime? DataUltimoNegocioDia { get; set; }

        /// <summary>
        /// Cotação do último negócio anterior
        /// </summary>
        public double CotacaoUltimoNegocioAnterior { get; set; }

        /// <summary>
        /// Cotação de fechamento do dia
        /// </summary>
        public double CotacaoFechamentoDia { get; set; }

        /// <summary>
        /// Cotação de ajuste
        /// </summary>
        public double CotacaoAjuste { get; set; }

        /// <summary>
        /// Situação de ajuste do dia: ajuste final, ajuste corrigido
        /// </summary>
        public CotacaoBMFSituacaoAjusteEnum SituacaoAjusteDia { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
