using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;


using log4net;
using System.Configuration;
using System.Data;
using Cortex.OMS.ServidorFIX;
using Gradual.Core.OMS.LimiteManager.Lib.Dados;


namespace Gradual.Core.OMS.LimiteManager.Database
{
    public class DbLimite
    {
        #region log4net declaration
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        private SqlConnection _sqlConn;
        private SqlCommand _sqlCommand;
        string _strConnectionStringDefault;
        string _strConnectionStringOMS;
        #endregion

        #region Properties
        public bool ConexaoIniciada
        {
            get
            {
                return !(_sqlConn == null);
            }
        }
        public bool ConexaoAberta
        {
            get
            {
                return (_sqlConn != null && _sqlConn.State == System.Data.ConnectionState.Open);
            }
        }
        
        #endregion

        public DbLimite()
        {
            _sqlConn = null;
            _sqlCommand = null;
            _strConnectionStringDefault = ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString;
            _strConnectionStringOMS = ConfigurationManager.ConnectionStrings["GradualOMS"].ConnectionString;
        }

        ~DbLimite()
        {
            if (null != _sqlCommand)
            {
                _sqlCommand.Dispose();
                _sqlCommand = null;
            }
            if (null!=_sqlConn)
            {
                _fecharConexao();
            }

        }
        private void _abrirConexao()
        {
            this._abrirConexao(_strConnectionStringDefault);
        }

        private void _abrirConexao(string strConnectionString)
        {
            if (!this.ConexaoAberta)
            {
                _sqlConn = new SqlConnection(strConnectionString);
                _sqlConn.Open();
            }
        }

        private void _fecharConexao()
        {
            try
            {
                _sqlConn.Close();
                _sqlConn.Dispose();
            }
            catch { }
        }

        public Dictionary<string, SymbolInfo> CarregarCadastroPapel()
        {
            try
            {
                Dictionary<string, SymbolInfo> ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringOMS);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_lmt_cadastropapel_sel", _sqlConn);
                int days;
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DaysSecurityList"))
                {
                    days = Convert.ToInt32(ConfigurationManager.AppSettings["DaysSecurityList"].ToString());
                }
                else
                    days = 5;
                
                _sqlCommand.Parameters.Add(new SqlParameter("@DataRegistro", DateTime.Now.AddDays(days * (-1))));
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    if (lDataSet.Tables[0].Rows.Count != 0)
                        ret = new Dictionary<string, SymbolInfo>();
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        SymbolInfo item = new SymbolInfo();
                        item.Instrumento = lRow["codigoInstrumento"].DBToString().ToUpper();
                        item.FormaCotacao = lRow["FormaCotacao"].DBToInt32();
                        item.LotePadrao = lRow["LotePadrao"].DBToInt32();
                        switch (lRow["SegmentoMercado"].DBToString())
                        {
                            case "04":
                                item.SegmentoMercado = SegmentoMercadoEnum.OPCAO;
                                break;
                            case "09":
                                item.SegmentoMercado = SegmentoMercadoEnum.OPCAO;
                                break;
                            case "01":
                                item.SegmentoMercado = SegmentoMercadoEnum.AVISTA;
                                break;
                            case "03":
                                item.SegmentoMercado = SegmentoMercadoEnum.FRACIONARIO;
                                break;
                            case "FUT":
                                break;
                        }

                        item.Trading.Instrumento = item.Instrumento.ToUpper();
                        item.Trading.DtNegocio = lRow["dt_negocio"].DBToDateTime();
                        item.Trading.DtAtualizacao = lRow["dt_atualizacao"].DBToDateTime();
                        item.Trading.VlrUltima = lRow["vl_ultima"].DBToDecimal();
                        item.Trading.VlrOscilacao = lRow["vl_oscilacao"].DBToDecimal();
                        item.Trading.VlrFechamento = lRow["vl_fechamento"].DBToDecimal();
                        ret.Add(item.Instrumento, item);
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lAdapter = null;
                lDataSet.Dispose();
                lDataSet = null;
                _sqlCommand = null;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro no carregamento do cadastro de papel: " + ex.Message, ex);
                return null;
            }
        }

        public List<TestSymbolInfo> CarregarPapeisDeTeste(string bolsa)
        {
            try
            {
                List <TestSymbolInfo> ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_fix_test_instrument", _sqlConn);
                _sqlCommand.Parameters.Add(new SqlParameter("@Exchange", bolsa));
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    if (lDataSet.Tables[0].Rows.Count != 0)
                        ret = new List<TestSymbolInfo>();
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        TestSymbolInfo item = new TestSymbolInfo();
                        item.Exchange = lRow["Exchange"].DBToString().ToUpper();
                        item.Instrument = lRow["Instrument"].DBToString().ToUpper();
                        ret.Add(item);
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lAdapter = null;
                lDataSet.Dispose();
                lDataSet = null;
                _sqlCommand = null;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro no carregamento do cadastro de papeis de teste: " + ex.Message, ex);
                return null;
            }
        }

        /*
        public TradingInfo CarregarCotacaoPapel(string symbol)
        {
            try
            {
                TradingInfo ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_sel_ativo_cotacao_oms", _sqlConn);
                _sqlCommand.Parameters.Add(new SqlParameter("@id_ativo", symbol));
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        ret = new TradingInfo();
                        ret.Instrumento = lRow["id_ativo"].DBToString();
                        ret.DtNegocio = lRow["dt_negocio"].DBToDateTime();
                        ret.DtAtualizacao = lRow["dt_atualizacao"].DBToDateTime();
                        ret.VlrUltima = lRow["vl_ultima"].DBToDecimal();
                        ret.VlrOscilacao = lRow["vl_oscilacao"].DBToDecimal();
                        ret.VlrFechamento = lRow["vl_fechamento"].DBToDecimal();
                    }
                }
                _fecharConexao();
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro no carregamento do cadastro de papel: " + ex.Message, ex);
                return null;
            }
        }
        */
        public List<int> ObterAccountHFT()
        {
            try
            {
                List<int> ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_lmt_obter_relacao_clientes_hft", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    if (lDataSet.Tables[0].Rows.Count > 0)
                        ret = new List<int>();
                        
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        ret.Add(lRow["id_cliente"].DBToInt32());
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lAdapter = null;
                lDataSet.Dispose();
                lDataSet = null;
                _sqlCommand = null;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao obter a lista de clientes: " + ex.Message, ex);
                return null;
            }
        }
        public ClientParameterPermissionInfo CarregarPermissoesParametros(int account)
        {
            try
            {
                ClientParameterPermissionInfo ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_lmt_sel_regras_cliente_oms", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@id_cliente", account));
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    if (lDataSet.Tables[0].Rows.Count > 0)
                        ret = new ClientParameterPermissionInfo();
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        ParameterPermissionClientInfo itemPermissao = new ParameterPermissionClientInfo();
                        ret.IdCliente = account;
                        itemPermissao.Especie = lRow["Especie"].DBToString();
                        itemPermissao.IdCliente = lRow["id_cliente"].DBToInt32();
                        itemPermissao.IdBolsa = lRow["id_bolsa"].DBToInt32();
                        itemPermissao.Descricao = lRow["ParametroPermissao"].DBToString();
                        itemPermissao.ValorParametro = lRow["valor"].DBToDecimal();
                        itemPermissao.ValorAlocado = lRow["vl_alocado"].DBToDecimal();
                        itemPermissao.DtValidade = lRow["dt_validade"].DBToDateTime();
                        itemPermissao.DtMovimento = lRow["dt_movimento"].DBToDateTime();
                        // Permissao
                        if (lRow["Especie"].ToString().Equals("Permissao"))
                        {
                            itemPermissao.Permissao = (RiscoPermissoesEnum)(lRow["idParametroPermissao"].DBToInt32());
                            itemPermissao.Parametro = RiscoParametrosEnum.Indefinido;
                            ret.Permissoes.Add(itemPermissao);
                        }
                        else
                        {
                            itemPermissao.Parametro = (RiscoParametrosEnum)(lRow["idParametroPermissao"].DBToInt32());
                            itemPermissao.Permissao = RiscoPermissoesEnum.Indefinido;
                            ret.Parametros.Add(itemPermissao);
                        }
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lAdapter = null;
                lDataSet.Dispose();
                lDataSet = null;
                _sqlCommand = null;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao obter as permissoes e parametros de clientes: " + ex.Message, ex);
                return null;
            }
        }

        public FatFingerInfo CarregarParametrosFatFinger(int account)
        {
            try
            {
                FatFingerInfo ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_lmt_sel_fatfinger", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@id_cliente", account));
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        ret = new FatFingerInfo();
                        ret.Account = account;
                        ret.Mercado = "BOVESPA";
                        ret.ValorRegra = lRow["vl_maximo"].DBToDecimal();
                        ret.DtValidadeRegra = lRow["dt_vencimento"].DBToDateTime();
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lDataSet.Dispose();
                _sqlCommand.Dispose();
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao obter os parametros de fat-finger de clientes: " + ex.Message, ex);
                return null;
            }
        }

        public RiskExposureClientInfo CarregarExposicaoRiscoCliente(int account, DateTime dtMovimento)
        {
            try
            {
                RiskExposureClientInfo ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_lmt_sel_exposicao_risco", _sqlConn);
                _sqlCommand.Parameters.Add(new SqlParameter("@idCliente", account));
                _sqlCommand.Parameters.Add(new SqlParameter("@dtMovimento", account));
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        ret = new RiskExposureClientInfo();
                        ret.IdCliente = lRow["id_cliente"].DBToInt32();
                        ret.LucroPrejuizo = lRow["vl_lucro_prejuizo"].DBToDecimal();
                        ret.PatrimonioLiquido = lRow["vl_patrimonio_liquido"].DBToDecimal();
                        ret.DataAtualizacao = lRow["dt_atualizacao"].DBToDateTime();

                        if (ret.LucroPrejuizo >= 0)
                        {
                            ret.LucroPrejuizo = 0;
                        }
                        else
                        {
                            ret.LucroPrejuizo = ret.LucroPrejuizo * -1;
                        }
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lDataSet.Dispose();
                _sqlCommand.Dispose();
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao obter parametros de exposicao do cliente: " + ex.Message, ex);
                return null;
            }
        }

        public List<RiskExposureGlobalInfo> CarregarExposicaoRiscoGlobal()
        {
            try
            {
                List<RiskExposureGlobalInfo> ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_lmt_sel_parametro_exposicao", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    if (lDataSet.Tables[0].Rows.Count > 0)
                        ret = new List<RiskExposureGlobalInfo>();
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        RiskExposureGlobalInfo item = new RiskExposureGlobalInfo();
                        item.OscilacaoMaxima = lRow["perc_oscilacao"].DBToDecimal();
                        item.PrejuizoMaximo = lRow["prejuizo_maximo"].DBToDecimal();
                        item.stAtivo = lRow["st_ativo"].DBToString()[0];
                        ret.Add(item);
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lDataSet.Dispose();
                _sqlCommand.Dispose();
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao obter parametros de exposicao global: " + ex.Message, ex);
                return null;
            }
        }

        public List<BlockedInstrumentInfo> CarregarPermissaoAtivo()
        {
            try
            {
                List<BlockedInstrumentInfo> ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_lmt_verificar_permissao_ativo", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if ( lDataSet.Tables.Count > 0)
                {

                    if (lDataSet.Tables[0].Rows.Count > 0)
                        ret = new List<BlockedInstrumentInfo>();
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        BlockedInstrumentInfo item = new BlockedInstrumentInfo();

                        char sentido = lRow["sentido"].DBToString()[0];
                        switch (sentido)
                        {
                            case 'V':
                                item.Sentido = SentidoBloqueioEnum.Venda; break;
                            case 'C':
                                item.Sentido = SentidoBloqueioEnum.Compra; break;
                            default:
                                item.Sentido = SentidoBloqueioEnum.Ambos; break;
                        }
                        item.Instrumento = lRow["ds_item"].DBToString();
                        ret.Add(item);
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lDataSet.Dispose();
                _sqlCommand.Dispose();
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao obter permissao global dos instrumentos: " + ex.Message, ex);
                return null;
            }
        }

        public List<BlockedInstrumentInfo> CarregarPermissaoAtivoGrupoCliente(int account)
        {
            try
            {
                List<BlockedInstrumentInfo> ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_lmt_sel_cliente_bloq_grupo", _sqlConn);
                _sqlCommand.Parameters.Add(new SqlParameter("@id_cliente", account));
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    ret = new List<BlockedInstrumentInfo>();
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        BlockedInstrumentInfo item = new BlockedInstrumentInfo();

                        char sentido = lRow["direcao"].DBToString()[0];
                        switch (sentido)
                        {
                            case 'V':
                                item.Sentido = SentidoBloqueioEnum.Venda; break;
                            case 'C':
                                item.Sentido = SentidoBloqueioEnum.Compra; break;
                            default:
                                item.Sentido = SentidoBloqueioEnum.Ambos; break;
                        }
                        item.Instrumento = lRow["ds_item"].DBToString();
                        ret.Add(item);
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lDataSet.Dispose();
                _sqlCommand.Dispose();
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao obter permissao de grupo dos instrumentos: " + ex.Message, ex);
                return null;
            }
        }

        public List<BlockedInstrumentInfo> CarregarPermissaoAtivoCliente(int account)
        {
            try
            {
                List<BlockedInstrumentInfo> ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_lmt_sel_cliente_bloqueio_instrumento", _sqlConn);
                _sqlCommand.Parameters.Add(new SqlParameter("@id_cliente", account));
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    ret = new List<BlockedInstrumentInfo>();
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        BlockedInstrumentInfo item = new BlockedInstrumentInfo();
                        char sentido = lRow["Direcao"].DBToString()[0];
                        switch (sentido)
                        {
                            case 'V':
                                item.Sentido = SentidoBloqueioEnum.Venda; break;
                            case 'C':
                                item.Sentido = SentidoBloqueioEnum.Compra; break;
                            default:
                                item.Sentido = SentidoBloqueioEnum.Ambos; break;
                        }
                        item.Instrumento = lRow["cd_ativo"].DBToString();
                        item.IdCliente = lRow["id_cliente"].DBToInt32();
                        ret.Add(item);
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lDataSet.Dispose();
                _sqlCommand.Dispose();
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao obter parametros de instrumentos do clientes: " + ex.Message, ex);
                return null;
            }
        }


        public Dictionary<string, OptionBlockInfo> CarregarVencimentoSerieOpcao()
        {
            try
            {
                Dictionary<string, OptionBlockInfo> ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_lmt_bloqueio_opcao", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    if (lDataSet.Tables[0].Rows.Count > 0)
                        ret = new Dictionary<string, OptionBlockInfo>();
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        OptionBlockInfo aux = new OptionBlockInfo();
                        aux.IdMovimento = lRow["idMovimento"].DBToInt32();
                        aux.Serie = lRow["ds_serie"].DBToString();
                        aux.DtBloqueio = lRow["dt_bloqueio"].DBToDateTime();
                        ret.Add(aux.Serie, aux);
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lDataSet.Dispose();
                _sqlCommand.Dispose();
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao obter Series de Opcao Bloqueadas: " + ex.Message, ex);
                return null;
            }
        }

        public List<OperatingLimitInfo> CarregarLimiteOperacional(int account)
        {
            try
            {
                List<OperatingLimitInfo> ret = null;
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_lmt_obter_relacao_limites", _sqlConn);
                _sqlCommand.Parameters.Add(new SqlParameter("@id_cliente", account));
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    if (lDataSet.Tables[0].Rows.Count > 0)
                        ret = new List<OperatingLimitInfo>();
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        OperatingLimitInfo item = new OperatingLimitInfo();
                        item.CodigoCliente = account;
                        item.CodigoParametroCliente = lRow["id_cliente_parametro"].DBToInt32();
                        item.DataValidade = lRow["dt_validade"].DBToDateTime();
                        item.TipoLimite = (TipoLimiteEnum)(lRow["id_parametro"].DBToInt32());
                        item.ValotTotal = lRow["vl_parametro"].DBToDecimal();
                        item.ValorAlocado = lRow["vl_alocado"].DBToDecimal();
                        item.ValorDisponivel = (item.ValotTotal - item.ValorAlocado);
                        ret.Add(item);
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lDataSet.Dispose();
                _sqlCommand.Dispose();
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao obter limite operacional do cliente: " + ex.Message, ex);
                return null;
            }
        }

        public ClientLimitBMFInfo CarregarLimitesBMF(int account)
        {
            try
            {
                ClientLimitBMFInfo ret = null;
                
                SqlDataAdapter lAdapter;
                _abrirConexao(_strConnectionStringDefault);
                DataSet lDataSet = new DataSet();
                _sqlCommand = new SqlCommand("prc_lmt_sel_limites_bmf", _sqlConn);
                _sqlCommand.Parameters.Add(new SqlParameter("@account", account));
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                lAdapter = new SqlDataAdapter(_sqlCommand);
                lAdapter.Fill(lDataSet);
                if (lDataSet.Tables.Count > 0)
                {
                    if (lDataSet.Tables[0].Rows.Count > 0 || lDataSet.Tables[0].Rows.Count > 0)
                        ret = new ClientLimitBMFInfo();
                    
                    foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                    {
                        ClientLimitContractBMFInfo ret1 = new ClientLimitContractBMFInfo();
                        ret1.Account = (lRow["account"]).DBToInt32();
                        ret1.IdClienteParametroBMF = (lRow["idClienteParametroBMF"]).DBToInt32();
                        ret1.Contrato = (lRow["Contrato"]).DBToString();
                        string sentido = lRow["sentido"].DBToString();
                        ret1.Sentido = string.IsNullOrEmpty(sentido) ? string.Empty : sentido;
                        ret1.QuantidadeTotal = (lRow["qtTotal"]).DBToInt32();
                        ret1.QuantidadeDisponivel = (lRow["qtDisponivel"]).DBToInt32();
                        ret1.DataValidade = (lRow["dtValidade"]).DBToDateTime();
                        ret1.DataMovimento = (lRow["dtMovimento"]).DBToDateTime();
                        ret1.QuantidadeMaximaOferta = (lRow["qtMaxOferta"]).DBToInt32();
                        ret.Account = ret1.Account;
                        ret.ContractLimit.Add(ret1);
                    }
                    foreach (DataRow lRow in lDataSet.Tables[1].Rows)
                    {
                        ClientLimitInstrumentBMFInfo ret2 = new ClientLimitInstrumentBMFInfo();
                        ret2.Account = ret.Account;
                        ret2.IdClienteParametroBMF = (lRow["IdClienteParametroBMF"]).DBToInt32();
                        ret2.IdClienteParametroInstrumento = (lRow["IdClienteParametroInstrumento"]).DBToInt32();
                        ret2.Instrumento = (lRow["Instrumento"]).DBToString();
                        ret2.dtMovimento = (lRow["dtMovimento"]).DBToDateTime();
                        ret2.QtTotalContratoPai = (lRow["QtTotalContratoPai"]).DBToInt32();
                        ret2.QtTotalInstrumento = (lRow["QtTotalInstrumento"]).DBToInt32();
                        ret2.QtDisponivel = (lRow["QtDisponivel"]).DBToInt32();
                        ret2.ContratoBase = (lRow["contrato"]).DBToString();
                        ret2.QuantidadeMaximaOferta = (lRow["qtMaxOferta"]).DBToInt32();
                        string sentido = lRow["sentido"].DBToString();
                        ret2.Sentido = string.IsNullOrEmpty(sentido)? string.Empty : sentido;
                        ret.InstrumentLimit.Add(ret2);
                        
                    }
                }
                _fecharConexao();
                lAdapter.Dispose();
                lDataSet.Dispose();
                _sqlCommand.Dispose();
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao obter limite de bmf do cliente: " + ex.Message, ex);
                return null;
            }

        }

    }
}
