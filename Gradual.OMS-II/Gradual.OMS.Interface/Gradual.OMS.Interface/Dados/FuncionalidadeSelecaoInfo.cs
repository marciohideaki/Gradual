using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Interface.Dados
{
    /// <summary>
    /// Indica o estado de seleção de uma funcionalidade
    /// </summary>
    [Serializable]
    public class FuncionalidadeSelecaoInfo
    {
        /// <summary>
        /// Código da funcionalidade
        /// </summary>
        public string CodigoFuncionalidade { get; set; }

        /// <summary>
        /// Nome da funcionalidade
        /// </summary>
        public string NomeFuncionalidade { get; set; }

        /// <summary>
        /// Indica se a funcionalidade está selecionada ou não
        /// </summary>
        public bool Selecionado { get; set; }
    }
}
