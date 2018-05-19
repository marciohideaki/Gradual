using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Comum.Permissoes;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Implementação do serviço para controle de segurança
    /// </summary>
    public class ServicoSeguranca : IServicoSeguranca
    {
        #region Variaveis Locais

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

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoSeguranca()
        {
            // Cria a lista de permissões
            _permissoes.CarregarPermissoes(_config.NamespacesPermissoes);

            // Carrega os complementos
            foreach (ComplementoAutenticacaoInfo complementoInfo in _config.ComplementosAutenticacao)
            {
                // Habilitado?
                if (complementoInfo.Habilitado)
                {
                    // Cria instancia
                    IComplementoAutenticacao complementoInstancia = 
                        (IComplementoAutenticacao)
                            Activator.CreateInstance(complementoInfo.TipoComplemento);

                    // Adiciona no dicionário
                    _complementosAutenticacao.Add(complementoInfo, complementoInstancia);
                }
            }

            // Verifica se deve inicializar segurança no início
            if (_config.InicializarAutomaticamente)
                this.InicializarSeguranca(
                    new InicializarSegurancaRequest());
        }

        #endregion

        #region IServicoSeguranca Members

        #region Comum

        /// <summary>
        /// Método genérico para processamento de mensagem
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public MensagemResponseBase ProcessarMensagem(MensagemRequestBase parametros)
        {
            // Inicializa
            Type tipo = parametros.GetType();
            MensagemResponseBase resposta = null;

            // Bloco de controle
            try
            {
                // Faz o log da mensagem
                Log.EfetuarLog(Serializador.TransformarEmString(parametros), LogTipoEnum.Passagem, ModulosOMS.ModuloSeguranca);

                // Repassa a chamada de acordo com o tipo da mensagem de parametros
                if (tipo == typeof(AssociarPerfilRequest))
                    resposta = this.AssociarPerfil((AssociarPerfilRequest)parametros);
                else if (tipo == typeof(AssociarPermissaoRequest))
                    resposta = this.AssociarPermissao((AssociarPermissaoRequest)parametros);
                else if (tipo == typeof(AssociarUsuarioGrupoRequest))
                    resposta = this.AssociarUsuarioGrupo((AssociarUsuarioGrupoRequest)parametros);
                else if (tipo == typeof(AutenticarUsuarioRequest))
                    resposta = this.AutenticarUsuario((AutenticarUsuarioRequest)parametros);
                else if (tipo == typeof(ListarPerfisRequest))
                    resposta = this.ListarPerfis((ListarPerfisRequest)parametros);
                else if (tipo == typeof(ListarPermissoesRequest))
                    resposta = this.ListarPermissoes((ListarPermissoesRequest)parametros);
                else if (tipo == typeof(ListarUsuarioGruposRequest))
                    resposta = this.ListarUsuarioGrupos((ListarUsuarioGruposRequest)parametros);
                else if (tipo == typeof(ListarUsuariosRequest))
                    resposta = this.ListarUsuarios((ListarUsuariosRequest)parametros);
                else if (tipo == typeof(ReceberPerfilRequest))
                    resposta = this.ReceberPerfil((ReceberPerfilRequest)parametros);
                else if (tipo == typeof(ReceberUsuarioRequest))
                    resposta = this.ReceberUsuario((ReceberUsuarioRequest)parametros);
                else if (tipo == typeof(ReceberUsuarioGrupoRequest))
                    resposta = this.ReceberUsuarioGrupo((ReceberUsuarioGrupoRequest)parametros);
                else if (tipo == typeof(RemoverPerfilRequest))
                    resposta = this.RemoverPerfil((RemoverPerfilRequest)parametros);
                else if (tipo == typeof(RemoverUsuarioRequest))
                    resposta = this.RemoverUsuario((RemoverUsuarioRequest)parametros);
                else if (tipo == typeof(RemoverUsuarioGrupoRequest))
                    resposta = this.RemoverUsuarioGrupo((RemoverUsuarioGrupoRequest)parametros);
                else if (tipo == typeof(SalvarPerfilRequest))
                    resposta = this.SalvarPerfil((SalvarPerfilRequest)parametros);
                else if (tipo == typeof(SalvarUsuarioRequest))
                    resposta = this.SalvarUsuario((SalvarUsuarioRequest)parametros);
                else if (tipo == typeof(SalvarUsuarioGrupoRequest))
                    resposta = this.SalvarUsuarioGrupo((SalvarUsuarioGrupoRequest)parametros);
                else if (tipo == typeof(VerificarPermissaoRequest))
                    resposta = this.VerificarPermissao((VerificarPermissaoRequest)parametros);
                else if (tipo == typeof(ValidarAssinaturaEletronicaRequest))
                    resposta = this.ValidarAssinaturaEletronica((ValidarAssinaturaEletronicaRequest)parametros);
                else if (tipo == typeof(AlterarAssinaturaEletronicaRequest))
                    resposta = this.AlterarAssinaturaEletronica((AlterarAssinaturaEletronicaRequest)parametros);
            }
            catch (Exception ex)
            {
                // Se tem mensagem, informa
                if (resposta != null)
                {
                    resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                    resposta.DescricaoResposta = ex.ToString();
                }

                // Efetua log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloSeguranca);
            }

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Inicializa o serviço de segurança.
        /// Garante a existencia do usuário administrador
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public InicializarSegurancaResponse InicializarSeguranca(InicializarSegurancaRequest parametros)
        {
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

            // Retorna
            return 
                new InicializarSegurancaResponse() 
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
            // Prepara resposta
            AutenticarUsuarioResponse resposta =
                new AutenticarUsuarioResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
            
            // Bloco de controle
            try
            {
                // Recebe o usuario informado
                UsuarioInfo usuarioInfo = _servicoPersistencia.ReceberUsuario(
                    new ReceberUsuarioRequest()
                    {
                        CodigoUsuario = parametros.CodigoUsuario,
                        Email = parametros.Email
                    }).Usuario;

                // Achou usuario?
                if (usuarioInfo != null)
                {
                    // Valida a senha
                    if (!_config.ValidarSenhas || parametros.Senha == usuarioInfo.Senha)
                    {
                        // Cria o usuario
                        Usuario usuario = new Usuario(usuarioInfo);

                        // Verifica se é admin ou possui permissão de acesso ao sistema
                        bool permiteAcesso =
                            usuario.PermissoesAssociadas.EhAdministrador ||
                            usuario.PermissoesAssociadas.ConsultarPermissao(
                                typeof(PermissaoAcessarSistema));

                        // Permite o acesso?
                        if (permiteAcesso)
                        {
                            // Cria sessaoInfo
                            SessaoInfo sessaoInfo =
                                new SessaoInfo()
                                {
                                    CodigoSistemaCliente = parametros.CodigoSistemaCliente,
                                    CodigoUsuario = usuarioInfo.CodigoUsuario,
                                    DataCriacao = DateTime.Now,
                                    DataUltimaConsulta = DateTime.Now,
                                    EhSessaoDeAdministrador = usuario.PermissoesAssociadas.EhAdministrador
                                };

                            // Inicia a criação da sessao
                            Sessao sessao =
                                new Sessao(sessaoInfo)
                                {
                                    Usuario = usuario
                                };

                            // Adiciona na lista de sessoes
                            _sessoes.Add(sessao.SessaoInfo.CodigoSessao, sessao);

                            // Passa pelos complementos. Eles complementar ou barrar a autenticação do usuário
                            bool resultadoComplementos = true;
                            foreach (KeyValuePair<ComplementoAutenticacaoInfo, IComplementoAutenticacao> item in _complementosAutenticacao)
                                resultadoComplementos =
                                    resultadoComplementos &&
                                    (item.Value.ComplementarAutenticacao(parametros, sessao).StatusResposta == MensagemResponseStatusEnum.OK);

                            // Salva o usuário para salvar os complementos
                            _servicoPersistencia.SalvarUsuario(
                                new SalvarUsuarioRequest()
                                {
                                    CodigoSessao = sessao.SessaoInfo.CodigoSessao,
                                    Usuario = sessao.Usuario.UsuarioInfo
                                });

                            // Permitido?
                            if (resultadoComplementos)
                            {
                                // Salva a sessaoInfo?

                                // Informa na resposta
                                resposta.Sessao = sessao.SessaoInfo;
                                resposta.StatusResposta = MensagemResponseStatusEnum.OK;
                            }
                            else
                            {
                                // Informa na resposta
                                resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                                resposta.DescricaoResposta = "Usuário não possui acesso ao sistema";
                            }
                        }
                        else
                        {
                            // Informa na resposta
                            resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                            resposta.DescricaoResposta = "Usuário não possui acesso ao sistema";
                        }
                    }
                    else
                    {
                        // Informa senha inválida
                        resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                        resposta.DescricaoResposta = "Login inválido";
                    }
                }
                else
                {
                    // Informa usuário inválido
                    resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                    resposta.DescricaoResposta = "Usuário inválido";
                }
            }
            catch (Exception ex)
            {
                // Log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloSeguranca);

                // Informa na mensagem
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            // Retorna
            return resposta;
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
            // Prepara resposta
            VerificarPermissaoResponse resposta =
                new VerificarPermissaoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Bloco de controle
            try
            {
                // Recupera a sessao
                Sessao sessao = null;
                if (_sessoes.ContainsKey(parametros.CodigoSessao))
                    sessao = _sessoes[parametros.CodigoSessao];

                // Achou a sessao?
                if (sessao != null)
                {
                    // Faz a consulta da permissao
                    if (parametros.CodigoPermissao != null)
                        resposta.Permitido = sessao.Usuario.PermissoesAssociadas.ConsultarPermissao(parametros.CodigoPermissao);
                    else
                        resposta.Permitido = sessao.Usuario.PermissoesAssociadas.ConsultarPermissao(parametros.TipoPermissao);
                }
                else
                {
                    // Informa na resposta
                    resposta.Permitido = false;
                    resposta.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                    resposta.DescricaoResposta = "Sessão inválida";
                }
            }
            catch (Exception ex)
            {
                // Log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloSeguranca);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita a lista de permissões do sistema
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarPermissoesResponse ListarPermissoes(ListarPermissoesRequest parametros)
        {
            // Retorna a lista de permissões
            return
                new ListarPermissoesResponse()
                {
                    Permissoes = this._permissoes.ListaPorTipo.Values.ToList()
                };
        }

        /// <summary>
        /// Solicita a adição de uma permissão a usuários, grupos ou perfis
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AssociarPermissaoResponse AssociarPermissao(AssociarPermissaoRequest parametros)
        {
            // Prepara resposta
            AssociarPermissaoResponse resposta =
                new AssociarPermissaoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Bloco de controle
            try
            {

                // Verifica se a sessao tem permissao para adicionar permissao
                VerificarPermissaoResponse verificarAdicionarPermissao =
                    this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            TipoPermissao = typeof(PermissaoAssociarPermissao)
                        });

                // Tem?
                if (verificarAdicionarPermissao.Permitido)
                {
                    // Localiza a permissao
                    PermissaoBase permissao = null;
                    if (parametros.CodigoPermissao != null)
                        permissao = _permissoes.ListaPorCodigo[permissao.PermissaoInfo.CodigoPermissao];
                    else
                        permissao = _permissoes.ListaPorTipo[parametros.TipoPermissao];

                    // Cria a associação de permissao
                    PermissaoAssociadaInfo permissaoAssociada =
                        new PermissaoAssociadaInfo()
                        {
                            CodigoPermissao = permissao.PermissaoInfo.CodigoPermissao,
                            Status = parametros.StatusPermissao
                        };

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
                            // Verifica se cria ou altera a existente
                            PermissaoAssociadaInfo permissaoAssociadaExistente =
                                (from p in usuarioInfo.Permissoes
                                 where p.CodigoPermissao == permissao.PermissaoInfo.CodigoPermissao
                                 select p).FirstOrDefault();
                            if (permissaoAssociadaExistente != null)
                                permissaoAssociadaExistente.Status = permissaoAssociada.Status;
                            else
                                usuarioInfo.Permissoes.Add(permissaoAssociada);

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

                    // Associa a usuário grupo?
                    if (parametros.CodigoUsuarioGrupo != null)
                    {
                        // Carrega o usuario
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
                            PermissaoAssociadaInfo permissaoAssociadaExistente =
                                (from p in usuarioGrupoInfo.Permissoes
                                 where p.CodigoPermissao == permissao.PermissaoInfo.CodigoPermissao
                                 select p).FirstOrDefault();
                            if (permissaoAssociadaExistente != null)
                                permissaoAssociadaExistente.Status = permissaoAssociada.Status;
                            else
                                usuarioGrupoInfo.Permissoes.Add(permissaoAssociada);

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

                    // Associa a perfil?
                    if (parametros.CodigoPerfil != null)
                    {
                        // Carrega o perfil
                        PerfilInfo perfilInfo = _servicoPersistencia.ReceberPerfil(
                            new ReceberPerfilRequest()
                            {
                                CodigoSessao = parametros.CodigoSessao,
                                CodigoPerfil = parametros.CodigoPerfil
                            }).Perfil;

                        // Apenas se tem usuário
                        if (perfilInfo != null)
                        {
                            // Verifica se cria ou altera a existente
                            PermissaoAssociadaInfo permissaoAssociadaExistente =
                                (from p in perfilInfo.Permissoes
                                 where p.CodigoPermissao == permissao.PermissaoInfo.CodigoPermissao
                                 select p).FirstOrDefault();
                            if (permissaoAssociadaExistente != null)
                                permissaoAssociadaExistente.Status = permissaoAssociada.Status;
                            else
                                perfilInfo.Permissoes.Add(permissaoAssociada);

                            // Salva o perfil
                            // ** Se o usuário não tiver permissões para salvar perfil, aqui
                            // ** não vai salvar.
                            _servicoPersistencia.SalvarPerfil(
                                new SalvarPerfilRequest()
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
                // Log
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloSeguranca);

                // Informa na resposta
                resposta.DescricaoResposta = ex.ToString();
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            // Retorna
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
            ValidarItemSegurancaResponse resposta =
                new ValidarItemSegurancaResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Sessao válida?
            if (_sessoes.ContainsKey(parametros.CodigoSessao))
            {
                // Pega a sessao
                Sessao sessao = _sessoes[parametros.CodigoSessao];

                // Varre os itens
                foreach (ItemSegurancaInfo itemSeguranca in parametros.ItensSeguranca)
                {
                    // Sinalizadores
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
                            Type tipoPermissao = Type.GetType(permissaoStr);
                            if (tipoPermissao != null)
                                codigoPermissao = _permissoes.ListaPorTipo[tipoPermissao].PermissaoInfo.CodigoPermissao;

                            // Tem a permissão?
                            if (sessao.Usuario.PermissoesAssociadas.ConsultarPermissao(codigoPermissao))
                                temItem = true;
                            else
                                naoTemItem = true;
                        }
                    }

                    // Deve continuar? Se for todas as condições e já deu alguma negativa não precisa continuar
                    if ((itemSeguranca.TipoAtivacao == ItemSegurancaAtivacaoTipoEnum.TodasAsCondicoes && !naoTemItem) ||
                        itemSeguranca.TipoAtivacao != ItemSegurancaAtivacaoTipoEnum.TodasAsCondicoes)
                    {
                        // Tem validação de perfis?
                        if (itemSeguranca.Perfis != null)
                        {
                            // Varre os perfis
                            foreach (string codigoPerfil in itemSeguranca.Perfis)
                            {
                                // Tem o perfil?
                                if (sessao.Usuario.Perfis.ContainsKey(codigoPerfil))
                                    temItem = true;
                                else
                                    naoTemItem = true;
                            }
                        }
                    }

                    // Deve continuar? Se for todas as condições e já deu alguma negativa não precisa continuar
                    if ((itemSeguranca.TipoAtivacao == ItemSegurancaAtivacaoTipoEnum.TodasAsCondicoes && !naoTemItem) ||
                        itemSeguranca.TipoAtivacao != ItemSegurancaAtivacaoTipoEnum.TodasAsCondicoes)
                    {
                        // Valida grupos
                        if (itemSeguranca.Grupos != null)
                        {
                            // Varre os grupos
                            foreach (string codigoGrupo in itemSeguranca.Grupos)
                            {
                                // Tem o grupo?
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

            // Retorna
            return resposta;
        }

        /// <summary>
        /// Solicita que sejam salvas todas as permissões do sistema.
        /// A persistencia se encarregará de salvar no local apropriado.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarPermissoesResponse SalvarPermissoes(SalvarPermissoesRequest parametros)
        {
            // Referencia para o serviço de persistencia
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();

            // Pede a lista de permissoes
            ListarPermissoesResponse responseListar =
                this.ListarPermissoes(
                    new ListarPermissoesRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao
                    });

            // Salva na persistencia
            foreach (PermissaoBase permissao in responseListar.Permissoes)
                servicoPersistencia.SalvarObjeto<PermissaoInfo>(
                    new SalvarObjetoRequest<PermissaoInfo>()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        Objeto = permissao.PermissaoInfo
                    });

            // Retorna
            return 
                new SalvarPermissoesResponse() 
                { 
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
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
                VerificarPermissaoResponse verificarListarUsuarios =
                    this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            TipoPermissao = typeof(PermissaoListarUsuarios)
                        });

                // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
                // pela aplicação e repassa a mensagem para o servico de persistencia
                if (verificarListarUsuarios.Permitido)
                    resposta = _servicoPersistencia.ListarUsuarios(parametros);
                else
                    resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;
            }
            catch (Exception ex)
            {
                // Loga a mensagem
                Log.EfetuarLog(ex, parametros, ModulosOMS.ModuloSeguranca);

                // Devolve o status na resposta
                resposta.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                resposta.DescricaoResposta = ex.ToString();
            }

            // Retorna 
            Log.EfetuarLog("Listar Usuarios 99", LogTipoEnum.Passagem, ModulosOMS.ModuloSeguranca);
            return resposta;
        }

        /// <summary>
        /// Salva o usuário informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarUsuarioResponse SalvarUsuario(SalvarUsuarioRequest parametros)
        {
            // Prepara a resposta
            SalvarUsuarioResponse resposta =
                new SalvarUsuarioResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Verifica se a sessao tem permissao para salvar usuario
            VerificarPermissaoResponse verificarSalvarUsuario =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest() 
                    { 
                        CodigoSessao = parametros.CodigoSessao,
                        TipoPermissao = typeof(PermissaoSalvarUsuario)
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

                // Tem permissao para associar permissoes?
                if (!this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            TipoPermissao = typeof(PermissaoAssociarPermissao)
                        }).Permitido)
                {
                    if (usuarioInfoOriginal != null)
                        parametros.Usuario.Permissoes = usuarioInfoOriginal.Permissoes;
                    else
                        parametros.Usuario.Perfis.Clear();
                }

                // Tem permissao para associar perfis?
                if (!this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            TipoPermissao = typeof(PermissaoAssociarPerfil)
                        }).Permitido)
                {
                    if (usuarioInfoOriginal != null)
                        parametros.Usuario.Perfis = usuarioInfoOriginal.Perfis;
                    else
                        parametros.Usuario.Perfis.Clear();
                }

                // Tem permissao para associar grupos?
                if (!this.VerificarPermissao(
                        new VerificarPermissaoRequest()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            TipoPermissao = typeof(PermissaoAssociarUsuarioGrupo)
                        }).Permitido)
                {
                    if (usuarioInfoOriginal != null)
                        parametros.Usuario.Grupos = usuarioInfoOriginal.Grupos;
                    else
                        parametros.Usuario.Grupos.Clear();
                }

                // Salva o usuário
                resposta = _servicoPersistencia.SalvarUsuario(parametros);
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

            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicitação de remoção do usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverUsuarioResponse RemoverUsuario(RemoverUsuarioRequest parametros)
        {
            // Prepara a resposta
            RemoverUsuarioResponse resposta =
                new RemoverUsuarioResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Verifica se a sessao tem permissao 
            VerificarPermissaoResponse verificarPermissao =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        TipoPermissao = typeof(PermissaoRemoverUsuario)
                    });

            // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
            // pela aplicação e repassa a mensagem para o servico de persistencia
            if (verificarPermissao.Permitido)
                resposta = _servicoPersistencia.RemoverUsuario(parametros);
            else
                resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;

            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicita a validação da assinatura eletrônica
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ValidarAssinaturaEletronicaResponse ValidarAssinaturaEletronica(ValidarAssinaturaEletronicaRequest parametros)
        {
            // Pega a sessao
            Sessao sessao = _sessoes[parametros.CodigoSessao];

            // Valida
            bool valido = sessao.Usuario.UsuarioInfo.AssinaturaEletronica == parametros.AssinaturaEletronica;

            // Retorna
            return 
                new ValidarAssinaturaEletronicaResponse() 
                { 
                    StatusResposta = valido ? MensagemResponseStatusEnum.OK : MensagemResponseStatusEnum.AcessoNaoPermitido,
                    DescricaoResposta = valido ? null : "Assinatura inválida"
                };
        }

        /// <summary>
        /// Solicita alteração da assinatura eletrônica para a sessão
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AlterarAssinaturaEletronicaResponse AlterarAssinaturaEletronica(AlterarAssinaturaEletronicaRequest parametros)
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

            // Retorna
            return new AlterarAssinaturaEletronicaResponse();
        }

        /// <summary>
        /// Solicita alteração de senha do usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AlterarSenhaResponse AlterarSenha(AlterarSenhaRequest parametros)
        {
            // Prepara resposta
            AlterarSenhaResponse resposta =
                new AlterarSenhaResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Pega o usuário
            UsuarioInfo usuarioInfo =
                _servicoPersistencia.ReceberUsuario(
                    new ReceberUsuarioRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        CodigoUsuario = parametros.CodigoUsuario
                    }).Usuario;

            // Verifica se a senha bate com a atual
            if (parametros.SenhaAtual == usuarioInfo.Senha)
            {
                // Altera a assinatura do usuario e no usuario da sessao
                usuarioInfo.Senha = parametros.NovaSenha;

                // Salva o usuario
                _servicoPersistencia.SalvarUsuario(
                    new SalvarUsuarioRequest()
                    {
                        Usuario = usuarioInfo
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
                        TipoPermissao = typeof(PermissaoListarUsuarioGrupos)
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
        public SalvarUsuarioGrupoResponse SalvarUsuarioGrupo(SalvarUsuarioGrupoRequest parametros)
        {
            // Prepara a resposta
            SalvarUsuarioGrupoResponse resposta =
                new SalvarUsuarioGrupoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Verifica se a sessao tem permissao para salvar usuario
            VerificarPermissaoResponse verificarSalvarUsuarioGrupo =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        TipoPermissao = typeof(PermissaoSalvarUsuarioGrupo)
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
                            TipoPermissao = typeof(PermissaoAssociarPermissao)
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
                            TipoPermissao = typeof(PermissaoAssociarPerfil)
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
        public AssociarUsuarioGrupoResponse AssociarUsuarioGrupo(AssociarUsuarioGrupoRequest parametros)
        {
            // Prepara resposta
            AssociarUsuarioGrupoResponse resposta =
                new AssociarUsuarioGrupoResponse()
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
                            TipoPermissao = typeof(PermissaoAssociarUsuarioGrupo)
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
                        TipoPermissao = typeof(PermissaoReceberUsuarioGrupo)
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
        public RemoverUsuarioGrupoResponse RemoverUsuarioGrupo(RemoverUsuarioGrupoRequest parametros)
        {
            // Prepara a resposta
            RemoverUsuarioGrupoResponse resposta =
                new RemoverUsuarioGrupoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Verifica se a sessao tem permissao 
            VerificarPermissaoResponse verificarPermissao =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        TipoPermissao = typeof(PermissaoRemoverUsuarioGrupo)
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

            // Verifica se a sessao tem permissao para salvar usuario
            VerificarPermissaoResponse verificarListarPerfis =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        TipoPermissao = typeof(PermissaoListarPerfis)
                    });

            // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
            // pela aplicação e repassa a mensagem para o servico de persistencia
            if (verificarListarPerfis.Permitido)
                resposta = _servicoPersistencia.ListarPerfis(parametros);
            else
                resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;

            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicitação para salvar um perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarPerfilResponse SalvarPerfil(SalvarPerfilRequest parametros)
        {
            // Prepara a resposta
            SalvarPerfilResponse resposta =
                new SalvarPerfilResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Verifica se a sessao tem permissao para salvar usuario
            VerificarPermissaoResponse verificarSalvarPerfil =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        TipoPermissao = typeof(PermissaoSalvarPerfil)
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
                            TipoPermissao = typeof(PermissaoAssociarPermissao)
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

            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicita a associação de um perfil com alguma entidade
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public AssociarPerfilResponse AssociarPerfil(AssociarPerfilRequest parametros)
        {
            // Prepara resposta
            AssociarPerfilResponse resposta =
                new AssociarPerfilResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

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
                            TipoPermissao = typeof(PermissaoAssociarPerfil)
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

            // Verifica se a sessao tem permissao 
            VerificarPermissaoResponse verificarPermissao =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        TipoPermissao = typeof(PermissaoReceberPerfil)
                    });

            // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
            // pela aplicação e repassa a mensagem para o servico de persistencia
            if (verificarPermissao.Permitido)
                resposta = _servicoPersistencia.ReceberPerfil(parametros);
            else
                resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;

            // Retorna 
            return resposta;
        }

        /// <summary>
        /// Solicitação remoção do perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverPerfilResponse RemoverPerfil(RemoverPerfilRequest parametros)
        {
            // Prepara a resposta
            RemoverPerfilResponse resposta =
                new RemoverPerfilResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Verifica se a sessao tem permissao 
            VerificarPermissaoResponse verificarPermissao =
                this.VerificarPermissao(
                    new VerificarPermissaoRequest()
                    {
                        CodigoSessao = parametros.CodigoSessao,
                        TipoPermissao = typeof(PermissaoRemoverPerfil)
                    });

            // Se tem a permissao, ajusta as propriedades que não podem ser alteradas
            // pela aplicação e repassa a mensagem para o servico de persistencia
            if (verificarPermissao.Permitido)
                resposta = _servicoPersistencia.RemoverPerfil(parametros);
            else
                resposta.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;

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

            // Retorna
            return resposta;
        }

        #endregion

        #endregion
    }
}
