using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info
{
    public class PLConsolidadoInstituicaoInfo
    {
        public string CodigoInstituicao { get; set; }

        public DateTime DataMes { get; set; }

        public DateTime DataAno { get; set; }

        public int CodigoTipo { get; set; }

        public double ValorPL { get; set; }

        
    }
}
