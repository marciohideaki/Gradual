using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Suitability.Service.Objetos
{
    public class Fundo
    {
        public int CodigoProduto    { get; set; }
        public int CodigoANBIMA     { get; set; }
        public int CodigoItau       { get; set; }
        public int CodigoFinancial  { get; set; }
        public string Nome          { get; set; }
        public string Cnpj          { get; set; }
        public string RiscoInvix    { get; set; }
        public string Perfil        { get; set; }
    }
}
