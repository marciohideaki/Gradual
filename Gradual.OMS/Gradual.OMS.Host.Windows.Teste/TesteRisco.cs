using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Comum.Permissoes;
using Gradual.OMS.Contratos.ContaCorrente;
using Gradual.OMS.Contratos.ContaCorrente.Dados;
using Gradual.OMS.Contratos.ContaCorrente.Mensagens;
using Gradual.OMS.Contratos.Custodia;
using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Contratos.Custodia.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Contratos.Risco.Dados;
using Gradual.OMS.Contratos.Risco.Mensagens;
using Gradual.OMS.Contratos.Risco.Regras;
using Gradual.OMS.Contratos.Risco.Regras.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.Risco.Regras;

using Orbite.RV.Contratos.Integracao.BVMF.Legado;
using Orbite.RV.Contratos.Integracao.BVMF.Legado.Mensagens;
using Orbite.RV.Contratos.MarketData.BMF;
using Orbite.RV.Contratos.MarketData.BMF.Dados;
using Orbite.RV.Contratos.MarketData.BMF.Mensagens;
using Orbite.RV.Contratos.MarketData.Bovespa;
using Orbite.RV.Contratos.MarketData.Bovespa.Dados;
using Orbite.RV.Contratos.MarketData.Bovespa.Mensagens;

namespace Gradual.OMS.Host.Windows.Teste
{
    /// <summary>
    /// Classe de teste para rotinas do risco, custodia, market data
    /// </summary>
    public class TesteRisco
    {
        #region Contantes - ou, informações de entrada dos testes

        /// <summary>
        /// Nome do usuário admin
        /// </summary>
        private string _consUsuarioAdmin = "Admin";

        /// <summary>
        /// Senha do usuario admin
        /// </summary>
        private string _consUsuarioAdminSenha = "123";

        /// <summary>
        /// Nome do usuário de teste
        /// </summary>
        private string _consUsuarioTeste = "Teste";

        /// <summary>
        /// Senha do usuário de teste
        /// </summary>
        private string _consUsuarioTesteSenha = "123";

        /// <summary>
        /// Código CBLC utilizado para sincronismo com o Sinacor
        /// Códigos possíveis:
        /// - 31940: Rafael
        /// - 39085: ??
        /// - 2833: Fundo
        /// </summary>
        private string _consCodigoCBLC = "2833";

        /// <summary>
        /// Código da custódia a ser utilizada
        /// </summary>
        private string _consCodigoCustodia = "09EC3965-A48D-4d19-9C75-355894077F61";

        /// <summary>
        /// Código da conta corrente a ser utilizada
        /// </summary>
        private string _consCodigoContaCorrente = "927582A3-6168-4f04-B55D-0D2626CE8285";

        /// <summary>
        /// Indica se a execução está em ambiente local (stand alone) ou conectado
        /// </summary>
        private bool _consExecutarLocal = true;

        /// <summary>
        /// Caminho do arquivo de layouts antigo
        /// </summary>
        private string _consCaminhoLayoutsLegado = @"K:\Projetos\LA\Config\layouts.xml";

        /// <summary>
        /// Código da regra de risco 1
        /// </summary>
        private string _consCodigoRegra1 = "CE187708-911F-41b5-9C03-19CA315870CF";

        /// <summary>
        /// Código da regra de risco 2
        /// </summary>
        private string _consCodigoRegra2 = "0C2877CE-B788-4dff-8611-0F22ACCD807E";

        /// <summary>
        /// Código da regra de risco 3
        /// </summary>
        private string _consCodigoRegra3 = "1A06EF39-2065-4a11-8B9F-FE64CE55C774";

        /// <summary>
        /// Código do perfil de risco
        /// </summary>
        private string _consCodigoPerfilRisco = "EF9AF9A5-2627-4da3-90B7-3709BC8CF0DC";

        /// <summary>
        /// Código do perfil de risco do usuario
        /// </summary>
        private string _consCodigoPerfilRiscoUsuario = null;

        #endregion

        #region Variáveis Locais

        /// <summary>
        /// Referencia para o servico de mensageria
        /// </summary>
        private IServicoMensageria _servicoMensageria = Ativador.Get<IServicoMensageria>();

        /// <summary>
        /// Informações da sessão do administrador
        /// </summary>
        private SessaoInfo _sessaoAdmin = null;

        /// <summary>
        /// Informações da sessão do usuário teste
        /// </summary>
        private SessaoInfo _sessaoUsuarioTeste = null;

        /// <summary>
        /// Usuário de teste
        /// </summary>
        private UsuarioInfo _usuarioTeste = null;

        /// <summary>
        /// Custódia do usuário de teste
        /// </summary>
        private CustodiaInfo _custodia = null;

        /// <summary>
        /// Conta corrente do usuário de teste
        /// </summary>
        private ContaCorrenteInfo _contaCorrente = null;

        /// <summary>
        /// Perfil de risco 
        /// </summary>
        private PerfilRiscoInfo _perfilRisco = null;

        /// <summary>
        /// Regra de risco 1
        /// </summary>
        private RegraRiscoInfo _regraRisco1 = null;

        /// <summary>
        /// Regra de risco 2
        /// </summary>
        private RegraRiscoInfo _regraRisco2 = null;

        /// <summary>
        /// Regra de risco 3
        /// </summary>
        private RegraRiscoInfo _regraRisco3 = null;

        #endregion

        /// <summary>
        /// Rotina de entrada dos testes
        /// </summary>
        public void Executar()
        {
            // Pede a migração dos layouts BVMF
            //ConverterBVMFLegadoResponse respostaConverter =
            //    (ConverterBVMFLegadoResponse)
            //        _servicoMensageria.ProcessarMensagem(
            //            new ConverterBVMFLegadoRequest()
            //            {
            //                CaminhoArquivo = _consCaminhoLayoutsLegado
            //            });
            //return;

            // Perfil de risco
            PerfilRiscoInfo perfilRisco = receberPerfilRisco();

            //// Lista de instrumentos bmf
            //ListarInstrumentosBMFResponse respostaInstrumentosBMF =
            //    (ListarInstrumentosBMFResponse)
            //        _servicoMensageria.ProcessarMensagem(
            //            new ListarInstrumentosBMFRequest()
            //            {
            //            });

            //// Lista de instrumentos bovespa
            //ListarInstrumentosBovespaResponse respostaInstrumentosBovespa =
            //    (ListarInstrumentosBovespaResponse)
            //        _servicoMensageria.ProcessarMensagem(
            //            new ListarInstrumentosBovespaRequest()
            //            {
            //            });

            // Garante a criação do usuário de teste
            receberUsuarioTeste();
            
            // Sincroniza a conta corrente e custodia para os objetos utilizados
            if (_consExecutarLocal)
            {
                sincronizarCustodiaLocal();
            }
            else
            {
                sincronizarContaCorrenteSinacor(_consCodigoCBLC);
                sincronizarCustodiaSinacor(_consCodigoCBLC);
            }

            // Garante a criação das regras de risco
            receberRegraRisco1();
            receberRegraRisco2();

            // Prepara uma mensagem de operação
            ExecutarOrdemRequest mensagemOrdem =
                new ExecutarOrdemRequest()
                {
                    CodigoSessao = receberSessaoUsuarioTeste().CodigoSessao,
                    Symbol = "USIM5",
                    OrderQty = 10,
                    Price = 5,
                    Side = OrdemDirecaoEnum.Compra
                };

            // Faz a validação
            ValidarMensagemResponse respostaValidar =
                (ValidarMensagemResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new ValidarMensagemRequest()
                        {
                            CodigoSessao = receberSessaoUsuarioTeste().CodigoSessao,
                            SessaoInfo = receberSessaoUsuarioTeste(),
                            Mensagem = mensagemOrdem
                        });
        }

        #region Setup do Cenário

        /// <summary>
        /// Retorna o perfil de risco
        /// </summary>
        private PerfilRiscoInfo receberPerfilRisco()
        {
            // Verifica se consta no cache
            if (_perfilRisco != null)
                return _perfilRisco;

            // Recebe Perfil
            PerfilRiscoInfo perfilRisco = 
                ((ReceberPerfilRiscoResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new ReceberPerfilRiscoRequest()
                        {
                            CodigoSessao = receberSessaoAdmin().CodigoSessao,
                            CodigoPerfilRisco = _consCodigoPerfilRisco
                        })).PerfilRiscoInfo;
            
            // Se não encontrou, cria
            if (perfilRisco == null)
                perfilRisco =
                    ((SalvarPerfilRiscoResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new SalvarPerfilRiscoRequest()
                        {
                            CodigoSessao = _sessaoAdmin.CodigoSessao,
                            PerfilRiscoInfo =
                                new PerfilRiscoInfo()
                                {
                                    CodigoPerfilRisco = _consCodigoPerfilRisco,
                                    NomePerfilRisco = "Perfil Risco Teste",
                                    DescricaoPerfilRisco = "Teste de perfil de risco"
                                }
                        })).PerfilRisco;

            // Coloca no cache
            _perfilRisco = perfilRisco;

            // Retorna
            return perfilRisco;
        }

        /// <summary>
        /// Garante a criação da regra de risco 1
        /// </summary>
        /// <returns></returns>
        private RegraRiscoInfo receberRegraRisco1()
        {
            // Verifica se consta no cache
            if (_regraRisco1 != null)
                return _regraRisco1;

            // Garante a criação da regra
           RegraRiscoInfo regraRisco =
                ((SalvarRegraRiscoResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new SalvarRegraRiscoRequest()
                        {
                            CodigoSessao = receberSessaoAdmin().CodigoSessao,
                            RegraRiscoInfo =
                                new RegraRiscoInfo()
                                {
                                    CodigoRegraRisco = _consCodigoRegra1,
                                    Agrupamento = 
                                        new RiscoGrupoInfo() 
                                        {
                                            CodigoUsuario = "123"
                                        },
                                    TipoRegra = typeof(RegraBloqueio)
                                }
                        })).RegraRisco;

            // Retorna
            return regraRisco;
        }

        /// <summary>
        /// Garante a criação da regra de risco 2
        /// </summary>
        /// <returns></returns>
        private RegraRiscoInfo receberRegraRisco2()
        {
            // Verifica se consta no cache
            if (_regraRisco1 != null)
                return _regraRisco1;

            // Garante a criação da regra
            RegraRiscoInfo regraRisco =
                ((SalvarRegraRiscoResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new SalvarRegraRiscoRequest()
                        {
                            CodigoSessao = receberSessaoAdmin().CodigoSessao,
                            RegraRiscoInfo =
                                new RegraRiscoInfo()
                                {
                                    CodigoRegraRisco = _consCodigoRegra2,
                                    Agrupamento =
                                        new RiscoGrupoInfo()
                                        {
                                            CodigoPerfilRisco = _consCodigoPerfilRisco
                                        },
                                    TipoRegra = typeof(RegraLimites),
                                    Config =
                                        new RegraLimitesConfig()
                                        {
                                            Limite = new LimiteInfo() 
                                            { 
                                                LimiteQuantidadeCustodiaInferior = 100,
                                                LimiteQuantidadeCustodiaSuperior = 100
                                            }
                                        }
                                }
                        })).RegraRisco;

            // Retorna
            return regraRisco;
        }

        /// <summary>
        /// Pede sincronização de conta corrente com o sinacor
        /// </summary>
        private void sincronizarContaCorrenteSinacor(string codigoCBLC)
        {
            // Pede sincronização da conta corrente do cliente informado
            SincronizarContaCorrenteResponse respostaSincronizar =
                (SincronizarContaCorrenteResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new SincronizarContaCorrenteRequest()
                        {
                            CodigoSessao = receberSessaoAdmin().CodigoSessao,
                            CodigoClienteCBLC = codigoCBLC,
                            CodigoContaCorrente = receberContaCorrente().CodigoContaCorrente
                        });

            // Invalida a conta corrente carregada
            _contaCorrente = null;
        }

        /// <summary>
        /// Pede sincronização de custódia com o sinacor
        /// </summary>
        private void sincronizarCustodiaSinacor(string codigoCBLC)
        {
            // Garante a existencia da custódia
            garantirExistenciaCustodia(_consCodigoCustodia);

            // Pede sincronização da custódia do cliente informado
            SincronizarCustodiaResponse respostaSincronizar =
                (SincronizarCustodiaResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new SincronizarCustodiaRequest()
                        {
                            CodigoSessao = receberSessaoAdmin().CodigoSessao,
                            CodigoClienteCBLC = codigoCBLC,
                            CodigoCustodia = _consCodigoCustodia
                        });

            // Invalida a custodia carregada
            _custodia = null;
        }

        /// <summary>
        /// Garante uma carga inicial para custodia
        /// </summary>
        private void sincronizarCustodiaLocal()
        {
            // Recebe a custodia
            CustodiaInfo custodia = garantirExistenciaCustodia(_consCodigoCustodia);

            // Insere posições de teste
            custodia.Posicoes.Clear();
            custodia.Posicoes.Add(
                new CustodiaPosicaoInfo()
                {
                    CodigoAtivo = "USIM5",
                    CodigoBolsa = "BOVESPA",
                    QuantidadeAbertura = 100,
                    QuantidadeAtual = 100
                });
            custodia.Posicoes.Add(
                new CustodiaPosicaoInfo()
                {
                    CodigoAtivo = "PETR4",
                    CodigoBolsa = "BOVESPA",
                    QuantidadeAbertura = 200,
                    QuantidadeAtual = 200
                });
            custodia.Posicoes.Add(
                new CustodiaPosicaoInfo()
                {
                    CodigoAtivo = "CSAN3",
                    CodigoBolsa = "BOVESPA",
                    QuantidadeAbertura = 100,
                    QuantidadeAtual = 100
                });

            // Salva custódia
            SalvarCustodiaResponse respostaSalvar =
                (SalvarCustodiaResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new SalvarCustodiaRequest()
                        {
                            CodigoSessao = _sessaoAdmin.CodigoSessao,
                            CustodiaInfo = custodia
                        });

            // Invalida cache
            _custodia = null;
        }

        /// <summary>
        /// Faz login com administrador e mantem a sessão
        /// </summary>
        private SessaoInfo receberSessaoAdmin()
        {
            // Se a sessão já estiver carregada, retorna ela
            if (_sessaoAdmin != null)
                return _sessaoAdmin;

            // Faz a autenticacao
            AutenticarUsuarioResponse respostaAutenticarUsuario =
                (AutenticarUsuarioResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new AutenticarUsuarioRequest()
                        {
                            CodigoUsuario = _consUsuarioAdmin,
                            Senha = _consUsuarioAdminSenha,
                            CodigoSistemaCliente = "teste"
                        });

            // Se nao autenticou, dispara erro
            if (respostaAutenticarUsuario.StatusResposta != MensagemResponseStatusEnum.OK)
                throw new Exception(respostaAutenticarUsuario.DescricaoResposta);

            // Salva no cache
            _sessaoAdmin = respostaAutenticarUsuario.Sessao;

            // Retorna a sessao
            return respostaAutenticarUsuario.Sessao;
        }

        /// <summary>
        /// Faz login com usuário de teste e mantem a sessão
        /// </summary>
        private SessaoInfo receberSessaoUsuarioTeste()
        {
            // Se a sessão já estiver carregada, retorna ela
            if (_sessaoUsuarioTeste != null)
                return _sessaoUsuarioTeste;

            // Faz a autenticacao
            AutenticarUsuarioResponse respostaAutenticarUsuario =
                (AutenticarUsuarioResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new AutenticarUsuarioRequest()
                        {
                            CodigoUsuario = _consUsuarioTeste,
                            Senha = _consUsuarioTesteSenha,
                            CodigoSistemaCliente = "teste"
                        });

            // Se nao autenticou, dispara erro
            if (respostaAutenticarUsuario.StatusResposta != MensagemResponseStatusEnum.OK)
                throw new Exception(respostaAutenticarUsuario.DescricaoResposta);

            // Salva no cache
            _sessaoUsuarioTeste = respostaAutenticarUsuario.Sessao;

            // Retorna a sessao
            return respostaAutenticarUsuario.Sessao;
        }

        /// <summary>
        /// Garante que o usuário necessário está criado com as caracteristicas
        /// necessárias
        /// </summary>
        private UsuarioInfo receberUsuarioTeste()
        {
            // Se o usuário já foi carregado, retorna o carregado
            if (_usuarioTeste != null)
                return _usuarioTeste;

            // Inicializa
            UsuarioInfo usuarioTeste = null;
            bool salvarUsuario = false;

            // Garante a criação do usuário
            ReceberUsuarioResponse respostaReceberUsuario =
                (ReceberUsuarioResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new ReceberUsuarioRequest()
                        {
                            CodigoSessao = receberSessaoAdmin().CodigoSessao,
                            CodigoUsuario = _consUsuarioTeste
                        });
            if (respostaReceberUsuario.Usuario == null)
            {
                // Cria o usuário
                usuarioTeste =
                    new UsuarioInfo()
                    {
                        CodigoUsuario = _consUsuarioTeste,
                        Senha = _consUsuarioTesteSenha,
                        Nome = _consUsuarioTeste,
                        Status = UsuarioStatusEnum.Habilitado
                    };

                // Sinaliza
                salvarUsuario = true;
            }
            else
            {
                // Pega o usuário
                usuarioTeste = respostaReceberUsuario.Usuario;
            }

            // Garante a criação do contexto do oms
            ContextoOMSInfo contextoOMS = usuarioTeste.Complementos.ReceberItem<ContextoOMSInfo>();
            if (contextoOMS == null)
            {
                // Cria o novo contexto e associa ao usuário
                contextoOMS =
                    new ContextoOMSInfo()
                    {
                        CodigoCBLC = _consCodigoCBLC,
                        CodigoPerfilRisco = _consCodigoPerfilRisco
                    };
                usuarioTeste.Complementos.AdicionarItem<ContextoOMSInfo>(contextoOMS);

                // Sinaliza
                salvarUsuario = true;
            }
            else
            {
                // Verifica se o código CBLC está correto
                if (contextoOMS.CodigoCBLC != _consCodigoCBLC || contextoOMS.CodigoPerfilRisco != _consCodigoPerfilRiscoUsuario)
                {
                    // Atribui
                    contextoOMS.CodigoCBLC = _consCodigoCBLC;
                    contextoOMS.CodigoPerfilRisco = _consCodigoPerfilRiscoUsuario;

                    // Sinaliza
                    salvarUsuario = true;
                }
            }

            // Garante que o usuário tem permissão de acesso ao sistema
            PermissaoAcessarSistema permissaoAcessarSistema = new PermissaoAcessarSistema();
            if (usuarioTeste.Permissoes.Find(p => p.CodigoPermissao == permissaoAcessarSistema.PermissaoInfo.CodigoPermissao) == null)
            {
                // Adiciona a permissao
                usuarioTeste.Permissoes.Add(
                    new PermissaoAssociadaInfo()
                    {
                        CodigoPermissao = permissaoAcessarSistema.PermissaoInfo.CodigoPermissao,
                        Status = PermissaoAssociadaStatusEnum.Permitido
                    });

                // Sinaliza
                salvarUsuario = true;
            }

            // Verifica se deve salvar o usuario
            if (salvarUsuario)
                usuarioTeste =
                    ((SalvarUsuarioResponse)
                        _servicoMensageria.ProcessarMensagem(
                            new SalvarUsuarioRequest()
                            {
                                CodigoSessao = receberSessaoAdmin().CodigoSessao,
                                Usuario = usuarioTeste
                            })).Usuario;

            // Salva no cache
            _usuarioTeste = usuarioTeste;

            // Retorna
            return usuarioTeste;
        }

        /// <summary>
        /// Faz a consulta e cache de custodia
        /// </summary>
        private CustodiaInfo receberCustodia()
        {
            // Verifica se já foi carregada a custódia
            if (_custodia != null)
                return _custodia;
            
            // Inicializa
            CustodiaInfo custodia = null;

            // Garante a criação da custodia
            ReceberCustodiaResponse respostaReceberCustodia =
                (ReceberCustodiaResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new ReceberCustodiaRequest()
                        {
                            CodigoSessao = receberSessaoAdmin().CodigoSessao,
                            CodigoCustodia = _consCodigoCustodia,
                            CarregarCotacoes = true
                        });
            if (respostaReceberCustodia.CustodiaInfo == null)
            {
                // Cria custódia e salva
                SalvarCustodiaResponse respostaSalvarCustodia =
                    (SalvarCustodiaResponse)
                        _servicoMensageria.ProcessarMensagem(
                            new SalvarCustodiaRequest()
                            {
                                CodigoSessao = receberSessaoAdmin().CodigoSessao,
                                CustodiaInfo =
                                    new CustodiaInfo()
                                    {
                                        CodigoCustodia = _consCodigoCustodia
                                    }
                            });

                // Pega custodia
                custodia = respostaSalvarCustodia.Custodia;

                // Informa a custódia no usuário e salva
                UsuarioInfo usuarioTeste = receberUsuarioTeste();
                usuarioTeste.Complementos.ReceberItem<ContextoOMSInfo>().CodigoCustodia = custodia.CodigoCustodia;
                _servicoMensageria.ProcessarMensagem(
                    new SalvarUsuarioRequest()
                    {
                        CodigoSessao = _sessaoAdmin.CodigoSessao,
                        Usuario = usuarioTeste
                    });
            }
            else
            {
                // Pega a custodia
                custodia = respostaReceberCustodia.CustodiaInfo;
            }

            // Salva no cache
            _custodia = custodia;

            // Retorna
            return custodia;
        }

        /// <summary>
        /// Garante a existencia da custodia informada
        /// </summary>
        /// <param name="codigoCustodia"></param>
        private CustodiaInfo garantirExistenciaCustodia(string codigoCustodia)
        {
            // Inicializa
            CustodiaInfo custodia = null;

            // Garante a criação da custodia
            ReceberCustodiaResponse respostaReceberCustodia =
                (ReceberCustodiaResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new ReceberCustodiaRequest()
                        {
                            CodigoSessao = receberSessaoAdmin().CodigoSessao,
                            CodigoCustodia = _consCodigoCustodia,
                            CarregarCotacoes = false
                        });
            if (respostaReceberCustodia.CustodiaInfo == null)
            {
                // Cria custódia e salva
                SalvarCustodiaResponse respostaSalvarCustodia =
                    (SalvarCustodiaResponse)
                        _servicoMensageria.ProcessarMensagem(
                            new SalvarCustodiaRequest()
                            {
                                CodigoSessao = receberSessaoAdmin().CodigoSessao,
                                CustodiaInfo =
                                    new CustodiaInfo()
                                    {
                                        CodigoCustodia = _consCodigoCustodia
                                    }
                            });

                // Pega custodia
                custodia = respostaSalvarCustodia.Custodia;

                // Informa a custódia no usuário e salva
                UsuarioInfo usuarioTeste = receberUsuarioTeste();
                usuarioTeste.Complementos.ReceberItem<ContextoOMSInfo>().CodigoCustodia = custodia.CodigoCustodia;
                _servicoMensageria.ProcessarMensagem(
                    new SalvarUsuarioRequest()
                    {
                        CodigoSessao = _sessaoAdmin.CodigoSessao,
                        Usuario = usuarioTeste
                    });
            }
            else
            {
                // Pega a custodia
                custodia = respostaReceberCustodia.CustodiaInfo;
            }

            // Retorna
            return custodia;
        }

        /// <summary>
        /// Faz a consulta e cache de conta corrente
        /// </summary>
        /// <returns></returns>
        private ContaCorrenteInfo receberContaCorrente()
        {
            // Se a conta já foi carregada, retorna a carregada
            if (_contaCorrente != null)
                return _contaCorrente;
            
            // Inicializa
            ContaCorrenteInfo contaCorrente = null;

            // Garante a criação da conta corrente
            ReceberContaCorrenteResponse respostaReceberContaCorrente =
                (ReceberContaCorrenteResponse)
                    _servicoMensageria.ProcessarMensagem(
                        new ReceberContaCorrenteRequest()
                        {
                            CodigoSessao = receberSessaoAdmin().CodigoSessao
                        });
            if (respostaReceberContaCorrente.ContaCorrenteInfo == null)
            {
                // Cria conta corrente e salva
                SalvarContaCorrenteResponse respostaSalvarContaCorrente =
                    (SalvarContaCorrenteResponse)
                        _servicoMensageria.ProcessarMensagem(
                            new SalvarContaCorrenteRequest()
                            {
                                CodigoSessao = receberSessaoAdmin().CodigoSessao,
                                ContaCorrenteInfo =
                                    new ContaCorrenteInfo()
                                    {
                                        CodigoContaCorrente = _consCodigoContaCorrente
                                    }
                            });

                // Pega a conta corrente
                contaCorrente = respostaSalvarContaCorrente.ContaCorrente;

                // Informa a conta corrente no usuário e salva
                UsuarioInfo usuarioTeste = receberUsuarioTeste();
                usuarioTeste.Complementos.ReceberItem<ContextoOMSInfo>().CodigoContaCorrente = contaCorrente.CodigoContaCorrente;
                _servicoMensageria.ProcessarMensagem(
                    new SalvarUsuarioRequest()
                    {
                        CodigoSessao = _sessaoAdmin.CodigoSessao,
                        Usuario = usuarioTeste
                    });
            }

            // Salva no cache
            _contaCorrente = contaCorrente;

            // Retorna
            return contaCorrente;
        }

        #endregion
    }
}
