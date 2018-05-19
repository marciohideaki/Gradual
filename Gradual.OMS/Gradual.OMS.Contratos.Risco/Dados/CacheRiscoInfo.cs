using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.ContaCorrente.Dados;
using Gradual.OMS.Contratos.Custodia.Dados;

namespace Gradual.OMS.Contratos.Risco.Dados
{
    /// <summary>
    /// Mantem informações de cache para o cliente
    /// </summary>
    [Serializable]
    public class CacheRiscoInfo
    {
        /// <summary>
        /// Custodia do cliente
        /// </summary>
        public CustodiaInfo Custodia { get; set; }

        /// <summary>
        /// Conta corrente do cliente
        /// </summary>
        public ContaCorrenteInfo ContaCorrente { get; set; }

        /// <summary>
        /// Informações do usuário
        /// </summary>
        public UsuarioInfo Usuario { get; set; }
    }
}
