using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Monitor.Custodia.Lib.Info;
using log4net;
using Gradual.Generico.Dados;
using Gradual.Generico.Geral;
using System.Data;
using System.Data.Common;
using Gradual.Monitor.Custodia.Lib.Mensageria;
using Gradual.Monitor.Custodia.Lib.Util;
using System.Collections;

namespace Gradual.Monitor.Custodia.DB
{
    public class MonitorCustodiaDB
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string gNomeConexaoOracle = "Sinacor";
        private static string gNomeConexaoSQLConfig = "Config";
        //private static MonitorCustodiaInfo gRetornoInterno;
        #endregion

        #region Métodos
        public static ClienteInfo ObterDadosCliente(int CodigoCliente, ref MonitorCustodiaInfo lRetornoInterno)
        {

            AcessaDados lAcessaDados = new AcessaDados();
            ClienteInfo _ClienteInfo = new ClienteInfo();

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

                            _ClienteInfo.Assessor               = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
                            _ClienteInfo.NomeCliente            = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            _ClienteInfo.NomeAssessor           = (lDataTable.Rows[i]["NM_ASSESSOR"]).DBToString();

                            if (TipoCliente == "BOVESPA")
                            {
                                lRetornoInterno.CodigoClienteBov = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                                _ClienteInfo.CodigoBovespa      = (lDataTable.Rows[i]["Codigo"]).DBToString();
                                //gRetornoInterno.StatusBovespa   = (lDataTable.Rows[i]["situac"]).DBToString();
                            }
                            else
                            {
                                _ClienteInfo.CodigoBMF          = (lDataTable.Rows[i]["Codigo"]).DBToString();
                                lRetornoInterno.CodigoClienteBmf = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                                //gRetornoInterno.StatusBMF       = (lDataTable.Rows[i]["situac"]).DBToString();
                            }

                            _ClienteInfo.Assessor    = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
                            _ClienteInfo.NomeCliente = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                        }
                    }
                }

                return _ClienteInfo;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static MonitorCustodiaInfo ConsultarDadosClienteMonitorCustodia(MonitorCustodiaInfo pParametros)
        {
            var lRetornoInterno = new MonitorCustodiaInfo();

            if (pParametros.CodigoClienteBmf.HasValue)
            {
                lRetornoInterno.CodigoClienteBmf = pParametros.CodigoClienteBmf.Value;
            }

            ObterDadosCliente(pParametros.CodigoClienteBov.HasValue ? pParametros.CodigoClienteBov.Value : pParametros.CodigoClienteBmf.Value,ref lRetornoInterno);

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
                lAcessaDados.AddInParameter(lDbCommand, "pId_Cliente", DbType.Int32, pParametros.CodigoClienteBmf);

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

        public static List<MonitorCustodiaInfo.CustodiaPosicao> ConsultarCustodiaNormal(MonitorCustodiaRequest pParametros)
        {
            MonitorCustodiaInfo lRetorno = new MonitorCustodiaInfo();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET2"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "IdCliente"   , DbType.Int32, pParametros.CodigoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "IdClienteBMF", DbType.Int32, pParametros.CodigoClienteBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
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
                            DtVencimento      = lDataTable.Rows[i]["DATA_VENC"].DBToDateTime(),
                            QtdeD1            = lDataTable.Rows[i]["QTDE_DA1"].DBToDecimal(),
                            QtdeD2            = lDataTable.Rows[i]["QTDE_DA2"].DBToDecimal(),
                            QtdeD3            = lDataTable.Rows[i]["QTDE_DA3"].DBToDecimal(),
                            CodigoSerie       = lDataTable.Rows[i]["COD_SERI"].DBToString(),
                            FatorCotacao      = lDataTable.Rows[i]["FAT_COT"].DBToDecimal(),
                            QtdeDATotal       = lDataTable.Rows[i]["QTDE_DATOTAL"].DBToDecimal(),
                        });
            }

            return lRetorno.ListaCustodia;
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

        }

        public List<DateTime> ObterFeriadosDI()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            string procedure = "prc_obter_relacao_feriado_DI";

            List<DateTime> lFeriadoDI = new List<DateTime>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQLConfig;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, procedure))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            //string Instrumento = (lDataTable.Rows[i]["cd_codneg"]).DBToString();
                            DateTime lFeriado = (lDataTable.Rows[i]["DT_feriado"]).DBToDateTime();

                            lFeriadoDI.Add(lFeriado);
                        }
                    }

                }

                return lFeriadoDI;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF> ConsultarCustodiaPosicaoDiaBMF(MonitorCustodiaInfo pParametros)
        {
            var lRetorno = new MonitorCustodiaInfo();
            var lAcessaDados = new AcessaDados();

            lRetorno.ListaPosicaoDiaBMF  = new List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF>();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET_DIA"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, pParametros.CodigoClienteBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        MonitorCustodiaInfo.CustodiaPosicaoDiaBMF lPosicao = new MonitorCustodiaInfo.CustodiaPosicaoDiaBMF();

                        lPosicao.CodigoInstrumento = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                        lPosicao.TipoMercado       = "FUT";
                        lPosicao.TipoGrupo         = "FUT";
                        lPosicao.IdCliente         = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32();
                        lPosicao.QtdeAtual         = 0;
                        lPosicao.QtdeDisponivel    = lDataTable.Rows[i]["QT_NEGOCIO"].DBToDecimal();
                        lPosicao.CodigoSerie       = lDataTable.Rows[i]["CD_SERIE"].DBToString();

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

            return lRetorno.ListaPosicaoDiaBMF;
        }

        public static List<MonitorCustodiaInfo.CustodiaGarantiaBMF> ConsultarFinanceiroGarantiaBMF(MonitorCustodiaInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            MonitorCustodiaInfo lRetorno = new MonitorCustodiaInfo();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GARANTIAS_BMF_DET_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, pParametros.CodigoClienteBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.ListaGarantias.Add(new MonitorCustodiaInfo.CustodiaGarantiaBMF()
                        {
                            CodigoClienteBmf      = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32(),
                            DescricaoGarantia     = lDataTable.Rows[i]["DESCRICAO"].DBToString(),
                            ValorGarantiaDeposito = lDataTable.Rows[i]["VL_GARANTIA"].DBToDecimal(),

                        });
                }
            }

            //ConsultarFinanceiroGarantiaDinheiro(ref lRetorno);

            return lRetorno.ListaGarantias;
        }

        private static void ConsultarFinanceiroGarantiaDinheiro(ref MonitorCustodiaInfo lRetorno)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GARANTIAS_BMF_DIN_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, lRetorno.CodigoClienteBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        if (lDataTable.Rows[i]["VL_DINHEIRO"].DBToDecimal() != 0)
                        {
                            lRetorno.ListaGarantias.Add(new MonitorCustodiaInfo.CustodiaGarantiaBMF()
                            {
                                CodigoClienteBmf      = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32(),
                                DescricaoGarantia     = "VALOR DINHEIRO",
                                ValorGarantiaDeposito = lDataTable.Rows[i]["VL_DINHEIRO"].DBToDecimal(),
                            });
                        }

                    }
                }
            }

        }

        public static List<MonitorCustodiaInfo.CustodiaGarantiaBMFOuro> ConsultarFinanceiroGarantiaBMFOuro(MonitorCustodiaInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            MonitorCustodiaInfo lRetorno = new MonitorCustodiaInfo();

            lRetorno.ListaGarantiasBMFOuro = new List<MonitorCustodiaInfo.CustodiaGarantiaBMFOuro>();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GAR_OURO_BMF_DET_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, pParametros.CodigoClienteBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.ListaGarantiasBMFOuro.Add(new MonitorCustodiaInfo.CustodiaGarantiaBMFOuro()
                        {
                            CodigoClienteBmf      = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32(),
                            DescricaoGarantia     = lDataTable.Rows[i]["DESCRICAO"].DBToString(),
                            ValorGarantiaDeposito = lDataTable.Rows[i]["VL_GARANTIA"].DBToDecimal(),

                        });
                }
            }

            return lRetorno.ListaGarantiasBMFOuro;
        }

        public static List<MonitorCustodiaInfo.CustodiaGarantiaBovespa> ConsultarFinanceiroGarantiaBovespa(MonitorCustodiaInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();
            MonitorCustodiaInfo lRetorno = new MonitorCustodiaInfo();

            lRetorno.ListaGarantiasBovespa = new List<MonitorCustodiaInfo.CustodiaGarantiaBovespa>();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GARANTIAS_BOV_DET_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, pParametros.CodigoClienteBov);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.ListaGarantiasBovespa.Add(new MonitorCustodiaInfo.CustodiaGarantiaBovespa()
                        {
                            CodigoClienteBov      = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32(),
                            DescricaoGarantia     = lDataTable.Rows[i]["DESCRICAO"].DBToString(),
                            ValorGarantiaDeposito = lDataTable.Rows[i]["VL_GARANTIA"].DBToDecimal(),
                            DtDeposito            = lDataTable.Rows[i]["DT_DEPOSITO"].DBToDateTime(),
                            Quantidade            = lDataTable.Rows[i]["QTDE_GARN"].DBToInt32(),
                            CodigoAtividade       = lDataTable.Rows[i]["COD_ATIV"].DBToString(),
                            FinalidadeGarantia    = lDataTable.Rows[i]["DESC_FINL_GARN"].DBToString(),
                            CodigoIsin            = lDataTable.Rows[i]["COD_ISIN"].DBToString(),
                            CodigoDistribuicao    = lDataTable.Rows[i]["NUM_DIST"].DBToInt32(),
                            NomeEmpresa           = lDataTable.Rows[i]["NOME_EMPR"].DBToString(),

                        });
                }
            }


            return lRetorno.ListaGarantiasBovespa;
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

        public Hashtable ObterCotacaoAtual(ref Hashtable htQuote)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            //Hashtable htQuote = new Hashtable();

            decimal ValorCotacao = 0;
            string IdAtivo = string.Empty;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotacoes_monitor"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            IdAtivo      = (lDataTable.Rows[i]["ds_ativo"]).DBToString();
                            ValorCotacao = (lDataTable.Rows[i]["vl_preco"]).DBToDecimal();
                            lock (htQuote)
                            {
                                htQuote.Add(IdAtivo.Trim(), ValorCotacao);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return htQuote;

        }
        
        public struct CotacaoValor
        {
            public string Ativo { get; set; }
            public decimal ValorCotacao { get; set; }
        }

        public CotacaoValor ObterCotacaoAtual(string pInstrumento)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            //Hashtable htQuote = new Hashtable();

            decimal ValorCotacao = 0;
            string IdAtivo = string.Empty;

            CotacaoValor lRetorno = new CotacaoValor();

            lRetorno.ValorCotacao = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotacoes_monitor_item"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pAtivo", DbType.AnsiString, pInstrumento);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);


                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            IdAtivo = (lDataTable.Rows[i]["ds_ativo"]).DBToString();
                            ValorCotacao = (lDataTable.Rows[i]["vl_preco"]).DBToDecimal();

                            lRetorno.Ativo = IdAtivo;
                            lRetorno.ValorCotacao = ValorCotacao;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lRetorno;

        }

        

        #endregion

        public static MonitorCustodiaInfo lRetornoInterno { get; set; }
    }
}
