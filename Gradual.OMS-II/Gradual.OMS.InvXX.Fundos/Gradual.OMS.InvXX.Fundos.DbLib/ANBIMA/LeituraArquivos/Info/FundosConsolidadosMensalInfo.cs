using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class FundosConsolidadosMensalInfo
    {
        public int CodigoTipo { get; set; }

        public DateTime DataMes { get; set; }

        public DateTime DataAno { get; set; }

        public double ValorSomatorioPL { get; set; }

        public double RetabilidadeMes { get; set; }

        public double RentabilidadeAno { get; set; }

        public string NumeroFundos { get; set; }

        public DateTime DataHora { get; set; }

    }
}
