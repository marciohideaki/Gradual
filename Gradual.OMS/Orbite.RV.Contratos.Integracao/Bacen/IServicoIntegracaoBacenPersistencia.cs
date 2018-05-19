using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.Integracao.Bacen
{
    public interface IServicoIntegracaoBacenPersistencia
    {
        void SalvarSerieLista();
        void SalvarSerie();
    }
}
