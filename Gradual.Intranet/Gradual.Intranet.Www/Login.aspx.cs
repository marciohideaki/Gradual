using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Intranet.Www
{
    public partial class Login : PaginaBase
    {
        #region | Propriedades

        private string GetEmailAlterarSenha
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["Email"]))
                    return string.Empty;

                return this.Request.Form["Email"];
            }
        }

        #endregion

        #region | Métodos

        private string ResponderLogout()
        {
            string lCodigoSessao = this.CodigoSessao;

            Session.Clear();

            MensagemResponseBase res = ServicoSeguranca.EfetuarLogOut(new MensagemRequestBase() { CodigoSessao = lCodigoSessao });

            if (res.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                RedirecionarPara("Login.aspx");

                return RESPOSTA_JA_ENVIADA_PELA_FUNCAO;
            }
            else
            {
                return RetornarErroAjax(res.DescricaoResposta);
            }
        }

        private string ResponderEfetuarLogin()
        {
            string lRetorno;

            string lUsuario, lSenha;

            lUsuario = Request.Form["Usuario"];
            lSenha   = Request.Form["Senha"];

            if (string.IsNullOrEmpty(lUsuario))
            {
                lRetorno = RetornarErroAjax("Campo 'Usuário' é obrigatório");
            }
            else if (string.IsNullOrEmpty(lSenha))
            {
                lRetorno = RetornarErroAjax("Campo 'Senha' é obrigatório");
            }
            else
            {
                AutenticarUsuarioResponse lResponseAutenticacao = null;
                
                try
                {
                    this.LimparSessionsDoSistema();
                    
                    AutenticarUsuarioRequest lRequestAuth = new AutenticarUsuarioRequest()
                        {
                            Email                = lUsuario, // Admin
                            Senha                = Criptografia.CalculateMD5Hash(lSenha), //123
                            IP                   = Request.ServerVariables["REMOTE_ADDR"],
                            CodigoSistemaCliente = "Intranet"
                        };

                    IServicoSeguranca lServico = this.ServicoSeguranca;
                    
                    lResponseAutenticacao      = lServico.AutenticarUsuario(lRequestAuth);

                    if (lResponseAutenticacao.StatusResposta != MensagemResponseStatusEnum.OK)
                    {
                        return RetornarErroAjax(lResponseAutenticacao.DescricaoResposta);
                    }

                    this.CodigoSessao = lResponseAutenticacao.Sessao.CodigoSessao;
                }
                catch (Exception ex)
                {
                    return RetornarErroAjax(ex.Message);
                }

                var lRetornoSessao = ServicoSeguranca.ReceberSessao(new ReceberSessaoRequest()
                {
                    CodigoSessao          = this.CodigoSessao,
                    CodigoSessaoARetornar = this.CodigoSessao,
                });

                this.Session["Usuario"] = new Usuario()
                {
                    Id              = lRetornoSessao.Usuario.CodigoUsuario.DBToInt32(),
                    Nome            = lRetornoSessao.Usuario.Nome,
                    EmailLogin      = lUsuario,
                    EhAdministrador = lResponseAutenticacao  != null ? lResponseAutenticacao.Sessao.EhSessaoDeAdministrador : false,
                    Perfis          = lRetornoSessao.Usuario.Perfis
                };

                lRetorno = base.RetornarSucessoAjax("Intranet/Default.aspx");
            }

            return lRetorno;
        }

        private void LimparSessionsDoSistema()
        {
            this.Session["CodigoAssessor"] = null;
        }

        public string ResponderEnviarEmailEsqueciMinhaSenha()
        {
            var lResposta = string.Empty;

            var lRetorno = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<EsqueciSenhaInfo>(
                new ReceberEntidadeCadastroRequest<EsqueciSenhaInfo>()
                {
                    EntidadeCadastro = new EsqueciSenhaInfo()
                    {
                        DsEmail = this.GetEmailAlterarSenha,
                        StAlteracaoFuncionario = true,
                    }
                });

            if (MensagemResponseStatusEnum.OK.Equals(lRetorno.StatusResposta)
            && (this.EnviarEmailNovaSenha(lRetorno.EntidadeCadastro.DsEmail, lRetorno.EntidadeCadastro.CdSenha)))
            {
                lResposta = base.RetornarSucessoAjax("");
            }
            else
            {
                lResposta = base.RetornarErroAjax("");
            }

            return lResposta;
        }

        private bool EnviarEmailNovaSenha(string pEmail, string pSenha)
        {
            var lVariaveisEmail = new Dictionary<string, string>();
            lVariaveisEmail.Add("[email]", pEmail.Split('@')[0]);
            lVariaveisEmail.Add("[senha]", pSenha);

            var lRetorno = base.EnviarEmail(pEmail, "Alteração de Senha da Intranet", "CadastroAlteracaoDeSenhaDeFuncionario.htm", lVariaveisEmail, Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos);

            return MensagemResponseStatusEnum.OK.Equals(lRetorno);
        }

        #endregion

        #region | Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[] { "EfetuarLogin"
                                                     , "Logout"
                                                     , "EnviarEmailEsqueciMinhaSenha"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderEfetuarLogin
                                                     , this.ResponderLogout
                                                     , this.ResponderEnviarEmailEsqueciMinhaSenha
                                                     });
        }

        #endregion
    }
}
