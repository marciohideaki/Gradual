using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.BMF.Dados
{
    /// <summary>
    /// Contem os tipos de instrumento BMF
    /// que o sistema trabalha
    /// </summary>
    public enum InstrumentoBMFTipoMercadoEnum
    {
        NaoInformado,
        Disponivel,
        Futuro,
        OpcaoDisponivel,
        OpcaoFuturo,
        Termo
    }
}
