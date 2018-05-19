using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Dados
{
    /// <summary>
    /// Informações básicas sobre um ativo.
    /// É base para as classes AtivoAcaoInfo, AtivoOpcaoInfo, AtivoFuturoInfo e 
    /// AtivoTermoInfo (por enquanto).
    /// </summary>
    public class AtivoInfo
    {
        /// <summary>
        /// Caso o ativo não seja mais negociado em bolsa, indica a data do último 
        /// dia negociado.
        /// </summary>
        public DateTime? DataNegociacaoFim { get; set; }

        /// <summary>
        /// Indica a data inicial de negociação do ativo.
        /// </summary>
        public DateTime DataNegociacaoInicio { get; set; }

        /// <summary>
        /// Indica o instrumento relacionado ao ativo.
        /// </summary>
        public InstrumentoInfo Instrumento { get; set; }

        /// <summary>
        /// Indica o status do ativo, se está habilitado, desabilitado, etc.
        /// </summary>
        public AtivoStatusEnum Status { get; set; }
    }
}
