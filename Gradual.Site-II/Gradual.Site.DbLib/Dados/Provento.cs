using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados
{
    public class Provento
    {
        public Int32    CodigoCliente   { get; set; }
        public DateTime DataPagamento   { get; set; }
        public String   Evento          { get; set; }
        public String   Ativo           { get; set; }
        public Int32    Quantidade      { get; set; }
        public decimal  Valor           { get; set; }

        public Provento() { }

        public Provento(System.Data.DataRow pRow)
        {
            this.CodigoCliente = pRow["CD_CLIENTE"].DBToInt32();
            this.DataPagamento = pRow["DT_PAGAMENTO"].DBToDateTime();
            this.Evento        = pRow["TP_PROVENTO"].DBToString();
            this.Ativo         = pRow["DS_ATIVO"].DBToString();
            this.Quantidade    = pRow["VL_QUANTIDADE"].DBToInt32();
            this.Valor         = pRow["VL_VALOR"].DBToDecimal();
        }
    }
}
