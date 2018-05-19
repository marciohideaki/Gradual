using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.OMS
{
    /// <summary>
    /// Interface para o serviço de integração entre o sinacor
    /// e a segurança
    /// </summary>
    public interface IServicoIntegracaoSinacorOMS
    {
        /// <summary>
        /// Solicita a sincronização da custodia de determinado cliente com o sinacor
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SincronizarCustodiaResponse SincronizarCustodia(SincronizarCustodiaRequest parametros);

        /// <summary>
        /// Solicita a sincronização da conta corrente de determinado cliente com o sinacor
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SincronizarContaCorrenteResponse SincronizarContaCorrente(SincronizarContaCorrenteRequest parametros);

        /// <summary>
        /// Faz a tradução do código CBLC para o código de usuário.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        TraduzirCodigoCBLCResponse TraduzirCodigoCBLC(TraduzirCodigoCBLCRequest parametros);

        /// <summary>
        /// Solicita a inicialização do usuário.
        /// Carrega os códigos CBLC através do CPF/CNPJ, cria custodia, conta corrente e pode sincronizar.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        InicializarUsuarioResponse InicializarUsuario(InicializarUsuarioRequest parametros);
    }
}
