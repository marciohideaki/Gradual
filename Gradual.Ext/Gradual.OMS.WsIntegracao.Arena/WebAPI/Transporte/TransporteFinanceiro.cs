using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.ContaCorrente.Lib;
using WebAPI_Test.Models;

namespace Gradual.OMS.WsIntegracao.Arena.Transporte
{
    public class TransporteFinanceiro
    {
        public static SaldoFinanceiroCliente TraduzirCustodiaInfo(ContaCorrenteInfo pParametros)
        {
            var lRetorno = new SaldoFinanceiroCliente();

            lRetorno.SaldoD0 = pParametros.SaldoD0;
            lRetorno.SaldoD1 = pParametros.SaldoD1;
            lRetorno.SaldoD2 = pParametros.SaldoD2;
            lRetorno.SaldoD3 = pParametros.SaldoD3;
            lRetorno.SaldoTotal = pParametros.SaldoD0 + pParametros.SaldoD1 + pParametros.SaldoD2 + pParametros.SaldoD3;

            return lRetorno;
        }
    }
}