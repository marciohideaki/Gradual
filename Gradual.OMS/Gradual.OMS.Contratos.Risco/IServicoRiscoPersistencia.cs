using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Risco.Mensagens;

namespace Gradual.OMS.Contratos.Risco
{
    /// <summary>
    /// Interface para o serviço de persistencia do risco
    /// </summary>
    public interface IServicoRiscoPersistencia
    {
        #region RegraRisco

        /// <summary>
        /// Lista regras de risco de acordo com o filtro informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarRegraRiscoResponse ListarRegraRisco(ListarRegraRiscoRequest parametros);

        /// <summary>
        /// Recebe a regra de risco solicitada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberRegraRiscoResponse ReceberRegraRisco(ReceberRegraRiscoRequest parametros);

        /// <summary>
        /// Remove a regra de risco informada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverRegraRiscoResponse RemoverRegraRisco(RemoverRegraRiscoRequest parametros);

        /// <summary>
        /// Salva a regra de risco informada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarRegraRiscoResponse SalvarRegraRisco(SalvarRegraRiscoRequest parametros);

        #endregion
    }
}
