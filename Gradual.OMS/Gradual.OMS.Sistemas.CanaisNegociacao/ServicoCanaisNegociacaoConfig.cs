using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.CanaisNegociacao.Dados;

namespace Gradual.OMS.Sistemas.CanaisNegociacao
{
    /// <summary>
    /// Configuração de canais
    /// </summary>
    public class ServicoCanaisNegociacaoConfig
    {
        /// <summary>
        /// Lista de canais a serem criados pelo serviço
        /// </summary>
        public List<CanalInfo> Canais { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public ServicoCanaisNegociacaoConfig()
        {
            this.Canais = new List<CanalInfo>();
        }
    }
}
