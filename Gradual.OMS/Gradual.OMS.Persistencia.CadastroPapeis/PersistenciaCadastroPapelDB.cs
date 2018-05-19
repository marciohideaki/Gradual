using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Persistencia.CadastroPapeis
{
    public class PersistenciaCadastroPapelDB : IPersistencia
    {
        #region constructors
        public PersistenciaCadastroPapelDB()
        { }
        #endregion

        #region Methods

        #endregion


        #region IPersistencia Members

        public AtualizarMetadadosResponse AtualizarMetadados(AtualizarMetadadosRequest parametros)
        {
            throw new System.NotImplementedException();
        }

        public ConsultarObjetosResponse<T> ConsultarObjetos<T>(ConsultarObjetosRequest<T> parametros) where T : ICodigoEntidade
        {
            throw new System.NotImplementedException();
        }

        public ListarTiposResponse ListarTipos(ListarTiposRequest parametros)
        {
            throw new System.NotImplementedException();
        }

        public ReceberObjetoResponse<T> ReceberObjeto<T>(ReceberObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            throw new System.NotImplementedException();
        }

        public RemoverObjetoResponse<T> RemoverObjeto<T>(RemoverObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            throw new System.NotImplementedException();
        }

        public SalvarObjetoResponse<T> SalvarObjeto<T>(SalvarObjetoRequest<T> parametros) where T : ICodigoEntidade
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            throw new System.NotImplementedException();
        }

        public void PararServico()
        {
            throw new System.NotImplementedException();
        }

        public ServicoStatus ReceberStatusServico()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
