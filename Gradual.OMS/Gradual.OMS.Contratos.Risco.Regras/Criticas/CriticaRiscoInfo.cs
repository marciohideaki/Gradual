using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Regras.Criticas
{
    /// <summary>
    /// Classe base para críticas do risco
    /// </summary>
    public class CriticaRiscoInfo : CriticaInfo
    {
        /// <summary>
        /// Agrupamento que gerou a crítica
        /// </summary>
        public RiscoGrupoInfo Agrupamento { get; set; }
    }
}
