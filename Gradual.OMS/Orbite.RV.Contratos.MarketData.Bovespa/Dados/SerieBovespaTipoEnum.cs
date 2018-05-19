using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Dados
{
    /// <summary>
    /// Tipos de séries de bolsa.
    /// </summary>
    public enum SerieBovespaTipoEnum
    {
        Desconhecido,
        Desdobramento,
        Dividendo,
        Grupamento,
        JurosCapitalProprio,
        Rendimento, 
        Subscricao
    }
}
