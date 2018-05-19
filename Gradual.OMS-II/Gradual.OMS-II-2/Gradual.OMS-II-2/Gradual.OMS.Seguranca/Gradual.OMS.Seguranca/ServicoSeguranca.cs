using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Timers;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Persistencias.Seguranca.Entidades;
using Gradual.OMS.Seguranca.Lib;
using System.Data.SqlClient;
using log4net;
using Gradual.OMS.SegurancaADM.Lib;
using Gradual.OMS.SegurancaADM.Lib.Mensagens;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Gradual.OMS.Seguranca
{
    /// <summary>
    /// Implementação do serviço para controle de segurança
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoSeguranca : IServicoSeguranca, IServicoControlavel, IServicoSegurancaADM
    {
        #region | Atributos

        /// <summary>
        /// Referencia para a classe de configurações
        /// </summary>
        private ServicoSegurancaConfig _config = GerenciadorConfig.ReceberConfig<ServicoSegurancaConfig>();

        /// <summary>
        /// Referencia para o serviço de persistencia da segurança
        /// </summary>
        private IServicoSegurancaPersistencia _servicoPersistencia = Ativador.Get<IServicoSegurancaPersistencia>();

        /// <summary>
        /// Coleção com as permissões do sistema
        /// </summary>
        private ListaPermissoesHelper _permissoes = new ListaPermissoesHelper();

        /// <summary>
        /// Lista de sessoes abertas
        /// </summary>
        private Dictionary<string, Sessao> _sessoes = new Dictionary<string, Sessao>();

        /// <summary>
        /// Dicionário com os complementos de autenticação
        /// </summary>
        private Dictionary<ComplementoAutenticacaoInfo, IComplementoAutenticacao> _complementosAutenticacao = new Dictionary<ComplementoAutenticacaoInfo, IComplementoAutenticacao>();

        private Timer tcker;
        private Timer tckSessoes;

        private ServicoStatus statusServico;

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region | Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoSeguranca()
        {   //--> Cria a lista de permissões

            if (_config != null)
            {   //--> Carrega os complementos
                IComplementoAutenticacao complementoInstancia;

                foreach (ComplementoAutenticacaoInfo complementoInfo in _config.ComplementosAutenticacao)
                {   //--> Habilitado?
                    if (complementoInfo.Habilitado)
                    {   //--> Cria instancia
                        complementoInstancia = (IComplementoAutenticacao)Activator.CreateInstance(complementoInfo.TipoComplemento);

                        _complementosAutenticacao.Add(complementoInfo, complementoInstancia); //--> Adiciona no dicionário
                    }
                }

                if (_config.InicializarAutomaticamente) //--> Verifica se deve inicializar segurança no início
                    this.InicializarSeguranca(new MensagemRequestBase());
            }
        }

        #endregion

        #region | IServicoSeguranca Members

        #region Comum

        /// <summary>
        /// Inicializa o serviço de segurança.
        /// Garante a existencia do usuário administrador
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase InicializarSeguranca(MensagemRequestBase parametros)
        {
            /*
            // Caso não exista, cria o usuário administrador
            UsuarioInfo usuarioInfo = _servicoPersistencia.ReceberUsuario(
                new ReceberUsuarioRequest()
                {
                    CodigoUsuario = _config.NomeUsuarioAdministrador
                }).Usuario;

            // Se não encontrou, cria
            if (usuarioInfo == null)
            {
                // Cria usuario
                UsuarioInfo usuarioAdmin =
                    new UsuarioInfo()
                    {
                        CodigoUsuario = _config.NomeUsuarioAdministrador,
                        Senha = _config.SenhaUsuarioAdministrador,
                        Email = _config.NomeUsuarioAdministrador,
                        Nome = "Adm",
                        NomeAbreviado = "Adm"
                    };

                // Adiciona permissao de admin
                usuarioAdmin.Permissoes.Add(
                    new PermissaoAssociadaInfo()
                    {
                        CodigoPermissao = _permissoes.ListaPorTipo[typeof(PermissaoAdministrador)].PermissaoInfo.CodigoPermissao,
                        Status = PermissaoAssociadaStatusEnum.Permitido
                    });

                // Salva
                _servicoPersistencia.SalvarUsuario(
                    new SalvarUsuarioRequest()
                    {
                        Usuario = usuarioAdmin
                    });
            }
            */

            return new MensagemResponseBase()
            {
                CodigoMensagemRequest = parametros.CodigoMensagem
            };
        }

        #endregion

        #region Autenticacao

        /// <summary>
        /// Solicita autenticação do usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AutenticarUsuarioResponse AutenticarUsuario(AutenticarUsuarioRequest parametros)
        {
            AutenticarUsuarioDinamicoRequest lRequest = new AutenticarUsuarioDinamicoRequest();
            lRequest.CodigoMensagem         = parametros.CodigoMensagem;
            lRequest.CodigoSessao           = parametros.CodigoSessao;
            lRequest.CodigoSistemaCliente   = parametros.CodigoSistemaCliente;
            lRequest.CodigoUsuario          = parametros.CodigoUsuario;
            lRequest.DataReferencia         = parametros.DataReferencia;
            lRequest.DescricaoUsuarioLogado = parametros.DescricaoUsuarioLogado;
            lRequest.Email                  = parametros.Email;
            lRequest.IdUsuarioLogado        = parametros.IdUsuarioLogado;
            lRequest.IP                     = parametros.IP;
            lRequest.Senha                  = parametros.Senha;
            return AutenticarUsuarioDinamico(lRequest);
        }

        public AutenticarUsuarioResponse AutenticarUsuarioDinamico(AutenticarUsuarioDinamicoRequest parametros)
        {   //--> Prepara resposta
            AutenticarUsuarioResponse resposta =
                new AutenticarUsuarioResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            string _Usuario       = string.Empty;
            string _IDUsuario     = string.Empty;
            string _IP  = string.Empty;
            int NrTentativasAcesso = 0;

            try
            {   //--> Bloco de controle
                UsuarioInfo usuarioInfo = _servicoPersistencia.ReceberUsuario(
                    new ReceberUsuarioRequest()
                    {   //--> Recebe o usuario informado
                        CodigoUsuario = parametros.CodigoUsuario,
                        Email = parametros.Email
                    }).Usuario;

                if (usuarioInfo != null) //--> Achou usuario?
                {
                    _Usuario = usuarioInfo.Email;
                    _IDUsuario = usuarioInfo.CodigoUsuario;
                    _IP = parametros.IP;

                    NrTentativasAcesso = new PersistenciaControleAcesso().ObterNumeroTentativaAcesso(int.Parse(_IDUsuario));

                    if (NrTentativasAcesso >= 3)
                    {
                        resposta.StatusResposta = MensagemResponseStatusEnum.ErroValidacao;
                        resposta.DescricaoResposta = "Número de tentativas de acesso (3x) sem sucesso. Para sua segurança a senha foi bloqueada. Para desbloqueio entre em contato com a nossa Central de Relacionamento: 4007 1873 (região metropolitana) ou 0800 655 1873 (demais regiões).";
                        return resposta;
                    }

                    logger.Debug(usuarioInfo.CodigoUsuario + " - " + usuarioInfo.Email);
                    
                    // Valida a senha
                    //if (/*!_config.ValidarSenhas ||*/ parametros.Senha == usuarioInfo.Senha)
                    if(ValidarSenha(parametros.Senha, usuarioInfo.Senha) || ValidarSenha(parametros.SenhaDinamica, usuarioInfo.SenhaDinamica))                    {
                        int CodigoAcesso = new PersistenciaControleAcesso().ObterCodigoAcessoSistema(int.Parse(_IDUsuario));

                        logger.Debug("Codigo acesso: " + CodigoAcesso);

                        resposta.CodigoAcessoSistema = CodigoAcesso;

                        Usuario usuario = new Usuario(usuarioInfo); //--> Cria o usuario

                        // Verifica se é admin ou possui permissão de acesso ao sistema
                        bool permiteAcesso = usuario.PermissoesAssociadas.EhAdministrador
                                          || usuario.PermissoesAssociadas.ConsultarPermissao(ConfigurationManager.AppSettings.Get("PermissaoDeAcessoAoSistema")); // 22FF518C-C7D3-4ff0-A0CB-96F2476068BB - 

                        logger.Debug("permiteAcesso " + permiteAcesso);

                        if (parametros.CodigoSistemaCliente == "GTI")
                        {
                            permiteAcesso = usuario.PermissoesAssociadas.ConsultarPermissaoSistema(usuarioInfo.Permissoes, permiteAcesso);
                        }
                        
                        if (permiteAcesso)
                        {   //--> Cria sessaoInfo

                           
                            var sessaoInfo = new SessaoInfo()
                            {
                                CodigoSistemaCliente = parametros.CodigoSistemaCliente,
                                CodigoUsuario = usuarioInfo.CodigoUsuario,
                                DataCriacao = DateTime.Now,
                                DataUltimaConsulta = DateTime.Now,
                                EhSessaoDeAdministrador = usuario.PermissoesAssociadas.EhAdministrador
                            };

                            var sessao = new Sessao(sessaoInfo)
                            {   //--> Inicia a criação da sessao
                                Usuario = usuario
                            };                           


                            _sessoes.Add(sessao.SessaoInfo.CodigoSessao, sessao); //--> Adiciona na lista de sessoes

                            // Passa pelos complementos. Eles complementar ou barrar a autenticação do usuário
                            bool resultadoComplementos = true;
                            foreach (KeyValuePair<ComplementoAutenticacaoInfo, IComplementoAutenticacao> item in _complementosAutenticacao)
                                resultadoComplementos = resultadoComplementos && (item.Value.ComplementarAutenticacao(parametros, sessao).StatusResposta == MensagemResponseStatusEnum.OK);

                            if (resultadoComplementos) //--> Permitido? Salva a sessaoInfo?
                            {   //--> Informa na resposta

                                resposta.Sessao = sessao.SessaoInfo;
                                resposta.StatusResposta = MensagemResponseStatusEnum.OK;

                                LogAcessoInfo log = new LogAcessoInfo()
                                {
                                    CodigoSessao = sessao.SessaoInfo.CodigoSessao,
                                    Usuario = sessao.Usuario.UsuarioInfo,
                                    IP = parametros.IP,
                                    Sistema = parametros.CodigoSistemaCliente,
                                };

                                MensagemResponseBase resLog = EfetuarLogDeAcesso(new EfetuarLogDeAcessoRequest()
                                {
                                    LogAcesso = log
                                });

                                AlterarPermissaoAcessoRequest _request = new AlterarPermissaoAcessoRequest();
                                _request.PermissaoAcessoUsuarioInfo.IdUsuario = int.Parse(_IDUsuario);
                                _request.PermissaoAcessoUsuarioInfo.UsuarioAcessoAcao = UsuarioAcessoEnum.Desbloqueio;

                                AlterarPermissaoAcessoResponse AlterarPermissaoAcesso = new PersistenciaControleAcesso().AlterarPermissaoAcesso(_request);

                                logger.Debug(resLog.DescricaoResposta);
                                logger.Debug(log);
                                //resLog.DescricaoResposta
                            }
                            else
                            {   //--> Informa na resposta
                                resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                                resposta.DescricaoResposta = "Usuário não possui acesso ao sistema";
                            }
                        }
                        else
                        {   //--> Informa na resposta                          
                            resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                            resposta.DescricaoResposta = "Usuário não possui acesso ao sistema";
                        }
                    }
                    else
                    {   //--> Informa senha inválida

                        //Bloco Bloquear tentativa de acesso.

                        VerificarTentativaAcessoRequest _request = new VerificarTentativaAcessoRequest();

                        _request._TentativaUsuarioInfo.IdUsuario = int.Parse(_IDUsuario);
                        _request._TentativaUsuarioInfo.DsUsuario = _Usuario;
                        _request._TentativaUsuarioInfo.IPUsuario = _IP;  

                        VerificarTentativaAcessoResponse _Resposta = 
                            new PersistenciaControleAcesso().ControlarTentativasAcesso(_request);
                        

                        if (_Resposta.StBloqueado == true){
                            resposta.DescricaoResposta = _Resposta.DescricaoResposta;
                            resposta.StatusResposta = MensagemResponseStatusEnum.ErroValidacao;
                        }
                        else{
                            resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                            resposta.DescricaoResposta = "Usuário inválido";
                        }
                    
                    }
                }
                else
                {   //--> Informa usuário inválido
                    resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                    resposta.DescricaoResposta = "Usuário inválido";
                }
            }
            catch (Exception ex)
            {
                logger.Error(parametros, ex); //--> Log

                resposta.DescricaoResposta = "Usuario não encontrado"; //--> Informa na mensagem
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return resposta;
        }

        #endregion

        #region Assessor Compartilhado

        public ListarAssessorCompartilhadoResponse ListarAssessorCompartilhado(ListarAssessorCompartilhadoRequest parametros)
        {
            ListarAssessorCompartilhadoResponse _response = new ListarAssessorCompartilhadoResponse();
            try{
                _response = new PersistenciaControleAcesso().ObterClientesCompartilhados(parametros);

            }
            catch (Exception ex){
                logger.Error("Ocorreu um erro ao carregar o método ListarAssessorCompartilhado. " + ex.Message, ex);
            }

            return _response;
        }


        #endregion

        #region Bloqueio Acesso
        /// <summary>
        /// Metodo responsavel por bloquear/desbloquear um usuario no controle de acesso.
        /// </summary>
        /// <param name="pParametros"></param>
        /// <returns></returns>
        public AlterarPermissaoAcessoResponse AlterarPermissaoAcesso(AlterarPermissaoAcessoRequest pParametros)
        {
            AlterarPermissaoAcessoResponse _response = new AlterarPermissaoAcessoResponse();
            try
            {
                _response = new PersistenciaControleAcesso().AlterarPermissaoAcesso(pParametros);
            }
            catch (Exception ex){
                logger.Error("Ocorreu um erro ao carregar o método AlterarPermissaoAcesso. " + ex.Message, ex);
            }

            return _response;
        }

        #endregion

        #region Permissões

        /// <summary>
        /// Faz a consulta para saber se a sessao possui determinada permissao
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public VerificarPermissaoResponse VerificarPermissao(VerificarPermissaoRequest parametros)
        {
            var resposta = new VerificarPermissaoResponse()
            {
                CodigoMensagemRequest = parametros.CodigoMensagem
            };

            try
            {   //--> Bloco de controle
                Sessao sessao = null; //--> Recupera a sessao

                if (_sessoes.ContainsKey(parametros.CodigoSessao))
                    sessao = _sessoes[parametros.CodigoSessao];

                if (sessao != null) //--> Achou a sessao?
                {
                    resposta.Permitido = sessao.Usuario.PermissoesAssociadas.ConsultarPermissao(parametros.CodigoPermissao);
                }
                else
                {   //--> Informa na resposta
                    resposta.Permitido = false;
                    resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                    resposta.DescricaoResposta = "Sessão inválida";
                }
            }
            catch (Exception ex)
            {
                logger.Error(parametros, ex); //--> Log

                resposta.DescricaoResposta = ex.ToString(); //--> Informa na resposta
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return resposta;
        }

        /// <summary>
        /// Solicita a lista de permissões do sistema
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarPermissoesResponse ListarPermissoes(ListarPermissoesRequest parametros)
        {
            var resposta = new ListarPermissoesResponse()
            {
                CodigoMensagemRequest = parametros.CodigoMensagem
            };

            try
            {
                resposta = _servicoPersistencia.ListarPermissoes(parametros);
            }
            catch (Exception ex)
            {
                logger.Error(parametros, ex);
                resposta.DescricaoResposta = ex.Message;
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return resposta;
        }

        /// <summary>
        /// Solicita a adição de uma permissão a usuários, grupos ou perfis
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase AssociarPermissao(AssociarPermissaoRequest parametros)
        {
            var resposta = new MensagemResponseBase()
            {
                CodigoMensagemRequest = parametros.CodigoMensagem
            };

            try
            {   //--> Verifica se a sessao tem permissao para adicionar permissao
                VerificarPermissaoResponse verificarAdicionarPermissao =
                    this.VerificarPermissao(new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        CodigoPermissao = "F3C957F0-1C4B-4ad5-8D39-C307F7237314"//typeof(PermissaoAssociarPermissao)
                    });

                if (verificarAdicionarPermissao.Permitido)
                {   //--> Localiza a permissao
                    PermissaoInfo permissao = null;
                    if (parametros.CodigoPermissao != null)
                        permissao = _permissoes.ListaPorCodigo[permissao.CodigoPermissao];

                    var permissaoAssociada = new PermissaoAssociadaInfo()
                    {   //--> Cria a associação de permissao
                        CodigoPermissao = permissao.CodigoPermissao,
                        Status = parametros.StatusPermissao
                    };

                    if (parametros.CodigoUsuario != null)
                    {
                        UsuarioInfo usuarioInfo = _servicoPersistencia.ReceberUsuario(
                            new ReceberUsuarioRequest()
                            {
                                CodigoUsuario = parametros.CodigoUsuario
                            }).Usuario;

                        // Apenas se tem usuário
                        if (usuarioInfo != null)
                        {
                            // Verifica se cria ou altera a existente
                            PermissaoAssociadaInfo permissaoAssociadaExistente = (from p in usuarioInfo.Permissoes
                                                                                  where p.CodigoPermissao == permissao.CodigoPermissao
                                                                                  select p).FirstOrDefault();
                            if (permissaoAssociadaExistente != null)
                                permissaoAssociadaExistente.Status = permissaoAssociada.Status;
                            else
                                usuarioInfo.Permissoes.Add(permissaoAssociada);

                            // Salva o usuario
                            // ** Se o usuário não tiver permissões para salvar usuário, aqui
                            // ** não vai salvar.
                            _servicoPersistencia.SalvarUsuario(new SalvarUsuarioRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                Usuario = usuarioInfo
                            });
                        }
                    }

                    
                    if (parametros.CodigoUsuarioGrupo != null)
                    {   //--> Associa a usuário grupo?
                        UsuarioGrupoInfo usuarioGrupoInfo = _servicoPersistencia.ReceberUsuarioGrupo(
                            new ReceberUsuarioGrupoRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoUsuarioGrupo = parametros.CodigoUsuario
                            }).UsuarioGrupo;

                        // Apenas se tem usuário
                        if (usuarioGrupoInfo != null)
                        {
                            // Verifica se cria ou altera a existente
                            PermissaoAssociadaInfo permissaoAssociadaExistente = (from p in usuarioGrupoInfo.Permissoes
                                                                                  where p.CodigoPermissao == permissao.CodigoPermissao
                                                                                  select p).FirstOrDefault();
                            if (permissaoAssociadaExistente != null)
                                permissaoAssociadaExistente.Status = permissaoAssociada.Status;
                            else
                                usuarioGrupoInfo.Permissoes.Add(permissaoAssociada);

                            // Salva o usuarioGrupo
                            // ** Se o usuário não tiver permissões para salvar usuárioGrupo, aqui
                            // ** não vai salvar.
                            _servicoPersistencia.SalvarUsuarioGrupo(new SalvarUsuarioGrupoRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                UsuarioGrupo = usuarioGrupoInfo
                            });
                        }
                    }

                    // Associa a perfil?
                    if (parametros.CodigoPerfil != null)
                    {
                        PerfilInfo perfilInfo = _servicoPersistencia.ReceberPerfil(
                            new ReceberPerfilRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoPerfil = parametros.CodigoPerfil
                            }).Perfil;

                        if (perfilInfo != null) //--> Apenas se tem usuário
                        {   //--> Verifica se cria ou altera a existente
                            PermissaoAssociadaInfo permissaoAssociadaExistente = (from p in perfilInfo.Permissoes
                                                                                  where p.CodigoPermissao == permissao.CodigoPermissao
                                                                                  select p).FirstOrDefault();
                            if (permissaoAssociadaExistente != null)
                                permissaoAssociadaExistente.Status = permissaoAssociada.Status;
                            else
                                perfilInfo.Permissoes.Add(permissaoAssociada);

                            // Salva o perfil
                            // ** Se o usuário não tiver permissões para salvar perfil, aqui
                            // ** não vai salvar.
                            _servicoPersistencia.SalvarPerfil(new SalvarPerfilRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                Perfil = perfilInfo
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(parametros, ex);

                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return resposta;
        }

        /// <summary>
        /// Solicita a validação de um item de segurança.
        /// Verifica se a sessão uma ou mais permissões informadas no item de segurança.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ValidarItemSegurancaResponse ValidarItemSeguranca(ValidarItemSegurancaRequest parametros)
        {
            // Prepara resposta
            var resposta = new ValidarItemSegurancaResponse()
            {
                CodigoMensagemRequest = parametros.CodigoMensagem
            };

            try
            {
                // Sessao válida?
                if (_sessoes.ContainsKey(parametros.CodigoSessao))
                {
                    Sessao sessao = _sessoes[parametros.CodigoSessao];

                    bool permiteAcesso = sessao.Usuario.PermissoesAssociadas.EhAdministrador
                                      || sessao.Usuario.PermissoesAssociadas.ConsultarPermissao("22FF518C-C7D3-4ff0-A0CB-96F2476068BB");


                    foreach (ItemSegurancaInfo itemSeguranca in parametros.ItensSeguranca) //--> Varre os itens
                    {   //--> Sinalizadores
                        bool naoTemItem = false;
                        bool temItem = false;

                        // Valida permissoes 
                        if (itemSeguranca.Permissoes != null)
                        {
                            // Varre as permissões por tipo
                            foreach (string permissaoStr in itemSeguranca.Permissoes)
                            {
                                // Acha o código da permissao
                                string codigoPermissao = permissaoStr;

                                // Tem a permissão?
                                if (sessao.Usuario.PermissoesAssociadas.ConsultarPermissao(codigoPermissao))
                                    temItem = true;
                                else
                                    naoTemItem = true;
                            }
                        }

                        //--> Deve continuar? Se for todas as condições e já deu alguma negativa não precisa continuar
                        if ((itemSeguranca.TipoAtivacao == ItemSegurancaAtivacaoTipoEnum.TodasAsCondicoes && !naoTemItem)
                        || ((itemSeguranca.TipoAtivacao != ItemSegurancaAtivacaoTipoEnum.TodasAsCondicoes)))
                        {   //--> Tem validação de perfis?
                            if (itemSeguranca.Perfis != null)
                            {   //--> Varre os perfis
                                foreach (string codigoPerfil in itemSeguranca.Perfis)
                                {   //--> Tem o perfil?
                                    if (sessao.Usuario.Perfis.ContainsKey(codigoPerfil))
                                        temItem = true;
                                    else
                                        naoTemItem = true;
                                }
                            }
                        }

                        // Deve continuar? Se for todas as condições e já deu alguma negativa não precisa continuar
                        if ((itemSeguranca.TipoAtivacao == ItemSegurancaAtivacaoTipoEnum.TodasAsCondicoes && !naoTemItem)
                        || ((itemSeguranca.TipoAtivacao != ItemSegurancaAtivacaoTipoEnum.TodasAsCondicoes)))
                        {   //--> Valida grupos
                            if (itemSeguranca.Grupos != null)
                            {   //--> Varre os grupos
                                foreach (string codigoGrupo in itemSeguranca.Grupos)
                                {   //--> Tem o grupo?
                                    if (sessao.Usuario.UsuariosGrupo.ContainsKey(codigoGrupo))
                                        temItem = true;
                                    else
                                        naoTemItem = true;
                                }
                            }
                        }

                        // Informa se valido ou não
                        if (itemSeguranca.TipoAtivacao == ItemSegurancaAtivacaoTipoEnum.QualquerCondicao)
                            itemSeguranca.Valido = temItem;
                        else if (itemSeguranca.TipoAtivacao == ItemSegurancaAtivacaoTipoEnum.TodasAsCondicoes)
                            itemSeguranca.Valido = temItem && !naoTemItem;
                        else if (itemSeguranca.TipoAtivacao == ItemSegurancaAtivacaoTipoEnum.NaoValidar)
                            itemSeguranca.Valido = true;

                        // Adiciona na resposta
                        resposta.ItensSeguranca.Add(itemSeguranca);
                    }
                }
                else
                {
                    // Sessao invalida
                    resposta.DescricaoResposta = "Sessao informada (" + parametros.CodigoSessao + ") inválida.";
                    resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                }

            }
            catch (Exception ex)
            {
                logger.Error(parametros, ex);

                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return resposta;
        }

        /// <summary>
        /// Solicita que sejam salvas todas as permissões do sistema.
        /// A persistencia se encarregará de salvar no local apropriado.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase SalvarPermissoes(SalvarPermissoesRequest parametros)
        {
            var resposta = new MensagemResponseBase()
            {
                CodigoMensagemRequest = parametros.CodigoMensagem
            };

            // Referencia para o serviço de persistencia
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();
            try
            {
                // Pede a lista de permissoes
                ListarPermissoesResponse responseListar = this.ListarPermissoes(
                    new ListarPermissoesRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao
                    });

                
                foreach (PermissaoInfo permissao in responseListar.Permissoes)
                    servicoPersistencia.SalvarObjeto<PermissaoInfo>(
                        new SalvarObjetoRequest<PermissaoInfo>()
                        {   //--> Salva na persistencia
                            CodigoSessao = parametros.CodigoSessao,
                            Objeto = permissao
                        });
            }
            catch (Exception ex)
            {
                logger.Error(parametros, ex);

                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return resposta;
        }

        public MensagemResponseBase SalvarPermissao(SalvarPermissaoRequest parametros)
        {
            // Prepara a resposta
            MensagemResponseBase resposta =
                new MensagemResponseBase()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            try
            {
                // Verifica se a sessao tem permissao para salvar usuario
                VerificarPermissaoResponse verificarSalvarPermissao =
                    this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoPermissao = "CB52FC26-3617-47DD-BC55-ABB8449D1484"//typeof(PermissaoSalvarPermissao)
                        });

                // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
                // pela aplicação e repassa a mensagem para o servico de persistencia
                if (verificarSalvarPermissao.Permitido)
                {
                    // Salva a permissão
                    resposta = _servicoPersistencia.SalvarPermissao(parametros);
                }
            }
            catch (Exception ex)
            {
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.Message;
            }

            // Retorna 
            return resposta;

        }

        public ReceberPermissaoResponse ReceberPermissao(ReceberPermissaoRequest parametros)
        {
            // Prepara a resposta
            ReceberPermissaoResponse resposta =
                new ReceberPermissaoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Verifica se a sessao tem permissao 
            VerificarPermissaoResponse verificarPermissao =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        CodigoPermissao = "832DD329-AEB3-4586-AAAF-A8C7834B4944"//typeof(PermissaoReceberPermissao)
                    });

            // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
            // pela aplicação e repassa a mensagem para o servico de persistencia
            if (verificarPermissao.Permitido)
                resposta = _servicoPersistencia.ReceberPermissao(parametros);
            else
                resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;

            // Retorna 
            return resposta;
        }

        #endregion

        #region Usuário

        /// <summary>
        /// Solicita lista de usuários
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarUsuariosResponse ListarUsuarios(ListarUsuariosRequest parametros)
        {
            // Prepara a resposta
            ListarUsuariosResponse resposta =
                new ListarUsuariosResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Bloco de controle
            try
            {
                // Verifica se a sessao tem permissao para salvar usuario
                //VerificarPermissaoResponse verificarListarUsuarios =
                //    this.VerificarPermissao(
                //        new VerificarPermissaoRequest()
                //        {
                //            CodigoSessao = parametros.CodigoSessao,
                //            CodigoPermissao = "A5488A98-0EB4-49df-8100-2815CC2D4DA2"//typeof(PermissaoListarUsuarios)
                //        });

                //// Se tem a permissao, ajusta as propriedades que não podem ser alteradas
                //// pela aplicação e repassa a mensagem para o servico de persistencia
                //if (verificarListarUsuarios.Permitido)
                //{

                logger.Debug("Procurando Usuarios");
                resposta = _servicoPersistencia.ListarUsuarios(parametros);
                logger.Debug("Usuarios Encontrados: " + resposta.Usuarios.Count.ToString());
                //}
                //else
                //{
                //    resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;
                //    logger.Debug("AcessoNaoPermitido");
                //}
            }
            catch (Exception ex)
            {
                // Loga a mensagem
                logger.Error(parametros, ex);

                // Devolve o status na resposta
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.ToString();
            }

            return resposta;
        }

        /// <summary>
        /// Salva o usuário informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarUsuarioResponse SalvarUsuario(SalvarUsuarioRequest parametros)
        {
            SalvarUsuarioDinamicoRequest pRequestDinamico = new SalvarUsuarioDinamicoRequest();
            pRequestDinamico.CodigoMensagem = parametros.CodigoMensagem;
            pRequestDinamico.CodigoSessao = parametros.CodigoSessao;
            pRequestDinamico.DataReferencia = parametros.DataReferencia;
            pRequestDinamico.DescricaoUsuarioLogado = parametros.DescricaoUsuarioLogado;
            pRequestDinamico.IdUsuarioLogado = parametros.IdUsuarioLogado;
            
            pRequestDinamico.Usuario.CodigoUsuario = parametros.Usuario.CodigoUsuario;
            pRequestDinamico.Usuario.Grupos = parametros.Usuario.Grupos;
            pRequestDinamico.Usuario.Grupos2 = parametros.Usuario.Grupos2;
            pRequestDinamico.Usuario.Nome = parametros.Usuario.Nome;
            pRequestDinamico.Usuario.NomeAbreviado = parametros.Usuario.NomeAbreviado;
            pRequestDinamico.Usuario.Senha = parametros.Usuario.Senha;
            pRequestDinamico.Usuario.AssinaturaEletronica = parametros.Usuario.AssinaturaEletronica;
            pRequestDinamico.Usuario.Origem = parametros.Usuario.Origem;
            pRequestDinamico.Usuario.Complementos = parametros.Usuario.Complementos;
            pRequestDinamico.Usuario.Perfis = parametros.Usuario.Perfis;
            pRequestDinamico.Usuario.Perfis2 = parametros.Usuario.Perfis2;
            pRequestDinamico.Usuario.Permissoes = parametros.Usuario.Permissoes;
            pRequestDinamico.Usuario.Relacoes = parametros.Usuario.Relacoes;
            pRequestDinamico.Usuario.Status = parametros.Usuario.Status;
            pRequestDinamico.Usuario.Email = parametros.Usuario.Email;
            pRequestDinamico.Usuario.CodigoTipoAcesso = parametros.Usuario.CodigoTipoAcesso;
            pRequestDinamico.Usuario.CodigoAssessor = parametros.Usuario.CodigoAssessor;
            pRequestDinamico.Usuario.CodigosFilhoAssessor = parametros.Usuario.CodigosFilhoAssessor;

            return SalvarUsuarioDinamico(pRequestDinamico);
        }

        public SalvarUsuarioResponse SalvarUsuarioDinamico(SalvarUsuarioDinamicoRequest parametros)
        {
            // Prepara a resposta
            SalvarUsuarioResponse resposta =
                new SalvarUsuarioResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            try
            {
                // Verifica se a sessao tem permissao para salvar usuario
                VerificarPermissaoResponse verificarSalvarUsuario =
                    this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoPermissao = "024958B9-0377-4d8c-B69A-A6C9C4410EE3"//typeof(PermissaoSalvarUsuario)
                        });

                // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
                // pela aplicação e repassa a mensagem para o servico de persistencia
                if (verificarSalvarUsuario.Permitido)
                {
                    // Carrega usuario
                    UsuarioInfo usuarioInfoOriginal =
                        _servicoPersistencia.ReceberUsuario(
                            new ReceberUsuarioRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoUsuario = parametros.Usuario.CodigoUsuario
                            }).Usuario;

                    // Mantem as senhas
                    if (usuarioInfoOriginal != null)
                    {
                        parametros.Usuario.Senha = usuarioInfoOriginal.Senha;
                        parametros.Usuario.AssinaturaEletronica = usuarioInfoOriginal.AssinaturaEletronica;
                    }

                    bool lStPermitido;

                    {   //--> Assossiar Permissão
                        lStPermitido = this.VerificarPermissao(new VerificarPermissaoRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoPermissao = "F3C957F0-1C4B-4ad5-8D39-C307F7237314" //typeof(PermissaoAssociarPermissao)
                            }).Permitido;

                        // Tem permissao para associar permissoes?
                        if (!lStPermitido)
                        {
                            if (usuarioInfoOriginal != null)
                                parametros.Usuario.Permissoes = usuarioInfoOriginal.Permissoes;
                            else
                                parametros.Usuario.Perfis.Clear();
                        }
                    }

                    {   //--> Assossiar Perfil
                        lStPermitido = this.VerificarPermissao(new VerificarPermissaoRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoPermissao = "D31AD6D1-FCA6-4529-ACBE-B5B9D60E5755" //typeof(PermissaoAssociarPerfil)
                            }).Permitido;

                        // Tem permissao para associar perfis?
                        if (!lStPermitido)
                        {
                            if (usuarioInfoOriginal != null)
                                parametros.Usuario.Perfis = usuarioInfoOriginal.Perfis;
                            else
                                parametros.Usuario.Perfis.Clear();
                        }
                    }

                    {   //--> Assossiar Grupo
                        lStPermitido = this.VerificarPermissao(new VerificarPermissaoRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoPermissao = "626EC9D9-D36E-4378-9761-BCA51F5D76B6" //typeof(PermissaoAssociarUsuarioGrupo)
                            }).Permitido;

                        // Tem permissao para associar grupos?
                        if (!lStPermitido)
                        {
                            if (usuarioInfoOriginal != null)
                                parametros.Usuario.Grupos = usuarioInfoOriginal.Grupos;
                            else
                                parametros.Usuario.Grupos.Clear();
                        }
                    }

                    // Salva o usuário
                    resposta = _servicoPersistencia.SalvarUsuarioDinamico(parametros);
                }
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicita detalhe do usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberUsuarioResponse ReceberUsuario(ReceberUsuarioRequest parametros)
        {
            // Prepara a resposta
            ReceberUsuarioResponse resposta =
                new ReceberUsuarioResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            try
            {
                // Solicita o usuário
                resposta = _servicoPersistencia.ReceberUsuario(parametros);

                // Achou o usuário?
                if (resposta.Usuario != null)
                {
                    // Não repassa as senhas
                    resposta.Usuario.Senha = null;
                    resposta.Usuario.AssinaturaEletronica = null;

                    // Passa o usuárioInfo pelos complementos
                    foreach (KeyValuePair<ComplementoAutenticacaoInfo, IComplementoAutenticacao> item in _complementosAutenticacao)
                        item.Value.ComplementarUsuario(resposta.Usuario);
                }
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicitação de remoção do usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase RemoverUsuario(RemoverUsuarioRequest parametros)
        {
            // Prepara a resposta
            MensagemResponseBase resposta =
                new MensagemResponseBase()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            try
            {
                // Verifica se a sessao tem permissao 
                VerificarPermissaoResponse verificarPermissao =
                    this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoPermissao = "B4F0748A-C5F5-4b39-9EC1-FBB15D1C1EA3"//typeof(PermissaoRemoverUsuario)
                        });

                // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
                // pela aplicação e repassa a mensagem para o servico de persistencia
                if (verificarPermissao.Permitido)
                    resposta = _servicoPersistencia.RemoverUsuario(parametros);
                else
                    resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicita a validação da assinatura eletrônica
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase ValidarAssinaturaEletronica(ValidarAssinaturaEletronicaRequest parametros)
        {

            MensagemResponseBase resposta =
            new MensagemResponseBase()
            {
                CodigoMensagemRequest = parametros.CodigoMensagem
            };


            try
            {
                // Pega a sessao
                Sessao sessao = _sessoes[parametros.CodigoSessao];

                // Valida
                bool valido = sessao.Usuario.UsuarioInfo.AssinaturaEletronica == parametros.AssinaturaEletronica;

                resposta.StatusResposta = valido ? MensagemResponseStatusEnum.OK : MensagemResponseStatusEnum.AcessoNaoPermitido;
                resposta.DescricaoResposta = valido ? null : "Assinatura inválida";

            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita alteração da assinatura eletrônica para a sessão
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase AlterarAssinaturaEletronica(AlterarAssinaturaEletronicaRequest parametros)
        {
            // Prepara resposta
            MensagemResponseBase resposta =
                new MensagemResponseBase()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            try
            {
                // Pega a sessao
                Sessao sessao = _sessoes[parametros.CodigoSessao];

                // Pega o usuário
                UsuarioInfo usuarioInfo =
                    _servicoPersistencia.ReceberUsuario(
                        new ReceberUsuarioRequest()
                        {
                            CodigoUsuario = sessao.Usuario.UsuarioInfo.CodigoUsuario
                        }).Usuario;

                // Altera a assinatura do usuario e no usuario da sessao
                usuarioInfo.AssinaturaEletronica = parametros.NovaAssinaturaEletronica;
                sessao.Usuario.UsuarioInfo.AssinaturaEletronica = parametros.NovaAssinaturaEletronica;

                // Salva o usuario
                _servicoPersistencia.SalvarUsuario(
                    new SalvarUsuarioRequest()
                    {
                        Usuario = usuarioInfo
                    });
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita alteração de senha do usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase AlterarSenha(AlterarSenhaRequest parametros)
        {
            // Prepara resposta
            MensagemResponseBase resposta =
                new MensagemResponseBase()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            try
            {

                // Pega o usuário
                //UsuarioInfo usuarioInfo =
                //    _servicoPersistencia.ReceberUsuario(
                //        new ReceberUsuarioRequest()
                //        {
                //            CodigoSessao = parametros.CodigoSessao,
                //            CodigoUsuario = parametros.CodigoUsuario
                //        }).Usuario;

                ReceberSessaoResponse lRes = ReceberSessao(new ReceberSessaoRequest() { CodigoSessaoARetornar = parametros.CodigoSessao, CodigoSessao = parametros.CodigoSessao });

                // Verifica se a senha bate com a atual
                if (parametros.SenhaAtual == lRes.Usuario.Senha)
                {
                    // Altera a assinatura do usuario e no usuario da sessao
                    lRes.Usuario.Senha = parametros.NovaSenha;

                    // Salva o usuario
                    _servicoPersistencia.SalvarUsuario(
                        new SalvarUsuarioRequest()
                        {
                            Usuario = lRes.Usuario
                        });
                }
                else
                {
                    // Informa senha invalida
                    resposta.Criticas.Add(
                        new CriticaInfo()
                        {
                            Descricao = "Senha atual inválida",
                            Status = CriticaStatusEnum.ErroNegocio
                        });

                    // Seta o status da resposta
                    resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                }
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            // Retorna
            return resposta;
        }

        #endregion

        #region UsuarioGrupo

        /// <summary>
        /// Solicita lista de grupos de usuários
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarUsuarioGruposResponse ListarUsuarioGrupos(ListarUsuarioGruposRequest parametros)
        {
            // Prepara a resposta
            ListarUsuarioGruposResponse resposta =
                new ListarUsuarioGruposResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Verifica se a sessao tem permissao para salvar usuario
            VerificarPermissaoResponse verificarListarUsuarioGrupos =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        CodigoPermissao = "FCB9C6BB-B54F-4d5f-9749-85BA6806C6A3"//typeof(PermissaoListarUsuarioGrupos)
                    });

            // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
            // pela aplicação e repassa a mensagem para o servico de persistencia
            if (verificarListarUsuarioGrupos.Permitido)
                resposta = _servicoPersistencia.ListarUsuarioGrupos(parametros);
            else
                resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;

            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicitação para salvar um grupo de usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase SalvarUsuarioGrupo(SalvarUsuarioGrupoRequest parametros)
        {
            // Prepara a resposta
            MensagemResponseBase resposta =
                new MensagemResponseBase()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Verifica se a sessao tem permissao para salvar usuario
            VerificarPermissaoResponse verificarSalvarUsuarioGrupo =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        CodigoPermissao = "0D08C6D1-3CD1-490d-9868-E2567ED25F7A"//typeof(PermissaoSalvarUsuarioGrupo)
                    });

            // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
            // pela aplicação e repassa a mensagem para o servico de persistencia
            if (verificarSalvarUsuarioGrupo.Permitido)
            {
                // Carrega usuario
                UsuarioGrupoInfo usuarioGrupoInfoOriginal =
                    _servicoPersistencia.ReceberUsuarioGrupo(
                        new ReceberUsuarioGrupoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoUsuarioGrupo = parametros.UsuarioGrupo.CodigoUsuarioGrupo
                        }).UsuarioGrupo;

                // Tem permissao para associar permissoes?
                if (!this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoPermissao = "F3C957F0-1C4B-4ad5-8D39-C307F7237314"//typeof(PermissaoAssociarPermissao)
                        }).Permitido)
                {
                    if (usuarioGrupoInfoOriginal != null)
                        parametros.UsuarioGrupo.Permissoes = usuarioGrupoInfoOriginal.Permissoes;
                    else
                        parametros.UsuarioGrupo.Perfis.Clear();
                }

                // Tem permissao para associar perfis?
                if (!this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoPermissao = "D31AD6D1-FCA6-4529-ACBE-B5B9D60E5755"//typeof(PermissaoAssociarPerfil)
                        }).Permitido)
                {
                    if (usuarioGrupoInfoOriginal != null)
                        parametros.UsuarioGrupo.Perfis = usuarioGrupoInfoOriginal.Perfis;
                    else
                        parametros.UsuarioGrupo.Perfis.Clear();
                }

                // Salva o usuário
                resposta = _servicoPersistencia.SalvarUsuarioGrupo(parametros);
            }

            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicita a associação de um usuário grupo a outra entidade
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase AssociarUsuarioGrupo(AssociarUsuarioGrupoRequest parametros)
        {
            // Prepara resposta
            MensagemResponseBase resposta =
                new MensagemResponseBase()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Verifica se o grupo existe
            UsuarioGrupoInfo usuarioGrupo =
                _servicoPersistencia.ReceberUsuarioGrupo(
                    new ReceberUsuarioGrupoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        CodigoUsuarioGrupo = parametros.CodigoUsuarioGrupo
                    }).UsuarioGrupo;

            // Achou o grupo?
            if (usuarioGrupo != null)
            {

                // Verifica se a sessao tem permissao para adicionar permissao
                VerificarPermissaoResponse verificarPermissaoResponse =
                    this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoPermissao = "626EC9D9-D36E-4378-9761-BCA51F5D76B6"//typeof(PermissaoAssociarUsuarioGrupo)
                        });

                // Tem?
                if (verificarPermissaoResponse.Permitido)
                {
                    // Associa a usuário?
                    if (parametros.CodigoUsuario != null)
                    {
                        // Carrega o usuario
                        UsuarioInfo usuarioInfo = _servicoPersistencia.ReceberUsuario(
                            new ReceberUsuarioRequest()
                            {
                                CodigoUsuario = parametros.CodigoUsuario
                            }).Usuario;

                        // Apenas se tem usuário
                        if (usuarioInfo != null)
                        {
                            // Verifica se ainda não possui a associação
                            if (!usuarioInfo.Grupos.Contains(parametros.CodigoUsuarioGrupo))
                                usuarioInfo.Grupos.Add(parametros.CodigoUsuarioGrupo);

                            // Salva o usuario
                            // ** Se o usuário não tiver permissões para salvar usuário, aqui
                            // ** não vai salvar.
                            _servicoPersistencia.SalvarUsuario(
                                new SalvarUsuarioRequest()
                                {
                                    CodigoSessao = parametros.CodigoSessao,
                                    Usuario = usuarioInfo
                                });
                        }
                    }
                }
                else
                {
                    // Informa acesso não permitido
                    resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;
                }
            }
            else
            {
                // Informa erro
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                resposta.DescricaoResposta = "Grupo de Usuário inválido para associação.";
            }

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita detalhe do grupo de usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberUsuarioGrupoResponse ReceberUsuarioGrupo(ReceberUsuarioGrupoRequest parametros)
        {
            // Prepara a resposta
            ReceberUsuarioGrupoResponse resposta =
                new ReceberUsuarioGrupoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Verifica se a sessao tem permissao 
            VerificarPermissaoResponse verificarPermissao =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        CodigoPermissao = "A0FE5EA1-86FF-4efb-A7DE-7A24996E4080"//typeof(PermissaoReceberUsuarioGrupo)
                    });

            // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
            // pela aplicação e repassa a mensagem para o servico de persistencia
            if (verificarPermissao.Permitido)
                resposta = _servicoPersistencia.ReceberUsuarioGrupo(parametros);
            else
                resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;

            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicitação remoção do grupo de usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase RemoverUsuarioGrupo(RemoverUsuarioGrupoRequest parametros)
        {
            // Prepara a resposta
            MensagemResponseBase resposta =
                new MensagemResponseBase()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Verifica se a sessao tem permissao 
            VerificarPermissaoResponse verificarPermissao =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        CodigoPermissao = "23BBC31B-DD05-4c41-9BC6-5A814FB2D31C"//typeof(PermissaoRemoverUsuarioGrupo)
                    });

            // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
            // pela aplicação e repassa a mensagem para o servico de persistencia
            if (verificarPermissao.Permitido)
                resposta = _servicoPersistencia.RemoverUsuarioGrupo(parametros);
            else
                resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;

            // Retorna 
            return resposta;
        }

        #endregion

        #region Perfil

        /// <summary>
        /// Solicitação de lista de perfis
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarPerfisResponse ListarPerfis(ListarPerfisRequest parametros)
        {
            // Prepara a resposta
            ListarPerfisResponse resposta =
                new ListarPerfisResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            try
            {
                // Verifica se a sessao tem permissao para salvar usuario
                VerificarPermissaoResponse verificarListarPerfis =
                    this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoPermissao = "3803F285-F6BA-4250-9315-D3F869FF2F5F"//typeof(PermissaoListarPerfis)
                        });

                // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
                // pela aplicação e repassa a mensagem para o servico de persistencia
                if (verificarListarPerfis.Permitido)
                    resposta = _servicoPersistencia.ListarPerfis(parametros);
                else
                    resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicitação para salvar um perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase SalvarPerfil(SalvarPerfilRequest parametros)
        {
            // Prepara a resposta
            MensagemResponseBase resposta =
                new MensagemResponseBase()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            try
            {
                // Verifica se a sessao tem permissao para salvar usuario
                VerificarPermissaoResponse verificarSalvarPerfil =
                    this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoPermissao = "7ED6A969-8C59-4e62-8BE7-CEB83134FF62" //typeof(PermissaoSalvarPerfil)
                        });

                // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
                // pela aplicação e repassa a mensagem para o servico de persistencia
                if (verificarSalvarPerfil.Permitido)
                {
                    // Carrega usuario
                    PerfilInfo perfilInfoOriginal =
                        _servicoPersistencia.ReceberPerfil(
                            new ReceberPerfilRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoPerfil = parametros.Perfil.CodigoPerfil
                            }).Perfil;

                    // Tem permissao para associar permissoes?
                    if (!this.VerificarPermissao(
                            new VerificarPermissaoRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoPermissao = "F3C957F0-1C4B-4ad5-8D39-C307F7237314"//typeof(PermissaoAssociarPermissao)
                            }).Permitido)
                    {
                        if (perfilInfoOriginal != null)
                            parametros.Perfil.Permissoes = perfilInfoOriginal.Permissoes;
                        else
                            parametros.Perfil.Permissoes.Clear();
                    }

                    // Salva o usuário
                    resposta = _servicoPersistencia.SalvarPerfil(parametros);
                }
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicita a associação de um perfil com alguma entidade
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase AssociarPerfil(AssociarPerfilRequest parametros)
        {
            // Prepara resposta
            MensagemResponseBase resposta =
                new MensagemResponseBase()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            try
            {

                // Verifica se o grupo existe
                PerfilInfo perfil =
                    _servicoPersistencia.ReceberPerfil(
                        new ReceberPerfilRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoPerfil = parametros.CodigoPerfil
                        }).Perfil;

                // Achou o perfil?
                if (perfil != null)
                {
                    // Verifica se a sessao tem permissao para adicionar permissao
                    VerificarPermissaoResponse verificarPermissaoResponse =
                        this.VerificarPermissao(
                            new VerificarPermissaoRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoPermissao = "D31AD6D1-FCA6-4529-ACBE-B5B9D60E5755"//typeof(PermissaoAssociarPerfil)
                            });

                    // Tem?
                    if (verificarPermissaoResponse.Permitido)
                    {
                        // Associa a usuário?
                        if (parametros.CodigoUsuario != null)
                        {
                            // Carrega o usuario
                            UsuarioInfo usuarioInfo = _servicoPersistencia.ReceberUsuario(
                                new ReceberUsuarioRequest()
                                {
                                    CodigoUsuario = parametros.CodigoUsuario
                                }).Usuario;

                            // Apenas se tem usuário
                            if (usuarioInfo != null)
                            {
                                // Verifica se ainda não possui a associação
                                if (!usuarioInfo.Perfis.Contains(parametros.CodigoPerfil))
                                    usuarioInfo.Perfis.Add(parametros.CodigoPerfil);

                                // Salva o usuario
                                // ** Se o usuário não tiver permissões para salvar usuário, aqui
                                // ** não vai salvar.
                                _servicoPersistencia.SalvarUsuario(
                                    new SalvarUsuarioRequest()
                                    {
                                        CodigoSessao = parametros.CodigoSessao,
                                        Usuario = usuarioInfo
                                    });
                            }
                        }

                        // Associa a grupo de usuários?
                        if (parametros.CodigoUsuarioGrupo != null)
                        {
                            // Carrega o usuarioGrupo
                            UsuarioGrupoInfo usuarioGrupoInfo = _servicoPersistencia.ReceberUsuarioGrupo(
                                new ReceberUsuarioGrupoRequest()
                                {
                                    CodigoUsuarioGrupo = parametros.CodigoUsuarioGrupo
                                }).UsuarioGrupo;

                            // Apenas se tem usuárioGrupo
                            if (usuarioGrupoInfo != null)
                            {
                                // Verifica se ainda não possui a associação
                                if (!usuarioGrupoInfo.Perfis.Contains(parametros.CodigoPerfil))
                                    usuarioGrupoInfo.Perfis.Add(parametros.CodigoPerfil);

                                // Salva o usuarioGrupo
                                // ** Se o usuário não tiver permissões para salvar usuárioGrupo, aqui
                                // ** não vai salvar.
                                _servicoPersistencia.SalvarUsuarioGrupo(
                                    new SalvarUsuarioGrupoRequest()
                                    {
                                        CodigoSessao = parametros.CodigoSessao,
                                        UsuarioGrupo = usuarioGrupoInfo
                                    });
                            }
                        }
                    }
                    else
                    {
                        // Informa acesso não permitido
                        resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;
                    }
                }
                else
                {
                    // Informa erro
                    resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                    resposta.DescricaoResposta = "Perfil inválido para associação.";
                }
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita detalhe do perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberPerfilResponse ReceberPerfil(ReceberPerfilRequest parametros)
        {
            // Prepara a resposta
            ReceberPerfilResponse resposta =
                new ReceberPerfilResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            try
            {

                // Verifica se a sessao tem permissao 
                VerificarPermissaoResponse verificarPermissao =
                    this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoPermissao = "FF941811-D66B-470f-89A7-45A0C5AD9537"//typeof(PermissaoReceberPerfil)
                        });

                // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
                // pela aplicação e repassa a mensagem para o servico de persistencia
                if (verificarPermissao.Permitido)
                    resposta = _servicoPersistencia.ReceberPerfil(parametros);
                else
                    resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicitação remoção do perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase RemoverPerfil(RemoverPerfilRequest parametros)
        {
            // Prepara a resposta
            MensagemResponseBase resposta =
                new MensagemResponseBase()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            try
            {
                // Verifica se a sessao tem permissao 
                VerificarPermissaoResponse verificarPermissao =
                    this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoPermissao = "54DDD6FF-871B-49cd-B3F6-745BAE281AEB"//typeof(PermissaoRemoverPerfil)
                        });

                // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
                // pela aplicação e repassa a mensagem para o servico de persistencia
                if (verificarPermissao.Permitido)
                    resposta = _servicoPersistencia.RemoverPerfil(parametros);
                else
                    resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicitação remoção do perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase RemoverPermissao(RemoverPermissaoRequest parametros)
        {
            // Prepara a resposta
            MensagemResponseBase resposta =
                new MensagemResponseBase()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            try
            {
                // Verifica se a sessao tem permissao 
                VerificarPermissaoResponse verificarPermissao =
                    this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            CodigoPermissao = "6C95C315-47A7-4450-A576-AC24538AE655"//typeof(PermissaoRemoverPermissao)
                        });

                // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
                // pela aplicação e repassa a mensagem para o servico de persistencia
                if (verificarPermissao.Permitido)
                    resposta = _servicoPersistencia.RemoverPermissao(parametros);
                else
                    resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            // Retorna 
            return resposta;
        }

        #endregion

        #region Sessao

        /// <summary>
        /// Retorna informações da sessão solicitada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberSessaoResponse ReceberSessao(ReceberSessaoRequest parametros)
        {
            // Prepara resposta
            ReceberSessaoResponse resposta =
                new ReceberSessaoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            try
            {
                // Se existir, pega a sessao
                if (_sessoes.ContainsKey(parametros.CodigoSessaoARetornar))
                {
                    // Pega a sessao
                    Sessao sessao = _sessoes[parametros.CodigoSessaoARetornar];

                    // Retorna a sessao
                    resposta.Sessao = sessao.SessaoInfo;

                    // Se pediu o usuario, retorna 
                    resposta.Usuario = sessao.Usuario.UsuarioInfo;
                }

            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            // Retorna
            return resposta;
        }

        public ListarSessoesResponse ListarSessoes(ListarSessoesRequest parametros)
        {
            // Prepara resposta
            ListarSessoesResponse resposta =
                new ListarSessoesResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            try
            {
                DbLib dbLib = new DbLib("Seguranca");
                Dictionary<string, object> paramsProc = new Dictionary<string, object>();

                if (parametros.BuscarDeDiasAnteriores)
                    paramsProc.Add("@DiasAnteriores", parametros.BuscarDeDiasAnteriores);

                DataSet ds = dbLib.ExecutarProcedure("prc_tb_LogAcesso_lst", paramsProc, new List<string>());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        Sessao _sessao = new Sessao(new SessaoInfo()
                        {
                            CodigoSessao = dr["CodigoSessao"].ToString(),
                            CodigoSistemaCliente = Convert.IsDBNull(dr["Sistema"]) ? string.Empty : dr["Sistema"].ToString(),
                            CodigoUsuario = dr["IdLogin"].ToString(),
                            DataCriacao = (DateTime)dr["DataLogIn"],
                        });
                        if (!parametros.BuscarDeDiasAnteriores)
                        {
                            _sessao.Id = (int)dr["IdLog"];
                            _sessao.IP = Convert.IsDBNull(dr["IP_Usuario"]) ? string.Empty : dr["IP_Usuario"].ToString();
                            _sessao.Usuario = new Usuario(new UsuarioInfo()
                            {
                                Email = Convert.IsDBNull(dr["email"]) ? string.Empty : dr["email"].ToString().ToString(),
                                CodigoUsuario = dr["IdLogin"].ToString(),
                                Nome = dr["dsUsuario"].ToString()
                            });
                            // Se existir, pega a sessao
                            if (_sessoes.ContainsKey(_sessao.SessaoInfo.CodigoSessao))
                                _sessao.SessaoAtiva = true;
                            else
                                _sessao.SessaoAtiva = false;
                        }
                        resposta.Sessoes.Add(_sessao);
                    }
                }

                resposta.StatusResposta = MensagemResponseStatusEnum.OK;
                resposta.DescricaoResposta = "Erro ao retornar os usuários logados.";
            }

            catch (Exception ex)
            {
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.Message;
                logger.Error(ex);
            }

            // Retorna
            return resposta;
        }

        public MensagemResponseBase EfetuarLogOut(MensagemRequestBase parametros)
        {
            logger.Debug("Efetuar LogOut: " + parametros.CodigoSessao);
            MensagemResponseBase resposta = new MensagemResponseBase()
            {
                CodigoMensagemRequest = parametros.CodigoMensagem
            };

            if (_sessoes.ContainsKey(parametros.CodigoSessao))
            {
                _sessoes.Remove(parametros.CodigoSessao);
            }

            try
            {
                this.LogOut(parametros.CodigoSessao);
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return resposta;
        }

        private void LogOut(string CodigoSessao)
        {
            logger.Debug(CodigoSessao);
            try
            {
                DbLib dbLib = new DbLib("Seguranca");
                Dictionary<string, object> paramsProc = new Dictionary<string, object>();

                paramsProc.Add("@CodigoSessao", CodigoSessao);

                dbLib.ExecutarProcedure("prc_tb_LogAcesso_LogOut", paramsProc, new List<string>());
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace);
            }

        }

        #endregion

        #endregion

        #region | IServicoControlavel Members

        public void IniciarServico()
        {
            logger.Info("Iniciando servico Seguranca");
            
            logger.Info("Carregando sessoes em cache");
            
            LoadSessions();

            logger.Debug("Iniciando Serviço de Limpeza de sessões.");
            tcker = new Timer(7200000);
            tcker.Elapsed += new ElapsedEventHandler(tcker_Elapsed);
            tcker.Enabled = true;

            tckSessoes = new Timer(300000);
            tckSessoes.Elapsed += new ElapsedEventHandler(tckSessoes_Elapsed);
            tckSessoes.Enabled = true;

            statusServico = ServicoStatus.EmExecucao;
        }

        void tckSessoes_Elapsed(object sender, ElapsedEventArgs e)
        {
            SaveSessions();
        }

        void tcker_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.LimparSessoes();
        }

        public void PararServico()
        {
            try
            {
                logger.Info("Finalizando servico Seguranca");

                if (tcker != null)
                {
                    tcker.Enabled = false;
                }

                if (tckSessoes != null)
                {
                    tckSessoes.Enabled = false;
                }

                logger.Info("Salvando Sessoes ativas, aguarde");

                SaveSessions();

                this.statusServico = ServicoStatus.Parado;

                logger.Info("Servico Seguranca Finalizado");
            }
            catch (Exception ex)
            {
                logger.Error("PararServico(): " + ex.Message, ex);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return this.statusServico;
        }

        #endregion

        #region IServicoSegurancaADM
        public StatusServicoSegurancaResponse ObterStatusServico(StatusServicoSegurancaRequest request)
        {
            StatusServicoSegurancaResponse response = new StatusServicoSegurancaResponse();

            response.ServicoSegurancaAtivo = true;

            return response;
        }
        #endregion // IServicoSegurancaADM

        private bool UsuarioJaLogado(string login)
        {
            return true;
        }

        private MensagemResponseBase EfetuarLogDeAcesso(EfetuarLogDeAcessoRequest parametros)
        {
            MensagemResponseBase resposta = new MensagemResponseBase()
            {
            };

            try
            {
                DbLib dbLib = new DbLib("Seguranca");
                Dictionary<string, object> paramsProc = new Dictionary<string, object>();

                paramsProc.Add("@IdLogin", int.Parse(parametros.LogAcesso.Usuario.CodigoUsuario));
                paramsProc.Add("@dsUsuario", parametros.LogAcesso.Usuario.Nome == string.Empty ? parametros.LogAcesso.Usuario.Email : parametros.LogAcesso.Usuario.Nome);
                paramsProc.Add("@Sistema", parametros.LogAcesso.Sistema);
                paramsProc.Add("@CodigoSessao", parametros.LogAcesso.CodigoSessao);
                paramsProc.Add("@IP_Usuario", parametros.LogAcesso.IP);
                paramsProc.Add("@Email", parametros.LogAcesso.Usuario.Email);

                dbLib.ExecutarProcedure("prc_tb_LogAcesso_ins", paramsProc, new List<string>());

                resposta.StatusResposta = MensagemResponseStatusEnum.OK;
                resposta.DescricaoResposta = "Log Efetuado com sucesso.";
            }
            catch (Exception ex)
            {
                // Log
                logger.Error(parametros, ex);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }
            return resposta;
        }

        private void LimparSessoes()
        {
            logger.Debug("Iniciando Limpeza das sessões inativas. em: " + DateTime.Now.ToString());

            ListarSessoesResponse lresSessoes = this.ListarSessoes(new ListarSessoesRequest()
            {
                BuscarDeDiasAnteriores = true
            });


            logger.Debug("Numero de sessões: " + lresSessoes.Sessoes.Count.ToString());

            if (lresSessoes.Sessoes.Count > 0)
            {
                try
                {
                    foreach (Sessao lSessao in lresSessoes.Sessoes)
                    {
                        logger.Debug("Sessão : " + lSessao.SessaoInfo.CodigoSessao);

                        if (ConfigurationManager.AppSettings["FlushInactiveSessionsToDB"] != null &&
                            ConfigurationManager.AppSettings["FlushInactiveSessionsToDB"].ToString().ToUpper().Equals("TRUE"))
                        {
                            this.LogOut(lSessao.SessaoInfo.CodigoSessao);
                        }
                        else
                        {
                            logger.Warn("Gravacao de logout na base desabilitada sessao [" + lSessao.SessaoInfo.CodigoSessao + "]");
                        }

                        if (_sessoes.ContainsKey(lSessao.SessaoInfo.CodigoSessao))
                        {
                            _sessoes.Remove(lSessao.SessaoInfo.CodigoSessao);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.StackTrace);
                }
            }

            logger.Debug(lresSessoes.Sessoes.Count.ToString() + " sessões inativas foram finalizada. em: " + DateTime.Now.ToString());
        }

        // ATP 2013-06-08
        // Mecanismo para persistir as sessoes em disco
        // para virada do cluster ou reinicio do servico 
        // sem derrubada de sessoes ativas
        #region PersistenciaSessoes
        public void SaveSessions()
        {
            Stream stream = null;
            string path = ConfigurationManager.AppSettings["SessionsCacheFile"].ToString();

            try
            {
                logger.Info("Salvando " + _sessoes.Count + " sessoes.");

                // Copia o dicionario pra nao locar por muito tempo na serializacao
                Dictionary<string, Sessao> aux = new Dictionary<string, Sessao>();
                lock (_sessoes)
                {
                    foreach (KeyValuePair<string, Sessao> item in _sessoes)
                    {
                        aux.Add(item.Key, item.Value);
                    }
                }

                stream = File.Open(path, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();

                bformatter.Serialize(stream, _sessoes.Count);

                foreach (KeyValuePair<string, Sessao> item in aux)
                {
                    bformatter.Serialize(stream, item.Key);
                    bformatter.Serialize(stream, item.Value);
                }

                stream.Close();
                stream = null;

                logger.Info("Sessoes salvas com sucesso");
            }
            catch (Exception ex)
            {
                logger.Error("SaveSessions(): " + ex.Message, ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public void LoadSessions()
        {
            Stream stream = null;
            string path = ConfigurationManager.AppSettings["SessionsCacheFile"].ToString();

            try
            {
                stream = File.Open(path, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                int sessionCount = (int)bformatter.Deserialize(stream);

                logger.Info("Carregando " + sessionCount + " sessoes");

                int loadedSessions = 0;
                for (int i = 0; i < sessionCount; i++)
                {
                    string sessionkey = (string) bformatter.Deserialize(stream);
                    Sessao sessao = (Sessao)bformatter.Deserialize(stream);

                    lock (_sessoes)
                    {
                        if (!_sessoes.ContainsKey(sessionkey))
                        {
                            logger.Info("Carregando sessao do cache [" + sessionkey + "]");
                            _sessoes.Add(sessionkey, sessao);
                            loadedSessions++;
                        }
                    }
                }

                logger.Info("Sessoes carregadas:" + loadedSessions);

                stream.Close();
                stream = null;
            }
            catch (Exception ex)
            {
                logger.Error("LoadSessions(): " + ex.Message, ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        #endregion //PersistenciaSessoes

        private bool ValidarSenha(String pSenhaDigitada, String pSenha)
        {
            Boolean lRetorno = false;

            if (!String.IsNullOrEmpty(pSenhaDigitada) && !String.IsNullOrEmpty(pSenha))
            {
                if (pSenhaDigitada.Equals(pSenha))
                {
                    lRetorno = true;
                }
            }

            return lRetorno;
        }

        private bool ValidarSenha(SenhaInfo CaracteresDigitados, SenhaInfo Caracteres)
        {
            Boolean lRetorno = false;

            if ((CaracteresDigitados != null) && Caracteres != null)
            {
                if (CaracteresDigitados.ValidarSenha(Caracteres))
                {
                    lRetorno = true;
                }
            }

            return lRetorno;
        }
    }
}
