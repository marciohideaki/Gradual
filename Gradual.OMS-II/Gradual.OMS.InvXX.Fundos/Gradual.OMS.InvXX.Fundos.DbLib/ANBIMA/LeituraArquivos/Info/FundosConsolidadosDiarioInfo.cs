using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class FundosConsolidadosDiarioInfo
    {
        public int CodigoTipo { get; set; }

        public DateTime Data { get; set; }

        public double ValorSomatorioPL { get; set; }

        public double RentabilidadeDia { get; set; }

        public double ValorPL { get; set; }

        public DateTime DataHora { get; set; } 
    }
}
