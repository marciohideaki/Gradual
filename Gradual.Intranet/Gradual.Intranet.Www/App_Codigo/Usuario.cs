using System;
using System.Collections.Generic;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;

namespace Gradual.Intranet
{
    public class Usuario
    {
        #region Propriedades

        public int Id                   { get; set; }
        
        public string Nome              { get; set; }

        public string EmailLogin        { get; set; }

        public bool EhAdministrador     { get; set; }

        public List<string> Permissoes  { get; set; }

        public List<string> Perfis      { get; set; }
        #endregion

        #region Métodos Públicos

        public bool VerificarPermissaoSimples(string pPermissao)
        {
            if (this.Permissoes == null)
            {
                this.Permissoes = new List<string>();

                AcessaDados lDados = new AcessaDados();

                lDados.ConnectionStringName = "Seguranca";

                using (DbCommand lCommand = lDados.CreateCommand(System.Data.CommandType.StoredProcedure, "prc_PermissoesPorUsuario_lst" ))
                {
                    lDados.AddInParameter(lCommand, "@CodigoUsuario", System.Data.DbType.String, this.Id.ToString());

                    DataTable lTable = lDados.ExecuteDbDataTable(lCommand);

                    foreach (DataRow lRow in lTable.Rows)
                    {
                        this.Permissoes.Add(lRow["CodigoPermissao"].ToString());
                    }
                }
            }

            return this.Permissoes.Contains(pPermissao);
        }

        #endregion
    }
}
