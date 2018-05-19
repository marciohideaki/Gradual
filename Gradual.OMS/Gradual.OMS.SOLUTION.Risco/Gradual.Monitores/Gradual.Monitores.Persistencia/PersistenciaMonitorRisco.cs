using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.Generico.Dados;
using System.Data.Common;
using log4net;
using Gradual.Monitores.Risco.Lib;
using System.Collections;

namespace Gradual.Monitores.Persistencia
{
    public class PersistenciaMonitorRisco
    {
        private const string gNomeConexaoSinacor = "SINACOR";
        private const string gNomeConexaoSQL = "Risco";
        private const string gNomeConexaoContaMargem = "CONTAMARGEM";

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PersistenciaMonitorRisco()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public decimal? GetAccountBalance(string Account)
        {
            AcessaDados lAcessaDados = new AcessaDados();


            decimal? Balance = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, "prc_obter_saldo_monitor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCodigoCliente", DbType.Int32, Account);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            //vl_total
                            Balance = (lDataTable.Rows[i]["vl_total"]).DBToDecimal();
                        }
                    }

                }

                return Balance;
            }
            catch (Exception ex)
            {
                string message = ex.Message;

            }

            return Balance;


        }

        public List<ClientPortifolio> GetPortifolio(string Account)
        {
            string procedure = "prc_obter_portifolio_aber";

            AcessaDados lAcessaDados = new AcessaDados();
            List<ClientPortifolio> lstClientPortifolio = new List<ClientPortifolio>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, procedure))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCodigoCliente", DbType.Int32, Account);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            ClientPortifolio _ClientPortifolio = new ClientPortifolio();

                            _ClientPortifolio.Account = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            _ClientPortifolio.Instrument = (lDataTable.Rows[i]["cd_negocio"]).ToString();
                            _ClientPortifolio.Quantity = (lDataTable.Rows[i]["Qtd"]).DBToInt32();
                            //_ClientPortifolio.Price = GetPrevClosing(_ClientPortifolio.Instrument);
                            _ClientPortifolio.Volume = (_ClientPortifolio.Price * _ClientPortifolio.Quantity);

                            lstClientPortifolio.Add(_ClientPortifolio);
                        }
                    }

                }

                return lstClientPortifolio;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public decimal ObterSaldoAbertura(int Account)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal Saldo = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_saldo_abertura"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, Account);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            //vl_total
                            Saldo = (lDataTable.Rows[i]["vl_total"]).DBToDecimal();
                        }
                    }

                }

                return Saldo;
            }
            catch (Exception ex)
            {
                string message = ex.Message;

            }

            return Saldo;

        }

        public decimal ObterSaldoBMF(int Account)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoTotal = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDOCLIENTE_BMF"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pId_Cliente", DbType.Int32, Account);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {                        
                        decimal Garantias = (lDataTable.Rows[0]["VL_TOTGAR"]).DBToDecimal();
                        decimal Bloqueios = (lDataTable.Rows[0]["VL_TOTMAR"]).DBToDecimal();

                        SaldoTotal = (Garantias - Bloqueios);

                    }            

                }

                return SaldoTotal;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao calcular a posicao de BMF do cliente: " + Account.ToString() ,ex);

            }

            return SaldoTotal;

        }

        public List<ClienteFundoInfo> ObterPosicaoFundoCliente(int CodigoCliente)
        {
            AcessaDados acesso = new AcessaDados();
            List<ClienteFundoInfo> lstFundos = new List<ClienteFundoInfo>();
            ClienteFundoInfo _ClienteFundoInfo = null;

            decimal ValorBruto = 0;

            try
            {
                acesso.ConnectionStringName = "Cadastro";
                string cpfcnpj = "";

                using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "cpfcnpj_sel_sp"))
                {
                    acesso.AddInParameter(cmd, "@cd_codigo", DbType.Int32, CodigoCliente);

                    DataTable table = acesso.ExecuteDbDataTable(cmd);

                    if (table.Rows.Count > 0)
                    {
                        DataRow dr = table.Rows[0];
                        cpfcnpj = dr["ds_cpfcnpj"].DBToString().PadLeft(15, '0');
                    }
                }

                acesso = new AcessaDados();

                acesso.ConnectionStringName = "ClubesFundos";

                using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "prc_sel_posicao_cotista"))
                {

                    acesso.AddInParameter(cmd, "@dsCpfCnpj", DbType.String, cpfcnpj);

                    DataTable table = acesso.ExecuteDbDataTable(cmd);

                    foreach (DataRow dr in table.Rows)
                    {
                        _ClienteFundoInfo = new ClienteFundoInfo();
                        _ClienteFundoInfo.NomeFundo  = dr["dsRazaoSocial"].DBToString();
                        _ClienteFundoInfo.Saldo      = dr["valorBruto"].DBToDecimal();
                        lstFundos.Add(_ClienteFundoInfo);
                    }
                }

                return lstFundos;

            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public decimal ObterSituacaoFinanceiraPatrimonial(int CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoTotal = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sfp_monitor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    decimal ValorItem = 0;

                    if (lDataTable.Rows.Count > 0)
                    {
                         ValorItem = (lDataTable.Rows[0]["VL_ben"]).DBToDecimal();
                         SaldoTotal += ValorItem;
                    }
                }

                return SaldoTotal;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao calcular a situacao financeira patrimonial: " + CodigoCliente.ToString(), ex);

            }

            return SaldoTotal;

        }


        public decimal ObterSaldoContaMargem(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoContaMargem = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoContaMargem;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cliente_contamargem"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        SaldoContaMargem = (lDataTable.Rows[0]["VL_LIMITE"]).DBToDecimal();
                    }
                    else
                    {
                        SaldoContaMargem = 0;
                    }
                }

                return SaldoContaMargem;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta margem do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }
        }


        public List<PosicaoTermoInfo> ObterPosicaoTermo(int idCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            PosicaoTermoInfo _PosicaoTermo;
            List<PosicaoTermoInfo> lstTermo = new List<PosicaoTermoInfo>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_movimento_termo"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CodigoCliente", DbType.Int32, idCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {        

                            _PosicaoTermo = new PosicaoTermoInfo();

                            _PosicaoTermo.IDCliente      = (lDataTable.Rows[i]["cod_cli"]).DBToInt32();
                            _PosicaoTermo.Instrumento    = (lDataTable.Rows[i]["cod_neg"]).DBToString();
                            _PosicaoTermo.DataExecucao   = (lDataTable.Rows[i]["data_exec"]).DBToDateTime();
                            _PosicaoTermo.DataVencimento = (lDataTable.Rows[i]["data_venc"]).DBToDateTime();
                            _PosicaoTermo.Quantidade     = (lDataTable.Rows[i]["qtde_disp"]).DBToInt32();
                            _PosicaoTermo.PrecoExecucao = (lDataTable.Rows[i]["val_nego"]).DBToDecimal();

                            lstTermo.Add(_PosicaoTermo);
                        }
                    }
                }

                return lstTermo;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

        }


        public List<BTCInfo> ObterPosicaoBTC(int idCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            BTCInfo _PosicaoBTC;
            List<BTCInfo> lstBTC = new List<BTCInfo>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_posicaoclient_btc"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, idCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _PosicaoBTC = new BTCInfo();

                            _PosicaoBTC.CodigoCliente = (lDataTable.Rows[i]["COD_CLI"]).DBToInt32();
                            _PosicaoBTC.Carteira = (lDataTable.Rows[i]["COD_CART"]).DBToInt32();
                            _PosicaoBTC.Instrumento = (lDataTable.Rows[i]["COD_NEG"]).DBToString();

                            _PosicaoBTC.DataAbertura = (lDataTable.Rows[i]["DATA_ABER"]).DBToDateTime();
                            _PosicaoBTC.DataVencimento = (lDataTable.Rows[i]["DATA_VENC"]).DBToDateTime();

                            _PosicaoBTC.PrecoMedio = (lDataTable.Rows[i]["PREC_MED"]).DBToDecimal();
                            _PosicaoBTC.Quantidade = (lDataTable.Rows[i]["QTDE_ACOE"]).DBToInt32();
                            _PosicaoBTC.Remuneracao = (lDataTable.Rows[i]["VAL_LIQ"]).DBToDecimal();
                            _PosicaoBTC.Taxa = (lDataTable.Rows[i]["TAXA_REMU"]).DBToDecimal();
                            _PosicaoBTC.TipoContrato = (lDataTable.Rows[i]["TIPO_COTR"]).DBToString();                                  

                            lstBTC.Add(_PosicaoBTC);
                        }
                    }
                }

                return lstBTC;
            }
            catch (Exception ex)
            {
                throw (ex);

            }        

        }

        public List<PosicaoBmfInfo> ObtemPosicaoIntradayBMFAFTER(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            PosicaoBmfInfo _PosicaoBmfInfo;
            List<PosicaoBmfInfo> PosicaoBMF = new List<PosicaoBmfInfo>();   

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RELACAO_ORDENS_AFTER"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _PosicaoBmfInfo = new PosicaoBmfInfo();

                            _PosicaoBmfInfo.Cliente                 = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            _PosicaoBmfInfo.Contrato                = (lDataTable.Rows[i]["cd_negocio"]).DBToString();
                            _PosicaoBmfInfo.QuantidadeContato       = (lDataTable.Rows[i]["qt_negocio"]).DBToInt32();
                            _PosicaoBmfInfo.PrecoAquisicaoContrato  = (lDataTable.Rows[i]["pr_negocio"]).DBToDecimal();
                            _PosicaoBmfInfo.DataOperacao            = (lDataTable.Rows[i]["dt_datmov"]).DBToDateTime();
                            _PosicaoBmfInfo.Sentido = (lDataTable.Rows[i]["cd_natope"]).DBToString();

                            PosicaoBMF.Add(_PosicaoBmfInfo);
                        }
                    }
                }

                return PosicaoBMF;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return PosicaoBMF;


        }

        public List<PosicaoBmfInfo> ObtemPosicaoIntradayBMF(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            PosicaoBmfInfo _PosicaoBmfInfo;
            List<PosicaoBmfInfo> PosicaoBMF = new List<PosicaoBmfInfo>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RELACAO_ORDENS_BMF"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _PosicaoBmfInfo = new PosicaoBmfInfo();

                            _PosicaoBmfInfo.Cliente = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            _PosicaoBmfInfo.Contrato = (lDataTable.Rows[i]["cd_negocio"]).DBToString();
                            _PosicaoBmfInfo.QuantidadeContato = (lDataTable.Rows[i]["qt_negocio"]).DBToInt32();
                            _PosicaoBmfInfo.PrecoAquisicaoContrato = (lDataTable.Rows[i]["pr_negocio"]).DBToDecimal();
                            _PosicaoBmfInfo.DataOperacao = (lDataTable.Rows[i]["dt_datmov"]).DBToDateTime();
                            _PosicaoBmfInfo.Sentido = (lDataTable.Rows[i]["cd_natope"]).DBToString();

                            PosicaoBMF.Add(_PosicaoBmfInfo);
                        }
                    }
                }

                return PosicaoBMF;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return PosicaoBMF;


        }

        public List<CustodiaAberturaInfo> ObterCustodiaAbertura(int CodigoCliente)
        {

            DateTime InitialDate = DateTime.Now;

            AcessaDados lAcessaDados = new AcessaDados();

            List<CustodiaAberturaInfo> _ListaCustodiaInfo = new List<CustodiaAberturaInfo>();
            CustodiaAberturaInfo _CustodiaInfo;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_custo_abertura_dia"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoCliente); ;                 

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _CustodiaInfo = new CustodiaAberturaInfo();

                            _CustodiaInfo.Instrumento = (lDataTable.Rows[i]["cd_negocio"]).DBToString();
                            _CustodiaInfo.Quantidade = (lDataTable.Rows[i]["qtde_atual"]).DBToInt32();
                            _CustodiaInfo.TipoMercado = (lDataTable.Rows[i]["tipo_merc"]).DBToString();
                            _CustodiaInfo.LoteNegociacao = (lDataTable.Rows[i]["nr_lotneg"]).DBToInt32();
                            _CustodiaInfo.CodigoCarteira = (lDataTable.Rows[i]["cod_cart"]).DBToInt32();
                            
                            _CustodiaInfo.CodigoCliente = CodigoCliente;    
                           
                            if (_CustodiaInfo.Quantidade > 0)
                            {
                                _ListaCustodiaInfo.Add(_CustodiaInfo);
                            }
                        }
                    }
                }      

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método ObtemCustodiaAbertura");
                logger.Info("Descricao do erro: " + ex.Message);
                logger.Info("StackTrace do erro: " + ex.StackTrace);
                throw (ex);
            }

            return _ListaCustodiaInfo;

        }

        public List<CustodiaAberturaInfo> ObterCustodiaAberturaBMF(int CodigoCliente)
        {

            DateTime InitialDate = DateTime.Now;

            AcessaDados lAcessaDados = new AcessaDados();

            List<CustodiaAberturaInfo> _ListaCustodiaInfo = new List<CustodiaAberturaInfo>();
            CustodiaAberturaInfo _CustodiaInfo;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_custodia_bmf_monitor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, CodigoCliente); ;

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _CustodiaInfo = new CustodiaAberturaInfo();

                            string Instrumento = (lDataTable.Rows[i]["cod_comm"]).DBToString() + "" + (lDataTable.Rows[i]["cod_seri"]).DBToString();

                            _CustodiaInfo.Instrumento = Instrumento;
                            _CustodiaInfo.Quantidade = (lDataTable.Rows[i]["qtde_disp"]).DBToInt32();                          
                            _CustodiaInfo.TipoMercado = "BMF";
                            _CustodiaInfo.LoteNegociacao = 1;
                            _CustodiaInfo.CodigoCarteira = 0;

                            _CustodiaInfo.CodigoCliente = CodigoCliente;

                            if (_CustodiaInfo.Quantidade != 0)
                            {
                                _ListaCustodiaInfo.Add(_CustodiaInfo);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método ObtemCustodiaAbertura");
                logger.Info("Descricao do erro: " + ex.Message);
                logger.Info("StackTrace do erro: " + ex.StackTrace);
                throw (ex);
            }

            return _ListaCustodiaInfo;

        }

        public List<PosicaoClienteIntradayInfo> ObtemPosicaoIntraday(string ClientID)
        {

            DateTime InitialDate = DateTime.Now;

            AcessaDados lAcessaDados = new AcessaDados();

            List<PosicaoClienteIntradayInfo> _ListaCliente = new List<PosicaoClienteIntradayInfo>();
            PosicaoClienteIntradayInfo _itemCliente;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;
                //prc_obter_movimento_operacao
                //using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_movimento_risco"))
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_movimento_operacao"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCodigoCliente", DbType.Int32, ClientID.DBToInt32());

                    DateTime dtPosi = DateTime.Now;

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    TimeSpan dtFinal = (DateTime.Now - dtPosi);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _itemCliente = new PosicaoClienteIntradayInfo();

                            _itemCliente.CodigoCliente = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            _itemCliente.NomeCliente       = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            _itemCliente.Assessor          = (lDataTable.Rows[i]["cd_assessor"]).DBToString();
                            _itemCliente.PrecoMedioNegocio = (lDataTable.Rows[i]["VL_NEGOCIO"]).DBToDecimal();
                            _itemCliente.Quantidade        = (lDataTable.Rows[i]["QT_NEGOCIO"]).DBToInt32();
                            _itemCliente.Instrumento       = (lDataTable.Rows[i]["CD_NEGOCIO"]).DBToString();
                            _itemCliente.SentidoOperacao   = (lDataTable.Rows[i]["CD_NATOPE"]).DBToChar();
                            _itemCliente.VolumeOperacao    = (lDataTable.Rows[i]["VALOR"]).DBToDecimal();

                            string InstrumentoAux = _itemCliente.Instrumento;

                            if (_itemCliente.Instrumento.Substring(_itemCliente.Instrumento.Length - 1, 1) == "F")
                            {
                                InstrumentoAux = _itemCliente.Instrumento.Remove(_itemCliente.Instrumento.Length - 1);
                            }

                            _itemCliente.Instrumento = InstrumentoAux;

                            _ListaCliente.Add(_itemCliente);
                        }
                    }
                }

                TimeSpan stamp = (InitialDate - DateTime.Now);                
           
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método GetTradesIntraday");
                logger.Info("Descricao do erro: " + ex.Message);
                logger.Info("StackTrace do erro: " + ex.StackTrace);
                throw (ex);
            }

            return _ListaCliente;

        }

        public LimitesInfo ObterLimiteOperacional(int Account)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            LimitesInfo _LimitsInfo = new LimitesInfo();
         

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_limite_agrupado_preco"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idClient", DbType.Int32, Account);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteDbDataTable(lDbCommand);


                    if (lDataTable.Rows.Count > 0)
                    {

                        decimal AllocatedBuyAvista = 0;
                        decimal AllocatedBuyOptions = 0;
                        decimal BuyAVista = 0;
                        decimal BuyOptions = 0;

                        decimal AllocatedSellAvista = 0;
                        decimal AllocatedSellOptions = 0;
                        decimal SellAVista = 0;
                        decimal SellOptions = 0;


                        BuyAVista = (lDataTable.Rows[0]["valor"]).DBToDecimal();
                        AllocatedBuyAvista = (lDataTable.Rows[0]["alocado"]).DBToDecimal();

                        BuyOptions = (lDataTable.Rows[1]["valor"]).DBToDecimal();
                        AllocatedBuyOptions = (lDataTable.Rows[1]["alocado"]).DBToDecimal();

                        SellAVista = (lDataTable.Rows[2]["valor"]).DBToDecimal();
                        AllocatedSellAvista = (lDataTable.Rows[2]["alocado"]).DBToDecimal();

                        SellOptions = (lDataTable.Rows[3]["valor"]).DBToDecimal();
                        AllocatedSellOptions = (lDataTable.Rows[3]["alocado"]).DBToDecimal();

                        _LimitsInfo.LimiteAVista = (BuyAVista + SellAVista);
                        _LimitsInfo.LimiteOpcoes = (BuyOptions + SellOptions);
                        _LimitsInfo.LimiteTotal = (_LimitsInfo.LimiteAVista + _LimitsInfo.LimiteOpcoes);
                        _LimitsInfo.LimiteDisponivel = (_LimitsInfo.LimiteTotal - (AllocatedBuyAvista + AllocatedSellAvista + AllocatedSellOptions));




                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return _LimitsInfo;

        }

        public List<int> ObterClientesPosicaoDia()
        {      
            AcessaDados lAcessaDados = new AcessaDados();
            List<int> lstClientes = new List<int>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RELACAO_CLIENTE_ORDENS"))
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
                logger.Info("Ocorreu um erro ao acessar o método ObterClientesPosicaoDia.", ex);
            }

            return lstClientes;
        }


        public List<int> ObterClientesPosicaoBMFAfter()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<int> lstClientes = new List<int>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

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
                logger.Info("Ocorreu um erro ao acessar o método ObterClientesPosicaoDia.", ex);
            }

            return lstClientes;
        }


        public ClienteInfo ObterDadosCliente(int CodigoCliente)
        {

            AcessaDados lAcessaDados = new AcessaDados();
            ClienteInfo _ClienteInfo = new ClienteInfo();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_obtem_cliente_asse_monitor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0){
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string TipoCliente = (lDataTable.Rows[i]["Tipo"]).DBToString();

                            _ClienteInfo.Assessor = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
                            _ClienteInfo.NomeCliente = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();

                            if (TipoCliente == "BOVESPA"){
                                _ClienteInfo.CodigoBovespa = (lDataTable.Rows[i]["Codigo"]).DBToString();
                            }
                            else{
                                _ClienteInfo.CodigoBMF = (lDataTable.Rows[i]["Codigo"]).DBToString();
                            }

                            _ClienteInfo.Assessor = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
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

        public int ObterContaBMF(int CodigoCliente)
        {

            AcessaDados lAcessaDados = new AcessaDados();
            int ContaBMF = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_obtem_cod_bmf_monitor")){
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0) {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++){
                            ContaBMF = (lDataTable.Rows[i]["Codigo"]).DBToInt32();             
                        }
                    }

                }

                return ContaBMF;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public decimal GetQuote(string Instrument)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal ValorCotacao = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obtem_cotacao_atual"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_ativo", DbType.AnsiString, Instrument);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteDbDataTable(lDbCommand);


                    if (lDataTable.Rows.Count > 0)
                    {
                        ValorCotacao = (lDataTable.Rows[0]["vl_ultima"]).DBToDecimal();

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return ValorCotacao;

        }

        public Hashtable ObterCotacaoAtual(ref Hashtable htQuote)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            //Hashtable htQuote = new Hashtable();
            
            decimal ValorCotacao = 0;
            string IdAtivo = string.Empty;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotacoes_monitor"))
                {

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);


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

        public Hashtable ObtemCotacaoFechamento()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Hashtable htQuote = new Hashtable();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_cotacoes_fech_monitor"))
                {
                   

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string IdAtivo = (lDataTable.Rows[i]["ds_ativo"]).DBToString();
                            decimal ValorCotacao = (lDataTable.Rows[i]["vl_fechamento"]).DBToDecimal();

                            lock (htQuote)
                            {
                                htQuote.Add(IdAtivo, ValorCotacao);
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

        public Hashtable ObtemCotacaoAbertura()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Hashtable htQuote = new Hashtable();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_cotacoes_abert_monitor"))
                {


                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string IdAtivo = (lDataTable.Rows[i]["ds_ativo"]).DBToString();
                            decimal ValorCotacao = (lDataTable.Rows[i]["vl_abertura"]).DBToDecimal();

                            lock (htQuote)
                            {
                                htQuote.Add(IdAtivo, ValorCotacao);
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


        public Hashtable ObtemTipoMercadoInstrumento()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Hashtable htQuote = new Hashtable();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_tipo_mercado_papel"))
                {


                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string IdAtivo = (lDataTable.Rows[i]["cd_codneg"]).DBToString();
                            string TipoMercado = (lDataTable.Rows[i]["cd_tpmerc"]).DBToString();

                            lock (htQuote)
                            {
                                if (!htQuote.Contains(IdAtivo))
                                {
                                    htQuote.Add(IdAtivo, TipoMercado);
                                }
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
    }
}
