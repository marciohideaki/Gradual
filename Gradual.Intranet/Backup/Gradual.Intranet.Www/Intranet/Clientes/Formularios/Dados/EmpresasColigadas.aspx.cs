using System;
using System.Collections.Generic;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{
    public partial class EmpresasColigadas : PaginaBaseAutenticada
    {
        #region | Métodos

        private string ResponderSalvar()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteEmpresaColigada lDados;

                SalvarEntidadeCadastroRequest<ClienteControladoraInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDados = JsonConvert.DeserializeObject<TransporteEmpresaColigada>(lObjetoJson);

                    lRequest = new SalvarEntidadeCadastroRequest<ClienteControladoraInfo>();

                    lRequest.EntidadeCadastro = lDados.ToClienteControladoraInfo();

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteControladoraInfo>(lRequest);

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
                RemoverEntidadeCadastroRequest<ClienteControladoraInfo> lRequest;

                RemoverEntidadeCadastroResponse lResponse;

                try
                {
                    ClienteControladoraInfo lClitelInfo = new ClienteControladoraInfo();
                    lClitelInfo.IdClienteControladora = int.Parse(lID);
                    lRequest = new RemoverEntidadeCadastroRequest<ClienteControladoraInfo>()
                    {
                        EntidadeCadastro = lClitelInfo,
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        IdUsuarioLogado = base.UsuarioLogado.Id
                    };
                    try
                    {
                        lResponse = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteControladoraInfo>(lRequest);

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
            ConsultarEntidadeCadastroRequest<ClienteControladoraInfo> lRequest;
            ConsultarEntidadeCadastroResponse<ClienteControladoraInfo> lResponse;

            bool lExcluir = true;

            ClienteControladoraInfo lDados = new ClienteControladoraInfo(Request["Id"]);

            lRequest = new ConsultarEntidadeCadastroRequest<ClienteControladoraInfo>()
            {
                EntidadeCadastro = lDados,
                DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                IdUsuarioLogado = base.UsuarioLogado.Id
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteControladoraInfo>(lRequest);

            btnSalvar.Visible = UsuarioPode("Salvar", "3593CB02-1772-4A6C-9893-BC90A7137B0E");

            NovaControladora.Visible = UsuarioPode("Incluir", "4239f749-5c0c-4fd0-abdd-281d94a11744", "3593CB02-1772-4A6C-9893-BC90A7137B0E");

            lExcluir = UsuarioPode("Excluir", "EF3F0430-C176-4B8F-8E2C-BE6B43178B92");

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado.Count == 0 && NovaControladora.Visible)
                    btnSalvar.Visible = true;

                IEnumerable<TransporteEmpresaColigada> lLista = from ClienteControladoraInfo t
                                                                in lResponse.Resultado
                                                                select new TransporteEmpresaColigada(t);

                hidClientes_EmpresasColigadas_ListaJson.Value = JsonConvert.SerializeObject(lLista);
            }
            //else
            //{
            //    RetornarErroAjax("Erro ao consultar os Diretores do cliente", lResponse.DescricaoResposta);
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
                                                , "Excluir"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderSalvar
                                                , ResponderCarregarHtmlComDados 
                                                , ResponderExcluir });
        }

        #endregion
    }
}