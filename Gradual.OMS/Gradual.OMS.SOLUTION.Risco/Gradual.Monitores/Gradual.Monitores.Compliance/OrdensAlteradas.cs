using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Monitores.Compliance
{
    public class OrdensAlteradas
    {
        public int      NumeroSeqOrdem { set; get; }
        public DateTime DataAlteracao { set; get; }
        public int      CodigoCliente { set; get; }
        public string   Instrumento { set; get; }
        public int      Quantidade { set; get; }
        public string   Sentido { set; get; }
        public string   TipoPessoa { set; get; }
        public bool     Vinculado { set; get; }
        public bool     ContaErro { set; get; }
        public decimal  DescontoCorretagem { set; get; }
        public string   Assessor { set; get; }
        public string   Usuario { set; get; }
        public string   UsuarioAlteracao { set; get; }
        
    }
}
