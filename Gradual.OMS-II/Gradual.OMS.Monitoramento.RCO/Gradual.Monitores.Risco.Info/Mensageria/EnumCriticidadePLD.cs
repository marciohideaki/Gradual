using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Monitores.Risco.Enum
{
    [Serializable]
    public enum EnumCriticidadePLD
    {
        PLDAPROVADO = 0,
        CRITICO = 1,
        ALERTA = 2,
        FOLGA = 3,
    }
}
