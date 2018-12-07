using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados;
using Gradual.OMS.Library;
using System.Globalization;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Site.Www.MinhaConta.Cadastro
{
    public partial class EsqueciSenha : PaginaBase
    {
        
        #region Propriedades

        private string Email
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_Email.Value))
                    return string.Empty;

                return this.txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_Email.Value.ToLower();
            }
        }

        private string CpfCnpj
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_CPFCNPJ.Value))
                    return string.Empty;

                return this.txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_CPFCNPJ.Value.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            }
        }

        private DateTime DataNascimentoFundacao
        {
            get
            {
                DateTime lRetorno = default(DateTime);

                string lData = txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_DataNascimentoFundacao.Value;

                DateTime.TryParse(lData, new CultureInfo("pt-BR"), DateTimeStyles.None, out lRetorno);

                return lRetorno;
            }
        }

        #endregion
        
        #region Métodos Private

        private bool EnviarEmailNovaSenha(string pNovaSenha)
        {
            System.Collections.Generic.Dictionary<string, string> lVariaveis = new System.Collections.Generic.Dictionary<string, string>();

            ReceberEntidadeCadastroRequest<VerificaNomeInfo> lRequest = new ReceberEntidadeCadastroRequest<VerificaNomeInfo>();
            ReceberEntidadeCadastroResponse<VerificaNomeInfo> lResponse;

            lRequest.EntidadeCadastro = new VerificaNomeInfo();
            lRequest.EntidadeCadastro.DsEmail = this.Email;
            lRequest.DescricaoUsuarioLogado = string.Concat("Tela Esqueci Senha do Portal - IP: ", base.Request.UserHostAddress);

            lResponse = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<VerificaNomeInfo>(lRequest);

            if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
            {
                gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaCadastro.ReceberEntidadeCadastro<VerificaNomeInfo>(DsEmail: [{0}]) em EsqueciMinhaSenha.aspx > EnviarEmailNovaSenha > [{1}]\r\n{2}"
                                    , lRequest.EntidadeCadastro.DsEmail
                                    , lResponse.StatusResposta
                                    , lResponse.DescricaoResposta);

                throw new Exception(lResponse.DescricaoResposta);
            }

            lVariaveis.Add("###NOME###", lResponse.EntidadeCadastro.DsNome);
            lVariaveis.Add("###SENHA###", String.Concat(new String[] { @"<a href=\", System.Configuration.ConfigurationManager.AppSettings.Get("HostDoSite"), "/MinhaConta/Cadastro/NovaSenha.aspx?", pNovaSenha, "\">clique aqui</a> para criar uma senha nova." }));
            MensagemResponseStatusEnum lRetornoEnvioEmail = base.EnviarEmail(this.Email, "Solicitação de Senha | Gradual Investimentos", "EmailEsqueciSenha.html", lVariaveis, eTipoEmailDisparo.Todos);

            if (lRetornoEnvioEmail == MensagemResponseStatusEnum.OK)
            {
                return true;
            }
            else
            {
                gLogger.ErrorFormat("Resposta com erro do EnviarEmail(DsEmail: [{0}]) em EsqueciMinhaSenha.aspx > EnviarEmailNovaSenha > [{1}]"
                                    , lRequest.EntidadeCadastro.DsEmail
                                    , lRetornoEnvioEmail);

                return false;
            }
        }

        private bool EnviarEmailSolicitacaoNovaSenha(String pHash)
        {
            System.Collections.Generic.Dictionary<string, string> lVariaveis = new System.Collections.Generic.Dictionary<string, string>();

            ReceberEntidadeCadastroRequest<VerificaNomeInfo> lRequest = new ReceberEntidadeCadastroRequest<VerificaNomeInfo>();
            ReceberEntidadeCadastroResponse<VerificaNomeInfo> lResponse;

            lRequest.EntidadeCadastro = new VerificaNomeInfo();
            lRequest.EntidadeCadastro.DsEmail = this.Email;
            lRequest.DescricaoUsuarioLogado = string.Concat("Tela Esqueci Senha do Portal - IP: ", base.Request.UserHostAddress);

            lResponse = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<VerificaNomeInfo>(lRequest);

            if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
            {
                gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaCadastro.ReceberEntidadeCadastro<VerificaNomeInfo>(DsEmail: [{0}]) em EsqueciMinhaSenha.aspx > EnviarEmailNovaSenha > [{1}]\r\n{2}"
                                    , lRequest.EntidadeCadastro.DsEmail
                                    , lResponse.StatusResposta
                                    , lResponse.DescricaoResposta);

                throw new Exception(lResponse.DescricaoResposta);
            }

            lVariaveis.Add("###NOME###", lResponse.EntidadeCadastro.DsNome);
            lVariaveis.Add("###SENHA###", pHash);

            MensagemResponseStatusEnum lRetornoEnvioEmail = base.EnviarEmail(this.Email, "Solicitação de Senha | Gradual Investimentos", "EmailEsqueciSenha.html", lVariaveis, eTipoEmailDisparo.Todos);

            if (lRetornoEnvioEmail == MensagemResponseStatusEnum.OK)
            {
                return true;
            }
            else
            {
                gLogger.ErrorFormat("Resposta com erro do EnviarEmail(DsEmail: [{0}]) em EsqueciMinhaSenha.aspx > EnviarEmailNovaSenha > [{1}]"
                                    , lRequest.EntidadeCadastro.DsEmail
                                    , lRetornoEnvioEmail);

                return false;
            }
        }

        private void LimparCampos()
        { 
            txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_Email.Value = string.Empty;
            txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_CPFCNPJ.Value = string.Empty;
            txtSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_DataNascimentoFundacao.Value = string.Empty;
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected new void Page_Init(object sender, EventArgs e)
        {
            this.PaginaMaster.BreadCrumbVisible = true;

            this.PaginaMaster.Crumb1Text = "Inicial";
            this.PaginaMaster.Crumb1Text = "Minha Conta";
            this.PaginaMaster.Crumb1Text = "Esqueci Minha Senha";
        }

        protected void btnSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_DataNascimentoFundacao_Enviar_Click(object sender, EventArgs e)
        {
            try
            {
                //ReceberEntidadeCadastroRequest<EsqueciSenhaInfo> lRequest = new ReceberEntidadeCadastroRequest<EsqueciSenhaInfo>();
                //ReceberEntidadeCadastroResponse<EsqueciSenhaInfo> lResponse;

                //lRequest.EntidadeCadastro = new EsqueciSenhaInfo();
                //lRequest.EntidadeCadastro.DsEmail = this.Email;
                //lRequest.EntidadeCadastro.DsCpfCnpj = this.CpfCnpj;
                //lRequest.EntidadeCadastro.DtNascimentoFundacao = this.DataNascimentoFundacao;
                //lRequest.DescricaoUsuarioLogado = string.Concat("Tela Esqueci Senha do Portal - IP: ", base.Request.UserHostAddress);

                //lResponse = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<EsqueciSenhaInfo>(lRequest);

                //if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
                //{
                //    gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaCadastro.ReceberEntidadeCadastro<EsqueciSenhaInfo>(DsEmail: [{0}], DsCpfCnpj: [{1}], DtNascimentoFundacao: [{2}], DescricaoUsuarioLogado: [{3}]) em EsqueciMinhaSenha.aspx > btnSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_DataNascimentoFundacao_Enviar_Click > [{4}]\r\n{5}"
                //                        , lRequest.EntidadeCadastro.DsEmail
                //                        , lRequest.EntidadeCadastro.DsCpfCnpj
                //                        , lRequest.EntidadeCadastro.DtNascimentoFundacao
                //                        , lRequest.DescricaoUsuarioLogado
                //                        , lResponse.StatusResposta
                //                        , lResponse.DescricaoResposta);

                //    throw new Exception(lResponse.DescricaoResposta);
                //}

                //if (this.EnviarEmailNovaSenha(lResponse.EntidadeCadastro.CdSenha))
                //{
                //    this.LimparCampos();

                //    //base.RetirarClienteDaSessao();

                //    base.ExibirMensagemJsOnLoad("I", "A nova  foi enviada para seu e-mail cadastrado.");
                //}
                //else
                //{
                //    base.ExibirMensagemJsOnLoad("E", "Atenção! Ocorreu um erro no envio do email; porém sua senha foi alterada. Favor entrar em contato com o atendimento.");
                //}


                ReceberEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.Portal.SolicitacaoNovaSenhaInfo> lRequest = new ReceberEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.Portal.SolicitacaoNovaSenhaInfo>();
                ReceberEntidadeCadastroResponse<Gradual.Intranet.Contratos.Dados.Portal.SolicitacaoNovaSenhaInfo> lResponse;

                lRequest.EntidadeCadastro                       = new Gradual.Intranet.Contratos.Dados.Portal.SolicitacaoNovaSenhaInfo();
                lRequest.EntidadeCadastro.DsEmail               = this.Email;
                lRequest.EntidadeCadastro.DsCpfCnpj             = this.CpfCnpj;
                lRequest.EntidadeCadastro.DtNascimentoFundacao  = this.DataNascimentoFundacao;
                lRequest.DescricaoUsuarioLogado                 = string.Concat("Tela Esqueci Senha do Portal - IP: ", base.Request.UserHostAddress);

                lResponse = base.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<Gradual.Intranet.Contratos.Dados.Portal.SolicitacaoNovaSenhaInfo>(lRequest);
                lResponse.EntidadeCadastro.ValidadeToken = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("PassTokenExpiration"));
                
                if (lResponse.StatusResposta != MensagemResponseStatusEnum.OK)
                {
                    gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaCadastro.ReceberEntidadeCadastro<SolicitacaoNovaSenhaInfo>(DsEmail: [{0}], DsCpfCnpj: [{1}], DtNascimentoFundacao: [{2}], DescricaoUsuarioLogado: [{3}]) em EsqueciMinhaSenha.aspx > btnSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_DataNascimentoFundacao_Enviar_Click > [{4}]\r\n{5}"
                                        , lRequest.EntidadeCadastro.DsEmail
                                        , lRequest.EntidadeCadastro.DsCpfCnpj
                                        , lRequest.EntidadeCadastro.DtNascimentoFundacao
                                        , lRequest.DescricaoUsuarioLogado
                                        , lResponse.StatusResposta
                                        , lResponse.DescricaoResposta);

                    throw new Exception(lResponse.DescricaoResposta);
                }

                if (this.EnviarEmailNovaSenha(GerarHashNovaSenha(lResponse.EntidadeCadastro)))
                {
                    this.LimparCampos();
                    //base.RetirarClienteDaSessao();
                    base.ExibirMensagemJsOnLoad("I", "A nova  foi enviada para seu e-mail cadastrado.");
                }
                else
                {
                    base.ExibirMensagemJsOnLoad("E", "Atenção! Ocorreu um erro no envio do email; porém sua senha foi alterada. Favor entrar em contato com o atendimento.");
                }

            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad("E", ex.Message.Contains("|") ? ex.Message.Split('|')[1] : ex.Message, false);
            }
        }

        private String GerarHashNovaSenha(Gradual.Intranet.Contratos.Dados.Portal.SolicitacaoNovaSenhaInfo pParametro)
        //private String GerarHashNovaSenha(Gradual.Integration.HomeBroker.User pParametro)
        {
            String lRetorno = String.Empty;
            byte[] obj;
            using (System.IO.FileStream fsSource = new System.IO.FileStream(System.Configuration.@ConfigurationManager.AppSettings.Get("PassPublicKey"), System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                bf.Serialize(ms, pParametro);
                
                obj = ms.ToArray();

                byte[] objCrypt = Gradual.Integration.HomeBroker.Crypto.Encrypt(obj, Gradual.Integration.HomeBroker.Crypto.ReadPublicKey(fsSource), true, true);
                obj = objCrypt;
                //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes( System.Text.Encoding.UTF8.GetString(objCrypt) );
                //lRetorno = System.Convert.ToBase64String(plainTextBytes);
                lRetorno = System.Convert.ToBase64String(objCrypt);
            }

            using (System.IO.FileStream fsPvtSource = new System.IO.FileStream(@System.Configuration.ConfigurationManager.AppSettings.Get("PassPrivateKey"), System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                Gradual.Integration.HomeBroker.User lSolicitacao;
                var retorno = Gradual.Integration.HomeBroker.Crypto.DefaultDecrypt(obj, fsPvtSource, "Gradual Investimentos");
            }

            return lRetorno;
        }

        protected void btnSiteGradual_MinhaConta_Cadastro_EsqueciMinhaSenha_DataNascimentoFundacao_Cadastrar_Click(object sender, EventArgs e)
        {
            base.Response.Redirect(HostERaizFormat("MinhaConta/Cadastro/CadastroPF.aspx"));
        }

        #endregion

    }
}