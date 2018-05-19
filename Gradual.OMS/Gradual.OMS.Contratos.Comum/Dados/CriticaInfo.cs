using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Comum.Dados
{
    /// <summary>
    /// Representa uma crítica na validação da mensagem.
    /// A crítica pode ser apenas informativa ou pode ser um erro na validação.
    /// </summary>
    [Serializable]
    public class CriticaInfo
    {
        /// <summary>
        /// Descricao da crítica
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Informa um status da crítica
        /// </summary>
        public CriticaStatusEnum Status { get; set; }
    }
}
