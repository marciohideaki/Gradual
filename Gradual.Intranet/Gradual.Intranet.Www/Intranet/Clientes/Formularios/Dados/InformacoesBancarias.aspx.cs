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
    public partial class InformacoesBancarias : PaginaBaseAutenticada
    {
        #region | Métodos

        private string ResponderSalvar()
        {
            string lRetorno = "";

            string lObjetoJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteDadosBancarios lDados;

                SalvarEntidadeCadastroRequest<ClienteBancoInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDados = JsonConvert.DeserializeObject<TransporteDadosBancarios>(lObjetoJson);

                    lRequest = new SalvarEntidadeCadastroRequest<ClienteBancoInfo>();

                    lRequest.EntidadeCadastro = lDados.ToClienteBancoInfo();

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteBancoInfo>(lRequest);

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
                RemoverEntidadeCadastroRequest<ClienteBancoInfo> lRequest;

                RemoverEntidadeCadastroResponse lResponse;

                try
                {
                    ClienteBancoInfo lClitelInfo = new ClienteBancoInfo();
                    lClitelInfo.IdBanco = int.Parse(lID);
                    lRequest = new RemoverEntidadeCadastroRequest<ClienteBancoInfo>()
                    {
                        EntidadeCadastro = lClitelInfo,
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        IdUsuarioLogado = base.UsuarioLogado.Id
                    };
                    try
                    {
                        lResponse = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteBancoInfo>(lRequest);

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
            ConsultarEntidadeCadastroRequest<ClienteBancoInfo> lRequest;
            ConsultarEntidadeCadastroResponse<ClienteBancoInfo> lResponse;

            bool lExcluir = true;

            ClienteBancoInfo lDados = new ClienteBancoInfo(Request["Id"]);

            lRequest = new ConsultarEntidadeCadastroRequest<ClienteBancoInfo>()
            {
                EntidadeCadastro = lDados,
                IdUsuarioLogado = base.UsuarioLogado.Id,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteBancoInfo>(lRequest);

            btnSalvar.Visible = UsuarioPode("Salvar", "A2EE4A42-FAB7-4784-B479-C7F224B5A7BB");

            NovaConta.Visible = UsuarioPode("Incluir", "4239f749-5c0c-4fd0-abdd-281d94a11744", "A2EE4A42-FAB7-4784-B479-C7F224B5A7BB");

            lExcluir = UsuarioPode("Excluir", "5D24F0AA-6026-4006-BE18-0CC0C8D19D47");

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado.Count == 0 && NovaConta.Visible)
                    btnSalvar.Visible = true;

                var lListaDeBancos = this.BuscarListaDoSinacor(new SinacorListaInfo(eInformacao.Banco));

                IEnumerable<TransporteDadosBancarios> lLista = (from ClienteBancoInfo t in lResponse.Resultado
                                                                orderby t.CdBanco
                                                                select new TransporteDadosBancarios(t, lExcluir, lListaDeBancos));

                lLista = lLista.OrderByDescending<TransporteDadosBancarios, string>(ban => ban.Banco);

                hidClientes_Conta_ListaJson.Value = JsonConvert.SerializeObject(lLista);
            }
            //else
            //{
            //    RetornarErroAjax("Erro ao consultar os bancos do cliente", lResponse.DescricaoResposta);
            //}

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
                                                , "Excluir" },
            new ResponderAcaoAjaxDelegate[] { ResponderSalvar
                                            , ResponderCarregarHtmlComDados 
                                            , ResponderExcluir });
            if (!Page.IsPostBack)
            {
                this.PopularControleComListaDoSinacor(eInformacao.Banco, rptClientes_Conta_Banco);

                this.PopularControleComListaDoSinacor(eInformacao.TipoConta, rptClientes_Conta_Tipo);
            }
        }

        #endregion
    }
}
