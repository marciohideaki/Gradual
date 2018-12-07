using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta
{
    public class FundoInfo
    {
        public string CodigoAnbima { get; set; }

        public int IdCliente { get; set; }

        public int CodigoFundo { get; set; }

        public string CodigoFundoItau { get; set; }

        public int Administrador { get; set; }

        public int TipoCarteira { get; set; }

        public int CGC { get; set; }

        public string NomeFundo { get; set; }

        public string Operacao { get; set; }

        public decimal? Cota { get; set; }

        public decimal? Quantidade { get; set; }

        public decimal? ValorBruto { get; set; }

        public decimal? IR { get; set; }

        public decimal? IOF { get; set; }

        public decimal? ValorLiquido { get; set; }

        public DateTime? DataInicioPesquisa { get; set; }

        public DateTime? DataFimPesquisa { get; set; }

        public DateTime? DataAtualizacao { get; set; }
    }
}
