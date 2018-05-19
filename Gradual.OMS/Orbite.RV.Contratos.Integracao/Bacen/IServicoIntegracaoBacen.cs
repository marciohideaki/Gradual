using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Orbite.RV.Contratos.Integracao.Bacen
{
    public interface IServicoIntegracaoBacen
    {
        DsBCB ReceberSerieLista();
        ListaSeriesProgress ReceberSerieLista(DsBCB ds);
        DataTable ReceberSerie(long[] series, DateTime dataInicial, DateTime dataFinal);
    }
}
