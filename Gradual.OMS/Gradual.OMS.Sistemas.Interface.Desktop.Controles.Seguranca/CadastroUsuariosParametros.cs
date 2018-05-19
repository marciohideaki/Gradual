using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Seguranca
{
    /// <summary>
    /// Parametros de tela do controle de cadastro de usuários
    /// </summary>
    [Serializable]
    public class CadastroUsuarioParametros
    {
        /// <summary>
        /// Coleção de layouts devexpress
        /// </summary>
        public LayoutsDevExpressHelper LayoutsDevExpress { get; set; }

        /// <summary>
        /// Parametros do detalhe de usuários
        /// </summary>
        public UsuarioDetalheParametros ParametrosUsuarioDetalhe { get; set; }

        /// <summary>
        /// Contrutor default
        /// </summary>
        public CadastroUsuarioParametros()
        {
            this.LayoutsDevExpress = new LayoutsDevExpressHelper();
            this.ParametrosUsuarioDetalhe = new UsuarioDetalheParametros();
        }
    }
}
