using System;
using System.Collections.Generic;
using System.ServiceModel;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using log4net;

namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Implementação do serviço de mensageria com callback
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoMensageriaComCallback : IServicoMensageriaComCallback
    {
        #region Variáveis Locais

        /// <summary>
        /// Referencia para o serviço de mensageria
        /// </summary>
        private IServicoMensageria _servicoMensageria = Ativador.Get<IServicoMensageria>();

        /// <summary>
        /// Dicionário com os clientes ativos.
        /// A chave é o código da sessao do cliente.
        /// </summary>
        private Dictionary<string, ServicoMensageriaClienteHelper> _clientes = new Dictionary<string, ServicoMensageriaClienteHelper>();

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        

        #region IServicoMensageriaComCallback Members

        /// <summary>
        /// Processa a mensagem solicitada.
        /// Faz o roteamento da mensagem para o devido serviço
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase ProcessarMensagem(MensagemRequestBase parametros)
        {
            // Repassa a mensagem
            return _servicoMensageria.ProcessarMensagem(parametros);
        }

        /// <summary>
        /// Solicita a assinatura de um evento de serviço
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AssinarEventoResponse AssinarEvento(AssinarEventoRequest parametros)
        {
            // Solicita a assinatura para o helper da sessao
            AssinarEventoResponse resposta = 
                new AssinarEventoResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Bloco de controle
            try
            {
                // Faz a solicitação para o helper da sessao
                resposta = _clientes[parametros.CodigoSessao].AssinarEvento(parametros);
            }
            catch (Exception ex)
            {
                // Faz o log
                logger.Error(parametros, ex);

                // Informa na mensagem
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            // Retorna
            return resposta;
        }

        #endregion

        #region IServicoComCallback Members

        /// <summary>
        /// Registro pelo WCF
        /// </summary>
        /// <param name="parametros"></param>
        public void Registrar(object parametros)
        {
            // Inicializa
            SessaoInfo sessaoInfo = (SessaoInfo)parametros;

            // Recebe o objeto de callback
            ICallbackEvento callback = OperationContext.Current.GetCallbackChannel<ICallbackEvento>();

            // Adiciona na coleção
            _clientes.Add(
                sessaoInfo.CodigoSessao, 
                new ServicoMensageriaClienteHelper() 
                { 
                    Callback = callback,
                    Sessao = sessaoInfo
                });
        }

        /// <summary>
        /// Registro Local
        /// </summary>
        /// <param name="parametros"></param>
        /// <param name="callback"></param>
        public void Registrar(object parametros, ICallbackEvento callback)
        {
            // Inicializa
            SessaoInfo sessaoInfo = (SessaoInfo)parametros;

            // Adiciona na coleção
            _clientes.Add(
                sessaoInfo.CodigoSessao,
                new ServicoMensageriaClienteHelper()
                {
                    Callback = callback,
                    Sessao = sessaoInfo
                });
        }

        #endregion
    }
}
