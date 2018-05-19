using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Implementação do serviço de persistencia de segurança
    /// </summary>
    public class ServicoSegurancaPersistencia : IServicoSegurancaPersistencia
    {
        /// <summary>
        /// Referencia para o serviço de persistencia
        /// </summary>
        private IServicoPersistencia _servicoPersistencia = Ativador.Get<IServicoPersistencia>();

        /// <summary>
        /// Coleção com as permissões do sistema
        /// </summary>
        private ListaPermissoesHelper _permissoes = new ListaPermissoesHelper();

        /// <summary>
        /// Referencia para a classe de configurações
        /// </summary>
        private ServicoSegurancaConfig _configSeguranca = GerenciadorConfig.ReceberConfig<ServicoSegurancaConfig>();

        #region Construtores

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoSegurancaPersistencia()
        {
            // Cria a lista de permissões
            _permissoes.CarregarPermissoes(_configSeguranca.NamespacesPermissoes);
        }

        #endregion

        #region IServicoSegurancaPersistencia Members

        /// <summary>
        /// Consulta de perfis de acordo com os filtros informados
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarPerfisResponse ListarPerfis(ListarPerfisRequest parametros)
        {
            // Transforma as condições em lista de condições para a pesquisa na persistencia
            List<CondicaoInfo> condicoes = new List<CondicaoInfo>();
            if (parametros.FiltroNomePerfil != null)
                condicoes.Add(
                    new CondicaoInfo(
                        "NomePerfil", CondicaoTipoEnum.Igual, parametros.FiltroNomePerfil));
            if (parametros.FiltroCodigoPerfil != null)
                condicoes.Add(
                    new CondicaoInfo(
                        "CodigoPerfil", CondicaoTipoEnum.Igual, parametros.FiltroCodigoPerfil));

            // Retorna a lista de acordo com os filtros
            return
                new ListarPerfisResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    Perfis =
                        _servicoPersistencia.ConsultarObjetos<PerfilInfo>(
                            new ConsultarObjetosRequest<PerfilInfo>() 
                            { 
                                Condicoes = condicoes 
                            }).Resultado
                };
        }

        /// <summary>
        /// Salva um perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarPerfilResponse SalvarPerfil(SalvarPerfilRequest parametros)
        {
            // Salva
            _servicoPersistencia.SalvarObjeto<PerfilInfo>(
                new SalvarObjetoRequest<PerfilInfo>() 
                { 
                    Objeto = parametros.Perfil 
                });

            // Retorna
            return
                new SalvarPerfilResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Receber o detalhe de um perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberPerfilResponse ReceberPerfil(ReceberPerfilRequest parametros)
        {
            // Faz a solicitação para a persistencia
            ReceberPerfilResponse resposta =
                new ReceberPerfilResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    Perfil =
                        _servicoPersistencia.ReceberObjeto<PerfilInfo>(
                            new ReceberObjetoRequest<PerfilInfo>()
                            {
                                CodigoObjeto = parametros.CodigoPerfil
                            }).Objeto
                };

            // Se pediu para preencher completo, verifica se está preenchido
            if (parametros.PreencherColecoesCompletas)
            {
                // Completa permissões
                foreach (PermissaoAssociadaInfo permissaoAssociada in resposta.Perfil.Permissoes)
                    permissaoAssociada.PermissaoInfo =
                        _permissoes.ListaPorCodigo[permissaoAssociada.CodigoPermissao].PermissaoInfo;
            }

            // Retorna o perfil solicitado
            return resposta;
        }

        /// <summary>
        /// Remove um perfil
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverPerfilResponse RemoverPerfil(RemoverPerfilRequest parametros)
        {
            // Remove o perfil
            _servicoPersistencia.RemoverObjeto<PerfilInfo>(
                new RemoverObjetoRequest<PerfilInfo>() 
                { 
                    CodigoObjeto = parametros.CodigoPerfil 
                });

            // Retorna
            return 
                new RemoverPerfilResponse() 
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Consulta de sessoes
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ConsultarSessoesResponse ConsultarSessoes(ConsultarSessoesRequest parametros)
        {
            // Transforma as condições em lista de condições para a pesquisa na persistencia
            List<CondicaoInfo> condicoes = new List<CondicaoInfo>();

            // Retorna a lista de acordo com os filtros
            return
                new ConsultarSessoesResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    Sessoes = 
                        _servicoPersistencia.ConsultarObjetos<SessaoInfo>(
                            new ConsultarObjetosRequest<SessaoInfo>() 
                            { 
                                Condicoes = condicoes 
                            }).Resultado
                };
        }

        /// <summary>
        /// Salva uma sessao
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarSessaoResponse SalvarSessao(SalvarSessaoRequest parametros)
        {
            // Salva
            _servicoPersistencia.SalvarObjeto<SessaoInfo>(
                new SalvarObjetoRequest<SessaoInfo>() 
                { 
                    Objeto = parametros.Sessao 
                });

            // Retorna
            return
                new SalvarSessaoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Recebe detalhe de uma sessao
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberSessaoResponse ReceberSessao(ReceberSessaoRequest parametros)
        {
            // Retorna a sessao solicitado
            return
                new ReceberSessaoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    Sessao =
                        _servicoPersistencia.ReceberObjeto<SessaoInfo>(
                            new ReceberObjetoRequest<SessaoInfo>() 
                            { 
                                CodigoObjeto = parametros.CodigoSessao 
                            }).Objeto
                };
        }

        /// <summary>
        /// Remove a sessao informada
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverSessaoResponse RemoverSessao(RemoverSessaoRequest parametros)
        {
            // Remove a sessao
            _servicoPersistencia.RemoverObjeto<SessaoInfo>(
                new RemoverObjetoRequest<SessaoInfo>() 
                { 
                    CodigoObjeto = parametros.CodigoSessao 
                });

            // Retorna
            return
                new RemoverSessaoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Consulta sistemas cliente de acordo com o filtro informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ConsultarSistemasClienteResponse ConsultarSistemasCliente(ConsultarSistemasClienteRequest parametros)
        {
            // Transforma as condições em lista de condições para a pesquisa na persistencia
            List<CondicaoInfo> condicoes = new List<CondicaoInfo>();

            // Retorna a lista de acordo com os filtros
            return
                new ConsultarSistemasClienteResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    SistemasCliente = 
                        _servicoPersistencia.ConsultarObjetos<SistemaClienteInfo>(
                            new ConsultarObjetosRequest<SistemaClienteInfo>() 
                            { 
                                Condicoes = condicoes 
                            }).Resultado
                };
        }

        /// <summary>
        /// Salva um sistema cliente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarSistemaClienteResponse SalvarSistemaCliente(SalvarSistemaClienteRequest parametros)
        {
            // Salva
            _servicoPersistencia.SalvarObjeto<SistemaClienteInfo>(
                new SalvarObjetoRequest<SistemaClienteInfo>() 
                { 
                    Objeto = parametros.SistemaCliente 
                });

            // Retorna
            return
                new SalvarSistemaClienteResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Recebe detalhe de um sistema cliente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberSistemaClienteResponse ReceberSistemaCliente(ReceberSistemaClienteRequest parametros)
        {
            // Retorna o sistema cliente solicitado
            return
                new ReceberSistemaClienteResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    SistemaCliente =
                        _servicoPersistencia.ReceberObjeto<SistemaClienteInfo>(
                            new ReceberObjetoRequest<SistemaClienteInfo>() 
                            { 
                                CodigoObjeto = parametros.CodigoSistemaCliente 
                            }).Objeto
                };
        }

        /// <summary>
        /// Remove um sistema cliente
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverSistemaClienteResponse RemoverSistemaCliente(RemoverSistemaClienteRequest parametros)
        {
            // Remove o sistema cliente
            _servicoPersistencia.RemoverObjeto<SistemaClienteInfo>(
                new RemoverObjetoRequest<SistemaClienteInfo>() 
                { 
                    CodigoObjeto = parametros.CodigoSistemaCliente 
                });

            // Retorna
            return
                new RemoverSistemaClienteResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Consulta de UsuarioGrupos
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarUsuarioGruposResponse ListarUsuarioGrupos(ListarUsuarioGruposRequest parametros)
        {
            // Transforma as condições em lista de condições para a pesquisa na persistencia
            List<CondicaoInfo> condicoes = new List<CondicaoInfo>();
            if (parametros.FiltroCodigoUsuarioGrupo != null)
                condicoes.Add(new CondicaoInfo("CodigoUsuarioGrupo", CondicaoTipoEnum.Igual, parametros.FiltroCodigoUsuarioGrupo));
            if (parametros.FiltroNomeUsuarioGrupo != null)
                condicoes.Add(new CondicaoInfo("NomeUsuarioGrupo", CondicaoTipoEnum.Igual, parametros.FiltroNomeUsuarioGrupo));

            // Retorna a lista de acordo com os filtros
            return
                new ListarUsuarioGruposResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    UsuarioGrupos = 
                        _servicoPersistencia.ConsultarObjetos<UsuarioGrupoInfo>(
                            new ConsultarObjetosRequest<UsuarioGrupoInfo>() 
                            { 
                                Condicoes = condicoes 
                            }).Resultado
                };
        }

        /// <summary>
        /// Salvar usuarioGrupo
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarUsuarioGrupoResponse SalvarUsuarioGrupo(SalvarUsuarioGrupoRequest parametros)
        {
            // Salva
            _servicoPersistencia.SalvarObjeto<UsuarioGrupoInfo>(
                new SalvarObjetoRequest<UsuarioGrupoInfo>() 
                { 
                    Objeto = parametros.UsuarioGrupo 
                });

            // Retorna
            return
                new SalvarUsuarioGrupoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Retorna um usuario grupo
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberUsuarioGrupoResponse ReceberUsuarioGrupo(ReceberUsuarioGrupoRequest parametros)
        {
            // Faz a solicitação para a persistencia
            ReceberUsuarioGrupoResponse resposta =
                new ReceberUsuarioGrupoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    UsuarioGrupo =
                        _servicoPersistencia.ReceberObjeto<UsuarioGrupoInfo>(
                            new ReceberObjetoRequest<UsuarioGrupoInfo>()
                            {
                                CodigoObjeto = parametros.CodigoUsuarioGrupo
                            }).Objeto
                };

            // Se pediu para preencher completo, verifica se está preenchido
            if (parametros.PreencherColecoesCompletas)
            {
                // Preencheu perfis?
                if (resposta.UsuarioGrupo.Perfis2 == null)
                {
                    // Cria a coleção
                    resposta.UsuarioGrupo.Perfis2 = new List<PerfilInfo>();

                    // Varre os grupos informados pedindo o detalhe do perfil
                    foreach (string codigoPerfil in resposta.UsuarioGrupo.Perfis)
                        resposta.UsuarioGrupo.Perfis2.Add(
                            this.ReceberPerfil(
                                new ReceberPerfilRequest()
                                {
                                    CodigoPerfil = codigoPerfil,
                                    PreencherColecoesCompletas = true
                                }).Perfil);
                }

                // Completa permissões
                foreach (PermissaoAssociadaInfo permissaoAssociada in resposta.UsuarioGrupo.Permissoes)
                    permissaoAssociada.PermissaoInfo =
                        _permissoes.ListaPorCodigo[permissaoAssociada.CodigoPermissao].PermissaoInfo;
            }

            // Retorna o usuario grupo solicitado
            return resposta;
        }

        /// <summary>
        /// Remove um usuário grupo
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverUsuarioGrupoResponse RemoverUsuarioGrupo(RemoverUsuarioGrupoRequest parametros)
        {
            // Remove o usuario grupo
            _servicoPersistencia.RemoverObjeto<UsuarioGrupoInfo>(
                new RemoverObjetoRequest<UsuarioGrupoInfo>() 
                { 
                    CodigoObjeto = parametros.CodigoUsuarioGrupo 
                });

            // Retorna
            return
                new RemoverUsuarioGrupoResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        /// <summary>
        /// Consulta de usuarios
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ListarUsuariosResponse ListarUsuarios(ListarUsuariosRequest parametros)
        {
            // Transforma as condições em lista de condições para a pesquisa na persistencia
            List<CondicaoInfo> condicoes = new List<CondicaoInfo>();
            if (parametros.FiltroCodigoPerfil != null)
                condicoes.Add(new CondicaoInfo("CodigoPerfil", CondicaoTipoEnum.Igual, parametros.FiltroCodigoPerfil));
            if (parametros.FiltroCodigoUsuario != null)
                condicoes.Add(new CondicaoInfo("CodigoUsuario", CondicaoTipoEnum.Igual, parametros.FiltroCodigoUsuario));
            if (parametros.FiltroCodigoUsuarioGrupo != null)
                condicoes.Add(new CondicaoInfo("CodigoUsuarioGrupo", CondicaoTipoEnum.Igual, parametros.FiltroCodigoUsuarioGrupo));
            if (parametros.FiltroNomeOuEmail != null)
                condicoes.Add(new CondicaoInfo("NomeUsuario", CondicaoTipoEnum.Igual, parametros.FiltroNomeOuEmail));
            if (parametros.FiltroStatus != null)
                condicoes.Add(new CondicaoInfo("Status", CondicaoTipoEnum.Igual, parametros.FiltroStatus));

            // Retorna a lista de acordo com os filtros
            return
                new ListarUsuariosResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    Usuarios = 
                        _servicoPersistencia.ConsultarObjetos<UsuarioInfo>(
                            new ConsultarObjetosRequest<UsuarioInfo>() 
                            { 
                                Condicoes = condicoes 
                            }).Resultado
                };
        }

        /// <summary>
        /// Salva um usuario
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SalvarUsuarioResponse SalvarUsuario(SalvarUsuarioRequest parametros)
        {
            // Faz o ajuste das relacoes com demais usuarios
            UsuarioInfo usuarioOginal = 
                _servicoPersistencia.ReceberObjeto<UsuarioInfo>(
                    new ReceberObjetoRequest<UsuarioInfo>() 
                    {
                        CodigoObjeto = parametros.Usuario.CodigoUsuario
                    }).Objeto;
            ajustarRelacoesUsuario(parametros.Usuario, usuarioOginal);
            
            // Salva
            SalvarObjetoResponse<UsuarioInfo> respostaSalvar =
                _servicoPersistencia.SalvarObjeto<UsuarioInfo>(
                    new SalvarObjetoRequest<UsuarioInfo>() 
                    { 
                        Objeto = parametros.Usuario 
                    });
            
            // Retorna
            return
                new SalvarUsuarioResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem,
                    Usuario = respostaSalvar.Objeto
                };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="usuarioOriginal"></param>
        private void ajustarRelacoesUsuario(UsuarioInfo usuario, UsuarioInfo usuarioOriginal)
        {
            // Inicializa
            List<UsuarioRelacaoInfo> relacoesAtualizar = new List<UsuarioRelacaoInfo>();
            List<UsuarioRelacaoInfo> relacoesRemover = new List<UsuarioRelacaoInfo>();

            // Verifica quais relacoes devem ser atualizadas
            foreach (UsuarioRelacaoInfo relacao in usuario.Relacoes)
                if (usuarioOriginal != null && localizarRelacao(relacao, usuarioOriginal) == null)
                    relacoesAtualizar.Add(relacao);

            // Verifica quais relacoes foram removidas
            if (usuarioOriginal != null)
                foreach (UsuarioRelacaoInfo relacao in usuarioOriginal.Relacoes)
                    if (localizarRelacao(relacao, usuario) == null)
                        relacoesRemover.Add(relacao);

            // Relacoes a atualizar
            foreach (UsuarioRelacaoInfo relacao in relacoesAtualizar)
            {
                // Acha o codigo do usuario a atualizar
                string codigoUsuario = 
                    relacao.CodigoUsuario1 != usuario.CodigoUsuario ? relacao.CodigoUsuario1 : relacao.CodigoUsuario2;

                // Recebe o usuario
                UsuarioInfo usuarioAtualizar =
                    _servicoPersistencia.ReceberObjeto<UsuarioInfo>(
                        new ReceberObjetoRequest<UsuarioInfo>() 
                        { 
                            CodigoObjeto = codigoUsuario
                        }).Objeto;

                // Cria a relacao
                usuarioAtualizar.Relacoes.Add(relacao);

                // Salva
                _servicoPersistencia.SalvarObjeto<UsuarioInfo>(
                    new SalvarObjetoRequest<UsuarioInfo>()
                    {
                        Objeto = usuarioAtualizar
                    });
            }

            // Relacoes a remover
            foreach (UsuarioRelacaoInfo relacao in relacoesRemover)
            {
                // Acha o codigo do usuario a atualizar
                string codigoUsuario =
                    relacao.CodigoUsuario1 != usuario.CodigoUsuario ? relacao.CodigoUsuario1 : relacao.CodigoUsuario2;

                // Recebe o usuario
                UsuarioInfo usuarioRemover =
                    _servicoPersistencia.ReceberObjeto<UsuarioInfo>(
                        new ReceberObjetoRequest<UsuarioInfo>()
                        {
                            CodigoObjeto = codigoUsuario
                        }).Objeto;

                // Cria a relacao
                usuarioRemover.Relacoes.Remove(localizarRelacao(relacao, usuarioRemover));

                // Salva
                _servicoPersistencia.SalvarObjeto<UsuarioInfo>(
                    new SalvarObjetoRequest<UsuarioInfo>()
                    {
                        Objeto = usuarioRemover
                    });
            }
        }

        /// <summary>
        /// Verifica na lista de relacoes do usuario alvo, se há alguma relacao
        /// com as caracteristicas da relacao informada
        /// </summary>
        /// <param name="relacaoALocalizar"></param>
        /// <param name="usuarioAlvo"></param>
        private UsuarioRelacaoInfo localizarRelacao(UsuarioRelacaoInfo relacaoALocalizar, UsuarioInfo usuarioAlvo)
        {
            // Tenta localizar
            UsuarioRelacaoInfo relacao =
                (from r in usuarioAlvo.Relacoes
                 where r.CodigoUsuario1 == relacaoALocalizar.CodigoUsuario1 &&
                       r.CodigoUsuario2 == relacaoALocalizar.CodigoUsuario2 &&
                       r.TipoRelacao == relacaoALocalizar.TipoRelacao
                 select r).FirstOrDefault();

            // Retorna
            return relacao;
        }

        /// <summary>
        /// Retorna o detalhe de um usuário
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ReceberUsuarioResponse ReceberUsuario(ReceberUsuarioRequest parametros)
        {
            // Faz a solicitação para a persistencia
            ReceberUsuarioResponse resposta = 
                new ReceberUsuarioResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };

            // Faz a consulta pelo código ou pelo email
            if (parametros.CodigoUsuario != null)
            {
                // Consulta por código do usuário
                resposta.Usuario =
                    _servicoPersistencia.ReceberObjeto<UsuarioInfo>(
                        new ReceberObjetoRequest<UsuarioInfo>()
                        {
                            CodigoObjeto = parametros.CodigoUsuario
                        }).Objeto;
            }
            else
            {
                // Consulta por email
                ConsultarObjetosResponse<UsuarioInfo> respostaConsultar =
                    _servicoPersistencia.ConsultarObjetos<UsuarioInfo>(
                        new ConsultarObjetosRequest<UsuarioInfo>()
                        {
                            CodigoSessao = parametros.CodigoSessao,
                            Condicoes =
                                new List<CondicaoInfo>() 
                                { 
                                    new CondicaoInfo("Email", CondicaoTipoEnum.Igual, parametros.Email)
                                }
                        });

                // Verifica se encontrou
                if (respostaConsultar.Resultado.Count > 0)
                    resposta.Usuario = respostaConsultar.Resultado[0];
            }

            // Se pediu para preencher completo, verifica se está preenchido
            if (parametros.PreencherColecoesCompletas)
            {
                // Preencheu grupos?
                if (resposta.Usuario.Grupos2 == null)
                {
                    // Cria a coleção
                    resposta.Usuario.Grupos2 = new List<UsuarioGrupoInfo>();

                    // Varre os grupos informados pedindo o detalhe do grupo
                    foreach (string codigoGrupo in resposta.Usuario.Grupos)
                        resposta.Usuario.Grupos2.Add(
                            this.ReceberUsuarioGrupo(
                                new ReceberUsuarioGrupoRequest()
                                {
                                    CodigoUsuarioGrupo = codigoGrupo,
                                    PreencherColecoesCompletas = true
                                }).UsuarioGrupo);
                }

                // Preencheu perfis?
                if (resposta.Usuario.Perfis2 == null)
                {
                    // Cria a coleção
                    resposta.Usuario.Perfis2 = new List<PerfilInfo>();

                    // Varre os grupos informados pedindo o detalhe do perfil
                    foreach (string codigoPerfil in resposta.Usuario.Perfis)
                        resposta.Usuario.Perfis2.Add(
                            this.ReceberPerfil(
                                new ReceberPerfilRequest()
                                {
                                    CodigoPerfil = codigoPerfil,
                                    PreencherColecoesCompletas = true
                                }).Perfil);
                }

                // Completa permissões
                foreach (PermissaoAssociadaInfo permissaoAssociada in resposta.Usuario.Permissoes)
                    permissaoAssociada.PermissaoInfo =
                        _permissoes.ListaPorCodigo[permissaoAssociada.CodigoPermissao].PermissaoInfo;
            }

            // Retorna o usuario solicitado
            return resposta;
        }

        /// <summary>
        /// Remove o usuário informado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public RemoverUsuarioResponse RemoverUsuario(RemoverUsuarioRequest parametros)
        {
            // Remove o usuario grupo
            _servicoPersistencia.RemoverObjeto<UsuarioInfo>(
                new RemoverObjetoRequest<UsuarioInfo>() 
                { 
                    CodigoObjeto = parametros.CodigoUsuario 
                });

            // Retorna
            return
                new RemoverUsuarioResponse()
                {
                    CodigoMensagemRequest = parametros.CodigoMensagem
                };
        }

        #endregion
    }
}
