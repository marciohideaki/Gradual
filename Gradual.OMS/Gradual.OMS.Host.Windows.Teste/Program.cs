using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Gradual.OMS.Contratos.CanaisNegociacao;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Comum.Permissoes;
using Gradual.OMS.Contratos.Integracao.Sinacor;
using Gradual.OMS.Contratos.Integracao.Sinacor.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.Mensagens;
using Gradual.OMS.Contratos.Interface;
using Gradual.OMS.Contratos.Interface.Dados;
using Gradual.OMS.Contratos.Interface.Desktop;
using Gradual.OMS.Contratos.Interface.Desktop.Controles.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Contratos.Risco;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Sistemas.CanaisNegociacao;
using Gradual.OMS.Sistemas.Comum;
using Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum;
using Gradual.OMS.Sistemas.Interface.Desktop.SkinDevExpress;
using Gradual.OMS.Sistemas.Ordens;
using Gradual.OMS.Sistemas.Risco;
using Gradual.OMS.Sistemas.Seguranca;
using Gradual.OMS.Sistemas.Seguranca.Persistencias;

using Gradual.OMS.Contratos.Automacao.Ordens;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;

using Gradual.OMS.Contratos.Custodia.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente.Mensagens;
using Gradual.OMS.Contratos.ContaCorrente.Dados;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Mensagens;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;

using Orbite.RV.Contratos.Integracao.BVMF;
using Orbite.RV.Contratos.Integracao.BVMF.Dados;
using Orbite.RV.Contratos.MarketData.BMF;
using Orbite.RV.Contratos.MarketData.BMF.Dados;
using Orbite.RV.Contratos.MarketData.BMF.Mensagens;
using Orbite.RV.Contratos.MarketData.Bovespa;
using Orbite.RV.Contratos.MarketData.Bovespa.Dados;
using Orbite.RV.Contratos.MarketData.Bovespa.Mensagens;

namespace Gradual.OMS.Host.Windows.Teste
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Carrega servicos do config
            ServicoHostColecao.Default.CarregarConfig("Default");
            ServicoHostColecao.Default.IniciarServicos();

            // Inicia a aplicação ou chama rotina de testes
            iniciarAplicacao();
            //testes();
            //testeContaCorrente();
            //testeOrdens2();
            //testeMarketData();
            //testeMarketDataBMF();
            //testeUsuario();
            //testeSeguranca();
            //testeInterface();
            //testeBruno();
            //new TesteRisco().Executar();

            // Finaliza servicos
            ServicoHostColecao.Default.PararServicos();
        }

        private static void testeBruno()
        {
            // Referencia para a mensageria
            IServicoMensageria servicoMensageria = Ativador.Get<IServicoMensageria>();

            // Faz a autenticacao
            AutenticarUsuarioResponse responseAutenticacao =
                (AutenticarUsuarioResponse)
                    servicoMensageria.ProcessarMensagem(
                        new AutenticarUsuarioRequest()
                        {
                            Email = "bribeiro@gradualinvestimentos.com.br",
                            Senha = "123"
                        });

            // Recebe o usuario
            ReceberUsuarioResponse respostaReceberUsuario =
                (ReceberUsuarioResponse)
                    servicoMensageria.ProcessarMensagem(
                        new ReceberUsuarioRequest()
                        {
                            CodigoSessao = null,
                            Email = "bribeiro@gradualinvestimentos.com.br"
                        });

            // Verifica se tem o contexto, senao cria
            ContextoOMSInfo contextoOMS =
                respostaReceberUsuario.Usuario.Complementos.ReceberItem<ContextoOMSInfo>();
            if (contextoOMS == null)
                contextoOMS = respostaReceberUsuario.Usuario.Complementos.AdicionarItem<ContextoOMSInfo>(new ContextoOMSInfo());

            // Verifica se tem conta corrente, senao cria
            if (contextoOMS.CodigoContaCorrente != null)
            {

                // Cria conta corrente vazia
                ContaCorrenteInfo contaCorrente =
                    new ContaCorrenteInfo()
                    {
                    };

                // Salva conta corrente criada
                SalvarContaCorrenteResponse respostaSalvarContaCorrente =
                    (SalvarContaCorrenteResponse)
                        servicoMensageria.ProcessarMensagem(
                            new SalvarContaCorrenteRequest()
                            {
                                CodigoSessao = responseAutenticacao.Sessao.CodigoSessao,
                                ContaCorrenteInfo = contaCorrente
                            });

                // Associa conta corrente ao usuário
                contextoOMS.CodigoContaCorrente = contaCorrente.CodigoContaCorrente;

                // Salva usuário
                SalvarUsuarioResponse respostaSalvarUsuario =
                    (SalvarUsuarioResponse)
                        servicoMensageria.ProcessarMensagem(
                            new SalvarUsuarioRequest()
                            {
                                CodigoSessao = responseAutenticacao.Sessao.CodigoSessao,
                                Usuario = respostaReceberUsuario.Usuario
                            });
            }    

        }

        private static void testeInterface()
        {
            IServicoMensageria servicoMensageria = Ativador.Get<IServicoMensageria>();

            AutenticarUsuarioResponse responseAutenticacao =
                (AutenticarUsuarioResponse)
                    servicoMensageria.ProcessarMensagem(
                        new AutenticarUsuarioRequest()
                        {
                            CodigoUsuario = "Admin",
                            Senha = "123"
                        });

            ReceberArvoreComandosInterfaceResponse respostaArvore =
                (ReceberArvoreComandosInterfaceResponse)
                    servicoMensageria.ProcessarMensagem(
                        new ReceberArvoreComandosInterfaceRequest()
                        {
                            CodigoSessao = responseAutenticacao.Sessao.CodigoSessao,
                            CodigoGrupoComandoInterface = "default"
                        });

            ReceberGrupoComandoInterfaceResponse respostaReceber =
                (ReceberGrupoComandoInterfaceResponse)
                    servicoMensageria.ProcessarMensagem(
                        new ReceberGrupoComandoInterfaceRequest()
                        {
                            CodigoSessao = responseAutenticacao.Sessao.CodigoSessao,
                            CodigoGrupoComandoInterface = "default"
                        });

        }

        private static void testeSeguranca()
        {
            IServicoMensageria servicoMensageria = Ativador.Get<IServicoMensageria>();
            ListarPermissoesResponse resposta = 
                (ListarPermissoesResponse)
                    servicoMensageria.ProcessarMensagem(
                        new ListarPermissoesRequest());
        }

        private static void testeUsuario()
        {
            IServicoMensageria servicoMensageria = Ativador.Get<IServicoMensageria>();

            AutenticarUsuarioResponse responseAutenticacao =
                (AutenticarUsuarioResponse)
                    servicoMensageria.ProcessarMensagem(
                        new AutenticarUsuarioRequest()
                        {
                            CodigoUsuario = "Admin",
                            Senha = "123"
                        });

            ListarUsuariosResponse respostaListarUsuario =
                (ListarUsuariosResponse)
                    servicoMensageria.ProcessarMensagem(
                        new ListarUsuariosRequest()
                        {
                        });

            //ReceberUsuarioResponse respostaUsuario =
            //    (ReceberUsuarioResponse)
            //        servicoMensageria.ProcessarMensagem(
            //            new ReceberUsuarioRequest()
            //            {
            //                CodigoSessao = responseAutenticacao.Sessao.CodigoSessao,
            //                CodigoUsuario = "Usuario1"
            //            });

            //ReceberCustodiaResponse respostaCustodia =
            //    (ReceberCustodiaResponse)
            //        servicoMensageria.ProcessarMensagem(
            //            new ReceberCustodiaRequest()
            //            {
            //                CodigoSessao = responseAutenticacao.Sessao.CodigoSessao,
            //                CodigoCustodia = respostaUsuario.Usuario.Complementos.ReceberItem<ContextoOMSInfo>().CodigoCustodia
            //            });

            //InicializarUsuarioResponse respostaInicializar =
            //    (InicializarUsuarioResponse)
            //        servicoMensageria.ProcessarMensagem(
            //            new InicializarUsuarioRequest()
            //            {
            //                CodigoSessao = responseAutenticacao.Sessao.CodigoSessao,
            //                CodigoCBLC = "38157",
            //                SincronizarContaCorrente = true,
            //                SincronizarCustodia = true,
            //                SincronizarContaInvestimento = true,
            //                SincronizarContaMargem = true,
            //                InferirCBLCInvestimento = true,
            //                Usuario = 
            //                    new UsuarioInfo() 
            //                    { 
            //                        CodigoUsuario = "Usuario1",
            //                        Email = "Usuario1",
            //                        Nome = "Usuario1",
            //                        Senha = "123"
            //                    }
            //            });
        }

        private static void testeMarketDataBMF()
        {
            IServicoMarketDataBMF servicoMarketBMF = Ativador.Get<IServicoMarketDataBMF>();
            ReceberHistoricoCotacaoBMFResponse respostaHistoricoCotacao1 =
                servicoMarketBMF.ReceberHistoricoCotacaoBMF(
                    new ReceberHistoricoCotacaoBMFRequest()
                    {
                        Instrumento =
                            new InstrumentoBMFInfo()
                            {
                                CodigoNegociacao = "INDQ10"
                            },
                        DataInicial = new DateTime(2004, 01, 01),
                        DataFinal = new DateTime(2010, 09, 01)
                    });
        }

        private static void testeMarketData()
        {
            //IServicoIntegracaoBVMF servicoIntegracaoBVMF = Ativador.Get<IServicoIntegracaoBVMF>();
            //servicoIntegracaoBVMF.ReceberArquivoMarketDataBovespa(Orbite.Comum.PeriodoEnum.Ano, new DateTime(2010, 01, 01), @"K:\Projetos\Gradual\OMS\Bin\BVMF\cota2010.txt");

            //IServicoIntegracaoBVMFArquivos servicoIntegracaoBVMFArquivos = Ativador.Get<IServicoIntegracaoBVMFArquivos>();
            //List<ArquivoBVMFInfo> diretorio = servicoIntegracaoBVMFArquivos.ListarDiretorio();

            IServicoMarketDataBovespa servicoMarketDataBovespa = Ativador.Get<IServicoMarketDataBovespa>();
            ReceberHistoricoCotacaoBovespaResponse respostaHistoricoCotacao1 =
                servicoMarketDataBovespa.ReceberHistoricoCotacaoBovespa(
                    new ReceberHistoricoCotacaoBovespaRequest()
                    {
                        Instrumento =
                            new InstrumentoBovespaInfo()
                            {
                                CodigoNegociacao = "PETR4"
                            },
                        DataInicial = new DateTime(2004, 01, 01),
                        DataFinal = new DateTime(2010, 03, 01)
                    });

            ReceberUltimaCotacaoBovespaResponse respostaUltimaCotacao =
                servicoMarketDataBovespa.ReceberUltimaCotacaoBovespa(
                    new ReceberUltimaCotacaoBovespaRequest()
                    {
                        Ativos = new List<string>() 
                        { 
                            "USIM5", "PETR4", "VALE5", "VALEA38"
                        }
                    });

            ReceberHistoricoCotacaoBovespaResponse respostaHistoricoCotacao2 =
                servicoMarketDataBovespa.ReceberHistoricoCotacaoBovespa(
                    new ReceberHistoricoCotacaoBovespaRequest()
                    {
                        Instrumento =
                            new InstrumentoBovespaInfo()
                            {
                                CodigoNegociacao = "USIM5"
                            },
                        DataInicial = new DateTime(2008, 01, 01),
                        DataFinal = new DateTime(2010, 03, 01)
                    });
        }

        private static void testeOrdens2()
        {
            IServicoMensageria servicoMensageria = Ativador.Get<IServicoMensageria>();

            AutenticarUsuarioResponse responseAutenticacao =
                (AutenticarUsuarioResponse)
                    servicoMensageria.ProcessarMensagem(
                        new AutenticarUsuarioRequest()
                        {
                            CodigoUsuario = "Admin",
                            Senha = "123"
                        });

            ListarOrdensResponse respostaListarOrdens =
                (ListarOrdensResponse)
                    servicoMensageria.ProcessarMensagem(
                        new ListarOrdensRequest()
                        {
                            CodigoSessao = responseAutenticacao.Sessao.CodigoSessao
                        });
        }

        private static void testesOrdens()
        {
            int qtde = 2000;
            IServicoOrdens servicoOrdens = Ativador.Get<IServicoOrdens>();
            DateTime dtIni = DateTime.Now;
            for (int i = 0; i < qtde; i++)
                servicoOrdens.ExecutarOrdem(
                    new ExecutarOrdemRequest()
                    {
                        Account = "1230",
                        CodigoBolsa = "BOVESPA",
                        OrderQty = 100,
                        Symbol = "USIM5",
                        Side = OrdemDirecaoEnum.Compra,
                        TimeInForce = OrdemValidadeEnum.ExecutaIntegralOuCancela,
                        OrdType = OrdemTipoEnum.Limitada,
                        Price = 35
                    });
            DateTime dtFim = DateTime.Now;
            TimeSpan diff = dtFim - dtIni;
            MessageBox.Show(diff.TotalMilliseconds.ToString() + "; " + (diff.TotalMilliseconds / qtde).ToString());
        }

        private static void testeContaCorrente()
        {
            IServicoMensageria servicoMensageria = Ativador.Get<IServicoMensageria>();
            ConsultarContasCorrentesResponse respostaConsultar =
                (ConsultarContasCorrentesResponse)
                    servicoMensageria.ProcessarMensagem(
                        new ConsultarContasCorrentesRequest()
                        {
                        });

            List<double> saldoProjetado = new List<double>(new double[] { 1, 2, 3, 4 });

            ContaCorrenteInfo contaCorrente = 
                new ContaCorrenteInfo()
                {
                    SaldoRegularAtual = 199,
                    SaldoRegularProjetado = saldoProjetado
                };

            SalvarContaCorrenteResponse respostaSalvar =
                (SalvarContaCorrenteResponse)
                    servicoMensageria.ProcessarMensagem(
                        new SalvarContaCorrenteRequest()
                        {
                            ContaCorrenteInfo = contaCorrente
                        });
        }

        private static void testes()
        {
            IServicoMensageria servicoMensageria = Ativador.Get<IServicoMensageria>();

            AutenticarUsuarioResponse responseAutenticacao =
                (AutenticarUsuarioResponse)
                    servicoMensageria.ProcessarMensagem(
                        new AutenticarUsuarioRequest()
                        {
                            CodigoUsuario = "Admin",
                            Senha = "123"
                        });

            CallbackEvento callbackEvento = new CallbackEvento();
            callbackEvento.Evento += new EventHandler<EventoEventArgs>(callbackEvento_Evento);
            callbackEvento.Evento2 += new EventHandler<EventArgs>(callbackEvento_Evento2);
            
            IServicoMensageriaComCallback servicoMensageriaComCallback = 
                Ativador.Get<IServicoMensageriaComCallback>(
                    callbackEvento, responseAutenticacao.Sessao);
            servicoMensageriaComCallback.AssinarEvento(
                new AssinarEventoRequest() 
                { 
                    CodigoSessao = responseAutenticacao.Sessao.CodigoSessao,
                    TipoServico = typeof(IServicoEcho).FullName + ", " + typeof(IServicoEcho).Assembly.FullName,
                    NomeEvento = "EventoEcho"
                });

            ExecutarEchoResponse responseEcho =
                (ExecutarEchoResponse)
                    servicoMensageria.ProcessarMensagem(
                        new ExecutarEchoRequest()
                        {
                            TipoFuncao = ExecutarEchoTipoFuncaoEnum.EcoarMensagem,
                            Mensagem = "teste"
                        });
        }

        private static void callbackEvento_Evento2(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void callbackEvento_Evento(object sender, EventoEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void testesPersistenciaSeguranca()
        {
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();

            UsuarioGrupoInfo usuarioGrupo =
                new UsuarioGrupoInfo()
                {
                    CodigoUsuarioGrupo = "grupo1",
                    NomeUsuarioGrupo = "grupo1",
                    Permissoes = 
                        new List<PermissaoAssociadaInfo>()
                        {
                            new PermissaoAssociadaInfo()
                            {
                                CodigoPermissao = "9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F",
                                Status = PermissaoAssociadaStatusEnum.Permitido
                            },
                            new PermissaoAssociadaInfo()
                            {
                                CodigoPermissao = "D31AD6D1-FCA6-4529-ACBE-B5B9D60E5755",
                                Status = PermissaoAssociadaStatusEnum.Negado
                            }
                        }
                };

            SalvarObjetoResponse<UsuarioGrupoInfo> resposta1 =
                servicoPersistencia.SalvarObjeto<UsuarioGrupoInfo>(
                    new SalvarObjetoRequest<UsuarioGrupoInfo>()
                    {
                        Objeto = usuarioGrupo
                    });

            ReceberObjetoResponse<UsuarioGrupoInfo> resposta0 =
                servicoPersistencia.ReceberObjeto<UsuarioGrupoInfo>(
                    new ReceberObjetoRequest<UsuarioGrupoInfo>()
                    {
                        CodigoObjeto = "grupo1"
                    });
        }

        private static void testeComandoInterface()
        {
            GrupoComandoInterfaceInfo grupoComando = new GrupoComandoInterfaceInfo();
            grupoComando.ComandosInterfaceRaiz.Add(
                new ComandoInterfaceInfo()
                {
                    CodigoComandoInterface = "c1",
                    CodigoSistema = "s1",
                    Nome = "c1",
                    Filhos = new List<ComandoInterfaceInfo>()
                    {
                        new ComandoInterfaceInfo()
                        {
                            CodigoComandoInterface = "c1.1",
                            CodigoSistema = "s1",
                            Nome = "c1.1",
                            Execucoes = new List<ComandoInterfaceExecucaoInfo>()
                            {
                                new ComandoInterfaceExecucaoInfo()
                                {
                                    Plataforma = InterfacePlataformaEnum.Web
                                }
                            }
                        },
                        new ComandoInterfaceInfo()
                        {
                            CodigoComandoInterface = "c1.2",
                            CodigoSistema = "s1",
                            Nome = "c1.2"
                        }
                    }
                });

            IServicoInterface servicoInterface = Ativador.Get<IServicoInterface>();
            SalvarGrupoComandoInterfaceResponse respostaSalvar =
                servicoInterface.SalvarGrupoComandoInterface(
                    new SalvarGrupoComandoInterfaceRequest()
                    {
                        GrupoComandoInterface = grupoComando
                    });
        }

        private static void testeMetadado()
        {
            IServicoMetadadoComum servicoMetadado = Ativador.Get<IServicoMetadadoComum>();
            GerarDbMetadadoResponse resposta =
                servicoMetadado.GerarMetadadoComum(
                    new GerarDbMetadadoRequest()
                    {
                        Enumeradores =
                        new Type[] 
                        {
                            typeof(EntidadeTipoSerializacaoEnum),
                            typeof(CriticaStatusEnum)
                        }
                    });
        }

        private static void testesPersistencia()
        {
            IServicoPersistencia servicoPersistencia = Ativador.Get<IServicoPersistencia>();
            PerfilDbLib db = new PerfilDbLib();

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ConnectionString);
            cn.Open();

            int qtde = 50000;

            new SqlCommand("truncate table TbPerfil", cn).ExecuteNonQuery();

            DateTime dt1b = DateTime.Now;
            for (int i = 0; i < qtde; i++)
            {
                db.Salvar(
                    new PerfilInfo()
                    {
                        CodigoPerfil = i.ToString(),
                        NomePerfil = "p" + i.ToString()
                    });
            }
            DateTime dt2b = DateTime.Now;

            new SqlCommand("truncate table TbPerfil", cn).ExecuteNonQuery();

            DateTime dt1a = DateTime.Now;
            for (int i = 0; i < qtde; i++)
            {
                servicoPersistencia.SalvarObjeto<PerfilInfo>(
                    new SalvarObjetoRequest<PerfilInfo>()
                    {
                        Objeto =
                            new PerfilInfo()
                            {
                                CodigoPerfil = i.ToString(),
                                NomePerfil = "p" + i.ToString()
                            }
                    });
            }
            DateTime dt2a = DateTime.Now;

            TimeSpan t1 = dt2a - dt1a;
            TimeSpan t2 = dt2b - dt1b;
            double diff = Math.Abs((t1.TotalMilliseconds - t2.TotalMilliseconds) / qtde);
            string msg =
                string.Format(
                    "Teste Persistencia -> Tempo pela persistencia: {0}; Tempo direto: {1}; Diff: {2}",
                    t1.TotalMilliseconds.ToString(),
                    t2.TotalMilliseconds.ToString(),
                    diff.ToString());
            Log.EfetuarLog(msg, LogTipoEnum.Aviso, ModulosOMS.ModuloInterfaceDesktop);
            MessageBox.Show(msg);
        }

        private static void iniciarAplicacao()
        {
            // Carrega o formulario
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //// Faz o login
            //FormLogin frmLogin = new FormLogin();
            //if (frmLogin.ShowDialog() == DialogResult.OK)
            //{
                // Pega o serviço de interface
                IServicoInterfaceDesktop servicoInterface = Ativador.Get<IServicoInterfaceDesktop>();

                // Inicia o launcher
                servicoInterface.IniciarLauncher();

                // Cria o contexto
                //InterfaceContextoOMS contexto = new InterfaceContextoOMS(frmLogin.SessaoInfo);
                //servicoInterface.Contexto.AdicionarItem(contexto);

                // Lança o sistema
                Application.Run((Form)servicoInterface.ReceberJanelaLauncher());
            //}
        }
    }
}
