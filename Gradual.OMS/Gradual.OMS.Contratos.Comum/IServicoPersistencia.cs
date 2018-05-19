using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Contratos.Comum
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
