using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Cadastro.Entidades
{
    public class EBensImoveis
    {
        public int ID_BensImoveis { get; set; }
        public int ID_Cliente { get; set; }
        public string UF { get; set; }
        public string Cidade { get; set; }
        public string Endereco { get; set; }
        public Nullable<decimal> Valor { get; set; }
        public int Tipo { get; set; }
    }
}
