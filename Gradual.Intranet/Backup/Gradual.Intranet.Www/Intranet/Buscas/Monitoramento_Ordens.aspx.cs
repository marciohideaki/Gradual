using System;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;

namespace Gradual.Intranet.Www.Intranet.Buscas
{
    public partial class Monitoramento_Ordens : PaginaBaseAutenticada
    {
        #region | Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            this.CarregarCombos();
        }

        #endregion

        private void CarregarCombos()
        {
            var lDados = new MonitoramentoOrdemDbLib().BuscarSistemaOrigem(new BuscarSistemaOrigemRequest());

            this.rptBusca_Monitoramento_Ordens_Sistema.DataSource = lDados.Resultado;
            this.rptBusca_Monitoramento_Ordens_Sistema.DataBind();
        }
    }
}
