using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco
{
    /// <summary>
    /// Parametros de tela do controle de cadastro de perfis de risco
    /// </summary>
    [Serializable]
    public class CadastroPerfilRiscoParametros
    {
        /// <summary>
        /// Coleção de layouts devexpress
        /// </summary>
        public LayoutsDevExpressHelper LayoutsDevExpress { get; set; }

        /// <summary>
        /// Parametros da janela de detalhe de perfil de risco
        /// </summary>
        public PerfilRiscoDetalheParametros ParametrosPerfilRiscoDetalhe { get; set; }

        /// <summary>
        /// Contrutor default
        /// </summary>
        public CadastroPerfilRiscoParametros()
        {
            this.LayoutsDevExpress = new LayoutsDevExpressHelper();
        }
    }
}
