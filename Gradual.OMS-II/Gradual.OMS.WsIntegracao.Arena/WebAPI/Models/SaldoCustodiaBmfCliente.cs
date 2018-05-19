using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WebAPI_Test.Models
{
    public class SaldoCustodiaBmfCliente
    {
        public string TipoMercadoria    { get; set; }
        
        public string Serie             { get; set; }
        
        public string Ativo             { get; set; }
        
        public decimal QuantidadeAbertura   { get; set; }
        
        public decimal QuantidadeComprado   { get; set; }
        
        public decimal QuantidadeVendido    { get; set; }
        
        public decimal QuantidadeAtual      { get; set; }
        
        public decimal PU               { get; set; }
        
        public decimal Ajuste           { get; set; }

        [JsonIgnore]
        public bool EhPosicao           { get; set; }
    }
}