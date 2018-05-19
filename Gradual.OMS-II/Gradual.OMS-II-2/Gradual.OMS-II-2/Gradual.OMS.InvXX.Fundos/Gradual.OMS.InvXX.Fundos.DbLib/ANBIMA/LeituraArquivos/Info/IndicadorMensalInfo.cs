using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class IndicadorMensalInfo
    {
        public string CodigoIndicador { get; set; }

        public int DataMes { get; set; }

        public int DataAno { get; set; }

        public double Volume { get; set; }

        public string Indice { get; set; }

        public double Quantidade { get; set; }

        public DateTime DataHora { get; set; }
    }
}
