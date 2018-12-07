using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Mensagens;

namespace Gradual.Site.DbLib.Dados.MinhaConta
{
    [Serializable]
    [DataContract]
    public class ContaBancariaInfo
    {
        public int CodigoDaEmpresa { get; set; }

        public int CodigoCliente { get; set; }

        public string NumeroDaAgencia { get; set; }
        public string DigitoDaAgencia { get; set; }

        public string NumeroDaConta { get; set; }
        public string DigitoDaConta { get; set; }

        public bool Principal { get; set; }

        public string NomeDoBanco { get; set; }

        public string CodigoDoBanco { get; set; }
    }
}
