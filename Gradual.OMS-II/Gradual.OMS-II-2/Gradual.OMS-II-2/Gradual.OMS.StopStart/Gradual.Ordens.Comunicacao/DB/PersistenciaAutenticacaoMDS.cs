using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;

namespace Gradual.OMS.Ordens.Comunicacao.DB
{
    public class PersistenciaAutenticacaoMDS
    {
        #region ExcluirMDSAuthentication
        /// <summary>
        /// Exclui o registro com os guids de autenticação do MDS
        /// </summary>
        /// <param name="pIdCliente">Id do cliente a ser excluído</param>
        /// <param name="pIdSistema">Id do sistema a ser excluído</param>
        /// <returns>Retorna verdadeiro se conseguiu excluir com sucesso</returns>
        public bool ExcluirMDSAuthentication(int pIdCliente, int pIdSistema)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "MDS";

            int count = 0;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_del_mds_authentication"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, pIdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@idSistema", DbType.Int32, pIdSistema);

                count = lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            return (count > 0) ? true : false;
        }
        #endregion
    }
}
