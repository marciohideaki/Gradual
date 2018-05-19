using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WebAPI_Test.Models
{
    public class PosicaoCliente
    {
        /// <summary>
        /// ID Cliente Gradual - Id Interno Usado nos sistemas gradual
        /// </summary>
        [JsonIgnore]
        public int IdClienteGradual                                     { get; set; }

        /// <summary>
        /// Codigo Bovespa Principal do Cliente
        /// </summary>
        public int CodigoBovespaCliente                                 { get; set; }
        
        /// <summary>
        /// Codigo Bmf do Cliente
        /// </summary>
        public int CodigoBmfCliente                                     { get; set; }
        
        /// <summary>
        /// Saldo Financeiro do Cliente Saldo D0 + Saldo D1 + Saldo D2 + Saldo D3 + Saldo Total
        /// </summary>
        public SaldoFinanceiroCliente SaldoFinanceiro                   { get; set; }
        
        /// <summary>
        /// Saldo de Custodia Bovespa do Cliente
        /// Codigo da Carteira
        /// Ativo
        /// Saldo Abertura
        /// Saldo D0
        /// Saldo D1
        /// Saldo D2
        /// Saldo D3
        /// Saldo Total
        /// </summary>
        public List<SaldoCustodiaBovespaCliente> SaldoCustodiaBovespaCliente  { get; set; }
        
        /// <summary>
        /// Saldo Custodia Bmf do Cliente
        /// Tipo de Mercadoria
        /// Serie 
        /// Ativo
        /// Quantidade Abertura
        /// Quantidade Comprado
        /// Quantidade Vendido
        /// Quantidade Atual
        /// PU
        /// Ajuste
        /// </summary>
        public List<SaldoCustodiaBmfCliente> SaldoCustodiaBmfCliente          { get; set; }
    }

    
}