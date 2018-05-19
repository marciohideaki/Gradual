using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.ContaCorrente.Mensagens;

namespace Gradual.OMS.Contratos.ContaCorrente
{
    /// <summary>
    /// Interface para o serviço de conta corrente
    /// </summary>
    public interface IServicoContaCorrentePersistencia
    {
        /// <summary>
        /// Lista Contas Correntes de acordo com o filtro informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ConsultarContasCorrentesResponse ConsultarContasCorrentes(ConsultarContasCorrentesRequest parametros);

        /// <summary>
        /// Recebe detalhe de uma Conta Corrente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberContaCorrenteResponse ReceberContaCorrente(ReceberContaCorrenteRequest parametros);

        /// <summary>
        /// Remove uma conta corrente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverContaCorrenteResponse RemoverContaCorrente(RemoverContaCorrenteRequest parametros);

        /// <summary>
        /// Salva uma conta corrente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarContaCorrenteResponse SalvarContaCorrente(SalvarContaCorrenteRequest parametros);
    }
}
