using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco
{
    /// <summary>
    /// Parametros de tela do controle de detalhe de perfis de risco
    /// </summary>
    [Serializable]
    public class TabRegraRiscoParametros
    {
        /// <summary>
        /// Parametros do controle base de regras de risco
        /// </summary>
        public object ParametrosRegraRiscoBase { get; set; }

        /// <summary>
        /// Contrutor default
        /// </summary>
        public TabRegraRiscoParametros()
        {
        }
    }
}
