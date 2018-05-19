using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Persistencia;


namespace Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response
{
    public class RemoverEntidadeResponse<T> : RemoverObjetoResponse<T>
    {
        public Nullable<Boolean> lStatus { set; get; }

    }
}
 