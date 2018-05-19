using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Ordens
{
    /// <summary>
    /// Classe de configuração do controle de boleto
    /// </summary>
    [Serializable]
    public class OperacaoBoletoParametros
    {
        /// <summary>
        /// Coleção de layouts devexpress
        /// </summary>
        public LayoutsDevExpressHelper LayoutsDevExpress { get; set; }

        /// <summary>
        /// Contrutor default
        /// </summary>
        public OperacaoBoletoParametros()
        {
            this.LayoutsDevExpress = new LayoutsDevExpressHelper();
        }
    }
}
