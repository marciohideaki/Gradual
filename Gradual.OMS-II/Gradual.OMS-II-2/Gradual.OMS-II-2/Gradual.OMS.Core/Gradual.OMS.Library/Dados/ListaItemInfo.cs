using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Representa um item numa lista de metadados.
    /// Geralmente terá uma relação com um item de
    /// enumerador, ou um tipo de classe, etc.
    /// </summary>
    public class ListaItemInfo
    {
        /// <summary>
        /// Descricao do elemento.
        /// Enumeradores fornecem esse valor através do atributo descrição,
        /// caso esteja presente.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Mnemonico do elemento.
        /// Em enumeradores este é o próprio nome do item.
        /// </summary>
        public string Mnemonico { get; set; }

        /// <summary>
        /// Valor do elemento.
        /// Em enumeradores este é o próprio valor do item.
        /// </summary>
        public string Valor { get; set; }
    }
}
