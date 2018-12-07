using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.Www.MinhaConta.Financeiro;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.MinhaConta
{
    public partial class Patrimonio_FundosInvestimentosWebUserControl1 : ucMinhaContaBase
    {
        public List<FundoPosicao> CurrentCustodiaFundoRendaFixa     { get; set; }
        public List<FundoPosicao> CurrentCustodiaFundoMultimercado  { get; set; }
        public List<FundoPosicao> CurrentCustodiaFundoAcao          { get; set; }
        public List<FundoPosicao> CurrentCustodiaFundoOutros        { get; set; }

        public decimal CurrentCustodiaFundoRendaFixaTotal           { get; set; }
        public decimal CurrentCustodiaFundoMultimercadoTotal        { get; set; }
        public decimal CurrentCustodiaFundoAcaoTotal                { get; set; }
        public decimal CurrentCustodiaFundoOutrosTotal              { get; set; }
        public decimal CurrentTotalGeral                            { get; set; }

        public String CurrentCustodiaFundoRendaFixaTotal_Rotulo     { get; set; }
        public String CurrentCustodiaFundoMultimercadoTotal_Rotulo  { get; set; }
        public String CurrentCustodiaFundoAcaoTotal_Rotulo          { get; set; }
        public String CurrentCustodiaFundoOutrosTotal_Rotulo        { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentCustodiaFundoRendaFixa                   = (List<FundoPosicao>)Session["FundoPosicaoRendaFixa"];
            CurrentCustodiaFundoMultimercado                = (List<FundoPosicao>)Session["FundoPosicaoMultimercado"];
            CurrentCustodiaFundoAcao                        = (List<FundoPosicao>)Session["FundoPosicaoAcao"];
            CurrentCustodiaFundoOutros                      = (List<FundoPosicao>)Session["FundoPosicaoOutros"];

            CurrentCustodiaFundoMultimercadoTotal_Rotulo    = Resource.Custodia_FundoMultimercado_Total;
            CurrentCustodiaFundoRendaFixaTotal_Rotulo       = Resource.Custodia_FundoRendaFixa_Total;
            CurrentCustodiaFundoAcaoTotal_Rotulo            = Resource.Custodia_FundoAcao_Total;
            CurrentCustodiaFundoOutrosTotal_Rotulo          = Resource.Custodia_FundoOutros_Total;

            if (CurrentCustodiaFundoRendaFixa != null)
            {
                CurrentCustodiaFundoRendaFixaTotal          = CurrentCustodiaFundoRendaFixa.AsEnumerable().Sum(x => x.ValorLiquido);
            }

            if (CurrentCustodiaFundoMultimercado != null)
            {
                CurrentCustodiaFundoMultimercadoTotal       = CurrentCustodiaFundoMultimercado.AsEnumerable().Sum(x => x.ValorLiquido);
            }

            if (CurrentCustodiaFundoAcao != null)
            {
                CurrentCustodiaFundoAcaoTotal               = CurrentCustodiaFundoAcao.AsEnumerable().Sum(x => x.ValorLiquido);
            }

            if (CurrentCustodiaFundoOutros != null)
            {
                CurrentCustodiaFundoOutrosTotal = CurrentCustodiaFundoOutros.AsEnumerable().Sum(x => x.ValorLiquido);
            }

            CurrentTotalGeral =     (CurrentCustodiaFundoRendaFixaTotal     != null ? CurrentCustodiaFundoRendaFixaTotal    : 0)
                                  + (CurrentCustodiaFundoMultimercadoTotal  != null ? CurrentCustodiaFundoMultimercadoTotal : 0)
                                  + (CurrentCustodiaFundoAcaoTotal          != null ? CurrentCustodiaFundoAcaoTotal         : 0)
                                  + (CurrentCustodiaFundoOutrosTotal        != null ? CurrentCustodiaFundoOutrosTotal       : 0);

            this.BindRepeaters();
        }

        private void BindRepeaters()
        {
            rptFundoPosicaoRendaFixa.DataSource = CurrentCustodiaFundoRendaFixa != null ? CurrentCustodiaFundoRendaFixa : new List<FundoPosicao>();
            rptFundoPosicaoRendaFixa.DataBind();

            rptFundoPosicaoMultimercado.DataSource = CurrentCustodiaFundoMultimercado!= null ? CurrentCustodiaFundoMultimercado : new List<FundoPosicao>();
            rptFundoPosicaoMultimercado.DataBind();

            rptFundoPosicaoAcao.DataSource = CurrentCustodiaFundoAcao!= null ? CurrentCustodiaFundoAcao : new List<FundoPosicao>();
            rptFundoPosicaoAcao.DataBind();

            rptFundoPosicaoOutros.DataSource = CurrentCustodiaFundoOutros != null ? CurrentCustodiaFundoOutros : new List<FundoPosicao>();
            rptFundoPosicaoOutros.DataBind();

            if (CurrentCustodiaFundoRendaFixa != null)
            {
                if (CurrentCustodiaFundoRendaFixa.Count <= 0)
                {
                    Control FooterTemplate = rptFundoPosicaoRendaFixa.Controls[rptFundoPosicaoRendaFixa.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
            }
            else
            {
                Control FooterTemplate = rptFundoPosicaoRendaFixa.Controls[rptFundoPosicaoRendaFixa.Controls.Count - 1].Controls[1];
                FooterTemplate.FindControl("trError").Visible = true;
            }

            if(CurrentCustodiaFundoMultimercado != null)
            {
                if (CurrentCustodiaFundoMultimercado.Count <= 0)
                {
                    Control FooterTemplate = rptFundoPosicaoMultimercado.Controls[rptFundoPosicaoMultimercado.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
            }
            else
            {
                Control FooterTemplate = rptFundoPosicaoMultimercado.Controls[rptFundoPosicaoMultimercado.Controls.Count - 1].Controls[1];
                FooterTemplate.FindControl("trError").Visible = true;
            }

            if(CurrentCustodiaFundoAcao != null)
            {
                if (CurrentCustodiaFundoAcao.Count <= 0)
                {
                    Control FooterTemplate = rptFundoPosicaoAcao.Controls[rptFundoPosicaoAcao.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
            }
            else
            {
                Control FooterTemplate = rptFundoPosicaoAcao.Controls[rptFundoPosicaoAcao.Controls.Count - 1].Controls[1];
                FooterTemplate.FindControl("trError").Visible = true;
            }

            if (CurrentCustodiaFundoOutros != null)
            {
                if (CurrentCustodiaFundoOutros.Count <= 0)
                {
                    Control FooterTemplate = rptFundoPosicaoOutros.Controls[rptFundoPosicaoOutros.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
            }
            else
            {
                Control FooterTemplate = rptFundoPosicaoOutros.Controls[rptFundoPosicaoOutros.Controls.Count - 1].Controls[1];
                FooterTemplate.FindControl("trError").Visible = true;
            }
        }

        protected string GetClass_Valor(decimal valor)
        {
            if (valor < 0)
            {
                return "MinhaConta_Negativo";
            }
            else
            {
                return "";
            }
        }

        protected string GetClassFundo_ValorLiquido(object data)
        {
            var rowOption = (FundoPosicao)data;
            if (rowOption.ValorLiquido < 0)
            {
                return "MinhaConta_Negativo";
            }
            else
            {
                return "";
            }
        }
    }
}