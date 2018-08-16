using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.OMS.Library;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;

namespace Gradual.Intranet.Www.Intranet.Sistema.Relatorios
{
    public partial class R0021 : PaginaBaseAutenticada
    {
        #region Propriedades

        private DateTime GetDataDe
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataInicio"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataAte
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataFim"], out lRetorno);

                return lRetorno.AddDays(1);
            }
        }

        #endregion

        #region Métodos Private

        private void ResponderBuscarItensParaListagemSimples()
        {
            ConsultarEntidadeCadastroRequest<ClienteAutorizacaoInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteAutorizacaoInfo>();
            ConsultarEntidadeCadastroResponse<ClienteAutorizacaoInfo> lResponse;

            lRequest.EntidadeCadastro = new ClienteAutorizacaoInfo();
            
            lRequest.EntidadeCadastro.DataAutorizacao_D1 = GetDataDe;   //usando essas propriedades como placeholders pra data
            lRequest.EntidadeCadastro.DataAutorizacao_D2 = GetDataAte.AddDays(1).AddSeconds(-1);

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteAutorizacaoInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                IEnumerable<TransporteClienteAutorizacaoCadastral> lLista = from ClienteAutorizacaoInfo t
                                                                              in lResponse.Resultado
                                                                          select new TransporteClienteAutorizacaoCadastral(t, false, false, false, false, false, false);

                rptRelatorio.DataSource = lLista;
                rptRelatorio.DataBind();

                rowLinhaDeNenhumItem.Visible = (lResponse.Resultado.Count == 0);
            }
        }

        #endregion

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (base.Acao == "BuscarItensParaListagemSimples")
            {
                this.ResponderBuscarItensParaListagemSimples();
            }
        }

        #endregion

    }
}