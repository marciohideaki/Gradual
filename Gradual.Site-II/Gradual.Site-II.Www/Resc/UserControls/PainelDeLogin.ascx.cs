using System;
using System.Text;
using Gradual.Generico.Geral;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Portal;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Library;
using System.Diagnostics;
using Gradual.OMS.Seguranca.Lib;


namespace Gradual.Site.Www.Resc.UserControls
{
    public partial class PainelDeLogin : UserControlBase
    {
        #region Propriedades

        public string VersaoDoSite
        {
            get
            {
                return ConfiguracoesValidadas.VersaoDoSite;
            }
        }

        public bool ExibirSemID { get; set; }

        public string IdDoPainel { get { return this.ExibirSemID ? "" : "pnlLogin"; } }
        
        public string SufixoInline { get { return this.ExibirInline ? "_InLine" : ""; } }

        public bool AparecerVisivel { get; set; }

        public string Estilo { get { return this.AparecerVisivel ? "" : "display: none;"; } }

        private bool _ExibirInline;

        public bool ExibirInline
        {
            get
            {
                return _ExibirInline;
            }

            set
            {
                _ExibirInline = value;

                pnlNavegacaoLogin_DadosDoUsuario.Visible = !_ExibirInline;

                //lnkNavegacaoLogin.Visible = !_ExibirInline;
            }
        }

        public string RaizDoSite
        {
            get
            {
                return ConfiguracoesValidadas.RaizDoSite;
            }
        }

        #endregion

        #region Métodos Private

        private void ConfigurarExibicaoUsuario()
        {
            if (!_ExibirInline)
            {
                if (this.PaginaBase.SessaoClienteLogado == null)
                {
                    pnlGradSite_MinhaConta_Cadastro_Login_ClienteLogin.Visible = true;
                    pnlNavegacaoLogin_DadosDoUsuario.Visible = false;
                }
                else
                {
                    pnlGradSite_MinhaConta_Cadastro_Login_ClienteLogin.Visible = false;
                    pnlNavegacaoLogin_DadosDoUsuario.Visible = true;

                    lblUsuario_Nome.Text = SessaoClienteLogado.Nome;
                    lblUsuario_Conta.Text = SessaoClienteLogado.CodigoPrincipal.ToCodigoClienteFormatado();

                    lblUsuario_Acesso.Text = string.Format("{0} às {1}", SessaoClienteLogado.DataDeUltimoLogin.ToString("dd/MM/yyyy"), SessaoClienteLogado.DataDeUltimoLogin.ToString("HH:mm"));

                    lblUsuario_Perfil.Text = string.IsNullOrEmpty(SessaoClienteLogado.PerfilSuitability) ? "n/d" : SessaoClienteLogado.PerfilSuitability;
                }
            }

            /*
            if (ConfiguracoesValidadas.AplicacaoEmModoDeTeste)
            {
                txtGradSite_MinhaConta_Cadastro_Login_Senha.Attributes["class"] = "senha";  //keyboardInput pro tecladinho virtual
                btnGradSite_MinhaConta_Cadastro_Login_EfetuarLogin.Attributes["style"] = "";
            }
            */
        }

        #endregion


        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            string lRedirecionamento = Request["r"];
            string lPound = Request["p"];

            if (!string.IsNullOrEmpty(lRedirecionamento))
            {
                if(!string.IsNullOrEmpty(lPound))
                    lPound = string.Format("#{0}", lPound);

                Session["RedirecionamentoPorFaltaDeLogin"] = string.Format("{0}{1}", lRedirecionamento, lPound);
            }

            this.ConfigurarExibicaoUsuario();

            /*
            try
            {
                this.ValidarExpiracaoDeSenha();
            }
            catch (Exception ex)
            {
                this.PaginaBase.ExibirMensagemJsOnLoad("E", ex.Message);
            }
            */
        }

        protected void btnGradSite_MinhaConta_Cadastro_Login_EsqueciSenha_Click(object sender, EventArgs e)
        {
            Response.Redirect(this.PaginaBase.HostERaizFormat("MinhaConta/Cadastro/EsqueciMinhaSenha.aspx"));
        }

        protected void btnGradSite_MinhaConta_Cadastro_Login_CadastreSe_Click(object sender, EventArgs e)
        {
            Response.Redirect(this.PaginaBase.HostERaizFormat("MinhaConta/Cadastro/MeuCadastro.aspx"));
        }

        protected void btnGradSite_MinhaConta_Cadastro_Login_EfetuarLogin_Click(object sender, EventArgs e)
        {
            try
            {
                this.ConfigurarExibicaoUsuario();
            }
            catch (Exception ex)
            {
                /*
                if(ex.Message.Contains("Erro interno"))
                {
                    this.PaginaBase.ExibirMensagemJsOnLoad("E", ex.Message, false);
                }
                else if (ex.Message.StartsWith("SENHA:"))
                {
                    this.PaginaBase.ExibirMensagemJsOnLoad("E", ex.Message.Substring(6), false);
                }
                else
                {
                    //exception não esperada
                    throw ex;
                }
                */

                throw ex;
            }
        }

        protected void txtGradSite_MinhaConta_Cadastro_Login_Usuario_Logout_Click(object sender, EventArgs e)
        {
            //this.EfetuarLogout();
        }

        protected void lnkNavegacaoNomeUsuario_Click(object sender, EventArgs e)
        {
            //this.EfetuarLogout();
        }

        #endregion

    }
}