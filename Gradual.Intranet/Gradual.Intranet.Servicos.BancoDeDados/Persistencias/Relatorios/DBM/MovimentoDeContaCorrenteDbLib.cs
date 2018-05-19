using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.DBM
{
    public class MovimentoDeContaCorrenteDbLib
    {
        public ConsultarObjetosResponse<MovimentoDeContaCorrenteInfo> ConsultarMovimentoDeContaCorrente(ConsultarEntidadeRequest<MovimentoDeContaCorrenteInfo> pParametro)
        {
            var lRetorno = new ConsultarObjetosResponse<MovimentoDeContaCorrenteInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_dbm_mvto_cc"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pdt_lancamento", DbType.Date, pParametro.Objeto.ConsultaDataLancamento);
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.String, pParametro.Objeto.ConsultaCodigoAssessor);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Resultado.Add(new MovimentoDeContaCorrenteInfo()
                        {
                            CdCliente = lLinha["CD_CLIENTE"].DBToInt32(),
                            DsLancamento = lLinha["DS_LANCAMENTO"].DBToString(),
                            DtLancamento = lLinha["DT_LANCAMENTO"].DBToDateTime(),
                            DtLiquidacao = lLinha["DT_LIQUIDACAO"].DBToDateTime(),
                            DtReferencia = lLinha["DT_REFERENCIA"].DBToDateTime(),
                            NmCliente = lLinha["NM_CLIENTE"].DBToString(),
                            VlLancamento = lLinha["VL_LANCAMENTO"].DBToDecimal(),
                        });
                    }
            }

            return lRetorno;
        }
    }
}
