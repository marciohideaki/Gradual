using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Sistemas.Comum
{
    /// <summary>
    /// Classe de configuração do serviço de persistencia
    /// </summary>
    public class ServicoPersistenciaConfig
    {
        public List<PersistenciaInfo> Persistencias { get; set; }

        public ServicoPersistenciaConfig()
        {
            this.Persistencias = new List<PersistenciaInfo>();
        }
    }
}
