using System;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Contratos.Dados.ControleDeOrdens;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.OMS.Email.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Seguranca.Lib;
using Gradual.Intranet.Contratos.Dados.Portal;

namespace Gradual.Intranet.Www
{
    public partial class TesteDevCodeBehind : PaginaBaseAutenticada
    {
        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            this.TestarSalvarHistoricoDeSenha();


            //if (IsPostBack)
            //{
            //    HttpPostedFile x = Request.Files[0];
            //}

            //this.ConsultarClienteRenovacaoCadastral();

            //this.Teste1();

            //this.ServicoEnviarEmail();

            //var xc = this.ServicoPersistenciaCadastro;

            //ExcluirTodosClientes();
            //ListarTodosDoSinacor();
            //ExportarCLiente();

            /*
            Response.Clear();

            //TODO: Cookie de "lembrar login", se houver redireciona pra SAC/Default.aspx

            this.ConsultarOrdemStartStop();

            // BuscarClientesResponse lResponse;
            BuscarClientesRequest lRequest = new BuscarClientesRequest();

             ExcluirTodosClientes()
             * 
             * 
            lRequest.TermoDeBusca = "%Maria%";
            lRequest.OpcaoBuscarPor = OpcoesBuscarPor.NomeCliente;

            //lResponse = this.ServicoCliente.BuscarClientes(lRequest);

            Server.Transfer("Login.aspx");*/
        }

        private void ConsultarClienteRenovacaoCadastral()
        {
            var lRetorno = ClienteDbLib.ReceberClienteRenovacaoCadastral(new Servicos.BancoDeDados.Propriedades.Request.ReceberEntidadeRequest<ClienteRenovacaoCadastralInfo>()
            {
                Objeto = new ClienteRenovacaoCadastralInfo() { DsCpfCnpj = "33414115859" }
            });


            if (lRetorno.Objeto.DtNascimentoFundacao > DateTime.Now)
            {

            }

        }

        private void ServicoReceberLoginAssinaturaEletronica()
        {
            var lLoginAssinaturaEletronicaInfo = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<LoginAssinaturaEletronicaInfo>(
                new ReceberEntidadeCadastroRequest<LoginAssinaturaEletronicaInfo>()
                {
                    EntidadeCadastro = new LoginAssinaturaEletronicaInfo()
                    {
                        //  CdCodigo = 45019,
                        DsEmail = "9946365@bol.com.br",
                        CdAssinaturaEletronica = "123123",

                    },
                });
        }

        #endregion

        #region | Métodos Teste Dev

        private void TestarSalvarHistoricoDeSenha()
        {
            var lRetorno = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<HistoricoSenhaInfo>(
                new SalvarEntidadeCadastroRequest<HistoricoSenhaInfo>()
                {
                    EntidadeCadastro = new HistoricoSenhaInfo()
                    {
                        CdSenha = "12121A84E8848E891FB7211BC8C0FAE7",
                        IdLogin = 236900,
                    }
                });

            if (lRetorno.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {

            }
        }

        private void TestarHistoricoDeSenha()
        {
            var lRetorno = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<HistoricoSenhaInfo>(
                new ConsultarEntidadeCadastroRequest<HistoricoSenhaInfo>()
                {
                    EntidadeCadastro = new HistoricoSenhaInfo()
                    {
                        IdLogin = 236900,
                        CdSenha = "4BADAEE57FED5610012A296273158F5F"
                    }
                });

            if (lRetorno.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {

            }
        }

        private void TesteLogin()
        {
            var lRetorno = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<EfetuarLoginInfo>(
                new ReceberEntidadeCadastroRequest<EfetuarLoginInfo>()
                {
                    EntidadeCadastro = new EfetuarLoginInfo()
                    {
                        DsEmail = "arosario@gradualinvestimentos.com.br",
                        CdSenha = "102030",
                    }
                });

            if (lRetorno.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {

            }
        }

        private void TesteIncrementoTentativaErrada()
        {
            var lRetorno = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LoginIncrementarTentativasErradasInfo>(
                new SalvarEntidadeCadastroRequest<LoginIncrementarTentativasErradasInfo>()
                {
                    EntidadeCadastro = new LoginIncrementarTentativasErradasInfo()
                    {
                        DsEmail = "arosario@gradualinvestimentos.com.br",
                        CdSenha = "102030",
                    }
                });

            if (lRetorno.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {

            }
        }

        private void TesteLiberacaoAcessoTentativasErradas()
        {
            var lRetorno = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LoginLiberarAcesoTentativasErradasInfo>(
                new SalvarEntidadeCadastroRequest<LoginLiberarAcesoTentativasErradasInfo>()
                {
                    EntidadeCadastro = new LoginLiberarAcesoTentativasErradasInfo()
                    {
                        DsEmail = "arosario@gradualinvestimentos.com.br",
                        CdSenha = "102030",
                    }
                });

            if (lRetorno.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {

            }
        }

        private void Teste1()
        {
            this.Session["Usuario"] = new Usuario() { Id = 66340, Nome = "Administrador" };

            base.RegistrarLogExclusao();
        }

        private void ServicoEnviarEmail()
        {
            var lDestinatarios = new System.Collections.Generic.List<string>();

            lDestinatarios.Add("arosario@gradualinvestimentos.com.br");


            var lServico = Ativador.Get<IServicoEmail>();

            var lStatusServico = lServico.ReceberStatusServico().ToString();

            var lEmail = new EnviarEmailRequest()
            {
                Objeto = new EmailInfo()
                    {
                        Assunto = "Teste de e-mail",
                        CorpoMensagem = string.Format("<html><body><p>TESTE DO SERVICO DE EMAIL</p><p>Realizado em: {0}</p></body></html>", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")),
                        Destinatarios = lDestinatarios,
                        Remetente = "arosario@gradualinvestimentos.com.br",
                    }
            };

            var lRetorno = lServico.Enviar(lEmail);

            var lStatus = lRetorno.StatusResposta;
        }

        private void GravarLogEmail()
        {
            var lLogEmailInfo = this.ServicoPersistencia.SalvarObjeto<LogEmailInfo>(
                new SalvarObjetoRequest<LogEmailInfo>()
                {
                    Objeto = new LogEmailInfo()
                    {
                        DtEnvio = DateTime.Now,
                        ETipoEmailDisparo = eTipoEmailDisparo.Assessor,
                        CorpoMensagem = "<html><body><p>Lucro funcionando</p> - <p>Teste de serviço.</p></body></html>",
                        Destinatarios = new System.Collections.Generic.List<string>() { "cobertor@gradualinvestimentos.com.br", },
                    },
                });
        }

        private void PendenciaClienteAssessorEnviarEmail()
        {
            var lResponse =
                this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<PendenciaClienteAssessorInfo>(
                    new ReceberEntidadeCadastroRequest<PendenciaClienteAssessorInfo>());
        }

        //private void ListarOrdens()
        //{
        //    var lListarOrdensInfo = new ListarOrdensInfo()
        //    {
        //        ListarOrdensRequest =
        //            new ListarOrdensRequest()
        //            {
        //                FiltroDataMaiorIgual = new DateTime(2010, 05, 10),
        //                FiltroDataMenor = DateTime.Now,
        //            },
        //        AutenticarUsuarioRequest =
        //            new AutenticarUsuarioRequest()
        //            {
        //                CodigoUsuario = "Admin",
        //                Senha = "123",
        //            }
        //    };

        //    ReceberEntidadeCadastroResponse<ListarOrdensInfo> lReceberResponse =
        //        (ReceberEntidadeCadastroResponse<ListarOrdensInfo>)
        //        this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ListarOrdensInfo>(
        //            new ReceberEntidadeCadastroRequest<ListarOrdensInfo>()
        //            {
        //                EntidadeCadastro = lListarOrdensInfo
        //            });

        //    if (MensagemResponseStatusEnum.OK.Equals(lReceberResponse.StatusResposta))
        //    {
        //        //Sua mensagem de sucesso
        //    }
        //    else
        //    {
        //        //Sua mensagem de erro.
        //    }
        //}

        private void ConsultarOrdemStartStop()
        {
            var lListarOrdemStartStopInfo = new ListarOrdemStartStopInfo()
            {
                ParametrosDeBuscaOrdemStarStop = new ParametrosDeBuscaOrdemStarStop()
                {
                    //CodigoItemAutomacaoOrdem = 0,
                    //DataReferenciaOrdemStop = new DateTime(2010, 05, 18),
                    StatusOrdem = null,
                    //Instrumento = "PETR4",
                    //Login = "1230",
                    //PesquisaClienteParametro = "1230",
                }
            };

            ReceberEntidadeCadastroResponse<ListarOrdemStartStopInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<ListarOrdemStartStopInfo>)
                   this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ListarOrdemStartStopInfo>(
                       new ReceberEntidadeCadastroRequest<ListarOrdemStartStopInfo>()
                       {
                           EntidadeCadastro = lListarOrdemStartStopInfo
                       });

            //IServicoAutomacaoOrdens lOrdens = Ativador.Get<IServicoAutomacaoOrdens>();
            //ListarItensAutomacaoOrdemRequest lListarItensAutomacaoOrdemRequest = new ListarItensAutomacaoOrdemRequest();
            //lListarItensAutomacaoOrdemRequest.CodigoItemAutomacaoOrdem = default(int);
            ////req.Instrument = txtInstrumento.Text;

            //ListarItensAutomacaoOrdemResponse res = lOrdens.ListarItemsAutomacaoOrdem(lListarItensAutomacaoOrdemRequest);

            //if (MensagemResponseStatusEnum.OK.Equals(lReceberResponse.StatusResposta))
            //{
            //    //Sua mensagem de sucesso
            //}
            //else
            //{
            //    //Sua mensagem de erro.
            //}

            //foreach (AutomacaoOrdensInfo item in res.ListaDeAutomacaoOrdens)
            //{
            //    string x = (item.IdStopstartStatus == (int)ItemAutomacaoStatusEnum.EnviadoParaServidorDeOrdens) ? "S" : "N";
            //}
        }

        private void CancelarOrdem()
        {
            var lCancelarOrdemInfo = new CancelarOrdemInfo()
            {
                //CancelarOrdemRequest = new CancelarOrdemRequest() { ClOrdID = "1023", },
                AutenticarUsuarioRequest = new AutenticarUsuarioRequest() { CodigoUsuario = "Admin", Senha = "123", },
            };

            RemoverEntidadeCadastroResponse lRemoverResponse =
                (RemoverEntidadeCadastroResponse)
                this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<CancelarOrdemInfo>(
                     new RemoverEntidadeCadastroRequest<CancelarOrdemInfo>()
                     {
                         EntidadeCadastro = lCancelarOrdemInfo
                     });
        }

        private void ListasSinacor()
        {


            SinacorListaInfo x = new SinacorListaInfo();
            x.Informacao = eInformacao.Assessor;
            x.Id = "100";

            ConsultarEntidadeCadastroResponse<SinacorListaInfo> lConsultaResponse =
                   (ConsultarEntidadeCadastroResponse<SinacorListaInfo>)
                   this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaInfo>(
                   new ConsultarEntidadeCadastroRequest<SinacorListaInfo>()
                   {
                       EntidadeCadastro = x
                   });
            // lRetorno = lConsultaResponse.Resultado;

        }

        private void ExcluirTodosClientes()
        {
            for (int i = 18215; i < 36140; i++)
            {
                RemoverEntidadeCadastroRequest<ClienteInfo> remov = new RemoverEntidadeCadastroRequest<ClienteInfo>();
                remov.EntidadeCadastro = new ClienteInfo();
                remov.EntidadeCadastro.IdCliente = i;

                RemoverEntidadeCadastroResponse lRemoverResponse =
                (RemoverEntidadeCadastroResponse)
                this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteInfo>(remov);
            }
        }

        private void ExportarCLiente()
        {
            SinacorExportarInfo lCliente = new SinacorExportarInfo();
            lCliente.Entrada = new SinacorExportacaoEntradaInfo();
            lCliente.Entrada.IdCliente = 16025;
            lCliente.Entrada.PrimeiraExportacao = false;
            lCliente.Entrada.CdCodigo = 31940;

            SalvarEntidadeCadastroRequest<SinacorExportarInfo> lClienteExportacao = new SalvarEntidadeCadastroRequest<SinacorExportarInfo>();
            lClienteExportacao.EntidadeCadastro = lCliente;



            SalvarEntidadeCadastroResponse lCadastroResponse =
                (SalvarEntidadeCadastroResponse)
                this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<SinacorExportarInfo>(lClienteExportacao);


            // lCadastroResponse.


        }

        private void ListarTodosDoSinacor()
        {
            //SinacorChaveClienteInfo lChave = new SinacorChaveClienteInfo();
            //EntidadeCadastro = lChave
            string ok = "";
            string erro = "";

            try
            {

                //Listar Todas as chaves
                #region "Listar Chaves do Sinacor"
                ConsultarEntidadeCadastroResponse<SinacorChaveClienteInfo> lTodasChavesSinacor =
                    (ConsultarEntidadeCadastroResponse<SinacorChaveClienteInfo>)
                    this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorChaveClienteInfo>(
                        new ConsultarEntidadeCadastroRequest<SinacorChaveClienteInfo>() { }
                    );
                #endregion
                //Para cada chave, pegar o Cliente na estrutura do Sinacor
                //System.Collections.Generic.List<SinacorClienteInfo> lTodosClienteSinacorCompleto = new System.Collections.Generic.List<SinacorClienteInfo>();
                #region "Listar Clientes do Sinacor a partir das Chaves"
                foreach (SinacorChaveClienteInfo item in lTodasChavesSinacor.Resultado)
                {
                    SinacorClienteInfo lCliente = new SinacorClienteInfo();
                    lCliente.ChaveCliente = item;
                    ReceberEntidadeCadastroResponse<SinacorClienteInfo> lClienteSinacor =
                    (ReceberEntidadeCadastroResponse<SinacorClienteInfo>)
                    this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<SinacorClienteInfo>(
                        new ReceberEntidadeCadastroRequest<SinacorClienteInfo>()
                        {
                            EntidadeCadastro = lCliente
                        }
                    );

                    //antes adidionava em um list, agora inclui
                    //lTodosClienteSinacorCompleto.Add(lClienteSinacor.EntidadeCadastro);
                    SalvarEntidadeCadastroRequest<SinacorClienteInfo> lClienteInclusao = new SalvarEntidadeCadastroRequest<SinacorClienteInfo>();
                    lClienteInclusao.EntidadeCadastro = lClienteSinacor.EntidadeCadastro;

                    try
                    {

                        SalvarEntidadeCadastroResponse lCadastroResponse =
                            (SalvarEntidadeCadastroResponse)
                            this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<SinacorClienteInfo>(lClienteInclusao);
                        ok += lClienteInclusao.EntidadeCadastro.ChaveCliente.CD_CPFCGC + " - " + Environment.NewLine;


                    }
                    catch (Exception ex)
                    {
                        erro += lClienteInclusao.EntidadeCadastro.ChaveCliente.CD_CPFCGC + " - " + ex.Message + " - " + Environment.NewLine;
                    }




                }
                #endregion

                ////Para cada cliente do Sinacor Importar para o Cadastro
                #region "Inserir Cliente a partir do Sinacor"
                //string ok = "";
                //string erro = "";
                //foreach (SinacorClienteInfo item in lTodosClienteSinacorCompleto)
                //{

                //    try
                //    {
                //        SalvarEntidadeCadastroRequest<SinacorClienteInfo> lCliente = new SalvarEntidadeCadastroRequest<SinacorClienteInfo>();
                //        lCliente.EntidadeCadastro = item;

                //        SalvarEntidadeCadastroResponse lCadastroResponse =
                //            (SalvarEntidadeCadastroResponse)
                //            this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<SinacorClienteInfo>(lCliente);

                //        ok += item.ChaveCliente.CD_CPFCGC + " - " + Environment.NewLine;
                //    }
                //    catch (Exception ex)
                //    {
                //        erro = item.ChaveCliente.CD_CPFCGC + " - " + ex.Message + " - " + Environment.NewLine;
                //    }
                //}
                #endregion

            }
            catch (Exception ex)
            {


                Response.Write(ex.Message);
            }

            string lOk = ok;
            string lErro = erro;

        }

        private void AtualizarDataValidadeClienteRenovacaoCadastral()
        {
            var lClientePendenciaCadastralInfo = new ClientePendenciaCadastralInfo()
            {
                IdCliente = 69
            };

            ConsultarEntidadeCadastroResponse<ClientePendenciaCadastralInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClientePendenciaCadastralInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientePendenciaCadastralInfo>(
                    new ConsultarEntidadeCadastroRequest<ClientePendenciaCadastralInfo>()
                    {
                        EntidadeCadastro = lClientePendenciaCadastralInfo
                    });

            if (lConsultaResponse.Resultado.Count > 0) //--> Valida se o cliente possui pendências cadastrais.
                throw new Exception("O cliente não pode ser atualizado, pois ainda possui pendências cadastrais.");
            else
            {
                SalvarEntidadeCadastroResponse lCadastroResponse =
                    (SalvarEntidadeCadastroResponse)
                    this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteRenovacaoCadastralInfo>(
                        new SalvarEntidadeCadastroRequest<ClienteRenovacaoCadastralInfo>()
                        {
                            EntidadeCadastro = new ClienteRenovacaoCadastralInfo { DsCpfCnpj = "30862071836" }
                        });
            }
        }

        private void ClienteRenovacaoCadastral()
        {
            var lClienteRenovacaoCadastral = new ClienteRenovacaoCadastralInfo();

            ConsultarEntidadeCadastroResponse<ClienteRenovacaoCadastralInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteRenovacaoCadastralInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteRenovacaoCadastralInfo>(
                    new ConsultarEntidadeCadastroRequest<ClienteRenovacaoCadastralInfo>()
                    {
                        EntidadeCadastro = lClienteRenovacaoCadastral
                    });

        }

        //private void FichaPessoaJuridica()
        //{
        //    var lFichaPessoaJuridicaInfo = new FichaPessoaJuridicaInfo();

        //    lFichaPessoaJuridicaInfo.ClienteResponse.Objeto = new ClienteInfo();

        //    lFichaPessoaJuridicaInfo.ClienteResponse.Objeto.IdCliente = 6;

        //    ReceberEntidadeCadastroResponse<FichaPessoaJuridicaInfo> lReceberResponse =
        //        (ReceberEntidadeCadastroResponse<FichaPessoaJuridicaInfo>)
        //        this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<FichaPessoaJuridicaInfo>(
        //            new ReceberEntidadeCadastroRequest<FichaPessoaJuridicaInfo>()
        //            {
        //                EntidadeCadastro = lFichaPessoaJuridicaInfo
        //            });
        //}

        //private void FichaPessoaFisica()
        //{
        //    var lFichaPessoaFisicaInfo = new FichaPessoaFisicaInfo();

        //    lFichaPessoaFisicaInfo.ClienteResponse.Objeto = new ClienteInfo();

        //    lFichaPessoaFisicaInfo.ClienteResponse.Objeto.IdCliente = 6;

        //    ReceberEntidadeCadastroResponse<FichaPessoaFisicaInfo> lReceberResponse =
        //        (ReceberEntidadeCadastroResponse<FichaPessoaFisicaInfo>)
        //        this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<FichaPessoaFisicaInfo>(
        //            new ReceberEntidadeCadastroRequest<FichaPessoaFisicaInfo>()
        //            {
        //                EntidadeCadastro = lFichaPessoaFisicaInfo
        //            });
        //}

        private void Telefone()
        {
            var lClienteTelefoneInfo = new ClienteTelefoneInfo()
            {
                DsDdd = "32",
                DsNumero = "84522829",
                DsRamal = null,
                IdCliente = 10,
                //IdTelefone = 6,
                IdTipoTelefone = 1,
                StPrincipal = true
            };


            SalvarEntidadeCadastroResponse lCadastroResponse =
                (SalvarEntidadeCadastroResponse)
                this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteTelefoneInfo>(
                new SalvarEntidadeCadastroRequest<ClienteTelefoneInfo>()
                {
                    EntidadeCadastro = lClienteTelefoneInfo
                });

            ConsultarEntidadeCadastroResponse<ClienteTelefoneInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteTelefoneInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteTelefoneInfo>(
                    new ConsultarEntidadeCadastroRequest<ClienteTelefoneInfo>()
                    {
                        EntidadeCadastro = lClienteTelefoneInfo
                    });

            ReceberEntidadeCadastroResponse<ClienteTelefoneInfo> lReceberResponse =
                (ReceberEntidadeCadastroResponse<ClienteTelefoneInfo>)
                this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteTelefoneInfo>(
                    new ReceberEntidadeCadastroRequest<ClienteTelefoneInfo>()
                    {
                        EntidadeCadastro = lClienteTelefoneInfo
                    });

            RemoverEntidadeCadastroResponse lRemoverResponse =
                (RemoverEntidadeCadastroResponse)
                this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteTelefoneInfo>(
                     new RemoverEntidadeCadastroRequest<ClienteTelefoneInfo>()
                     {
                         EntidadeCadastro = lClienteTelefoneInfo
                     });
        }

        private void Login()
        {
            var lLoginInfo = new LoginInfo()
            {
                CdAssinaturaEletronica = "Via Embratel",
                CdSenha = "123456",
                DsEmail = "sistemas@gradualinvestimentos.com.br",
                DsRespostaFrase = "bla bla bla",
                DtUltimaExpiracao = DateTime.Now.AddMonths(-5),
                IdFrase = 1,
                //IdLogin = 6,
                NrTentativasErradas = 4
            };


            SalvarEntidadeCadastroResponse lCadastroResponse =
                (SalvarEntidadeCadastroResponse)
                this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<LoginInfo>(
                new SalvarEntidadeCadastroRequest<LoginInfo>()
                    {
                        EntidadeCadastro = lLoginInfo
                    });

            ConsultarEntidadeCadastroResponse<LoginInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<LoginInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<LoginInfo>(
                    new ConsultarEntidadeCadastroRequest<LoginInfo>()
                    {
                        EntidadeCadastro = lLoginInfo
                    });

            ReceberEntidadeCadastroResponse<LoginInfo> lReceberResponse =
                (ReceberEntidadeCadastroResponse<LoginInfo>)
                this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<LoginInfo>(
                    new ReceberEntidadeCadastroRequest<LoginInfo>()
                    {
                        EntidadeCadastro = lLoginInfo
                    });

            RemoverEntidadeCadastroResponse lRemoverResponse =
                (RemoverEntidadeCadastroResponse)
                this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<LoginInfo>(
                     new RemoverEntidadeCadastroRequest<LoginInfo>()
                     {
                         EntidadeCadastro = lLoginInfo
                     });
        }

        private void EnderecoCliente()
        {
            ClienteEnderecoInfo lClienteEnderecoInfo = new ClienteEnderecoInfo()
            {
                CdPais = "BRA",
                CdUf = "SP",
                DsBairro = "ITAIM",
                DsCidade = "SAO PAULO",
                DsComplemento = "NA",
                DsLogradouro = "AV. SAO CARLOS",
                DsNumero = "331",
                IdCliente = 6,
                IdTipoEndereco = 3,
                NrCep = 03755,
                NrCepExt = 555,
                StPrincipal = true,
                IdEndereco = 29
            };

            SalvarEntidadeCadastroResponse lCadastroResponse =
                (SalvarEntidadeCadastroResponse)
                this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteEnderecoInfo>(
                    new SalvarEntidadeCadastroRequest<ClienteEnderecoInfo>()
                    {
                        EntidadeCadastro = lClienteEnderecoInfo
                    });

            RemoverEntidadeCadastroResponse lRemoverResponse =
                (RemoverEntidadeCadastroResponse)
                this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteEnderecoInfo>(
                    new RemoverEntidadeCadastroRequest<ClienteEnderecoInfo>()
                    {
                        EntidadeCadastro = lClienteEnderecoInfo
                    });

            ConsultarEntidadeCadastroResponse<ClienteEnderecoInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteEnderecoInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteEnderecoInfo>(
                    new ConsultarEntidadeCadastroRequest<ClienteEnderecoInfo>()
                    {
                        EntidadeCadastro = lClienteEnderecoInfo
                    });

            ReceberEntidadeCadastroResponse<ClienteEnderecoInfo> lReceberResponse =
                (ReceberEntidadeCadastroResponse<ClienteEnderecoInfo>)
                this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteEnderecoInfo>(
                    new ReceberEntidadeCadastroRequest<ClienteEnderecoInfo>()
                    {
                        EntidadeCadastro = lClienteEnderecoInfo
                    });
        }

        private void ConsultaListaSinacor()
        {

            var lConfiguracaoInfo = new SinacorListaInfo()
            {
                Informacao = eInformacao.Banco
            };


            ConsultarEntidadeCadastroResponse<SinacorListaInfo> lConsultaResponse =
                            (ConsultarEntidadeCadastroResponse<SinacorListaInfo>)
                            this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaInfo>(
                            new ConsultarEntidadeCadastroRequest<SinacorListaInfo>()
                            {
                                EntidadeCadastro = lConfiguracaoInfo
                            });


        }

        private void ClienteBusca()
        {
            var lClienteBusca = new ClienteResumidoInfo()
            {
                OpcaoBuscarPor = OpcoesBuscarPor.CodBovespa,
                TermoDeBusca = "1231",
                OpcaoPasso = OpcoesPasso.Visitante,
                OpcaoStatus = OpcoesStatus.Ativo,
                OpcaoTipo = OpcoesTipo.ClientePF
            };

            ConsultarEntidadeCadastroResponse<ClienteResumidoInfo> lConsultaResponse =
                            (ConsultarEntidadeCadastroResponse<ClienteResumidoInfo>)
                            this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteResumidoInfo>(
                            new ConsultarEntidadeCadastroRequest<ClienteResumidoInfo>()
                            {
                                EntidadeCadastro = lClienteBusca
                            });
        }

        private void Configuracao()
        {
            var lConfiguracaoInfo = new ConfiguracaoInfo()
            {
                Configuracao = EConfiguracaoDescricao.PeriodicidadeRenovacaoCadastral,
                IdConfiguracao = 3,
                Valor = "22"
            };

            SalvarEntidadeCadastroResponse lSalvarEntidadeCadastro =
          (SalvarEntidadeCadastroResponse)
          this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ConfiguracaoInfo>(
              new SalvarEntidadeCadastroRequest<ConfiguracaoInfo>()
              {
                  EntidadeCadastro = lConfiguracaoInfo
              });

            ConsultarEntidadeCadastroResponse<ConfiguracaoInfo> lConsultaResponse =
                            (ConsultarEntidadeCadastroResponse<ConfiguracaoInfo>)
                            this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ConfiguracaoInfo>(
                            new ConsultarEntidadeCadastroRequest<ConfiguracaoInfo>()
                            {
                                EntidadeCadastro = lConfiguracaoInfo
                            });

            ReceberEntidadeCadastroResponse<ConfiguracaoInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<ConfiguracaoInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ConfiguracaoInfo>(
               new ReceberEntidadeCadastroRequest<ConfiguracaoInfo>()
               {
                   EntidadeCadastro = lConfiguracaoInfo
               });

            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ConfiguracaoInfo>(
                         new RemoverEntidadeCadastroRequest<ConfiguracaoInfo>()
                         {
                             EntidadeCadastro = lConfiguracaoInfo
                         });

        }

        private void Cliente()
        {
            var lClienteInfo = new ClienteInfo()
            {
                IdCliente = 15,
                DsNome = "Altamir de Abreu",
                IdLogin = 2,
                DtUltimaAtualizacao = DateTime.Now,
                DsCpfCnpj = "00272272434",
                DtPasso1 = DateTime.Now,
                TpPessoa = '1',
                TpCliente = 0,
                StPasso = 1,
                DsCargo = "Don"
            };


            SalvarEntidadeCadastroResponse lSalvarEntidadeCadastro =
          (SalvarEntidadeCadastroResponse)
          this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteInfo>(
              new SalvarEntidadeCadastroRequest<ClienteInfo>()
              {
                  EntidadeCadastro = lClienteInfo
              });

            //lClienteInfo.IdCliente = lSalvarEntidadeCadastro.CodigoMensagem

            ConsultarEntidadeCadastroResponse<ClienteInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteInfo>(
                new ConsultarEntidadeCadastroRequest<ClienteInfo>()
                {
                    EntidadeCadastro = lClienteInfo
                });

            ReceberEntidadeCadastroResponse<ClienteInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<ClienteInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteInfo>(
               new ReceberEntidadeCadastroRequest<ClienteInfo>()
               {
                   EntidadeCadastro = lClienteInfo
               });

            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteInfo>(
                         new RemoverEntidadeCadastroRequest<ClienteInfo>()
                         {
                             EntidadeCadastro = lClienteInfo
                         });
        }

        private void SituacaoFinanceiraPatrimonial()
        {
            ClienteSituacaoFinanceiraPatrimonialInfo lClienteSituacaoFinanceiraPatrimonialInfo = new ClienteSituacaoFinanceiraPatrimonialInfo()
            {
                DtAtualizacao = DateTime.Now,
                DtCapitalSocial = DateTime.Now,
                DtPatrimonioLiquido = DateTime.Now,
                IdCliente = 6,
                DsOutrosRendimentos = "carro",
                VlTotalAplicacaoFinanceira = 100,
                VlTotalOutrosRendimentos = 100,
                VlTotalSalarioProLabore = 100,
                VlTotalBensImoveis = 100,
                VlTotalBensMoveis = 100,
                VlTotalPatrimonioLiquido = 100,
                IdSituacaoFinanceiraPatrimonial = 5
            };


            SalvarEntidadeCadastroResponse lCadastroResponse =
          (SalvarEntidadeCadastroResponse)
          this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteSituacaoFinanceiraPatrimonialInfo>(
              new SalvarEntidadeCadastroRequest<ClienteSituacaoFinanceiraPatrimonialInfo>()
              {
                  EntidadeCadastro = lClienteSituacaoFinanceiraPatrimonialInfo
              });


            RemoverEntidadeCadastroResponse lRemoverResponse =
             (RemoverEntidadeCadastroResponse)
                 this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteSituacaoFinanceiraPatrimonialInfo>(
                     new RemoverEntidadeCadastroRequest<ClienteSituacaoFinanceiraPatrimonialInfo>()
                     {
                         EntidadeCadastro = lClienteSituacaoFinanceiraPatrimonialInfo
                     });

        }

        private void ClienteContrato()
        {
            ClienteContratoInfo lClienteContratoInfo = new ClienteContratoInfo()
            {
                DtAssinatura = DateTime.Now,
                IdCliente = 6,
                IdContrato = 3
            };

            SalvarEntidadeCadastroResponse lCadastroResponse =
           (SalvarEntidadeCadastroResponse)
           this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteContratoInfo>(
               new SalvarEntidadeCadastroRequest<ClienteContratoInfo>()
               {
                   EntidadeCadastro = lClienteContratoInfo
               });

            ConsultarEntidadeCadastroResponse<ClienteContratoInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteContratoInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteContratoInfo>(
                new ConsultarEntidadeCadastroRequest<ClienteContratoInfo>()
                {
                    EntidadeCadastro = lClienteContratoInfo
                });


            ReceberEntidadeCadastroResponse<ClienteContratoInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<ClienteContratoInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteContratoInfo>(
               new ReceberEntidadeCadastroRequest<ClienteContratoInfo>()
               {
                   EntidadeCadastro = lClienteContratoInfo
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteContratoInfo>(
                         new RemoverEntidadeCadastroRequest<ClienteContratoInfo>()
                         {
                             EntidadeCadastro = lClienteContratoInfo
                         });
        }

        /// <summary>
        /// ClienteContrato
        /// </summary>
        private void Contrato()
        {
            ContratoInfo lContratoInfo = new ContratoInfo()
            {
                DsContrato = "Termo",
                DsPath = "caminho",
                StObrigatorio = true,
                IdContrato = 2
            };

            SalvarEntidadeCadastroResponse lCadastroResponse =
           (SalvarEntidadeCadastroResponse)
           this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ContratoInfo>(
               new SalvarEntidadeCadastroRequest<ContratoInfo>()
               {
                   EntidadeCadastro = lContratoInfo
               });

            ConsultarEntidadeCadastroResponse<ContratoInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ContratoInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ContratoInfo>(
                new ConsultarEntidadeCadastroRequest<ContratoInfo>()
                {
                    EntidadeCadastro = lContratoInfo
                });

            ReceberEntidadeCadastroResponse<ContratoInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<ContratoInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ContratoInfo>(
               new ReceberEntidadeCadastroRequest<ContratoInfo>()
               {
                   EntidadeCadastro = lContratoInfo
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ContratoInfo>(
                         new RemoverEntidadeCadastroRequest<ContratoInfo>()
                         {
                             EntidadeCadastro = lContratoInfo
                         });
        }

        /// <summary>
        /// Cadastro de paises no blacklist
        /// </summary>
        private void AtividadesIlicitas()
        {
            AtividadeIlicitaInfo lAtividadeIlicitaInfo = new AtividadeIlicitaInfo()
            {
                CdAtividade = "1",
                IdAtividadeIlicita = 1
            };

            SalvarEntidadeCadastroResponse lCadastroResponse =
           (SalvarEntidadeCadastroResponse)
           this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<AtividadeIlicitaInfo>(
               new SalvarEntidadeCadastroRequest<AtividadeIlicitaInfo>()
               {
                   EntidadeCadastro = lAtividadeIlicitaInfo
               });

            ConsultarEntidadeCadastroResponse<AtividadeIlicitaInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<AtividadeIlicitaInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<AtividadeIlicitaInfo>(
                new ConsultarEntidadeCadastroRequest<AtividadeIlicitaInfo>()
                {
                    EntidadeCadastro = lAtividadeIlicitaInfo
                });

            ReceberEntidadeCadastroResponse<AtividadeIlicitaInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<AtividadeIlicitaInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<AtividadeIlicitaInfo>(
               new ReceberEntidadeCadastroRequest<AtividadeIlicitaInfo>()
               {
                   EntidadeCadastro = lAtividadeIlicitaInfo
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<AtividadeIlicitaInfo>(
                         new RemoverEntidadeCadastroRequest<AtividadeIlicitaInfo>()
                         {
                             EntidadeCadastro = lAtividadeIlicitaInfo
                         });

        }

        /// <summary>
        /// Cadastro de paises no blacklist
        /// </summary>
        private void PaisesBlackList()
        {
            PaisesBlackListInfo lPaisesBlackListInfo = new PaisesBlackListInfo()
            {
                CdPais = "BRA",
                IdPaisBlackList = 1
            };

            SalvarEntidadeCadastroResponse lCadastroResponse =
           (SalvarEntidadeCadastroResponse)
           this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<PaisesBlackListInfo>(
               new SalvarEntidadeCadastroRequest<PaisesBlackListInfo>()
               {
                   EntidadeCadastro = lPaisesBlackListInfo
               });

            ConsultarEntidadeCadastroResponse<PaisesBlackListInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<PaisesBlackListInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<PaisesBlackListInfo>(
                new ConsultarEntidadeCadastroRequest<PaisesBlackListInfo>()
                {
                    EntidadeCadastro = lPaisesBlackListInfo
                });

            ReceberEntidadeCadastroResponse<PaisesBlackListInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<PaisesBlackListInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<PaisesBlackListInfo>(
               new ReceberEntidadeCadastroRequest<PaisesBlackListInfo>()
               {
                   EntidadeCadastro = lPaisesBlackListInfo
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<PaisesBlackListInfo>(
                         new RemoverEntidadeCadastroRequest<PaisesBlackListInfo>()
                         {
                             EntidadeCadastro = lPaisesBlackListInfo
                         });

        }

        /// <summary>
        /// Cadastro de frases
        /// </summary>
        private void Frases()
        {
            FrasesInfo lFraseInfo = new FrasesInfo()
            {
                DsFrase = "Qual e o nome da mamis",
                IdFrase = 2
            };

            SalvarEntidadeCadastroResponse lCadastroResponse =
           (SalvarEntidadeCadastroResponse)
           this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<FrasesInfo>(
               new SalvarEntidadeCadastroRequest<FrasesInfo>()
               {
                   EntidadeCadastro = lFraseInfo
               });

            ConsultarEntidadeCadastroResponse<FrasesInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<FrasesInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<FrasesInfo>(
                new ConsultarEntidadeCadastroRequest<FrasesInfo>()
                {
                    EntidadeCadastro = lFraseInfo
                });

            ReceberEntidadeCadastroResponse<FrasesInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<FrasesInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<FrasesInfo>(
               new ReceberEntidadeCadastroRequest<FrasesInfo>()
               {
                   EntidadeCadastro = lFraseInfo
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<FrasesInfo>(
                         new RemoverEntidadeCadastroRequest<FrasesInfo>()
                         {
                             EntidadeCadastro = lFraseInfo
                         });

        }

        /// <summary>
        /// Cliente Controladora.
        /// </summary>
        private void ClienteControladora()
        {
            ClienteControladoraInfo lClienteControladoraInfo = new ClienteControladoraInfo()
            {
                DsCpfCnpj = "33414115859",
                DsNomeRazaoSocial = "SSSSSSS",
                IdCliente = 6,
                IdClienteControladora = 3
            };

            SalvarEntidadeCadastroResponse lCadastroResponse =
           (SalvarEntidadeCadastroResponse)
           this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteControladoraInfo>(
               new SalvarEntidadeCadastroRequest<ClienteControladoraInfo>()
               {
                   EntidadeCadastro = lClienteControladoraInfo
               });

            ConsultarEntidadeCadastroResponse<ClienteControladoraInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteControladoraInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteControladoraInfo>(
                new ConsultarEntidadeCadastroRequest<ClienteControladoraInfo>()
                {
                    EntidadeCadastro = lClienteControladoraInfo
                });

            ReceberEntidadeCadastroResponse<ClienteControladoraInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<ClienteControladoraInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteControladoraInfo>(
               new ReceberEntidadeCadastroRequest<ClienteControladoraInfo>()
               {
                   EntidadeCadastro = lClienteControladoraInfo
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteControladoraInfo>(
                         new RemoverEntidadeCadastroRequest<ClienteControladoraInfo>()
                         {
                             EntidadeCadastro = lClienteControladoraInfo
                         });

        }

        /// <summary>
        /// Cliente Diretpr
        /// </summary>
        private void ClienteDiretor()
        {
            ClienteDiretorInfo lClienteDiretorInfo = new ClienteDiretorInfo()
            {
                NrCpfCnpj = "3341334334",
                DsIdentidade = "30764585",
                DsNome = "Rafaelllllll",
                IdCliente = 6,
                IdClienteDiretor = 2
            };


            SalvarEntidadeCadastroResponse lCadastroResponse =
           (SalvarEntidadeCadastroResponse)
           this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteDiretorInfo>(
               new SalvarEntidadeCadastroRequest<ClienteDiretorInfo>()
               {
                   EntidadeCadastro = lClienteDiretorInfo
               });

            ConsultarEntidadeCadastroResponse<ClienteDiretorInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteDiretorInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteDiretorInfo>(
                new ConsultarEntidadeCadastroRequest<ClienteDiretorInfo>()
                {
                    EntidadeCadastro = lClienteDiretorInfo
                });

            ReceberEntidadeCadastroResponse<ClienteDiretorInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<ClienteDiretorInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteDiretorInfo>(
               new ReceberEntidadeCadastroRequest<ClienteDiretorInfo>()
               {
                   EntidadeCadastro = lClienteDiretorInfo
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteDiretorInfo>(
                         new RemoverEntidadeCadastroRequest<ClienteDiretorInfo>()
                         {
                             EntidadeCadastro = lClienteDiretorInfo
                         });

        }

        /// <summary>
        /// Emitente
        /// </summary>
        private void Emitente()
        {
            ClienteEmitenteInfo lClienteEmitenteInfo = new ClienteEmitenteInfo()
            {
                CdSistema = "001",
                NrCpfCnpj = "33414115859",
                DsData = DateTime.Now,
                DsEmail = "RS007@gradualinvestimentos.com.br",
                DsNome = "RS007001",
                DsNumeroDocumento = "987654321",
                DtNascimento = DateTime.Now.AddDays(-60),
                IdCliente = 6,
                StPrincipal = true,
                IdPessoaAutorizada = 11
            };

            SalvarEntidadeCadastroResponse lCadastroResponse =
       (SalvarEntidadeCadastroResponse)
           this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteEmitenteInfo>(
               new SalvarEntidadeCadastroRequest<ClienteEmitenteInfo>()
               {
                   EntidadeCadastro = lClienteEmitenteInfo
               });

            ConsultarEntidadeCadastroResponse<ClienteEmitenteInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteEmitenteInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteEmitenteInfo>(
                new ConsultarEntidadeCadastroRequest<ClienteEmitenteInfo>()
                {
                    EntidadeCadastro = lClienteEmitenteInfo
                });

            ReceberEntidadeCadastroResponse<ClienteEmitenteInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<ClienteEmitenteInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteEmitenteInfo>(
               new ReceberEntidadeCadastroRequest<ClienteEmitenteInfo>()
               {
                   EntidadeCadastro = lClienteEmitenteInfo
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteEmitenteInfo>(
                         new RemoverEntidadeCadastroRequest<ClienteEmitenteInfo>()
                         {
                             EntidadeCadastro = lClienteEmitenteInfo
                         });


        }

        /// <summary>
        /// Cliente procurador representante
        /// </summary>
        private void ClienteProcuradorRepresentante()
        {

            ClienteProcuradorRepresentanteInfo lClienteProcuradorRepresentanteInfo = new ClienteProcuradorRepresentanteInfo()
            {
                CdOrgaoEmissor = "003",
                CdUfOrgaoEmissor = "004",
                NrCpfCnpj = "33414115874",
                DsNome = "Rafael",
                DsNumeroDocumento = "30764585",
                DtNascimento = DateTime.Now,
                IdCliente = 6,
                IdProcuradorRepresentante = 1,
                TpDocumento = "RG"
            };

            SalvarEntidadeCadastroResponse lCadastroResponse =
       (SalvarEntidadeCadastroResponse)
           this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteProcuradorRepresentanteInfo>(
               new SalvarEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo>()
               {
                   EntidadeCadastro = lClienteProcuradorRepresentanteInfo
               });

            ConsultarEntidadeCadastroResponse<ClienteProcuradorRepresentanteInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteProcuradorRepresentanteInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteProcuradorRepresentanteInfo>(
                new ConsultarEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo>()
                {
                    EntidadeCadastro = lClienteProcuradorRepresentanteInfo
                });

            ReceberEntidadeCadastroResponse<ClienteProcuradorRepresentanteInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<ClienteProcuradorRepresentanteInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteProcuradorRepresentanteInfo>(
               new ReceberEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo>()
               {
                   EntidadeCadastro = lClienteProcuradorRepresentanteInfo
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteProcuradorRepresentanteInfo>(
                         new RemoverEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo>()
                         {
                             EntidadeCadastro = lClienteProcuradorRepresentanteInfo
                         });
        }

        /// <summary>
        /// Cliente banco
        /// </summary>
        private void ClienteBanco()
        {
            ClienteBancoInfo lClienteBancoInfo =
            new ClienteBancoInfo()
            {
                CdBanco = "007",
                DsAgencia = "3751",
                DsConta = "04702",
                DsContaDigito = "1",
                IdBanco = 48,
                IdCliente = 6,
                StPrincipal = true,
                TpConta = "CC"
            };


            SalvarEntidadeCadastroResponse lCadastroResponse =
          (SalvarEntidadeCadastroResponse)

              this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteBancoInfo>(
                  new SalvarEntidadeCadastroRequest<ClienteBancoInfo>()
                  {
                      EntidadeCadastro = lClienteBancoInfo
                  });


            ConsultarEntidadeCadastroResponse<ClienteBancoInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClienteBancoInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteBancoInfo>(
                new ConsultarEntidadeCadastroRequest<ClienteBancoInfo>()
                {
                    EntidadeCadastro = lClienteBancoInfo
                });

            ReceberEntidadeCadastroResponse<ClienteBancoInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<ClienteBancoInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteBancoInfo>(
               new ReceberEntidadeCadastroRequest<ClienteBancoInfo>()
               {
                   EntidadeCadastro = lClienteBancoInfo
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteBancoInfo>(
                         new RemoverEntidadeCadastroRequest<ClienteBancoInfo>()
                         {
                             EntidadeCadastro = lClienteBancoInfo
                         });

        }

        /// <summary>
        /// Tipo de Telefone
        /// </summary>
        private void TipoTelefone()
        {
            TipoTelefoneInfo lTipoTelefoneInfo =
            new TipoTelefoneInfo()
            {
                DsTelefone = "Celular Teste",
                IdTipoTelefone = 2
            };


            SalvarEntidadeCadastroResponse lCadastroResponse =
          (SalvarEntidadeCadastroResponse)
              this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<TipoTelefoneInfo>(
                  new SalvarEntidadeCadastroRequest<TipoTelefoneInfo>()
                  {
                      EntidadeCadastro = lTipoTelefoneInfo
                  });

            ConsultarEntidadeCadastroResponse<TipoTelefoneInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<TipoTelefoneInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<TipoTelefoneInfo>(
                new ConsultarEntidadeCadastroRequest<TipoTelefoneInfo>()
                {
                    EntidadeCadastro = lTipoTelefoneInfo
                });

            ReceberEntidadeCadastroResponse<TipoTelefoneInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<TipoTelefoneInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<TipoTelefoneInfo>(
               new ReceberEntidadeCadastroRequest<TipoTelefoneInfo>()
               {
                   EntidadeCadastro = lTipoTelefoneInfo
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<TipoTelefoneInfo>(
                         new RemoverEntidadeCadastroRequest<TipoTelefoneInfo>()
                         {
                             EntidadeCadastro = lTipoTelefoneInfo
                         });


        }

        /// <summary>
        /// Cliente pendencia cadastral
        /// </summary>
        private void ClientePendenciaCadastral()
        {

            ClientePendenciaCadastralInfo info = new ClientePendenciaCadastralInfo();

            info.DsPendencia = "chap chap ";
            info.DtPendencia = DateTime.Now;
            info.IdCliente = 6;
            info.IdTipoPendencia = 8;
            info.IdPendenciaCadastral = 31;

            SalvarEntidadeCadastroResponse lCadastroResponse =
            (SalvarEntidadeCadastroResponse)
             this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClientePendenciaCadastralInfo>(
             new SalvarEntidadeCadastroRequest<ClientePendenciaCadastralInfo>()
             {
                 EntidadeCadastro = info
             });


            ConsultarEntidadeCadastroResponse<ClientePendenciaCadastralInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<ClientePendenciaCadastralInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientePendenciaCadastralInfo>(
                new ConsultarEntidadeCadastroRequest<ClientePendenciaCadastralInfo>()
                {
                    EntidadeCadastro = info
                });

            ReceberEntidadeCadastroResponse<ClientePendenciaCadastralInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<ClientePendenciaCadastralInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClientePendenciaCadastralInfo>(
               new ReceberEntidadeCadastroRequest<ClientePendenciaCadastralInfo>()
               {
                   EntidadeCadastro = info
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClientePendenciaCadastralInfo>(
                         new RemoverEntidadeCadastroRequest<ClientePendenciaCadastralInfo>()
                         {
                             EntidadeCadastro = info
                         });


        }

        /// <summary>
        /// Tipo de Pendencia Cadastral
        /// </summary>
        private void PendenciaCadastral()
        {
            TipoDePendenciaCadastralInfo lPendenciaInfo =
            new TipoDePendenciaCadastralInfo()
            {
                IdTipoPendencia = 8,
                DsPendencia = "Serasa"
            };


            SalvarEntidadeCadastroResponse lCadastroResponse =
          (SalvarEntidadeCadastroResponse)
              this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<TipoDePendenciaCadastralInfo>(
                  new SalvarEntidadeCadastroRequest<TipoDePendenciaCadastralInfo>()
                  {
                      EntidadeCadastro = lPendenciaInfo
                  });

            ConsultarEntidadeCadastroResponse<TipoDePendenciaCadastralInfo> lConsultaResponse =
                (ConsultarEntidadeCadastroResponse<TipoDePendenciaCadastralInfo>)
                this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<TipoDePendenciaCadastralInfo>(
                new ConsultarEntidadeCadastroRequest<TipoDePendenciaCadastralInfo>()
                {
                    EntidadeCadastro = lPendenciaInfo
                });

            ReceberEntidadeCadastroResponse<TipoDePendenciaCadastralInfo> lReceberResponse =
               (ReceberEntidadeCadastroResponse<TipoDePendenciaCadastralInfo>)
               this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<TipoDePendenciaCadastralInfo>(
               new ReceberEntidadeCadastroRequest<TipoDePendenciaCadastralInfo>()
               {
                   EntidadeCadastro = lPendenciaInfo
               });


            RemoverEntidadeCadastroResponse lRemoverResponse =
                 (RemoverEntidadeCadastroResponse)
                     this.ServicoPersistenciaCadastro.RemoverEntidadeCadastro<TipoDePendenciaCadastralInfo>(
                         new RemoverEntidadeCadastroRequest<TipoDePendenciaCadastralInfo>()
                         {
                             EntidadeCadastro = lPendenciaInfo
                         });

        }

        #endregion
    }
}
