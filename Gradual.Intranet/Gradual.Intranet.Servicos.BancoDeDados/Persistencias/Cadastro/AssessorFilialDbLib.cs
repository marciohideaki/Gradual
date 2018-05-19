using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        public static ReceberObjetoResponse<AssessorFilialInfo> ReceberIdFilialDeAssessor(ReceberEntidadeRequest<AssessorFilialInfo> pParametros)
        {
            var lRetorno = new ReceberObjetoResponse<AssessorFilialInfo>();
            var lAcessaDados = new ConexaoDbHelper();
            lRetorno.Objeto = new AssessorFilialInfo();

            lAcessaDados.ConnectionStringName = gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_filial_por_assessor_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.Int32, pParametros.Objeto.ConsultaCdAssessor);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    lRetorno.Objeto = new AssessorFilialInfo()
                    {
                        CdAssessor = lDataTable.Rows[0]["id_assessor"].DBToInt32(),
                        CdFilial = lDataTable.Rows[0]["id_filial"].DBToInt32(),
                        DsFilial = lDataTable.Rows[0]["ds_filial"].DBToString(),
                    };
            }

            return lRetorno;
        }
    }
}
