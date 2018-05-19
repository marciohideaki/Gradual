

namespace Gradual.OMS.Persistencia
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
