using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Sistemas.Integracao.Sinacor
{
    /// <summary>
    /// Classe de configuração do serviço de integração sinacor
    /// </summary>
    [Serializable]
    public class ServicoIntegracaoSinacorConfig
    {
        /// <summary>
        /// String de conexão de acesso ao banco de dados do sinacor
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// String de conexão para acesso à conta margem
        /// </summary>
        public string ConnectionStringContaMargem { get; set; }
    }
}
