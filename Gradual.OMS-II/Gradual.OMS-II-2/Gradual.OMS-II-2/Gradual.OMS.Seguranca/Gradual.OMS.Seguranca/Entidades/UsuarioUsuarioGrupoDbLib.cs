using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    public class UsuarioUsuarioGrupoDbLib : ItemGrupoDbLibBase
    {
        public UsuarioUsuarioGrupoDbLib()
        {
            base.DbLib = new DbLib("Seguranca");
            base.NomeProcDel = "prc_UsuariosGrupos_del";
            base.NomeProcSel = "prc_GruposPorUsuario_sel";
            base.NomeProcIns = "prc_UsuariosGrupos_ins";
        }
    }
}
