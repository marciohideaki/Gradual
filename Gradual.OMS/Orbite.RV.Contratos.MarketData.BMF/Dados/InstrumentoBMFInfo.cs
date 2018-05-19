using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.BMF.Dados
{
    /// <summary>
    /// Representa um instrumento BMF
    /// </summary>
    public class InstrumentoBMFInfo
    {
        /// <summary>
        /// Código utilizado na negociação do instrumento
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
        /// Série ou vencimento do instrumento. Exemplo: Z10, X10, V11, etc.
        /// </summary>
        public string SerieVencimento { get; set; }

        /// <summary>
        /// Data de vencimento do instrumento.
        /// Instrumentos com tipo de mercado = disponível não tem data de vencimento
        /// </summary>
        public DateTime? DataVencimento { get; set; }

        /// <summary>
        /// Data de início de negociação do instrumento
        /// </summary>
        public DateTime DataInicioNegociacao { get; set; }

        /// <summary>
        /// Data de limite de negociação do instrumento
        /// </summary>
        public DateTime DataLimiteNegociacao { get; set; }
    }
}
