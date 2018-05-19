using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.BMF.Dados
{
    /// <summary>
    /// Representa um instrumento do tipo opção.
    /// Adiciona os campos para informar o tipo da opção
    /// </summary>
    public class InstrumentoBMFOpcaoInfo : InstrumentoBMFInfo
    {
        /// <summary>
        /// Tipo da opção: compra ou venda
        /// </summary>
        public InstrumentoBMFOpcaoTipoEnum TipoOpcao { get; set; }

        /// <summary>
        /// Modelo da opção: americana ou europeia
        /// </summary>
        public InstrumentoBMFOpcaoModeloEnum ModeloOpcao { get; set; }
    }
}
