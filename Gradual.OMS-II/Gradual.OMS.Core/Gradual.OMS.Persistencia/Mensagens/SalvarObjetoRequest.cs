using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencia
{
    public class SalvarObjetoRequest<T> : MensagemRequestBase
    {
        /// <summary>
        /// Permite carregar, opcionalmente, o código da sessão
        /// </summary>
        public string CodigoSessao { get; set; }

        /// <summary>
        /// Objeto a ser salvo
        /// </summary>
        public T Objeto { get; set; }


    }
}
