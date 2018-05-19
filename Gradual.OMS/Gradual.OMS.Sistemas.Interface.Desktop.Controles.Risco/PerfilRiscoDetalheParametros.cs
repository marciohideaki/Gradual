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
    public class PerfilRiscoDetalheParametros
    {
        /// <summary>
        /// Coleção de layouts devexpress
        /// </summary>
        public LayoutsDevExpressHelper LayoutsDevExpress { get; set; }

        /// <summary>
        /// Parametros da tab de regras de risco
        /// </summary>
        public TabRegraRiscoParametros ParametrosTabRegraRisco { get; set; }

        /// <summary>
        /// Contrutor default
        /// </summary>
        public PerfilRiscoDetalheParametros()
        {
            this.LayoutsDevExpress = new LayoutsDevExpressHelper();
        }
    }
}
