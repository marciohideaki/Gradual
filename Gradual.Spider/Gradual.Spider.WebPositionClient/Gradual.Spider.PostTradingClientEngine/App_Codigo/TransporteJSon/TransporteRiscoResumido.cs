using Gradual.Spider.SupervisorRisco.Lib.Dados;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Gradual.Spider.PostTradingClientEngine.App_Codigo.TransporteJSon
{
    /// <summary>
    /// Classe de transporte da grid de risco resumido
    /// </summary>
    public class TransporteRiscoResumido
    {
        private CultureInfo gCultura = new CultureInfo("pt-BR");

        public string CodigoCliente     { get; set; }
        public string CustodiaAbertura  { get; set; }
        public string CCAbertura        { get; set; }
        public string Garantias         { get; set; }
        public string Produtos          { get; set; }
        public string TotalAbertura     { get; set; }
        public string PLBovespa         { get; set; }
        public string PLBmf             { get; set; }
        public string PLTotal           { get; set; }
        public string SFP               { get; set; }
        public string PercAtingido      { get; set; }

        public List<TransporteRiscoResumido> ListaTransporte = new List<TransporteRiscoResumido>();

        public TransporteRiscoResumido() { }

        public TransporteRiscoResumido(List<ConsolidatedRiskInfo> pInfo)
        {
            foreach (ConsolidatedRiskInfo info in pInfo )
            {
                var lTrans = new TransporteRiscoResumido();

                lTrans.CodigoCliente    = info.Account.ToString();
                lTrans.CustodiaAbertura = info.TotalCustodiaAbertura.ToString("n2", gCultura);
                lTrans.CCAbertura = info.TotalContaCorrenteAbertura.ToString("n2", gCultura);
                lTrans.Garantias = info.TotalGarantias.ToString("n2", gCultura);
                lTrans.Produtos = info.TotalProdutos.ToString("n2", gCultura);
                lTrans.TotalAbertura = info.SaldoTotalAbertura.ToString("n2", gCultura);
                lTrans.PLBovespa = info.PLBovespa.ToString("n2", gCultura);
                lTrans.PLBmf = info.PLBmf.ToString("n2", gCultura);
                lTrans.PLTotal = info.PLTotal.ToString("n2", gCultura);
                lTrans.SFP = info.SFP.ToString("n2", gCultura);
                lTrans.PercAtingido = info.TotalPercentualAtingido.ToString("n2", gCultura);

                ListaTransporte.Add(lTrans);
            }
        }
    }
}