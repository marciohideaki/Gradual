using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados.Fundos
{
    public class IntegracaoFundosIndexadorInfo
    {
        public IntegracaoFundosIndexadorInfo()
        {

        }
        public string Descricao             { get; set; }
        public string NomeIndexador         { get; set; }
        public int IdIndexador              { get; set; }
        public decimal RetornoAno           { get; set; }
        public decimal RetornoMes           { get; set; }
        public decimal Retorno12Meses       { get; set; }
        public decimal Retorno24Meses       { get; set; }
        public decimal Retorno36Meses       { get; set; }
        public decimal Volatidade           { get; set; }
        public decimal Sharpe               { get; set; }
        public decimal RentabilHistorica    { get; set; }
    }
}
