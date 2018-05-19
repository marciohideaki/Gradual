using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class TaxaAdministracaoInfo
    {
        public string CodigoFundo { get; set; }

        public DateTime DataInicio { get; set; }

        public double TaxaFixa { get; set; }

        public char CobraTaxaPerfomance { get; set; }

        public string TaxaPerfomance { get; set; }

        public string RegraTaxaPerformance { get; set; }

        public string TaxaEntrada { get; set; }

        public string TaxaSaida { get; set; }

        public int PeriodoCobTxPerf { get; set; }

        public char Unidade { get; set; }

        public char TaxaComposta { get; set; }

        public DateTime DataHora { get; set; }
    }
}
