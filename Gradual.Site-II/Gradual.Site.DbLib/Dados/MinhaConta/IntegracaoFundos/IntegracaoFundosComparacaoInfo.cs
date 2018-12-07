using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class IntegracaoFundosComparacaoInfo
    {
        public int IdProduto                { get; set; }

        public string NomeProduto           { get; set; }

        public decimal ROI                  { get; set; }

        public decimal Volatilidade         { get; set; }

        public decimal Rentabil12Meses      { get; set; }

        public decimal Rentabil24Meses      { get; set; }

        public decimal RentabilHistorica    { get; set; }

        public decimal CDI                  { get; set; }

        public decimal AplicacaoMinima      { get; set; }

        public decimal Sharpe               { get; set; }

        public decimal Variacao             { get; set; }

        public decimal Valor                { get; set; }
    }
}
