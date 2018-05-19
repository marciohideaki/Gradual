using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco
{
    public partial class RiscoDbLib
    {
        #region | Métodos CRUD

        public ConsultarObjetosResponse<RiscoParametrosInfo> ConsultarRiscoParametros(ConsultarEntidadeRequest<RiscoParametrosInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoParametrosInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRiscoNovoOMS;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_parametro_risco_lst"))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable) for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoParametrosInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        public ConsultarObjetosResponse<ContratoBmfInfo> ListarContratosBMF(ConsultarEntidadeRequest<ContratoBmfInfo> pParametros)
        {
            var lRetorno     = new ConsultarObjetosResponse<ContratoBmfInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRiscoNovoOMS;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, "select * from tb_contrato_bmf order by ds_contrato asc"))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        var lContrato = new ContratoBmfInfo();
                        DataRow lRow = lDataTable.Rows[i];

                        lContrato.CodigoContrato    = lRow["ds_contrato"].DBToString();
                        lContrato.DescricaoContrato = lRow["ds_contrato"].DBToString() + ".FUT";
                        
                        lRetorno.Resultado.Add(lContrato);
                    }
                }
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private RiscoParametrosInfo CarregarEntidadeRiscoParametrosInfo(DataRow pLinha)
        {
            return new RiscoParametrosInfo()
            {
                DsParametro = pLinha["ds_parametro"].DBToString(),
                IdBolsa = pLinha["id_bolsa"].DBToInt32(),
                IdParametro = pLinha["id_parametro"].DBToInt32(),
            };
        }

        #endregion
    }
}
