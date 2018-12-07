using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.Www;

namespace Gradual.Site.Www.MinhaConta
{
    public partial class Default : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.ValidarSessao();

                //Response.Redirect("~/MinhaConta/Financeiro/SaldoseLimites.aspx");
                Response.Redirect("~/MinhaConta/Financeiro/MinhaConta.aspx");

                //this.CarregarDados();

                RodarJavascriptOnLoad("MinhaConta_GerarGrafico(); GradSite_VerificarPositivoseNegativos('#tblSaldo td.ValorNumerico');");
            }
            catch (Exception ex)
            {
                base.ExibirMensagemJsOnLoad(ex);
            }
        }
    }
}