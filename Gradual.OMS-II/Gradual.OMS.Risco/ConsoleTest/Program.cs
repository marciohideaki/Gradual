using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using Gradual.OMS.Contratos.Risco.Custodia.Mensageria;
using Gradual.OMS.Contratos.Risco.Custodia.Info;


namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        private static void DispararEventoBovespaBmf(object state) {
            System.Threading.Timer Timer = new System.Threading.Timer(new TimerCallback(ObterSaldoCustodia), state, 0, 2000);
        }

        private  void ObterSaldoCustodia(object state)
        {

            SaldoCustodiaRiscoRequest _SaldoRequest = new SaldoCustodiaRiscoRequest()
            {
                IdCliente = 31940
            };

            SaldoCustodiaRiscoResponse<CustodiaClienteInfo> resultData =
                new  Gradual.OMS.Risco.Custodia.ServicoRiscoCustodia().ObterCustodiaCliente(_SaldoRequest);


        }

    }
}
