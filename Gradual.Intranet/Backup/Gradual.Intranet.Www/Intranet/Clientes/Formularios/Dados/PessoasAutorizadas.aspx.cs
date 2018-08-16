using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{
    public partial class PessoasAutorizadas : PaginaBaseAutenticada
    {
        #region | Metodos

        private string ResponderSalvar()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteEmitente lDados;

                SalvarEntidadeCadastroRequest<ClienteEmitenteInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDados = JsonConvert.DeserializeObject<TransporteEmitente>(lObjetoJson);

                    lRequest = new SalvarEntidadeCadastroRequest<ClienteEmitenteInfo>();

                    lRequest.EntidadeCadastro = lDados.ToClienteEmitente();

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteEmitenteInfo>(lRequest);

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
                RemoverEntidadeCadastroRequest<ClienteEmitenteInfo> lRequest;

                RemoverEntidadeCadastroResponse lResponse;

                try
                {
                    ClienteEmitenteInfo lEmitente = new ClienteEmitenteInfo();
                    lEmitente.IdPessoaAutorizada = int.Parse(lID);
                    lRequest = new RemoverEntidadeCadastroRequest<ClienteEmitenteInfo>()
                    {
                        EntidadeCadastro = lEmitente,
                         DescricaoUsuarioLogado= base.UsuarioLogado.Nome,
                          IdUsuarioLogado=base.UsuarioLogado.Id
                    };
                    try
                    {
                        lResponse = ServicoPersistenciaCadastro.RemoverEntidadeCadastro<ClienteEmitenteInfo>(lRequest);

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
            ConsultarEntidadeCadastroRequest<ClienteEmitenteInfo> lRequest;
            ConsultarEntidadeCadastroResponse<ClienteEmitenteInfo> lResponse;

            bool lExcluir = true;

            ClienteEmitenteInfo lDados = new ClienteEmitenteInfo()
            {
                IdCliente = int.Parse(Request["Id"])
            };

            lRequest = new ConsultarEntidadeCadastroRequest<ClienteEmitenteInfo>()
            {
                EntidadeCadastro = lDados,
                 DescricaoUsuarioLogado=base.UsuarioLogado.Nome,
                  IdUsuarioLogado= base.UsuarioLogado.Id
            };

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteEmitenteInfo>(lRequest);
            
            btnSalvar.Visible = UsuarioPode("Salvar", "45231EA2-CA95-48a7-8006-8A55BCFCA3CD");

            NovaPessoaAutorizada.Visible = UsuarioPode("Incluir", "4239f749-5c0c-4fd0-abdd-281d94a11744", "45231EA2-CA95-48a7-8006-8A55BCFCA3CD");

            lExcluir = UsuarioPode("Excluir", "45231EA2-CA95-48a7-8006-8A55BCFCA3CD");

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado.Count == 0 && NovaPessoaAutorizada.Visible)
                {
                    btnSalvar.Visible = true;
                }
                IEnumerable<TransporteEmitente> lLista = from ClienteEmitenteInfo t
                                                           in lResponse.Resultado
                                                         select new TransporteEmitente(t, lExcluir);

                hidClientes_PessoasAutorizadas_ListaJson.Value = JsonConvert.SerializeObject(lLista);
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
                                                , ResponderExcluir});
        }

        #endregion
    }
}