using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Monitores.Risco.Lib
{
    public class BTCInfo
    {
        public int CodigoCliente       { set; get; }

        public string Instrumento      { set; get; }

        public string TipoContrato     { set; get; }

        public DateTime DataAbertura   { set; get; }

        public DateTime DataVencimento { set; get; }

        public int Carteira            { set; get; }

        public int Quantidade          { set; get; }

        public decimal PrecoMedio      { set; get; }

        public decimal Taxa { set; get; }

        public decimal PrecoMercado    { set; get; }

        public decimal Remuneracao     { set; get; }


    }

}
