using System;

namespace Gradual.MinhaConta.Entidade
{
    public class EFundos
    {
        public Nullable<int> Carteira { get; set; }

        public string NomeFundo { get; set; }

        public Nullable<int> Cliente { get; set; }

        public string NomeCliente { get; set; }

        public Nullable<decimal> Cota { get; set; }

        public Nullable<decimal> Quantidade { get; set; }

        public Nullable<decimal> ValorBruto { get; set; }

        public Nullable<decimal> IR { get; set; }

        public Nullable<decimal> IOF { get; set; }

        public Nullable<decimal> ValorLiquido { get; set; }

        public Nullable<DateTime> DataAtu { get; set; }

        public string CpfCnpj { get; set; }
    }
}
