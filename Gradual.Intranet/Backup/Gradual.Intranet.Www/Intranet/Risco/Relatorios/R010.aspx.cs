using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco;

namespace Gradual.Intranet.Www.Intranet.Risco.Relatorios
{
    public partial class R010 : PaginaBaseAutenticada
    {

        #region |Propriedades
        private string GetAtivo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["Papel"]))
                    return null;

                return this.Request.Form["Papel"];
            }
        }
        #endregion

        #region |Eventos
        
        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                if (this.Acao == "BuscarItensParaListagemSimples")
                {
                    this.ResponderBuscarItensParaListagemSimples();
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax(ex.ToString());
            }
        }
        #endregion

        #region |Métodos
        private void ResponderBuscarItensParaListagemSimples()
        {
            var lEntidadeCadastro = new RiscoClienteMercadoVistaOpcaoRelInfo();
            {
                lEntidadeCadastro.ConsultaDsAtivo = this.GetAtivo;

            };

            var lConsulta = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoClienteMercadoVistaOpcaoRelInfo>(
                new ConsultarEntidadeCadastroRequest<RiscoClienteMercadoVistaOpcaoRelInfo>()
                {
                    EntidadeCadastro = lEntidadeCadastro
                });

            if (null != lConsulta.Resultado && !0.Equals(lConsulta.Resultado.Count))
            {
                var lListaTransporte = new TransporteRelatorio_010().TraduzirLista(lConsulta.Resultado);
                base.PopularComboComListaGenerica<TransporteRelatorio_010>(lListaTransporte, this.rptRelatorio);
                //this.lblTotalBloqueado.Text = (lListaTransporte != null && lListaTransporte.Count > 0) ? lListaTransporte[0].BloqueadoTotalGeral : "0,00";
                this.rowLinhaDeNenhumItem.Visible = false;
            }
            else
                this.rowLinhaDeNenhumItem.Visible = true;
        }
        #endregion
    }
}