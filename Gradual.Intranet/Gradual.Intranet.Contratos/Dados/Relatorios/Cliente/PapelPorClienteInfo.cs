using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados
{
    public class PapelPorClienteInfo
    {
        public DateTime DataInicial         { get; set; }
        public DateTime DataFinal           { get; set; }

        public Nullable<int> CodigoCliente  { get; set; }
        public Nullable<int> CodigoAssessor { get; set; }
        public DateTime DataPregao          { get; set; }
        public string Papel                 { get; set; }
        public int QtdeCompras              { get; set; }
        public int QtdeVendas               { get; set; }
        public int QtdeLiquida              { get; set; }
        public decimal Preco                { get; set; }
        public decimal VolCompras           { get; set; }
        public decimal VolVendas            { get; set; }
        public decimal VolLiquido           { get; set; }
        public decimal VlNegocio            { get; set; }

        public int TotalQtdeCompras         { get; set; }
        public int TotalQtdeVendas          { get; set; }

        public decimal TotalVolVendas       { get; set; }
        public decimal TotalVolCompras      { get; set; }

        public string MostraTotal           { get; set; }
        public int Ordem { get; set; }

        public List<PapelPorClienteInfo> Resultado { get; set; }

        public int IdUsuarioLogado { get; set; }
        
        public PapelPorClienteInfo()
        {
            Resultado = new List<PapelPorClienteInfo>();
        }

        
    }
}
