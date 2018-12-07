using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados;
using Gradual.OMS.Library;

namespace Gradual.Site.Www.MinhaConta.Cadastro
{
    public partial class SolicitarAlteracao : PaginaBase
    {
        #region Propriedades

        private char TipoDeAlteracao
        {
            get
            {
                return this.cboCadastro_SolicitarAlteracao_Tipo.Value.DBToChar();
            }
        }

        private string InformacaoAlterada
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.cboCadastro_SolicitarAlteracao_InformacaoAlterada.Value))
                    return this.cboCadastro_SolicitarAlteracao_InformacaoAlterada.Value;

                return string.Empty;
            }
        }

        private string DescricaoDoMotivo
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.txtCadastro_SolicitarAlteracao_DescricaoMotivo.Value))
                    return Server.HtmlDecode(this.txtCadastro_SolicitarAlteracao_DescricaoMotivo.Value);

                return string.Empty;
            }
        }

        #endregion

        #region Métodos

        private void GravarDados()
        {
            SalvarEntidadeCadastroRequest<ClienteAlteracaoInfo> lRequest = new SalvarEntidadeCadastroRequest<ClienteAlteracaoInfo>();
            SalvarEntidadeCadastroResponse lResponse;

            lRequest.IdUsuarioLogado = base.SessaoClienteLogado.IdLogin;
            lRequest.DescricaoUsuarioLogado = base.SessaoClienteLogado.Nome;

            lRequest.EntidadeCadastro = new ClienteAlteracaoInfo();

            lRequest.EntidadeCadastro.CdTipo        = this.TipoDeAlteracao;
            lRequest.EntidadeCadastro.DsDescricao   = this.DescricaoDoMotivo;
            lRequest.EntidadeCadastro.DsInformacao  = this.InformacaoAlterada;
            lRequest.EntidadeCadastro.IdCliente     = base.SessaoClienteLogado.IdCliente.DBToInt32();

            lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteAlteracaoInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                txtCadastro_SolicitarAlteracao_DescricaoMotivo.Value = "";
                cboCadastro_SolicitarAlteracao_Tipo.SelectedIndex = 0;
                cboCadastro_SolicitarAlteracao_InformacaoAlterada.SelectedIndex = 0;

                ExibirMensagemJsOnLoad("I", "Solicitação enviada com sucesso.");
            }
            else
            {
                string lMensagem = string.Format("Resposta do serviço com erro em SolicitarAlteracao.aspx: [{0}]\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta);

                gLogger.ErrorFormat(lMensagem);

                ExibirMensagemJsOnLoad("E", "Erro no envio dos dados.", false, lMensagem);
            }
        }

        #endregion

        
        #region Event Handlers
        
        protected new void Page_Init(object sender, EventArgs e)
        {
            this.PaginaMaster.BreadCrumbVisible = true;

            this.PaginaMaster.Crumb1Text = "Minha Conta";
            this.PaginaMaster.Crumb2Text = "Meu Cadastro";
            this.PaginaMaster.Crumb3Text = "Solicitar Alteração";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidarSessao();

            /*
            base.RegistrarRespostasAjax(new string[] {   CONST_FUNCAO_CASO_NAO_HAJA_ACTION 
                                                     }
                   , new ResponderAcaoAjaxDelegate[] {   CarregarDados
                                                     } );
            */
        }
        
        protected void btnCadastro_SolicitarAlteracao_GravarDados_Click(object sender, EventArgs e)
        {
            GravarDados();
        }

        #endregion
    }
}