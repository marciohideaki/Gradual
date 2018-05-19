using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Custodia.Lib.Mensageria;
using Gradual.OMS.Custodia.Lib;
using Gradual.OMS.Custodia.Lib.Info;
using Gradual.OMS.Risco.Persistencia.Lib;

namespace Gradual.OMS.Risco.Custodia
{
    public class ServicoCustodia : IServicoCustodia
    {
        /// <summary>
        /// Obtem a custódia do cliente
        /// </summary>
        /// <param name="pParametros"> informações do cliente</param>
        /// <returns>Custódia do cliente</returns>
        public SaldoCustodiaResponse<CustodiaClienteInfo> ObterCustodiaCliente(SaldoCustodiaRequest pParametros)
        {
            return new PersistenciaCustodia().ObterSaldoCustodiaCliente(pParametros);
        }

        /// <summary>
        /// Obtem a custódia do cliente
        /// </summary>
        /// <param name="pParametros"> informações do cliente</param>
        /// <returns>Custódia do cliente</returns>
        public SaldoCustodiaResponse<CustodiaClienteInfo> ObterCustodiaConsolidada(SaldoCustodiaRequest pParametros)
        {
            return new PersistenciaCustodia().ObterSaldoCustodiaConsolidada(pParametros);
        }

        /// <summary>
        /// Obtem a custódia do cliente
        /// </summary>
        /// <param name="pParametros"> informações do cliente</param>
        /// <returns>Custódia do cliente</returns>
        public SaldoCustodiaResponse<CustodiaClienteBMFInfo> ObterCustodiaClienteBMF(SaldoCustodiaRequest pParametros)
        {
            return new PersistenciaCustodia().ObterCustodiaClienteBMF(pParametros);
        }

        /// <summary>
        /// Método responsável por incluir um bloqueio de custodia para o cliente
        /// </summary>
        /// <param name="pParametros">informações do cliente</param>
        /// <returns>ClienteCustodiaBloqueioResponse</returns>
        public ClienteCustodiaBloqueioResponse<ClienteCustodiaBloqueioInfo> InserirBloqueioCliente(ClienteCustodiaBloqueioRequest<ClienteCustodiaBloqueioInfo> pParametros)
        {
            return new PersistenciaCustodia().InserirBloqueioCliente(pParametros);
        }

        /// <summary>
        /// Método responsável por retornar o(s) bloqueio(s) em custódia do cliente;
        /// </summary>
        /// <param name="pParametros">informações do cliente</param>
        /// <returns>ClienteCustodiaBloqueioResponse</returns>
        public ClienteCustodiaBloqueioResponse<ClienteCustodiaBloqueioInfo> ListarBloqueioCliente(ClienteCustodiaBloqueioRequest<ClienteCustodiaBloqueioInfo> pParametros)
        {
            return new PersistenciaCustodia().ListarBloqueioCliente(pParametros);
        }

        /// <summary>
        /// Obtem a custódia do cliente para mostrar na intranet
        /// </summary>
        /// <param name="pParametros">Informações do cliente</param>
        /// <returns>Custódia do cliente para mostrar na intranet</returns>
        public SaldoCustodiaResponse<CustodiaClienteInfo> ObterSaldoCustodiaClienteIntranet(SaldoCustodiaRequest pParametros)
        {
            return new PersistenciaCustodia().ObterSaldoCustodiaClienteIntranet(pParametros);
        }

        /// <summary>
        /// Obtem as taxas de custódia do cliente
        /// </summary>
        /// <param name="pParametros"> informações do cliente</param>
        /// <returns>taxas de custódia do cliente</returns>
        public TaxaCustodiaResponse<TaxaCustodiaClienteInfo> ObterTaxaCustodiaCliente(TaxaCustodiaRequest pParametros)
        {
            return new PersistenciaCustodia().ObterTaxaCustodiaCliente(pParametros);
        }

        /// <summary>
        /// Obtem a custódia do cliente
        /// </summary>
        /// <param name="pParametros">Informações do cliente</param>
        /// <returns>Custódia do cliente </returns>
        public CustodiaResponse<CustodiaClienteInfo> ObterCustodiaCliente(CustodiaRequest pParametros)
        {
            return new PersistenciaCustodia().ObterCustodiaCliente(pParametros);
        }
    }
}
