using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_Test.Models
{
    public class SaldoCustodiaBovespaCliente
    {
        public string CodigoCarteira    { get; set; }
        
        public string Ativo             { get; set; }
        
        public decimal SaldoAbertura        { get; set; }
               
        public decimal SaldoD0              { get; set; }
               
        public decimal SaldoD1              { get; set; }
               
        public decimal SaldoD2              { get; set; }
               
        public decimal SaldoD3              { get; set; }
               
        public decimal SaldoTotal           { get; set; }
    }
}