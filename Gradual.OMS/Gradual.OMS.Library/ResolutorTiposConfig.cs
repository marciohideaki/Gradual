using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Classe de configuração para o resolutor de tipos
    /// </summary>
    [Serializable]
    public class ResolutorTiposConfig
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
        /// Construtor default
        /// </summary>
        public ResolutorTiposConfig()
        {
            this.IncluirNamespaces = new List<string>();
            this.IncluirTipos = new List<string>();
            this.ExcluirTipos = new List<string>();
            this.AprofundarNamespaces = true;
        }
    }
}
