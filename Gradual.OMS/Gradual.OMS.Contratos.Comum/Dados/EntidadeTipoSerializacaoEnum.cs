using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Dados
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
