using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.CotacaoStreamer.Dados
{
    public class ComparadorDecrescente : IComparer<RankingInfo>
    {
        public int Compare(RankingInfo a, RankingInfo b)
        {
            return -(a.CompareTo(b));
        }
    }
}
