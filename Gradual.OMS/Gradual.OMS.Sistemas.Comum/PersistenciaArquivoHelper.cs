using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Classe de auxílio para manter os objetos que devem ser serializados
    /// na persistencia binária
    /// </summary>
    [Serializable]
    public class PersistenciaArquivoHelper
    {
        /// <summary>
        /// Lista de entidades carregadas
        /// </summary>
        public Dictionary<string, EntidadeInfo> Entidades { get; set; }

        /// <summary>
        /// Construtor default.
        /// </summary>
        public PersistenciaArquivoHelper()
        {
            this.Entidades = new Dictionary<string, EntidadeInfo>();
        }
    }
}
