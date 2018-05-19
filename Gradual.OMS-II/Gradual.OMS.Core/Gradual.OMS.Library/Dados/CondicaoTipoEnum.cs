using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Lista de tipos de condições
    /// </summary>
    public enum CondicaoTipoEnum
    {
        Igual,
        Diferente,
        Maior,
        MaiorIgual,
        Menor,
        MenorIgual,
        FazParteDaLista,
        NaoFazParteDaLista
    }
}
