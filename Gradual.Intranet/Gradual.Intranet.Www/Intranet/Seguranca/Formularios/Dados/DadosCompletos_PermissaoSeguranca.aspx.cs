using System;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados
{
    [ValidarSegurancaAttribute("9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F", "1", "1")]
    public partial class DadosCompletos_PermissaoSeguranca : PaginaBaseAutenticada
    {
        public string ResponderCarregarHtmlComDados()
        {
            string Id = Request["Id"];

            TransporteSegurancaPermissaoSeguranca lDadosPermissao;
            ReceberPermissaoRequest lRequest = new ReceberPermissaoRequest()
            {
                CodigoSessao = this.CodigoSessao,
                CodigoPermissao = Id
            };

            ReceberPermissaoResponse lResponse = this.ServicoSeguranca.ReceberPermissao(lRequest);
            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lDadosPermissao = new TransporteSegurancaPermissaoSeguranca(lResponse.Permissao);

                hidDadosCompletos_Seguranca_PermissaoSeguranca.Value = JsonConvert.SerializeObject(lDadosPermissao);
            }
            else
            {
                return RetornarErroAjax(lResponse.DescricaoResposta);
            }
            return string.Empty;
        }

        public string ResponderCadastrar()
        {
            string lretorno = string.Empty;
            string lObjetoJson = Request["ObjetoJson"];

            TransporteSegurancaPermissaoSeguranca lDadosPermissao = null;

            MensagemResponseBase lResponse = null;
            SalvarPermissaoRequest lRequest = new SalvarPermissaoRequest();

            PermissaoInfo lPermissaoInfo = new PermissaoInfo();

            try
            {
                lDadosPermissao = JsonConvert.DeserializeObject<TransporteSegurancaPermissaoSeguranca>(lObjetoJson);

                lPermissaoInfo = lDadosPermissao.ToPermissaoInfo();

                lRequest.Permissao = lPermissaoInfo;

                lRequest.CodigoSessao = this.CodigoSessao;

                lResponse = ServicoSeguranca.SalvarPermissao(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (string.IsNullOrWhiteSpace(lDadosPermissao.Id))
                        base.RegistrarLogInclusao();
                    else
                        base.RegistrarLogAlteracao();

                    lretorno = RetornarSucessoAjax(new TransporteRetornoDeCadastro(lResponse.DescricaoResposta), "Permissão cadastrada com sucesso");
                }
                else
                {
                    lretorno = RetornarErroAjax(lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                lretorno = RetornarErroAjax("Erro durante o envio do request para cadastrar permissão", ex);
            }
            return lretorno;
        }

        public string ResponderExcluir()
        {
            string lretorno = string.Empty;
            string lPermissaoId = Request["Id"];
            RemoverPermissaoRequest lRequest = new RemoverPermissaoRequest()
            {
                CodigoSessao = this.CodigoSessao,
                CodigoPermissao = lPermissaoId

            };

            MensagemResponseBase lResponse = null;
            try
            {
                lResponse = this.ServicoSeguranca.RemoverPermissao(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lretorno = RetornarSucessoAjax("Permissao excluida com sucesso!");
                    base.RegistrarLogExclusao();
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

        public string ResponderAtualizar()
        {
            return ResponderCadastrar();
        }
        
        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] { "Cadastrar"
                                                     , "Excluir"
                                                     , "Atualizar"
                                                     , "CarregarHtmlComDados"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { ResponderCadastrar
                                                     , ResponderExcluir 
                                                     , ResponderAtualizar
                                                     , ResponderCarregarHtmlComDados
                                                     });

        }

        #endregion
    }
}
