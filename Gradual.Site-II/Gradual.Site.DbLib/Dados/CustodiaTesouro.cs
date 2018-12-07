using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados
{
    public class CustodiaTesouro
    {
        public String   Instrumento     { get; set; }
        public decimal  Quantidade      { get; set; }
        public decimal  PrecoMedio      { get; set; }
        public decimal  Preco           { get; set; }
        public String   DataPosicao     { get; set; }
        public String   Emissor         { get; set; }
        public decimal  ValorPosicao    { get; set; }
        public String   IR              { get; set; }
        public String   IOF             { get; set; }
        public decimal  ValorLiquido    { get; set; }

        public CustodiaTesouro() { } 

        public CustodiaTesouro(System.Data.DataRow pRow)
        {
            this.Instrumento    = pRow["TIPO_TITU"].DBToString();
            this.DataPosicao    = pRow["DATA_COMPRA"].DBToString();
            this.Quantidade     = pRow["QTDE_TITU"].DBToDecimal();
            this.ValorPosicao   = pRow["VAL_POSI"].DBToDecimal();
            this.Preco          = this.ValorPosicao / this.Quantidade;
        }
    }
}
