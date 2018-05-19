using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Interface.Dados
{
    /// <summary>
    /// Representa os tipos de plataformas
    /// </summary>
    public enum InterfacePlataformaEnum
    {
        /// <summary>
        /// Indica plataforma desktop
        /// </summary>
        Desktop,

        /// <summary>
        /// Indica plataforma web
        /// </summary>
        Web,

        /// <summary>
        /// Indica qualquer plataforma.
        /// Pode ser utilizado em filtros para indicar qualquer plataforma.
        /// </summary>
        Qualquer
    }
}
