using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Dados
{
    /// <summary>
    /// Contem os tipos de séries utilizados no sistema
    /// </summary>
    public enum SerieTipoEnum
    {
        Desconhecido,
        Outros,
        SerieGenerica,
        Negocios,
        FechamentoDiario,
        Desdobramento,
        Grupamento,
        JurosCapitalProprio,
        Rendimento,
        Dividendo,
        Noticias
    }
}
