using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using log4net;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{
    public partial class Contratos : PaginaBaseAutenticada
    {
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Members
        private string ID_TERMO_ALAVANCAGEM = ConfiguracoesValidadas.IDTermoAlavancagem;
        IEnumerable<TransporteContrato> ListaContratos = null;
        List<int> ListaIdContratos = new List<int>();
        private List<int> _RequestIdContrato = null;
        
        public List<int> RequestIdContrato
        {
            get
            {
                if (_RequestIdContrato == null)
                {
                    string lContratos = Request.Form["Contratos"];

                    if (!string.IsNullOrEmpty(lContratos))
                    {
                        string[] lResp = lContratos.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        //string[] lResposta;

                        _RequestIdContrato = new List<int>();

                        for (int a = 0; a < lResp.Length; a++)
                            _RequestIdContrato.Add(Convert.ToInt32(lResp[a]));
                        
                    }
                }

                return _RequestIdContrato;
            }
        }
        #endregion

        #region Métodos Private

        private string ResponderSalvar()
        {
            string lRetorno = "";

            //string lObjetoJson = Request["ObjetoJson"];
            string lParentId = Request.Form["ParentId"];

            if (!string.IsNullOrEmpty(lParentId))
            {
                SalvarEntidadeCadastroRequest<ClienteContratoInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lRequest = new SalvarEntidadeCadastroRequest<ClienteContratoInfo>()
                        {
                            EntidadeCadastro = new ClienteContratoInfo() 
                            { 
                                IdCliente = lParentId.DBToInt32(),
                                DtAssinatura = DateTime.Now,
                                LstIdContrato = RequestIdContrato
                            }
                            , 
                            IdUsuarioLogado = base.UsuarioLogado.Id, 
                            DescricaoUsuarioLogado = base.UsuarioLogado.Nome
                        };

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteContratoInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            // #416 - Suitability

                            //if ((bool)Session["InseriNovoSuitability_" + lParentId] && 
                            //    RequestIdContrato.Contains(ID_TERMO_ALAVANCAGEM.DBToInt32()))
                            //{
                            //    this.InserirSuitability();
                            //}

                            lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.DescricaoResposta), "Dados alterados com sucesso");

                            base.RegistrarLogInclusao(new Gradual.Intranet.Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = lParentId.DBToInt32() });
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                        }
                    }
                    catch (Exception exEnvioRequest)
                    {
                        lRetorno = RetornarErroAjax("Erro durante o envio do request para salvar os dados", exEnvioRequest);
                    }
                }
                catch (Exception exConversao)
                {
                    lRetorno = RetornarErroAjax("Erro ao converter os dados", exConversao);
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para alterar");
            }

            return lRetorno;
        }

        private void InserirSuitability()
        {
            var lInfo = new ClienteSuitabilityInfo();

            string lParentId = Request.Form["ParentId"];

            lInfo.ds_status                = "Finalizado";
            lInfo.ds_fonte                 = "Intranet-TermoAlavancagem";
            lInfo.dt_realizacao            = DateTime.Now;
            lInfo.IdCliente                = lParentId.DBToInt32(); ;
            lInfo.ds_loginrealizado        = this.UsuarioLogado.Nome;
            lInfo.ds_respostas             = "";
            lInfo.st_preenchidopelocliente = false;
            lInfo.ds_perfil                = "Arrojado";

            var lRequest = new SalvarEntidadeCadastroRequest<ClienteSuitabilityInfo>();

            lRequest.EntidadeCadastro = lInfo;

            lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

            lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

            var lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteSuitabilityInfo>(lRequest);

            if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
            {
                base.RegistrarLogInclusao(new LogIntranetInfo() //--> Registrando o Log
                {
                    IdClienteAfetado = lRequest.EntidadeCadastro.IdCliente,
                    DsObservacao = string.Concat("Suitability do cliente: id_cliente = ", lRequest.EntidadeCadastro.IdCliente)
                });

                gLogger.Error("Suitability salvo com sucesso. Enviando email de alteração de perfil.");
                EnviarEmailComPerfilDoInvestidor(this.Request.Form["Email"], lRequest.EntidadeCadastro.IdCliente, lInfo.ds_perfil);
            }
            else
            {
                gLogger.Error("Erro ao salvar o Suitability.");
            }
        }

        private void EnviarEmailComPerfilDoInvestidor(String pEmailDestino, Nullable<int> pIdCliente, String pPerfil)
        {
            string lNomeArquivoEmail = string.Empty;

            switch (pPerfil)
            {
                case "Arrojado":
                    lNomeArquivoEmail = string.Format("EmailSuitability-Arrojado.html");
                    break;

                case "Conservador":
                    lNomeArquivoEmail = string.Format("EmailSuitability-Conservador.html");
                    break;

                case "Moderado":
                    lNomeArquivoEmail = string.Format("EmailSuitability-Moderado.html");
                    break;
            }

            List<String> lDestinatarios = new List<String>() { pEmailDestino };

            base.EnviarEmailSuitability(pIdCliente, pPerfil, lDestinatarios, "Perfil do Investidor | Confira o seu portfólio recomendado", lNomeArquivoEmail, new Dictionary<string, string>(), Gradual.Intranet.Contratos.Dados.Enumeradores.eTipoEmailDisparo.Compliance, null);


        }

        private string ResponderCarregarHtmlComDados()
        {
            ConsultarEntidadeCadastroRequest<ClienteContratoInfo> lRequest;
            ConsultarEntidadeCadastroResponse<ClienteContratoInfo> lResponse;

            ClienteContratoInfo lDados = new ClienteContratoInfo(Request["Id"]);

            txtClientes_Contratos_ParentId.Value = Request["Id"];

            lRequest = new ConsultarEntidadeCadastroRequest<ClienteContratoInfo>() { 
                EntidadeCadastro = lDados, 
                IdUsuarioLogado = base.UsuarioLogado.Id,
                DescricaoUsuarioLogado=base.UsuarioLogado.Nome
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteContratoInfo>(lRequest);

            btnSalvar.Visible = UsuarioPode("Salvar", "35A7D558-1FC1-43b6-86D9-C2FC95A6B5DA");

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                ListaContratos = from ClienteContratoInfo t in lResponse.Resultado select new TransporteContrato(t);
                
                foreach (var contrato in ListaContratos)
                    ListaIdContratos.Add(contrato.Id);
            }
            else
            {
                //RetornarErroAjax("Erro ao consultar os Diretores do cliente", lResponse.DescricaoResposta);
            }

            return string.Empty;    //só para obedecer assinatura
        }

        private void BindContratos()
        {
            string lParentId = Request.Form["Id"];

            List<ContratoInfo> lLista = this.BuscarListaDoCadastro<ContratoInfo>(new ContratoInfo());
            List<TransporteContrato> lListaContrato = new List<TransporteContrato>();
            TransporteContrato lContrato = null;

            foreach (ContratoInfo contrato in lLista)
            {
                lContrato = new TransporteContrato();
                lContrato.DsContrato = contrato.DsContrato;
                lContrato.IdContrato = contrato.IdContrato.ToString();
                lContrato.Checked = ListaIdContratos.Contains(Convert.ToInt32(contrato.IdContrato)) ? "checked='yes'" : string.Empty;
                lListaContrato.Add(lContrato);
            }

            Session["InseriNovoSuitability_" + lParentId] = ListaIdContratos.Contains(ID_TERMO_ALAVANCAGEM.DBToInt32()) ? false : true;

            rptClientes_Contratos.DataSource = lListaContrato;
            rptClientes_Contratos.DataBind();
        }

        #endregion

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { 
                                                    "Salvar"
                                                  , "CarregarHtmlComDados"
                                                },
                new ResponderAcaoAjaxDelegate[] { 
                                                    ResponderSalvar
                                                  , ResponderCarregarHtmlComDados });
            if (!this.Page.IsPostBack)
                this.BindContratos();

        }
        
        #endregion
    }
}