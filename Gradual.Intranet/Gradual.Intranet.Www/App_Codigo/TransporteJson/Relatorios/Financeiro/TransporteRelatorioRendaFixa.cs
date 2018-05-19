using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro
{
    public class TransporteRelatorioRendaFixa
    {
        public string Titulo           { get; set; }
        public string Aplicacao        { get; set; }
        public string Vencimento       { get; set; }
        public string Taxa {get; set;}
        public string Quantidade       { get; set; }
        public string ValorOriginal    { get; set; }
        public string SaldoBruto       { get; set; }
        public string IRRF             { get; set; }
        public string IOF              { get; set; }
        public string SaldoLiquido     { get; set; }
        public string CodigoCliente    { get; set; }

        public List<TransporteRelatorioRendaFixa> TraduzirLista(List<RendaFixaInfo> lLista , Nullable<int> pVencimento)
        {
            var lRetorno = new List<TransporteRelatorioRendaFixa>();

            lLista.ForEach(renda => 
            {
                var lTrans = new TransporteRelatorioRendaFixa();

                if (pVencimento.HasValue && pVencimento.Value > 0)
                {
                    DateTime lDataSomada = DateTime.Now.AddDays(double.Parse(pVencimento.ToString()));

                    if (lDataSomada >= renda.Vencimento && DateTime.Today <= renda.Vencimento)
                    {
                        lTrans.CodigoCliente = renda.CodigoCliente.ToString();
                        lTrans.Titulo        = renda.Titulo;
                        lTrans.Aplicacao     = renda.Aplicacao.ToString("dd/MM/yyyy");
                        lTrans.Vencimento    = renda.Vencimento.ToString("dd/MM/yyyy");
                        lTrans.Taxa          = renda.Taxa.ToString("N4") + " %";
                        lTrans.Quantidade    = renda.Quantidade.ToString("N8");
                        lTrans.ValorOriginal = renda.ValorOriginal.ToString("N2");
                        lTrans.SaldoBruto    = renda.SaldoBruto.ToString("N2");
                        lTrans.IRRF          = renda.IRRF.ToString("N2");
                        lTrans.IOF           = renda.IOF.ToString("N2");
                        lTrans.SaldoLiquido  = renda.SaldoLiquido.ToString("N2");
                        lTrans.CodigoCliente = renda.CodigoCliente.ToString();

                        lRetorno.Add(lTrans);
                    }
                }
                else
                {

                    lTrans.CodigoCliente = renda.CodigoCliente.ToString();
                    lTrans.Titulo        = renda.Titulo;
                    lTrans.Aplicacao     = renda.Aplicacao.ToString("dd/MM/yyyy");
                    lTrans.Vencimento    = renda.Vencimento.ToString("dd/MM/yyyy");
                    lTrans.Taxa          = renda.Taxa.ToString("N4") + " %";
                    lTrans.Quantidade    = renda.Quantidade.ToString("N8");
                    lTrans.ValorOriginal = renda.ValorOriginal.ToString("N2");
                    lTrans.SaldoBruto    = renda.SaldoBruto.ToString("N2");
                    lTrans.IRRF          = renda.IRRF.ToString("N2");
                    lTrans.IOF           = renda.IOF.ToString("N2");
                    lTrans.SaldoLiquido  = renda.SaldoLiquido.ToString("N2");
                    lTrans.CodigoCliente = renda.CodigoCliente.ToString();
                    
                    lRetorno.Add(lTrans);

                }
                
            });

            return lRetorno;
        }
    }
}