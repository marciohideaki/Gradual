using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.OMS.Monitor.Custodia.Lib.Util;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using Gradual.OMS.Monitor.Custodia.Lib.Mensageria;
using System.Configuration;
using Gradual.Generico.Dados;
using Gradual.Generico.Geral;
using System.Data;
using System.Data.Common;
using System.Collections;

namespace Gradual.OMS.Monitor.Custodia.DB
{
    public class MonitorCustodiaDB
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string gNomeConexaoOracle        = "Sinacor";
        private static string gNomeCOnexaoOracleSinacor = "CORRWIN";
        private static string gNomeConexaoSQLConfig     = "Config";
        private static string gNomeConexaoSQLCadastro   = "Cadastro";
        private static string gNomeConexaoGradualOMS    = "GradualOMS";

        public static MonitorCustodiaInfo lRetornoInterno { get; set; }
        #endregion

        #region Atributos de INstrumentos de BMF
        private const string DI            = "DI1";
        private const string DOLAR         = "DOL";
        private const string INDICE        = "IND";
        private const string MINIBOLSA     = "WIN";
        private const string MINIDOLAR     = "WDL";
        private const string MINIDOLARFUT  = "WDO";
        private const string CHEIOBOI      = "BGI";
        private const string MINIBOI       = "WBG";
        private const string EURO          = "EUR";
        private const string MINIEURO      = "WEU";
        private const string CAFE          = "ICF";
        private const string MINICAFE      = "WCF";
        private const string FUTUROACUCAR  = "ISU";
        private const string ETANOL        = "ETH";
        private const string ETANOLFISICO  = "ETN";
        private const string MILHO         = "CCM";
        private const string SOJA          = "SFI";
        private const string OURO          = "OZ1";
        private const string ROLAGEMDOLAR  = "DR1";
        private const string ROLAGEMINDICE = "IR1";
        private const string ROLAGEMBOI    = "BR1";
        private const string ROLAGEMCAFE   = "CR1";
        private const string ROLAGEMMILHO  = "MR1";
        private const string ROLAGEMSOJA   = "SR1";
        #endregion

        #region Métodos
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
                            lRetornoInterno.NomeAssessor   = (lDataTable.Rows[i]["NM_ASSESSOR"]).DBToString();
                            lRetornoInterno.NomeCliente    = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            lRetornoInterno.CpfCnpj        = (lDataTable.Rows[i]["CD_CPFCGC"]).DBToString();

                            if (TipoCliente == "BOVESPA")
                            {
                                lRetornoInterno.CodigoClienteBov = (lDataTable.Rows[i]["Codigo"]).DBToInt32();
                                lRetornoInterno.StatusBov        = (lDataTable.Rows[i]["situac"]).DBToString();
                            }
                            else
                            {
                                lRetornoInterno.StatusBmf        = (lDataTable.Rows[i]["situac"]).DBToString();
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

        public static List<MonitorCustodiaInfo.CustodiaPosicao> ObterCustodiaAberturaBMF(MonitorCustodiaRequest pParametros)
        {

            DateTime InitialDate = DateTime.Now;

            AcessaDados lAcessaDados = new AcessaDados();

            List<MonitorCustodiaInfo.CustodiaPosicao> _ListaCustodiaInfo = new List<MonitorCustodiaInfo.CustodiaPosicao>();
            MonitorCustodiaInfo.CustodiaPosicao _CustodiaInfo;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_custodia_bmf_monitor2"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, pParametros.CodigoClienteBmf);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _CustodiaInfo = new MonitorCustodiaInfo.CustodiaPosicao();

                            string Instrumento = string.Empty;

                            if ((lDataTable.Rows[i]["tipo_merc"]).DBToString().Equals("OPD"))
                            {
                                Instrumento = (lDataTable.Rows[i]["cod_neg"]).DBToString();
                            }
                            else
                            {
                                Instrumento = (lDataTable.Rows[i]["cod_comm"]).DBToString() + "" + (lDataTable.Rows[i]["cod_seri"]).DBToString();
                            }

                            CotacaoValor lCotacao = ObterCotacaoAtual(Instrumento);

                            _CustodiaInfo.CodigoInstrumento = Instrumento;
                            _CustodiaInfo.QtdeAtual         = (lDataTable.Rows[i]["qtde_disp"]).DBToInt32();
                            _CustodiaInfo.TipoMercado       = "FUT";
                            //_CustodiaInfo.LoteNegociacao  = 1;
                            _CustodiaInfo.CodigoCarteira    = 0;
                            _CustodiaInfo.CodigoSerie       = (lDataTable.Rows[i]["cod_seri"]).DBToString();
                            _CustodiaInfo.IdCliente         = pParametros.CodigoClienteBmf.Value;
                            _CustodiaInfo.Cotacao           = lCotacao.ValorCotacao;
                            _CustodiaInfo.ValorFechamento   = lCotacao.ValorAjuste; //lCotacao.ValorFechamento;
                            _CustodiaInfo.Variacao          = lCotacao.Variacao;
                            _CustodiaInfo.ValorPU           =  lCotacao.ValorPU;

                            if (_CustodiaInfo.QtdeAtual != 0)
                            {
                                _ListaCustodiaInfo.Add(_CustodiaInfo);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                gLogger.Info("Ocorreu um erro ao acessar o método ObtemCustodiaAbertura");
                gLogger.Info("Descricao do erro: " + ex.Message);
                gLogger.Info("StackTrace do erro: " + ex.StackTrace);
                throw (ex);
            }

            return _ListaCustodiaInfo;

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

            try
            {
                
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET3"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, pParametros.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "IdClienteBMF", DbType.Int32, pParametros.CodigoClienteBmf);

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

                            CotacaoValor lCotacao = ObterCotacaoAtual(lDataTable.Rows[i]["COD_NEG"].DBToString());

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
                                Cotacao           = lCotacao.ValorCotacao,
                                ValorFechamento   = lCotacao.ValorFechamento,
                                Variacao          = lCotacao.Variacao,
                                //ValorPU = lCotacao.ValorPU
                            });
                        }
                }
                if (lAcessaDados != null)
                {
                    lAcessaDados.Dispose();
                    lAcessaDados = null;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return lRetorno.ListaCustodia;
        }

        public static List<MonitorCustodiaInfo.CustodiaPosicao> ConsultarCustodiaNormalSemCarteira(MonitorCustodiaRequest pParametros)
        {
            MonitorCustodiaInfo lRetorno = new MonitorCustodiaInfo();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET4"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, pParametros.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "IdClienteBMF", DbType.Int32, pParametros.CodigoClienteBmf);

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

                            CotacaoValor lCotacao = ObterCotacaoAtual(lDataTable.Rows[i]["COD_NEG"].DBToString());

                            lRetorno.ListaCustodia.Add(new MonitorCustodiaInfo.CustodiaPosicao()
                            {
                                CodigoInstrumento   = lDataTable.Rows[i]["COD_NEG"].DBToString(),
                                CodigoCarteira      = 0,//lDataTable.Rows[i]["COD_CART"].DBToInt32(),
                                DescricaoCarteira   = "",//lDataTable.Rows[i]["DESC_CART"].DBToString().Trim(),
                                TipoMercado         = lDataTable.Rows[i]["TIPO_MERC"].DBToString(),
                                TipoGrupo           = lDataTable.Rows[i]["TIPO_GRUP"].DBToString(),
                                IdCliente           = lDataTable.Rows[i]["COD_CLI"].DBToInt32(),
                                QtdeAtual           = lDataTable.Rows[i]["QTDE_ATUAL"].DBToDecimal(),
                                QtdeLiquidar        = lDataTable.Rows[i]["QTDE_LIQUID"].DBToDecimal(),
                                QtdeDisponivel      = lDataTable.Rows[i]["QTDE_DISP"].DBToDecimal(),
                                QtdeAExecVenda      = lDataTable.Rows[i]["QTDE_AEXE_VDA"].DBToDecimal(),
                                QtdeAExecCompra     = lDataTable.Rows[i]["QTDE_AEXE_CPA"].DBToDecimal(),
                                NomeEmpresa         = lDataTable.Rows[i]["NOME_EMP_EMI"].DBToString(),
                                ValorPosicao        = lDataTable.Rows[i]["VAL_POSI"].DBToDecimal(),
                                DtVencimento        = lDataTable.Rows[i]["DATA_VENC"].DBToDateTime(),
                                QtdeD1              = lDataTable.Rows[i]["QTDE_DA1"].DBToDecimal(),
                                QtdeD2              = lDataTable.Rows[i]["QTDE_DA2"].DBToDecimal(),
                                QtdeD3              = lDataTable.Rows[i]["QTDE_DA3"].DBToDecimal(),
                                CodigoSerie         = lDataTable.Rows[i]["COD_SERI"].DBToString(),
                                FatorCotacao        = lDataTable.Rows[i]["FAT_COT"].DBToDecimal(),
                                QtdeDATotal         = lDataTable.Rows[i]["QTDE_DATOTAL"].DBToDecimal(),
                                Cotacao             = lCotacao.ValorCotacao,
                                ValorFechamento     = lCotacao.ValorFechamento,
                                Variacao            = lCotacao.Variacao,
                                //ValorPU = lCotacao.ValorPU
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
            return lRetorno.ListaCustodia;
        }

        public static List<MonitorCustodiaInfo.CustodiaPosicao> ConsultarCustodiaNormalSql(MonitorCustodiaRequest pParametros)
        {
            MonitorCustodiaInfo lRetorno = new MonitorCustodiaInfo();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoGradualOMS;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_vcfposicao_risco_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@Id_Cliente", DbType.Int32, pParametros.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@Id_ClienteBMF", DbType.Int32, pParametros.CodigoClienteBmf);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            if (lDataTable.Rows[i]["TIPO_MERC"].DBToString().Equals("OPF") ||
                                lDataTable.Rows[i]["TIPO_MERC"].DBToString().Equals("FUT") ||
                                lDataTable.Rows[i]["TIPO_MERC"].DBToString().Equals("OPD"))
                            {
                                continue;
                            }

                            CotacaoValor lCotacao = ObterCotacaoAtual(lDataTable.Rows[i]["COD_NEG"].DBToString());

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
                                QtdeDATotal       = lDataTable.Rows[i]["QTDE_TOTAL"].DBToDecimal(),
                                Cotacao           = lCotacao.ValorCotacao,
                                ValorFechamento   = lCotacao.ValorFechamento,
                                Variacao          = lCotacao.Variacao,
                                //ValorPU = lCotacao.ValorPU
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
            finally
            {
                if (lAcessaDados != null)
                {
                    lAcessaDados.Dispose();
                    lAcessaDados = null;
                }
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
            finally
            {
                if (lAcessaDados != null)
                {
                    lAcessaDados.Dispose();
                    lAcessaDados = null;
                }
            }
        }

        public static List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF> ConsultarCustodiaPosicaoDiaBMF(MonitorCustodiaInfo pParametros)
        {
            var lRetorno = new MonitorCustodiaInfo();
            var lAcessaDados = new AcessaDados();

            lRetorno.ListaPosicaoDiaBMF = new List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF>();
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET_DIA"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, pParametros.CodigoClienteBmf);

                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            MonitorCustodiaInfo.CustodiaPosicaoDiaBMF lPosicao = new MonitorCustodiaInfo.CustodiaPosicaoDiaBMF();

                            CotacaoValor lCotacao = ObterCotacaoAtual(lDataTable.Rows[i]["CD_NEGOCIO"].DBToString());

                            var lPosicaoEncontrada = lRetorno.ListaPosicaoDiaBMF.Find(posicao => { return posicao.CodigoInstrumento == lDataTable.Rows[i]["CD_NEGOCIO"].DBToString(); });

                            if (string.IsNullOrEmpty(lPosicaoEncontrada.CodigoInstrumento))
                            {
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

                                //lPosicao.ValorPU           = lCotacao.ValorPU;

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
                            else
                            {
                                var lPosicaoARemover = new MonitorCustodiaInfo.CustodiaPosicaoDiaBMF();

                                lPosicaoARemover = lPosicaoEncontrada;

                                string lSentido = lDataTable.Rows[i]["CD_NATOPE"].DBToString();

                                if (lSentido.Equals("V"))
                                {
                                    lPosicaoEncontrada.QtdeAExecVenda += lDataTable.Rows[i]["QT_NEGOCIO"].DBToDecimal();

                                    lPosicaoEncontrada.PrecoNegocioVenda += lDataTable.Rows[i]["PR_NEGOCIO"].DBToDecimal();
                                }
                                else if (lSentido.Equals("C"))
                                {
                                    lPosicaoEncontrada.QtdeAExecCompra += lDataTable.Rows[i]["QT_NEGOCIO"].DBToDecimal();

                                    lPosicaoEncontrada.PrecoNegocioCompra += lDataTable.Rows[i]["PR_NEGOCIO"].DBToDecimal();
                                }

                                lRetorno.ListaPosicaoDiaBMF.Remove(lPosicaoARemover);

                                lRetorno.ListaPosicaoDiaBMF.Add(lPosicaoEncontrada);
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
            return lRetorno.ListaPosicaoDiaBMF;
        }

        public static List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF> ConsultarCustodiaPosicaoDiaBMFSql(MonitorCustodiaInfo pParametros)
        {
            var lRetorno = new MonitorCustodiaInfo();
            var lAcessaDados = new AcessaDados();

            lRetorno.ListaPosicaoDiaBMF = new List<MonitorCustodiaInfo.CustodiaPosicaoDiaBMF>();
            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoGradualOMS;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_vcfposicao_dia_risco_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@Id_ClienteBmf", DbType.Int32, pParametros.CodigoClienteBmf);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            MonitorCustodiaInfo.CustodiaPosicaoDiaBMF lPosicao = new MonitorCustodiaInfo.CustodiaPosicaoDiaBMF();

                            CotacaoValor lCotacao = ObterCotacaoAtual(lDataTable.Rows[i]["COD_NEG"].DBToString());

                            lPosicao.CodigoInstrumento = lDataTable.Rows[i]["COD_NEG"].DBToString();
                            lPosicao.TipoMercado       = "FUT";
                            lPosicao.TipoGrupo         = "FUT";
                            lPosicao.IdCliente         = lDataTable.Rows[i]["ID_CLIENTE"].DBToInt32();
                            lPosicao.QtdeAtual         = 0;
                            lPosicao.QtdeDisponivel    = lDataTable.Rows[i]["QTDE_DISP"].DBToDecimal();
                            lPosicao.CodigoSerie       = lDataTable.Rows[i]["COD_SERIE"].DBToString();
                            lPosicao.ValorFechamento   = lCotacao.ValorAbertura;
                            lPosicao.Cotacao           = lCotacao.ValorCotacao;
                            lPosicao.Variacao          = lCotacao.Variacao;
                            lPosicao.ValorPU           = lCotacao.ValorPU;
                            lPosicao.ValorAjuste       = lCotacao.ValorAjuste;

                            string lSentido = lDataTable.Rows[i]["SENTIDO"].DBToString();

                            lPosicao.Sentido = lSentido;

                            if (lSentido.Equals("V"))
                            {
                                lPosicao.QtdeAExecVenda = lDataTable.Rows[i]["qtde_aexec_venda"].DBToDecimal();

                                lPosicao.PrecoNegocioVenda = lDataTable.Rows[i]["preco_negocio_venda"].DBToDecimal();
                            }
                            else if (lSentido.Equals("C"))
                            {
                                lPosicao.QtdeAExecCompra = lDataTable.Rows[i]["qtde_aexec_compra"].DBToDecimal();

                                lPosicao.PrecoNegocioCompra = lDataTable.Rows[i]["preco_negocio_compra"].DBToDecimal();
                            }

                            lRetorno.ListaPosicaoDiaBMF.Add(lPosicao);
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

            ConsultarFinanceiroGarantiaDinheiro(ref lRetorno, pParametros);
            if (lAcessaDados != null)
            {
                lAcessaDados.Dispose();
                lAcessaDados = null;
            }

            return lRetorno.ListaGarantias;
        }

        private static void ConsultarFinanceiroGarantiaDinheiro(ref MonitorCustodiaInfo lRetorno, MonitorCustodiaInfo pParametros)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GARANTIAS_BMF_DIN_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, pParametros.CodigoClienteBmf);

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
            if (lAcessaDados != null)
            {
                lAcessaDados.Dispose();
                lAcessaDados = null;
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
            
            if (lAcessaDados != null)
            {
                lAcessaDados.Dispose();
                lAcessaDados = null;
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

            if (lAcessaDados != null)
            {
                lAcessaDados.Dispose();
                lAcessaDados = null;
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
                            IdAtivo = (lDataTable.Rows[i]["ds_ativo"]).DBToString();
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
            public string  Ativo            { get; set; }
            public decimal ValorCotacao     { get; set; }
            public decimal ValorAbertura    { get; set; }
            public decimal ValorFechamento  { get; set; }
            public decimal Variacao         { get; set; }
            public DateTime HoraCotacao     { get; set; }
            public decimal ValorAjuste      { get; set; }
            public decimal ValorPU          { get; set; }
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

        #endregion
    }
}

