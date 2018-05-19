using System;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{
    public partial class InformacoesPatrimoniais : PaginaBaseAutenticada
    {
        #region | Métodos

        private string ResponderSalvar()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = this.Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteInformacoesPatrimoniais lDados;

                SalvarEntidadeCadastroRequest<ClienteSituacaoFinanceiraPatrimonialInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDados = JsonConvert.DeserializeObject<TransporteInformacoesPatrimoniais>(lObjetoJson);

                    lRequest = new SalvarEntidadeCadastroRequest<ClienteSituacaoFinanceiraPatrimonialInfo>();

                    lRequest.EntidadeCadastro = lDados.ToClienteSituacaoFinanceiraPatrimonialInfo();

                    lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;

                    lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ClienteSituacaoFinanceiraPatrimonialInfo>(lRequest);

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
                        lRetorno = RetornarErroAjax("Erro durante o envio do request para alterar o ClienteInfo", exEnvioRequest);
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

        private string ResponderCarregarHtmlComDados()
        {
            ClienteSituacaoFinanceiraPatrimonialInfo lPatrimonial = new ClienteSituacaoFinanceiraPatrimonialInfo(Request["Id"]);

            ReceberEntidadeCadastroRequest<ClienteSituacaoFinanceiraPatrimonialInfo> req = new ReceberEntidadeCadastroRequest<ClienteSituacaoFinanceiraPatrimonialInfo>()
            {
                CodigoEntidadeCadastro = Request["Id"],
                EntidadeCadastro = lPatrimonial,
                 DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                  IdUsuarioLogado = base.UsuarioLogado.Id
            };

            btnSalvar.Visible = UsuarioPode("Salvar", "336585A3-167D-4cc6-8ADB-01FE9931A042");

            lPatrimonial = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<ClienteSituacaoFinanceiraPatrimonialInfo>(req).EntidadeCadastro;

            if (lPatrimonial.IdCliente == 0 && UsuarioPode("Incluir", "4239f749-5c0c-4fd0-abdd-281d94a11744"))
                btnSalvar.Visible = true;

            TransporteInformacoesPatrimoniais lTransportePatrimonial = new TransporteInformacoesPatrimoniais(lPatrimonial);

            hidClientes_InfPatrimoniais_DadosCompletos.Value = JsonConvert.SerializeObject(lTransportePatrimonial);

            return string.Empty;    //só para obedecer assinatura
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { "Salvar"
                                                , "CarregarHtmlComDados"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderSalvar
                                                , ResponderCarregarHtmlComDados
                                                });
        }

        #endregion
    }
}
