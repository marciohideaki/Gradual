using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Cadastro.Entidades
{
    public class EBensOutros
    {
        public int ID_BensOutros { get; set; }
        public int ID_Cliente { get; set; }
        public string Descricao { get; set; }
        public Nullable<decimal> Valor { get; set; }
        public int Tipo { get; set; }
    }
}
