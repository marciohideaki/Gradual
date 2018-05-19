using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Contem informações complementares para o tipo representando a mensagem
    /// </summary>
    public class MensagemAttribute : Attribute
    {
        /// <summary>
        /// Caso o atributo represente uma classe de request, esta propriedade relaciona
        /// qual o tipo da resposta a esta mensagem
        /// </summary>
        public Type TipoMensagemResponse { get; set; }
    }
}
