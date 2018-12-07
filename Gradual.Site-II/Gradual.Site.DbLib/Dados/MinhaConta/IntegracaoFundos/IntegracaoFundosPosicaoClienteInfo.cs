using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class IntegracaoFundosPosicaoClienteInfo
    {
        public int CodCliente { get; set; }

        public string NomeCliente { get; set; }

        public IntegracaoFundosInfo Fundo { get; set; }

        public decimal ValorCota { get; set; }

        public decimal QtdCotas { get; set; }

        public decimal ValorBruto { get; set; }

        public decimal IR { get; set; }

        public decimal IOF { get; set; }

        public decimal ValorLiquido { get; set; }

        public DateTime DataProcessamento { get; set; }

        public IntegracaoFundosPosicaoClienteInfo()
        {
            Fundo = new IntegracaoFundosInfo();
        }
    }
}
