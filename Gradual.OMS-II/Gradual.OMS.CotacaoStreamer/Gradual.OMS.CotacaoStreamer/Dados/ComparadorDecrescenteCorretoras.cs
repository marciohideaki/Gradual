using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    public class ComparadorDecrescenteCorretoras : IComparer<CorretorasInfo>
    {
        public int Compare(CorretorasInfo a, CorretorasInfo b)
        {
            return -(a.CompareTo(b));
        }
    }
}
