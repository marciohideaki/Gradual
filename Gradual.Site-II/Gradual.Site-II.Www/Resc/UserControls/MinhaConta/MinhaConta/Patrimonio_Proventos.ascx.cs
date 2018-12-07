using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.MinhaConta
{
    public partial class Patrimonio_Proventos : System.Web.UI.UserControl
    {
        public List<Gradual.Site.DbLib.Dados.Provento> CurrentProventos { get; set; }
        public decimal CurrentTotalProventos                            { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentProventos = (List<Gradual.Site.DbLib.Dados.Provento>)Session["Proventos"];

            if (CurrentProventos != null)
            {
                CurrentTotalProventos = CurrentProventos.AsEnumerable().Sum(x => x.Valor);
            }

            this.BindRepeaters();
        }

        private void BindRepeaters()
        {
            rptProventos.DataSource = CurrentProventos != null ? CurrentProventos : new List<Gradual.Site.DbLib.Dados.Provento>();
            rptProventos.DataBind();

            if (CurrentProventos != null)
            {
                if (CurrentProventos.Count <= 0)
                {
                    Control FooterTemplate = rptProventos.Controls[rptProventos.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
            }
            else
            {
                Control FooterTemplate = rptProventos.Controls[rptProventos.Controls.Count - 1].Controls[1];
                FooterTemplate.FindControl("trError").Visible = true;
            }
        }
    }
}