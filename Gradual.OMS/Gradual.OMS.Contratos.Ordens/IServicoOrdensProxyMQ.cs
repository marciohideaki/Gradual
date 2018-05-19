using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Contratos.Ordens
{
    /// <summary>
    /// Interface para o serviço de proxy entre ordens e MSMQ.
    /// </summary>
    public interface IServicoOrdensProxyMQ : IServicoControlavel
    {
    }
}
