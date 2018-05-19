using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Contratos.Risco
{
    /// <summary>
    /// Interface do serviço de risco
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoRisco : IServicoControlavel
    {
        #region Validação de Risco

        /// <summary>
        /// Executa a validação da operação sem necessariamente executar a operação.
        /// É apenas uma rechamada para o serviço de validação
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ValidarOperacaoResponse ValidarOperacao(ValidarOperacaoRequest parametros);

        #endregion

        #region RegraRisco

        /// <summary>
        /// Retorna a lista de regras disponíveis para serem utilizadas
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarRegrasDisponiveisResponse ListarRegrasDisponiveis(ListarRegrasDisponiveisResponse parametros);

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

        /// <summary>
        /// Consulta as regras a serem executadas para um determinado 
        /// agrupamento através da lista de regras.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ConsultarRegrasDoAgrupamentoResponse ConsultarRegrasDoAgrupamento(ConsultarRegrasDoAgrupamentoRequest parametros);

        /// <summary>
        /// Solicita que o risco recarregue a lista de regras
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RecarregarListaRegrasResponse RecarregarListaRegras(RecarregarListaRegrasRequest parametros);

        #endregion

        #region Clientes e Custódia

        /// <summary>
        /// Pede inicialização do cliente.
        /// Neste momento a custódia do cliente é carregada no risco e armazenada em cache
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        InicializarClienteResponse InicializarCliente(InicializarClienteRequest parametros);

        /// <summary>
        /// Pede liberação do cliente.
        /// Neste momento o cache de custódia do cliente é liberado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        LiberarClienteResponse LiberarCliente(LiberarClienteRequest parametros);

        /// <summary>
        /// Solicita os caches de risco do cliente.
        /// Retorna árvore de custódia e informações de conta corrente.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberCacheRiscoResponse ReceberCacheRisco(ReceberCacheRiscoRequest parametros);

        #endregion

        #region Perfil de Risco

        /// <summary>
        /// Solicita lista de perfis de risco
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarPerfisRiscoResponse ListarPerfisRisco(ListarPerfisRiscoRequest parametros);

        /// <summary>
        /// Solicita detalhe de um perfil de risco
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberPerfilRiscoResponse ReceberPerfilRisco(ReceberPerfilRiscoRequest parametros);

        /// <summary>
        /// Remove um perfil de risco
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverPerfilRiscoResponse RemoverPerfilRisco(RemoverPerfilRiscoRequest parametros);

        /// <summary>
        /// Salva um perfil de risco
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarPerfilRiscoResponse SalvarPerfilRisco(SalvarPerfilRiscoRequest parametros);

        #endregion

        #region Ticket de Risco

        /// <summary>
        /// Solicita lista de tickets de risco
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ListarTicketsRiscoResponse ListarTicketsRisco(ListarTicketsRiscoRequest parametros);

        /// <summary>
        /// Solicita detalhe de um Ticket de risco
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ReceberTicketRiscoResponse ReceberTicketRisco(ReceberTicketRiscoRequest parametros);

        /// <summary>
        /// Remove um Ticket de risco
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        RemoverTicketRiscoResponse RemoverTicketRisco(RemoverTicketRiscoRequest parametros);

        /// <summary>
        /// Salva um Ticket de risco
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SalvarTicketRiscoResponse SalvarTicketRisco(SalvarTicketRiscoRequest parametros);

        #endregion

        #region Conta Corrente

        /// <summary>
        /// Solicita ao risco o processamento da operação
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        ProcessarOperacaoResponse ProcessarOperacao(ProcessarOperacaoRequest parametros);

        /// <summary>
        /// Solicita a sincronizacao de uma conta corrente com o sinacor
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        SincronizarContaCorrenteSinacorResponse SincronizarContaCorrenteSinacor(SincronizarContaCorrenteSinacorRequest parametros);

        #endregion

    }
}
