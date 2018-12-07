using System;
using System.Text;
using Gradual.Generico.Geral;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Portal;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Library;
using System.Diagnostics;
using Gradual.OMS.Seguranca.Lib;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.Www.Async
{
    public partial class Login : PaginaBase
    {
        #region Métodos Private

        private void ProcessarLoginPrimeiroAcesso()
        {
            if (SessaoClienteLogado != null && !SessaoClienteLogado.PrimeiroLoginJaVerificado)
            {
                ReceberEntidadeCadastroRequest<PrimeiroAcessoValidaInfo> lRequest = new ReceberEntidadeCadastroRequest<PrimeiroAcessoValidaInfo>();
                ReceberEntidadeCadastroResponse<PrimeiroAcessoValidaInfo> lResponse;

                lRequest.EntidadeCadastro = new PrimeiroAcessoValidaInfo();

                lRequest.EntidadeCadastro.DsEmail = SessaoClienteLogado.Email;
                lRequest.EntidadeCadastro.CdCodigo = SessaoClienteLogado.CodigoPrincipal.DBToInt32();

                lRequest.DescricaoUsuarioLogado = string.Format("Tela de Login do Portal - IP: [{0}]", base.Request.UserHostAddress);

                lResponse = ServicoPersistenciaCadastro.ReceberEntidadeCadastro<PrimeiroAcessoValidaInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    SessaoClienteLogado.PrimeiroLoginJaVerificado = true;

                    if (lResponse.EntidadeCadastro.StPrimeiroAcesso)
                    {
                        this.Response.Redirect(HostERaizFormat("MinhaConta/Cadastro/PrimeiroAcesso.aspx"));

                        return;
                    }
                }
                else
                {
                    throw new Exception(lResponse.DescricaoResposta);
                }
            }
        }

        private AutenticarUsuarioResponse LoginViaServicoSeguranca(string pEmailCodigo, string pSenha)
        {
            AutenticarUsuarioRequest  lRequestAutenticacao;
            AutenticarUsuarioResponse lResponseAutenticacao;

            ReceberSessaoRequest  lSessaoRequest = new ReceberSessaoRequest();
            ReceberSessaoResponse lSessaoResponse;

            ContextoOMSInfo lContextoOMS;

            lRequestAutenticacao = new AutenticarUsuarioRequest();

            lRequestAutenticacao.Email = pEmailCodigo;
            lRequestAutenticacao.Senha = Criptografia.CalculateMD5Hash(pSenha);
            
            lRequestAutenticacao.CodigoSistemaCliente = "Portal";

            lRequestAutenticacao.IP    = Request.ServerVariables["REMOTE_ADDR"];

            try
            {
                gLogger.InfoFormat("PainelDeLogin.ascx - LoginViaServicoSeguranca(DsEmail:[{0}])", lRequestAutenticacao.Email);

                lResponseAutenticacao = ServicoSeguranca.AutenticarUsuario(lRequestAutenticacao);
            }
            catch (System.ServiceModel.CommunicationObjectFaultedException)
            {
                Application["ServicoSeguranca"] = null;

                lResponseAutenticacao = ServicoSeguranca.AutenticarUsuario(lRequestAutenticacao);
            }

            gLogger.InfoFormat("PainelDeLogin.ascx - LoginViaServicoSeguranca(DsEmail:[{0}]) Resposta: [{1}] [{2}]"
                                , lRequestAutenticacao.Email
                                , lResponseAutenticacao.StatusResposta
                                , lResponseAutenticacao.DescricaoResposta);

            if (lResponseAutenticacao.StatusResposta == Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
            {
                lSessaoRequest = new ReceberSessaoRequest();

                lSessaoRequest.CodigoSessao          = lResponseAutenticacao.Sessao.CodigoSessao;
                lSessaoRequest.CodigoSessaoARetornar = lResponseAutenticacao.Sessao.CodigoSessao;

                gLogger.InfoFormat("PainelDeLogin.ascx - LoginViaServicoSeguranca - ReceberSessao([{0}])", lSessaoRequest.CodigoSessao);

                lSessaoResponse = ServicoSeguranca.ReceberSessao(lSessaoRequest);

                if (lSessaoResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    gLogger.Info("PainelDeLogin.ascx - LoginViaServicoSeguranca - ReceberContexto()");

                    lContextoOMS = lSessaoResponse.Usuario.Complementos.ReceberItem<ContextoOMSInfo>();

                    SessaoClienteLogado = new TransporteSessaoClienteLogado();

                    SessaoClienteLogado.Senha = lRequestAutenticacao.Senha;

                    SessaoClienteLogado.CodigoDaSessao = lResponseAutenticacao.Sessao.CodigoSessao;
                    
                    SessaoClienteLogado.TipoAcesso = (TransporteSessaoClienteLogado.EnumTipoCliente)lSessaoResponse.Usuario.CodigoTipoAcesso;
                    //SessaoClienteLogado.CodAssessor = lSessaoResponse.Usuario.CodigoAssessor;

                    SessaoClienteLogado.IdLogin          = lSessaoResponse.Usuario.CodigoUsuario.DBToInt32();
                    SessaoClienteLogado.CodigoBMF        = lContextoOMS.CodigoBMF.DBToInt32();
                    SessaoClienteLogado.CodigoPrincipal  = lContextoOMS.CodigoCBLC;
                    SessaoClienteLogado.Nome             = lSessaoResponse.Usuario.Nome;
                    SessaoClienteLogado.Email            = lSessaoResponse.Usuario.Email;

                    if (SessaoClienteLogado.CodigoPrincipal == "31940")
                    {
                        if(!SessaoClienteLogado.Pode(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarCMS))
                        {
                            SessaoClienteLogado.Permissoes.Add(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarCMS);
                        }
                    }

                    gLogger.InfoFormat("PainelDeLogin.ascx - Sessão para [{0}]: [{1}]"
                                        , lRequestAutenticacao.Email
                                        , lResponseAutenticacao.Sessao.CodigoSessao);
                }
                else
                {
                    gLogger.ErrorFormat("Retorno do serviço de segurança com erro em PainelDeLogin.ascx - LoginViaServicoSeguranca - ReceberSessao(DsEmail:[{0}]) [{1}]\r\n{2}"
                                        , lSessaoRequest.CodigoSessao
                                        , lSessaoResponse.StatusResposta
                                        , lSessaoResponse.DescricaoResposta);
                }
            }
            else
            {
                gLogger.ErrorFormat("Retorno do serviço de segurança com erro em PainelDeLogin.ascx - LoginViaServicoSeguranca(DsEmail:[{0}]) [{1}]\r\n{2}"
                                    , lRequestAutenticacao.Email
                                    , lResponseAutenticacao.StatusResposta
                                    , lResponseAutenticacao.DescricaoResposta);

            }

            return lResponseAutenticacao;
        }

        private void BuscarPermissoesDoUsuario()
        {
            ReceberUsuarioRequest lRequest = new ReceberUsuarioRequest();
            ReceberUsuarioResponse lResponse;

            if (!string.IsNullOrEmpty(SessaoClienteLogado.CodigoDaSessao))
            {
                lRequest.CodigoUsuario = SessaoClienteLogado.IdLogin.DBToString();
                lRequest.CodigoSessao = SessaoClienteLogado.CodigoDaSessao;

                gLogger.ErrorFormat("PainelDeLogin.ascx > BuscarPermissoesDoUsuario(CodigoSessao [{0}], IdLogin [{1}])"
                                    , lRequest.CodigoSessao
                                    , lRequest.CodigoUsuario);

                lResponse = ServicoSeguranca.ReceberUsuario(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    gLogger.InfoFormat("PainelDeLogin.ascx > Retorno OK do serviço: [{0}] permissões para [{1}])"
                                        , lResponse.Usuario.Permissoes.Count
                                        , lRequest.CodigoUsuario);

                    foreach (PermissaoAssociadaInfo lPermissao in lResponse.Usuario.Permissoes)
                    {
                        if (lPermissao.CodigoPermissao == ConfiguracoesValidadas.CodigoPermissao_EditarCMS)
                            SessaoClienteLogado.Permissoes.Add(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarCMS);

                        if (lPermissao.CodigoPermissao == ConfiguracoesValidadas.CodigoPermissao_EditarAnaliseEconomica)
                            SessaoClienteLogado.Permissoes.Add(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseEconomica);

                        if (lPermissao.CodigoPermissao == ConfiguracoesValidadas.CodigoPermissao_EditarAnaliseFundamentalista)
                            SessaoClienteLogado.Permissoes.Add(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseFundamentalista);

                        if (lPermissao.CodigoPermissao == ConfiguracoesValidadas.CodigoPermissao_EditarAnaliseGrafica)
                            SessaoClienteLogado.Permissoes.Add(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarAnaliseGrafica);

                        if (lPermissao.CodigoPermissao == ConfiguracoesValidadas.CodigoPermissao_EditarCarteirasRecomendadas)
                            SessaoClienteLogado.Permissoes.Add(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarCarteirasRecomendadas);

                        if (lPermissao.CodigoPermissao == ConfiguracoesValidadas.CodigoPermissao_EditarNikkei)
                            SessaoClienteLogado.Permissoes.Add(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarNikkei);

                        if (lPermissao.CodigoPermissao == ConfiguracoesValidadas.CodigoPermissao_EditarGradiusGestao)
                            SessaoClienteLogado.Permissoes.Add(TransporteSessaoClienteLogado.PermissoesPertinentesAoSite.EditarGradiusGestao);
                    }
                }
                else
                {
                    gLogger.ErrorFormat("Retorno com erro do ServicoSeguranca em PainelDeLogin.ascx > BuscarPermissoesDoUsuario(CodigoSessao [{0}], IdLogin [{1}]) [{2}] \r\n{3}"
                                        , lRequest.CodigoSessao
                                        , lRequest.CodigoUsuario
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);
                }
            }
        }

        private string ResponderEfetuarLogin()
        {
            string lMensagem;

            AutenticarUsuarioResponse lResponse;

            try
            {
                lResponse = LoginViaServicoSeguranca(Request["login"], Request["senha"]);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    ((PaginaBase)this.Page).CarregarClienteEmSessao(null);

                    BuscarPermissoesDoUsuario();

                    ValidarExpiracaoDeSenha();

                    ProcessarLoginPrimeiroAcesso();

                    lMensagem = "URL:" + RedirecionarResposta();
                }
                else if(lResponse.StatusResposta == MensagemResponseStatusEnum.ErroValidacao)
                {
                    //estouraram as 3 tentativas, usuário bloqueado

                    lMensagem = "SENHA:A quantidade limite de erros de senha foi excedida e seu acesso foi bloqueado. Para efetuar o desbloqueio, entre em contato com a nossa Central de Atendimento.";
                }
                else
                {
                    lMensagem = lResponse.DescricaoResposta.ToLower();

                    if (lMensagem == "usuário inválido" || lMensagem == "usuario não encontrado")
                    {
                        //usuário/senha incorretos

                        lMensagem = "SENHA:Usuário ou Senha incorretos.";
                    }
                    else if (lMensagem.Contains("não possui acesso ao sistema"))
                    {
                        lMensagem = "SENHA:Usuário não possui acesso ao sistema. Favor entrar em contato com o atendimento.";
                    }
                    else
                    {
                        throw new Exception(string.Format("Erro interno ao realizar login; favor entrar em contato com o atendimento. Código de referência: [{0}]", DateTime.Now.ToString("yyMMdd-HHmmss")));
                    }
                }

                lMensagem = RetornarSucessoAjax(lMensagem);
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Loing.aspx: ResponderEfetuarLogin() [{0}]\r\n{1}", ex.Message, ex.StackTrace);

                lMensagem = RetornarErroAjax(("Erro de login: " + ex.Message), ex.StackTrace);
            }

            return lMensagem;
        }

        private string RedirecionarResposta()
        {
            string lUrlAnterior;

            string lRetorno = "";

            lUrlAnterior = string.Format("{0}", Session["RedirecionamentoPorFaltaDeLogin"]);

            double lTesteDoRandomico;

            if (double.TryParse(lUrlAnterior, out lTesteDoRandomico))
            {
                //às vezes ele guarda somente o ?r=111199992929292 e daí tenta redirecionar pra um número... 
                //se esse parse passar, então aconteceu isso... precisa zerar:
                lUrlAnterior = "";
            }

            if (lUrlAnterior.ToLower().Contains("login.aspx"))
                lRetorno = "~/Default.aspx";

            if (lUrlAnterior.ToLower().Contains("esqueciminhasenha.aspx"))
                lUrlAnterior = null;

            if (!string.IsNullOrEmpty(lUrlAnterior))
            {
                lRetorno = lUrlAnterior;
            }
            else
            {
                if (!ConfiguracoesValidadas.AplicacaoEmModoDeTeste)
                {
                    if (SessaoClienteLogado.Passo == 4)
                    {
                        lRetorno = HostERaizFormat("MinhaConta/");
                    }
                    else
                    {
                        lRetorno = HostERaizFormat("MinhaConta/Cadastro/MeuCadastro.aspx");
                    }
                }
            }

            return lRetorno;
        }

        private void ValidarExpiracaoDeSenha()
        {
            if (SessaoClienteLogado != null && !SessaoClienteLogado.ExpiracaoDeSenhaJaValidada)
            {
                ReceberEntidadeCadastroRequest<LoginInfo> lRequest;
                ReceberEntidadeCadastroResponse<LoginInfo> lResponse;

                lRequest = new ReceberEntidadeCadastroRequest<LoginInfo>();
                lRequest.EntidadeCadastro = new LoginInfo();

                lRequest.EntidadeCadastro.IdLogin = SessaoClienteLogado.IdLogin;

                lResponse = ServicoPersistenciaCadastro.ReceberEntidadeCadastro<LoginInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.EntidadeCadastro != null)
                    {
                        //TODO: isso aqui é só pra poder testar no banco de produção, porque lá a DLL de segurança ainda não foi atualizada,
                        //então mesmo o usuario do rafa estando com a data correta de atualização da senha no banco, a DLL não preenche o 
                        //objeto, então fica sempre no mínimo...

                        //poooorém, isso precisaria também atualizar a data, porque se for desse jeito pra produção ninguém nunca vai chegar nos 45 dias, porque
                        //todos começam com o datetime null, 

                        if (lResponse.EntidadeCadastro.DtUltimaAlteracaosSenha == DateTime.MinValue)
                            lResponse.EntidadeCadastro.DtUltimaAlteracaosSenha = DateTime.Now;

                        if ((lResponse.EntidadeCadastro.DtUltimaAlteracaosSenha.AddDays(45) < DateTime.Today)
                        && (!this.Request.Url.OriginalString.Contains("AlterarSenha.aspx"))
                           )
                        {
                            this.Response.Redirect(HostERaizFormat("MinhaConta/Cadastro/AlterarSenha.aspx?t=Renovacao"));
                        }
                        else
                        {
                            SessaoClienteLogado.ExpiracaoDeSenhaJaValidada = true;
                        }
                    }
                }
                else
                {
                    gLogger.ErrorFormat("Erro em ValidarExpiracaoDeSenha(Código [{0}]) [{1}]\r\n{2}"
                                        , lRequest.EntidadeCadastro.IdLogin
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);
                }
            }
        }

        private string ResponderEfetuarLogout()
        {
            if (SessaoClienteLogado != null)
            {
                base.RetirarClienteDaSessao();
            }

            return RetornarSucessoAjax("ok");
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            RegistrarRespostasAjax(new string[]{
                                                "EfetuarLogin",
                                                "EfetuarLogout"
                                                },
                    new ResponderAcaoAjaxDelegate[]{
                                                ResponderEfetuarLogin,
                                                ResponderEfetuarLogout
                                                });
        }

        #endregion

    }
}