using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Risco
{
    /// <summary>
    /// Parametros do controle de cadastro de tickets de risco
    /// </summary>
    [Serializable]
    public class CadastroTicketsRiscoParametros
    {
        /// <summary>
        /// Coleção de layouts devexpress
        /// </summary>
        public LayoutsDevExpressHelper LayoutsDevExpress { get; set; }

        /// <summary>
        /// Contrutor default
        /// </summary>
        public CadastroTicketsRiscoParametros()
        {
            this.LayoutsDevExpress = new LayoutsDevExpressHelper();
        }
    }
}
