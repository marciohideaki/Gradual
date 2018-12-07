using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados
{
    public class ChamadaMargem
    {
        public DateTime DataReferencia  { get; set; }
        public Int32 CodigoCliente      { get; set; }
        public String Bolsa             { get; set; }
        public decimal ValorDebito      { get; set; }
        public decimal ValorCredito     { get; set; }
        public decimal ValorLancamento  { get; set; }

        public ChamadaMargem() { } 

        public ChamadaMargem(System.Data.DataRow pRow)
        {
            this.DataReferencia     = pRow["DataLancamento"].DBToDateTime();
            this.Bolsa              = pRow["Bolsa"].DBToString();
            this.CodigoCliente      = pRow["CodigoCliente"].DBToInt32();
            this.ValorDebito        = pRow["ValorDebito"].DBToDecimal();
            this.ValorCredito       = pRow["ValorCredito"].DBToDecimal();
            this.ValorLancamento    = pRow["ValorLancamento"].DBToDecimal();
        }
    }
}
