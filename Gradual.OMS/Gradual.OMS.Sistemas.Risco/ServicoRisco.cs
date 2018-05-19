using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente;
using Gradual.OMS.Contratos.ContaCorrente.Dados;
using Gradual.OMS.Contratos.ContaCorrente.Mensagens;
using Gradual.OMS.Contratos.Custodia;
using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Custodia.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Risco
{
    /// <summary>
    /// Implementação do serviço de risco
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoRisco : IServicoRisco
    {
        #region Variáveis Locais

        /// <summary>
        /// Configurações do serviço de risco
        /// </summary>
        private ServicoRiscoConfig _config = GerenciadorConfig.ReceberConfig<ServicoRiscoConfig>();

        /// <summary>
        /// Lista de regras de risco disponíveis para cadastro
        /// </summary>
        private List<Type> _regrasDisponiveis = new List<Type>();

        /// <summary>
        /// Referencia para o servico de persistencia
        /// </summary>
        private IServicoRiscoPersistencia _servicoPersistencia = null;

        /// <summary>
        /// Regras que não tem código de cliente no grupo
        /// </summary>
        private List<RegraRiscoBase> _regras = new List<RegraRiscoBase>();

        /// <summary>
        /// Regras que tem código do cliente no grupo
        /// </summary>
        private Dictionary<string, List<RegraRiscoBase>> _regrasCliente =
            new Dictionary<string, List<RegraRiscoBase>>();

        /// <summary>
        /// Mantém o cache de custódias de clientes.
        /// A chave é o código do usuario
        /// </summary>
        private Dictionary<string, CacheRiscoInfo> _cacheRisco =
            new Dictionary<string, CacheRiscoInfo>();

        /// <summary>
        /// Timer de atualização de conta corrente
        /// </summary>
        private Timer _timerContaCorrente = null;

        /// <summary>
        /// Timer de atualização de custódia
        /// </summary>
        private Timer _timerCustodia = null;

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoRisco()
        {
        }

        #endregion

        #region Rotinas Locais

        /// <summary>
        /// Faz a inicialização do serviço
        /// </summary>
        private void inicializar()
        {
            // Bloco de controle
            try
            {
                // Pega referencia para o servico de persistencia
                _servicoPersistencia = Ativador.Get<IServicoRiscoPersistencia>();

                // Carrega lista de regras disponíveis
                foreach (string ns in _config.NamespacesRegras)
                {
                    // Inicializa e pega referencia ao assembly
                    string[] ns2 = ns.Split(',');
                    Assembly assembly = Assembly.Load(ns2[1].Trim());

                    // Varre adicionando os tipos de regra
                    foreach (Type tipo in assembly.GetTypes())
                        if (tipo.IsSubclassOf(typeof(RegraBase)))
                        {
                            object[] attrs = tipo.GetCustomAttributes(typeof(RegraAttribute), true);
                            if (attrs.Length > 0 && ((RegraAttribute)attrs[0]).RegraDeUsuario)
                                if (!_regrasDisponiveis.Contains(tipo))
                                    _regrasDisponiveis.Add(tipo);
                        }
                }

                // Carrega a árvore de regras
                carregarListaRegras();

                // Timer de atualização de conta corrente?
                if (_config.AtualizarContaCorrentePeriodicamente)
                    _timerContaCorrente =
                        new Timer(
                            timerContaCorrente,
                            null,
                            new TimeSpan(0, 0, _config.TempoAtualizacaoContaCorrente),
                            new TimeSpan(0, 0, _config.TempoAtualizacaoContaCorrente));

                // Timer de atualização de custódia?
                if (_config.AtualizarCustodiaPeriodicamente)
                    _timerCustodia =
                        new Timer(
                            timerCustodia,
                            null,
                            new TimeSpan(0, 0, _config.TempoAtualizacaoCustodia),
                            new TimeSpan(0, 0, _config.TempoAtualizacaoCustodia));
            }
            catch (Exception ex)
            {
                // Faz o log
                Log.EfetuarLog(ex, "ServicoRisco.Inicializar", ModulosOMS.ModuloRisco);

                // Repassa o erro
                throw (ex);
            }
        }

        /// <summary>
        /// Callback do timer de atualização de conta corrente
        /// </summary>
        /// <param name="param"></param>
        private void timerContaCorrente(object param)
        {
            // Inicializa
            IServicoContaCorrente servicoContaCorrente = Ativador.Get<IServicoContaCorrente>();

            // Pega a lista de usuários a ser atualizada
            IEnumerable<string> listaUsuarios =
                from c in _cacheRisco
                select c.Key;

            // Varre atualizando as informações
            foreach (string usuario in listaUsuarios)
            {
                // Pega informacoes da contacorrente
                ContaCorrenteInfo contaCorrenteAtual =
                    _cacheRisco[usuario].ContaCorrente;

                // Carrega conta corrente do cliente
                // TODO: Esta chamada precisa passar um código de sessão para passar na validação. Precisa criar uma forma de usuário de sistema poder fazer a chamada
                ContaCorrenteInfo contaCorrente =
                    ((ReceberContaCorrenteResponse)
                        servicoContaCorrente.ReceberContaCorrente(
                            new ReceberContaCorrenteRequest()
                            {
                                //CodigoSessao = parametros.CodigoSessao,
                                CodigoContaCorrente = contaCorrenteAtual.CodigoContaCorrente
                            })).ContaCorrenteInfo;

                // Atualiza o cache
                lock(_cacheRisco)
                    _cacheRisco[usuario].ContaCorrente = contaCorrente;
            }
        }

        /// <summary>
        /// Callback do timer de atualização de custódia
        /// </summary>
        /// <param name="param"></param>
        private void timerCustodia(object param)
        {
            // Inicializa
            IServicoCustodia servicoCustodia = Ativador.Get<IServicoCustodia>();
            
            // Pega a lista de usuários a ser atualizada
            IEnumerable<string> listaUsuarios =
                from c in _cacheRisco
                select c.Key;

            // Varre atualizando as informações
            foreach (string usuario in listaUsuarios)
            {
                // Codigo da custodia
                string codigoCustodia = _cacheRisco[usuario].Custodia.CodigoCustodia;

                // Carrega a custódia do cliente
                CustodiaInfo custodia =
                    servicoCustodia.ReceberCustodia(
                        new ReceberCustodiaRequest()
                        {
                            //CodigoSessao = parametros.CodigoSessao,
                            CodigoCustodia = codigoCustodia
                        }).CustodiaInfo;

                // Atualiza o cache
                lock(_cacheRisco)
                    _cacheRisco[usuario].Custodia = custodia;
            }
        }

        /// <summary>
        /// Faz a carga inicial da lista de regras do risco
        /// </summary>
        private void carregarListaRegras()
        {
            // Lista as regras habilitadas
            List<RegraRiscoInfo> regrasInfo = 
                _servicoPersistencia.ListarRegraRisco(
                    new ListarRegraRiscoRequest()).Resultado;

            // Exclui as regras atuais
            _regrasCliente.Clear();
            _regras.Clear();

            // Varre criando a regra e adicionando no local correto (de usuário ou não)
            foreach (RegraRiscoInfo regraRiscoInfo in regrasInfo)
            {
                // Cria a regra
                RegraRiscoBase regra = 
                    (RegraRiscoBase)
                        Activator.CreateInstance(
                            regraRiscoInfo.TipoRegra, 
                            new object[] 
                            { 
                                regraRiscoInfo
                            });

                // É de cliente ou não?
                if (regraRiscoInfo.Agrupamento.CodigoUsuario != null)
                {
                    // É regra de cliente, garante que a entrada do cliente consta no dicionário
                    if (!_regrasCliente.ContainsKey(regraRiscoInfo.Agrupamento.CodigoUsuario))
                        _regrasCliente.Add(
                            regraRiscoInfo.Agrupamento.CodigoUsuario, new List<RegraRiscoBase>());

                    // Adiciona a regra
                    _regrasCliente[regraRiscoInfo.Agrupamento.CodigoUsuario].Add(regra);
                }
                else
                {
                    // Não é regra de cliente, adiciona a regra
                    _regras.Add(regra);
                }
            }
        }

        #endregion

        #region IServicoRisco Members

        #region Validação de Risco

        /// <summary>
        /// Executa a validação da operação sem necessariamente executar a operação.
        /// É apenas uma rechamada para o serviço de validação
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ValidarOperacaoResponse ValidarOperacao(ValidarOperacaoRequest parametros)
        {
            // Faz a validação da mensagem
            IServicoValidacao servicoValidacao = Ativador.Get<IServicoValidacao>();
            ValidarMensagemResponse validacaoResponse =
                servicoValidacao.ValidarMensagem(
                    new ValidarMensagemRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        Mensagem = parametros.Mensagem
                    });

            // Retorna
            return 
                new ValidarOperacaoResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    Validacao = validacaoResponse
                };
        }

        #endregion

        #region RegraRisco

        /// <summary>
        /// Retorna a lista de regras disponíveis para serem utilizadas
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarRegrasDisponiveisResponse ListarRegrasDisponiveis(ListarRegrasDisponiveisResponse parametros)
        {
            // Inicializa
            ListarRegrasDisponiveisResponse resposta =
                new ListarRegrasDisponiveisResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Varre a lista de regras disponiveis criando o RegraInfo
            foreach (Type tipo in _regrasDisponiveis)
                resposta.Regras.Add(
                    new RegraInfo()
                    {
                        TipoRegra = tipo
                    });

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Lista regras de risco de acordo com o filtro informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarRegraRiscoResponse ListarRegraRisco(ListarRegraRiscoRequest parametros)
        {
            // Repassa para o serviço de persistencia
            return _servicoPersistencia.ListarRegraRisco(parametros);
        }

        /// <summary>
        /// Recebe a regra de risco solicitada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberRegraRiscoResponse ReceberRegraRisco(ReceberRegraRiscoRequest parametros)
        {
            // Repassa para o serviço de persistencia
            return _servicoPersistencia.ReceberRegraRisco(parametros);
        }

        /// <summary>
        /// Remove a regra de risco informada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverRegraRiscoResponse RemoverRegraRisco(RemoverRegraRiscoRequest parametros)
        {
            // Repassa para o serviço de persistencia
            return _servicoPersistencia.RemoverRegraRisco(parametros);
        }

        /// <summary>
        /// Salva a regra de risco informada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarRegraRiscoResponse SalvarRegraRisco(SalvarRegraRiscoRequest parametros)
        {
            // Repassa para o serviço de persistencia
            return _servicoPersistencia.SalvarRegraRisco(parametros);
        }

        /// <summary>
        /// Consulta as regras a serem executadas para um determinado 
        /// agrupamento através da árvore de regras.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ConsultarRegrasDoAgrupamentoResponse ConsultarRegrasDoAgrupamento(ConsultarRegrasDoAgrupamentoRequest parametros)
        {
            // Prepara o retorno
            ConsultarRegrasDoAgrupamentoResponse resposta =
                new ConsultarRegrasDoAgrupamentoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Faz a consulta na lista de regras geral
            resposta.Regras.AddRange(
                from r in _regras
                where 
                     r.RegraInfo2.Habilitado &&
                    (r.RegraInfo2.Agrupamento.CodigoAtivo == null || 
                     r.RegraInfo2.Agrupamento.CodigoAtivo == parametros.Agrupamento.CodigoAtivo) &&
                    (r.RegraInfo2.Agrupamento.CodigoAtivoBase == null ||
                     parametros.Agrupamento.CodigoAtivo.StartsWith(r.RegraInfo2.Agrupamento.CodigoAtivoBase)) &&
                    (r.RegraInfo2.Agrupamento.CodigoBolsa == null ||
                     r.RegraInfo2.Agrupamento.CodigoBolsa == parametros.Agrupamento.CodigoBolsa) &&
                    (r.RegraInfo2.Agrupamento.CodigoPerfilRisco == null ||
                     r.RegraInfo2.Agrupamento.CodigoPerfilRisco == parametros.Agrupamento.CodigoPerfilRisco) &&
                    (r.RegraInfo2.Agrupamento.CodigoSistemaCliente == null ||
                     r.RegraInfo2.Agrupamento.CodigoSistemaCliente == parametros.Agrupamento.CodigoSistemaCliente)
                select (RegraBase)r
            );

            // Se tem cliente, faz a consulta na lista de regras do cliente
            if (parametros.Agrupamento.CodigoUsuario != null && _regrasCliente.ContainsKey(parametros.Agrupamento.CodigoUsuario))
                resposta.Regras.AddRange(
                    from r in _regrasCliente[parametros.Agrupamento.CodigoUsuario]
                    where
                         r.RegraInfo2.Habilitado &&
                        (r.RegraInfo2.Agrupamento.CodigoAtivo == null ||
                         r.RegraInfo2.Agrupamento.CodigoAtivo == parametros.Agrupamento.CodigoAtivo) &&
                        (r.RegraInfo2.Agrupamento.CodigoAtivoBase == null ||
                         parametros.Agrupamento.CodigoAtivo.StartsWith(r.RegraInfo2.Agrupamento.CodigoAtivoBase)) &&
                        (r.RegraInfo2.Agrupamento.CodigoBolsa == null ||
                         r.RegraInfo2.Agrupamento.CodigoBolsa == parametros.Agrupamento.CodigoBolsa) &&
                        (r.RegraInfo2.Agrupamento.CodigoPerfilRisco == null ||
                         r.RegraInfo2.Agrupamento.CodigoPerfilRisco == parametros.Agrupamento.CodigoPerfilRisco) &&
                        (r.RegraInfo2.Agrupamento.CodigoSistemaCliente == null ||
                         r.RegraInfo2.Agrupamento.CodigoSistemaCliente == parametros.Agrupamento.CodigoSistemaCliente)
                    select (RegraBase)r
                );

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita que o risco recarregue a árvore de regras
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RecarregarListaRegrasResponse RecarregarListaRegras(RecarregarListaRegrasRequest parametros)
        {
            // Solicita recarga
            carregarListaRegras();

            // Retorna
            return
                new RecarregarListaRegrasResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        #endregion

        #region Clientes e Custódia

        /// <summary>
        /// Pede inicialização do cliente.
        /// Neste momento a custódia do cliente é carregada no risco e armazenada em cache
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public InicializarClienteResponse InicializarCliente(InicializarClienteRequest parametros)
        {
            // Pega informações de usuário do cliente
            IServicoSeguranca servicoSeguranca = Ativador.Get<IServicoSeguranca>();
            UsuarioInfo usuarioInfo =
                servicoSeguranca.ReceberUsuario(
                    new ReceberUsuarioRequest() 
                    { 
                        CodigoSessao = parametros.CodigoSessao,
                        CodigoUsuario = parametros.CodigoUsuario
                    }).Usuario;
            
            // Pega o contexto do usuário
            ContextoOMSInfo contextoOMS = 
                usuarioInfo.Complementos.ReceberItem<ContextoOMSInfo>();

            // Carrega conta corrente do cliente
            IServicoContaCorrente servicoContaCorrente = Ativador.Get<IServicoContaCorrente>();
            ContaCorrenteInfo contaCorrente =
                ((ReceberContaCorrenteResponse)
                    servicoContaCorrente.ReceberContaCorrente(
                        new ReceberContaCorrenteRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoContaCorrente = contextoOMS.CodigoContaCorrente
                        })).ContaCorrenteInfo;

            // Carrega a custódia do cliente
            IServicoCustodia servicoCustodia = Ativador.Get<IServicoCustodia>();
            CustodiaInfo custodia = 
                servicoCustodia.ReceberCustodia(
                    new ReceberCustodiaRequest() 
                    { 
                        CodigoSessao = parametros.CodigoSessao, 
                        CodigoCustodia = contextoOMS.CodigoCustodia, 
                        CarregarCotacoes = true
                    }).CustodiaInfo;

            // Cria novo objeto de cache
            CacheRiscoInfo cache = 
                new CacheRiscoInfo() 
                { 
                    ContaCorrente = contaCorrente,
                    Custodia = custodia,
                    Usuario = usuarioInfo
                };
            
            // Adiciona no cache
            if (!_cacheRisco.ContainsKey(parametros.CodigoUsuario))
                _cacheRisco.Add(parametros.CodigoUsuario, cache);
            else
                _cacheRisco[parametros.CodigoUsuario] = cache;

            // Retorna
            return 
                new InicializarClienteResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Pede liberação do cliente.
        /// Neste momento o cache de custódia do cliente é liberado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public LiberarClienteResponse LiberarCliente(LiberarClienteRequest parametros)
        {
            // Remove o item de cache de custodia do cliente
            if (_cacheRisco.ContainsKey(parametros.CodigoUsuario))
                _cacheRisco.Remove(parametros.CodigoUsuario);

            // Retorna
            return 
                new LiberarClienteResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Solicita os caches de risco do cliente.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberCacheRiscoResponse ReceberCacheRisco(ReceberCacheRiscoRequest parametros)
        {
            // Prepara resposta
            ReceberCacheRiscoResponse resposta = 
                new ReceberCacheRiscoResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Custódia está no cache?
            if (!_cacheRisco.ContainsKey(parametros.CodigoUsuario))
                this.InicializarCliente(
                    new InicializarClienteRequest() 
                    { 
                        CodigoUsuario = parametros.CodigoUsuario,
                        CodigoSessao = parametros.CodigoSessao
                    });

            // Se tem, informa o cache na resposta
            if (_cacheRisco.ContainsKey(parametros.CodigoUsuario))
                resposta.CacheRisco = _cacheRisco[parametros.CodigoUsuario];

            // Retorna
            return resposta;
        }

        #endregion

        #region Perfil de Risco

        /// <summary>
        /// Solicita lista de perfis de risco
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarPerfisRiscoResponse ListarPerfisRisco(ListarPerfisRiscoRequest parametros)
        {
            // Prepara retorno
            ListarPerfisRiscoResponse resposta = 
                new ListarPerfisRiscoResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Inicializa
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();

            // Faz a consulta
            resposta.Resultado =
                servicoPersistencia.ConsultarObjetos<PerfilRiscoInfo>(
                    new ConsultarObjetosRequest<PerfilRiscoInfo>() 
                    { 
                        CodigoSessao = parametros.CodigoSessao
                    }).Resultado;

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita detalhe de um perfil de risco
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberPerfilRiscoResponse ReceberPerfilRisco(ReceberPerfilRiscoRequest parametros)
        {
            // Prepara retorno
            ReceberPerfilRiscoResponse resposta =
                new ReceberPerfilRiscoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Inicializa
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();

            // Faz a consulta
            resposta.PerfilRiscoInfo =
                servicoPersistencia.ReceberObjeto<PerfilRiscoInfo>(
                    new ReceberObjetoRequest<PerfilRiscoInfo>()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        CodigoObjeto = parametros.CodigoPerfilRisco
                    }).Objeto;

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Remove um perfil de risco
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverPerfilRiscoResponse RemoverPerfilRisco(RemoverPerfilRiscoRequest parametros)
        {
            // Prepara retorno
            RemoverPerfilRiscoResponse resposta =
                new RemoverPerfilRiscoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Inicializa
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();

            // Faz a consulta
            servicoPersistencia.RemoverObjeto<PerfilRiscoInfo>(
                new RemoverObjetoRequest<PerfilRiscoInfo>()
                {
                    CodigoSessao = parametros.CodigoSessao,
                    CodigoObjeto = parametros.CodigoPerfilRisco
                });

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Salva um perfil de risco
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarPerfilRiscoResponse SalvarPerfilRisco(SalvarPerfilRiscoRequest parametros)
        {
            // Prepara retorno
            SalvarPerfilRiscoResponse resposta =
                new SalvarPerfilRiscoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Inicializa
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();

            // Faz a consulta
            SalvarObjetoResponse<PerfilRiscoInfo> respostaSalvar =
                servicoPersistencia.SalvarObjeto<PerfilRiscoInfo>(
                    new SalvarObjetoRequest<PerfilRiscoInfo>()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        Objeto = parametros.PerfilRiscoInfo
                    });

            // Inclui perfil salvo na resposta
            resposta.PerfilRisco = respostaSalvar.Objeto;

            // Retorna
            return resposta;
        }

        #endregion

        #region Ticket de Risco

        /// <summary>
        /// Lista tickets de risco de acordo com o filtro informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarTicketsRiscoResponse ListarTicketsRisco(ListarTicketsRiscoRequest parametros)
        {
            // Solicita a lista
            ConsultarObjetosResponse<TicketRiscoInfo> retorno =
                Ativador.Get<IServicoPersistencia>().ConsultarObjetos<TicketRiscoInfo>(
                    new ConsultarObjetosRequest<TicketRiscoInfo>()
                    {
                    });

            // Retorna
            return
                new ListarTicketsRiscoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    Resultado = retorno.Resultado
                };
        }

        /// <summary>
        /// Recebe o ticket de risco solicitado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberTicketRiscoResponse ReceberTicketRisco(ReceberTicketRiscoRequest parametros)
        {
            // Retorna o objeto solicitado
            return
                new ReceberTicketRiscoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    TicketRiscoInfo =
                        Ativador.Get<IServicoPersistencia>().ReceberObjeto<TicketRiscoInfo>(
                            new ReceberObjetoRequest<TicketRiscoInfo>()
                            {
                                CodigoObjeto = parametros.CodigoTicketRisco
                            }).Objeto
                };
        }

        /// <summary>
        /// Remove o ticket de risco informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverTicketRiscoResponse RemoverTicketRisco(RemoverTicketRiscoRequest parametros)
        {
            // Remove
            Ativador.Get<IServicoPersistencia>().RemoverObjeto<TicketRiscoInfo>(
                new RemoverObjetoRequest<TicketRiscoInfo>()
                {
                    CodigoObjeto = parametros.CodigoTicketRisco
                });

            // Retorna
            return
                new RemoverTicketRiscoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Salva o ticket de risco informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarTicketRiscoResponse SalvarTicketRisco(SalvarTicketRiscoRequest parametros)
        {
            // Salva
            Ativador.Get<IServicoPersistencia>().SalvarObjeto<TicketRiscoInfo>(
                new SalvarObjetoRequest<TicketRiscoInfo>()
                {
                    Objeto = parametros.TicketRiscoInfo
                });

            // Retorna
            return
                new SalvarTicketRiscoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        #endregion

        #region Conta Corrente

        /// <summary>
        /// Solicita ao risco o processamento da operação
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ProcessarOperacaoResponse ProcessarOperacao(ProcessarOperacaoRequest parametros)
        {
            // Se usuário não está no cache, pede inicialização
            if (!_cacheRisco.ContainsKey(parametros.CodigoUsuario))
                this.InicializarCliente(
                    new InicializarClienteRequest() 
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        CodigoUsuario = parametros.CodigoUsuario
                    });

            // Pega o cache
            CacheRiscoInfo cacheRisco = _cacheRisco[parametros.CodigoUsuario];

            // Contabiliza a operação na conta corrente de acordo com o evento
            switch (parametros.TipoEvento)
            {
                case ProcessarOperacaoTipoEventoEnum.Provisionar:
                case ProcessarOperacaoTipoEventoEnum.Reservar:
                case ProcessarOperacaoTipoEventoEnum.Alterar:

                    // Verifica se tem a parcela criada
                    SaldoBloqueadoParcelaInfo parcela = 
                        cacheRisco.ContaCorrente.SaldoBloqueadoComposicao.Find(p => p.CodigoReferencia == parametros.CodigoReferenciaOperacao);
                    if (parcela == null)
                    {
                        // Cria parcela
                        parcela =
                            new SaldoBloqueadoParcelaInfo() 
                            { 
                                CodigoReferencia = parametros.CodigoReferenciaOperacao,
                                DataInsercao = DateTime.Now,
                            };

                        // Adiciona na coleção
                        cacheRisco.ContaCorrente.SaldoBloqueadoComposicao.Add(parcela);
                    }

                    // Atualiza parcela
                    parcela.TipoParcela = SaldoBloqueadoParcelaTipoEnum.Permanente;
                    parcela.DataUltimaAtualizacao = DateTime.Now;
                    parcela.TipoParcela = SaldoBloqueadoParcelaTipoEnum.Temporario;
                    if (parametros.ValorOperacao.HasValue)
                        parcela.ValorParcelaBloqueio = parametros.ValorOperacao.Value;
                    
                    break;
                case ProcessarOperacaoTipoEventoEnum.Remover:

                    // Verifica se tem a parcela criada
                    SaldoBloqueadoParcelaInfo parcelaRemover =
                        cacheRisco.ContaCorrente.SaldoBloqueadoComposicao.Find(p => p.CodigoReferencia == parametros.CodigoReferenciaOperacao);
                    cacheRisco.ContaCorrente.SaldoBloqueadoComposicao.Remove(parcelaRemover);
                    
                    break;
            }
            
            // Retorna
            return 
                new ProcessarOperacaoResponse() 
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Solicita a sincronizacao de uma conta corrente com o sinacor
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SincronizarContaCorrenteSinacorResponse SincronizarContaCorrenteSinacor(SincronizarContaCorrenteSinacorRequest parametros)
        {
            // Prepara resposta
            SincronizarContaCorrenteSinacorResponse resposta =
                new SincronizarContaCorrenteSinacorResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Bloco de controle
            try
            {
                // Inicializa
                IServicoIntegracaoSinacorOMS servicoIntegracao = Ativador.Get<IServicoIntegracaoSinacorOMS>();
                IServicoSeguranca servicoSeguranca = Ativador.Get<IServicoSeguranca>();
                ContaCorrenteInfo contaCorrente = parametros.ContaCorrente;
                UsuarioInfo usuario = parametros.Usuario;

                // Se ainda não tem, chega no usuario
                if (usuario == null)
                    usuario =
                        servicoSeguranca.ReceberUsuario(
                            new ReceberUsuarioRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoUsuario = parametros.CodigoUsuario
                            }).Usuario;

                // Se ainda não tem, carrega a conta corrente
                if (contaCorrente == null)
                    contaCorrente =
                        Ativador.Get<IServicoContaCorrente>().ReceberContaCorrente(
                            new ReceberContaCorrenteRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoContaCorrente = usuario.Complementos.ReceberItem<ContextoOMSInfo>().CodigoContaCorrente
                            }).ContaCorrenteInfo;

                // Deve atualizar conta corrente ou conta investimento?
                if (parametros.SincronizarContaCorrente || parametros.SincronizarContaInvestimento)
                {
                    // Faz o pedido de atualizacao
                    servicoIntegracao.SincronizarContaCorrente(
                        new SincronizarContaCorrenteRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            ContaCorrente = contaCorrente,
                            Usuario = usuario,
                            SincronizarContaCorrente = parametros.SincronizarContaCorrente,
                            SincronizarContaInvestimento = parametros.SincronizarContaInvestimento,
                            SalvarEntidades = false
                        });
                }

                // Deve atualizar conta margem?

                // Atualiza o saldo bloqueado desconsiderando operações que já vieram do sinacor

            }
            catch (Exception ex)
            {
                // Faz o log
                Log.EfetuarLog(ex);

                // Informa na mensagem
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            // Retorna
            return resposta;
        }

        #endregion

        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            // Faz a inicialização do serviço
            inicializar();
        }

        public void PararServico()
        {
        }

        public ServicoStatus ReceberStatusServico()
        {
            return ServicoStatus.Indefinido;
        }

        #endregion
    }
}
