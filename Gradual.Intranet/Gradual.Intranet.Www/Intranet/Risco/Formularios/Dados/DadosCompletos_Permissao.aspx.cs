using System;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class DadosCompletos_Permissao : PaginaBaseAutenticada
    {
        #region | Métodos

        public string ResponderExcluir()
        {
            string lretorno = string.Empty;
            string lId = Request["Id"];

            RemoverPermissaoRiscoRequest lRequest = new RemoverPermissaoRiscoRequest() { CodigoPermissao = int.Parse(lId) };
            lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
            lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
            RemoverPermissaoRiscoResponse lResponse = null;
            try
            {
                lResponse = this.ServicoRegrasRisco.RemoverPermissaoRisco(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.BusinessException)
                    {
                        lretorno = RetornarErroAjax(lResponse.MessageException);
                    }
                    else
                    {
                        lretorno = RetornarSucessoAjax(lResponse.DescricaoResposta);
                        base.RegistrarLogExclusao();
                    }
                }
                else
                {
                    lretorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lretorno = RetornarErroAjax(ex.Message);
            }
            return lretorno;

        }

        private string ResponderAtualizar()
        {
            string lRetorno = "";
            string lObjetoJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                TransporteRiscoPermissao lDadosPermissao;

                SalvarPermissaoRiscoRequest lRequest;

                SalvarPermissaoRiscoResponse lResponse;
                try
                {
                    lDadosPermissao = JsonConvert.DeserializeObject<TransporteRiscoPermissao>(lObjetoJson);
                    lRequest = new SalvarPermissaoRiscoRequest();
                    lRequest.PermissaoRisco = lDadosPermissao.ToPermissaoRiscoInfo();
                    string lAcao = "alterada";
                    if (lRequest.PermissaoRisco.CodigoPermissao == 0)
                    {
                        lAcao = "inserida";
                    }
                    try
                    {
                        lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                        lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
                        lResponse = this.ServicoRegrasRisco.SalvarPermissaoRisco(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            var lMensagem = string.Format("Permissão {0} com sucesso", lAcao);

                            lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.PermissaoRisco.CodigoPermissao), lMensagem);

                            if (lRequest.PermissaoRisco.CodigoPermissao == 0)
                                base.RegistrarLogInclusao(lMensagem);
                            else
                                base.RegistrarLogAlteracao(lMensagem);
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                        }
                    }
                    catch (Exception exEnvioRequest)
                    {
                        lRetorno = RetornarErroAjax("Erro durante o envio do request para alterar a Permissão", exEnvioRequest);
                    }
                }
                catch (Exception exDeserializacaoCliente)
                {
                    lRetorno = RetornarErroAjax("Erro durante a deserialização dos dados da permissão", exDeserializacaoCliente);
                }
            }
            else
            {
                lRetorno = RetornarErroAjax("Foi enviada ação de cadastro sem objeto para alterar");
            }
            return lRetorno;
        }

        private string ResponderCadastrar()
        {
            return this.ResponderAtualizar();
        }

        private string ResponderCarregarHtmlComDados()
        {
            int lIdDoObjeto;

            if (int.TryParse(Request["Id"], out lIdDoObjeto))
            {
                ReceberPermissaoRiscoRequest lRequest = new ReceberPermissaoRiscoRequest()
                {
                    CodigoPermissao = lIdDoObjeto
                };
                lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
                ReceberPermissaoRiscoResponse lResPermissao = this.ServicoRegrasRisco.ReceberPermissaoRisco(lRequest);

                TransporteRiscoPermissao lPermissao = new TransporteRiscoPermissao(lResPermissao.PermissaoRisco);
                //{
                //    Id = lIdDoObjeto.ToString(),
                //    Nome = lResPermissao.PermissaoRisco.NomePermissao
                //};


                hidDadosCompletos_Risco_Permissao.Value = JsonConvert.SerializeObject(lPermissao);

            }

            return string.Empty;    //só para obedecer assinatura
        }

        private void PopularBolsas()
        {
            ListarBolsaResponse lResBolsa = this.ServicoRegrasRisco.ListarBolsasRisco(new ListarBolsaRequest());
            if (lResBolsa.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                rptRisco_DadosCompletos_Permissao_Bolsa.DataSource = lResBolsa.Bolsas;
                rptRisco_DadosCompletos_Permissao_Bolsa.DataBind();
            }
            else
            {
                throw new Exception(lResBolsa.DescricaoResposta);
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            RegistrarRespostasAjax(new string[] { "Cadastrar"
                                                , "Atualizar"
                                                , "CarregarHtmlComDados"
                                                , "Excluir"
                                                },
                new ResponderAcaoAjaxDelegate[] { ResponderCadastrar
                                                , ResponderAtualizar
                                                , ResponderCarregarHtmlComDados
                                                , ResponderExcluir});
            if (!IsPostBack)
            {
                PopularBolsas();
            }
        }

        #endregion
    }
}