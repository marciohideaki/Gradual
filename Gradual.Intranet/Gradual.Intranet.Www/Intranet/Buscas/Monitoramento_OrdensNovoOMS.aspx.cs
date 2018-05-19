using System;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.OMS.AcompanhamentoOrdens.Lib.Mensageria;

namespace Gradual.Intranet.Www.Intranet.Buscas
{
    public partial class Monitoramento_OrdensNovoOMS : PaginaBaseAutenticada
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            this.CarregarCombos();
        }

        private void CarregarCombos()
        {
            var lDados = new MonitoramentoOrdemDbLib().BuscarSistemaOrigem(new BuscarSistemaOrigemRequest());

            this.rptBusca_Monitoramento_OrdensNovoOMS_Sistema.DataSource = lDados.Resultado;
            this.rptBusca_Monitoramento_OrdensNovoOMS_Sistema.DataBind();
        }
    }
}