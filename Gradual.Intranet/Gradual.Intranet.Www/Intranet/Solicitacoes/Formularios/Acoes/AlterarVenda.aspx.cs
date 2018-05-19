using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados;
using Newtonsoft.Json;
using Gradual.OMS.Library;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;

namespace Gradual.Intranet.Www.Intranet.Solicitacoes.Formularios.Acoes
{
    public partial class AlterarVenda : PaginaBaseAutenticada
    {
        #region Métodos Private

        private string ResponderCarregarHtmlComDados()
        {
            string lMensagem;

            ConsultarEntidadeCadastroRequest<AlteracaoDeVendaInfo>  lRequest = new ConsultarEntidadeCadastroRequest<AlteracaoDeVendaInfo>();
            ConsultarEntidadeCadastroResponse<AlteracaoDeVendaInfo> lResponse;

            lRequest.EntidadeCadastro = new AlteracaoDeVendaInfo();
            lRequest.EntidadeCadastro.Busca_IdVenda = Convert.ToInt32(Request["Id"]);

            try
            {
                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<AlteracaoDeVendaInfo>(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    rptHistoricoDeAlteracoes.DataSource = lResponse.Resultado;
                    rptHistoricoDeAlteracoes.DataBind();
                }
                else
                {
                    lMensagem = string.Format("Resposta com erro do serviço");

                    Logger.Error(lMensagem);
                }
            }
            catch (Exception exBusca)
            {
                lMensagem = string.Format("Erro em DadosDeVenda.aspx > ResponderCarregarHtmlComDados() [{0}]\r\n{1}", exBusca.Message, exBusca.StackTrace);

                Logger.Error(lMensagem);
            }

            return string.Empty;
        }

        private string ResponderSalvar()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = this.Request["ObjetoJson"];

            if (!string.IsNullOrEmpty(lObjetoJson))
            {
                AlteracaoDeVendaInfo lDados;

                SalvarEntidadeCadastroRequest<AlteracaoDeVendaInfo> lRequest;

                SalvarEntidadeCadastroResponse lResponse;

                try
                {
                    lDados = JsonConvert.DeserializeObject<AlteracaoDeVendaInfo>(lObjetoJson);

                    lRequest = new SalvarEntidadeCadastroRequest<AlteracaoDeVendaInfo>();

                    lDados.DtData = DateTime.Now;
                    lDados.DsUsuario = base.UsuarioLogado.Nome;

                    lRequest.EntidadeCadastro = lDados;

                    try
                    {
                        lResponse = this.ServicoPersistenciaCadastro.SalvarEntidadeCadastro<AlteracaoDeVendaInfo>(lRequest);

                        if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                        {
                            lRetorno = RetornarSucessoAjax(Convert.ToInt32(lResponse.DescricaoResposta), "Dados alterados com sucesso");
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

        #endregion

        #region Event Handlers

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

        #endregion
    }
}