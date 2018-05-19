using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos.Dados.Risco;
using System;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Risco
{
    public static class RiscoMonitorPrejuizoGeralDbLib
    {
        #region Atributos
        private static string gNomeConexaoOracle = "Sinacor";
        private static string gNomeConexaoConfig = "Config";
        #endregion

        #region Propriedades
        private static ConsultarObjetosResponse<MonitorRiscoInfo> gRetorno { get; set; }
        private static List<MonitorRiscoInfo> gRetornoLista;
        private static MonitorRiscoInfo gRetornoInterno;
        #endregion

        public static void ObterDadosCliente(int? CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            //MonitorRiscoInfo _ClienteInfo = new MonitorRiscoInfo();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obtem_cliente_asse_monitor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string TipoCliente = (lDataTable.Rows[i]["Tipo"]).DBToString();

                            gRetornoInterno.Assessor             = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
                            gRetornoInterno.NomeCliente          = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            gRetornoInterno.NomeAssessor         = (lDataTable.Rows[i]["NM_ASSESSOR"]).DBToString();

                            if (TipoCliente == "BOVESPA")
                            {
                                gRetornoInterno.CodigoCliente    = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                                gRetornoInterno.StatusBovespa    = (lDataTable.Rows[i]["situac"]).DBToString();
                            }
                            else
                            {
                                gRetornoInterno.CodigoClienteBmf = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                                gRetornoInterno.StatusBMF        = (lDataTable.Rows[i]["situac"]).DBToString();
                            }

                            gRetornoInterno.Assessor             = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
                            gRetornoInterno.NomeCliente          = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void  ObterContaBMF(int CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_obtem_cod_bmf_monitor"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, CodigoCliente);

                DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        gRetornoInterno.CodigoClienteBmf= (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                        gRetornoInterno.StatusBMF       = (lDataTable.Rows[i]["Status"]).DBToString();
                    }
                }
            }
        }

        public  static ConsultarObjetosResponse<MonitorRiscoInfo> ConsultarDadosMonitorRisco(ConsultarEntidadeRequest<MonitorRiscoInfo> pParametros)
        {
            gRetorno = new ConsultarObjetosResponse<MonitorRiscoInfo>();

            gRetornoLista = new List<MonitorRiscoInfo>();

            gRetornoInterno  = new MonitorRiscoInfo();

            if (pParametros.Objeto.CodigoClienteBmf != null)
            {
                gRetornoInterno.CodigoClienteBmf = pParametros.Objeto.CodigoClienteBmf;
            }

            if (pParametros.Objeto.CodigoAssessor.HasValue)
            {
                List<int> lClientes =  ClienteDbLib.ReceberListaClientesAssessoresVinculados(pParametros.Objeto.CodigoAssessor.Value);

                int lCliente = pParametros.Objeto.CodigoCliente.HasValue ? pParametros.Objeto.CodigoCliente.Value : pParametros.Objeto.CodigoClienteBmf.Value;

                if (!lClientes.Contains(lCliente))
                {
                    return gRetorno;
                }
            }

            ObterDadosCliente(pParametros.Objeto.CodigoCliente.HasValue ? pParametros.Objeto.CodigoCliente : pParametros.Objeto.CodigoClienteBmf);

            ConsultarMargemRequeridaBMF(pParametros);

            ConsultarGarantiaBovespa(pParametros);

            if ((gRetornoInterno.CodigoClienteBmf == null))
            {
                ObterContaBMF(pParametros.Objeto.CodigoCliente.Value);
            }

            if (gRetornoInterno.CodigoClienteBmf != null && gRetornoInterno.CodigoCliente == null)
            {
                gRetornoInterno.CodigoCliente = gRetornoInterno.CodigoClienteBmf;
            }

            //gRetorno.DescricaoResposta = string.Format("Posição em Monitor de Risco do cliente: {0} carregado com sucesso", pParametros.CodigoCliente.DBToString());
            //gRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

            gRetornoLista.Add(gRetornoInterno);

            gRetorno.Resultado = gRetornoLista;

            return gRetorno;
        }

        private static  void ConsultarMargemRequeridaBMF(ConsultarEntidadeRequest<MonitorRiscoInfo> pParametros)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;
            
            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDOCLIENTE_BMF"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pId_Cliente", DbType.Int32, pParametros.Objeto.CodigoClienteBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        gRetornoInterno.ValorMargemRequerida = lDataTable.Rows[i]["VL_TOTMAR"].DBToDecimal();
                    }
                }
            }
        }

        private static void ConsultarGarantiaBovespa(ConsultarEntidadeRequest<MonitorRiscoInfo> pParametros)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_GARANTIA_BOV"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pId_Cliente", DbType.Int32, pParametros.Objeto.CodigoCliente);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        gRetornoInterno.ValorGarantiaDeposito       = lDataTable.Rows[i]["VALO_GARN_DEPO"].DBToDecimal();
                        gRetornoInterno.DataMovimentoGarantia       = lDataTable.Rows[i]["DATA_MVTO"].DBToDateTime();
                        gRetornoInterno.ValorMargemRequeridaBovespa = lDataTable.Rows[i]["VALO_GARN_REQD"].DBToDecimal();
                        //gRetorno.Relatorio.ValorGarantiaDeposito = lDataTable.Rows[i]["VALO_GARN_DEPO"].DBToDecimal();
                        //gRetorno.Relatorio.DataMovimentoGarantia = lDataTable.Rows[i]["DATA_MVTO"].DBToDateTime();
                    }
                }
            }
        }
        
        public static ReceberObjetoResponse<MonitorRiscoFeriadosInfo> ReceberFeriados(ReceberEntidadeRequest<MonitorRiscoFeriadosInfo> pParametros)
        {
            ReceberObjetoResponse<MonitorRiscoFeriadosInfo> lRetorno = new ReceberObjetoResponse<MonitorRiscoFeriadosInfo>();

            lRetorno.Objeto = new MonitorRiscoFeriadosInfo();

            lRetorno.Objeto.ListaFeriados = new List<DateTime>(); 

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoConfig;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obtem_feriados_sel"))
            {
                //lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, CodigoCliente);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        lRetorno.Objeto.ListaFeriados.Add(lDataTable.Rows[i]["dt_feriado"].DBToDateTime());
                    }
                }
            }

            return lRetorno;

        }
    }
}
