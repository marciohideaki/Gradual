using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Custodia.Mensagens;

namespace Gradual.OMS.Contratos.Custodia
{
    /// <summary>
    /// Interface para o serviço de custodia
    /// </summary>
    public interface IServicoCustodia
    {
        /// <summary>
        /// Lista custodias de acordo com o filtro informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ConsultarCustodiasResponse ConsultarCustodias(ConsultarCustodiasRequest parametros);

        /// <summary>
        /// Recebe detalhe de uma custodia
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberCustodiaResponse ReceberCustodia(ReceberCustodiaRequest parametros);

        /// <summary>
        /// Remove uma custódia
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverCustodiaResponse RemoverCustodia(RemoverCustodiaRequest parametros);

        /// <summary>
        /// Salva uma custódia
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarCustodiaResponse SalvarCustodia(SalvarCustodiaRequest parametros);
    }
}
