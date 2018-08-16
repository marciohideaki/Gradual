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
    public partial class Procuradores : PaginaBaseAutenticada
    {
        #region | Métodos

        private string ResponderSalvar()
        {
            string lRetorno = "";

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

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

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
            string lRetorno = string.Empty;

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
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        IdUsuarioLogado = base.UsuarioLogado.Id
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

            var lDados = new ClienteProcuradorRepresentanteInfo(Request["Id"]);

            lRequest = new ConsultarEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo>()
            {
                EntidadeCadastro = lDados,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteProcuradorRepresentanteInfo>(lRequest);

            btnSalvar.Visible = UsuarioPode("Salvar", "FDE3F8E9-F9E0-4FF8-801F-B83312BA66C6");

            NovoProcurador.Visible = UsuarioPode("Incluir", "4239f749-5c0c-4fd0-abdd-281d94a11744", "FDE3F8E9-F9E0-4FF8-801F-B83312BA66C6");

            lExcluir = UsuarioPode("Excluir", "52A14EA8-3982-43A1-95F3-5EAC62F7018B");

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado.Count == 0 && NovoProcurador.Visible)
                {
                    btnSalvar.Visible = true;
                }
                IEnumerable<TransporteProcuradores> lLista = from ClienteProcuradorRepresentanteInfo t
                                                             in lResponse.Resultado
                                                             select new TransporteProcuradores(t, lExcluir);

                hidClientes_Procuradores_ListaJson.Value = JsonConvert.SerializeObject(lLista);
            }
            else
            {
                //RetornarErroAjax("Erro ao consultar os telefones do cliente", lResponse.DescricaoResposta);
            }

            //hidDadosCompletos_PF.Value = JsonConvert.SerializeObject(lClientePf);

            return string.Empty;    //só para obedecer assinatura
        }
        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "Salvar"
                                                , "CarregarHtmlComDados"
                                                , "Excluir"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderSalvar
                                                , ResponderCarregarHtmlComDados 
                                                , ResponderExcluir
                                                });

            if (!Page.IsPostBack)
            {
                this.PopularControleComListaDoSinacor(eInformacao.Estado, rptClientes_Procuradores_UfOrgaoEmissor);

                this.PopularControleComListaDoSinacor(eInformacao.OrgaoEmissor, rptClientes_Procuradores_OrgaoEmissor);

                this.PopularControleComListaDoSinacor(eInformacao.TipoDocumento, rptClientes_Procuradores_TipoDocumento);
            }
        }

        #endregion
    }
}
