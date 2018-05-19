using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Cadastro.Entidades
{
    public class EClienteSistema
    {
        public System.Nullable<int> ID_Cliente { get; set; }
        public string Conta { get; set; }
        public System.Nullable<char> Principal { get; set; }
        public System.Nullable<int> ID_TipoConta { get; set; }
        public System.Nullable<int> ID_Sistema { get; set; }
        public System.Nullable<int> ID_ClienteSistema { get; set; }
        public System.Nullable<char> Ativa { get; set; }
        public System.Nullable<int> AssessorSinacor { get; set; }
    }
}


