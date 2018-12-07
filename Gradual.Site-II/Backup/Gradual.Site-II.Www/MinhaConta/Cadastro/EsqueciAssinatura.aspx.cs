using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Site.Www.MinhaConta.Cadastro
{
    public partial class EsqueciAssinatura : PaginaBase
    {
        #region Propriedades

        private string Email
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_Email.Value))
                    return string.Empty;

                return this.txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_Email.Value.ToLower();
            }
        }

        private string CpfCnpj
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_CPFCNPJ.Value))
                    return string.Empty;

                return this.txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_CPFCNPJ.Value.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            }
        }

        private DateTime DataNascimentoFundacao
        {
            get
            {
                DateTime lRetorno = default(DateTime);

                DateTime.TryParse(this.txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_DataNascimentoFundacao.Value, out lRetorno);

                return lRetorno;
            }
        }

        #endregion
        
        #region Métodos Private

        private void CadastrarNovaAssinatura()
        {
            string lMensagem = "";

            try
            {
                ReceberEntidadeCadastroRequest<EsqueciAssinaturaEletronicaInfo> lRequest = new ReceberEntidadeCadastroRequest<EsqueciAssinaturaEletronicaInfo>();
                ReceberEntidadeCadastroResponse<EsqueciAssinaturaEletronicaInfo> lResponse;

                lRequest.EntidadeCadastro = new EsqueciAssinaturaEletronicaInfo();

                lRequest.EntidadeCadastro.dsEmail               = this.Email;
                lRequest.EntidadeCadastro.dsCpfCnpj             = this.CpfCnpj;
                lRequest.EntidadeCadastro.dtNascimentoFundacao  = this.DataNascimentoFundacao;
                lRequest.DescricaoUsuarioLogado                 = string.Concat("Tela Esqueci Assinatura do Portal - IP: ", base.Request.UserHostAddress);

                lResponse = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<EsqueciAssinaturaEletronicaInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    Dictionary<string, string> lVariaveis = new Dictionary<string, string>();

                    ReceberEntidadeCadastroRequest<VerificaNomeInfo> lRequestBuscarNome = new ReceberEntidadeCadastroRequest<VerificaNomeInfo>();
                    ReceberEntidadeCadastroResponse<VerificaNomeInfo> lResponseBuscarNome;

                    MensagemResponseStatusEnum lRetornoEnvioEmail;

                    lRequestBuscarNome.EntidadeCadastro         = new VerificaNomeInfo();
                    lRequestBuscarNome.EntidadeCadastro.DsEmail = this.Email;
                    lRequestBuscarNome.DescricaoUsuarioLogado   = string.Concat("Tela Esqueci Assinatura do Portal - IP: ", base.Request.UserHostAddress);

                    lResponseBuscarNome = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<VerificaNomeInfo>(lRequestBuscarNome);

                    if (lResponseBuscarNome.StatusResposta != MensagemResponseStatusEnum.OK)
                    {
                        gLogger.Error(string.Format("Erro em EsqueciMinhaAssinatura.aspx > CadastrarNovaAssinatura(): Resposta com erro do ServicoPersistenciaCadastro.ReceberEntidadeCadastro<VerificaNomeInfo>(pEmail [{0}]) [{1}]\r\n{2}\r\n** A pesar do erro, o sistema tentará enviar o email de nova assinatura sem o nome do cliente **"
                                                    , lRequestBuscarNome.EntidadeCadastro.DsEmail
                                                    , lResponseBuscarNome.StatusResposta
                                                    , lResponse.EntidadeCadastro.cdAssinaturaEletronica));

                        lResponseBuscarNome.EntidadeCadastro.DsNome = "Cliente";
                    }

                    lVariaveis.Add("###NOME###",  lResponseBuscarNome.EntidadeCadastro.DsNome);
                    lVariaveis.Add("###SENHA###", lResponse.EntidadeCadastro.cdAssinaturaEletronica);

                    gLogger.InfoFormat("EsqueciMinhaAssinatura.aspx > CadastrarNovaAssinatura > Enviando email para [{0}] [{1}]; nova assinatura: [{2}]"
                                        , this.Email
                                        ,  lResponseBuscarNome.EntidadeCadastro.DsNome
                                        , lResponse.DescricaoResposta);

                    lRetornoEnvioEmail = base.EnviarEmail(this.Email, "Solicitação de Assinatura | Gradual Investimentos", "EmailEsqueciAssinatura.html", lVariaveis, eTipoEmailDisparo.Todos);

                    if (lRetornoEnvioEmail == MensagemResponseStatusEnum.OK)
                    {
                        this.LimparCampos();

                        base.RetirarClienteDaSessao();

                        base.ExibirMensagemJsOnLoad("I", "A nova assinatura eletrônica foi enviada para seu e-mail cadastrado.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                    }
                    else
                    {
                        lMensagem = string.Format("Erro em EsqueciMinhaAssinatura.aspx > CadastrarNovaAssinatura(): Resposta com erro ao enviar email para [{0}]", this.Email);
                    }
                }
                else
                {
                    if (lResponse.DescricaoResposta.Contains("OS DADOS INFORMADOS ESTÃO INCORRETOS"))
                    {
                        ExibirMensagemJsOnLoad("I", "Não foram encontrados registros com essas informações de email e documentação.");
                    }
                    else
                    {
                        lMensagem = string.Format("Erro em EsqueciMinhaAssinatura.aspx > CadastrarNovaAssinatura(): Resposta com erro do ServicoPersistenciaCadastro.ReceberEntidadeCadastro<EsqueciAssinaturaEletronicaInfo>(pEmail [{0}], pCpfCnpj [{1}], pDataNascFund [{2}]) [{3}]\r\n{4}"
                                                  , lRequest.EntidadeCadastro.dsEmail
                                                  , lRequest.EntidadeCadastro.dsCpfCnpj
                                                  , lRequest.EntidadeCadastro.dtNascimentoFundacao
                                                  , lResponse.StatusResposta
                                                  , lResponse.DescricaoResposta);
                    }
                }
            }
            catch (Exception ex)
            {
                lMensagem = string.Format("Erro em EsqueciMinhaAssinatura.aspx > CadastrarNovaAssinatura(): [{0}]\r\n{1}"
                                            , ex.Message
                                            , ex.StackTrace);
            }

            if (!string.IsNullOrEmpty(lMensagem))
            {
                gLogger.Error(lMensagem);

                base.ExibirMensagemJsOnLoad("E", "Erro ao processar nova assinatura eletrônica", false, lMensagem);
            }
        }

        private void LimparCampos()
        { 
            txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_Email.Value = string.Empty;
            txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_CPFCNPJ.Value = string.Empty;
            txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_DataNascimentoFundacao.Value = string.Empty;
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ValidarSessao();

            RodarJavascriptOnLoad("window.setInterval(MinhaConta_ExecutarFakeKeyPress, 1000);");
        }
        
        protected new void Page_Init(object sender, EventArgs e)
        {
            this.PaginaMaster.BreadCrumbVisible = true;
            
            this.PaginaMaster.Crumb1Text = "Inicial";
            this.PaginaMaster.Crumb1Text = "Minha Conta";
            this.PaginaMaster.Crumb1Text = "Esqueci Minha Assinatura Eletrônica";
        }

        protected void btnSiteGradual_MinhaConta_Cadastro_EsqueciMinhaAssinatura_Enviar_Click(object sender, EventArgs e)
        {
            CadastrarNovaAssinatura();
        }

        #endregion
    }
}