using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados
{
    public class LojaInfo
    {
        public System.Int32  Codigo         { get; set; }
        public System.String NomeFantasia   { get; set; }
        public System.String RazaoSocial    { get; set; }
        public long CNPJ                    { get; set; }
        public System.String Endereco       { get; set; }
        public System.String Telefone       { get; set; }
    }
}
