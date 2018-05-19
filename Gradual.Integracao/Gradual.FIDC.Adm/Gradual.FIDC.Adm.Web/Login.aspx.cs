using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.FIDC.Adm.Web
{
    public partial class Login : PaginaBase
    {

        #region Métodos Públicos

        private string ResponderLogout()
        {

            IServicoSeguranca lServicoSeguranca = this.InstanciarServico<IServicoSeguranca>();

            MensagemResponseBase lResponse = lServicoSeguranca.EfetuarLogOut(new MensagemRequestBase() { CodigoSessao = this.CodigoSessao });

            DisporServico(lServicoSeguranca);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                Session.Clear();

                ///RedirecionarPara("../Login.aspx");
                //Response.Redirect()
                return CONST_RESPOSTA_JA_ENVIADA_PELA_FUNCAO;
            }
            else
            {
                return RetornarErroAjax(lResponse.DescricaoResposta);
            }
        }
        #endregion

        #region Event Handlers
        public void btnAutenticar_Click(object sender, EventArgs args)
        {
            string lUsuario, lSenha;

            lUsuario = txtLogin.Text;
            lSenha = txtSenha.Text;

            string lRetorno = string.Empty;

            AutenticarUsuarioRequest lRequestAutenticacao;
            AutenticarUsuarioResponse lResponseAutenticacao;

            ReceberSessaoRequest lRequestSessao;
            ReceberSessaoResponse lResponseSessao;

            IServicoSeguranca lServicoSeguranca = this.InstanciarServico<IServicoSeguranca>();

            lRequestAutenticacao = new AutenticarUsuarioRequest();

            lRequestAutenticacao.Email = lUsuario;
            lRequestAutenticacao.Senha = Criptografia.CalculateMD5Hash(lSenha);
            lRequestAutenticacao.IP = Request.ServerVariables["REMOTE_ADDR"];
            lRequestAutenticacao.CodigoSistemaCliente = "InvXX";

            lResponseAutenticacao = lServicoSeguranca.AutenticarUsuario(lRequestAutenticacao);

            if (lResponseAutenticacao.StatusResposta != Gradual.OMS.Library.MensagemResponseStatusEnum.OK)
            {
                //lRetorno = RetornarErroAjax(lResponseAutenticacao.DescricaoResposta);

                Response.Write("<script LANGUAGE='JavaScript' >alert('" + lResponseAutenticacao.DescricaoResposta + "')</script>");

                return;
            }

            lRequestSessao = new ReceberSessaoRequest();

            lRequestSessao.CodigoSessaoARetornar = lResponseAutenticacao.Sessao.CodigoSessao;

            lRequestSessao.CodigoSessao = lResponseAutenticacao.Sessao.CodigoSessao;
            
            Session["CodigoSessao"] = lResponseAutenticacao.Sessao.CodigoSessao;

            lResponseSessao = lServicoSeguranca.ReceberSessao(lRequestSessao);

            base.UsuarioLogado = new Usuario()
            {
                CodigoDaSessao = lResponseAutenticacao.Sessao.CodigoSessao
                ,
                IdDoUsuario = lResponseSessao.Usuario.Complementos.ReceberItem<ContextoOMSInfo>().CodigoCBLC
                ,
                Nome = lResponseSessao.Usuario.Nome
                ,
                TipoAcesso = (TipoAcesso)Enum.Parse(typeof(TipoAcesso), lResponseSessao.Usuario.CodigoTipoAcesso.ToString())
                ,
                CodAssessor = lResponseSessao.Usuario.CodigoAssessor
                ,
                IdLogin = int.Parse(lResponseSessao.Sessao.CodigoUsuario)
            };

            base.UsuarioLogado.IdDoUsuario = lResponseSessao.Sessao.CodigoUsuario;

            base.UsuarioLogado.CodBovespa = base.UsuarioLogado.IdDoUsuario;

            if (this.UsuarioLogado.CodBovespaTipoInt == 31940)
            {
                this.UsuarioLogado.CodBmf = 31217;
            }

            DisporServico(lServicoSeguranca);

            Response.Redirect("default.aspx?guid=" + lResponseSessao.Sessao.CodigoSessao);


        }

        public string ResponderEfetuarLogin()
        {
            return string.Empty;
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            RegistrarRespostasAjax(new string[] { 
                                                "EfetuarLogin",
                                                "Logout"
                                                },
            new ResponderAcaoAjaxDelegate[] { 
                                                ResponderEfetuarLogin ,
                                                ResponderLogout
                                                });
        }
        #endregion
    }
}