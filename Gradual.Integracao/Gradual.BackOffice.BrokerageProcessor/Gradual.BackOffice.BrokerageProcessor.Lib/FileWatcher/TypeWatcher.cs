using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.FileWatcher
{
    public class TypeWatcher
    {
        public const int UNDEFINED = 0;
        public const int BMF = 1;
        public const int BOVESPA = 2;
        public const int POSICAO_BMF = 3;

        //ATP 2015-11-27
        public const int COLD_BTC=4;
        public const int COLD_LIQ=5;
        public const int COLD_POS_CLI=6;
        public const int COLD_CUST=7;
        public const int COLD_MARG=8;
        public const int COLD_GART=9;
        public const int COLD_TERMO=10;
        public const int COLD_DIVIDENDO = 11;

    }
}
