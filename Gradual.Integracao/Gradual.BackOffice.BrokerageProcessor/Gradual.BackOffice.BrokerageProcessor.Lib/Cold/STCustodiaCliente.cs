using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.Cold
{
    public class STCustodiaCliente
    {
        public int CodBolsa { get; set; }
        public string Carteira { get; set; }
        public string Papel { get; set; }
        public string ISIN { get; set; }
        public string Situacao { get; set; }
        public long SaldoCustodia { get; set; }
        public long BloqueioDeposito { get; set; }

        public Dictionary<string, STLancamentoPrevisto> Lancamentos { get; set; }

        public STCustodiaCliente()
        {
            Lancamentos = new Dictionary<string, STLancamentoPrevisto>();
        }
    }

    public class STLancamentoPrevisto
    {
        public string DataPrevisao { get; set; }
        public long LctoPrevDebito { get; set; }
        public long LctoPrevCredito { get; set; }
    }
}
