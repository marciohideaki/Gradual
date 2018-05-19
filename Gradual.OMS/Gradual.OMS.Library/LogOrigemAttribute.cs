using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Atributo para identificar uma origem de log.
    /// </summary>
    public class LogOrigemAttribute : Attribute
    {
        /// <summary>
        /// Indica o nome da origem a ser utilizada no log.
        /// Caso este valor seja nulo, utiliza a string retornada
        /// pelo método como nome da origem.
        /// </summary>
        public string Origem { get; set; }
    }
}
