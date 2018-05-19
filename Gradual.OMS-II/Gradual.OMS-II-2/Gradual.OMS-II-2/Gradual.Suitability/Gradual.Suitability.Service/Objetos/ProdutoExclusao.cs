using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Suitability.Service.Objetos
{
    public class ProdutoExclusao
    {
        public int Codigo { get; set; }
        public int CodigoProduto { get; set; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }
        public string Cnpj { get; set; }

        public ProdutoExclusao() { }
    }
}
