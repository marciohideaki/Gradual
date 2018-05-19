using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;

namespace Gradual.OMS.Contratos.Ordens
{
    /// <summary>
    /// Contrato referente ao serviço de ordens.
    /// </summary>
    [ServiceContract(Namespace = "http://gradual")]
    public interface IServicoOrdens
    {
        /// <summary>
        /// Função chamada para envio de solicitação de execução de ordens.
        /// </summary>
        /// <param name="parametros">Mensagem com informações da solicitação de execução da ordem</param>
        /// <returns></returns>
        [OperationContract]
        ExecutarOrdemResponse ExecutarOrdem(ExecutarOrdemRequest parametros);

        /// <summary>
        /// Solicita o cancelamento de uma ordem
        /// </summary>
        /// <param name="parametros">Parametros com as infomações da ordem a ser cancelada.</param>
        /// <returns></returns>
        [OperationContract]
        CancelarOrdemResponse CancelarOrdem(CancelarOrdemRequest parametros);

        /// <summary>
        /// Recebe o detalhe de uma ordem
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        ReceberOrdemResponse ReceberOrdem(ReceberOrdemRequest parametros);

        /// <summary>
        /// Recebe lista de ordens de acordo com determinado filtro
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        ListarOrdensResponse ListarOrdens(ListarOrdensRequest parametros);

        /// <summary>
        /// Recebe lista de mensagens de acordo com os filtros
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        ListarMensagensResponse ListarMensagens(ListarMensagensRequest parametros);

        /// <summary>
        /// Pede lista de instrumentos para o canal.
        /// </summary>
        /// <param name="parametros">Mensagem de Requisição de Lista de Instrumentos</param>
        /// <returns></returns>
        [OperationContract]
        ListarInstrumentosResponse ListarInstrumentos(ListarInstrumentosRequest parametros);

        /// <summary>
        /// Faz a solicitação de sincronização para o canal.
        /// No caso dos canais fix, pede a retransmissão das mensagens e trata os ExecutionReports.
        /// </summary>
        /// <param name="parametros">Mensagem de solicitacao de sincronizacao</param>
        /// <returns></returns>
        [OperationContract]
        SincronizarCanalResponse SincronizarCanal(SincronizarCanalRequest parametros);

        /// <summary>
        /// Indica uma mensagem inválida recebida pelo canal
        /// </summary>
        /// <param name="parametros">Mensagem de Sinalização de Mensagem Inválida</param>
        [OperationContract]
        SinalizarMensagemInvalidaResponse SinalizarMensagemInvalida(SinalizarMensagemInvalidaRequest parametros);

        /// <summary>
        /// Função chamada pelo sistema de canais para indicar alteração de status de ordem ou alguma execução parcial.
        /// </summary>
        /// <param name="parametros">Mensagem de Sinalizar execução de ordem</param>
        [OperationContract]
        SinalizarExecucaoOrdemResponse SinalizarExecucaoOrdem(SinalizarExecucaoOrdemRequest parametros);

        /// <summary>
        /// Sinaliza que a mensagem de cancelamento de ordem foi rejeitada.
        /// </summary>
        /// <param name="parametros">Mensagem de Sinalização de Rejeição de Cancelamento de Ordem</param>
        [OperationContract]
        SinalizarRejeicaoCancelamentoOrdemResponse SinalizarRejeicaoCancelamentoOrdem(SinalizarRejeicaoCancelamentoOrdemRequest parametros);

        /// <summary>
        /// Faz a geração do código da mensagem.
        /// Gera um código utilizando uma string para indicar um grupo de mensagens, a hora, e 
        /// um sequencial para o caso de muitas mensagens ao mesmo tempo.
        /// A geração é nesse formato para evitar ter que persistir um número sequencial.
        /// </summary>
        /// <param name="grupo"></param>
        /// <returns></returns>
        [OperationContract]
        string GerarCodigoMensagem(string grupo);

        /// <summary>
        /// Função chamada pelo sistema de canais para informar a lista de instrumentos solicitada anteriormente.
        /// </summary>
        /// <param name="parametros">Mensagem com a lista de instrumentos</param>
        [OperationContract]
        SinalizarListaInstrumentosResponse SinalizarListaInstrumentos(SinalizarListaInstrumentosRequest parametros);

        /// <summary>
        /// Consulta de instrumentos pelo filtro informado
        /// </summary>
        /// <param name="securityID">SecurityID do instrumento</param>
        /// <returns>Informações do instrumento</returns>
        [OperationContract]
        ConsultarInstrumentosResponse ConsultarInstrumentos(ConsultarInstrumentosRequest parametros);

        /// <summary>
        /// Solicita a alteração de uma ordem
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [OperationContract]
        AlterarOrdemResponse AlterarOrdem(AlterarOrdemRequest parametros);

        /// <summary>
        /// Este método decide qual das operações deve chamar de acordo com o tipo da mensagem.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        MensagemResponseBase ProcessarMensagem(MensagemRequestBase parametros);

        /// <summary>
        /// Evento disparado em qualquer recebimento de mensagem de sinalização.
        /// Intercepta ListaInstrumentos, ExecucaoOrdem, MensagemInvalida, e futuras mensagens implementadas.
        /// </summary>
        event EventHandler<SinalizarEventArgs> EventoSinalizacao;

        /// <summary>
        /// Evento disparado por SinalizarListaInstrumentos para indicar a chegada da lista 
        /// de instrumentos através de algum canal.
        /// Por ser um evento, por enquanto esta operação não é suportada pelo WCF.
        /// </summary>
        event EventHandler<SinalizarListaInstrumentosEventArgs> EventoSinalizarListaInstrumentos;

        /// <summary>
        /// Sinaliza recebimento de mensagens inválidas. 
        /// </summary>
        event EventHandler<SinalizarMensagemInvalidaEventArgs> EventoSinalizarMensagemInvalida;

        /// <summary>
        /// Sinaliza recebimento rejeição de cancelamento de ordem. 
        /// </summary>
        event EventHandler<SinalizarRejeicaoCancelamentoOrdemEventArgs> EventoSinalizarRejeicaoCancelamentoOrdem;

        /// <summary>
        /// Sinaliza as execuções de ordens. Equivalente ao ExecutionReport do Fix.
        /// </summary>
        event EventHandler<SinalizarExecucaoOrdemEventArgs> EventoSinalizarExecucaoOrdem;
    }
}
