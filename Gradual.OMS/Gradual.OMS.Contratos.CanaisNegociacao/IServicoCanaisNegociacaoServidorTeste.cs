using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Contratos.CanaisNegociacao
{
    /// <summary>
    /// Interface para o serviço Servidor Fix de teste.
    /// Este serviço sobe um servidor Fix para testar conexões fix de clientes.
    /// O servidor irá responder mensagens padrões apenas para teste.
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoCanaisNegociacaoServidorTeste : IServicoControlavel, IServicoID
    {
    }
}
