using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Monitores.Risco.Lib
{
     [Serializable]
    public class ClienteInfo
    {
        public string NomeCliente   { set; get; }
        public string Assessor      { set; get; }
        public string CodigoBMF     { set; get; }
        public string CodigoBovespa { set; get; }
    }
}
