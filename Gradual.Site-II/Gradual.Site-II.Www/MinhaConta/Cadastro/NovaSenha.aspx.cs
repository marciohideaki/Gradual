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
    public partial class NovaSenha : PaginaBase
    {
        Gradual.Intranet.Contratos.Dados.Portal.SolicitacaoNovaSenhaInfo gSolicitacao;

        private string GetHash
        {
            get
            {
                String lParametro = Request.RawUrl.Replace("/MinhaConta/Cadastro/NovaSenha.aspx?", String.Empty);
                lParametro = lParametro.Replace("/MinhaConta/Cadastro/NovaSenha.aspx", String.Empty);
                
                if (string.IsNullOrWhiteSpace(lParametro))
                    return string.Empty;

                return lParametro;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            String lAcao = String.Empty;

            if (!String.IsNullOrEmpty(Request.Params["Acao"]))
            {
                lAcao = Request.Params["Acao"];
            }

            //if (!lAcao.Equals("AlterarSenhaDinamica") && !lAcao.Equals("AlterarAssinaturaDinamica"))
            //{
            //    base.ValidarSessao();
            //}
            //else
            //{
            //    if (this.SessaoClienteLogado == null)
            //    {
            //        Session["RedirecionamentoPorFaltaDeLogin"] = Request.Url.PathAndQuery;

            //        this.Response.Redirect(HostERaizFormat("MinhaConta/Login.aspx"));
            //    }
            //}

            base.RegistrarRespostasAjax(new string[] {   "NovaSenha"
                                                     }
                   , new ResponderAcaoAjaxDelegate[] {   GravarNovaSenha
                                                     });


            String lHash = GetHash;
            if (!String.IsNullOrEmpty(lHash))
            {
                //using (System.IO.FileStream fsPvtSource = new System.IO.FileStream(@System.Configuration.ConfigurationManager.AppSettings.Get("PrivateKey"), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                using (System.IO.FileStream fsPvtSource = new System.IO.FileStream(@System.Configuration.ConfigurationManager.AppSettings.Get("PassPrivateKey"), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    Gradual.Intranet.Contratos.Dados.Portal.SolicitacaoNovaSenhaInfo lSolicitacaoNovaSenha;

                    Gradual.Integration.HomeBroker.User lSolicitacao;
                    byte[] obj = System.Convert.FromBase64String(lHash);
                    lSolicitacaoNovaSenha = (Gradual.Intranet.Contratos.Dados.Portal.SolicitacaoNovaSenhaInfo)ByteArrayToObject(Gradual.Integration.HomeBroker.Crypto.DefaultDecrypt(obj, fsPvtSource, "Gradual Investimentos"));

                    if (DateTime.Now > lSolicitacaoNovaSenha.DataHora.AddSeconds(lSolicitacaoNovaSenha.ValidadeToken))
                    {
                        Conteudo.Visible = false;
                        Expirado.Visible = true;
                    }
                    else
                    {
                        Expirado.Visible = false;
                        gSolicitacao = lSolicitacaoNovaSenha;
                        Session["SolicitacaoSenha"] = gSolicitacao;
                    }
                }
            }
            else
            {
                Conteudo.Visible = false;
                Inexistente.Visible = true;
            }
        }

        private void ValidarHistoricoDeSenhaDinamica() { }

        private string GravarNovaSenha()
        {

            string lRetorno = "";

            if (Session["SolicitacaoSenha"] != null)
            {
                gSolicitacao = (Gradual.Intranet.Contratos.Dados.Portal.SolicitacaoNovaSenhaInfo)Session["SolicitacaoSenha"];

                SalvarEntidadeCadastroRequest<AlterarSenhaInfo> lRequest = new SalvarEntidadeCadastroRequest<AlterarSenhaInfo>();
                SalvarEntidadeCadastroRequest<AlterarSenhaDinamicaInfo> lRequestDinamico = new SalvarEntidadeCadastroRequest<AlterarSenhaDinamicaInfo>();
                SalvarEntidadeCadastroResponse lResponse;
                Gradual.Site.DbLib.Mensagens.TipoTecladoResponse lTipoTecladoResponse = null;

                string lSenhaAtual, lSenhaNova, lSenhaNovaC, lTipoTeclado;



                lSenhaAtual = Request["SenhaAtual"];
                lSenhaNova = Request["SenhaNova"];
                lSenhaNovaC = Request["SenhaNovaC"];
                lTipoTeclado = Request["TipoTeclado"];


                if (lSenhaNova == lSenhaNovaC)
                {
                    if (lSenhaNova.Length == 6)
                    {
                        try
                        {
                            ValidarHistoricoDeSenhaDinamica();
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "JA_UTILIZADA")
                            {
                                lRetorno = RetornarSucessoAjax("JA_UTILIZADA");

                                return lRetorno;
                            }
                            else
                            {
                                throw ex;
                            }
                        }

                        lRequestDinamico.EntidadeCadastro = new AlterarSenhaDinamicaInfo();
                        lRequestDinamico.EntidadeCadastro.CdSenhaAntiga = gSolicitacao.CdSenha;
                        lRequestDinamico.EntidadeCadastro.CdSenhaNova = lSenhaNova;
                        lRequestDinamico.EntidadeCadastro.IdLogin = Int32.Parse(gSolicitacao.Login);
                        lRequestDinamico.IdUsuarioLogado = Int32.Parse(gSolicitacao.Login);
                        lRequestDinamico.DescricaoUsuarioLogado = gSolicitacao.DsEmail;

                        //if (lTipoTeclado.Equals(1) || lTipoTeclado.Equals(2))
                        //{
                        //    lRequestDinamico.EntidadeCadastro.SenhaDinamica = new Gradual.OMS.Seguranca.Lib.SenhaInfo(lSenhaAtual);
                        //}


                        //lRequestDinamico.EntidadeCadastro.CdSenhaAntiga = gSolicitacao.CdSenha;
                        lRequestDinamico.EntidadeCadastro.SenhaDinamicaNova = new Gradual.OMS.Seguranca.Lib.SenhaInfo(lSenhaNova);

                        lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<AlterarSenhaDinamicaInfo>(lRequestDinamico);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {

                            Session["ExibirMensagemTrocaSenha"] = false;

                            if (!ConfiguracoesValidadas.AplicacaoEmModoDeTeste)
                            {

                                MensagemResponseStatusEnum lRetornoEnvioEmail = this.EnviarEmailConfirmacaoAlteracao();

                                if (Session["TipoTeclado"] != null)
                                {
                                    lTipoTecladoResponse = (Gradual.Site.DbLib.Mensagens.TipoTecladoResponse)Session["TipoTeclado"];
                                }

                                if (lTipoTecladoResponse.Teclado.Equals(Gradual.Site.DbLib.Mensagens.TipoTeclado.DINAMICO) || lTipoTecladoResponse.Teclado.Equals(Gradual.Site.DbLib.Mensagens.TipoTeclado.DINAMICO_ASSINATURA))
                                {
                                    lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso. <br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                                    try
                                    {
                                        GravarHistoricoSenha();
                                    }
                                    catch (Exception ex)
                                    {
                                        gLogger.ErrorFormat("Erro MeuCadastro.aspx.cs > ResponderAlterarSenhaDinamica(): [{0}]\r\n{1}"
                                        , ex.Message
                                        , ex.StackTrace);
                                    }

                                    base.RetirarClienteDaSessao();
                                }
                                else
                                {
                                    if (lRetornoEnvioEmail == MensagemResponseStatusEnum.OK)
                                    {
                                        //lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso. Você também receberá um email de confirmação de alteração da sua senha.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                                        lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso. Você também receberá um email de confirmação de alteração da sua senha.");
                                    }
                                    else
                                    {
                                        //lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso, entrentanto não foi possível enviar o e-mail de confirmação de alteração de senha. Verifique seu e-mail.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
                                        lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso, entrentanto não foi possível enviar o e-mail de confirmação de alteração de senha. Verifique seu e-mail.");
                                    }
                                }
                            }
                            else
                            {
                                lRetorno = RetornarSucessoAjax("Sua senha foi alterada com sucesso. O email não será enviado porque a aplicação está em teste.");
                            }
                        }
                        else
                        {
                            if (lResponse.DescricaoResposta.Contains("já utilizada anteriormente"))
                            {
                                lRetorno = RetornarSucessoAjax("JA_UTILIZADA");
                            }
                            else if (lResponse.DescricaoResposta.Contains("senha atual não confere"))
                            {
                                lRetorno = RetornarSucessoAjax("SENHA_ERRADA");
                            }
                            else
                            {
                                gLogger.ErrorFormat("Erro em AlterarSenha.aspx > RealizarAlteracaoDeSenha(IdLogin [{0}]) [{1}]\r\n{2}"
                                                    , lRequest.EntidadeCadastro.IdLogin
                                                    , lResponse.StatusResposta
                                                    , lResponse.DescricaoResposta);

                                lRetorno = RetornarSucessoAjax("Erro ao alterar a senha, favor tentar novamente ou entrar em contato com o atendimento.");
                            }
                        }
                    }
                    else
                    {
                        lRetorno = RetornarSucessoAjax("A nova senha deve ter 6 caracteres numéricos.");
                    }
                }
                else
                {
                    lRetorno = RetornarSucessoAjax("A confirmação da senha não confere!");
                }

            }
            else
            {
                lRetorno = RetornarSucessoAjax("Desculpe o transtorno mas no momento não foi possível alterar sua senha.");
            }

            return lRetorno;
        }

        private static Object ByteArrayToObject(byte[] arrBytes)
        {
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binForm = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, System.IO.SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
 
        protected void btnSiteGradual_NovaSenha_Click(object sender, EventArgs e)
        {
            Console.Write("");
        }

        private void GravarHistoricoSenha()
        {
            SalvarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.Portal.HistoricoSenhaInfo> lRequest = new SalvarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.Portal.HistoricoSenhaInfo>();
            SalvarEntidadeCadastroResponse lResponse;

            lRequest.EntidadeCadastro = new Gradual.Intranet.Contratos.Dados.Portal.HistoricoSenhaInfo();

            lRequest.EntidadeCadastro.CdSenha = Request["SenhaNova"];
            lRequest.EntidadeCadastro.IdLogin = base.SessaoClienteLogado.IdLogin;

            lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<Gradual.Intranet.Contratos.Dados.Portal.HistoricoSenhaInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                gLogger.InfoFormat("Histórico de Nova senha para usuário [{0}] gravado com sucesso.", lRequest.EntidadeCadastro.IdLogin);
            }
            else
            {
                gLogger.ErrorFormat("Resposta com erro do ServicoPersistenciaCadastro.SalvarEntidadeCadastro<HistoricoSenhaInfo>(pIdLogin: [{0}]) em AlterarSenha.aspx > GravarHistoricoSenha() > [{1}]\r\n{2}"
                                    , lRequest.EntidadeCadastro.IdLogin
                                    , lResponse.StatusResposta
                                    , lResponse.DescricaoResposta);

                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        private MensagemResponseStatusEnum EnviarEmailConfirmacaoAlteracao()
        {
            Dictionary<string, string> lParametrosDoEmail = new Dictionary<string, string>();

            lParametrosDoEmail.Add("###NOME###", String.Format(" código {0} ", gSolicitacao.Login));
            lParametrosDoEmail.Add("###SENHA###", Request["SenhaNova"]);

            return base.EnviarEmail(gSolicitacao.DsEmail, "Alteração de Senha | Gradual Investimentos", "EmailNovaSenha.html", lParametrosDoEmail, Gradual.Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Todos);
        }
    }

}
