using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Indica os tipos de log utilizados pelo utilitario Log
    /// </summary>
    public enum LogTipoEnum
    {
        /// <summary>
        /// Indica uma entrada de log de erro
        /// </summary>
        Erro,

        /// <summary>
        /// Indica uma entrada de log de aviso
        /// </summary>
        Aviso,

        /// <summary>
        /// Indica uma entrada de log de passagem, informação, etc.
        /// </summary>
        Passagem
    }
}
