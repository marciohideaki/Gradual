using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_Test.Models
{
    public class PosicaoCliente
    {
        public int CodigoBovespaCliente                                 { get; set; }
        
        public int CodigoBmfCliente                                     { get; set; }
        
        public SaldoFinanceiroCliente SaldoFinanceiro                   { get; set; }
        
        public List<SaldoCustodiaBovespaCliente> SaldoCustodiaBovespaCliente  { get; set; }
        
        public List<SaldoCustodiaBmfCliente> SaldoCustodiaBmfCliente          { get; set; }
    }

    
}