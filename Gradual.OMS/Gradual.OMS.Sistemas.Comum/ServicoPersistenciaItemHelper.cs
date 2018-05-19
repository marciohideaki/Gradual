using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Classe auxiliar para armazenar informações sobre um item de persistencia criado
    /// </summary>
    public class ServicoPersistenciaItemHelper
    {
        /// <summary>
        /// Informacoes da persistencia
        /// </summary>
        public PersistenciaInfo PersistenciaInfo { get; set; }

        /// <summary>
        /// Instancia da persistencia
        /// </summary>
        public IPersistencia Instancia { get; set; }
    }
}
