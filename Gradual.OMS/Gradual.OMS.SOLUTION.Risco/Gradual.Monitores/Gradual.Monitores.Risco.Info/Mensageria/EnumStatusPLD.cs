using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Monitores.Risco.Enum
{
    [Serializable]
    public enum EnumStatusPLD
    {
        COMPLETO   = 0,
        APROVADO   = 1 ,
        REJEITADO  = 2 ,
        EMANALISE  = 3        
    }
}
