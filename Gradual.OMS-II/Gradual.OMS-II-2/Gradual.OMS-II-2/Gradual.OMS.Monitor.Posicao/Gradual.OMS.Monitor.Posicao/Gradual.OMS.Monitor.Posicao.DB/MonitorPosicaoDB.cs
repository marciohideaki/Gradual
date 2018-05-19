using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Generico.Dados;
using Gradual.OMS.Monitor.Posicao.Lib.Util;
using System.Data.Common;
using System.Data;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using System.Collections;

namespace Gradual.OMS.Monitor.Posicao.DB
{
    public class MonitorPosicaoDB
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string gNomeConexaoOracle        = "Sinacor";
        private static string gNomeCOnexaoOracleSinacor = "CORRWIN";
        private static string gNomeConexaoSQLConfig     = "Config";
        private static string gNomeConexaoSQLCadastro   = "Cadastro";
        private static string gNomeConexaoGradualOMS    = "GradualOMS";

        //public static MonitorCustodiaInfo lRetornoInterno { get; set; }
        public static void ObterDadosCliente(int CodigoCliente, ref MonitorCustodiaInfo lRetornoInterno)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_obtem_cliente_asse_monitor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string TipoCliente = (lDataTable.Rows[i]["Tipo"]).DBToString();

                            lRetornoInterno.CodigoAssessor = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
                            lRetornoInterno.NomeAssessor = (lDataTable.Rows[i]["NM_ASSESSOR"]).DBToString();
                            lRetornoInterno.NomeCliente = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();

                            if (TipoCliente == "BOVESPA")
                            {
                                lRetornoInterno.CodigoClienteBov = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                                lRetornoInterno.StatusBov = (lDataTable.Rows[i]["situac"]).DBToString();
                            }
                            else
                            {
                                lRetornoInterno.StatusBmf = (lDataTable.Rows[i]["situac"]).DBToString();
                                lRetornoInterno.CodigoClienteBmf = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static List<int> ReceberListaClientesAssessoresVinculados(int CodigoAssessor, int? CodigoLogin)
        {
            var lRetorno = new List<int>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoSQLCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "ListarClientesAssessoresVinculadosRisco_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_assessor", DbType.Int32, CodigoAssessor);

                if (CodigoLogin.HasValue)
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_login", DbType.Int32, CodigoLogin.Value);
                }

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0) foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Add(lLinha["cd_codigo"].DBToInt32());
            }

            return lRetorno;
        }

        public static MonitorCustodiaInfo ConsultarDadosClienteMonitorCustodia(MonitorCustodiaInfo pParametros)
        {
            var lRetornoInterno = new MonitorCustodiaInfo();

            if (pParametros.CodigoClienteBmf.HasValue)
            {
                lRetornoInterno.CodigoClienteBmf = pParametros.CodigoClienteBmf.Value;
            }

            if (pParametros.CodAssessor.HasValue)
            {
                List<int> lClientes = ReceberListaClientesAssessoresVinculados(pParametros.CodAssessor.Value, pParametros.CodLogin);

                int lCliente = pParametros.CodigoClienteBov.HasValue ? pParametros.CodigoClienteBov.Value : pParametros.CodigoClienteBmf.Value;

                if (!lClientes.Contains(lCliente))
                {
                    return lRetornoInterno;
                }
            }

            ObterDadosCliente(pParametros.CodigoClienteBov.HasValue ? pParametros.CodigoClienteBov.Value : pParametros.CodigoClienteBmf.Value, ref lRetornoInterno);

            ConsultarMargemRequeridaBMF(pParametros, ref lRetornoInterno);

            ConsultarGarantiaBovespa(pParametros, ref lRetornoInterno);

            if (!lRetornoInterno.CodigoClienteBmf.HasValue)
            {
                ObterContaBMF(pParametros.CodigoClienteBov.Value, ref lRetornoInterno);
            }

            if (lRetornoInterno.CodigoClienteBmf.HasValue && !lRetornoInterno.CodigoClienteBov.HasValue)
            {
                lRetornoInterno.CodigoClienteBov = lRetornoInterno.CodigoClienteBmf;
            }

            return lRetornoInterno;
        }

        private static void ConsultarGarantiaBovespa(MonitorCustodiaInfo pParametros, ref MonitorCustodiaInfo lRetornoInterno)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_GARANTIA_BOV"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pId_Cliente", DbType.Int32, pParametros.CodigoClienteBov);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetornoInterno.ValorGarantiaDeposito = lDataTable.Rows[i]["VALO_GARN_DEPO"].DBToDecimal();
                        lRetornoInterno.DataMovimentoGarantia = lDataTable.Rows[i]["DATA_MVTO"].DBToDateTime();
                        lRetornoInterno.ValorMargemRequeridaBovespa = lDataTable.Rows[i]["VALO_GARN_REQD"].DBToDecimal();
                    }
                }
            }
        }

        private static void ConsultarMargemRequeridaBMF(MonitorCustodiaInfo pParametros, ref MonitorCustodiaInfo lRetornoInterno)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDOCLIENTE_BMF"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pId_Cliente", DbType.Int32, lRetornoInterno.CodigoClienteBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetornoInterno.ValorMargemRequerida = lDataTable.Rows[i]["VL_TOTMAR"].DBToDecimal();
                    }
                }
            }
        }

        public static void ObterContaBMF(int CodigoCliente, ref MonitorCustodiaInfo lRetornoInterno)
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
                        lRetornoInterno.CodigoClienteBmf = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                        //gRetornoInterno.StatusBMF = (lDataTable.Rows[i]["Status"]).DBToString();
                    }
                }
            }
        }

        public static List<MonitorCustodiaInfo.CustodiaPosicao> ConsultarCustodiaNormal(Nullable<int> CodigoCliente, Nullable<int> CodigoClienteBmf)
        {
            MonitorCustodiaInfo lRetorno = new MonitorCustodiaInfo();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET3"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "IdClienteBMF", DbType.Int32, CodigoClienteBmf);

                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            if (lDataTable.Rows[i]["TIPO_MERC"].DBToString().Equals("OPF") ||
                                lDataTable.Rows[i]["TIPO_MERC"].DBToString().Equals("FUT") ||
                                lDataTable.Rows[i]["TIPO_MERC"].DBToString().Equals("OPD"))
                            {
                                continue;
                            }

                            lRetorno.ListaCustodia.Add(new MonitorCustodiaInfo.CustodiaPosicao()
                            {
                                CodigoInstrumento = lDataTable.Rows[i]["COD_NEG"].DBToString(),
                                CodigoCarteira    = lDataTable.Rows[i]["COD_CART"].DBToInt32(),
                                DescricaoCarteira = lDataTable.Rows[i]["DESC_CART"].DBToString().Trim(),
                                TipoMercado       = lDataTable.Rows[i]["TIPO_MERC"].DBToString(),
                                TipoGrupo         = lDataTable.Rows[i]["TIPO_GRUP"].DBToString(),
                                IdCliente         = lDataTable.Rows[i]["COD_CLI"].DBToInt32(),
                                QtdeAtual         = lDataTable.Rows[i]["QTDE_ATUAL"].DBToDecimal(),
                                QtdeLiquidar      = lDataTable.Rows[i]["QTDE_LIQUID"].DBToDecimal(),
                                QtdeDisponivel    = lDataTable.Rows[i]["QTDE_DISP"].DBToDecimal(),
                                QtdeAExecVenda    = lDataTable.Rows[i]["QTDE_AEXE_VDA"].DBToDecimal(),
                                QtdeAExecCompra   = lDataTable.Rows[i]["QTDE_AEXE_CPA"].DBToDecimal(),
                                NomeEmpresa       = lDataTable.Rows[i]["NOME_EMP_EMI"].DBToString(),
                                ValorPosicao      = lDataTable.Rows[i]["VAL_POSI"].DBToDecimal(),
                                DtVencimento      = lDataTable.Rows[i]["DATA_VENC"].DBToDateTime(Extensions.eDateNull.Permite),
                                QtdeD1            = lDataTable.Rows[i]["QTDE_DA1"].DBToDecimal(),
                                QtdeD2            = lDataTable.Rows[i]["QTDE_DA2"].DBToDecimal(),
                                QtdeD3            = lDataTable.Rows[i]["QTDE_DA3"].DBToDecimal(),
                                CodigoSerie       = lDataTable.Rows[i]["COD_SERI"].DBToString(),
                                FatorCotacao      = lDataTable.Rows[i]["FAT_COT"].DBToDecimal(),
                                QtdeDATotal       = lDataTable.Rows[i]["QTDE_DATOTAL"].DBToDecimal(),
                            });
                        }
                }
                if (lAcessaDados != null)
                {
                    lAcessaDados.Dispose();
                    lAcessaDados = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            if (lRetorno.ListaCustodia.Count > 0)
            {
                AtualizaInserePosicaoNormal(lRetorno.ListaCustodia);
            }

            return lRetorno.ListaCustodia;
        }

        

        public static double ObteCotacaoPtax()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            double lSaldo = 0.0D;

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_OBTER_COTACAO_PTAX"))
            {
                DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        lSaldo = (lDataTable.Rows[i]["VL_DOLVDA_ATU"]).DBToDouble();
                    }
                }

            }

            return lSaldo;
        }

        public static Hashtable ObterVencimentosDI()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            string procedure = "prc_obter_relacao_DI";

            Hashtable htVencimentoDI = new Hashtable();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, procedure))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string Instrumento = (lDataTable.Rows[i]["cd_codneg"]).DBToString();
                            DateTime Vencimento = (lDataTable.Rows[i]["DT_VENC"]).DBToDateTime();

                            htVencimentoDI.Add(Instrumento, Vencimento);
                        }
                    }

                }

                return htVencimentoDI;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (lAcessaDados != null)
                {
                    lAcessaDados.Dispose();
                    lAcessaDados = null;
                }
            }

        }

        public static CotacaoValor ObterCotacaoAtual(string pInstrumento)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            //Hashtable htQuote = new Hashtable();

            decimal ValorCotacao = 0;
            string IdAtivo = string.Empty;

            CotacaoValor lRetorno = new CotacaoValor();

            lRetorno.ValorCotacao = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoGradualOMS;// gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotacoes_monitor_item"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@pAtivo", DbType.AnsiString, pInstrumento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            IdAtivo                  = (lDataTable.Rows[i]["id_ativo"]).DBToString();
                            ValorCotacao             = (lDataTable.Rows[i]["vl_ultima"]).DBToDecimal();
                            lRetorno.Ativo           = IdAtivo;
                            lRetorno.ValorCotacao    = ValorCotacao;
                            lRetorno.ValorFechamento = (lDataTable.Rows[i]["vl_fechamento"]).DBToDecimal();
                            lRetorno.ValorAbertura   = (lDataTable.Rows[i]["VL_abertura"]).DBToDecimal();
                            lRetorno.Variacao        = (lDataTable.Rows[i]["vl_oscilacao"]).DBToDecimal();
                            lRetorno.ValorAjuste     = (lDataTable.Rows[i]["vl_ajuste"].DBToDecimal());
                            lRetorno.ValorPU         = (lDataTable.Rows[i]["vl_pu"].DBToDecimal());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (lAcessaDados != null)
                {
                    lAcessaDados.Dispose();
                    lAcessaDados = null;
                }
            }
            return lRetorno;

        }

        public struct CotacaoValor
        {
            public string Ativo { get; set; }
            public decimal ValorCotacao { get; set; }
            public decimal ValorAbertura { get; set; }
            public decimal ValorFechamento { get; set; }
            public decimal Variacao { get; set; }
            public DateTime HoraCotacao { get; set; }
            public decimal ValorAjuste { get; set; }
            public decimal ValorPU { get; set; }
        }

        public List<int> ObterClientesPosicaoDia()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<int> lstClientes = new List<int>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RELACAO_CLIENTE_ORDENS"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            int IdClient = Convert.ToInt32(lDataTable.Rows[i]["cd_cliente"]);
                            lock (lstClientes)
                            {
                                lstClientes.Add(IdClient);
                            }
                        }
                    }

                }

                return lstClientes;
            }
            catch (Exception ex)
            {
                gLogger.Info("Ocorreu um erro ao acessar o método ObterClientesPosicaoDia.", ex);
            }

            return lstClientes;
        }

        public List<int> ObterClientesPosicaoBMFAfter()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<int> lstClientes = new List<int>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_relacao_bmfafter"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            int IdClient = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            lock (lstClientes)
                            {
                                lstClientes.Add(IdClient);
                            }
                        }
                    }

                }

                return lstClientes;
            }
            catch (Exception ex)
            {
                gLogger.Info("Ocorreu um erro ao acessar o método ObterClientesPosicaoDia.", ex);
            }

            return lstClientes;
        }

        public List<int> ListaClientesOperaramUltimoMomento()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            var ListaClientes = new List<int>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RELACAO_CLIENTE_OPERARAM"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            ListaClientes.Add(int.Parse(lDataTable.Rows[i]["cd_cliente"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (lAcessaDados != null)
                {
                    lAcessaDados.Dispose();
                    lAcessaDados = null;
                }
            }

            return ListaClientes;
        }

        public static void AtualizaInserePosicaoNormal(List<MonitorCustodiaInfo.CustodiaPosicao> ListaCustodia )
        {
            AcessaDados lDados = new AcessaDados();
            try
            {
                int lId_Cliente = ListaCustodia[0].IdCliente;

                lDados.ConnectionStringName = gNomeConexaoGradualOMS;

                DbCommand lCommandDel = lDados.CreateCommand(CommandType.StoredProcedure, "prc_vcfposicao_risco_del");

                lDados.AddInParameter(lCommandDel, "@cod_cli", DbType.Int32, lId_Cliente);

                lDados.ExecuteNonQuery(lCommandDel);

                foreach (MonitorCustodiaInfo.CustodiaPosicao posicao in ListaCustodia)
                {
                    lDados.ConnectionStringName = gNomeConexaoGradualOMS;

                    DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_vcfposicao_risco_ins");

                    lDados.AddInParameter(lCommand, "@cod_cart",     DbType.Int32,posicao.CodigoCarteira);
                    lDados.AddInParameter(lCommand, "@desc_cart",    DbType.String,posicao.DescricaoCarteira);
                    lDados.AddInParameter(lCommand, "@tipo_grup",    DbType.String,posicao.TipoGrupo);
                    lDados.AddInParameter(lCommand, "@tipo_merc",    DbType.String,posicao.TipoMercado);
                    lDados.AddInParameter(lCommand, "@cod_cli",      DbType.Int32,posicao.IdCliente);
                    lDados.AddInParameter(lCommand, "@qtde_disp",    DbType.Decimal,posicao.QtdeDisponivel);
                    lDados.AddInParameter(lCommand, "@qtde_aexe_cpa",DbType.Decimal,posicao.QtdeAExecCompra);
                    lDados.AddInParameter(lCommand, "@qtde_aexe_vda",DbType.Decimal,posicao.QtdeAExecVenda);
                    lDados.AddInParameter(lCommand, "@qtde_liquid",  DbType.Decimal,posicao.QtdeLiquidar);
                    lDados.AddInParameter(lCommand, "@qtde_atual",   DbType.Decimal,posicao.QtdeAtual);
                    lDados.AddInParameter(lCommand, "@nome_emp_emi", DbType.String,posicao.NomeEmpresa);
                    lDados.AddInParameter(lCommand, "@val_posi",     DbType.Decimal,posicao.ValorPosicao);
                    lDados.AddInParameter(lCommand, "@data_venc",    DbType.DateTime,posicao.DtVencimento);
                    lDados.AddInParameter(lCommand, "@qtde_da1",     DbType.Decimal,posicao.QtdeD1);
                    lDados.AddInParameter(lCommand, "@qtde_da2",     DbType.Decimal,posicao.QtdeD2);
                    lDados.AddInParameter(lCommand, "@qtde_da3",     DbType.Decimal,posicao.QtdeD3);
                    lDados.AddInParameter(lCommand, "@cod_neg",      DbType.String,posicao.CodigoInstrumento);
                    lDados.AddInParameter(lCommand, "@cod_seri",     DbType.String,posicao.CodigoSerie);
                    lDados.AddInParameter(lCommand, "@fat_cot",      DbType.Int32,posicao.FatorCotacao);
                    lDados.AddInParameter(lCommand, "@qtde_total",   DbType.Decimal,posicao.QtdeDATotal);

                    // Executa a operação no banco.
                    lDados.ExecuteNonQuery(lCommand);
                }
            }
            catch (Exception ex)
            {
                 throw (ex);
            }
        }

        public static List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF> ConsultarCustodiaPosicaoDiaBMF(Nullable<int> CodigoClienteBmf)
        {
            var lRetorno = new MonitorCustodiaInfo();
            var lAcessaDados = new AcessaDados();

            lRetorno.ListaPosicaoDiaBMF = new List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF>();
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET_DIA"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoClienteBmf.Value);

                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            MonitorCustodiaInfo.CustodiaPosicaoDiaBMF lPosicao = new MonitorCustodiaInfo.CustodiaPosicaoDiaBMF();

                            CotacaoValor lCotacao = ObterCotacaoAtual(lDataTable.Rows[i]["CD_NEGOCIO"].DBToString());

                            lPosicao.CodigoInstrumento = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                            lPosicao.TipoMercado       = "FUT";
                            lPosicao.TipoGrupo         = "FUT";
                            lPosicao.IdCliente         = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32();
                            lPosicao.QtdeAtual         = 0;
                            lPosicao.QtdeDisponivel    = lDataTable.Rows[i]["QT_NEGOCIO"].DBToDecimal();
                            lPosicao.CodigoSerie       = lDataTable.Rows[i]["CD_SERIE"].DBToString();
                            lPosicao.ValorFechamento   = lCotacao.ValorAbertura;
                            lPosicao.Cotacao           = lCotacao.ValorCotacao;
                            lPosicao.Variacao          = lCotacao.Variacao;
                            lPosicao.ValorPU           = lCotacao.ValorPU;
                            lPosicao.ValorAjuste       = lCotacao.ValorAjuste;

                            string lSentido = lDataTable.Rows[i]["CD_NATOPE"].DBToString();

                            lPosicao.Sentido = lSentido;

                            if (lSentido.Equals("V"))
                            {
                                lPosicao.QtdeAExecVenda = lDataTable.Rows[i]["QT_NEGOCIO"].DBToDecimal();

                                lPosicao.PrecoNegocioVenda = lDataTable.Rows[i]["PR_NEGOCIO"].DBToDecimal();
                            }
                            else if (lSentido.Equals("C"))
                            {
                                lPosicao.QtdeAExecCompra = lDataTable.Rows[i]["QT_NEGOCIO"].DBToDecimal();

                                lPosicao.PrecoNegocioCompra = lDataTable.Rows[i]["PR_NEGOCIO"].DBToDecimal();
                            }

                            lRetorno.ListaPosicaoDiaBMF.Add(lPosicao);
                        }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (lAcessaDados != null)
                {
                    lAcessaDados.Dispose();
                    lAcessaDados = null;
                }
            }

            if (lRetorno.ListaPosicaoDiaBMF.Count > 0)
            {
                AtualizaInserePosicaoDiaBmf(lRetorno.ListaPosicaoDiaBMF);
            }

            return lRetorno.ListaPosicaoDiaBMF;
        }

        public static void AtualizaInserePosicaoDiaBmf( List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF> ListaCustodiaDia)
        {
            AcessaDados lDados = new AcessaDados();

            try
            {
                int lId_Cliente = ListaCustodiaDia[0].IdCliente;

                lDados.ConnectionStringName = gNomeConexaoGradualOMS;

                DbCommand lCommandDel = lDados.CreateCommand(CommandType.StoredProcedure, "prc_vcfposicao_dia_risco_del");

                lDados.AddInParameter(lCommandDel, "@id_cliente", DbType.Int32,lId_Cliente );

                lDados.ExecuteNonQuery(lCommandDel);

                foreach (MonitorCustodiaInfo.CustodiaPosicaoDiaBMF posicaoDia in ListaCustodiaDia)
                {
                    lDados.ConnectionStringName = gNomeConexaoGradualOMS;

                    DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_vcfposicao_dia_risco_ins");

                    lDados.AddInParameter(lCommand, "@cod_neg"  ,       DbType.String,              posicaoDia.CodigoInstrumento);
                    lDados.AddInParameter(lCommand, "@tipo_merc",       DbType.String,              posicaoDia.TipoMercado);
                    lDados.AddInParameter(lCommand, "@tipo_grup",       DbType.String,              posicaoDia.TipoGrupo);
                    lDados.AddInParameter(lCommand, "@id_cliente",      DbType.Int32,               posicaoDia.IdCliente);
                    lDados.AddInParameter(lCommand, "@qtde_disp",			DbType.Decimal,         posicaoDia.QtdeDisponivel);
                    lDados.AddInParameter(lCommand, "@cod_serie",           DbType.String,         posicaoDia.CodigoSerie);
                    lDados.AddInParameter(lCommand, "@valor_fechamento",	DbType.Decimal,         posicaoDia.ValorFechamento);
                    lDados.AddInParameter(lCommand, "@cotacao",			    DbType.Decimal,         posicaoDia.Cotacao);
                    lDados.AddInParameter(lCommand, "@variacao",            DbType.Decimal,         posicaoDia.Variacao);
                    lDados.AddInParameter(lCommand, "@valor_pu",			DbType.Int32,           posicaoDia.ValorPU);
                    lDados.AddInParameter(lCommand, "@ajuste",				DbType.Decimal,         posicaoDia.ValorAjuste);
                    lDados.AddInParameter(lCommand, "@sentido",             DbType.String,         posicaoDia.Sentido);
                    lDados.AddInParameter(lCommand, "@qtde_aexec_venda",    DbType.String,          posicaoDia.QtdeAExecVenda);
                    lDados.AddInParameter(lCommand, "@qtde_aexec_compra",   DbType.Decimal,       posicaoDia.QtdeAExecCompra);
                    lDados.AddInParameter(lCommand, "@preco_negocio_venda", DbType.Decimal,       posicaoDia.PrecoNegocioVenda);
                    lDados.AddInParameter(lCommand, "@preco_negocio_compra",DbType.Decimal ,        posicaoDia.PrecoNegocioCompra);

                    // Executa a operação no banco.
                    lDados.ExecuteNonQuery(lCommand);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void ZeraPosicaoTodos()
        {
            AcessaDados lDados = new AcessaDados();

            try
            {

                lDados.ConnectionStringName = gNomeConexaoGradualOMS;

                DbCommand lCommandDel1 = lDados.CreateCommand(CommandType.StoredProcedure, "prc_vcfposicao_dia_risco_todos_del");

                lDados.ExecuteNonQuery(lCommandDel1);


                DbCommand lCommandDel2 = lDados.CreateCommand(CommandType.StoredProcedure, "prc_vcfposicao_risco_todos_del");

                lDados.ExecuteNonQuery(lCommandDel2);
                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        #endregion
    }
}
