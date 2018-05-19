using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Interface.Dados
{
    /// <summary>
    /// Representa um sistema ou módulo.
    /// Por exemplo: sistema de cadastros, sistema de ordens, sistema de risco, etc.
    /// </summary>
    [Serializable]
    public class SistemaInfo
    {
        /// <summary>
        /// Código do sistema
        /// </summary>
        public string CodigoSistema { get; set; }

        /// <summary>
        /// Nome do sistema
        /// </summary>
        public string NomeSistema { get; set; }
    }
}
