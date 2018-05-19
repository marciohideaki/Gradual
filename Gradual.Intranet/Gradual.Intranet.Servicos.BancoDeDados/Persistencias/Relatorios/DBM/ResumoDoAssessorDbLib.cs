using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.DBM
{
    public class ResumoDoAssessorDbLib
    {
        public ReceberObjetoResponse<ResumoDoAssessorCadastroInfo> ReceberCadastro(ReceberEntidadeRequest<ResumoDoAssessorCadastroInfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ResumoDoAssessorCadastroInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_DBM_ResAss_Cadastro_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.Int32, pParametro.Objeto.ConsultaCodigoAssessor);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    lRetorno.Objeto = new ResumoDoAssessorCadastroInfo()
                    {
                        QtClientesAtivos = lDataTable.Rows[0]["qt_ativos"].DBToInt32(),
                        QtClientesInativos = lDataTable.Rows[0]["qt_inativos"].DBToInt32(),
                        QtClientesNovos = lDataTable.Rows[0]["qt_novos_clientes"].DBToInt32(),
                        QtClientesInstitucional = lDataTable.Rows[0]["qt_institucional"].DBToInt32(),
                        QtClientesNoVarejo = lDataTable.Rows[0]["qt_varejo"].DBToInt32(),
                        QtTotalClientes = lDataTable.Rows[0]["qt_total"].DBToInt32(),
                        VlPercentOperouNoMes = lDataTable.Rows[0]["vl_percent_operou_mes"].DBToDecimal(),
                        VlPercenturalComCustodia = lDataTable.Rows[0]["vl_percent_custodia"].DBToDecimal(),
                    };
                }
            }

            return lRetorno;
        }

        public ReceberObjetoResponse<ResumoDoAssessorReceitaInfo> ReceberReceita(ReceberEntidadeRequest<ResumoDoAssessorReceitaInfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ResumoDoAssessorReceitaInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prd_DBM_ResAss_Receita_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.Int32, pParametro.Objeto.ConsultaCodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_inicio", DbType.DateTime, pParametro.Objeto.ConsultaDataInicial);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_fim", DbType.DateTime, pParametro.Objeto.ConsultaDataFinal);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    lRetorno.Objeto = new ResumoDoAssessorReceitaInfo();

                    lRetorno.Objeto.QtBovespaClientes = lDataTable.Rows[0]["qt_bovespaclientes"].DBToDecimal();
                    lRetorno.Objeto.VlBovespaCorretagem = lDataTable.Rows[0]["vl_bovespacorretagem"].DBToDecimal();
                    lRetorno.Objeto.QtBmfClientes = lDataTable.Rows[0]["qt_bmfclientes"].DBToDecimal();
                    lRetorno.Objeto.VlBmfCorretagem = lDataTable.Rows[0]["vl_bmfcorretagem"].DBToDecimal();
                    lRetorno.Objeto.VlTesouroCorretagem = lDataTable.Rows[0]["vl_tesouro_corretagem"].DBToDecimal();
                    lRetorno.Objeto.QtTesouroClientes = lDataTable.Rows[0]["qt_tesouro_clientes"].DBToDecimal();
                    lRetorno.Objeto.VlOutrosCorretagem = lDataTable.Rows[0]["vl_outros_corretagem"].DBToDecimal();
                    lRetorno.Objeto.QtOutrosClientes = lDataTable.Rows[0]["qt_outros_clientes"].DBToDecimal();
                    lRetorno.Objeto.VlBtcCorretagem = lDataTable.Rows[0]["vl_btc_corretagem"].DBToDecimal();
                    lRetorno.Objeto.QtBtcClientes = lDataTable.Rows[0]["qt_btc_clientes"].DBToDecimal();
                }
            }

            return lRetorno;
        }

        public ReceberObjetoResponse<ResumoDoAssessorCanalInfo> ReceberCanal(ReceberEntidadeRequest<ResumoDoAssessorCanalInfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ResumoDoAssessorCanalInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prd_DBM_ResAss_Canal_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.Int32, pParametro.Objeto.ConsultaCodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_inicio", DbType.DateTime, pParametro.Objeto.ConsultaDataInicial);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_fim", DbType.DateTime, pParametro.Objeto.ConsultaDataFinal);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    lRetorno.Objeto = new ResumoDoAssessorCanalInfo();
                    lRetorno.Objeto.QtHbValor = lDataTable.Rows[0]["qt_hb_valor"].DBToDecimal();
                    lRetorno.Objeto.VlHbPercentual = lDataTable.Rows[0]["vl_hb_percentual"].DBToDecimal();
                    lRetorno.Objeto.QtRepassadorValor = lDataTable.Rows[0]["qt_repassador_valor"].DBToDecimal();
                    lRetorno.Objeto.VlRepassadorPercentual = lDataTable.Rows[0]["vl_repassador_percentual"].DBToDecimal();
                    lRetorno.Objeto.QtMesaValor = lDataTable.Rows[0]["qt_mesa_valor"].DBToDecimal();
                    lRetorno.Objeto.VlMesaPercentual = lDataTable.Rows[0]["vl_mesa_percentual"].DBToDecimal();
                }
            }


            return lRetorno;
        }

        public ReceberObjetoResponse<ResumoDoAssessorMetricasInfo> ReceberMetricas(ReceberEntidadeRequest<ResumoDoAssessorMetricasInfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ResumoDoAssessorMetricasInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prd_DBM_ResAss_Metricas_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.Int32, pParametro.Objeto.ConsultaCdAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_inicio", DbType.Date, pParametro.Objeto.ConsultaDataInicio);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_fim", DbType.Date, pParametro.Objeto.ConsultaDataFim);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    lRetorno.Objeto = new ResumoDoAssessorMetricasInfo();
                    lRetorno.Objeto.VlCorretagemMesAnterior = lDataTable.Rows[0]["vl_corretagem_mes_anterior"].DBToDecimal();
                    lRetorno.Objeto.VlCorretagemMes = lDataTable.Rows[0]["vl_corretagem_mes"].DBToDecimal();
                    lRetorno.Objeto.VlCorretagemAno = lDataTable.Rows[0]["vl_corretagem_periodo"].DBToDecimal();
                    lRetorno.Objeto.VlCorretagemDia = lDataTable.Rows[0]["vl_corretagem_dia"].DBToDecimal();
                    lRetorno.Objeto.QtCadastrosMesAnterior = lDataTable.Rows[0]["qt_cadastros_mes_anterior"].DBToInt32();
                    lRetorno.Objeto.QtCadastrosMes = lDataTable.Rows[0]["qt_cadastros_mes"].DBToInt32();
                    lRetorno.Objeto.QtCadastrosMediaAno = lDataTable.Rows[0]["qt_cadastros_periodo"].DBToDecimal();
                    lRetorno.Objeto.QtCadastrosDia = lDataTable.Rows[0]["qt_cadastros_dia"].DBToInt32();
                }
            }

            return lRetorno;
        }

        public ConsultarObjetosResponse<ResumoDoAssessorTop10Info> ConsultarClientesTop10(ConsultarEntidadeRequest<ResumoDoAssessorTop10Info> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<ResumoDoAssessorTop10Info>();
            var lAcessaDados = new ConexaoDbHelper();

            lRetorno.Resultado = new List<ResumoDoAssessorTop10Info>();
            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_dbm_ressass_top10cli"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.Int32, pParametros.Objeto.ConsultaCodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_inicial", DbType.Date, pParametros.Objeto.ConsultaDataInicial);
                lAcessaDados.AddInParameter(lDbCommand, "pdt_final", DbType.Date, pParametros.Objeto.ConsultaDataFinal);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Resultado.Add(new ResumoDoAssessorTop10Info()
                        {
                            DsNomeCliente = lLinha["NM_CLIENTE"].DBToString(),
                            VlCorretagem = lLinha["vl_totalcorretagem"].DBToDecimal(),
                            VlCustodia = lLinha["vl_custodia"].DBToDecimal(),
                            VlPercentualAcumulado = 0.DBToDecimal(),
                            VlPercentualDevMedia = lLinha["vl_percentualdesconto"].DBToDecimal(),
                            VlPercentualTotal = lLinha["vl_percentualcliente"].DBToDecimal()
                        });
            }

            return lRetorno;
        }
    }
}
