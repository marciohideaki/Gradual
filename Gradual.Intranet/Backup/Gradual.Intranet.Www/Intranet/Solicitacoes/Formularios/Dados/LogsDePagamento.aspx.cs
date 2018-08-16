using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.Intranet.Solicitacoes.Formularios.Dados
{
    public partial class LogsDePagamento : PaginaBaseAutenticada
    {
        #region Métodos Private

        private string ResponderCarregarHtmlComDados()
        {
            string lMensagem;

            ConsultarEntidadeCadastroRequest<PagamentoLogInfo>  lRequest = new ConsultarEntidadeCadastroRequest<PagamentoLogInfo>();
            ConsultarEntidadeCadastroResponse<PagamentoLogInfo> lResponse;

            lRequest.EntidadeCadastro = new PagamentoLogInfo();
            lRequest.EntidadeCadastro.Busca_IdVenda = Convert.ToInt32(Request["Id"]);

            try
            {
                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<PagamentoLogInfo>(lRequest);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    rptLog.DataSource = lResponse.Resultado;

                    rptLog.DataBind();
                }
                else
                {
                    lMensagem = string.Format("Resposta com erro do serviço");

                    Logger.Error(lMensagem);
                }
            }
            catch (Exception exBusca)
            {
                lMensagem = string.Format("Erro em LogsDePagamento.aspx > ResponderCarregarHtmlComDados() [{0}]\r\n{1}", exBusca.Message, exBusca.StackTrace);

                Logger.Error(lMensagem);
            }

            return string.Empty;
        }

        #endregion

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] {
                                                  "CarregarHtmlComDados"
                                                },
                new ResponderAcaoAjaxDelegate[] {
                                                  ResponderCarregarHtmlComDados
                                                });
        }

        #endregion
    }
}