using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// Identifica uma combinação de Sistema + Bolsa + ClienteTipo a ser relacionada com um canal
    /// </summary>
    public class RelacaoBolsaCanalInfo
    {
        #region Chave Primaria

        /// <summary>
        /// Codigo que indica o tipo do Sistema Cliente (ex: home broker, plataforma mesa, robo, etc)
        /// Faz parte da primary key.
        /// </summary>
        public string CodigoSistema { get; set; }

        /// <summary>
        /// Código Interno de Bolsa
        /// Faz parte da primary key.
        /// </summary>
        public string CodigoBolsa { get; set; }

        /// <summary>
        /// Código que indica o tipo de cliente (ex: financeiro, não financeiro, institucional, etc)
        /// Faz parte da primary key.
        /// </summary>
        public string ClienteTipo { get; set; }

        #endregion

        /// <summary>
        /// Código do canal a ser utilizado por esta combinação de Sistema + Bolsa + ClienteTipo
        /// </summary>
        public string CodigoCanal { get; set; }
    }
}
