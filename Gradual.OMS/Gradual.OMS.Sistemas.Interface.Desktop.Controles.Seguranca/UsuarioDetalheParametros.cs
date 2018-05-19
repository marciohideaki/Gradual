using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    /// <summary>
    /// Parametros de tela do controle de detalhe de usuário
    /// </summary>
    [Serializable]
    public class UsuarioDetalheParametros
    {
        /// <summary>
        /// Coleção de layouts devexpress
        /// </summary>
        public LayoutsDevExpressHelper LayoutsDevExpress { get; set; }

        /// <summary>
        /// Parametros das tabs complementares
        /// </summary>
        public Dictionary<Type, object> ParametrosTabsComplementares { get; set; }

        /// <summary>
        /// Layout da janela
        /// </summary>
        public LayoutJanelaHelper LayoutJanela { get; set; }

        /// <summary>
        /// Contrutor default
        /// </summary>
        public UsuarioDetalheParametros()
        {
            this.LayoutsDevExpress = new LayoutsDevExpressHelper();
            this.ParametrosTabsComplementares = new Dictionary<Type, object>();
            this.LayoutJanela = new LayoutJanelaHelper();
        }
    }
}
