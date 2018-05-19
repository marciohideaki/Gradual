using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Implementação do serviço de mensageria
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoMensageria : IServicoMensageria
    {
        #region Variáveis Locais

        /// <summary>
        /// Constante da mensagem de erro na etapa de validação
        /// </summary>
        private const string constDescricaoErroValidacao =
            "Erro na etapa de validação. A propriedade Criticas contém a lista de críticas encontradas na validação.";

        /// <summary>
        /// Configurações do serviço
        /// </summary>
        private ServicoMensageriaConfig _config = GerenciadorConfig.ReceberConfig<ServicoMensageriaConfig>();

        /// <summary>
        /// Lista de tipos associados aos métodos de execução
        /// </summary>
        private Dictionary<Type, MethodInfo> _tiposConhecidos = new Dictionary<Type, MethodInfo>();

        /// <summary>
        /// Referencia para o serviço de validação
        /// </summary>
        private IServicoValidacao _servicoValidacao = Ativador.Get<IServicoValidacao>();

        private static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoMensageria()
        {
            // Inicializa a lista de tipos conhecidos
            inicializarListaTipos();
        }

        /// <summary>
        /// Inicializa a lista com os tipos conhecidos
        /// </summary>
        private void inicializarListaTipos()
        {
            // Lista de tipos a serem processados
            List<Type> tipos = new List<Type>();

            // Adiciona namespaces
            foreach (string nsx in _config.IncluirNamespaces)
            {
                // Pega o assembly
                string[] ns = nsx.Split(',');
                string ns0 = ns[0].Trim();
                Assembly assembly = Assembly.Load(ns[1].Trim());

                // Faz o filtro e inclui
                tipos.AddRange(
                    from t in assembly.GetTypes()
                    where (t.Namespace == ns0 ||
                           (_config.AprofundarNamespaces && t.Namespace.StartsWith(ns0)))
                           && !t.IsGenericType
                           && t.IsInterface
                    select t);
            }

            // Adiciona tipos
            foreach (string tipoStr in _config.IncluirTipos)
            {
                // Pega o tipo
                Type tipo = Type.GetType(tipoStr);

                // Inclui se for interface
                if (tipo!=null && tipo.IsInterface)
                    tipos.Add(tipo);
            }

            // Exclui tipos
            foreach (string tipoStr in _config.ExcluirTipos)
            {
                // Pega o tipo
                Type tipo = Type.GetType(tipoStr);

                // Exclui
                if (tipos.Contains(tipo))
                    tipos.Remove(tipo);
            }

            // Processa os tipos encontrados
            foreach (Type tipo in tipos)
            {
                // Varre os métodos 
                foreach (MethodInfo method in tipo.GetMethods())
                {
                    // Verifica se tem 1 parametro e é do tipo MensagemRequestBase. Se positivo adiciona na coleção
                    // Caso tenha mais de 1, aqui irá ocorrer um erro
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == 1)
                    {
                        Type tipoParametro = parameters[0].ParameterType;
                        if (tipoParametro.IsSubclassOf(typeof(MensagemRequestBase)))
                        {
                            if (!_tiposConhecidos.ContainsKey(tipoParametro))
                                _tiposConhecidos.Add(tipoParametro, method);
                            else
                                throw new Exception(
                                    string.Format(
                                        "Mais de um método de serviço ({0}, {1}) processam o mesmo tipo de mensagem ({2}). O serviço de mensageria não consegue rotear esse tipo de mensagem. Ajuste as configurações para não ocorrer essa situação.",
                                        _tiposConhecidos[tipoParametro].ReflectedType.Name + "." + _tiposConhecidos[tipoParametro].Name, 
                                        method.ReflectedType.Name + "." + method.Name, 
                                        tipoParametro.Name));
                        }
                    }
                }
            }
        }

        #endregion

        #region IServicoMensageria Members

        /// <summary>
        /// Processa a mensagem solicitada.
        /// Faz o roteamento da mensagem para o devido serviço
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase ProcessarMensagem(MensagemRequestBase parametros)
        {
            // Prepara resposta
            MensagemResponseBase resposta = null;

            // Bloco de controle
            try
            {
                // Passa a mensagem pela validação
                ValidarMensagemResponse respostaValidacao =
                    _servicoValidacao.ValidarMensagem(
                        new ValidarMensagemRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            Mensagem = parametros
                        });

                // Verifica o retorno da validação
                if (respostaValidacao.ContextoValidacao.MensagemValida)
                {
                    // Tipo da mensagem
                    Type tipoMensagem = parametros.GetType();

                    // Acha o método que processa este tipo de mensagem
                    MethodInfo metodo = _tiposConhecidos[tipoMensagem];

                    // Tipo do servico
                    Type tipoServico = metodo.ReflectedType;

                    // Pega instancia do servico
                    object servico = Ativador.Get(tipoServico);

                    // Executa
                    resposta = (MensagemResponseBase)metodo.Invoke(servico, new object[] { parametros });

                    // Faz o log da mensagem
                    logger.Info("ServicoMensageria.ProcessarMensagem: " + Serializador.TransformarEmString(parametros));
                }
                else
                {
                    // Cria resposta
                    resposta = criarResponsePadrao(parametros.GetType());

                    // Preenche a resposta
                    resposta.CodigoMensagemRequest = parametros.CodigoMensagem;
                    resposta.Criticas = respostaValidacao.Criticas;
                    resposta.StatusResposta = MensagemResponseStatusEnum.ErroValidacao;
                    resposta.DescricaoResposta = constDescricaoErroValidacao;

                    // Faz o log
                    logger.Error("ServicoMensageria.ProcessarMensagem (Erro de Validação): " + Serializador.TransformarEmString(respostaValidacao) );
                }
            }
            catch (Exception ex)
            {
                // Faz o log
                logger.Error("Erro ao processar mensagem: " + parametros.CodigoMensagem, ex);
                
                // Cria resposta
                resposta = criarResponsePadrao(parametros.GetType());

                // Preenche a resposta
                resposta.CodigoMensagemRequest = parametros.CodigoMensagem;
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.ToString();
            }

            // Retorna
            return resposta;
        }

        #endregion

        #region Rotinas Locais

        /// <summary>
        /// Para o tipo do request informado, acha o tipo do response
        /// </summary>
        /// <param name="tipoRequest"></param>
        /// <returns></returns>
        private MensagemResponseBase criarResponsePadrao(Type tipoRequest)
        {
            // Prepara resposta
            MensagemResponseBase resposta = null;

            // Cria a resposta relacionada e adiciona a lista de criticas da validação
            object[] attrs = tipoRequest.GetCustomAttributes(typeof(MensagemAttribute), true);
            if (attrs.Length > 0 && ((MensagemAttribute)attrs[0]).TipoMensagemResponse != null)
                resposta = (MensagemResponseBase)Activator.CreateInstance(((MensagemAttribute)attrs[0]).TipoMensagemResponse);
            else
                resposta = new MensagemErroValidacaoResponse();

            // Retorna
            return resposta;
        }

        #endregion
    }
}
