using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta
{
    public class ClienteFundosInfo
    {
        public int IdCliente { get; set; }

        public string NomeFundo { get; set; }

        public decimal Cota { get; set; }

        public decimal Quantidade { get; set; }

        public decimal ValorBruto { get; set; }

        public decimal IR { get; set; }

        public decimal IOF { get; set; }

        public decimal ValorLiquido { get; set; }

        public DateTime DataInicioPesquisa { get; set; }

        public DateTime DataFimPesquisa { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
