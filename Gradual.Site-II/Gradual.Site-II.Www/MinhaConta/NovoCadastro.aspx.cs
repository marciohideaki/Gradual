using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.OMS.Library;
using Newtonsoft.Json;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Email.Lib;
using System.IO;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Seguranca.Lib;
using System.Globalization;
using Gradual.Servico.FichaCadastral.Lib;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;

namespace Gradual.Site.Www
{
    public partial class NovoCadastro : PaginaBase
    {
        #region Propriedades
        
        public bool ModoDeTeste
        {
            get
            {
                return ConfiguracoesValidadas.AplicacaoEmModoDeTeste;
            }
        }

        #endregion

        #region Métodos Private

        private object ListaComSelecione(List<SinacorListaInfo> pLista)
        {
            if (pLista[0].Id != "")
            {
                pLista.Insert(0, new SinacorListaInfo() { Id="", Value ="[SELECIONE]" });
            }

            return pLista;
        }

        private void CarregarAssessorCadastro()
        {
            string lCodigoAcessor = Request["a"];

            Session["CodigoAssessorCadastro"] = 18;
            Session["NomeAssessorCadastro"] = "0018 - 18 - PA - PEDRO PAULO - PF - SP";

            if (!string.IsNullOrEmpty(lCodigoAcessor))
            {
                foreach (SinacorListaInfo lAcessor in DadosDeAplicacao.Assessores)
                {
                    if (lAcessor.Id == lCodigoAcessor)
                    {
                        Session["CodigoAssessorCadastro"] = lAcessor.Id;
                        Session["NomeAssessorCadastro"] = lAcessor.Value;

                        break;
                    }
                }
            }

            //txtCadastro_PFPasso1_Assessor.Value = Session["CodigoAssessorCadastro"].ToString();
            //lblCadastro_PFPasso1_AssessorInicial.Text = Session["NomeAssessorCadastro"].ToString();
        }

        private string CarregarDados()
        {
            CarregarAssessorCadastro();

            try
            {
                RodarJavascriptOnLoad("GradSite_Cadastro_Iniciar();");
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em Cadastro_PF_Passo2.aspx > CarregarDados(): [{0}]\r\n{1}"
                                    , ex.Message
                                    , ex.StackTrace);
            }

            return string.Empty;
        }
        
        private MensagemResponseStatusEnum EnviarEmailCadastralParaCliente(TransporteCadastro pDados)
        {
            //if (ConfiguracoesValidadas.AplicacaoEmModoDeTeste)
            //    return MensagemResponseStatusEnum.OK;

            string lCorpoEmail = string.Empty;
            string lRemetente = ConfiguracoesValidadas.Email_Atendimento;

            Dictionary<string, string> lVariaveis = new Dictionary<string, string>();

            lVariaveis.Add("###NOME###",        pDados.NomeCompleto);
            lVariaveis.Add("###LOGIN###",       pDados.Email);
            lVariaveis.Add("###SENHA###",       pDados.Senha);
            lVariaveis.Add("###ASSINATURA###",  pDados.AssEletronica);

            lCorpoEmail = "CadastroPasso1Completo_Padrao.html";

            return base.EnviarEmail(pDados.Email, "Bem-vindo à Gradual", lCorpoEmail, lVariaveis, eTipoEmailDisparo.Todos);
        }

        private MensagemResponseStatusEnum EnviarEmailMaior65(TransporteCadastro pDados)
        {
            //if (ConfiguracoesValidadas.AplicacaoEmModoDeTeste)
            //    return MensagemResponseStatusEnum.OK;

            string lCorpoEmail = string.Empty;
            string lRemetente = ConfiguracoesValidadas.Email_Compliance;

            Dictionary<string, string> lVariaveis = new Dictionary<string, string>();

            lVariaveis.Add("###NOME###", pDados.NomeCompleto);
            lVariaveis.Add("###CPF###",  pDados.CPF);

            lCorpoEmail = "CadastroClienteMaior65.htm";

            return base.EnviarEmail(lRemetente, "Novo cliente maior de 65 anos", lCorpoEmail, lVariaveis, eTipoEmailDisparo.Todos);
        }

        private MensagemResponseStatusEnum EnviarEmailMenor18(TransporteCadastro pDados)
        {
            //if (ConfiguracoesValidadas.AplicacaoEmModoDeTeste)
            //    return MensagemResponseStatusEnum.OK;

            string lCorpoEmail = string.Empty;
            string lRemetente = ConfiguracoesValidadas.Email_Compliance;

            Dictionary<string, string> lVariaveis = new Dictionary<string, string>();

            lVariaveis.Add("###NOME###", pDados.NomeCompleto);
            lVariaveis.Add("###CPF###",  pDados.CPF);

            lCorpoEmail = "CadastroClienteMenor18.htm";

            return base.EnviarEmail(lRemetente, "Novo cliente menor de 18 anos", lCorpoEmail, lVariaveis, eTipoEmailDisparo.Todos);
        }

        private string SalvarPasso1DoCliente(TransporteCadastro pDados)
        {
            string lRetorno;

            SalvarEntidadeCadastroRequest<Passo1Info> lRequest = new SalvarEntidadeCadastroRequest<Passo1Info>();
            SalvarEntidadeCadastroResponse lResponse;

            lRequest.EntidadeCadastro = pDados.ToPasso1Info();

            if (!pDados.CodigoTipoOperacaoCliente.Equals("3"))
            {
                lRequest.EntidadeCadastro.IdAssessorInicial = Convert.ToInt32(Session["CodigoAssessorCadastro"]);
            }
            else
            {
                lRequest.EntidadeCadastro.IdAssessorInicial = 602;
            }

            lResponse = base.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<Passo1Info>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                MensagemResponseStatusEnum lResponseEnvioEmail = this.EnviarEmailCadastralParaCliente(pDados);

                SessaoClienteLogado = new TransporteSessaoClienteLogado();

                SessaoClienteLogado.Senha = Criptografia.CalculateMD5Hash(lRequest.EntidadeCadastro.CdSenha);
                SessaoClienteLogado.Email = lRequest.EntidadeCadastro.DsEmail;

                if (pDados.Idade < 18)
                    EnviarEmailMenor18(pDados);

                if (pDados.Idade > 65)
                    EnviarEmailMaior65(pDados);

                gLogger.InfoFormat("Cadastro Passo 1: Carregando cliente em sessão [{0}]", SessaoClienteLogado.Email);

                base.CarregarClienteEmSessao(lResponse.DescricaoResposta);

                gLogger.InfoFormat("Cadastro Passo 1: Buscando código de sessão para [{0}]", SessaoClienteLogado.Email);

                base.BuscarCodigoDeSessaoParaUsuarioLogado();

                gLogger.InfoFormat("Cadastro Passo 1: Dados salvos com sucesso para [{0}]", SessaoClienteLogado.Email);

                lRetorno = base.RetornarSucessoAjax("redirecionar_novo");
            }
            else
            {
                if (lResponse.DescricaoResposta.ToLower().Contains("e-mail já cadastrado"))
                {
                    lRetorno = base.RetornarSucessoAjax("erro: email já cadastrado");
                }
                else if (lResponse.DescricaoResposta.ToLower().Contains("cpf/cnpj já cadastrado"))
                {
                    lRetorno = base.RetornarSucessoAjax("erro: cpf já cadastrado");
                }
                else
                {
                    lRetorno = base.RetornarErroAjax("Erro de retorno do serviço em ResponderSalvarDados():\r\n{0}\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta);
                }
            }

            return lRetorno;
        }

        private string ResponderSalvarPasso1()
        {
            string lRetorno;

            lRetorno = RetornarSucessoAjax("redirecionar_novo");

            try
            {
                TransporteCadastro lDados = JsonConvert.DeserializeObject<TransporteCadastro>(Request["DadosDeCadastro"]);

                if (SessaoClienteLogado == null)
                {
                    lRetorno = SalvarPasso1DoCliente(lDados);
                }
                else
                {
                    throw new Exception("Usuário logado não pode fazer novo cadastro.");
                }
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax("Erro em ResponderSalvarPasso1()", ex);
            }

            return lRetorno;
        }

        private string ResponderBuscarAssessor()
        {
            string lRetorno = RetornarSucessoAjax("NAO_ENCONTRADO");

            string lAssessor = Request["Assessor"];

            string lNome = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.Assessor, lAssessor);

            if (!string.IsNullOrEmpty(lNome))
            {
                Session["CodigoAssessorCadastro"] = lAssessor;

                lRetorno = RetornarSucessoAjax(lNome);
            }

            return lRetorno;
        }

        private string ResponderTestarEnvioDeEmailCadastral()
        {
            TransporteCadastro lDados = new TransporteCadastro();

            lDados.NomeCompleto = "Nome do Cliente Aqui";
            lDados.Email = "lleal@gradualinvestimentos.com.br";
            lDados.Senha = "123456789";
            lDados.AssEletronica = "987654321";

            object lResposta = EnviarEmailCadastralParaCliente(lDados);

            return RetornarSucessoAjax(lResposta, "resultado");
        }

        #endregion

        #region Event Handlers

        protected new void Page_Init(object sender, EventArgs e)
        {
            this.PaginaMaster.BreadCrumbVisible = true;

            this.PaginaMaster.Crumb1Text = "Abra Sua Conta";
            this.PaginaMaster.Crumb2Text = "Dados Cadastrais";
            this.PaginaMaster.Crumb3Text = "Meus Dados";

            this.PaginaMaster.Crumb3Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SessaoClienteLogado != null)
            {
                Response.Redirect("Cadastro/MeuCadastro.aspx");
            }

            base.RegistrarRespostasAjax(new string[] {   "SalvarPasso1"
                                                       , "BuscarAssessor"
                                                       , "TestarEnvioDeEmailCadastral"
                                                       , CONST_FUNCAO_CASO_NAO_HAJA_ACTION 
                                                     }
                   , new ResponderAcaoAjaxDelegate[] {   ResponderSalvarPasso1
                                                       , ResponderBuscarAssessor
                                                       , ResponderTestarEnvioDeEmailCadastral
                                                       , CarregarDados
                                                     } );
        }

        #endregion
    }
}