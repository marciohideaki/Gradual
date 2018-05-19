
using Gradual.OMS.Persistencia;

namespace Gradual.OMS.CadastroPapeis.Entidades
{
    public interface IEntidadeDbLib<T>
    {
        /// <param name="lRequest"></param>
        ConsultarObjetosResponse<T> ConsultarObjetos(ConsultarObjetosRequest<T> lRequest);

        /// 
        /// <param name="lRequest"></param>
        ReceberObjetoResponse<T> ReceberObjeto(ReceberObjetoRequest<T> lRequest);

        /// 
        /// <param name="lRequest"></param>
        RemoverObjetoResponse<T> RemoverObjeto(RemoverObjetoRequest<T> lRequest);

        /// 
        /// <param name="lRequest"></param>
        SalvarObjetoResponse<T> SalvarObjeto(SalvarObjetoRequest<T> lRequest);
    }
}
