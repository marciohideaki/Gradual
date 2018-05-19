using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Bovespa.Mensagens
{
    /// <summary>
    /// Tipos de lista de instrumentos a serem retornados
    /// </summary>
    public enum ListarInstrumentosBovespaTipoListaEnum
    {
        Padrao,
        ApenasHabilitados,
        NegociadosNaDataReferencia,
        HistoricoCompleto
    }
}
