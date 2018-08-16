using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class Transporte_PosicaoCotistaRebate
    {
        public string CodigoCliente     { get; set; }

        public string NomeCliente       { get; set; }

        public string CodigoAssessor    { get; set; }

        public string NomeFundo         { get; set; }

        public string CodigoAnbima      { get; set; }

        public string ValorAplicacao    { get; set; }

        public string DataAplicacao     { get; set; }

        public string ValorRepasse      { get; set; }

        private string CalculoRepasse(string CodigoAnbima, decimal pValorBruto, DateTime pDataInicial, DateTime pDataFinal)
        {
            string lRetorno = string.Empty;

            double lTaxaFundosAnual = 0.0D;

            double lTaxaFundosDia = 0.0D;

            double lTaxaRepasse = 0.0D;

            double lValorAdminDia = 0.0D;

            double lValorRepasse = 0.0D;

            double Potencia = 1.0D / 252.0D;

            lTaxaFundosAnual = ClienteDbLib.GetTaxaAdminFundo(CodigoAnbima).DadosMovimentacao.VlTaxaAdmin.DBToDouble();

            lTaxaFundosDia = Math.Pow((1 + lTaxaFundosAnual / 100), Potencia) - 1;

            lTaxaRepasse = ClienteDbLib.GetFundoFinancialRepate(CodigoAnbima).DadosMovimentacao.VlTaxaRepasse.DBToDouble();

            lTaxaRepasse = (lTaxaRepasse / 100);

            lValorAdminDia = lTaxaFundosDia * pValorBruto.DBToDouble();

            lValorRepasse = lValorAdminDia * lTaxaRepasse;

            int lDias = pDataInicial.Date.Subtract(pDataFinal).Days;

            if (lDias == 0)
            {
                lDias = 1;
            }

            lRetorno = ( Math.Abs(lDias) * lValorRepasse).ToString("N2");

            return lRetorno;
        }

        public List<Transporte_PosicaoCotistaRebate> TraduzirLista(List<Transporte_PosicaoCotista> pListaPosicao, ClienteResumidoInfo pCliente, DateTime DataInicial, DateTime DataFinal )
        {
            var lRetorno = new List<Transporte_PosicaoCotistaRebate>();

            pListaPosicao.ForEach(posicao => 
            {
                var lDataAplicacao = posicao.DataAplicacao.DBToDateTime();

                //if (DataInicial <= lDataAplicacao && lDataAplicacao <= DataFinal )
                //{
                    var lTrans = new Transporte_PosicaoCotistaRebate();

                    lTrans.CodigoCliente = pCliente.CodBovespa;

                    lTrans.NomeCliente = pCliente.NomeCliente;

                    lTrans.CodigoAssessor = pCliente.CodAssessor.Value.ToString();

                    lTrans.NomeFundo = posicao.NomeFundo;

                    lTrans.CodigoAnbima = posicao.CodigoAnbima;

                    lTrans.ValorAplicacao = posicao.ValorLiquido;

                    lTrans.DataAplicacao = posicao.DataAtualizacao;

                    lTrans.ValorRepasse = this.CalculoRepasse(posicao.CodigoAnbima, decimal.Parse(posicao.ValorLiquido), DataInicial, DataFinal);

                    lRetorno.Add(lTrans);
                //}
            });

            return lRetorno;
        }

        
    }
}