using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Monitores.Risco.Enum
{
    [Serializable]
    public enum EnumProporcaoPrejuiso
    {
        SEMINFORMACAO = 0,
        ATE2K       = 10,
        MAIORQUE2K  = 11,
        MAIORQUE5K  = 5,
        MAIORQUE10K = 2,
        MAIORQUE15K = 6,
        MAIORQUE20K = 7
    }
}
