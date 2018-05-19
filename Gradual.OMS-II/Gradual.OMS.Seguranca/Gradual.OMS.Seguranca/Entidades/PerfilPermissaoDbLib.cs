using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    public class PerfilPermissaoDbLib : ItemPermissaoDbLibBase
    {
        public PerfilPermissaoDbLib()
        {
            base.NomeProcSel = "prc_PermissoesPorPerfil_sel";
            base.NomeProcIns = "prc_PerfisPermissoes_ins";
            base.NomeProcDel = "prc_PerfisPermissoes_del";
            base.DbLib = new DbLib("Seguranca");
        }
    }
}
