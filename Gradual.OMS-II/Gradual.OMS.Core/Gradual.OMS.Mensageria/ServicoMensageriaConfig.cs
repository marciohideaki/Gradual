using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Mensageria
{
    /// <summary>
    /// Classe de configurações para o serviço de mensageria
    /// </summary>
    public class ServicoMensageriaConfig
    {
        /// <summary>
        /// Adiciona os tipos contidos nos namespaces informados
        /// </summary>
        public List<string> IncluirNamespaces { get; set; }

        /// <summary>
        /// Indica se a lista de namespaces devem ser batidas no primeiro nivel ou
        /// deve-se descer aos subniveis, se existirem
        /// </summary>
        public bool AprofundarNamespaces { get; set; }

        /// <summary>
        /// Adiciona os tipos informados
        /// </summary>
        public List<string> IncluirTipos { get; set; }

        /// <summary>
        /// Da lista final, exclui os tipos informados
        /// </summary>
        public List<string> ExcluirTipos { get; set; }

        /// <summary>
        /// Indica se as mensagens deverão ser validadas antes de ser repassadas 
        /// ao serviço correspondente
        /// </summary>
        public bool ValidarMensagens { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public ServicoMensageriaConfig()
        {
            this.IncluirNamespaces = new List<string>();
            this.IncluirTipos = new List<string>();
            this.ExcluirTipos = new List<string>();
            this.AprofundarNamespaces = true;
        }
    }
}
