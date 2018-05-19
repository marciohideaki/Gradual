using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class IndicadoresInfo
    {
        public string CodigoIndicador { get; set; }

        public string Descricao { get; set; }

        public double Valores { get; set; }

        public double Volume { get; set; }

        public double Taxa { get; set; }

        public string Indice { get; set; }

        public int Quantidade { get; set; }

        public DateTime DataHora { get; set; }


    }
}
