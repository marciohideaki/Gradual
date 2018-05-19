using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Contratos.Carteira.Dados
{
	/// <summary>
	/// Representa uma posição referente a um instrumento.
	/// </summary>
    public class PosicaoInstrumentoInfo : PosicaoInfo
    {
        #region Propriedades

        /// <summary>
        /// Indica o instrumento a que se refere esta posição.
        /// </summary>
        public InstrumentoInfo Instrumento { get; set; }

        /// <summary>
        /// Representa informações sobre o grupo de valores atuais
        /// </summary>
        public GrupoValoresInfo GrupoAtual { get; set; }

        /// <summary>
        /// Representa informações sobre o grupo de valores de carregamento
        /// </summary>
        public GrupoValoresInfo GrupoCarregamento { get; set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor default.
        /// </summary>
        public PosicaoInstrumentoInfo()
        {
            // Inicializa os objetos de grupo
            this.GrupoAtual = new GrupoValoresInfo();
            this.GrupoCarregamento = new GrupoValoresInfo();
        }

        #endregion
    }
}
