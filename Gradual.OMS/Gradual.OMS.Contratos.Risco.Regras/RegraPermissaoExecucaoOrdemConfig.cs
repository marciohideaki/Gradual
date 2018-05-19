using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Risco.Regras
{
    /// <summary>
    /// Classe de configurações para a regra de permissão de execução
    /// </summary>
    [Serializable]
    public class RegraPermissaoExecucaoOrdemConfig
    {
        /// <summary>
        /// A regra do assessor verifica se o CBLC da ordem solicitada tem como assessor o usuário
        /// que solicitou a execução.
        /// </summary>
        public string AplicarRegraAssessor { get; set; }

        /// <summary>
        /// A regra do usuário verifica se o CBLC da ordem solicitada é do usuário que solicitou a 
        /// execução.
        /// </summary>
        public string AplicarRegraUsuario { get; set; }
    }
}
