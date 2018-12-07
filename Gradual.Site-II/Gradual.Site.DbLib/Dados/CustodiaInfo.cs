using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados
{
    public class CustodiaInfo
    {
        public string CodigoAtivo { get; set; }
        public string NomeAtivo { get; set; }
        public string Mercado { get; set; }
        public string TipoCarteira { get; set; }
        public string TipoGrupo { get; set; }
        public Nullable<decimal> QuantidadeAexecutarCompra { get; set; }
        public Nullable<decimal> QuantidadeAexecutarVenda { get; set; }
        public Nullable<decimal> QuantidadeTotal { get; set; }
        public Nullable<decimal> ValorCotacao { get; set; }
        public Nullable<decimal> ValorFinanceiro { get; set; }
        public Nullable<decimal> SaldoD1 { get; set; }
        public Nullable<decimal> SaldoD2 { get; set; }
        public Nullable<decimal> SaldoD3 { get; set; }
        public Nullable<decimal> ValorAtual { get; set; }
        public DateTime? DataVencimento { get; set; }
    }
}
