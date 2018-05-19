using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Interface a ser implementada por serviços que podem ter seu ciclo de vida controlado.
    /// Expõe métodos Iniciar, Parar e ReceberStatus.
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoControlavel
    {
        /// <summary>
        /// Solicita a inicialização do serviço
        /// </summary>
        [OperationContract]
        void IniciarServico();

        /// <summary>
        /// Solicita a finalização do serviço
        /// </summary>
        [OperationContract]
        void PararServico();

        /// <summary>
        /// Recebe o status atual do serviço
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ServicoStatus ReceberStatusServico();
    }
}
