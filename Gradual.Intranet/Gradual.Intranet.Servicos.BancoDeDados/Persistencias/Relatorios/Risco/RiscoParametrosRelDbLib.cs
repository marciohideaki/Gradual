using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Risco
{
    public partial class RiscoRelDbLib
    {
        #region | Métodos CRUD

        public ConsultarObjetosResponse<RiscoParametrosRelInfo> ConsultarRiscoParametros(ConsultarEntidadeRequest<RiscoParametrosRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoParametrosRelInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRisco;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_relatorio_parametro_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_bolsa", DbType.Int32, pParametros.Objeto.ConsultaIdBolsa);
                lAcessaDados.AddInParameter(lDbCommand, "@id_parametro", DbType.Int32, pParametros.Objeto.ConsultaIdParametro);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable) for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoParametrosRelInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private RiscoParametrosRelInfo CarregarEntidadeRiscoParametrosRelInfo(DataRow pLinha)
        {
            return new RiscoParametrosRelInfo() 
            {
                 DsBolsa = pLinha["ds_bolsa"].DBToString(),
                  DsParametro = pLinha["ds_parametro"].DBToString(),
            };
        }

        #endregion
    }
}
