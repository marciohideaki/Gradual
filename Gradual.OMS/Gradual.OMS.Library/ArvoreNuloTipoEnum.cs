using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Indica como deve ser o tratamento de nulo na chave
    /// </summary>
    public enum ArvoreNuloTipoEnum
    {
        /// <summary>
        /// Caso o valor da chave seja nulo, utiliza o caminho default
        /// </summary>
        TratarNuloComoDefault,

        /// <summary>
        /// Caso o valor da chave seja nulo, utiliza todos os elementos,
        /// o caminho default e os itens filhos
        /// </summary>
        TratarNuloComoTodos
    }
}
