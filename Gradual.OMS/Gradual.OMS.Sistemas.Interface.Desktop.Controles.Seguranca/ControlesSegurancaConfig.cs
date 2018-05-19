using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Gradual.OMS.Contratos.Interface.Desktop.Controles.Seguranca.Dados;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    /// <summary>
    /// Configurações para os controles de seguranca
    /// </summary>
    public class ControlesSegurancaConfig
    {
        /// <summary>
        /// Propriedade auxiliar para serialização
        /// </summary>
        public List<TabComplementarInfo> TabsComplementaresUsuario { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ControlesSegurancaConfig()
        {
            this.TabsComplementaresUsuario = new List<TabComplementarInfo>();
        }
    }
}
