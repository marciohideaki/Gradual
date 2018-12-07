using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados
{
    public class Garantia
    {
        public DateTime DataMovimento   { get; set; }
        public DateTime DataDeposito    { get; set; }
        public String   Descricao       { get; set; }
        public decimal  Valor           { get; set; }

        public Garantia() { }

        public Garantia(System.Data.DataRow pRow) 
        {
            this.DataMovimento  = pRow["DATA_MVTO"].DBToDateTime();
            this.DataDeposito   = pRow["DATA_DEPO"].DBToDateTime();
            this.Descricao      = pRow["DESC_FINL_DINH"].DBToString();
            this.Valor          = pRow["VAL_GARN_DEPO"].DBToDecimal(); 
        }
    }
}
