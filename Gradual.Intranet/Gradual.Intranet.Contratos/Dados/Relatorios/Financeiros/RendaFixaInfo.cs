using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class RendaFixaInfo : ICodigoEntidade
    {
        public Nullable<int> CodigoAssessor { get; set; }
        public Nullable<int> CodigoCliente  { get; set; }
        public string Titulo                { get; set; }
        public DateTime Aplicacao           { get; set; }
        public DateTime Vencimento          { get; set; }
        public decimal Taxa                 { get; set; }
        public decimal Quantidade           { get; set; }
        public decimal ValorOriginal        { get; set; }
        public decimal SaldoBruto           { get; set; }
        public decimal IRRF                 { get; set; }
        public decimal IOF                  { get; set; }
        public decimal SaldoLiquido         { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
