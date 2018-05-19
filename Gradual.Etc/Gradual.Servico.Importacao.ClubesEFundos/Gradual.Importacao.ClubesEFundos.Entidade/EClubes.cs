using System;

namespace Gradual.MinhaConta.Entidade
{

    public class EClubes
    {
        public Nullable<int> Codigo_Bolsa { get; set; }
        public Nullable<int> Codigo_da_Empresa { get; set; }
        public string Nome_do_Clube { get; set; }
        public Nullable<DateTime> Data { get; set; }
        public Nullable<decimal> Cotacao { get; set; }
        public Nullable<Int64> Codigo_Cliente { get; set; }
        public string Nome_do_Cliente { get; set; }
        public Nullable<int> Dcquantidade { get; set; }
        public Nullable<int> Dccotacao { get; set; }
        public Nullable<DateTime> Data_Inicial { get; set; }
        public Nullable<decimal> Saldo_Quantidade { get; set; }
        public Nullable<decimal> Saldo_Bruto { get; set; }
        public Nullable<decimal> IR { get; set; }
        public Nullable<decimal> IOF { get; set; }
        public Nullable<decimal> Rendimento { get; set; }
        public Nullable<decimal> Performance { get; set; }
        public Nullable<decimal> Saldo_Liquido { get; set; }
        public Nullable<decimal> Saldo_Inicial { get; set; }
        public Nullable<int> Codigo_do_Agente { get; set; }
        public string Nome_do_Agente { get; set; }
        public Nullable<int> Tipo { get; set; }
        public string CPF_CGC { get; set; }
        public Nullable<DateTime> Data_Atualizacao { get; set; }
    }
}
