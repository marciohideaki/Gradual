using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Cadastro.Entidades
{
    public class EEmitente
    {
        public System.Nullable<int> ID_Emitente { get; set; }
        public System.Nullable<int> ID_Cliente { get; set; }
        public System.Nullable<char> Principal { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Identidade { get; set; }
        public string Sistema { get; set; }
    }
}
