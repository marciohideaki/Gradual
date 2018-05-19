using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Atributo para mensagens de request.
    /// Complementa informações do tipo, como por exemplo, qual o tipo do response
    /// esperado.
    /// </summary>
    public class MensagemRequestAttribute : Attribute
    {
        /// <summary>
        /// Tipo do response esperado para o request
        /// </summary>
        public Type TipoResponse { get; set; }
    }
}
