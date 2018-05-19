using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;

namespace Gradual.OMS.Contratos.Ordens
{
    /// <summary>
    /// Interface para serviço de persistencia de ordens
    /// </summary>
    public interface IServicoOrdensPersistencia 
    {
        /// <summary>
        /// Salva informações de uma ordem
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarOrdemResponse SalvarOrdem(SalvarOrdemRequest parametros);

        /// <summary>
        /// Recebe informações detalhadas de uma ordem
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberOrdemResponse ReceberOrdem(ReceberOrdemRequest parametros);

        /// <summary>
        /// Recebe lista de ordens de acordo com determinados filtros
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarOrdensResponse ListarOrdens(ListarOrdensRequest parametros);
    }
}
