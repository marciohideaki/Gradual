using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados
{
    public class GarantiaBMF
    {
        public DateTime DataMovimento   { get; set; }
        public DateTime DataDeposito    { get; set; }
        public String   Descricao       { get; set; }
        public decimal  Valor           { get; set; }

        public GarantiaBMF() { }

        public GarantiaBMF(System.Data.DataRow pRow) 
        {
            this.DataMovimento  = pRow["DT_DATMOV"].DBToDateTime();
            this.DataDeposito   = pRow["DT_DATMOV"].DBToDateTime();
            this.Descricao      = pRow["DS_ATIVO"].DBToString();
            this.Valor          = pRow["VL_GARANTIA"].DBToDecimal(); 
        }
    }
}
