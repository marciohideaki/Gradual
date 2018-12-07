using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.Www.MinhaConta.Financeiro;
using System.Web.UI.HtmlControls;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.MinhaConta
{
    public partial class Patrimonio_RendaFixa : ucMinhaContaBase
    {
        public List<Gradual.Site.DbLib.Dados.CustodiaTesouro> CurrentCustodiaTesouro { get; set; }
        public List<CustodiaTesouro> CurrentTituloPrivado   { get; set; }

        public decimal CurrentCustodiaTesouroTotal          { get; set; }
        public decimal CurrentTituloPrivadoTotal            { get; set; }
        public decimal CurrentTotalGeral                    { get; set; }
        public String CurrentCustodiaTesouroTotal_Rotulo    { get; set; }
        public String CurrentTituloPrivadoTotal_Rotulo      { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentCustodiaTesouro                  = (List<Gradual.Site.DbLib.Dados.CustodiaTesouro>)Session["CustodiaTesouro"];
            CurrentTituloPrivado                    = (List<CustodiaTesouro>)Session["TituloPrivado"];

            CurrentCustodiaTesouroTotal_Rotulo      = Resource.Custodia_TesouroTotal;
            CurrentTituloPrivadoTotal_Rotulo        = Resource.Custodia_RendaFixaTotal;

            if (CurrentCustodiaTesouro != null)
            {
                CurrentCustodiaTesouroTotal         = CurrentCustodiaTesouro.AsEnumerable().Sum(x => x.ValorPosicao);
            }

            if (CurrentTituloPrivado != null)
            {
                CurrentTituloPrivadoTotal           = CurrentTituloPrivado.AsEnumerable().Sum(x => x.ValorBruto);
            }

            CurrentTotalGeral                       = (CurrentCustodiaTesouroTotal != null ? CurrentCustodiaTesouroTotal : 0) + (CurrentTituloPrivadoTotal != null ? CurrentTituloPrivadoTotal : 0);

            this.BindRepeaters();
        }

        private void BindRepeaters()
        {
            rptCustodiaTesouro.DataSource = CurrentCustodiaTesouro;
            rptCustodiaTesouro.DataBind();

            rptListaTituloPrivado.DataSource = CurrentTituloPrivado;
            rptListaTituloPrivado.DataBind();

            if (CurrentCustodiaTesouro != null)
            {
                if (CurrentCustodiaTesouro.Count <= 0)
                {
                    Control FooterTemplate = rptCustodiaTesouro.Controls[rptCustodiaTesouro.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
            }
            else
            {
                Control FooterTemplate = rptCustodiaTesouro.Controls[rptCustodiaTesouro.Controls.Count - 1].Controls[1];
                FooterTemplate.FindControl("trError").Visible = true;
            }

            if(CurrentTituloPrivado != null)
            {
                if (CurrentTituloPrivado.Count <= 0)
                {
                    Control FooterTemplate = rptListaTituloPrivado.Controls[rptListaTituloPrivado.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
            }
            else
            {
                Control FooterTemplate = rptListaTituloPrivado.Controls[rptListaTituloPrivado.Controls.Count - 1].Controls[1];
                FooterTemplate.FindControl("trError").Visible = true;
            }
        }
    }
}