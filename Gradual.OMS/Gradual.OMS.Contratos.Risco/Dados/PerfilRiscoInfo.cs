using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.OMS.Contratos.Risco.Dados
{
    /// <summary>
    /// Representa um perfil de risco que pode ser associado a um cliente.
    /// </summary>
    [Serializable]
    public class PerfilRiscoInfo : ICodigoEntidade
    {
        /// <summary>
        /// Código do perfil de risco
        /// </summary>
        public string CodigoPerfilRisco { get; set; }

        /// <summary>
        /// Nome do perfil de risco
        /// </summary>
        public string NomePerfilRisco { get; set; }

        /// <summary>
        /// Descrição do perfil de risco
        /// </summary>
        public string DescricaoPerfilRisco { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public PerfilRiscoInfo()
        {
            this.CodigoPerfilRisco = Guid.NewGuid().ToString();
        }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            return this.CodigoPerfilRisco;
        }

        #endregion
    }
}
