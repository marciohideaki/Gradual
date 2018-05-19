using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Risco.Dados;

namespace Gradual.OMS.Contratos.Risco.Regras
{
    /// <summary>
    /// Classe de configuração para a regra de limites de valor por operacao
    /// </summary>
    [Serializable]
    public class RegraLimitesConfig
    {
        /// <summary>
        /// Informacoes do limite a regra validar
        /// </summary>
        public LimiteInfo Limite { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public RegraLimitesConfig()
        {
            this.Limite = new LimiteInfo();
        }
    }
}
