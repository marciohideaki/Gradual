using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Ordens
{
    /// <summary>
    /// Implementação do serviço de ordens servidor.
    /// Gerencia os pedidos de diversos clientes
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoOrdensServidor : IServicoOrdensServidor
    {
        /// <summary>
        /// Constante da mensagem de erro na etapa de validação
        /// </summary>
        private const string constDescricaoErroValidacao = 
            "Erro na etapa de validação. A propriedade Criticas contém a lista de críticas encontradas na validação.";

        #region Variaveis Locais

        /// <summary>
        /// Referencia ao servico de ordens
        /// </summary>
        private IServicoOrdens _servicoOrdens = null;
        
        /// <summary>
        /// Dicionario associando lista de sessões e callbacks
        /// </summary>
        private Dictionary<string, ICallbackEvento> _callbacks = new Dictionary<string, ICallbackEvento>();

        /// <summary>
        /// Referencia para o serviço de validação
        /// </summary>
        private IServicoValidacao _servicoValidacao = Ativador.Get<IServicoValidacao>();

        #endregion

        #region Construtor e Rotinas Locais

        /// <summary>
        /// Construtor. Pega referencia para o servico de ordens.
        /// Na sequencia de ativação de serviços, o serviço de ordens servidor tem que ser mencionado
        /// depois do serviço de ordens para que neste momento a referencia possa ser pega sem erros.
        /// </summary>
        public ServicoOrdensServidor()
        {
            // Mantem instancia para o servico de ordens
            _servicoOrdens = Ativador.Get<IServicoOrdens>();

            // Faz a assinatura dos eventos
            _servicoOrdens.EventoSinalizacao += new EventHandler<SinalizarEventArgs>(_servicoOrdens_EventoSinalizacao);
        }

        /// <summary>
        /// Evento de sinalização de alguma ocorrencia no serviço de ordens.
        /// Verifica se deve ser repassado para algum cliente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _servicoOrdens_EventoSinalizacao(object sender, SinalizarEventArgs e)
        {
            // Trata erros pois pode ocorrer erro na comunicação com o cliente
            try
            {
                // Verifica se tem sessao correspondente e envia a mensagem
                if (e.Mensagem.CodigoSessao != null)
                    if (_callbacks.ContainsKey(e.Mensagem.CodigoSessao))
                        _callbacks[e.Mensagem.CodigoSessao].SinalizarEvento(
                            new EventoInfo(e.Mensagem, EventoInfoSerializacaoTipoEnum.Binario));
            }
            catch (Exception ex)
            {
                // Se for erro de comunicacao, remove a sessao da lista
                Log.EfetuarLog(ex, "_servicoOrdens_EventoSinalizacao", ModulosOMS.ModuloOrdens);
            }
        }

        #endregion

        #region IServicoOrdensServidor Members

        /// <summary>
        /// Pede o processamento da mensagem. Repassa a mensagem para o serviço de ordens.
        /// </summary>
        /// <param name="mensagem"></param>
        /// <returns></returns>
        public MensagemResponseBase ProcessarMensagem(MensagemRequestBase mensagem)
        {
            // Inicializa
            MensagemResponseBase resposta = null;
            
            // Bloco de controle
            try
            {
                // Faz a validação da mensagem
                //ValidarMensagemResponse validacaoResponse =
                //    _servicoValidacao.ValidarMensagem(
                //        new ValidarMensagemRequest()
                //        {
                //            CodigoSessao = mensagem.CodigoSessao,
                //            Mensagem = mensagem
                //        });

                // Se está ok, executa
                //if (validacaoResponse.ContextoValidacao.MensagemValida)
                if (true)
                {
                    // Faz o pedido do processamento e retorna a resposta
                    resposta = _servicoOrdens.ProcessarMensagem(mensagem);
                }
                else
                {
                    // Cria a resposta relacionada e adiciona a lista de criticas da validação
                    object[] attrs = mensagem.GetType().GetCustomAttributes(typeof(MensagemAttribute), true);
                    if (attrs.Length > 0 && ((MensagemAttribute)attrs[0]).TipoMensagemResponse != null)
                    {
                        // Cria a mensagem de resposta
                        resposta = (MensagemResponseBase)Activator.CreateInstance(((MensagemAttribute)attrs[0]).TipoMensagemResponse);

                        // Preenche a resposta
                        resposta.CodigoMensagemRequest = mensagem.CodigoMensagem;
                        //resposta.Criticas = validacaoResponse.Criticas;
                        resposta.StatusResposta = MensagemResponseStatusEnum.ErroValidacao;
                        resposta.DescricaoResposta = constDescricaoErroValidacao;
                    }
                    else
                    {
                        // Se não encontrou o atributo que relaciona o request e o response, devolve uma resposta comum de mensagem não válida
                        resposta =
                            new MensagemErroValidacaoResponse()
                            {
                                CodigoMensagemRequest = mensagem.CodigoMensagem,
                                //Criticas = validacaoResponse.Criticas,
                                StatusResposta = MensagemResponseStatusEnum.ErroValidacao,
                                DescricaoResposta = constDescricaoErroValidacao
                            };
                    }
                }
            }
            catch (Exception ex)
            {
                // Faz o log
                Log.EfetuarLog(ex, mensagem, ModulosOMS.ModuloOrdens);

                // Monta resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            // Retorna
            return resposta;
        }

        #endregion

        #region IServicoComCallback Members

        /// <summary>
        /// Este método é disparado por um cliente WCF, pedindo o registro no servidor de ordens e
        /// passando as informações da sessão como parametros.
        /// </summary>
        /// <param name="parametros"></param>
        public void Registrar(object parametros)
        {
            // Inicializa
            SessaoOrdensInfo sessaoInfo = (SessaoOrdensInfo)parametros;

            // Recebe o objeto de callback
            ICallbackEvento callback = OperationContext.Current.GetCallbackChannel<ICallbackEvento>();

            // Adiciona na coleção
            _callbacks.Add(sessaoInfo.CodigoSessao, callback);
        }

        /// <summary>
        /// Este método é disparado por um cliente local, pedindo o registro no servidor de ordens
        /// e passando as informações da sessão como parametros.
        /// </summary>
        /// <param name="parametros"></param>
        /// <param name="callback"></param>
        public void Registrar(object parametros, ICallbackEvento callback)
        {
            // Inicializa
            SessaoOrdensInfo sessaoInfo = (SessaoOrdensInfo)parametros;

            // Adiciona na coleção
            _callbacks.Add(sessaoInfo.CodigoSessao, callback);
        }

        #endregion
    }
}
