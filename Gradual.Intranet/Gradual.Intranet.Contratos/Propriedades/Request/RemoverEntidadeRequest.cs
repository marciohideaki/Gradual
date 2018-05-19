using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Persistencia;


namespace Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request
{
    public sealed class RemoverEntidadeRequest<T> :RemoverObjetoRequest<T>{

        public T Objeto { set; get; }


    }
}
 