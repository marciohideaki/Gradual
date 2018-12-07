using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Site.Www.MinhaConta.Financeiro;
using Gradual.Site.DbLib.Persistencias;
using Gradual.Site.DbLib;
using Gradual.Site.DbLib.Dados;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.MinhaConta
{
    public partial class Patrimonio_RendaVariavel : ucMinhaContaBase
    {
        public List<CustodiaAcao>   CurrentCustodiaAcao                 { get; set; }
        public List<CustodiaOpcao>  CurrentCustodiaOpcao                { get; set; }
        public List<CustodiaTermo>  CurrentCustodiaTermo                { get; set; }
        public List<CustodiaTermo>  CurrentCustodiaTermoALiquidar       { get; set; }
        public List<CustodiaFI>     CurrentCustodiaFundo                { get; set; }
        public List<CustodiaBTC>    CurrentBTC                          { get; set; }
        public List<Gradual.Site.DbLib.Dados.Provento> CurrentProventos { get; set; }

        public decimal CurrentCustodiaAcaoTotal                         { get; set; }
        public decimal CurrentCustodiaOpcaoTotal                        { get; set; }
        public decimal CurrentCustodiaTermoTotal                        { get; set; }
        public decimal CurrentCustodiaFundoTotal                        { get; set; }
        public decimal CurrentTotalGeral                                { get; set; }
        public decimal CurrentTotalBTC                                  { get; set; }
        public decimal CurrentTotalProventos                            { get; set; }

        public String CurrentCustodiaAcaoTotal_Rotulo                   { get; set; }
        public String CurrentCustodiaOpcaoTotal_Rotulo                  { get; set; }
        public String CurrentCustodiaTermoTotal_Rotulo                  { get; set; }
        public String CurrentCustodiaFundoTotal_Rotulo                  { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> lstFatorCotacao1000 = new List<string>();

            lstFatorCotacao1000.Add("CEGR3");
            lstFatorCotacao1000.Add("CAFE3");
            lstFatorCotacao1000.Add("CAFE4");
            lstFatorCotacao1000.Add("CBEE3");
            lstFatorCotacao1000.Add("SGEN4");
            lstFatorCotacao1000.Add("PMET6");
            lstFatorCotacao1000.Add("EBTP3");
            lstFatorCotacao1000.Add("EBTP4");
            lstFatorCotacao1000.Add("TOYB3");
            lstFatorCotacao1000.Add("TOYB4");
            lstFatorCotacao1000.Add("FNAM11");
            lstFatorCotacao1000.Add("FNOR11");
            lstFatorCotacao1000.Add("NORD3");

            CurrentCustodiaAcaoTotal_Rotulo     = Resource.Custodia_RendaVariavelTotal;
            CurrentCustodiaOpcaoTotal_Rotulo    = Resource.Custodia_OpcoesTotal;
            CurrentCustodiaTermoTotal_Rotulo    = Resource.Custodia_TermoTotal;
            CurrentCustodiaFundoTotal_Rotulo    = Resource.Custodia_FundoTotal;

            CurrentCustodiaAcao                 = (List<CustodiaAcao>)Session["CustodiaAcao"];
            CurrentCustodiaOpcao                = (List<CustodiaOpcao>)Session["CustodiaOpcao"];
            CurrentCustodiaTermo                = (List<CustodiaTermo>)Session["CustodiaTermo"];
            CurrentCustodiaTermoALiquidar       = (List<CustodiaTermo>)Session["CustodiaTermoALiquidar"];
            CurrentCustodiaFundo                = (List<CustodiaFI>)Session["CustodiaFundo"];
            CurrentBTC                          = (List<CustodiaBTC>)Session["CustodiaBTC"];
            CurrentProventos                    = (List<Gradual.Site.DbLib.Dados.Provento>)Session["Proventos"];


            CurrentTotalBTC = 0;

            if (CurrentBTC != null)
            {
                if (CurrentBTC.Count > 0)
                {
                    //decimal SubTotal1 = CurrentBTC.AsEnumerable().Where(c => lstFatorCotacao1000.Contains(c.CodigoNegocio)).Sum(c => (c.PrecoMercado * c.Quantidade) / 1000);
                    //decimal SubTotal2 = CurrentBTC.AsEnumerable().Where(c => !lstFatorCotacao1000.Contains(c.CodigoNegocio)).Sum(c => (c.PrecoMercado * c.Quantidade));
                    //CurrentTotalBTC = SubTotal1 + SubTotal2;

                    CurrentTotalBTC = CurrentBTC.AsEnumerable().Sum(c => (c.Financeiro));
                }
            }

            if (CurrentCustodiaAcao != null)
            {
                CurrentCustodiaAcaoTotal = CurrentCustodiaAcao.AsEnumerable().Sum(x => x.ValorPosicao);
            }

            if (CurrentCustodiaOpcao != null)
            {
                CurrentCustodiaOpcaoTotal = CurrentCustodiaOpcao.AsEnumerable().Sum(x => x.ValorPosicao);
            }

            if (CurrentCustodiaTermo != null)
            {
                CurrentCustodiaTermoTotal = CurrentCustodiaTermo.AsEnumerable().Sum(x => x.ResultadoTermo);
            }

            if (CurrentCustodiaFundo != null)
            {
                CurrentCustodiaFundoTotal = CurrentCustodiaFundo.AsEnumerable().Sum(x => x.ValorPosicao);
            }

            if (CurrentBTC != null)
            {
                CurrentTotalBTC = CurrentBTC.AsEnumerable().Sum(x => x.Financeiro);
            }

            if (CurrentProventos != null)
            {
                CurrentTotalProventos = CurrentProventos.AsEnumerable().Sum(x => x.Valor);
            }


            CurrentTotalGeral =   (CurrentCustodiaAcaoTotal     != null ? CurrentCustodiaAcaoTotal  : 0)
                                + (CurrentCustodiaOpcaoTotal    != null ? CurrentCustodiaOpcaoTotal : 0)
                                + (CurrentCustodiaTermoTotal    != null ? CurrentCustodiaTermoTotal : 0)
                                + (CurrentCustodiaFundoTotal    != null ? CurrentCustodiaFundoTotal : 0)
                                + (CurrentTotalBTC              != null ? CurrentTotalBTC           : 0)
                                + (CurrentTotalProventos        != null ? CurrentTotalProventos     : 0);

            this.BindRepeaters();
        }

        private void BindRepeaters()
        {
            rptCustodiaAcao.DataSource = CurrentCustodiaAcao;
            rptCustodiaAcao.DataBind();

            rptCustodiaOpcao.DataSource = CurrentCustodiaOpcao;
            rptCustodiaOpcao.DataBind();

            rptCustodiaTermo.DataSource = CurrentCustodiaTermo;
            rptCustodiaTermo.DataBind();

            rptCustodiaFundo.DataSource = CurrentCustodiaFundo;
            rptCustodiaFundo.DataBind();

            rptBTC.DataSource = CurrentBTC;
            rptBTC.DataBind();

            rptProventos.DataSource = CurrentProventos != null ? CurrentProventos : new List<Gradual.Site.DbLib.Dados.Provento>();
            rptProventos.DataBind();

            if (CurrentCustodiaAcao != null)
            {
                if (CurrentCustodiaAcao.Count <= 0)
                {
                    Control FooterTemplate = rptCustodiaAcao.Controls[rptCustodiaAcao.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
            }

            if (CurrentCustodiaOpcao != null)
            {
                if (CurrentCustodiaOpcao.Count <= 0)
                {
                    Control FooterTemplate = rptCustodiaOpcao.Controls[rptCustodiaOpcao.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
            }

            if (CurrentCustodiaTermo != null)
            {
                if (CurrentCustodiaTermo.Count <= 0)
                {
                    Control FooterTemplate = rptCustodiaTermo.Controls[rptCustodiaTermo.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
            }

            if (CurrentCustodiaFundo != null)
            {
                if (CurrentCustodiaFundo.Count <= 0)
                {
                    Control FooterTemplate = rptCustodiaFundo.Controls[rptCustodiaFundo.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
            }

            if (CurrentBTC != null)
            {
                if (CurrentBTC.Count <= 0)
                {
                    Control FooterTemplate = rptBTC.Controls[rptBTC.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
            }

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

        protected string GetClassCustodiaAcao_ValorPosicao(object data)
        {
            var rowOption = (CustodiaAcao)data;
            if (rowOption.ValorPosicao < 0)
            {
                return "MinhaConta_Negativo";
            }
            else
            {
                return "";
            }
        }

        protected string GetClassCustodiaOpcao_ValorPosicao(object data)
        {
            var rowOption = (CustodiaOpcao)data;
            if (rowOption.ValorPosicao < 0)
            {
                return "MinhaConta_Negativo";
            }
            else
            {
                return "";
            }
        }

        protected string GetClassCustodiaBTC_ValorPosicao(object data)
        {
            var rowOption = (CustodiaBTC)data;
            if (rowOption.Financeiro < 0)
            {
                return "MinhaConta_Negativo";
            }
            else
            {
                return "";
            }
        }

        protected string GetClassCustodiaTermo_ValorPosicao(object data)
        {
            var rowOption = (CustodiaTermo)data;
            if (rowOption.Financeiro < 0)
            {
                return "MinhaConta_Negativo";
            }
            else
            {
                return "";
            }
        }

        protected string GetClassCustodiaFI_ValorPosicao(object data)
        {
            var rowOption = (CustodiaFI)data;
            if (rowOption.ValorPosicao < 0)
            {
                return "MinhaConta_Negativo";
            }
            else
            {
                return "";
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
    }
}