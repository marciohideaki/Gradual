using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Monitores.Compliance
{
    public class OrdensAlteradasCabecalho
    {

        public int NumeroSeqOrdem { set; get; }

        public bool DayTrade { set; get; }

        public string TipoMercado { set; get; }

        public DateTime DataHoraOrdem { set; get; }

        public string Justificativa { set; get; }
    }
}
