using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Regras.Criticas
{
    /// <summary>
    /// Representa uma crítica de validação de limite.
    /// Carrega a informação do limite que realizou a validação
    /// </summary>
    public class CriticaRiscoLimiteInfo : CriticaRiscoInfo
    {
        /// <summary>
        /// Indica o limite que foi utilizado na validação que originou a crítica
        /// </summary>
        public LimiteInfo LimiteInfo { get; set; }

        /// <summary>
        /// Valor que está tendo o limite testado
        /// </summary>
        public double ValorTestado { get; set; }

        /// <summary>
        /// Valor do limite que está sendo comparado
        /// </summary>
        public double ValorLimite { get; set; }
    }
}
