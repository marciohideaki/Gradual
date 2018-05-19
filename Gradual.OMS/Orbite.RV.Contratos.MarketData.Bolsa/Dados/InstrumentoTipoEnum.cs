using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Dados
{
    /// <summary>
    /// Contem os tipos de instrumento que o sistema trabalha.
    /// Os tipos Fundo e Titulo são sugestões de uso futuro.
    /// </summary>
    public enum InstrumentoTipoEnum
    {
        Desconhecido,
        Acao,
        Opcao,
        Termo,
        Futuro,
        Leilao,
        Fundo,
        Titulo
    }
}
