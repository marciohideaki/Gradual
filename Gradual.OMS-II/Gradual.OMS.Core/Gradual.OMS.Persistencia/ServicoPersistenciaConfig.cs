using System.Collections.Generic;


namespace Gradual.OMS.Persistencia
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
