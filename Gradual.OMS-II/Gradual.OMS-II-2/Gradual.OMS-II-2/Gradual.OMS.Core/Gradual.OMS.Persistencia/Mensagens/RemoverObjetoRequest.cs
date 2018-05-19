using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia
{
    public class RemoverObjetoRequest<T> : MensagemRequestBase
    {
        /// <summary>
        /// Permite carregar, opcionalmente, o código da sessão
        /// </summary>
        public string CodigoSessao { get; set; }

        /// <summary>
        /// Código do objeto a remover
        /// </summary>
        public string CodigoObjeto { get; set; }



    }
}
