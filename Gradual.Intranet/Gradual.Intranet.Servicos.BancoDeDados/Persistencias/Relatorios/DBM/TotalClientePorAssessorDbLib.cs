using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.DBM
{
    public class TotalClientePorAssessorDbLib
    {
        public ConsultarObjetosResponse<TotalClientePorAssessorInfo> ConsultarTotalDeAssessorPorCliente(ConsultarEntidadeRequest<TotalClientePorAssessorInfo> pParametro)
        {
            var lRetorno = new ConsultarObjetosResponse<TotalClientePorAssessorInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_dbm_total_ass_cli"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.String, pParametro.Objeto.ConsultaCdAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_negocioInicio", DbType.Date, pParametro.Objeto.ConsultaDtNegocioInicio);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_negocioFim", DbType.Date, pParametro.Objeto.ConsultaDtNegocioFim);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Resultado.Add(new TotalClientePorAssessorInfo()
                        {
                            CdAssessor = lLinha["CD_ASSESSOR"].DBToInt32(),
                            CdCliente = lLinha["CD_CLIENTE"].DBToInt32(),
                            DsBolsa = lLinha["ds_bolsa"].DBToString(),
                            NmAssessor = lLinha["NM_ASSESSOR"].DBToString(),
                            NmCliente = lLinha["NM_CLIENTE"].DBToString(),
                            PcDescontoDv = lLinha["pc_desconto_dv"].DBToDecimal(),
                            VlCorretagemBruta = lLinha["vl_corretagem_bruta"].DBToDecimal(),
                            VlCorretagemLiquida = lLinha["vl_corretagem_liquida"].DBToDecimal(),
                            VlDescontoDv = lLinha["vl_desconto_dv"].DBToDecimal(),
                            VlFg = lLinha["vl_fg"].DBToDecimal(),
                            VlPc = lLinha["vl_pc"].DBToDecimal(),
                            VlVc = lLinha["vl_vc"].DBToDecimal()
                        });
                    }
            }

            return lRetorno;
        }
    }
}
