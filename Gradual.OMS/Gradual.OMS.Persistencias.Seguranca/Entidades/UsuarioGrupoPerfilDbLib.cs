using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    public class UsuarioGrupoPerfilDbLib : ItemPerfilDbLibBase
    {
        public UsuarioGrupoPerfilDbLib()
        {
            base.DbLib = new DbLib("Seguranca");
            base.NomeProcDel = "prc_GruposPerfis_del";
            base.NomeProcIns = "prc_GruposPerfis_ins";
            base.NomeProcSel = "prc_PerfisPorGrupo_sel";
        }
    }
}
