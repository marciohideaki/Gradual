using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.MinhaConta.TesouroDireto
{
    public partial class MenuTesouro : System.Web.UI.UserControl
    {
        public bool AbasHabilitadas
        {
            get
            {
                return this.lnkConsultaTitulos.Enabled;
            }
            set
            {
                lnkVenda.Enabled =
                lnkCompra.Enabled =
                lnkExtrato.Enabled =
                lnkProtocolo.Enabled =
                lnkConsultaTitulos.Enabled = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string lUrl = Request.Url.PathAndQuery.ToLower();

            lnkCompra.NavigateUrl = string.Format("{0}/MinhaConta/TesouroDireto/Compra.aspx", ((PaginaBase)this.Page).HostERaizHttps);

            lnkVenda.NavigateUrl = string.Format("{0}/MinhaConta/TesouroDireto/Venda.aspx", ((PaginaBase)this.Page).HostERaizHttps);

            lnkConsultaTitulos.NavigateUrl = string.Format("{0}/MinhaConta/TesouroDireto/Consulta.aspx", ((PaginaBase)this.Page).HostERaizHttps);

            lnkExtrato.NavigateUrl = string.Format("{0}/MinhaConta/TesouroDireto/Extrato.aspx", ((PaginaBase)this.Page).HostERaizHttps);

            lnkProtocolo.NavigateUrl = string.Format("{0}/MinhaConta/TesouroDireto/ConsultarProtocolo.aspx", ((PaginaBase)this.Page).HostERaizHttps);

            if (lUrl.Contains("compra.aspx")) lnkCompra.CssClass                = "Selecionado";

            if (lUrl.Contains("venda.aspx")) lnkVenda.CssClass                  = "Selecionado";

            if (lUrl.Contains("consulta.aspx")) lnkConsultaTitulos.CssClass       = "Selecionado";

            if (lUrl.Contains("extrato.aspx")) lnkExtrato.CssClass              = "Selecionado";

            if (lUrl.Contains("consultarprotocolo.aspx")) lnkProtocolo.CssClass  = "Selecionado";
        }
    }
}