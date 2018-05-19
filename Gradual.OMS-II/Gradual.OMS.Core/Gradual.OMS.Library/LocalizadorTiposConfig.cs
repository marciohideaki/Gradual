using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library
{
    /// <summary>
    /// Classe de configuração para o localizadorTiposHelper.
    /// Permite que seja associado tipos a serem trabalhados a um 
    /// tipo chamador.
    /// </summary>
    public class LocalizadorTiposConfig
    {
        /// <summary>
        /// Lista com os grupos chamadores
        /// </summary>
        public List<LocalizadorGrupoTipoInfo> Grupos { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public LocalizadorTiposConfig()
        {
            this.Grupos = new List<LocalizadorGrupoTipoInfo>();
        }
    }
}
