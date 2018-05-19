using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Contratos.Integracao.Sinacor.Mensagens;

namespace Gradual.OMS.Contratos.Integracao.Sinacor
{
    /// <summary>
    /// Interface para o serviço de integração com o sistema Sinacor
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoIntegracaoSinacor
    {
        /// <summary>
        /// Recebe informações do cliente
        /// </summary>
        /// <param name="codigoCBLC"></param>
        [OperationContract]
        ReceberClienteSinacorResponse ReceberClienteSinacor(ReceberClienteSinacorRequest parametros);

        /// <summary>
        /// Recebe informações de custódia de um cliente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberCustodiaSinacorResponse ReceberCustodiaSinacor(ReceberCustodiaSinacorRequest parametros);

        /// <summary>
        /// Recebe informações de saldo da conta corrente do sinacor.
        /// Retorna o saldo em d0 e os projetados d1, d2 e d3
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberSaldoContaCorrenteSinacorResponse ReceberSaldoContaCorrenteSinacor(ReceberSaldoContaCorrenteSinacorRequest parametros);

        /// <summary>
        /// Recebe informações de saldo da conta margem do sinacor.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberSaldoContaMargemSinacorResponse ReceberSaldoContaMargemSinacor(ReceberSaldoContaMargemSinacorRequest parametros);

        /// <summary>
        /// Recebe lista de cblc´s de cliente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarCBLCsClienteSinacorResponse ListarCBLCsClienteSinacor(ListarCBLCsClienteSinacorRequest parametros);
    }
}
