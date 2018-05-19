using System.ServiceModel;


namespace Gradual.OMS.Persistencia
{
	/// <summary>
	/// Interface para o serviço de persistencia 
	/// </summary>
    [ServiceContract(Namespace = "http://gradual")]
	public interface IServicoPersistencia : IPersistencia
    {
        /// <summary>
        /// Solicita que o serviço adicione a persistencia informada, relacionando
        /// aos tipos de objeto informados
        /// </summary>
        /// <param name="persistenciaInfo"></param>
        void AdicionarPersistencia(PersistenciaInfo persistenciaInfo);
    }
}
