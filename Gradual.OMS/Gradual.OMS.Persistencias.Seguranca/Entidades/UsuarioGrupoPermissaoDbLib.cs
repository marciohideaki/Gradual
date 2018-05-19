using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    public class UsuarioGrupoPermissaoDbLib : ItemPermissaoDbLibBase
    {
        public UsuarioGrupoPermissaoDbLib()
        {
            base.NomeProcSel = "prc_PermissoesPorGrupo_sel";
            base.NomeProcIns = "prc_GruposPermissoes_ins";
            base.NomeProcDel = "prc_GruposPermissoes_del";
            base.DbLib = new DbLib("Seguranca");
        }
   }
}
