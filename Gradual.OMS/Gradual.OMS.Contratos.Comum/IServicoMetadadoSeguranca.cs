using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Contratos.Comum
{
    /// <summary>
    /// Interface do serviço de metadados de segurança
    /// </summary>
    public interface IServicoMetadadoSeguranca
    {
        /// <summary>
        /// Solicita a geração de metadados de segurança
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        GerarMetadadoSegurancaResponse GerarMetadadoSeguranca(GerarMetadadoSegurancaRequest parametros);
    }
}
