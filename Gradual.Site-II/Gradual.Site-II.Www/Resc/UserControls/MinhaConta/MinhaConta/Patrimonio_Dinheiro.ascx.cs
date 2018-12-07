using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.Monitores.Risco.Info;
using Gradual.Site.Www.MinhaConta.Financeiro;
using Gradual.Site.DbLib.Dados;

namespace Gradual.Site.Www.Resc.UserControls.MinhaConta.MinhaConta
{
    public partial class Patrimonio_Dinheiro : ucMinhaContaBase
    {
        public Gradual.Site.Www.MinhaConta.Financeiro.MinhaContaCC CurrentMinhaConta        { get; set; }
        public FinanceiroExtratoInfo CurrentExtrato                                         { get; set; }
        //public List<Gradual.Site.Www.MinhaConta.Financeiro.Garantia> CurrentGarantiaBOV     { get; set; }
        //public List<Gradual.Site.Www.MinhaConta.Financeiro.GarantiaBMF> CurrentGarantiaBMF  { get; set; }
        public List<Gradual.Site.Www.MinhaConta.Financeiro.Garantia> CurrentGarantia        { get; set; }
        public MonitorLucroPrejuizoResponse CurrentLucroPrejuizo { get; set; }
        

        public decimal CurrentGarantiaBOVTotal          { get; set; }
        public decimal CurrentGarantiaBMFTotal          { get; set; }
        public decimal CurrentSaldoProjetado            { get; set; }
        public decimal CurrentOperacoesDia              { get; set; }
        public decimal CurrentTotalGeral                { get; set; }
        public decimal CurrentGarantiaGeral             { get; set; }

        public decimal CurrentOperacoesD0               { get; set; }
        public decimal CurrentOperacoesD1               { get; set; }
        public decimal CurrentOperacoesD2               { get; set; }
        public decimal CurrentOperacoesD3               { get; set; }
        
        public decimal OperacoesCompraOpcao             { get; set; }
        public decimal OperacoesVendaOpcao              { get; set; }
        public decimal OperacoesOpcaoTotal              { get; set; }
        public decimal OperacoesBMFLucroPrejuizo        { get; set; }
        public decimal PosicaoBMFLucroPrejuizo          { get; set; }
        public decimal OperacoesCompraVista             { get; set; }
        public decimal OperacoesVendaVista              { get; set; }

        public String CurrentGarantiaValor_Rotulo       { get; set; }
        public String CurrentGarantiaValorTotal_Rotulo  { get; set; }



        public String CurrentSaldoD0Valor_Rotulo        { get; set; }
        public String CurrentSaldoD1Valor_Rotulo        { get; set; }
        public String CurrentSaldoD2Valor_Rotulo        { get; set; }
        public String CurrentSaldoD3Valor_Rotulo        { get; set; }

        public String CurrentSaldoProjetadoValor_Rotulo { get; set; }
        public String CurrentOperacoesDiaValor_Rotulo   { get; set; }
        public String CurrentTotalValor_Rotulo          { get; set; }

        public decimal CurrentSaldoAbertura             { get; set; }

        public decimal CurrentSaldoD0                   { get; set; }
        public decimal CurrentSaldoD1                   { get; set; }
        public decimal CurrentSaldoD2                   { get; set; }
        public decimal CurrentSaldoD3                   { get; set; }

        public decimal CurrentSaldoFinalD0              { get; set; }
        public decimal CurrentSaldoFinalD1              { get; set; }
        public decimal CurrentSaldoFinalD2              { get; set; }
        public decimal CurrentSaldoFinalD3              { get; set; }

        public List<ChamadaMargem> CurrentChamadaMargem       { get; set; }

        DateTime CurrentDataD1 { get; set; }
        DateTime CurrentDataD2 { get; set; }
        DateTime CurrentDataD3 { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {


            CurrentGarantiaValor_Rotulo         = Resource.Garantia_Valor;
            CurrentGarantiaValorTotal_Rotulo    = Resource.Garantia_Total;
            CurrentSaldoD0Valor_Rotulo          = Resource.SaldoD0_Valor;
            CurrentSaldoD1Valor_Rotulo          = Resource.SaldoD1_Valor;
            CurrentSaldoD2Valor_Rotulo          = Resource.SaldoD2_Valor;
            CurrentSaldoD3Valor_Rotulo          = Resource.SaldoD3_Valor;
            CurrentSaldoProjetadoValor_Rotulo   = Resource.SaldoProjetado_Valor;
            CurrentOperacoesDiaValor_Rotulo     = Resource.OperacoesDia_Valor;
            CurrentTotalValor_Rotulo            = Resource.Total;

            CurrentSaldoAbertura                = (decimal)Session["SaldoAbertura"];
            CurrentMinhaConta                   = (Gradual.Site.Www.MinhaConta.Financeiro.MinhaContaCC)Session["MinhaContaCC"];
            CurrentGarantia                     = (List<Gradual.Site.Www.MinhaConta.Financeiro.Garantia>)Session["Garantia"];
            CurrentExtrato                      = (FinanceiroExtratoInfo)Session["Extrato"];
            CurrentLucroPrejuizo                = (MonitorLucroPrejuizoResponse)Session["LucroPrejuizo"];
            CurrentChamadaMargem                = (List<ChamadaMargem>)Session["ChamadaMargem"];

            CurrentDataD1                       = (DateTime)Session["DataD1"];
            CurrentDataD2                       = (DateTime)Session["DataD2"];
            CurrentDataD3                       = (DateTime)Session["DataD3"];

            CurrentSaldoD0                      = CurrentMinhaConta.SaldoD0_Valor - CurrentSaldoAbertura;
            CurrentSaldoD1                      = CurrentMinhaConta.SaldoD1_Valor;
            CurrentSaldoD2                      = CurrentMinhaConta.SaldoD2_Valor;
            CurrentSaldoD3                      = CurrentMinhaConta.SaldoD3_Valor;

            CurrentSaldoFinalD0                 = CurrentSaldoAbertura + CurrentSaldoD0;
            CurrentSaldoFinalD1                 = CurrentSaldoD1 + CurrentSaldoFinalD0;
            CurrentSaldoFinalD2                 = CurrentSaldoD2 + CurrentSaldoFinalD1;
            CurrentSaldoFinalD3                 = CurrentSaldoD3 + CurrentSaldoFinalD2;

            //decimal SubTotal1 = CurrentBTC.AsEnumerable().Sum(c => (c.PrecoMercado * c.Quantidade));

            if (CurrentMinhaConta != null)
            {
                CurrentSaldoProjetado = CurrentMinhaConta.SaldoD1_Valor + CurrentMinhaConta.SaldoD2_Valor + CurrentMinhaConta.SaldoD3_Valor;
            }

            if (CurrentMinhaConta != null)
            {
                CurrentTotalGeral = CurrentMinhaConta.SaldoD0_Valor + CurrentSaldoProjetado;
            }

            if (CurrentGarantia != null)
            {
                CurrentGarantiaGeral = CurrentGarantia.AsEnumerable().Sum(x => x.Valor);
            }

            if (CurrentLucroPrejuizo != null)
            {
                OperacoesCompraOpcao    = CurrentLucroPrejuizo.Monitor[0].Operacoes.AsEnumerable().Where(c => c.TipoMercado == "OPC" || c.TipoMercado == "OPV").Sum(c => c.VLNegocioCompra) * -1;
                OperacoesVendaOpcao     = CurrentLucroPrejuizo.Monitor[0].Operacoes.AsEnumerable().Where(c => c.TipoMercado == "OPC" || c.TipoMercado == "OPV").Sum(c => c.VLNegocioVenda);
              
                OperacoesBMFLucroPrejuizo = 0;
                PosicaoBMFLucroPrejuizo = 0;

                if (CurrentLucroPrejuizo.Monitor[0].OrdensBMF != null)
                {
                    OperacoesBMFLucroPrejuizo = CurrentLucroPrejuizo.Monitor[0].OrdensBMF.AsEnumerable().Sum(c => c.LucroPrejuizoContrato);
                }

                if (CurrentLucroPrejuizo.Monitor[0].LucroPrejuizoBMF != null)
                {
                    PosicaoBMFLucroPrejuizo = CurrentLucroPrejuizo.Monitor[0].LucroPrejuizoBMF;
                }

                OperacoesCompraVista        = CurrentLucroPrejuizo.Monitor[0].Operacoes.AsEnumerable().Where(c => c.TipoMercado == "VIS").Sum(c => c.VLNegocioCompra) * -1;
                OperacoesVendaVista         = CurrentLucroPrejuizo.Monitor[0].Operacoes.AsEnumerable().Where(c => c.TipoMercado == "VIS").Sum(c => c.VLNegocioVenda);
                CurrentOperacoesD1          = PosicaoBMFLucroPrejuizo + OperacoesBMFLucroPrejuizo + OperacoesCompraOpcao + OperacoesVendaOpcao;
                CurrentOperacoesD3          = OperacoesCompraVista + OperacoesVendaVista;
            }

            CurrentOperacoesDia = CurrentOperacoesD0 + CurrentOperacoesD1 + CurrentOperacoesD3;

            CurrentTotalGeral = CurrentTotalGeral + CurrentGarantiaGeral;

            if (!this.IsPostBack)
            {
                this.BindRepeaters();
            }
        }

        private void BindRepeaters()
        {
            rptGarantia.DataSource = CurrentGarantia;
            rptGarantia.DataBind();

            if (CurrentGarantia != null)
            {
                if (CurrentGarantia.Count <= 0)
                {
                    Control FooterTemplate = rptGarantia.Controls[rptGarantia.Controls.Count - 1].Controls[0];
                    FooterTemplate.FindControl("trEmpty").Visible = true;
                }
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