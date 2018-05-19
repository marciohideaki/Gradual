using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{
    public partial class RepresentantesLegais : PaginaBaseAutenticada
    {
        #region | Metodos

        private string ResponderSalvar()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteProcuradores lDados;

                SalvarEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDados = JsonConvert.DeserializeObject<TransporteProcuradores>(lObjetoJson);

                    lRequest = new SalvarEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo>();

                    lRequest.EntidadeCadastro = lDados.ToClienteRepresentanteInfo();

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteProcuradorRepresentanteInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.DescricaoResposta), "Dados alterados com sucesso");

                            if (lDados.Id > 0)         //--> Registrando o Log.
                                base.RegistrarLogAlteracao(new Gradual.Intranet.Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = lDados.ParentId });
                            else
                                base.RegistrarLogInclusao(new Gradual.Intranet.Contratos.Dados.Cadastro.LogIntranetInfo() { IdClienteAfetado = lDados.ParentId });
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
        
        private string ResponderExcluir()
        {
            string lRetorno = "";

            string lID = Request["Id"];

            if (!string.IsNullOrEmpty(lID))
            {
                RemoverEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo> lRequest;

                RemoverEntidadeCadastroResponse lResponse;

                try
                {
                    ClienteProcuradorRepresentanteInfo lProcurador = new ClienteProcuradorRepresentanteInfo();
                    lProcurador.IdProcuradorRepresentante = int.Parse(lID);
                    lRequest = new RemoverEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo>()
                    {
                        EntidadeCadastro = lProcurador,
                         IdUsuarioLogado = base.UsuarioLogado.Id,
                          DescricaoUsuarioLogado = base.UsuarioLogado.Nome
                    };
                    try
                    {
                        lResponse = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteProcuradorRepresentanteInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax("Dados Excluidos com sucesso", new object[] { });

                            base.RegistrarLogExclusao(); //--> Controle de log.
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                        }
                    }
                    catch (Exception exEnvioRequest)
                    {
                        lRetorno = RetornarErroAjax("Erro durante o envio do request para excluir os dados", exEnvioRequest);
                    }
                }
                catch (Exception exConversao)
                {
                    lRetorno = RetornarErroAjax("Erro ao converter os dados", exConversao);
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para excluir");
            }

            return lRetorno;
        }
        
        private string ResponderCarregarHtmlComDados()
        {
            ConsultarEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo> lRequest;
            ConsultarEntidadeCadastroResponse<ClienteProcuradorRepresentanteInfo> lResponse;

            bool lExcluir = true;

            ClienteProcuradorRepresentanteInfo lDados = new ClienteProcuradorRepresentanteInfo()
            {
                IdCliente = int.Parse(Request["Id"])
            };

            lRequest = new ConsultarEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo>()
            {
                EntidadeCadastro = lDados,
                 IdUsuarioLogado = base.UsuarioLogado.Id,
                  DescricaoUsuarioLogado = base.UsuarioLogado.Nome
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteProcuradorRepresentanteInfo>(lRequest);

            btnSalvar.Visible = UsuarioPode("Salvar", "675B2420-F136-4512-AE7F-BF5639F97CD2");

            NovoRepresentante.Visible = UsuarioPode("Incluir", "4239f749-5c0c-4fd0-abdd-281d94a11744", "675B2420-F136-4512-AE7F-BF5639F97CD2");

            lExcluir = UsuarioPode("Excluir", "1161D23B-7038-421c-A9AE-C17248E50269");

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado.Count == 0 && NovoRepresentante.Visible)
                    btnSalvar.Visible = true;

                IEnumerable<TransporteProcuradores> lLista = from ClienteProcuradorRepresentanteInfo t
                                                             in lResponse.Resultado
                                                             select new TransporteProcuradores(t, lExcluir);

                hidClientes_Representantes_ListaJson.Value = JsonConvert.SerializeObject(lLista);
            }
            //else
            //{
            //    RetornarErroAjax("Erro ao consultar os telefones do cliente", lResponse.DescricaoResposta);
            //}

            //hidDadosCompletos_PF.Value = JsonConvert.SerializeObject(lClientePf);

            return string.Empty;    //só para obedecer assinatura
        }
        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { 
                                                    "Salvar"
                                                  , "CarregarHtmlComDados"
                                                  , "Excluir"
                                                },
                new ResponderAcaoAjaxDelegate[] { 
                                                    ResponderSalvar
                                                  , ResponderCarregarHtmlComDados 
                                                  , ResponderExcluir});

            if (!Page.IsPostBack)
            {
                this.PopularControleComListaDoSinacor(eInformacao.Estado,        rptClientes_Representantes_UfOrgaoEmissor);

                this.PopularControleComListaDoSinacor(eInformacao.OrgaoEmissor,  rptClientes_Representantes_OrgaoEmissor);

                this.PopularControleComListaDoSinacor(eInformacao.TipoDocumento, rptClientes_Representantes_TipoDocumento);

                this.PopularControleComListaDoSinacor(eInformacao.SituacaoLegalRepresentante, rptClientes_Representantes_TipoSituacaoLegal);

            }
        }
        #endregion
    }
}
