using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Persistencia
{
    /// <summary>
    /// Classe de configuracao do cadastro de persistencia de objetos
    /// </summary>
    [Serializable]
    public class CadastroPersistenciaParametros
    {
        /// <summary>
        /// Coleção de layouts devexpress
        /// </summary>
        public LayoutsDevExpressHelper LayoutsDevExpress { get; set; }

        /// <summary>
        /// Contrutor default
        /// </summary>
        public CadastroPersistenciaParametros()
        {
            this.LayoutsDevExpress = new LayoutsDevExpressHelper();
        }
    }
}
