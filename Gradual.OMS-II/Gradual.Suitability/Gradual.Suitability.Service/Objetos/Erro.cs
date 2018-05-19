using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Suitability.Service.Objetos
{
    public enum TipoErro
    {
        OperacaoBovespa,
        OperacaoBMF,
        Fundo
    }

    public class Erro
    {
        public TipoErro Tipo { get; set; }
        public string CodigoCliente { get; set; }
        public String Mensagem { get; set; }
    }
}
