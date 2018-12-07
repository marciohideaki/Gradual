using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class SaldoIntegracaoFundosInfo
    {
        public string ClienteCodigo { get; set; }

        public string ClienteTelefone { get; set; }

        public string ClienteNome { get; set; }

        public decimal SaldoDisponivel { get; set; }

        public decimal SaldoBloqueado { get; set; }

        public decimal SaldoTotal { get; set; }
    }
}
