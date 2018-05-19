using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Tipos de serializacao de entidades
    /// </summary>
    public enum EntidadeTipoSerializacaoEnum
    {
        /// <summary>
        /// Serialização Binária
        /// </summary>
        [Description("Serialização Binária")]
        Binaria,

        /// <summary>
        /// Serialização XML
        /// </summary>
        [Description("Serialização XML")]
        XML
    }
}
