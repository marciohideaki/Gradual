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
    public partial class Diretores : PaginaBaseAutenticada
    {
        #region | Métodos

        private string ResponderSalvar()
        {
            string lRetorno = "";

            string lObjetoJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteDiretor lDados;

                SalvarEntidadeCadastroRequest<ClienteDiretorInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDados = JsonConvert.DeserializeObject<TransporteDiretor>(lObjetoJson);

                    lRequest = new SalvarEntidadeCadastroRequest<ClienteDiretorInfo>();

                    lRequest.EntidadeCadastro = lDados.ToClienteDiretorInfo();

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteDiretorInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.DescricaoResposta), "Dados alterados com sucesso");

                            if (lDados.Id > 0)         //--> Registrando o Log.
                                base.RegistrarLogAlteracao(new Gradual.Intranet.Contratos.Dados.Cadastro.LogIntranetInfo() { IdLoginClienteAfetado = lDados.ParentId });
                            else
                                base.RegistrarLogInclusao(new Gradual.Intranet.Contratos.Dados.Cadastro.LogIntranetInfo() { IdLoginClienteAfetado = lDados.ParentId });
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
                RemoverEntidadeCadastroRequest<ClienteDiretorInfo> lRequest;

                RemoverEntidadeCadastroResponse lResponse;

                try
                {
                    ClienteDiretorInfo lClitelInfo = new ClienteDiretorInfo();

                    lClitelInfo.IdClienteDiretor = int.Parse(lID);

                    lRequest = new RemoverEntidadeCadastroRequest<ClienteDiretorInfo>()
                    {
                        EntidadeCadastro = lClitelInfo,
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        IdUsuarioLogado = base.UsuarioLogado.Id
                    };

                    try
                    {
                        lResponse = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteDiretorInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax("Dados Excluidos com sucesso", new object[] { });
                            base.RegistrarLogExclusao();
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
            ConsultarEntidadeCadastroRequest<ClienteDiretorInfo> lRequest;
            ConsultarEntidadeCadastroResponse<ClienteDiretorInfo> lResponse;

            bool lExcluir = true;

            ClienteDiretorInfo lDados = new ClienteDiretorInfo(Request["Id"]);

            lRequest = new ConsultarEntidadeCadastroRequest<ClienteDiretorInfo>()
            {
                EntidadeCadastro = lDados,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteDiretorInfo>(lRequest);

            btnSalvar.Visible = UsuarioPode("Salvar", "923DF791-18B0-4761-8276-10D99FBA71CE");

            NovoDiretor.Visible = UsuarioPode("Incluir", "4239f749-5c0c-4fd0-abdd-281d94a11744", "923DF791-18B0-4761-8276-10D99FBA71CE");

            lExcluir = UsuarioPode("Excluir", "1898E735-06A4-445F-A6FC-C8E4635832FF");


            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado.Count == 0 && NovoDiretor.Visible)
                {
                    btnSalvar.Visible = true;
                }

                IEnumerable<TransporteDiretor> lLista = from ClienteDiretorInfo t
                                                           in lResponse.Resultado
                                                        select new TransporteDiretor(t, lExcluir);

                hidClientes_Diretores_ListaJson.Value = JsonConvert.SerializeObject(lLista);
            }
            else
            {
                //RetornarErroAjax("Erro ao consultar os Diretores do cliente", lResponse.DescricaoResposta);
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
                                                , ResponderExcluir });

            if (!Page.IsPostBack)
            {
                this.PopularControleComListaDoSinacor(eInformacao.Estado, rptClientes_Diretores_UfOrgaoEmissor);

                this.PopularControleComListaDoSinacor(eInformacao.OrgaoEmissor, rptClientes_Diretores_OrgaoEmissor);
            }
        }

        #endregion
    }
}
