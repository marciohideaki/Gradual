using System.ServiceModel;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Persistencia
{
    /// <summary>
    /// Interface para implementação de um controlador de persistencia
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    public interface IPersistencia : IServicoControlavel
    {
        /// <summary>
        /// Lista todos os tipos que existem no repositório, ou os tipos que o repositório
        /// trabalha
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ListarTiposResponse ListarTipos(ListarTiposRequest parametros);

        /// <summary>
        /// Solicita atualização de metadados.
        /// Esta operação solicita que a persistencia faça a atualização de 
        /// metadados, por exemplo, inserindo os Lista/ListaItem, a lista
        /// de permissões, etc. Cada persistencia trabalha com os metadados
        /// que necessitar
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        AtualizarMetadadosResponse AtualizarMetadados(AtualizarMetadadosRequest parametros);

        /// <summary>
        /// Lista os objetos do tipo informado que obedecem às condições.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ConsultarObjetosResponse<T> ConsultarObjetos<T>(ConsultarObjetosRequest<T> parametros) where T : ICodigoEntidade;

        /// <summary>
        /// Recebe o objeto desejado
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberObjetoResponse<T> ReceberObjeto<T>(ReceberObjetoRequest<T> parametros) where T : ICodigoEntidade;

        /// <summary>
        /// Salva o objeto informado através de mensagem
        /// </summary>
        /// <param name="parametros"></param>
        SalvarObjetoResponse<T> SalvarObjeto<T>(SalvarObjetoRequest<T> parametros) where T : ICodigoEntidade;

        /// <summary>
        /// Remove o objeto informado pelo código
        /// </summary>
        /// <param name="parametros"></param>
        RemoverObjetoResponse<T> RemoverObjeto<T>(RemoverObjetoRequest<T> parametros) where T : ICodigoEntidade;
    }
}
