using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    public class UsuarioPerfilDbLib : ItemPerfilDbLibBase
    {
        public UsuarioPerfilDbLib()
        {
            base.DbLib = new DbLib("Seguranca");
            base.NomeProcDel = "prc_UsuariosPerfis_del";
            base.NomeProcIns = "prc_UsuariosPerfis_ins";
            base.NomeProcSel = "prc_PerfisPorUsuario_sel";
        }
    }
}
