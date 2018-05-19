using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.DBM
{
    public class SaldoProjecoesEmContaCorrenteDbLib
    {
        public ConsultarObjetosResponse<SaldoProjecoesEmContaCorrenteInfo> ConsultarSaldoProjecoesEmContaCorrente(ConsultarEntidadeRequest<SaldoProjecoesEmContaCorrenteInfo> pParametro)
        {
            var lRetorno = new ConsultarObjetosResponse<SaldoProjecoesEmContaCorrenteInfo>();
            var lAcessaDados = new ConexaoDbHelper();
             
            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_dbm_saldoprojcc"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.String, pParametro.Objeto.ConsultaCdAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_posicao", DbType.Date, pParametro.Objeto.ConsultaDataOperacao);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Resultado.Add(new SaldoProjecoesEmContaCorrenteInfo()
                        {
                            CdAssessor = lLinha["CD_ASSESSOR"].DBToInt32(),
                            CdCliente = lLinha["CD_CLIENTE"].DBToInt32(),
                            NmAssessor = lLinha["NM_ASSESSOR"].DBToString(),
                            NmCliente = lLinha["NM_CLIENTE"].DBToString(),
                            VlALiquidar = lLinha["A_LIQUIDAR"].DBToDecimal(),
                            VlDisponivel = lLinha["VL_DISPONIVEL"].DBToDecimal(),
                            VlProjetado1 = lLinha["VL_PROJET1"].DBToDecimal(),
                            VlProjetado2 = lLinha["VL_PROJET2"].DBToDecimal(),
                            VlTotal = lLinha["VL_TOTAL"].DBToDecimal(),
                        });
                    }
            }

            return lRetorno;
        }
    }
}
