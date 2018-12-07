using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta
{
    public class RendaFixaInfo 
    {
        public int CodigoCliente        { get; set; }
        public string Titulo            { get; set; }
        public string Emissor           { get; set; }
        public DateTime Aplicacao       { get; set; }
        public DateTime Vencimento      { get; set; }
        public decimal Taxa             { get; set; }
        public decimal Quantidade       { get; set; }
        public decimal ValorOriginal    { get; set; }
        public decimal SaldoBruto       { get; set; }
        public decimal IRRF             { get; set; }
        public decimal IOF              { get; set; }
        public decimal SaldoLiquido     { get; set; }
    }
}
