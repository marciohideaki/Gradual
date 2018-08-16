using System;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados
{
    public partial class DadosCompletos_Parametro : PaginaBaseAutenticada
    {
        #region | Métodos

        private void PopularBolsas()
        {
            ListarBolsaResponse lResBolsa = this.ServicoRegrasRisco.ListarBolsasRisco(new ListarBolsaRequest());
            if (lResBolsa.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                rptRisco_DadosCompletos_Parametro_Bolsa.DataSource = lResBolsa.Bolsas;
                rptRisco_DadosCompletos_Parametro_Bolsa.DataBind();
            }
            else
            {
                throw new Exception(lResBolsa.DescricaoResposta);
            }
        }

        private string ResponderExcluir()
        {

            string lretorno = string.Empty;
            string lId = Request["Id"];

            RemoverParametroRiscoRequest lRequest = new RemoverParametroRiscoRequest() { CodigoParametro = int.Parse(lId), DescricaoUsuarioLogado = base.UsuarioLogado.Nome, IdUsuarioLogado = base.UsuarioLogado.Id };
            RemoverParametroRiscoResponse lResponse = null;
            try
            {
                lResponse = this.ServicoRegrasRisco.RemoverParametroRisco(lRequest);

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
                TransporteRiscoParametro lDadosParametro;

                SalvarParametroRiscoRequest lRequest;

                SalvarParametroRiscoResponse lResponse;
                try
                {
                    lDadosParametro = JsonConvert.DeserializeObject<TransporteRiscoParametro>(lObjetoJson);
                    lRequest = new SalvarParametroRiscoRequest();
                    lRequest.ParametroRisco = lDadosParametro.ToParametroRiscoInfo();
                    string lAcao = "alterado";
                    if (lRequest.ParametroRisco.CodigoParametro == 0)
                    {
                        lAcao = "inserido";
                    }
                    try
                    {
                        lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                        lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;
                        lResponse = this.ServicoRegrasRisco.SalvarParametroRisco(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            var lMensagem = string.Format("Parâmetro {0}  com sucesso" + lAcao);

                            lRetorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.ParametroRisco.CodigoParametro), lMensagem);

                            if (lRequest.ParametroRisco.CodigoParametro == 0)
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
                        lRetorno = RetornarErroAjax("Erro durante o envio do request para alterar o Parâmetro", exEnvioRequest);
                    }
                }
                catch (Exception exDeserializacaoCliente)
                {
                    lRetorno = RetornarErroAjax("Erro durante a deserialização dos dados do parâmetro", exDeserializacaoCliente);
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
                ReceberParametroRiscoRequest lRequest = new ReceberParametroRiscoRequest()
                {
                    CodigoParametro = lIdDoObjeto
                };

                lRequest.DescricaoUsuarioLogado = base.UsuarioLogado.Nome;
                lRequest.IdUsuarioLogado = base.UsuarioLogado.Id;

                ReceberParametroRiscoResponse lResParametro = this.ServicoRegrasRisco.ReceberParametroRisco(lRequest);

                TransporteRiscoParametro lParametro = new TransporteRiscoParametro(lResParametro.ParametroRisco);
                //{
                //    Id = lIdDoObjeto.ToString(),
                //    Nome = lResParametro.ParametroRisco.NomeParametro,
                //    Bolsa = lResParametro.ParametroRisco
                //};


                hidDadosCompletos_Risco_Parametro.Value = JsonConvert.SerializeObject(lParametro);

            }

            return string.Empty;    //só para obedecer assinatura
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            base.RegistrarRespostasAjax(new string[] { "Cadastrar"
                                                     , "Atualizar"
                                                     , "CarregarHtmlComDados"
                                                     , "Excluir"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { this.ResponderCadastrar
                                                     , this.ResponderAtualizar
                                                     , this.ResponderCarregarHtmlComDados
                                                     , this.ResponderExcluir});
            if (!IsPostBack)
            {
                this.PopularBolsas();
            }
        }

        #endregion
    }
}
