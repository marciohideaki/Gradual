using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.HomeBroker
{
    public partial class Avisos : PaginaBaseAutenticada
    {
        #region | Métodos

        /// <summary>
        /// Carrega os dados no formulário
        /// </summary>
        /// <returns></returns>
        private string ResponderCarregarHtmlComDados()
        {
            ConsultarEntidadeCadastroRequest<AvisoHomeBrokerInfo> lRequest = new ConsultarEntidadeCadastroRequest<AvisoHomeBrokerInfo>();
            ConsultarEntidadeCadastroResponse<AvisoHomeBrokerInfo> lResponse;

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<AvisoHomeBrokerInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                IEnumerable<TransporteAvisoHomeBroker> lLista = from AvisoHomeBrokerInfo a
                                                                  in lResponse.Resultado
                                                                select new TransporteAvisoHomeBroker(a);

                rptListaDeAvisos.DataSource = lLista;
                rptListaDeAvisos.DataBind();

                rowLinhaDeNenhumItem.Visible = (lLista.Count().Equals(0));
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
                    TransporteAvisoHomeBroker lTransporte = JsonConvert.DeserializeObject<TransporteAvisoHomeBroker>(lJson);

                    try
                    {
                        AvisoHomeBrokerInfo lAviso = lTransporte.ToAvisoHomeBrokerInfo();

                        SalvarEntidadeCadastroRequest<AvisoHomeBrokerInfo> lRequest = new SalvarEntidadeCadastroRequest<AvisoHomeBrokerInfo>();

                        SalvarEntidadeCadastroResponse lResponse;

                        lRequest.EntidadeCadastro = lAviso;

                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<AvisoHomeBrokerInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            int lIDCadastrado;

                            if (int.TryParse(lResponse.DescricaoResposta, out lIDCadastrado))
                            {
                                //foi uma inclusão, recebe o ID novo:
                                lRequest.EntidadeCadastro.IdAviso = int.Parse(lResponse.DescricaoResposta);

                                if (lTransporte.CodigoAviso > 0)
                                    base.RegistrarLogAlteracao(string.Concat("Mensagem inserida: ", lAviso.DsAviso));
                                else
                                    base.RegistrarLogInclusao(string.Concat("Mensagem inserida: ", lAviso.DsAviso));
                            }

                            var lTransporteRetorno = new TransporteAvisoHomeBroker(lRequest.EntidadeCadastro);

                            lTransporteRetorno.AtualizarTextoTruncado();

                            lRetorno = RetornarSucessoAjax(lTransporteRetorno, "Dados salvos com sucesso");
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

            return lRetorno;
        }

        public string ResponderSalvar_Teste()
        {
            string lRetorno = "";

            string lJson = Request["ObjetoJson"];

            TransporteAvisoHomeBroker lTransporte = JsonConvert.DeserializeObject<TransporteAvisoHomeBroker>(lJson);

            if (lTransporte.CodigoAviso == 0)
                lTransporte.CodigoAviso = 9999;

            lRetorno = RetornarSucessoAjax(lTransporte, "Dados salvos com sucesso");

            return lRetorno;
        }

        public string ResponderExcluir()
        {
            return "";
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);


            base.RegistrarRespostasAjax(new string[] { "SalvarAviso"
                                                     , "BuscarAviso"
                                                     , "Excluir"
                                                     },
                     new ResponderAcaoAjaxDelegate[] { ResponderSalvar
                                                     , ResponderBuscar
                                                     , ResponderExcluir
                                                     });
            if (!Page.IsPostBack)
                this.ResponderCarregarHtmlComDados();
        }

        #endregion
    }
}