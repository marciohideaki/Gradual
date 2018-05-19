using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;
using System.Data;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    public interface IEntidadeDbLib<T>
    {
        ConsultarObjetosResponse<T> ConsultarObjetos(ConsultarObjetosRequest<T> parametros);

        ReceberObjetoResponse<T> ReceberObjeto(ReceberObjetoRequest<T> parametros);

        RemoverObjetoResponse<T> RemoverObjeto(RemoverObjetoRequest<T> parametros);

        SalvarObjetoResponse<T> SalvarObjeto(SalvarObjetoRequest<T> parametros);

        T MontarObjeto(DataRow dr);
    }
}
