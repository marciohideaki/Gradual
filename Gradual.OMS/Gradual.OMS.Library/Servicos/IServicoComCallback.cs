using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Gradual.OMS.Library.Servicos
{
    /// <summary>
    /// Interface para sinalizar um serviço que trabalha com callbacks.
    /// Esta interface trabalha exclusivamente com callbacks do tipo ICallbackEvento
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoComCallback
    {
        /// <summary>
        /// Pede para o serviço registrar o callback.
        /// Este overload, que não pede o callback, é utilizado quando
        /// a comunicação é por wcf. Neste caso, o serviço wcf irá conseguir
        /// o callback pelos próprios mecanismos do wcf.
        /// </summary>
        /// <param name="parametros"></param>
        [OperationContract]
        void Registrar(object parametros);

        /// <summary>
        /// Pede para o serviço registrar o callback.
        /// Este overload, que pede o callback, é utilizado quando a 
        /// comunicação não é via wcf. Neste caso o callback tem que
        /// ser enviado explicitamente.
        /// </summary>
        /// <param name="parametros"></param>
        /// <param name="callback"></param>
        void Registrar(object parametros, ICallbackEvento callback);
    }
}
