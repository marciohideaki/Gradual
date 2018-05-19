using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia
{
    public class ReceberObjetoRequest<T> : MensagemRequestBase
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
