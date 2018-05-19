using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.MarketData.Bolsa.Dados
{
    /// <summary>
    /// Complemento de informação de ativo para opção
    /// </summary>
    public class InstrumentoOpcaoInfo : InstrumentoInfo
    {
        /// <summary>
        /// Data de vencimento da opção
        /// </summary>
        public DateTime DataVencimento { get; set; }

        /// <summary>
        /// Preço de exercício da opção
        /// </summary>
        public double PrecoExercicio { get; set; }

        /// <summary>
        /// Instrumento referencia, no caso aponta para a ação referente
        /// </summary>
        public InstrumentoInfo InstrumentoBase { get; set; }

        /// <summary>
        /// Indica o tipo da opção: call, put
        /// </summary>
        public InstrumentoOpcaoTipoEnum TipoOpcao { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public InstrumentoOpcaoInfo()
        {
            this.Tipo = InstrumentoTipoEnum.Opcao;
        }

    }
}
