using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Interface.Desktop.Dados
{
    /// <summary>
    /// Classe de auxilio para a consulta de controles.
    /// Armazena o controle e a janela em que o controle está
    /// </summary>
    public class ConsultaControlesHelper
    {
        /// <summary>
        /// Referencia para o controleInfo
        /// </summary>
        public ControleInfo Controle { get; set; }

        /// <summary>
        /// Referencia para a janelaInfo
        /// </summary>
        public JanelaInfo Janela { get; set; }

        /// <summary>
        /// Referencia para o host
        /// </summary>
        public HostInfo Host { get; set; }
    }
}
