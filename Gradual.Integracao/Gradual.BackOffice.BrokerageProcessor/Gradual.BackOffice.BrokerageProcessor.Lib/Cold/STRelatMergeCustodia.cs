using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.Cold
{
    public class STRelatMergeCustodia
    {
        public int CodContaBRP { get; set; }
        public int CodConta { get; set; }
        public string Carteira { get; set; }
        public string Papel { get; set; }

        public long SaldoCustodiaGRD { get; set; }
        public long BloqueioDepositoGRD { get; set; }
        public long LctoPrevDebitoGRD { get; set; }
        public long LctoPrevCreditoGRD { get; set; }

        public long SaldoCustodiaBRP { get; set; }
        public long BloqueioDepositoBRP { get; set; }
        public long LctoPrevDebitoBRP { get; set; }
        public long LctoPrevCreditoBRP { get; set; }
    }
}
