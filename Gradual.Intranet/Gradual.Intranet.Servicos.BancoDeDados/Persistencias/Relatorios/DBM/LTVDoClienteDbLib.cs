using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.DBM
{
    public class LTVDoClienteLTVDoClienteDbLib
    {
        public ConsultarObjetosResponse<LTVDoClienteInfo> ConsultarBDM(ConsultarEntidadeRequest<LTVDoClienteInfo> pParametro)
        {
            var lRetorno = new ConsultarObjetosResponse<LTVDoClienteInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_dbm_ltv_cliente_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCodigoCliente", DbType.Int32, pParametro.Objeto.ConsultaCodigoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.String, pParametro.Objeto.ConsultaCodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "pDataInicial", DbType.Date, pParametro.Objeto.ConsultaDataDe);
                lAcessaDados.AddInParameter(lDbCommand, "pDataFinal", DbType.Date, pParametro.Objeto.ConsultaDataAte);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Resultado.Add(new LTVDoClienteInfo()
                        {
                            CodigoCliente = lLinha["CD_CLIENTE"].DBToString(),
                            NomeCliente = lLinha["NM_CLIENTE"].DBToString(),
                            MesNegocio = lLinha["DT_NEGOCIO"].DBToString(),
                            ValorCorretagemPorPeriodo = lLinha["VL_CORRET_PERIODO"].DBToDecimal(),
                            ValorVolumePorPeriodo = lLinha["VL_Vol_Periodo"].DBToDecimal(),
                        });
            }

            return lRetorno;
        }
    }
}
