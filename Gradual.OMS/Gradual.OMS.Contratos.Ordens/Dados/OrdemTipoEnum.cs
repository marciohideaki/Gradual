using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// Enumerador de tipos de ordens
    /// </summary>
    public enum OrdemTipoEnum
    {
        NaoInformado,
        NaoImplementado,
        Limitada,
        MarketWithLeftOverLimit,
        StopLimitada,
        Mercado,
        OnClose
    }
}
