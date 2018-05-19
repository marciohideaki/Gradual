using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Dados;
using System.Data;


namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    public class UsuarioPermissaoDbLib : ItemPermissaoDbLibBase
    {
        public UsuarioPermissaoDbLib()
        {
            base.NomeProcSel = "prc_PermissoesPorUsuario_sel";
            base.NomeProcIns = "prc_UsuariosPermissoes_ins";
            base.NomeProcDel = "prc_UsuariosPermissoes_del";
            base.DbLib = new DbLib("Seguranca");
        }
    }
}
