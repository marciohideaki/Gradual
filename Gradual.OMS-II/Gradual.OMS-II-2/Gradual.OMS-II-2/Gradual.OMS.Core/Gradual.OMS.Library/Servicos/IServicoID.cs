using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Interface para serviços que podem ter várias instâncias.
    /// Tem associação com leitura de arquivos de configuração.
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoID
    {
        /// <summary>
        /// Atribui o id ao serviço
        /// </summary>
        /// <param name="id"></param>
        [OperationContract]
        void SetarID(string id);

        /// <summary>
        /// Recebe o id do servico
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string ReceberID();
    }
}
