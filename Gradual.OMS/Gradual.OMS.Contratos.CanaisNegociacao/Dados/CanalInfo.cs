using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.CanaisNegociacao.Dados
{
    /// <summary>
    /// Contem informações sobre um canal
    /// </summary>
    [Serializable]
    public class CanalInfo
    {
        /// <summary>
        /// Identificador do canal
        /// </summary>
        public string IdCanal { get; set; }

        /// <summary>
        /// Tipo da classe do canal
        /// </summary>
        public string TipoCanal { get; set; }
    }
}
