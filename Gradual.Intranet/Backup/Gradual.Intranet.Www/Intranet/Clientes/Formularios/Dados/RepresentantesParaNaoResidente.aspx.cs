using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Servicos.BancoDeDados;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{
    public partial class RepresentantesParaNaoResidente : PaginaBaseAutenticada
    {
        #region | Métodos

        private string ResponderSalvar()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = this.Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteInvestidorNaoResidente lDados;

                SalvarEntidadeCadastroRequest<ClienteInvestidorNaoResidenteInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDados = JsonConvert.DeserializeObject<TransporteInvestidorNaoResidente>(lObjetoJson);

                    lRequest = new SalvarEntidadeCadastroRequest<ClienteInvestidorNaoResidenteInfo>();

                    lRequest.EntidadeCadastro = lDados.ToClienteInvestidorNaoResidenteInfo();

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteInvestidorNaoResidenteInfo>(lRequest);

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

            string lID = this.Request["Id"];

            if (!string.IsNullOrEmpty(lID))
            {
                RemoverEntidadeCadastroRequest<ClienteInvestidorNaoResidenteInfo> lRequest;

                RemoverEntidadeCadastroResponse lResponse;

                try
                {
                    ClienteInvestidorNaoResidenteInfo lClitelInfo = new ClienteInvestidorNaoResidenteInfo();
                    lClitelInfo.IdInvestidorNaoResidente = int.Parse(lID);
                    lRequest = new RemoverEntidadeCadastroRequest<ClienteInvestidorNaoResidenteInfo>()
                    {
                        EntidadeCadastro = lClitelInfo,
                         DescricaoUsuarioLogado= base.UsuarioLogado.Nome,
                          IdUsuarioLogado = base.UsuarioLogado.Id
                    };
                    try
                    {
                        lResponse = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteInvestidorNaoResidenteInfo>(lRequest);

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
            ConsultarEntidadeCadastroRequest<ClienteInvestidorNaoResidenteInfo>  lRequest;
            ConsultarEntidadeCadastroResponse<ClienteInvestidorNaoResidenteInfo> lResponse;

            bool lExcluir = true;

            ClienteInvestidorNaoResidenteInfo lDados = new ClienteInvestidorNaoResidenteInfo(Request["Id"]);

            lRequest = new ConsultarEntidadeCadastroRequest<ClienteInvestidorNaoResidenteInfo>()
            {
                EntidadeCadastro = lDados,
                 DescricaoUsuarioLogado= base.UsuarioLogado.Nome,
                  IdUsuarioLogado = base.UsuarioLogado.Id
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteInvestidorNaoResidenteInfo>(lRequest);

            btnSalvar.Visible = UsuarioPode("Salvar", "F60D51DD-CC20-45A8-9244-6F96ACA50BF2");

            NovoRepresentante.Visible = UsuarioPode("Incluir", "4239f749-5c0c-4fd0-abdd-281d94a11744", "F60D51DD-CC20-45A8-9244-6F96ACA50BF2");

            lExcluir = UsuarioPode("Excluir", "12C2713C-D55B-43FE-B102-5266A3C827AB");

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado.Count == 0 && NovoRepresentante.Visible)
                {
                    btnSalvar.Visible = true;
                }
                IEnumerable<TransporteInvestidorNaoResidente> lLista = from ClienteInvestidorNaoResidenteInfo t
                                                                       in lResponse.Resultado
                                                                       select new TransporteInvestidorNaoResidente(t, lExcluir);

                hidClientes_RepresentantesParaNaoResidente_ListaJson.Value = JsonConvert.SerializeObject(lLista);
            }
            //else
            //{
            //    RetornarErroAjax("Erro ao consultar os Diretores do cliente", lResponse.DescricaoResposta);
            //}

            //hidDadosCompletos_PF.Value = JsonConvert.SerializeObject(lClientePf);

            return string.Empty;    //só para obedecer assinatura
        }

        #endregion

        #region Events
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
                                                  , ResponderExcluir });

            if (!Page.IsPostBack)
            {
                this.PopularControleComListaDoSinacor(  eInformacao.Pais
                                                      , rptClientes_RepresentantesParaNaoResidente_PaisDeOrigem
                                                      , rptClientes_RepresentantesParaNaoResidente_PaisDeOrigem);
            }
        }
        #endregion
    }
}
