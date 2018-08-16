using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Solicitacoes.Formularios.Dados
{
    public partial class CadastroDeCambio : PaginaBaseAutenticada
    {
        /// <summary>
        /// Carrega os dados no formulário
        /// </summary>
        /// <returns></returns>
        private string ResponderCarregarHtmlComDados()
        {
            ConsultarEntidadeCadastroRequest<ProdutoInfo> lRequest = new ConsultarEntidadeCadastroRequest<ProdutoInfo>();
            ConsultarEntidadeCadastroResponse<ProdutoInfo> lResponse;

            lRequest.EntidadeCadastro = new ProdutoInfo();
            lRequest.EntidadeCadastro.IdPlano = 2;  //produtos de câmbio = 2

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ProdutoInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                rptListaDeProdutos.DataSource = lResponse.Resultado;
                rptListaDeProdutos.DataBind();

                rowLinhaDeNenhumItem.Visible = (lResponse.Resultado.Count == 0);
            }

            return string.Empty;    //só para obedecer assinatura
        }
        
        public string ResponderSalvar()
        {
            string lRetorno = "";

            string lJson = Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lJson))
            {
                try
                {
                    TransporteProdutoInfo lTransporte = JsonConvert.DeserializeObject<TransporteProdutoInfo>(lJson);

                    try
                    {
                        ProdutoInfo lProduto = lTransporte.ToProdutoInfo();

                        lProduto.IdPlano = 2;   //fixo: "Cambio"

                        SalvarEntidadeCadastroRequest<ProdutoInfo> lRequest = new SalvarEntidadeCadastroRequest<ProdutoInfo>();

                        SalvarEntidadeCadastroResponse lResponse;

                        lRequest.EntidadeCadastro = lProduto;

                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<ProdutoInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lTransporte.IdProduto = ((ProdutoInfo)lResponse.Objeto).IdProduto.Value.ToString();

                            lRetorno = RetornarSucessoAjax(lTransporte, "Dados salvos com sucesso");
                        }
                        else
                        {
                            lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                        }
                    }
                    catch (Exception ex)
                    {
                        lRetorno = RetornarErroAjax("Erro ao salvar objeto", ex);
                    }
                }
                catch (Exception exJson)
                {
                    lRetorno = RetornarErroAjax("Erro ao deserializar objeto JSON [{0}]", exJson, lJson);
                }
            }

            return lRetorno;
        }

        public string ResponderBuscar()
        {
            string lRetorno = "";
            /*
            string lID = Request["ID"];

            if (!string.IsNullOrEmpty(lID))
            {
                int lIdDoObjeto;

                if (int.TryParse(lID, out lIdDoObjeto))
                {
                    ReceberEntidadeCadastroRequest<AvisoHomeBrokerInfo> lRequest = new ReceberEntidadeCadastroRequest<AvisoHomeBrokerInfo>();

                    ReceberEntidadeCadastroResponse<AvisoHomeBrokerInfo> lResponse;

                    lRequest.EntidadeCadastro = new AvisoHomeBrokerInfo();

                    lRequest.EntidadeCadastro.IdAviso = lIdDoObjeto;

                    lResponse = this.ServicoPersistenciaCadastro.ReceberEntidadeCadastro<AvisoHomeBrokerInfo>(lRequest);

                    if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                    {
                        lRetorno = RetornarSucessoAjax(new TransporteAvisoHomeBroker(lResponse.EntidadeCadastro), "Objeto encontrado");
                    }
                    else
                    {
                        lRetorno = RetornarErroAjax(lResponse.DescricaoResposta);
                    }
                }
                else
                {
                    lRetorno = RetornarErroAjax("ID inválido");
                }
            }
            */
            return lRetorno;
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] {
                                                    "CarregarHtmlComDados"
                                                  , "Salvar"
                                                },
                new ResponderAcaoAjaxDelegate[] {
                                                    ResponderCarregarHtmlComDados
                                                  , ResponderSalvar
                                                });
        }
    }
}