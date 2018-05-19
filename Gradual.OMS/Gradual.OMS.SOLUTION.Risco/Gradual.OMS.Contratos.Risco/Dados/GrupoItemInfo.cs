using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Risco.Dados
{
    public class GrupoItemInfo
    {
        public int CodigoGrupoItem { get; set; }

        public string NomeGrupoItem { get; set; }

        public GrupoInfo Grupo { get; set; }

    }
}
