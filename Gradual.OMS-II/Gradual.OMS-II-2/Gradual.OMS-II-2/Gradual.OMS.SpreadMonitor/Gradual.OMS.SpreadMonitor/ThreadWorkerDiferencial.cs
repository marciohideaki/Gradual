using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.SpreadMonitor
{
    public class ThreadWorkerDiferencial : ThreadWorkerBase
    {
        protected override int getAlgoritmosPorThread()
        {
            throw new NotImplementedException();
        }

        public override Lib.Dados.AlgoStruct DoAlgoritmo(Lib.Dados.AlgoStruct algo, Lib.Dados.CotacaoInfo cotacao)
        {
            throw new NotImplementedException();
        }
    }
}
