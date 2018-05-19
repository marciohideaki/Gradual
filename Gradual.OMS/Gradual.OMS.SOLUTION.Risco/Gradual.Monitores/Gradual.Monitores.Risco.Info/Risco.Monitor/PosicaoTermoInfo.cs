using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Monitores.Risco.Lib
{
    public class PosicaoTermoInfo
    {

        public int IDCliente { set; get; }

        public string Instrumento { set; get; }

        public DateTime DataExecucao { set; get; }

        public DateTime DataVencimento { set; get; }

        public int Quantidade { set; get; }

        public decimal PrecoExecucao { set; get; }

        public decimal PrecoMercado { set; get; }

        public decimal LucroPrejuizo { set; get; }

    }
}
