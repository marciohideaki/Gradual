using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    public class ReceberObjetoRequest<T>
    {
        /// <summary>
        /// Permite carregar, opcionalmente, o código da sessão
        /// </summary>
        public string CodigoSessao { get; set; }

        /// <summary>
        /// Código do objeto requisitado
        /// </summary>
        public string CodigoObjeto { get; set; }
    }
}
