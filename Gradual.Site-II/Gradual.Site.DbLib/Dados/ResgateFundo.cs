using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados
{
    public class ResgateFundo
    {
        public Int32 CodigoCliente      { get; set; }
        public Int32 IdCarteira         { get; set; }
        public DateTime DataAgendamento { get; set; }
        public DateTime DataLiquidacao  { get; set; }
        public Decimal ValorLiquido     { get; set; }

        public ResgateFundo() { }

        public ResgateFundo(System.Data.DataRow pRow)
        {
            this.CodigoCliente      = pRow["IdCotista"].DBToInt32();
            this.IdCarteira         = pRow["IdCarteira"].DBToInt32();
            this.DataAgendamento    = pRow["DataAgendamento"].DBToDateTime();
            this.DataLiquidacao     = pRow["DataLiquidacao"].DBToDateTime();
            this.ValorLiquido       = pRow["ValorLiquido"].DBToDecimal();
        }
    }
}
