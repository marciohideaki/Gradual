using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.MarketData
{
    /// <summary>
    /// Parametros de tela do controle de cadastro de perfis de risco
    /// </summary>
    [Serializable]
    public class CadastroPapeisBovespaParametros
    {
        /// <summary>
        /// Coleção de layouts devexpress
        /// </summary>
        public LayoutsDevExpressHelper LayoutsDevExpress { get; set; }

        /// <summary>
        /// Contrutor default
        /// </summary>
        public CadastroPapeisBovespaParametros()
        {
            this.LayoutsDevExpress = new LayoutsDevExpressHelper();
        }
    }
}
