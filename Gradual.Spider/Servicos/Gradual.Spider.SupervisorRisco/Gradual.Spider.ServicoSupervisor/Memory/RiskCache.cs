using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Spider.SupervisorRisco.Lib.Dados;
using log4net;
using Gradual.Spider.SupervisorRisco.DB.Lib;
using Gradual.Spider.DataSync.Lib.Mensagens;
using Gradual.Core.Spider.OrderFixProcessing.Lib.Dados;
using System.Threading;
using Gradual.Spider.SupervisorRisco.DB.Lib.Persistencia;
using System.Collections.Concurrent;
using System.Configuration;
using Gradual.Spider.SupervisorRisco.Lib.Util;
using Gradual.Spider.ServicoSupervisor.Calculator;
using Gradual.Spider.ServicoSupervisor.Cotacao;
using Gradual.Spider.SupervisorRisco.Lib.Handlers;

namespace Gradual.Spider.ServicoSupervisor.Memory
{
    

    public class AccountBvspBMFEventArgs : EventArgs
    {
        public EventAction Action { get; set; }
        public int AccountBvsp {get;set;}
        public int AccountBMF {get;set;}
    }

    public class BlockedInstrumentEventArgs  : EventArgs
    {
        public EventAction Action { get; set; }
        public SymbolKey SymbolKey { get; set; }
        public BlockedInstrumentInfo BlockedInstrument { get; set; }
    }

    public class ClientLimiBMFEventArgs : EventArgs
    {
        public EventAction Action { get; set; }
        public int Account { get; set; }
        public ClientLimitBMFInfo ClientLimitBMF { get; set; }
    }

    public class ClientParameterPermissionEventArgs : EventArgs
    {
        public EventAction Action { get; set; }
        public int Account { get; set; }
        public ClientParameterPermissionInfo ClientParameterPermission { get; set; }
    }

    public class FatFingerEventArgs : EventArgs
    {
        public EventAction Action { get; set; }
        public int Account { get; set; }
        public FatFingerInfo FatFinger { get; set; }
    }

    public class OperatingLimitEventArgs : EventArgs
    {
        public EventAction Action { get; set; }
        public int Account { get; set; }
        public List<OperatingLimitInfo> OperationLimit { get; set; }
    }


    public class OptionBlockEventArgs : EventArgs 
    {
        public EventAction Action { get; set; }
        public string Key { get; set; }
        public OptionBlockInfo OptionBlock { get; set; }
    }

    public class SymbolListEventArgs : EventArgs
    {
        public EventAction Action { get; set; }
        public string Instrumento { get; set; }
        public SymbolInfo Symbol { get; set; }
    }

    public class TestSymbolEventArgs : EventArgs 
    {
        public EventAction Action { get; set; }
        public string Instrumento { get; set; }
        public TestSymbolInfo TestSymbol {get; set; }
    }

    public class ContaBrokerEventArgs : EventArgs
    {
        public EventAction Action { get; set; }
        public int Account { get; set; }
        public List<ContaBrokerInfo> ContaBroker { get; set; }
    }

    public class RestrictionSymbolEventArgs : EventArgs
    {
        public EventAction Action { get; set; }
        public string Symbol { get; set; }
        public RestrictionSymbolInfo RestrictionSymbol { get; set; }
    }

    public class RestrictionGroupSymbolEventArgs : EventArgs
    {
        public EventAction Action { get; set; }
        public string IdGrupo { get; set; }
        public RestrictionGroupSymbolInfo RestrictionGroupSymbol { get; set; }
    }

    public class RestrictionGlobalEventArgs : EventArgs
    {
        public EventAction Action { get; set; }
        public int Account { get; set; }
        public RestrictionGlobalInfo RestrictionGlobal { get; set; }
    }



    public class MaxLossEventArgs : EventArgs
    {
        public EventAction Action { get; set; }
        public int Account { get; set; }
        public List<OperatingLimitInfo> MaxLoss { get; set; }
    }

    public delegate void AccountBvspBMFUpdateHandler(object sender, AccountBvspBMFEventArgs args);
    public delegate void BlockedInstrumentUpdateHandler(object sender, BlockedInstrumentEventArgs args);
    public delegate void ClientLimitBMFUpdateHandler(object sender, ClientLimiBMFEventArgs args);
    public delegate void ClienteParameterPermissionUpdateHandler( object sender, ClientParameterPermissionEventArgs args);
    public delegate void FatFingerUpdateHandler( object sender, FatFingerEventArgs args);
    public delegate void OperatingLimitUpdateHandler(object sender, OperatingLimitEventArgs args);
    public delegate void OptionBlockUpdateHandler(object sender, OptionBlockEventArgs args);
    public delegate void SymbolListUpdateHandler(object sender, SymbolListEventArgs args);
    public delegate void TestSymbolUpdateHandler(object sender, TestSymbolEventArgs args);
    public delegate void ContaBrokerUpdateHandler(object sender, ContaBrokerEventArgs args);

    public delegate void RestrictionSymbolUpdateHandler(object sender, RestrictionSymbolEventArgs args);
    public delegate void RestrictionGroupSymbolUpdateHandler(object sender, RestrictionGroupSymbolEventArgs args);
    public delegate void RestrictionGlobalUpdateHandler(object sender, RestrictionGlobalEventArgs args);

    // public delegate void PositionClientUpdateHandler(object sender, PositionClientEventArgs args);
    public delegate void MaxLossUpdateHandler(object sender, MaxLossEventArgs args);

    public class RiskCache
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        #region Private Variables
        protected ConcurrentDictionary<string, SymbolInfo> _dicSymbols;
        protected ConcurrentDictionary<int, ClientParameterPermissionInfo> _dicParameters;
        protected ConcurrentDictionary<int, FatFingerInfo> _dicFatFinger;
        protected ConcurrentDictionary<int, RiskExposureClientInfo> _dicRiskExposureClient;
        protected ConcurrentDictionary<SymbolKey, BlockedInstrumentInfo> _dicBlockedSymbolGlobal;
        protected ConcurrentDictionary<SymbolKey, BlockedInstrumentInfo> _dicBlockedSymbolGroupClient;
        protected ConcurrentDictionary<SymbolKey, BlockedInstrumentInfo> _dicBlockedSymbolClient;
        protected ConcurrentDictionary<string, OptionBlockInfo> _dicOptionSeriesBlocked;
        protected ConcurrentDictionary<int, List<OperatingLimitInfo>> _dicOperatingLimit;
        protected ConcurrentDictionary<int, ClientLimitBMFInfo> _dicClientLimitBMF;
        protected ConcurrentDictionary<string, TestSymbolInfo> _dicTestSymbols;
        private List<int> _lstIdClient;
        private List<RiskExposureGlobalInfo> _lstRiskExposureGlobal;
        private Dictionary<TipoLimiteEnum, TipoLimiteEnum> _dicDualTipoLimite;
        protected Dictionary<int, int> _dicAccBvspBmf;
        protected Dictionary<int, List<ContaBrokerInfo>> _dicContaBroker;

        // Novos Limites / Restricoes
        protected ConcurrentDictionary<string, RestrictionSymbolInfo> _dicRestrSymbol;
        protected ConcurrentDictionary<int, RestrictionGlobalInfo> _dicRestrGlobal;
        protected ConcurrentDictionary<string, RestrictionGroupSymbolInfo> _dicRestrGroupSymbol;

        // Novos Limites / PositionClient
        protected ConcurrentDictionary<int, List<PosClientSymbolInfo>> _dicPositionClient;
        protected ConcurrentDictionary<int, List<ExecSymbolInfo>> _dicExecSymbols;
        protected ConcurrentDictionary<int, SaldoCcInfo> _dicSaldoCc;

        protected ConcurrentDictionary<string, List<PosClientSymbolInfo>> _dicPosClientBySymbol;

        DbRisco _db;
        object _sync = new object();
        bool _isLoaded = false;

        // Thread Controls for database updates

        Queue<OperatingLimitInfo> _qBovespa;
        Queue<ClientLimitBMFInfo> _qBMF;
        //Queue<FixLimitInfo> _qFixLimit;
        Thread _thPersistMovto;
        Thread _thRestr;

        // PersistFixLimit _pstFixLimit;
        Dictionary<string, PersistLimit> _dicPersistArq;

        // Restricoes
        ConcurrentQueue<TORestriction> _cqRest;
        object _syncRest = new object();
        PersistGlobal _pGlobal;
        PersistGroupSymbol _pGroup;
        PersistSymbol _pSymbol;

        // Perda Maxima
        ConcurrentQueue<TOMaxLoss> _cqMaxLoss;
        object _syncMaxLoss = new object();
        Thread _thMaxLoss;
        PersistMaxLoss _pMaxLoss;


        // PositionClient
        ConcurrentQueue<TOPosClient> _cqPosClient;
        object _syncPosClient = new object();
        Thread _thPosClient;
        PersistPositionClient _pPosClient;
        
        Thread _thPosDb = null; // atualizacao das informacoes de cotacao na positionclient e talvez outras
        bool _isRunningDB = false;

        int _codCarteiraGeral = 0;


        // Auxiliary structures
        ConcurrentQueue<MDSNegocioEventArgs> _cqMds;
        Thread _thMds;
        object _syncMds = new object();
        #endregion

        #region events
        public event AccountBvspBMFUpdateHandler OnAccountBvspBMFUpdate;
        public event BlockedInstrumentUpdateHandler OnBlockedInstrumentUpdate;
        public event ClientLimitBMFUpdateHandler OnClientLimitBMFUpdate;
        public event ClienteParameterPermissionUpdateHandler OnClientParameterPermissionUpdate;
        public event FatFingerUpdateHandler OnFatFingerUpdate;
        public event OperatingLimitUpdateHandler OnOperatingLimitUpdate;
        public event OptionBlockUpdateHandler OnOptionBlockUpdate;
        public event SymbolListUpdateHandler OnSymbolListUpdate;
        public event TestSymbolUpdateHandler OnTestSymbolUpdate;
        public event ContaBrokerUpdateHandler OnContaBrokerUpdate;
        
        public event RestrictionSymbolUpdateHandler OnRestrictionSymbolUpdate;
        public event RestrictionGroupSymbolUpdateHandler OnRestrictionGroupSymbolUpdate;
        public event RestrictionGlobalUpdateHandler OnRestrictionGlobalUpdate;

        // public event PositionClientUpdateHandler OnPositionClientUpdate;

        public event MaxLossUpdateHandler OnMaxLossUpdate;
        
        #endregion

        #region Static Objects
        private static RiskCache _me = null;
        public static RiskCache Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new RiskCache();
                }

                return _me;
            }
        }
        #endregion

        #region Properties
        public bool IsDataLoaded
        {
            get { return _isLoaded; }
        }
        #endregion



        // Constructor / Destructor
        public RiskCache()
        {
            _db = null;
            _isLoaded = false;
            _pSymbol = new PersistSymbol("RestrictionSymbol");
            _pGroup = new PersistGroupSymbol("RestrictionGroupSymbol");
            _pGlobal = new PersistGlobal("RestrictionGlobal");
            _pMaxLoss = new PersistMaxLoss("MaxLoss");
            _pPosClient = new PersistPositionClient("PositionClient", true);
            if (ConfigurationManager.AppSettings.AllKeys.Contains("CodCarteiraGeral"))
                _codCarteiraGeral = Convert.ToInt32(ConfigurationManager.AppSettings["CodCarteiraGeral"].ToString());
            else
                _codCarteiraGeral = 0;
                    
        }

        ~RiskCache()
        {
        }


        #region LoadData Functions
        /// <summary>
        /// Efetuar a chamada do banco para montagem da memoria das informacoes de risco
        /// </summary>
        public void LoadRiskData(bool loadSecurityList = true)
        {
            try
            {
                lock (_sync)
                {
                    //logger.Info("LoadRiskData(): Carregando dados do Controle de risco... ");
                    _dicSymbols = new ConcurrentDictionary<string, SymbolInfo>();
                    //_dicTestSymbols = new ConcurrentDictionary<string, TestSymbolInfo>();
                    //_dicParameters = new ConcurrentDictionary<int, ClientParameterPermissionInfo>();
                    //_dicFatFinger = new ConcurrentDictionary<int, FatFingerInfo>();
                    //_dicRiskExposureClient = new ConcurrentDictionary<int, RiskExposureClientInfo>();
                    //_dicBlockedSymbolGlobal = new ConcurrentDictionary<SymbolKey, BlockedInstrumentInfo>();
                    //_dicBlockedSymbolGroupClient = new ConcurrentDictionary<SymbolKey, BlockedInstrumentInfo>();
                    //_dicBlockedSymbolClient = new ConcurrentDictionary<SymbolKey, BlockedInstrumentInfo>();
                    //_dicOptionSeriesBlocked = new ConcurrentDictionary<string, OptionBlockInfo>();
                    //_dicOperatingLimit = new ConcurrentDictionary<int, List<OperatingLimitInfo>>();
                    //_dicClientLimitBMF = new ConcurrentDictionary<int, ClientLimitBMFInfo>();
                    //_dicAccBvspBmf = new Dictionary<int, int>();
                    //_dicContaBroker = new Dictionary<int, List<ContaBrokerInfo>>();

                    //_dicRestrGlobal = new ConcurrentDictionary<int, RestrictionGlobalInfo>();
                    //_dicRestrGroupSymbol = new ConcurrentDictionary<string, RestrictionGroupSymbolInfo>();
                    //_dicRestrSymbol = new ConcurrentDictionary<string, RestrictionSymbolInfo>();

                    //_dicPositionClient = new ConcurrentDictionary<int, List<PosClientSymbolInfo>>();
                    //_dicExecSymbols = new ConcurrentDictionary<int, List<ExecSymbolInfo>>();
                    //_dicPosClientBySymbol = new ConcurrentDictionary<string, List<PosClientSymbolInfo>>();

                    //_dicSaldoCc = new ConcurrentDictionary<int, SaldoCcInfo>();

                    //_lstIdClient = new List<int>();
                    //_lstRiskExposureGlobal = new List<RiskExposureGlobalInfo>();

                    //// Operacoes Dual de Limites (atualizacao) - bovespa
                    //_dicDualTipoLimite = new Dictionary<TipoLimiteEnum, TipoLimiteEnum>();
                    //_dicDualTipoLimite.Add(TipoLimiteEnum.COMPRAAVISTA, TipoLimiteEnum.VENDAAVISTA);
                    //_dicDualTipoLimite.Add(TipoLimiteEnum.VENDAAVISTA, TipoLimiteEnum.COMPRAAVISTA);
                    //_dicDualTipoLimite.Add(TipoLimiteEnum.COMPRAOPCOES, TipoLimiteEnum.VENDAOPCOES);
                    //_dicDualTipoLimite.Add(TipoLimiteEnum.VENDAOPCOES, TipoLimiteEnum.COMPRAOPCOES);


                    //// Controles de persistencia de arquivos
                    //_dicPersistArq = new Dictionary<string, PersistLimit>();

                    //logger.Info("Criando camada banco de dados - consulta");
                    if (null == _db)
                        _db = new DbRisco();

                    //logger.Info("Carregando clientes disponiveis com tipo de permissao de roteamento via Spider");
                    //if (false == _loadClientAccounts())
                    //{
                    //    throw new Exception("Problema no carregamento da lista de id_clientes");
                    //}

                    //logger.Info("Carregando paridade de clientes account bovespa / bmf");
                    //if (false == _loadAccountsBvspBmf())
                    //{
                    //    throw new Exception("Problema no carregamento de paridade de account bovespa / bmf");
                    //}

                    //logger.Info("Carregando Parametros e Permissoes globais");
                    //if (false == _loadPermissionAndParameters())
                    //{
                    //    throw new Exception("Problema no carregamento de parametros e permissoes globais");
                    //}

                    if (loadSecurityList)
                    {
                        logger.Info("Carregando cadastro de papeis");
                        if (false == _loadInstrument())
                        {
                            logger.Info("Cadastro de papeis zerado ou nulo");
                            throw new Exception("Problema no carregamento do cadastro de papeis");
                        }
                    }
                    //else
                    //    logger.Info("Flag de Carregamento de Cadastro de papeis nao habilitado");

                    ////logger.Info("Carregando parametros de fatfinger");
                    ////if (false == _loadFatFingerParameters())
                    ////{
                    ////    throw new Exception("Problemas no carregamento de parametros de fatfinger");
                    ////}

                    //logger.Info("Carregando parametros de exposicao de risco por cliente");
                    //if (false == _loadClientRiskExposure())
                    //{
                    //    throw new Exception("Problemas no carregamento de parametros de exposicao de risco por cliente");
                    //}

                    //logger.Info("Carregando instrumentos bloqueados globais");
                    //if (false == _loadBlockedInstrument())
                    //{
                    //    throw new Exception("Problemas no carregamento de instrumentos bloqueados globais");
                    //}

                    //logger.Info("Carregando instrumentos bloqueados por grupo de clientes");
                    //if (false == _loadBlockedInstrumentPerGroup())
                    //{
                    //    throw new Exception("Problemas no carregamento de instrumentos bloqueados por grupo de clientes");
                    //}

                    //logger.Info("Carregando instrumentos bloqueados por cliente");
                    //if (false == _loadBlockedInstrumentPerClient())
                    //{
                    //    throw new Exception("Problemas no carregamento de instrumentos bloqueados por cliente");
                    //}

                    //logger.Info("Carregando serie de vencimento de opcoes");
                    //if (false == _loadOptionSeries())
                    //{
                    //    throw new Exception("Problemas no carregamento de serie de vencimento de opcoes");
                    //}

                    //logger.Info("Carregando limites operacionais");
                    //if (false == _loadOperationalLimit())
                    //{
                    //    throw new Exception("Problemas no carregamento de limites operacionais");
                    //}

                    //logger.Info("Carregando limites BMF");
                    //if (false == _loadClientLimitBMF())
                    //{
                    //    throw new Exception("Problemas no carregamento de limites bmf");
                    //}

                    //logger.Info("Carregando Contas Broker");
                    //if (false == _loadContaBroker())
                    //{
                    //    throw new Exception("Problemas no carregamento das contas broker...");
                    //}

                    //logger.Info("Carregando Restricoes Ativo");
                    //if (false == _loadRestrictionSymbol())
                    //{
                    //    throw new Exception("Problemas no carregamento das restricoes de ativos...");
                    //}

                    //logger.Info("Carregando Restricoes Grupo Ativos");
                    //if (false == _loadRestrictionGroupSymbol())
                    //{
                    //    throw new Exception("Problemas no carregamento das restricoes grupo de ativos...");
                    //}

                    //logger.Info("Carregando Restricoes Globais");
                    //if (false == _loadRestrictionGlobal())
                    //{
                    //    throw new Exception("Problemas no carregamento das restricoes globais...");
                    //}

                    //logger.Info("Carregando Posicao de Clientes - Abertura - Pode demorar um pouco...");
                    
                    //if (false == _loadPositionClient())
                    //{
                    //    throw new Exception("Problemas no carregamento de posicao de clientes - abertura");
                    //}

                    //logger.Info("Carregando Posicao de Clientes por simbolo...");
                    //// TODO [FF] - Montar abertura
                    //if (false == _loadPositionClientBySymbol())
                    //{
                    //    throw new Exception("Problemas no carregamento de posicao de clientes por simbolo - abertura");
                    //}

                    
                    //logger.Info("LoadRiskData(): Dados carregados - LimitControl... ");
                    _isLoaded = true;
                    
                    logger.Info("Iniciando Thread de Movimentacao de Limite");
                    
                    //if (null == _qBMF)  _qBMF = new Queue<ClientLimitBMFInfo>();
                    //if (null == _qBovespa) _qBovespa = new Queue<OperatingLimitInfo>();
                    //if (null == _cqRest) _cqRest = new ConcurrentQueue<TORestriction>();
                    //if (null == _cqMaxLoss) _cqMaxLoss = new ConcurrentQueue<TOMaxLoss>();
                    //if (null == _cqPosClient) _cqPosClient = new ConcurrentQueue<TOPosClient>();

                    if (null == _cqMds) _cqMds = new ConcurrentQueue<MDSNegocioEventArgs>();

                    //_thPersistMovto = new Thread(new ThreadStart(_processMvto));
                    //_thPersistMovto.Start();

                    //_thRestr = new Thread(new ThreadStart(_processRestrictionObject));
                    //_thRestr.Start();

                    //_thMaxLoss = new Thread(new ThreadStart(_processMaxLoss));
                    //_thMaxLoss.Start();
                    
                    //_thPosClient = new Thread(new ThreadStart(_processPosClient));
                    //_thPosClient.Start();

                    //_thPosDb = new Thread(new ThreadStart(_processPosClientDB));
                    //_thPosDb.Start();

                    _thMds = new Thread(new ThreadStart(_processMdsNegocio));
                    _thMds.Start();


                    

                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na cargas das informacoes de limite: " + ex.Message, ex);
                _isLoaded = false;
                throw ex;
            }
        }

        public void UnloadRiskData(bool unloadSecurityList = true)
        {
            try
            {
                lock (_sync)
                {
                    logger.Info("Unload Risk Data...");
                    _isLoaded = false;
                    if (unloadSecurityList)
                    {
                        _dicSymbols.Clear(); _dicSymbols = null;
                        _dicTestSymbols.Clear(); _dicTestSymbols = null;
                    }
                    if (null != _dicParameters) _dicParameters.Clear(); _dicParameters = null;
                    if (null != _dicFatFinger) _dicFatFinger.Clear(); _dicFatFinger = null;
                    if (null != _dicRiskExposureClient) _dicRiskExposureClient.Clear(); _dicRiskExposureClient = null;
                    if (null != _dicBlockedSymbolGlobal) _dicBlockedSymbolGlobal.Clear(); _dicBlockedSymbolGlobal = null;
                    if (null != _dicBlockedSymbolGroupClient) _dicBlockedSymbolGroupClient.Clear(); _dicBlockedSymbolGroupClient = null;
                    if (null != _dicBlockedSymbolClient) _dicBlockedSymbolClient.Clear(); _dicBlockedSymbolClient = null;
                    if (null != _dicOptionSeriesBlocked) _dicOptionSeriesBlocked.Clear(); _dicOptionSeriesBlocked = null;
                    if (null != _dicOperatingLimit) _dicOperatingLimit.Clear(); _dicOperatingLimit = null;
                    if (null != _dicClientLimitBMF) _dicClientLimitBMF.Clear(); _dicClientLimitBMF = null;
                    if (null != _lstIdClient) _lstIdClient.Clear(); _lstIdClient = null;
                    if (null != _lstRiskExposureGlobal) _lstRiskExposureGlobal.Clear(); _lstRiskExposureGlobal = null;
                    if (null != _dicDualTipoLimite) _dicDualTipoLimite.Clear(); _dicDualTipoLimite = null;
                    if (null != _dicAccBvspBmf) _dicAccBvspBmf.Clear(); _dicAccBvspBmf = null;
                    if (null != _dicContaBroker) _dicContaBroker.Clear(); _dicContaBroker = null;

                    if (null != _dicRestrGlobal) _dicRestrGlobal.Clear(); _dicRestrGlobal = null;
                    if (null != _dicRestrGroupSymbol) _dicRestrGroupSymbol.Clear(); _dicRestrGroupSymbol = null;
                    if (null != _dicRestrSymbol) _dicRestrSymbol.Clear(); _dicRestrSymbol = null;

                    if (null != _dicPositionClient) _dicPositionClient.Clear(); _dicPositionClient = null;
                    if (null != _dicExecSymbols) _dicExecSymbols.Clear(); _dicExecSymbols = null;
                    if (null != _dicPosClientBySymbol) _dicPosClientBySymbol.Clear(); _dicPosClientBySymbol = null;
                    if (null != _dicSaldoCc) _dicSaldoCc.Clear(); _dicSaldoCc = null;

                    // Parar as threads
                    logger.Info("Parando threads de atualizacao de limites...");
                    if (null != _thPersistMovto && _thPersistMovto.IsAlive)
                    {
                        _thPersistMovto.Join(500);
                        try
                        {
                            if (_thPersistMovto.IsAlive) _thPersistMovto.Abort();
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Thread _thPersisMovto aborted...");
                        }
                    }

                    logger.Info("Parando threads de atualizacao de restricoes...");
                    if (null != _thRestr && _thRestr.IsAlive)
                    {
                        _thRestr.Join(500);
                        try
                        {
                            if (_thRestr.IsAlive) _thRestr.Abort();
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Thread _thRestr aborted...");
                        }
                        _thRestr = null;
                    }

                    logger.Info("Parando threads de atualizacao de perda maxima...");
                    if (null != _thMaxLoss && _thMaxLoss.IsAlive)
                    {
                        _thMaxLoss.Join(500);
                        try
                        {
                            if (_thMaxLoss.IsAlive) _thMaxLoss.Abort();
                        }
                        catch
                        {
                            logger.Error("Thread _thMaxLoss aborted...");
                        }
                        _thMaxLoss = null;
                    }

                    logger.Info("Parando threads de atualizacao de position client...");
                    if (null != _thPosClient && _thPosClient.IsAlive)
                    {
                        _thPosClient.Join(500);
                        try
                        {
                            if (_thPosClient.IsAlive) _thPosClient.Abort();
                        }
                        catch
                        {
                            logger.Error("Thread _thPosClient aborted...");
                        }
                        _thPosClient = null;
                    }

                    logger.Info("Parando threads de atualizacao de informacoes de position client DB...");
                    if (null != _thPosDb && _thPosDb.IsAlive)
                    {
                        _thPosDb.Join(500);
                        try
                        {
                            if (_thPosDb.IsAlive) _thPosDb.Abort();
                        }
                        catch
                        {
                            logger.Error("Thread _thPosDb aborted...");
                        }
                        _thPosDb = null;
                    }

                    logger.Info("Parando threads de atualizacao de informacoes de mds...");
                    if (null != _thMds && _thMds.IsAlive)
                    {
                        _thMds.Join(500);
                        try
                        {
                            if (_thMds.IsAlive) _thMds.Abort();
                        }
                        catch
                        {
                            logger.Error("Thread _thMds aborted...");
                        }
                        _thMds = null;
                    }

                    // Limpar as filas
                    lock (_qBMF)
                    {
                        _qBMF.Clear(); _qBMF = null;
                    }
                    lock (_qBovespa)
                    {
                        _qBovespa.Clear(); _qBovespa = null;
                    }

                    _cqRest = null;

                    _dicPersistArq.Clear(); _dicPersistArq = null;
                    logger.Info("Fim Unload Risk Data...");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na limpeza das estruturas internas...: " + ex.Message, ex);
            }
        }


        private bool _loadClientAccounts(int codCliente = 0)
        {
            try
            {
                if (codCliente == 0)
                {
                    _lstIdClient = _db.CarregarContasSpider();
                    if (null == _lstIdClient)
                    {
                        logger.Info("Problemas na carga da lista dos clientes");
                        return false;
                    }
                }
                else
                {
                    lock (_lstIdClient)
                    {
                        if (!_lstIdClient.Contains(codCliente))
                            _lstIdClient.Add(codCliente);
                    }
                }
                logger.Info("_loadClientAccounts() - No. Registros: " + _lstIdClient.Count);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na carga dos codigos de cliente: " + ex.Message, ex);
                return false;
            }
        }

        private bool _loadAccountsBvspBmf()
        {
            try
            {
                Dictionary<int, int> ret = _db.CarregarAccountBvspBmf();
                if (null == _dicAccBvspBmf)
                    _dicAccBvspBmf = new Dictionary<int, int>();
                lock (_dicAccBvspBmf)
                {
                    _dicAccBvspBmf = ret;
                }

                if (ret == null)
                    _dicAccBvspBmf = new Dictionary<int, int>();
                logger.Info("_loadAccountBvsBmf() - No. Registros: " + _dicAccBvspBmf.Count);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na carga da paridade de accounts bovespa/bmf codigos de cliente: " + ex.Message, ex);
                return false;
            }
        }

        private bool _loadPermissionAndParameters(int account = 0)
        {
            try
            {
                if (account == 0)
                {
                    if (null != _lstIdClient && _lstIdClient.Count > 0)
                    {
                        foreach (int acc in _lstIdClient)
                        {
                            ClientParameterPermissionInfo aux = _db.CarregarPermissoesParametros(acc);
                            if (null != aux)
                                _dicParameters.AddOrUpdate(acc, aux, (key, oldValue) => aux);
                            else
                                logger.Info("Nao foi retornado nenhum parametro/permissao para este cliente: " + acc);
                        }

                    }
                }
                // Load only one account
                else
                {
                    ClientParameterPermissionInfo aux = _db.CarregarPermissoesParametros(account);
                    if (null != aux)
                    {
                        lock (_dicParameters) { _dicParameters.AddOrUpdate(account, aux, (key, oldValue) => aux); }
                    }
                    else
                        logger.Info("Nao foi retornado nenhum parametro/permissao para este cliente especifico: " + account);
                }
                logger.Info("_loadPermissionAndParameters() - No. Registros: " + _dicParameters.Count);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga das permissoes e parametros de acesso: " + ex.Message, ex);
                return false;
            }
        }
        private bool _loadInstrument()
        {
            try
            {
                _dicSymbols = _db.CarregarCadastroPapel();
                if (null != _dicSymbols)
                {
                    logger.Info("_loadInstrument() - No. Registros: " + _dicSymbols.Count);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga dos instrumentos: " + ex.Message, ex);
                return false;
            }
        }

        private bool _loadBlockedInstrument()
        {
            try
            {
                List<BlockedInstrumentInfo> aux = _db.CarregarPermissaoAtivo();
                if (null != aux)
                {
                    foreach (BlockedInstrumentInfo item in aux)
                    {
                        SymbolKey instKey = new SymbolKey();
                        //BlockedInstrumentInfo itemAux = null;
                        instKey.Instrument = item.Instrumento.ToUpper();
                        instKey.Side = item.Sentido;
                        if (instKey.Side != SentidoBloqueioEnum.Ambos)
                        {
                            if (!_dicBlockedSymbolGlobal.ContainsKey(instKey))
                                _dicBlockedSymbolGlobal.AddOrUpdate(instKey, item, (key, oldValue) => item);
                        }
                        else
                        {
                            // Ambos, adicionara Side C /V
                            instKey.Side = SentidoBloqueioEnum.Compra;
                            if (!_dicBlockedSymbolGlobal.ContainsKey(instKey))
                                _dicBlockedSymbolGlobal.AddOrUpdate(instKey, item, (key, oldValue) => item);
                            SymbolKey instKey2 = new SymbolKey();
                            instKey2.Instrument = item.Instrumento.ToUpper();
                            instKey2.Side = SentidoBloqueioEnum.Venda;
                            if (!_dicBlockedSymbolGlobal.ContainsKey(instKey2))
                                _dicBlockedSymbolGlobal.AddOrUpdate(instKey2, item, (key, oldValue) => item);
                        }
                    }
                }
                logger.Info("_loadBlockedInstrument() - No. Registros: " + _dicBlockedSymbolGlobal.Count);

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga dos instrumentos bloqueados: " + ex.Message, ex);
                return false;
            }
        }

        private bool _loadBlockedInstrumentPerGroup(int account = 0)
        {
            try
            {
                if (account == 0)
                {
                    if (null != _lstIdClient && _lstIdClient.Count > 0)
                    {
                        foreach (int acc in _lstIdClient)
                        {
                            List<BlockedInstrumentInfo> lst = _db.CarregarPermissaoAtivoGrupoCliente(acc);
                            foreach (BlockedInstrumentInfo item in lst)
                            {
                                if (null != item)
                                {
                                    SymbolKey instKey = new SymbolKey();
                                    instKey.Account = acc;
                                    instKey.Instrument = item.Instrumento.ToUpper();
                                    instKey.Side = item.Sentido;
                                    if (instKey.Side != SentidoBloqueioEnum.Ambos)
                                    {
                                        if (!_dicBlockedSymbolGroupClient.ContainsKey(instKey))
                                            _dicBlockedSymbolGroupClient.AddOrUpdate(instKey, item, (key, oldValue) => item);
                                    }
                                    else
                                    {
                                        // Adicionar C / V
                                        instKey.Side = SentidoBloqueioEnum.Compra;
                                        if (!_dicBlockedSymbolGroupClient.ContainsKey(instKey))
                                            _dicBlockedSymbolGroupClient.AddOrUpdate(instKey, item, (key, oldValue) => item);
                                        SymbolKey instKey2 = new SymbolKey();
                                        instKey2.Instrument = item.Instrumento.ToUpper();
                                        instKey2.Side = SentidoBloqueioEnum.Venda;
                                        instKey2.Account = acc;
                                        if (!_dicBlockedSymbolGroupClient.ContainsKey(instKey2))
                                            _dicBlockedSymbolGroupClient.AddOrUpdate(instKey2, item, (key, oldValue) => item);

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    List<BlockedInstrumentInfo> lst = _db.CarregarPermissaoAtivoGrupoCliente(account);
                    foreach (BlockedInstrumentInfo item in lst)
                    {
                        if (null != item)
                        {
                            SymbolKey instKey = new SymbolKey();
                            instKey.Account = account;
                            instKey.Instrument = item.Instrumento.ToUpper();
                            instKey.Side = item.Sentido;
                            if (instKey.Side != SentidoBloqueioEnum.Ambos)
                            {
                                lock (_dicBlockedSymbolGroupClient)
                                {
                                    if (!_dicBlockedSymbolGroupClient.ContainsKey(instKey))
                                        _dicBlockedSymbolGroupClient.AddOrUpdate(instKey, item, (key, oldValue) => item);
                                }
                            }
                            else
                            {
                                // Adicionar C / V
                                instKey.Side = SentidoBloqueioEnum.Compra;
                                if (!_dicBlockedSymbolGroupClient.ContainsKey(instKey))
                                    _dicBlockedSymbolGroupClient.AddOrUpdate(instKey, item, (key, oldValue) => item);
                                SymbolKey instKey2 = new SymbolKey();
                                instKey2.Instrument = item.Instrumento.ToUpper();
                                instKey2.Side = SentidoBloqueioEnum.Venda;
                                instKey2.Account = account;
                                lock (_dicBlockedSymbolGroupClient)
                                {
                                    if (!_dicBlockedSymbolGroupClient.ContainsKey(instKey2))
                                        _dicBlockedSymbolGroupClient.AddOrUpdate(instKey2, item, (key, oldValue) => item);
                                }
                            }
                        }
                    }
                }
                logger.Info("_loadBlockedInstrumentPerGroup() - No. Registros: " + _dicBlockedSymbolGroupClient.Count);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga dos instrumentos bloqueados por grupo: " + ex.Message, ex);
                return false;
            }
        }

        private bool _loadBlockedInstrumentPerClient(int account = 0)
        {
            try
            {
                if (account == 0)
                {
                    if (null != _lstIdClient && _lstIdClient.Count > 0)
                    {
                        foreach (int acc in _lstIdClient)
                        {
                            List<BlockedInstrumentInfo> lst = _db.CarregarPermissaoAtivoCliente(acc);
                            foreach (BlockedInstrumentInfo item in lst)
                            {
                                if (null != item)
                                {
                                    SymbolKey instKey = new SymbolKey();
                                    instKey.Account = acc;
                                    instKey.Instrument = item.Instrumento.ToUpper();
                                    instKey.Side = item.Sentido;
                                    if (instKey.Side != SentidoBloqueioEnum.Ambos)
                                    {
                                        if (!_dicBlockedSymbolClient.ContainsKey(instKey))
                                            _dicBlockedSymbolClient.AddOrUpdate(instKey, item, (key, oldValue) => item);
                                    }
                                    else
                                    {
                                        // Cadastrar C/V
                                        // Adicionar C / V
                                        instKey.Side = SentidoBloqueioEnum.Compra;
                                        if (!_dicBlockedSymbolClient.ContainsKey(instKey))
                                            _dicBlockedSymbolClient.AddOrUpdate(instKey, item, (key, oldValue) => item);
                                        SymbolKey instKey2 = new SymbolKey();
                                        instKey2.Instrument = item.Instrumento.ToUpper();
                                        instKey2.Side = SentidoBloqueioEnum.Venda;
                                        instKey2.Account = acc;
                                        if (!_dicBlockedSymbolClient.ContainsKey(instKey2))
                                            _dicBlockedSymbolClient.AddOrUpdate(instKey2, item, (key, oldValue) => item);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    List<BlockedInstrumentInfo> lst = _db.CarregarPermissaoAtivoCliente(account);
                    foreach (BlockedInstrumentInfo item in lst)
                    {
                        if (null != item)
                        {
                            SymbolKey instKey = new SymbolKey();
                            instKey.Account = account;
                            instKey.Instrument = item.Instrumento.ToUpper();
                            instKey.Side = item.Sentido;
                            if (instKey.Side != SentidoBloqueioEnum.Ambos)
                            {
                                lock (_dicBlockedSymbolClient)
                                {
                                    if (!_dicBlockedSymbolClient.ContainsKey(instKey))
                                        _dicBlockedSymbolClient.AddOrUpdate(instKey, item, (key, oldValue) => item);
                                }
                            }
                            else
                            {
                                // Cadastrar C/V
                                // Adicionar C / V
                                instKey.Side = SentidoBloqueioEnum.Compra;
                                if (!_dicBlockedSymbolClient.ContainsKey(instKey))
                                    _dicBlockedSymbolClient.AddOrUpdate(instKey, item, (key, oldValue) => item);
                                SymbolKey instKey2 = new SymbolKey();
                                instKey2.Instrument = item.Instrumento.ToUpper();
                                instKey2.Side = SentidoBloqueioEnum.Venda;
                                instKey2.Account = account;
                                lock (_dicBlockedSymbolClient)
                                {
                                    if (!_dicBlockedSymbolClient.ContainsKey(instKey2))
                                        _dicBlockedSymbolClient.AddOrUpdate(instKey2, item, (key, oldValue) => item);
                                }
                            }
                        }
                    }
                }
                logger.Info("_loadBlockedInstrumentPerClient() - No. Registros: " + _dicBlockedSymbolClient.Count);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga dos instrumentos bloqueados por cliente: " + ex.Message, ex);
                return false;
            }
        }

        private bool _loadClientLimitBMF(int account = 0)
        {
            try
            {
                if (account == 0)
                {
                    if (null != _lstIdClient && _lstIdClient.Count > 0)
                    {
                        foreach (int acc in _lstIdClient)
                        {
                            int accType;
                            int accountaux = this.ParseAccount(acc, out accType);
                            if (accType == AccType.BVSP)
                                accountaux = acc;
                            logger.Info("_loadClientLimitBMF: " + accountaux);
                            ClientLimitBMFInfo item = _db.CarregarLimitesBMF(accountaux);
                            ClientLimitBMFInfo aux = null;
                            if (null != item)
                            {
                                if (_dicClientLimitBMF.TryGetValue(accountaux, out aux))
                                    aux = item;
                                else
                                    _dicClientLimitBMF.AddOrUpdate(accountaux, item, (key, oldValue) => item);
                            }
                        }
                    }
                }
                else
                {
                    int accType;
                    int accountaux = this.ParseAccount(account, out accType);
                    if (accType == AccType.BVSP)
                        accountaux = account;
                    logger.Info("_loadClientLimitBMF: " + accountaux);
                    ClientLimitBMFInfo item = _db.CarregarLimitesBMF(accountaux);
                    if (null != item)
                        lock (_dicClientLimitBMF)
                        {
                            ClientLimitBMFInfo aux = null;
                            if (_dicClientLimitBMF.TryGetValue(accountaux, out aux))
                                aux = item;
                            else
                                _dicClientLimitBMF.AddOrUpdate(accountaux, item, (key, oldValue) => item);
                        }
                }
                logger.Info("_loadClientLimitBMF() - No. Registros: " + _dicClientLimitBMF.Count);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga dos parametros de Exposicao de risco por cliente: " + ex.Message, ex);
                return false;
            }
        }

        private bool _loadClientRiskExposure(int account = 0)
        {
            try
            {
                if (account == 0)
                {
                    if (null != _lstIdClient && _lstIdClient.Count > 0)
                    {
                        foreach (int acc in _lstIdClient)
                        {
                            RiskExposureClientInfo item = _db.CarregarExposicaoRiscoCliente(acc, DateTime.Now);
                            if (null != item)
                                _dicRiskExposureClient.AddOrUpdate(acc, item, (key, oldValue) => item);
                        }
                    }
                }
                // Load only one account
                else
                {
                    RiskExposureClientInfo item = _db.CarregarExposicaoRiscoCliente(account, DateTime.Now);
                    if (null != item)
                    {
                        lock (_dicRiskExposureClient) { _dicRiskExposureClient.AddOrUpdate(account, item, (key, oldValue) => item); }
                    }
                }
                logger.Info("_loadClientRiskExposure() - No. Registros: " + _dicRiskExposureClient.Count);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga dos parametros de Exposicao de risco por cliente: " + ex.Message, ex);
                return false;
            }
        }


        //private bool _loadFatFingerParameters(int account = 0)
        //{
        //    try
        //    {
        //        if (account == 0)
        //        {
        //            if (null != _lstIdClient && _lstIdClient.Count > 0)
        //            {
        //                foreach (int acc in _lstIdClient)
        //                {
        //                    FatFingerInfo fat = _db.CarregarParametrosFatFinger(acc);
        //                    if (null != fat)
        //                        _dicFatFinger.Add(acc, fat);
        //                }
        //            }
        //        }
        //        // Load only one account
        //        else
        //        {
        //            FatFingerInfo fat = _db.CarregarParametrosFatFinger(account);
        //            if (null != fat)
        //            {
        //                lock (_dicFatFinger) { _dicFatFinger.Add(account, fat); }
        //            }
        //        }
        //        logger.Info("_loadFatFingerParameters() - No. Registros: " + _dicFatFinger.Count);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Erro na carga das permissoes de FatFinger: " + ex.Message, ex);
        //        return false;
        //    }
        //}


        private bool _loadOperationalLimit(int account = 0)
        {
            try
            {
                if (account == 0)
                {
                    if (null != _lstIdClient && _lstIdClient.Count > 0)
                    {
                        foreach (int acc in _lstIdClient)
                        {
                            List<OperatingLimitInfo> item = _db.CarregarLimiteOperacional(acc);
                            if (null != item)
                                _dicOperatingLimit.AddOrUpdate(acc, item, (key, oldValue) => item);
                        }
                    }
                }
                else
                {
                    List<OperatingLimitInfo> item = _db.CarregarLimiteOperacional(account);
                    if (null != item)
                        lock (_dicOperatingLimit)
                        {
                            _dicOperatingLimit.AddOrUpdate(account, item, (key, oldValue) => item);
                        }
                }
                logger.Info("_loadOperationalLimit() - No. Registros: " + _dicOperatingLimit.Count);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga dos parametros de Exposicao de risco por cliente: " + ex.Message, ex);
                return false;
            }
        }

        private bool _loadOptionSeries()
        {
            try
            {
                _dicOptionSeriesBlocked = _db.CarregarVencimentoSerieOpcao();
                if (null != _dicOptionSeriesBlocked)
                    logger.Info("_loadOptionSeries() - No. Registros: " + _dicOptionSeriesBlocked.Count);
                else
                    logger.Info("_loadOptionSeries() - Nao foi retornado nenhum registro!");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga das series de opcoes de vencimento: " + ex.Message, ex);
                return false;
            }
        }

        private bool _loadContaBroker()
        {
            try
            {
                lock (_dicContaBroker)
                    _dicContaBroker = _db.CarregarContaBroker();
                if (null != _dicContaBroker)
                    logger.Info("_loadContraBroker() - No. Registros: " + _dicContaBroker.Count);
                else
                    logger.Info("_loadContraBroker() - Nao foram encontrados registros");

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga das contas broker: " + ex.Message, ex);
                return false;
            }

        }

        private bool _loadRestrictionSymbol()
        {
            try
            {
                logger.Info("Limpando movimento diario de RestrictionSymbol...");
                _db.LimparRestrictionSymbol();
                _dicRestrSymbol = _db.CarregarRestrictionSymbol();
                if (null != _dicRestrSymbol)
                    logger.Info("_loadRestrictionSymbol() - No. Registros: " + _dicRestrSymbol.Count);
                else
                    logger.Info("_loadRestrictionSymbol() - Nao foram encontrado registros");

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga de restriction symbol: " + ex.Message, ex);
                return false;
            }
        }
         
        private bool _loadRestrictionGroupSymbol()
        {
            try
            {
                logger.Info("Limpando movimento diario de RestrictionGroupSymbol...");
                _db.LimparRestrictionGroupSymbol();
                _dicRestrGroupSymbol = _db.CarregarRestrictionGroupSymbol();
                if (null != _dicRestrGroupSymbol)
                    logger.Info("_loadRestrictionGroupSymbol() - No. Registros: " + _dicRestrGroupSymbol.Count);
                else
                    logger.Info("_loadRestrictionGroupSymbol() - Nao foram encontrado registros");

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga de restriction group symbol: " + ex.Message, ex);
                return false;
            }
        }

        
        private bool _loadRestrictionGlobal()
        {
            try
            {
                logger.Info("Limpando movimento diario de RestrictionGlobal...");
                _db.LimparRestrictionGlobal();
                _dicRestrGlobal = _db.CarregarRestrictionGlobal();
                if (null != _dicRestrGlobal)
                    logger.Info("_loadRestrictionGlobal() - No. Registros: " + _dicRestrGlobal.Count);
                else
                    logger.Info("_loadRestrictionGlobal() - Nao foram encontrado registros");

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga de restriction global: " + ex.Message, ex);
                return false;
            }
        }

        //private bool _loadSaldoCC()
        //{
        //    try
        //    {
        //        DbRiscoOracle dbOracle = new DbRiscoOracle();
        //        _dicSaldoCc = dbOracle.CarregarSaldoCc();
        //        if (null != _dicSaldoCc)
        //            logger.Info("_loadSaldoCC() - No. Registros: " + _dicSaldoCc.Count);
        //        else
        //            logger.Info("_loadSaldoCC() - Nao foram encontrado registros");

        //        _db.LimparSaldoCC();
        //        foreach (KeyValuePair<int, SaldoCcInfo> item in _dicSaldoCc)
        //        {
        //            item.Value.DtMovimento = DateTime.Now;
        //            _db.AtualizarSaldoCC(item.Value);
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Erro na carga de Saldo CC: " + ex.Message, ex);
        //        return false;
        //    }

        //}



        private bool _loadPositionClient()
        {
            try
            {
                DbRiscoOracle dbOracle = new DbRiscoOracle();
                _dicPositionClient = dbOracle.CarregarPosicaoAberturaOrig(_dicSymbols);
                if (null != _dicPositionClient)
                    logger.Info("_loadPositionClient() - No. Registros: " + _dicPositionClient.Count);
                else
                    logger.Info("_loadPositionClient() - Nao foram encontrado registros");

                logger.Info("Efetuando a carga no banco de dados SQL Server...");
                logger.Info("Excluindo os registros do dia corrente...");
                _db.LimparPositionClient();
                foreach (KeyValuePair<int, List<PosClientSymbolInfo>> item in _dicPositionClient)
                {
                    List<PosClientSymbolInfo> lst = item.Value;
                    for (int i = 0; i < lst.Count; i++)
                    {
                        _db.AtualizarPositionClient(lst[i]);
                        _db.InserirPositionClientMvto(lst[i]);
                        _pPosClient.TraceInfo(lst[i]);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga de load position client: " + ex.Message, ex);
                return false;
            }
        }

        private bool _loadPositionClientBySymbol()
        {
            try
            {
                List<PosClientSymbolInfo> lstBySymbol = null;
                foreach (KeyValuePair<int, List<PosClientSymbolInfo>> item in _dicPositionClient)
                {
                    
                    foreach (PosClientSymbolInfo posCli in item.Value)
                    {
                        if (!_dicPosClientBySymbol.TryGetValue(posCli.Ativo, out lstBySymbol))
                        {
                            lstBySymbol = new List<PosClientSymbolInfo>();
                            _dicPosClientBySymbol.AddOrUpdate(posCli.Ativo, lstBySymbol, (key, oldValue) =>lstBySymbol);
                        }
                        lstBySymbol.Add(posCli);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga de load position client by symbol: " + ex.Message, ex);
                return false;
            }
        }

        public int ParseAccount(int account, out int marketAcc)
        {

            int conta;
            marketAcc = AccType.BVSP_BMF;
            // Bovespa e retorna bmf
            if (_dicAccBvspBmf.TryGetValue(account, out conta))
            {
                marketAcc = AccType.BMF;
                return conta;
            }
            if (_dicAccBvspBmf.ContainsValue(account))
            {
                KeyValuePair<int, int> key = _dicAccBvspBmf.FirstOrDefault(x => x.Value == account);
                conta = key.Key;
                marketAcc = AccType.BVSP;
                return conta;
            }
            conta = account;
            return conta;
        }
        #endregion

        private void AddOrUpdateOperatingLimit(int account, OperatingLimitInfo limit)
        {
            try
            {
                List<OperatingLimitInfo> lst;
                OperatingLimitEventArgs args = new OperatingLimitEventArgs();
                args.Account = account;

                if (_dicOperatingLimit.ContainsKey(account))
                {
                    lst = _dicOperatingLimit[account];

                    if (lst.Contains(limit))
                        lst.Remove(limit);

                    lst.Add(limit);
                    _dicOperatingLimit[account] = lst;

                    args.Action = EventAction.UPDATE;
                    args.OperationLimit = _dicOperatingLimit[account].ToList();
                }
                else
                {
                    lst = new List<OperatingLimitInfo>();
                    lst.Add(limit);
                    _dicOperatingLimit.AddOrUpdate(account, lst, (key, oldValue) => lst);

                    args.Action = EventAction.INSERT;
                    args.OperationLimit = _dicOperatingLimit[account].ToList();
                }

                if (OnOperatingLimitUpdate != null)
                {
                    OnOperatingLimitUpdate(this, args);
                }
            }
            catch (Exception ex)
            {
                logger.Error("AddOrUpdateOperatingLimit: " + ex.Message, ex);
            }

        }

        private void RemoveOperatingLimit(int account)
        {
            try
            {
                if (_dicOperatingLimit.ContainsKey(account))
                {
                    List<OperatingLimitInfo> lst; 
                    //_dicOperatingLimit[account].Clear();
                    _dicOperatingLimit.TryRemove(account, out lst);
                    
                    OperatingLimitEventArgs args = new OperatingLimitEventArgs();

                    args.OperationLimit = lst;
                    args.Account = account;
                    args.Action = EventAction.DELETE;

                    if (OnOperatingLimitUpdate != null)
                    {
                        OnOperatingLimitUpdate(this, args);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("RemoveOperatingLimit: " + ex.Message, ex);
            }
        }

        private void AddOrUpdateLimitBMF(int account, ClientLimitBMFInfo limit)
        {
            try
            {

                ClientLimiBMFEventArgs args = new ClientLimiBMFEventArgs();
                args.Account = account;
                args.ClientLimitBMF = limit;

                if (_dicClientLimitBMF.ContainsKey(account))
                {
                    _dicClientLimitBMF[account] = limit;

                    args.Action = EventAction.UPDATE;
                }
                else
                {
                    _dicClientLimitBMF.AddOrUpdate(account, limit, (key, oldValue) => limit);
                }

                if (OnClientLimitBMFUpdate != null)
                {
                    OnClientLimitBMFUpdate(this, args);
                }
            }
            catch (Exception ex)
            {
                logger.Error("AddOrUpdateLimitBMF: " + ex.Message, ex);
            }
        }


        private void RemoveLimitBMF(int account)
        {
            try
            {
                if (_dicClientLimitBMF.ContainsKey(account))
                {
                    ClientLimitBMFInfo limit = null;// _dicClientLimitBMF[account];
                    _dicClientLimitBMF.TryRemove(account, out limit);

                    ClientLimiBMFEventArgs args = new ClientLimiBMFEventArgs();

                    args.ClientLimitBMF = limit;
                    args.Account = account;
                    args.Action = EventAction.DELETE;

                    if (OnClientLimitBMFUpdate != null)
                    {
                        OnClientLimitBMFUpdate(this, args);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("RemoveLimitBMF: " + ex.Message, ex);
            }
        }


        public Dictionary<int,int> SnapshotAccountBvspBMF()
        {
            Dictionary<int, int> ret = new Dictionary<int, int>();
            try
            {
                lock (_sync)
                {
                    if (null != _dicAccBvspBmf)
                    {
                        KeyValuePair<int, int>[] items = _dicAccBvspBmf.ToArray();
                        foreach (KeyValuePair<int, int> item in items)
                        {
                            ret.Add(item.Key, item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("AccountBvspBMF sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na composicao do snapshot AccountBvspBmf: " + ex.Message, ex);
            }
            return ret;
        }

        public ConcurrentDictionary<SymbolKey, BlockedInstrumentInfo> SnapshotBlockedInstrument(BlockedInstrumentMsgType blockedInstrumentMsgType)
        {
            ConcurrentDictionary<SymbolKey, BlockedInstrumentInfo> ret = new ConcurrentDictionary<SymbolKey, BlockedInstrumentInfo>();

            KeyValuePair<SymbolKey, BlockedInstrumentInfo>[] items = null;
            try
            {
                lock (_sync)
                {
                    if (blockedInstrumentMsgType == BlockedInstrumentMsgType.BlockedSymbolClient)
                        items = null != _dicBlockedSymbolClient ? _dicBlockedSymbolClient.ToArray() :null;
                    else
                        if (blockedInstrumentMsgType == BlockedInstrumentMsgType.BlockedSymbolGroupClient)
                            items = null != _dicBlockedSymbolGroupClient?_dicBlockedSymbolGroupClient.ToArray(): null;
                        else
                            items = null != _dicBlockedSymbolGlobal ? _dicBlockedSymbolGlobal.ToArray(): null;
                }

                if (items != null)
                {
                    foreach (KeyValuePair<SymbolKey, BlockedInstrumentInfo> item in items)
                    {
                        ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                    }
                }
                else
                {
                    logger.Info("BlockedInstrument sem registros...");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot BlocketInstrument: " + ex.Message, ex);
            }
            return ret;
        }

        public ConcurrentDictionary<int, ClientLimitBMFInfo> SnapshotClientLimitBMF()
        {
            ConcurrentDictionary<int, ClientLimitBMFInfo> ret = new ConcurrentDictionary<int, ClientLimitBMFInfo>();
            try
            {
                lock (_sync)
                {
                    if (null != _dicClientLimitBMF)
                    {
                        KeyValuePair<int, ClientLimitBMFInfo>[] items = _dicClientLimitBMF.ToArray();

                        foreach (KeyValuePair<int, ClientLimitBMFInfo> item in items)
                        {
                            ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("ClientLimitBMF sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot ClientLimitBMF: " + ex.Message, ex);
            }
            return ret;
        }

        public ConcurrentDictionary<int, ClientParameterPermissionInfo> SnapshotClientParameterPermission()
        {
            ConcurrentDictionary<int, ClientParameterPermissionInfo> ret = new ConcurrentDictionary<int, ClientParameterPermissionInfo>();
            try
            {
                lock (_sync)
                {
                    if (null != _dicParameters)
                    {
                        KeyValuePair<int, ClientParameterPermissionInfo>[] items = _dicParameters.ToArray();
                        foreach (KeyValuePair<int, ClientParameterPermissionInfo> item in items)
                        {
                            ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("ClientParameterPermission sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot ClientParameterPermission: " + ex.Message, ex);
            }
            return ret;
        }

        public ConcurrentDictionary<int, FatFingerInfo> SnapshotFatFinger()
        {
            ConcurrentDictionary<int, FatFingerInfo> ret = new ConcurrentDictionary<int, FatFingerInfo>();
            try
            {
                lock (_sync)
                {
                    if (null != _dicFatFinger)
                    {
                        KeyValuePair<int, FatFingerInfo>[] items = _dicFatFinger.ToArray();

                        foreach (KeyValuePair<int, FatFingerInfo> item in items)
                        {
                            ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("FatFinger sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot FatFinger: " + ex.Message, ex);
            }
            return ret;
        }
        /// <summary>
        /// SnapshotOperatingLimit tambem já envia os limites de MaxLoss
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<int, List<OperatingLimitInfo>> SnapshotOperatingLimit()
        {
            ConcurrentDictionary<int, List<OperatingLimitInfo>> ret = new ConcurrentDictionary<int, List<OperatingLimitInfo>>();
            try
            {
                lock (_sync)
                {
                    if (null != _dicOperatingLimit)
                    {
                        KeyValuePair<int, List<OperatingLimitInfo>>[] items = _dicOperatingLimit.ToArray();
                        foreach (KeyValuePair<int, List<OperatingLimitInfo>> item in items)
                        {
                            ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("OperatingLimit sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot OperatingLimit: " + ex.Message, ex);
            }
            return ret;
        }

        public ConcurrentDictionary<string, OptionBlockInfo> SnapshotOptionBlock()
        {
            ConcurrentDictionary<string, OptionBlockInfo> ret = new ConcurrentDictionary<string, OptionBlockInfo>();

            try
            {
                lock (_sync)
                {
                    if (null != _dicOptionSeriesBlocked)
                    {
                        KeyValuePair<string, OptionBlockInfo>[] items = _dicOptionSeriesBlocked.ToArray();

                        foreach (KeyValuePair<string, OptionBlockInfo> item in items)
                        {
                            ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("OptionSeriesBlocked sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot OptionBlock: " + ex.Message, ex);
            }
                 

            return ret;
        }

        public ConcurrentDictionary<string, SymbolInfo> SnapshotSymbolList()
        {
            ConcurrentDictionary<string, SymbolInfo> ret = new ConcurrentDictionary<string, SymbolInfo>();
            try
            {
                lock (_sync)
                {
                    if (null != _dicSymbols)
                    {
                        KeyValuePair<string, SymbolInfo>[] items = _dicSymbols.ToArray();

                        foreach (KeyValuePair<string, SymbolInfo> item in items)
                        {
                            ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("Symbols sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot SymbolList: " + ex.Message, ex);
            }
            return ret;
        }

        public ConcurrentDictionary<string, TestSymbolInfo> SnapshotTestSymbol()
        {
            ConcurrentDictionary<string, TestSymbolInfo> ret = new ConcurrentDictionary<string, TestSymbolInfo>();
            try
            {
                lock (_sync)
                {
                    if (null != _dicTestSymbols)
                    {
                        KeyValuePair<string, TestSymbolInfo>[] items = _dicTestSymbols.ToArray();

                        foreach (KeyValuePair<string, TestSymbolInfo> item in items)
                        {
                            ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);

                        }
                    }
                    else
                    {
                        logger.Info("TestSymbols sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot TestSymbol: " + ex.Message, ex);
            }
            return ret;
        }

        public Dictionary<int, List<ContaBrokerInfo>> SnapshotContaBroker()
        {
            Dictionary<int, List<ContaBrokerInfo>> ret = new Dictionary<int, List<ContaBrokerInfo>>();
            try
            {
                lock (_sync)
                {
                    if (null != _dicContaBroker)
                    {
                        KeyValuePair<int, List<ContaBrokerInfo>>[] items = _dicContaBroker.ToArray();

                        foreach (KeyValuePair<int, List<ContaBrokerInfo>> item in items)
                        {
                            ret.Add(item.Key, item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("ContaBroker sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot ContaBroker: " + ex.Message, ex);
            }
            return ret;
        }

        public ConcurrentDictionary<string, RestrictionSymbolInfo> SnapshotRestrictionSymbol()
        {
            ConcurrentDictionary<string, RestrictionSymbolInfo> ret = new ConcurrentDictionary<string, RestrictionSymbolInfo>();
            try
            {
                lock (_sync)
                {
                    if (null != _dicRestrSymbol)
                    {
                        KeyValuePair<string, RestrictionSymbolInfo>[] items = _dicRestrSymbol.ToArray();

                        foreach (KeyValuePair<string, RestrictionSymbolInfo> item in items)
                        {
                            ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("Restriction Symbol sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot Restriction Symbol: " + ex.Message, ex);
            }
            return ret;
        }

        public ConcurrentDictionary<string, RestrictionGroupSymbolInfo> SnapshotRestrictionGroupSymbol()
        {
            ConcurrentDictionary<string, RestrictionGroupSymbolInfo> ret = new ConcurrentDictionary<string, RestrictionGroupSymbolInfo>();
            try
            {
                lock (_sync)
                {
                    if (null != _dicRestrGroupSymbol)
                    {
                        KeyValuePair<string, RestrictionGroupSymbolInfo>[] items = _dicRestrGroupSymbol.ToArray();

                        foreach (KeyValuePair<string, RestrictionGroupSymbolInfo> item in items)
                        {
                            ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("Restriction Group Symbol sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot Restriction Group Symbol: " + ex.Message, ex);
            }
            return ret;
        }

        public ConcurrentDictionary<int, RestrictionGlobalInfo> SnapshotRestrictionGlobal()
        {
            ConcurrentDictionary<int, RestrictionGlobalInfo> ret = new ConcurrentDictionary<int, RestrictionGlobalInfo>();
            try
            {
                lock (_sync)
                {
                    if (null != _dicRestrGlobal)
                    {
                        KeyValuePair<int, RestrictionGlobalInfo>[] items = _dicRestrGlobal.ToArray();

                        foreach (KeyValuePair<int, RestrictionGlobalInfo> item in items)
                        {
                            ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("Restriction Global sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot Restriction Global: " + ex.Message, ex);
            }
            return ret;
        }

        public ConcurrentDictionary<int, List<PosClientSymbolInfo>> SnapshotPositionClient()
        {
            ConcurrentDictionary<int, List<PosClientSymbolInfo>> ret = new ConcurrentDictionary<int, List<PosClientSymbolInfo>>();
            try
            {
                lock (_sync)
                {
                    if (null != _dicPositionClient)
                    {
                        KeyValuePair<int, List<PosClientSymbolInfo>>[] items = _dicPositionClient.ToArray();

                        foreach (KeyValuePair<int, List<PosClientSymbolInfo>> item in items)
                        {
                            ret.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        }
                    }
                    else
                    {
                        logger.Info("Restriction Global sem registros...");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na montagem do snapshot Restriction Global: " + ex.Message, ex);
            }
            return ret;
        }

        public SymbolInfo UpdateSymbol(string instrumento, SymbolInfo info)
        {
            try
            {
                SymbolListEventArgs args = new SymbolListEventArgs();
                SymbolInfo ret = null;
                if (_dicSymbols.ContainsKey(instrumento) )
                {
                    _dicSymbols[instrumento].DtNegocio = info.DtNegocio;
                    _dicSymbols[instrumento].VlrUltima = info.VlrUltima;
                    _dicSymbols[instrumento].VlrOscilacao = info.VlrOscilacao;
                    
                    args.Action = EventAction.UPDATE;
                    args.Instrumento = instrumento;
                    args.Symbol = _dicSymbols[instrumento];
                    ret = args.Symbol;
                    if (OnSymbolListUpdate != null)
                    {
                        OnSymbolListUpdate(this, args);
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("UpdateSymbol: " + ex.Message, ex);
                return null;
            }
        }


        /// <summary>
        /// Atualizacao das informacoes de cotacao da position client e recalculo de L/P e Financeiro Net
        /// </summary>
        /// <param name="symbol"></param>
        public void UpdateMdsPositionClient(SymbolInfo symbol)
        {
            try
            {
                List<PosClientSymbolInfo> lst = null;
                if (_dicPosClientBySymbol.TryGetValue(symbol.Instrumento, out lst))
                {
                    
                    List<PosClientSymbolInfo> lstXpto = lst.Where(x => x.UltPreco != symbol.VlrUltima).ToList();
                    if (lstXpto == null || lstXpto.Count==0)
                        return;
                    if (lstXpto[0].Bolsa.Equals(Exchange.Bovespa))
                    {

                        for (int i = 0; i < lstXpto.Count; i++)
                        {
                            PosClientSymbolInfo c = lstXpto[i];
                            c.Variacao = symbol.VlrOscilacao;
                            c.UltPreco = symbol.VlrUltima;
                            c.FinancNet = (c.NetExec * symbol.VlrUltima);
                            c.LucroPrej = ((c.PcMedC * c.QtdExecC) - (c.PcMedV * c.QtdExecV)) - c.FinancNet;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < lstXpto.Count; i++)
                        {
                            PosClientSymbolInfo c = lstXpto[i];
                            c.Variacao = symbol.VlrOscilacao;
                            c.UltPreco = symbol.VlrUltima;
                            BmfCalculator.Instance.CalcularPosicaoBmf(c);
                        }
                    }

                    // Efetuar ativacao do evento
                    List<PosClientSymbolInfo> lstXx = lstXpto.Where(x => !x.QtdExecC.Equals(decimal.Zero) || !x.QtdExecV.Equals(decimal.Zero)).ToList();
                    int len = lstXx.Count;
                    
                    //for (int i = 0; i < len; i++)
                    //{
                    //    if (OnPositionClientUpdate != null)
                    //    {
                    //        PositionClientEventArgs sync = new PositionClientEventArgs();
                    //        sync.Action = EventAction.UPDATE;
                    //        sync.Account = lstXx[i].Account;
                    //        lstXx[i].DtMovimento = DateTime.Now;
                    //        lstXx[i].MsgId++;
                    //        lstXx[i].EventSource = "MDS";
                    //        List<PosClientSymbolInfo> lstAux = new List<PosClientSymbolInfo>();
                    //        lstAux.Add(lstXx[i]);
                    //        // sync.PosClient = lstPosCli;
                    //        sync.PosClient = lstAux;
                    //        OnPositionClientUpdate(this, sync);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                logger.Error("UpdateMdsPositionClient: " + ex.Message, ex);
            }
        }



        #region "Limit Calculation"
        /// <summary>
        /// Calculo de perda maxima 
        /// </summary>
        /// <param name="ord"></param>
        public void CalculateMaxLoss(TOSpOrder toOrd)
        {
            try
            {
                SpiderOrderInfo ord = toOrd.Order;
                 //Verificar se o exchange eh Bovespa para efetuar calculo de perda maxima
                 if (ord.Exchange.Equals(Exchange.Bovespa, StringComparison.InvariantCultureIgnoreCase))
                 {
                    // Compor / atualizar a memoria em caso de nao existir
                    OperatingLimitInfo maxLoss = _createMaxLossLimit(ord);
                    // OperatingLimitInfo maxLoss = null;
                    if (maxLoss != null)
                    {
                        List<PosClientSymbolInfo> lstOut = null;
                        // Buscar PositionClient da conta
                        if (_dicPositionClient.TryGetValue(ord.Account, out lstOut))
                        {
                            // Clonar a lista para evitar problemas de enumeration
                            List<PosClientSymbolInfo> lstAux = new List<PosClientSymbolInfo>();
                            lock (lstOut)
                                lstAux.AddRange(lstOut);
                            decimal lucprj = decimal.Zero;
                            
                            // Efetuar a somatoria das variaveis
                            if (maxLoss.TipoLimite == TipoLimiteEnum.PERDAMAXIMAAVISTA)
                                lucprj = lstAux.Select(x => x).Where(x => x.SegmentoMercado == SegmentoMercadoEnum.AVISTA).Sum(x => x.LucroPrej);
                            if (maxLoss.TipoLimite == TipoLimiteEnum.PERDAMAXIMAOPCOES)
                                lucprj = lstAux.Select(x => x).Where(x => x.SegmentoMercado == SegmentoMercadoEnum.OPCAO).Sum(x => x.LucroPrej);
                            
                            maxLoss.CodigoCliente = ord.Account;
                            maxLoss.ValorAlocado = lucprj;
                            maxLoss.ValorMovimento = lucprj;
                            TOMaxLoss to = new TOMaxLoss();
                            to.MaxLoss = maxLoss;
                            this._addMaxLossObject(to);

                            if (OnMaxLossUpdate != null)
                            {
                                MaxLossEventArgs args = new MaxLossEventArgs();
                                args.Action = EventAction.UPDATE;
                                args.Account = ord.Account;
                                args.MaxLoss = new List<OperatingLimitInfo>();
                                args.MaxLoss.Add(maxLoss);
                                OnMaxLossUpdate(this, args);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no calculo de Perda Maxima: " + ex.Message, ex);
            }
        }
        private OperatingLimitInfo _createMaxLossLimit(SpiderOrderInfo ord)
        {
            try
            {
                OperatingLimitInfo ret = null;
                // Verificar se o account e limites existem no limite operacional

                
                List<OperatingLimitInfo> lstLmt = null;
                if (!_dicOperatingLimit.TryGetValue(ord.Account, out lstLmt))
                {
                    lstLmt = new List<OperatingLimitInfo>();
                    OperatingLimitInfo aux = new OperatingLimitInfo();
                    aux.TipoLimite = TipoLimiteEnum.PERDAMAXIMAAVISTA;
                    OperatingLimitInfo aux2 = new OperatingLimitInfo();
                    aux2.TipoLimite = TipoLimiteEnum.PERDAMAXIMAOPCOES;
                    lock (lstLmt)
                    {
                        lstLmt.Add(aux);
                        lstLmt.Add(aux2);
                    }
                    _dicOperatingLimit.AddOrUpdate(ord.Account, lstLmt, (key, oldValue) => lstLmt);
                }
                else
                {
                    // Validar se tem o limite na collection
                    if (!lstLmt.Exists(x => x.TipoLimite == TipoLimiteEnum.PERDAMAXIMAOPCOES))
                    {
                        OperatingLimitInfo aux = new OperatingLimitInfo();
                        aux.TipoLimite = TipoLimiteEnum.PERDAMAXIMAOPCOES;
                        lock(lstLmt)
                            lstLmt.Add(aux);
                    }
                    // Validar se tem o limite na collection
                    if (!lstLmt.Exists(x => x.TipoLimite == TipoLimiteEnum.PERDAMAXIMAAVISTA))
                    {
                        OperatingLimitInfo aux = new OperatingLimitInfo();
                        aux.TipoLimite = TipoLimiteEnum.PERDAMAXIMAAVISTA;
                        lock (lstLmt)
                            lstLmt.Add(aux);
                    }
                }
                SymbolInfo symb = null;
                if (_dicSymbols.TryGetValue(ord.Symbol, out symb))
                {
                    if (symb.SegmentoMercado == SegmentoMercadoEnum.AVISTA)
                        ret = lstLmt.FirstOrDefault(x => x.TipoLimite == TipoLimiteEnum.PERDAMAXIMAAVISTA);
                    else
                        ret = lstLmt.FirstOrDefault(x => x.TipoLimite == TipoLimiteEnum.PERDAMAXIMAOPCOES);

                }


                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na adicao do limite: " + ex.Message, ex);
                return null;
            }

        }


        public void CalculateRestSymbol(TOSpOrder toOrd)
        {
            try
            {
                // Verificar se o simbolo esta presente na colecao
                SpiderOrderInfo ord = toOrd.Order;
                RestrictionSymbolInfo rSymbol = null;
                bool updtEvt = false;
                if (!_dicRestrSymbol.TryGetValue(ord.Symbol, out rSymbol))
                    return;

                SymbolInfo symb = null;
                if (!_dicSymbols.TryGetValue(ord.Symbol, out symb))
                {
                    logger.Error("Symbol not found");
                    return;
                }
                decimal lastPx = symb.VlrUltima;
                rSymbol.DtAtualizacao = DateTime.Now;
                switch (ord.OrdStatus)
                {
                    case 0:
                        rSymbol.VolumeNetAlocado += (ord.OrderQty - ord.CumQty) * lastPx;
                        rSymbol.QuantidadeNetAlocada += (ord.OrderQty - ord.CumQty);
                        updtEvt = true;
                        break;
                    case 5:
                        int origOrdQty = _getOrigOrdQty(ord.Details);
                        rSymbol.VolumeNetAlocado += (ord.OrderQty - origOrdQty) * lastPx;
                        rSymbol.QuantidadeNetAlocada += (ord.OrderQty - origOrdQty);
                        updtEvt = true;
                        break;
                    case 4:
                        rSymbol.VolumeNetAlocado -= (ord.OrderQty - ord.CumQty) * lastPx;
                        rSymbol.QuantidadeNetAlocada -= (ord.OrderQty - ord.CumQty);
                        updtEvt = true;
                        break;
                    case 1:
                    case 2:
                        int lastTradeQty = this._getLastTradeQty(ord.Details);
                        rSymbol.VolumeNetAlocado -= lastTradeQty * lastPx;
                        rSymbol.QuantidadeNetAlocada = -lastTradeQty;
                        updtEvt = true;
                        break;
                }

                // Enfileirar para gravacao em banco de dados / arquivo;
                if (updtEvt)
                {
                    TORestriction to = new TORestriction();
                    to.TypeRest = TypeRestriction.SYMBOL;
                    to.RestrictionObject = rSymbol;
                    this._addRestrictionObject(to);

                    // Ativar evento para atualziar memoria client
                    if (OnRestrictionSymbolUpdate != null)
                    {
                        RestrictionSymbolEventArgs sync = new RestrictionSymbolEventArgs();
                        sync.Action = EventAction.UPDATE;
                        sync.RestrictionSymbol = rSymbol;
                        sync.Symbol = rSymbol.Symbol;
                        OnRestrictionSymbolUpdate(this, sync);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no calculo de Restriction Symbol: " + ex.Message, ex);
            }
        }

        public void CalculateRestGroup(TOSpOrder toOrd)
        {
            try
            {
                SpiderOrderInfo ord = toOrd.Order;
                SymbolInfo itemSymbol = null;
                if (!_dicSymbols.TryGetValue(ord.Symbol, out itemSymbol))
                {
                    logger.Error("Ativo / Simbolo NAO encontrado no cadastro de papeis: " + ord.Symbol);
                    return;
                }
                bool updtEvt = false;
                RestrictionGroupSymbolInfo rest = null;
                if (_dicRestrGroupSymbol.TryGetValue(itemSymbol.GrupoCotacao, out rest))
                {
                    decimal lastPx = itemSymbol.VlrUltima;
                    rest.DtAtualizacao = DateTime.Now;
                    switch (ord.OrdStatus)
                    {
                        case 0:
                            rest.VolumeNetAlocado += (ord.OrderQty - ord.CumQty) * lastPx;
                            rest.QuantidadeNetAlocada += (ord.OrderQty - ord.CumQty);
                            updtEvt = true;
                            break;
                        case 5:
                            int origOrdQty = _getOrigOrdQty(ord.Details);
                            rest.VolumeNetAlocado += (ord.OrderQty - origOrdQty) * lastPx;
                            rest.QuantidadeNetAlocada += (ord.OrderQty - origOrdQty);
                            updtEvt = true;
                            break;
                        case 4:
                            rest.VolumeNetAlocado -= (ord.OrderQty - ord.CumQty) * lastPx;
                            rest.QuantidadeNetAlocada -= (ord.OrderQty - ord.CumQty);
                            updtEvt = true;
                            break;
                        case 1:
                        case 2:
                            // TradeQty
                            int lastTradeQty = this._getLastTradeQty(ord.Details);
                            rest.VolumeNetAlocado -= lastTradeQty * lastPx;
                            rest.QuantidadeNetAlocada = -lastTradeQty;
                            updtEvt = true;
                            break;
                    }

                    // Enfileirar para gravacao em banco de dados / arquivo;
                    if (updtEvt)
                    {
                        TORestriction to = new TORestriction();
                        to.TypeRest = TypeRestriction.GROUP_SYMBOL;
                        to.RestrictionObject = rest;
                        this._addRestrictionObject(to);

                        // Ativar o evento para atualizacao e descida para o client;
                        if (OnRestrictionGroupSymbolUpdate != null)
                        {
                            RestrictionGroupSymbolEventArgs sync = new RestrictionGroupSymbolEventArgs();
                            sync.Action = EventAction.UPDATE;
                            sync.IdGrupo = rest.IdGrupo;
                            sync.RestrictionGroupSymbol = rest;
                            OnRestrictionGroupSymbolUpdate(this, sync);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                logger.Error("Problemas no calculo de Restriction Group Symbol: " + ex.Message, ex);
            }
        }

        public void CalculateRestGlobal(TOSpOrder toOrd)
        {
            try
            {
                SpiderOrderInfo ord = toOrd.Order;
                RestrictionGlobalInfo rest = null;
                if (!_dicRestrGlobal.TryGetValue(ord.Account, out rest))
                {
                    rest = new RestrictionGlobalInfo();
                    rest.Account = ord.Account;
                    _dicRestrGlobal.AddOrUpdate(ord.Account, rest, (key, oldValue) => rest);
                    // Ativar o evento para sincronizar memoria client
                    if (OnRestrictionGlobalUpdate != null)
                    {
                        RestrictionGlobalEventArgs sync = new RestrictionGlobalEventArgs();
                        sync.Action = EventAction.INSERT;
                        sync.Account = ord.Account;
                        sync.RestrictionGlobal = rest;
                        OnRestrictionGlobalUpdate(this, sync);
                    }
                }
                else //{
                //if (_dicRestrGlobal.TryGetValue(ord.Account, out rest))
                {
                    int signal = 1;
                    if (ord.Side == 2)
                        signal = -1;

                    
                    
                    SymbolInfo symb = null;
                    if (!_dicSymbols.TryGetValue(ord.Symbol, out symb))
                    {
                        logger.Error("Symbol not found!! ==>" + ord.Symbol);
                        return;
                    }

                    
                    decimal price = symb.VlrUltima;
                    bool updtEvt = false;
                    // TODO [FF] Efetuar todos os calculos dessa bagaca
                    switch (ord.OrdStatus)
                    {
                        case 1:
                        case 2:
                            {
                                // TradeQty
                                int lastTradeQty = this._getLastTradeQty(ord.Details);
                                rest.VolumeNetAlocado += (signal * lastTradeQty * price);
                                rest.QuantidadeNetAlocada += (signal * lastTradeQty);
                                rest.DtAtualizacao = DateTime.Now;
                                updtEvt = true;
                            }
                            break;
                    }

                    if (updtEvt)
                    {
                        // Enfileirar para gravacao em banco de dados / arquivo;
                        TORestriction to = new TORestriction();
                        to.TypeRest = TypeRestriction.GLOBAL;
                        to.RestrictionObject = rest;
                        this._addRestrictionObject(to);

                        // Ativar o evento para sincronizar memoria client
                        if (OnRestrictionGlobalUpdate != null)
                        {
                            RestrictionGlobalEventArgs sync = new RestrictionGlobalEventArgs();
                            sync.Action = EventAction.UPDATE;
                            sync.Account = rest.Account;
                            sync.RestrictionGlobal = rest;
                            OnRestrictionGlobalUpdate(this, sync);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no calculo de Restriction Global: " + ex.Message, ex);
            }
        }

        // TODO [FF] - fazer a logica para calculo desta merda
        public void CalculatePositionClient(TOSpOrder toOrd)
        {
            try
            {
                SpiderOrderInfo ord = toOrd.Order;
                int codCart = 0;
                // Ignorar em caso de bmf
                if (ord.Exchange.Equals(Exchange.Bovespa, StringComparison.InvariantCultureIgnoreCase))
                    codCart = _codCarteiraGeral;
                   

                List<PosClientSymbolInfo> lstPosCli = null;
                int side = ord.Side;

                if (!_dicPositionClient.TryGetValue(ord.Account, out lstPosCli))
                {
                    lstPosCli = new List<PosClientSymbolInfo>();
                    PosClientSymbolInfo pos = new PosClientSymbolInfo();
                    pos.Account = ord.Account;
                    pos.Ativo = ord.Symbol;
                    pos.DtMovimento = DateTime.Now.Date;
                    pos.DtPosicao = DateTime.Now;
                    pos.Bolsa = ord.Exchange;
                    pos.CodCarteira = codCart;
                    if (ord.Account.Equals(0))
                        pos.ExecBroker = ord.ExecBroker;
                    else
                        pos.ExecBroker = pos.Account.ToString();
                    SymbolInfo aux = null;
                    if (_dicSymbols.TryGetValue(ord.Symbol, out aux))
                    {
                        pos.TipoMercado = ParserTipoMercado.Segmento2TipoMercado(aux.SegmentoMercadoValor, aux.IndicadorOpcao);
                        pos.DtVencimento = aux.DtVencimento;
                        pos.CodPapelObjeto = aux.CodigoPapelObjeto;
                    }
                    // Adicionar o obj a lista                    
                    lstPosCli.Add(pos);
                    _dicPositionClient.AddOrUpdate(ord.Account, lstPosCli, (key, oldValue) => lstPosCli);

                }

                SymbolInfo symbInfo = new SymbolInfo();
                if (!_dicSymbols.TryGetValue(ord.Symbol, out symbInfo))
                {
                    logger.Info("Symbol not found ===> " + ord.Symbol);
                    symbInfo = new SymbolInfo();
                }
                bool updtEvt = false;
                PosClientSymbolInfo posCli = null;
                if (ord.Account.Equals(0))
                    posCli = lstPosCli.FirstOrDefault(x => x.Ativo == ord.Symbol && x.CodCarteira == codCart && x.ExecBroker.Equals(ord.ExecBroker, StringComparison.CurrentCultureIgnoreCase));
                else
                    posCli = lstPosCli.FirstOrDefault(x => x.Ativo == ord.Symbol && x.CodCarteira == codCart);
                if (null == posCli)
                {
                    posCli = new PosClientSymbolInfo();
                    posCli.Account = ord.Account;
                    posCli.Ativo = ord.Symbol;
                    posCli.Variacao = symbInfo.VlrOscilacao;
                    posCli.UltPreco = symbInfo.VlrUltima;
                    posCli.PrecoFechamento = symbInfo.VlrFechamento;
                    posCli.DtPosicao = DateTime.Now.Date;
                    posCli.DtMovimento = DateTime.Now;
                    posCli.Bolsa = ord.Exchange;
                    posCli.CodCarteira = codCart;
                    if (ord.Account.Equals(0))
                        posCli.ExecBroker = ord.ExecBroker;
                    else
                        posCli.ExecBroker = ord.Account.ToString();

                    SymbolInfo aux = null;
                    if (_dicSymbols.TryGetValue(ord.Symbol, out aux))
                    {
                        posCli.TipoMercado = ParserTipoMercado.Segmento2TipoMercado(aux.SegmentoMercadoValor, aux.IndicadorOpcao);
                        posCli.DtVencimento = aux.DtVencimento;
                        posCli.CodPapelObjeto = aux.CodigoPapelObjeto;
                    }
                    lstPosCli.Add(posCli);
                }

                if (null != posCli)
                {
                    if (toOrd.MsgType == MsgTOType.SNAPSHOT)
                        updtEvt = this._calculatePosCliSnapshot(toOrd, posCli);
                    else
                        updtEvt = this._calculatePosCliIncremental(toOrd, posCli);

                    // Buscar o ativo na lista e atualizar com informacoes de cotacao e valores que dependam dele
                    // Validacao da bolsa para efetuar calculo financeiro de forma diferente (Bov / BMF)
                    posCli.Variacao = symbInfo.VlrOscilacao;
                    posCli.UltPreco = symbInfo.VlrUltima;
                    if (ord.Exchange.Equals(Exchange.Bovespa, StringComparison.CurrentCultureIgnoreCase))
                    {
                        // Calculo Bov: preco medio e qtd.
                        posCli.FinancNet = posCli.NetExec * posCli.UltPreco;
                        decimal alpha = (posCli.PcMedC * posCli.QtdExecC) - (posCli.PcMedV * posCli.QtdExecV);
                        decimal beta = posCli.FinancNet;
                        posCli.LucroPrej = alpha - beta;
                    }
                    else
                    {
                        // Calculo BMF: de acordo com o tipo de mercado
                        if (!BmfCalculator.Instance.CalcularPosicaoBmf(posCli))
                        {
                            logger.Error("Problemas no calculo de Finaceiro Net e L/P de BMF");
                        }
                    }

                    // Atualizacao do timestamp
                    posCli.DtMovimento = DateTime.Now;
                    posCli.MsgId++;
                    posCli.EventSource = "POSCLI";

                    // Processar PositionClient By Symbol (montagem da estrutura para facilidade no calculo de L/P e Financeiro NET)
                    this._processPosClientBySymbol(posCli);


                    // Enfileirar para gravar as informacoes na tabela sqlserver
                    TOPosClient to = new TOPosClient();
                    to.PositionClient = posCli;
                    this._addPosClient(to);

                    //// Ativar o evento para sincronizar memoria client
                    //if (updtEvt)
                    //{
                    //    if (OnPositionClientUpdate != null)
                    //    {
                    //        PositionClientEventArgs sync = new PositionClientEventArgs();
                    //        sync.Action = EventAction.UPDATE;
                    //        sync.Account = ord.Account;
                    //        List<PosClientSymbolInfo> lstAux = new List<PosClientSymbolInfo>();
                    //        lstAux.Add(posCli);
                    //        sync.PosClient = lstAux;
                    //        OnPositionClientUpdate(this, sync);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no calculo de PositionClient: " + ex.Message, ex);
            }
        }
        private void _processPosClientBySymbol(PosClientSymbolInfo pos)
        {
            try
            {
                List<PosClientSymbolInfo> lst = null;
                if (_dicPosClientBySymbol.TryGetValue(pos.Ativo, out lst))
                {
                    PosClientSymbolInfo item = lst.FirstOrDefault(x => x.Ativo.Equals(pos.Ativo) && x.Account.Equals(pos.Account) && x.CodCarteira.Equals(pos.CodCarteira) && x.ExecBroker.Equals(pos.ExecBroker));
                    if (item == null)
                        lst.Add(pos);
                    else
                        item = pos;
                }
                else
                {
                    lst = new List<PosClientSymbolInfo>();
                    lst.Add(pos);
                    _dicPosClientBySymbol.AddOrUpdate(pos.Ativo, lst, (key, oldValue) => lst);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento de position client por simbolo: " + ex.Message, ex);
            }
        }
        private bool _calculatePosCliIncremental(TOSpOrder to, PosClientSymbolInfo pos)
        {
            bool update = false;
            try
            {
                SpiderOrderInfo ord = to.Order;
                

                if (null != pos)
                {
                    int cumQty = ord.CumQty;
                    switch (ord.OrdStatus)
                    {
                        case 0:
                            {
                                if (ord.Side == 1)
                                    pos.QtdAbC += ord.OrderQty;
                                else
                                    pos.QtdAbV += ord.OrderQty;
                                pos.NetAb = pos.QtdAbC - pos.QtdAbV;
                                update = true;
                            }
                            break;
                        case 5:
                            {
                                int origOrdQty = _getOrigOrdQty(ord.Details); ;
                                if (ord.Side == 1)
                                    pos.QtdAbC += ord.OrderQty - origOrdQty - cumQty;
                                else
                                    pos.QtdAbV += ord.OrderQty - origOrdQty - cumQty;

                                update = true;
                            }
                            break;
                        case 4:
                            {
                                if (ord.Side == 1)
                                    pos.QtdAbC -= (ord.OrderQty - cumQty);
                                else
                                    pos.QtdAbV -= (ord.OrderQty - cumQty);
                                update = true;
                            }
                            break;
                        case 1:
                        case 2:
                            {
                                // TradeQty
                                SpiderOrderDetailInfo det = this._getLastTradeQtyDet(ord.Details);
                                int lastTradeQty = 0;
                                int qtdAb = 0;
                                if (det != null)
                                    lastTradeQty = det.TradeQty;
                                qtdAb = lastTradeQty;
                                if (ord.Side == 1)
                                {
                                    pos.QtdExecC += lastTradeQty;
                                    pos.QtdAbC -= qtdAb;
                                }
                                else
                                {
                                    pos.QtdExecV += lastTradeQty;
                                    pos.QtdAbV -= qtdAb;
                                }
                                update = true;

                                //pos.NetExec = pos.QtdExecC - pos.QtdExecV;
                                //pos.NetAb = pos.QtdAbC - pos.QtdAbV;


                                // Gravar informacoes da execucao e calcular o preco medio
                                List<ExecSymbolInfo> lstExecs = null;
                                if (!_dicExecSymbols.TryGetValue(ord.Account, out lstExecs))
                                {
                                    lstExecs = new List<ExecSymbolInfo>();
                                    _dicExecSymbols.AddOrUpdate(ord.Account, lstExecs, (key, oldvalue) => lstExecs);
                                }
                                ExecSymbolInfo exec = new ExecSymbolInfo();
                                exec.Symbol = ord.Symbol;
                                exec.Qty = lastTradeQty;
                                exec.Side = ord.Side;
                                if (det != null)
                                {
                                    exec.Price = det.Price;
                                    lstExecs.Add(exec);
                                }
                                else
                                {
                                    logger.Error("SpiderOrderDetalhe não encontrado: " + exec.Symbol);
                                }

                                List<ExecSymbolInfo> lstAvg = lstExecs.Select(x => x).Where(x => x.Side == ord.Side && x.Symbol == ord.Symbol).ToList();
                                if (ord.Side == 1)
                                    pos.PcMedC = Tools.CalculateWeightedAvgPx(lstAvg);
                                else
                                    pos.PcMedV = Tools.CalculateWeightedAvgPx(lstAvg);
                                update = true;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no calculo de PositionClient Incremental: " + ex.Message, ex);
            }
            return update;
        }

        private bool _calculatePosCliSnapshot(TOSpOrder to, PosClientSymbolInfo pos)
        {
            bool update = false;
            try
            {
                SpiderOrderInfo ord = to.Order;


                if (null != pos)
                {
                    int cumQty = ord.CumQty;
                    switch (ord.OrdStatus)
                    {
                        case 0:
                            {
                                if (ord.Side == 1)
                                    pos.QtdAbC += ord.OrderQty;
                                else
                                    pos.QtdAbV += ord.OrderQty;
                                pos.NetAb = pos.QtdAbC - pos.QtdAbV;
                                update = true;
                            }
                            break;
                        case 5:
                            {
                                int origOrdQty = _getOrigOrdQty(ord.Details);
                                if (ord.Side == 1)
                                {
                                    pos.QtdAbC += ord.OrderQty - cumQty;
                                    pos.QtdExecC += cumQty;
                                }
                                else
                                {
                                    pos.QtdAbV += ord.OrderQty - cumQty;
                                    pos.QtdExecV += cumQty;
                                }
                                update = true;
                            }
                            break;
                        case 4:
                            {
                                if (ord.Side == 1)
                                    pos.QtdExecC += cumQty;
                                else
                                    pos.QtdExecV += cumQty;
                                update = true;
                            }
                            break;
                        case 1:
                        case 2:
                            {
                                // TradeQty
                                if (ord.Side == 1)
                                {
                                    pos.QtdExecC += cumQty;
                                }
                                else
                                {
                                    pos.QtdExecV += cumQty;
                                }
                                update = true;
                            }
                            break;
                    }

                    if (ord.CumQty > 0)
                    {
                        pos.NetExec = pos.QtdExecC - pos.QtdExecV;
                        pos.NetAb = pos.QtdAbC - pos.QtdAbV;

                        // Adicionar os detalhes para calculo de Preco Medio
                        List<ExecSymbolInfo> lstExecs  = null;
                        if (!_dicExecSymbols.TryGetValue(ord.Account, out lstExecs))
                        {
                            lstExecs = new List<ExecSymbolInfo>();
                            _dicExecSymbols.AddOrUpdate(ord.Account, lstExecs, (key, oldvalue) => lstExecs);
                        }

                        foreach (SpiderOrderDetailInfo od in ord.Details)
                        {
                            if (od.OrderStatusID == 1 || od.OrderStatusID == 2)
                            {
                                ExecSymbolInfo eS = new ExecSymbolInfo();
                                eS.Symbol = ord.Symbol;
                                eS.Qty = od.TradeQty;
                                eS.Side = ord.Side;
                                eS.Price = od.Price;
                                lstExecs.Add(eS);
                            }
                        }
                        List<ExecSymbolInfo> lstAvg = lstExecs.Select(x => x).Where(x => x.Side == ord.Side && x.Symbol == ord.Symbol).ToList();
                        if (ord.Side == 1)
                            pos.PcMedC = Tools.CalculateWeightedAvgPx(lstAvg);
                        else
                            pos.PcMedV = Tools.CalculateWeightedAvgPx(lstAvg);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no calculo de PositionClient Incremental: " + ex.Message, ex);
            }
            return update;
        }

        public void CalculateOperationalLimit(TOSpOrder to)
        {
            try
            {
                SpiderOrderInfo ord = to.Order;
                // Bovespa
                if (ord.Exchange.Equals(Exchange.Bovespa, StringComparison.InvariantCultureIgnoreCase))
                {
                    this._calculateBovespaLimit(ord);
                }
                // BMF
                {
                    this._calculateBMFLimit(ord);
                }

            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento do objeto SpiderOrderInfo: " + ex.Message, ex);
            }
        }

        private LimitResponse _calculateBovespaLimit(SpiderOrderInfo o)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                // Valida se existe o limite na memoria para o account em questao
                List<OperatingLimitInfo> lst = null;
                if (!_dicOperatingLimit.TryGetValue(o.Account, out lst))
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_OPERATING_LIMIT_NOT_FOUND, ErrorMessages.ERR_OPERATING_LIMIT_NOT_FOUND);
                    return ret;
                }
                // Saber qual o tipo de limite para ser descontado
                SymbolInfo ativo;
                TipoLimiteEnum tpLimite = _findLimitType(o.Symbol, o.Side.ToString(), out ativo);
                if (tpLimite == TipoLimiteEnum.INDEFINIDO)
                {
                    logger.Error("Erro na busca do tipo de limite... Considerando A VISTA Compra / Venda!!");
                    tpLimite = o.Side.Equals("1")? TipoLimiteEnum.COMPRAAVISTA: TipoLimiteEnum.VENDAAVISTA;
                }
                // Calculo de valor alocado de acordo com o status
                decimal vlrUltimoPreco = decimal.Zero;
                if (ativo!=null)
                    vlrUltimoPreco = ativo.VlrUltima;
                decimal vlrAlocado = Decimal.Zero;
                int ordStatus = (int)o.OrdStatus;
                switch (o.OrdStatus)
                {
                    case (int) SpiderOrderStatus.NOVA:
                    case (int) SpiderOrderStatus.CANCELADA:
                        vlrAlocado = o.OrderQty * vlrUltimoPreco;
                        ret = this.UpdateOperationalLimitBovespa(o.Account, tpLimite, vlrAlocado, vlrUltimoPreco, o.Side, ordStatus);
                        break;
                    case (int) SpiderOrderStatus.SUBSTITUIDA:
                        {
                            // Buscar o penultimo status do detail (new ou replaced): Vai retornar ao menos 2 registros. Sempre pegar o penultimo
                            //List<SpiderOrderDetailInfo> lstDet = o.Details.Select(x=>x).Where(x=>x.OrderStatusID == (int) SpiderOrderStatus.NOVA || 
                            //    x.OrderStatusID == (int) SpiderOrderStatus.SUBSTITUIDA).OrderByDescending(x=>x.OrderDetailID).ToList();
                            //// TODO [FF]: Validar a ordem dos details para ler as informacoes
                            //SpiderOrderDetailInfo od = lstDet[1];
                            int origOrdQty = _getOrigOrdQty(o.Details);
                            vlrAlocado = (o.OrderQty * vlrUltimoPreco) - (origOrdQty * vlrUltimoPreco);
                            // TODO [FF]: Fazer a chamada do update de limite
                            ret = this.UpdateOperationalLimitBovespa(o.Account, tpLimite, vlrAlocado, vlrUltimoPreco, o.Side, ordStatus);
                        }
                        break;
                    case (int) SpiderOrderStatus.PARCIALMENTEEXECUTADA:
                    case (int) SpiderOrderStatus.EXECUTADA:
                        vlrAlocado = o.CumQty * vlrUltimoPreco;
                        ret = this.UpdateOperationalLimitBovespa(o.Account, tpLimite, vlrAlocado, vlrUltimoPreco, o.Side, ordStatus);
                        break;
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no calculo do limite bovespa: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message);
                return ret;
            }
        }
        private LimitResponse _calculateBMFLimit(SpiderOrderInfo o)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                int ordStatus = (int) o.OrdStatus;
                switch (o.OrdStatus)
                {
                    case (int)SpiderOrderStatus.NOVA:
                        {
                            bool calcLimite = true;
                            // Verificar se eh ordem stop e se eh na alocacao ou no disparo (verificar por detail)
                            if (o.OrdTypeID.Equals("52")) // Stop
                            {
                                // Verificar se existe dois status "New" nos Details (caso sim, foi disparo da ordem)
                                List<SpiderOrderDetailInfo> lstDet = o.Details.Select(x => x).Where(x => x.OrderStatusID == (int)SpiderOrderStatus.NOVA).ToList();
                                if (lstDet != null && lstDet.Count >= 2)
                                    calcLimite = false;
                            }
                            if (calcLimite)
                                ret = this.UpdateOperationalLimitBMF(o.Account, o.Symbol, o.OrderQty, 0, o.Side.ToString(), ordStatus);
                        }
                        break;
                    case (int)SpiderOrderStatus.SUBSTITUIDA:
                        {
                            int qtd = this._getOrigOrdQty(o.Details);
                            int qtdResult = o.OrderQty - qtd; // OBS: Pode ocorrer erro, caso nao haja cadastro do new order. Neste caso retorna 0
                            ret = this.UpdateOperationalLimitBMF(o.Account, o.Symbol, qtdResult, o.OrderQty, o.Side.ToString(), ordStatus);
                        }
                        break;
                    case (int)SpiderOrderStatus.CANCELADA:
                        {
                            int qtdResult = o.OrderQty - o.CumQty;
                            ret = this.UpdateOperationalLimitBMF(o.Account, o.Symbol, qtdResult, 0, o.Side.ToString(), ordStatus);
                        }
                        break;

                    case (int)SpiderOrderStatus.PARCIALMENTEEXECUTADA:
                    case (int)SpiderOrderStatus.EXECUTADA:
                        {
                            // Buscar o LastQty a partir dos details
                            List<SpiderOrderDetailInfo> lstDet = o.Details.Select(x => x).Where(x => x.OrderStatusID == (int)SpiderOrderStatus.PARCIALMENTEEXECUTADA ||
                                x.OrderStatusID == (int)SpiderOrderStatus.EXECUTADA).OrderByDescending(x => x.OrderDetailID).ToList();
                            // TODO [FF]: Validar a ordem dos details para ler as informacoes
                            int qtdResult = this._getLastTradeQty(o.Details);
                            ret = this.UpdateOperationalLimitBMF(o.Account, o.Symbol, qtdResult, 0, o.Side.ToString(), ordStatus);
                        }
                        break;
                }
                
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no calculo do limite bovespa: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message);
                return ret;
            }
        }

        private TipoLimiteEnum _findLimitType(string symbol, string side, out SymbolInfo symb)
        {
            try
            {
                SymbolInfo aux = null;
                symb = aux;
                TipoLimiteEnum ret;
                if (_dicSymbols.TryGetValue(symbol, out aux))
                {
                    symb = aux;
                    if (side.Equals("1"))
                    {
                        switch (aux.SegmentoMercado)
                        {
                            case SegmentoMercadoEnum.AVISTA:
                            case SegmentoMercadoEnum.FRACIONARIO:
                            case SegmentoMercadoEnum.INTEGRALFRACIONARIO:
                                ret = TipoLimiteEnum.COMPRAAVISTA;
                                break;
                            case SegmentoMercadoEnum.OPCAO:
                                ret = TipoLimiteEnum.COMPRAOPCOES;
                                break;
                            default:
                                ret = TipoLimiteEnum.COMPRAAVISTA;
                                break;
                        }
                    }
                    else
                    {
                        switch (aux.SegmentoMercado)
                        {
                            case SegmentoMercadoEnum.AVISTA:
                            case SegmentoMercadoEnum.FRACIONARIO:
                            case SegmentoMercadoEnum.INTEGRALFRACIONARIO:
                                ret = TipoLimiteEnum.VENDAAVISTA;
                                break;
                            case SegmentoMercadoEnum.OPCAO:
                                ret = TipoLimiteEnum.VENDAOPCOES;
                                break;
                            default:
                                ret = TipoLimiteEnum.VENDAAVISTA;
                                break;
                        }
                    }
                }
                else
                {
                    ret = TipoLimiteEnum.INDEFINIDO;
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Nao foi possivel achar o limite: " + ex.Message, ex);
                symb = null;
                return TipoLimiteEnum.INDEFINIDO;
            }
        }

        private LimitResponse UpdateOperationalLimitBMF(int account, string symbol, int orderQty, int newOrderQty, string sentido , int st)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }

                // Verificacao se a conta recebida eh conta master (contabroker).
                // Se tiver mais de uma conta master associada, calcular o limite DAS DUAS CONTAS
                List<ContaBrokerInfo> lstContaBroker = null;
                List<int> lstAccounts = new List<int>();
                if (_dicContaBroker.TryGetValue(account, out lstContaBroker))
                {
                    for (int k = 0; k < lstContaBroker.Count; k++)
                        lstAccounts.Add(lstContaBroker[k].IdContaBroker);
                }
                else
                {
                    lstAccounts.Add(account);
                }

                for (int i = 0; i < lstAccounts.Count; i++)
                {
                    // Buscar informacoes da regra de limite operacional
                    ClientLimitBMFInfo cliLimitBmfInfo = null;
                    if (!_dicClientLimitBMF.TryGetValue(account, out cliLimitBmfInfo))
                    {
                        ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_NOT_FOUND, ErrorMessages.ERR_BMF_LIMIT_NOT_FOUND);
                        return ret;
                    }
                    string side = string.Empty;
                    if (sentido.Equals("1"))
                        side = "C";
                    else
                        side = "V";

                    ClientLimitBMFInfo item1 = new ClientLimitBMFInfo();
                    ClientLimitBMFInfo item2 = new ClientLimitBMFInfo();
                    string contract = symbol.Substring(0, 3);
                    ClientLimitContractBMFInfo cliContractBMF = cliLimitBmfInfo.ContractLimit.Find(x => x.Contrato.Equals(contract) && x.Sentido.Equals(side, StringComparison.InvariantCultureIgnoreCase));

                    // TODO[FF]: Efetuar o novo recalculo a partir deste ponto
                    // Calculo de contrato
                    if (null != cliContractBMF)
                    {
                        switch (st)
                        {
                            case (int)SpiderOrderStatus.CANCELADA:
                                cliContractBMF.QuantidadeDisponivel = cliContractBMF.QuantidadeDisponivel + orderQty;
                                break;
                            case (int)SpiderOrderStatus.NOVA:
                            case (int)SpiderOrderStatus.SUBSTITUIDA:
                                cliContractBMF.QuantidadeDisponivel = cliContractBMF.QuantidadeDisponivel - orderQty;
                                break;
                        }
                        cliContractBMF.DataMovimento = DateTime.Now;
                        item1.ContractLimit.Add(cliContractBMF);
                        // if filled or partial filled, then calculate the opposite side
                        if (st == (int)SpiderOrderStatus.EXECUTADA || st == (int)SpiderOrderStatus.PARCIALMENTEEXECUTADA)
                        {
                            ClientLimitContractBMFInfo cliContractBMFDual = null;
                            string sentidoAux = string.Empty;
                            if (side.Equals("C"))
                            {
                                sentidoAux = "V";
                                cliContractBMFDual = cliLimitBmfInfo.ContractLimit.Find(x => x.Contrato.Equals(contract) && x.Sentido.Equals(sentidoAux, StringComparison.InvariantCultureIgnoreCase));
                            }
                            else
                            {
                                sentidoAux = "C";
                                cliContractBMFDual = cliLimitBmfInfo.ContractLimit.Find(x => x.Contrato.Equals(contract) && x.Sentido.Equals(sentidoAux, StringComparison.InvariantCultureIgnoreCase));
                            }
                            cliContractBMFDual.QuantidadeDisponivel = cliContractBMFDual.QuantidadeDisponivel + orderQty;
                            item2.ContractLimit.Add(cliContractBMFDual);
                            // logger.InfoFormat("Contract [{0}] Side [{1}] QtdDisponivel: [{2}]", contract, sentidoAux, cliContractBMFDual.QuantidadeDisponivel);
                        }
                    }


                    // Calculo de instrumento, caso exista o vencimento configurado
                    ClientLimitInstrumentBMFInfo cliInstrumentBMF = null;
                    if (null != cliLimitBmfInfo.InstrumentLimit)
                    {
                        cliInstrumentBMF = cliLimitBmfInfo.InstrumentLimit.Find(x => x.Instrumento.Equals(symbol) && x.Sentido.Equals(side, StringComparison.InvariantCultureIgnoreCase));

                        if (null != cliInstrumentBMF)
                        {
                            switch (st)
                            {
                                case (int)SpiderOrderStatus.CANCELADA:
                                    cliInstrumentBMF.QtDisponivel = cliInstrumentBMF.QtDisponivel + orderQty;
                                    break;
                                case (int)SpiderOrderStatus.NOVA:
                                case (int)SpiderOrderStatus.SUBSTITUIDA:
                                    cliInstrumentBMF.QtDisponivel = cliInstrumentBMF.QtDisponivel - orderQty;
                                    break;
                            }

                            // logger.InfoFormat("Symbol [{0}] Side:[{1}] OrderQty:[{2}] QtDisponivel: [{3}]", symbol, sentido, orderQty, cliInstrumentBMF.QtDisponivel); 
                            cliInstrumentBMF.dtMovimento = DateTime.Now;
                            item1.InstrumentLimit.Add(cliInstrumentBMF);
                            // if filled or partial filled, then calculate the opposite side
                            if (st == (int)SpiderOrderStatus.EXECUTADA || st == (int)SpiderOrderStatus.PARCIALMENTEEXECUTADA)
                            {
                                ClientLimitInstrumentBMFInfo cliInstrumentBMFDual = null;
                                string sentidoAux = string.Empty;
                                if (side.Equals("C") && null != cliLimitBmfInfo.InstrumentLimit)
                                {
                                    sentidoAux = "V";
                                    cliInstrumentBMFDual = cliLimitBmfInfo.InstrumentLimit.Find(x => x.Instrumento.Equals(symbol) && x.Sentido.Equals(sentidoAux, StringComparison.InvariantCultureIgnoreCase));
                                }

                                if (side.Equals("V") && null != cliLimitBmfInfo.InstrumentLimit)
                                {
                                    sentidoAux = "C";
                                    cliInstrumentBMFDual = cliLimitBmfInfo.InstrumentLimit.Find(x => x.Instrumento.Equals(symbol) && x.Sentido.Equals(sentidoAux, StringComparison.InvariantCultureIgnoreCase));
                                }
                                cliInstrumentBMFDual.QtDisponivel = cliInstrumentBMFDual.QtDisponivel + orderQty;
                                cliInstrumentBMFDual.dtMovimento = DateTime.Now;
                                item2.InstrumentLimit.Add(cliInstrumentBMFDual);
                            }
                        }
                        else
                        {
                            logger.Info("Instrumento BMF nao encontrado. Symbol: " + symbol + " Considerando somente calculo do contrato pai");
                        }
                    }

                    item1.Account = account;
                    item2.Account = account;

                    this._addBmfMvto(item1);
                    this._addBmfMvto(item2);
                    this.AddOrUpdateLimitBMF(account, cliLimitBmfInfo);
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao dos valores dos limites operacionais BMF: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }

        public LimitResponse UpdateOperationalLimitBovespa(int account, TipoLimiteEnum tpLimite, decimal vlrAlocado, decimal precoBase, int side, int st)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }
                
                // Verificacao se a conta recebida eh conta master (contabroker).
                // Se tiver mais de uma conta master associada, calcular o limite DAS DUAS CONTAS
                List<ContaBrokerInfo> lstContaBroker = null;
                List<int> lstAccounts = new List<int>();
                if (_dicContaBroker.TryGetValue(account, out lstContaBroker))
                {
                    for (int k = 0; k < lstContaBroker.Count; k++)
                        lstAccounts.Add(lstContaBroker[k].IdContaBroker);
                }
                else
                {
                    lstAccounts.Add(account);
                }

                for (int i = 0; i < lstAccounts.Count; i++)
                {
                    int acc = lstAccounts[i];
                    // Buscar informacoes da regra de limite operacional
                    List<OperatingLimitInfo> opLimitLst = null;
                    if (!_dicOperatingLimit.TryGetValue(acc, out opLimitLst))
                    {
                        string msg = string.Format(ErrorMessages.ERR_OPERATING_LIMIT_NOT_FOUND, tpLimite.ToString());
                        ret = _fillResponse(ErrorMessages.ERR_CODE_OPERATING_LIMIT_NOT_FOUND, msg);
                        return ret;
                    }

                    decimal vlrAux = Decimal.Zero;
                    OperatingLimitInfo opLimit = opLimitLst.Find(x => x.TipoLimite == tpLimite);
                    if (null == opLimit)
                    {
                        string msg = string.Format(ErrorMessages.ERR_OPERATING_LIMIT_NOT_FOUND, tpLimite.ToString());
                        ret = _fillResponse(ErrorMessages.ERR_CODE_OPERATING_LIMIT_NOT_FOUND, msg);
                        return ret;
                    }

                    //if (!st.Equals(OrdStatus.FILLED) || !st.Equals(OrdStatus.PARTIALLY_FILLED))
                    //{
                    // tratamento dos valores alocados (parte)
                    switch (st)
                    {
                        case (int)SpiderOrderStatus.CANCELADA:
                            vlrAux = (-1) * vlrAlocado;
                            opLimit.ValorAlocado += vlrAux;
                            break;
                        case (int)SpiderOrderStatus.NOVA:
                            vlrAux = vlrAlocado;
                            opLimit.ValorAlocado += vlrAux;
                            break;
                        case (int)SpiderOrderStatus.SUBSTITUIDA:
                            vlrAux = vlrAlocado;
                            // opLimit.ValorAlocado = (vlrAux - opLimit.ValorAlocado) + opLimit.ValorAlocado;
                            opLimit.ValorAlocado = opLimit.ValorAlocado + vlrAux;
                            break;
                        case (int)SpiderOrderStatus.EXECUTADA:
                        case (int)SpiderOrderStatus.PARCIALMENTEEXECUTADA:
                            vlrAux = (-1) * vlrAlocado;
                            opLimit.ValorAlocado += vlrAux; // devolvendo a ordem em aberto
                            break;

                    }
                    opLimit.ValorDisponivel = opLimit.ValotTotal - opLimit.ValorAlocado;
                    opLimit.PrecoBase = precoBase;
                    opLimit.ValorMovimento = vlrAux;
                    opLimit.StNatureza = opLimit.ValorMovimento < 0 ? "C" : "D"; // Rever a logica
                    this._addBvspMvto(opLimit);
                    this.AddOrUpdateOperatingLimit(acc, opLimit);
                    //}
                    // If filled or partially filled, then calculate the dual limit
                    if (st == (int)SpiderOrderStatus.EXECUTADA || st == (int)SpiderOrderStatus.PARCIALMENTEEXECUTADA)
                    {
                        // Debita o limite de compra / venda
                        opLimit.ValorAlocado += vlrAlocado;
                        opLimit.ValorDisponivel = opLimit.ValotTotal - opLimit.ValorAlocado;
                        opLimit.PrecoBase = precoBase;
                        opLimit.ValorMovimento = vlrAux;
                        opLimit.StNatureza = opLimit.ValorMovimento < 0 ? "C" : "D"; // Rever a logica
                        this._addBvspMvto(opLimit);
                        this.AddOrUpdateOperatingLimit(acc, opLimit);
                        // Credita o limite de compra / venda
                        OperatingLimitInfo opLimitDual = opLimitLst.Find(x => x.TipoLimite == _dicDualTipoLimite[tpLimite]);
                        if (null == opLimitDual)
                        {
                            string msg = string.Format(ErrorMessages.ERR_OPERATING_LIMIT_NOT_FOUND, _dicDualTipoLimite[tpLimite].ToString());
                            ret = _fillResponse(ErrorMessages.ERR_CODE_OPERATING_LIMIT_NOT_FOUND, msg);
                            return ret;
                        }

                        //switch (st)
                        //{
                        //    case OrdStatus.FILLED:
                        //    case OrdStatus.PARTIALLY_FILLED:
                        vlrAux = (-1) * vlrAlocado;
                        opLimitDual.ValorAlocado += vlrAux; // devolvendo o limite original
                        //        break;
                        //}

                        opLimitDual.ValorDisponivel = opLimitDual.ValotTotal - opLimitDual.ValorAlocado;
                        opLimitDual.PrecoBase = precoBase;
                        opLimitDual.ValorMovimento = (-1) * vlrAux;
                        opLimitDual.StNatureza = opLimitDual.ValorMovimento < 0 ? "C" : "D"; // Rever a logica
                        this._addBvspMvto(opLimitDual);
                        this.AddOrUpdateOperatingLimit(acc, opLimitDual);
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao dos valores dos limites operacionais BOVESPA: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }


        private LimitResponse _fillResponse(int code, string msg, string stack = "")
        {
            LimitResponse ret = new LimitResponse();
            ret.ErrorMessage = msg;
            ret.ErrorCode = code;
            ret.ErrorStack = stack;
            return ret;
        }

        #endregion


        #region Financial Limits
        private void _addBvspMvto(OperatingLimitInfo item)
        {
            lock (_qBovespa)
            {
                _qBovespa.Enqueue(item);
                //Monitor.Pulse(_queueBovespa);
            }
        }

        private void _addBmfMvto(ClientLimitBMFInfo item)
        {
            lock (_qBMF)
            {
                _qBMF.Enqueue(item);
                //Monitor.Pulse(_queueBMF);
            }
        }

        private void _processMvto()
        {
            try
            {
                while (_isLoaded)
                {
                    if ((_qBovespa.Count > 0) || (_qBMF.Count > 0))
                    {
                        if (_qBovespa.Count > 0)
                        {
                            List<OperatingLimitInfo> lstMvtoBov = new List<OperatingLimitInfo>();
                            lock (_qBovespa)
                            {
                                lstMvtoBov.AddRange(_qBovespa.ToArray());
                                _qBovespa.Clear();
                            }

                            int len = lstMvtoBov.Count;
                            for (int i = 0; i < len; i++)
                            {
                                string chave = Exchange.Bovespa + "." + lstMvtoBov[i].CodigoCliente;

                                if (!_dicPersistArq.ContainsKey(chave))
                                {
                                    PersistLimit pstArq = new PersistLimit(Exchange.Bovespa, lstMvtoBov[i].CodigoCliente);
                                    _dicPersistArq.Add(chave, pstArq);
                                }
                                _dicPersistArq[chave].InserirMvtoBovespa(lstMvtoBov[i]);
                                _db.InserirMvtoBvsp(lstMvtoBov[i]);
                            }
                            // Atualizar o estado mais atual de cada tipo de Limite (Compra Vista, Venda Vista, Comra Opcao, Venda Opcao)
                            OperatingLimitInfo item = lstMvtoBov.LastOrDefault(x => x.TipoLimite == TipoLimiteEnum.COMPRAAVISTA);
                            if (null != item)
                            {
                                _db.AtualizarMvtoBvsp(item);
                            }
                            item = lstMvtoBov.LastOrDefault(x => x.TipoLimite == TipoLimiteEnum.VENDAAVISTA);
                            if (null != item)
                            {
                                _db.AtualizarMvtoBvsp(item);
                            }
                            item = lstMvtoBov.LastOrDefault(x => x.TipoLimite == TipoLimiteEnum.COMPRAOPCOES);
                            if (null != item)
                            {
                                _db.AtualizarMvtoBvsp(item);
                            }
                            item = lstMvtoBov.LastOrDefault(x => x.TipoLimite == TipoLimiteEnum.VENDAOPCOES);
                            if (null != item)
                            {
                                _db.AtualizarMvtoBvsp(item);
                            }
                            lstMvtoBov.Clear();
                            lstMvtoBov = null;
                        }
                        if (_qBMF.Count > 0)
                        {
                            List<ClientLimitBMFInfo> lstMvtoBmf = new List<ClientLimitBMFInfo>();
                            Dictionary<string, ClientLimitContractBMFInfo> dicContr = new Dictionary<string, ClientLimitContractBMFInfo>();
                            Dictionary<string, ClientLimitInstrumentBMFInfo> dicInst = new Dictionary<string, ClientLimitInstrumentBMFInfo>();
                            lock (_qBMF)
                            {
                                lstMvtoBmf.AddRange(_qBMF.ToArray());
                                _qBMF.Clear();
                            }

                            int len = lstMvtoBmf.Count;
                            for (int i = 0; i < len; i++)
                            {
                                string chave = Exchange.Bmf + "." + lstMvtoBmf[i].Account;

                                if (!_dicPersistArq.ContainsKey(chave))
                                {
                                    PersistLimit pstArq = new PersistLimit(Exchange.Bmf, lstMvtoBmf[i].Account);
                                    _dicPersistArq.Add(chave, pstArq);
                                }
                                _dicPersistArq[chave].InserirMvtoBmf(lstMvtoBmf[i]);

                                if (lstMvtoBmf[i].ContractLimit != null && lstMvtoBmf[i].ContractLimit.Count > 0)
                                {
                                    ClientLimitContractBMFInfo item = lstMvtoBmf[i].ContractLimit[0];
                                    ClientLimitContractBMFInfo itemAux = null;
                                    string chave1 = item.Contrato + "-" + item.Sentido;
                                    if (dicContr.TryGetValue(chave1, out itemAux))
                                    {
                                        itemAux = item;
                                    }
                                    else
                                        dicContr.Add(chave1, item);
                                }

                                if (lstMvtoBmf[i].InstrumentLimit != null && lstMvtoBmf[i].InstrumentLimit.Count > 0)
                                {
                                    ClientLimitInstrumentBMFInfo item = lstMvtoBmf[i].InstrumentLimit[0];
                                    ClientLimitInstrumentBMFInfo itemAux = null;
                                    string chave2 = item.Instrumento + "-" + item.Sentido;
                                    if (dicInst.TryGetValue(chave2, out itemAux))
                                    {
                                        itemAux = item;
                                    }
                                    else
                                        dicInst.Add(chave2, item);
                                }
                                // _lmtDb.InserirMvtoBmf(lstMvtoBmf[i]);
                            }

                            foreach (KeyValuePair<string, ClientLimitContractBMFInfo> x in dicContr)
                            {
                                _db.AtualizarMvtoBMFContrato(x.Value);
                            }

                            foreach (KeyValuePair<string, ClientLimitInstrumentBMFInfo> x in dicInst)
                            {
                                _db.AtualizarMvtoBMFInstrumento(x.Value);
                            }
                            
                            dicInst.Clear();
                            dicContr.Clear();
                            lstMvtoBmf.Clear();
                            lstMvtoBmf = null;
                            dicContr = null;
                            dicInst = null;
                        }
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao gravar movimentacao:" + ex.Message, ex);
            }
        }

        private void _addRestrictionObject(TORestriction to)
        {
            _cqRest.Enqueue(to);
            lock (_syncRest)
                Monitor.Pulse(_syncRest);
        }

        private void _processRestrictionObject()
        {
            try
            {
                while (_isLoaded)
                {
                    TORestriction to = null;
                    if (_cqRest.TryDequeue(out to))
                    {
                        switch (to.TypeRest)
                        {
                            case TypeRestriction.SYMBOL:
                                _db.AtualizarRestrictionSymbol(to.RestrictionObject as RestrictionSymbolInfo);
                                break;
                            case TypeRestriction.GROUP_SYMBOL:
                                _db.AtualizarRestrictionGroupSymbol(to.RestrictionObject as RestrictionGroupSymbolInfo);
                                break;
                            case TypeRestriction.GLOBAL:
                                _db.AtualizarRestrictionGlobal(to.RestrictionObject as RestrictionGlobalInfo);
                                break;
                        }
                        this._saveRestrictionToFile(to);
                    }
                    else
                    {
                        lock (_syncRest)
                            Monitor.Wait(_syncRest, 100);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro no processamento do objeto de restricao: " + ex.Message, ex);
            }

        }
        private void _saveRestrictionToFile(TORestriction to)
        {
            try
            {
                switch (to.TypeRest)
                {
                    case TypeRestriction.SYMBOL:
                        _pSymbol.TraceInfo(to.RestrictionObject);
                        _db.InserirRestrictionSymbolMvto(to.RestrictionObject as RestrictionSymbolInfo);
                        break;
                    case TypeRestriction.GLOBAL:
                        _pGlobal.TraceInfo(to.RestrictionObject);
                        _db.InserirRestrictionGlobalMvto(to.RestrictionObject as RestrictionGlobalInfo);
                        break;
                    case TypeRestriction.GROUP_SYMBOL:
                        _pGroup.TraceInfo(to.RestrictionObject);
                        _db.InserirRestrictionGroupSymbolMvto(to.RestrictionObject as RestrictionGroupSymbolInfo);
                        break;

                }
            }
            catch (Exception ex)
            {
                logger.Error("Nao foi possivel gravar info de restricao no arquivo: " + ex.Message, ex);
            }
        }

        private void _addMaxLossObject(TOMaxLoss to)
        {
            _cqMaxLoss.Enqueue(to);
            lock (_syncMaxLoss)
                Monitor.Pulse(_syncMaxLoss);
        }

        private void _processMaxLoss()
        {
            while (_isLoaded)
            {
                try
                {
                    TOMaxLoss to = null;
                    if (_cqMaxLoss.TryDequeue(out to))
                    {
                        _db.AtualizarMaxLossLimit(to.MaxLoss);
                        _pMaxLoss.TraceInfo(to.MaxLoss);
                    }
                    else
                    {
                        lock (_syncMaxLoss)
                            Monitor.Wait(_syncMaxLoss, 100);
                    }
                }
                catch{}
            }
        }

        private void _addPosClient(TOPosClient to)
        {
            _cqPosClient.Enqueue(to);
            lock (_syncPosClient)
                Monitor.Pulse(_syncPosClient);
        }

        private void _processPosClient()
        {
            while (_isLoaded)
            {
                try
                {
                    TOPosClient to = null;
                    if (_cqPosClient.TryDequeue(out to))
                    {
                        _db.AtualizarPositionClient(to.PositionClient);
                        //_db.InserirPositionClientMvto(to.PositionClient);
                        _pPosClient.TraceInfo(to.PositionClient);
                    }
                    else
                    {
                        lock (_syncPosClient)
                            Monitor.Wait(_syncPosClient, 100);
                    }
                }
                catch { }
            }
        }

        private void _processPosClientDB()
        {
            int i = 0;
            int mdsRefresh = 10;
            int sleep = 10;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("PositionClientDBRefresh"))
                mdsRefresh = Convert.ToInt32(ConfigurationManager.AppSettings["PositionClientDBRefresh"].ToString()); 
            while (_isLoaded)
            {
                try
                {
                    if (i >= mdsRefresh * sleep)
                    {
                        if (_isRunningDB)
                            break;
                        _isRunningDB = true;
                        this._atualizaPosClientDB();
                        i = 0;
                        _isRunningDB = false;
                    }
                    Thread.Sleep(sleep *10);
                    i++;
                }
                catch 
                {
                    i = 0;
                    _isRunningDB = false;
                }
            }
        }
        private void _atualizaPosClientDB()
        {
            try
            {
                List<PosClientSymbolInfo> lstAux = new List<PosClientSymbolInfo>();
                foreach (KeyValuePair<int, List<PosClientSymbolInfo>> item in _dicPositionClient)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        SymbolInfo aux = null;
                        if (_dicSymbols.TryGetValue(item.Value[i].Ativo, out aux))
                        {
                            item.Value[i].Variacao = aux.VlrOscilacao;
                            item.Value[i].UltPreco = aux.VlrUltima;
                            item.Value[i].PrecoFechamento = aux.VlrFechamento;
                            item.Value[i].SegmentoMercado = aux.SegmentoMercado;
                        }
                    }
                    //lstAux.AddRange(item.Value.GroupBy(x => x.Ativo).Select(g=>g.First()).ToList());
                    lstAux.AddRange(item.Value);
                }
                
                logger.Info("======> Atualizando registros PositionClient");
                for (int i = 0; i < lstAux.Count; i++)
                {
                    if (_db != null)
                    {
                        _db.AtualizarPositionClient(lstAux[i]);
                    }
                }
                logger.Info("======> Registros atualizados: " + lstAux.Count);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao das informacoes de cotacao: " + ex.Message, ex);
            }
        }



        private void _processMdsNegocio()
        {
            try
            {
                while (_isLoaded)
                {
                    try
                    {
                        MDSNegocioEventArgs mds = null;
                        if (_cqMds.TryDequeue(out mds))
                        {
                            if (mds != null)
                                this._atualizaNegocioMds(mds);
                        }
                        else
                        {
                            lock (_syncMds)
                                Monitor.Wait(_syncMds, 100);
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no processamento mds negocio: " + ex.Message, ex);
            }
        }

        private void _atualizaNegocioMds(MDSNegocioEventArgs args)
        {
            try
            {
                SymbolInfo info = new SymbolInfo();

                info.Instrumento = args.Instrumento;
                info.DtAtualizacao = args.DtAtualizacao;
                info.DtNegocio = args.DtNegocio;
                info.VlrOscilacao = args.VlrOscilacao;
                info.VlrUltima = args.VlrUltima;
                

                //logger.DebugFormat("Atualizando [{0}] DtAtu[{1}] DtHr [{2}] Osc[{3}] Prc[{4}]",
                //    info.Instrumento,
                //    info.DtAtualizacao,
                //    info.DtNegocio,
                //    info.VlrOscilacao,
                //    info.VlrUltima);

                // Atualizar informacoes do cadastro de papel
                SymbolInfo aux = this.UpdateSymbol(args.Instrumento, info);
                
                // Atualizar as informacoes de Mds para positionclient
                TOSymbolInfo to = new TOSymbolInfo();
                to.Instrumento = aux==null? info: aux;
                PositionClientManager.Instance.EnqueueSymbol(to);

                //// Atualizar informacoes da Position Client
                //this.UpdateMdsPositionClient(info);
            }
            catch (Exception ex)
            {
                logger.Error("Instance_OnNegocio: " + ex.Message, ex);
            }
        }



        public void Instance_OnNegocio(object sender, MDSNegocioEventArgs args)
        {
            try
            {
                _cqMds.Enqueue(args);
                lock (_syncMds)
                    Monitor.Pulse(_syncMds);


            }
            catch (Exception ex)
            {
                logger.Error("Instance_OnNegocio: " + ex.Message, ex);
            }
        }
        #endregion

        #region MDS Function
        
        #endregion


        #region Auxiliary Functions
        private int _getOrigOrdQty(List<SpiderOrderDetailInfo> dets)
        {
            int ordID = 0;
            try
            {
                // Buscar o penultimo status do detail (new ou replaced): Vai retornar ao menos 2 registros. Sempre pegar o penultimo
                List<SpiderOrderDetailInfo> lstDet = dets.Select(x => x).Where(x => x.OrderStatusID == (int)SpiderOrderStatus.NOVA ||
                    x.OrderStatusID == (int)SpiderOrderStatus.SUBSTITUIDA).OrderByDescending(x => x.OrderDetailID).ToList();
                // TODO [FF]: Validar a ordem dos details para ler as informacoes
                if (dets.Count > 0)
                    ordID = dets[0].OrderID;
                SpiderOrderDetailInfo od = lstDet[1];
                return od.OrderQty;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Problemas na busca do Original OrderQty. OrderID[{0}]. MsgErr[{1}/{2}]: ", ordID, ex.StackTrace,ex.Message);
                return 0;
            }
        }

        private int _getLastTradeQty(List<SpiderOrderDetailInfo> dets)
        {
            try
            {
                // Buscar o penultimo status do detail (new ou replaced): Vai retornar ao menos 2 registros. Sempre pegar o penultimo
                List<SpiderOrderDetailInfo> lstDet = dets.Select(x => x).Where(x => x.OrderStatusID == (int)SpiderOrderStatus.EXECUTADA ||
                    x.OrderStatusID == (int)SpiderOrderStatus.PARCIALMENTEEXECUTADA).OrderByDescending(x => x.OrderDetailID).ToList();
                // TODO [FF]: Validar a ordem dos details para ler as informacoes
                
                SpiderOrderDetailInfo od;
                if (lstDet.Count > 0)
                {
                    od = lstDet[0];
                    return od.TradeQty;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na busca do Original OrderQty: " + ex.Message, ex);
                return 0;
            }
        }

        private SpiderOrderDetailInfo _getLastTradeQtyDet(List<SpiderOrderDetailInfo> dets)
        {
            try
            {
                // Buscar o penultimo status do detail (new ou replaced): Vai retornar ao menos 2 registros. Sempre pegar o penultimo
                List<SpiderOrderDetailInfo> lstDet = dets.Select(x => x).Where(x => x.OrderStatusID == (int)SpiderOrderStatus.EXECUTADA ||
                    x.OrderStatusID == (int)SpiderOrderStatus.PARCIALMENTEEXECUTADA).OrderByDescending(x => x.OrderDetailID).ToList();
                // TODO [FF]: Validar a ordem dos details para ler as informacoes

                SpiderOrderDetailInfo od;
                if (lstDet.Count > 0)
                {
                    od = lstDet[0];
                    return od;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na busca do TradeQty Det: " + ex.Message, ex);
                return null;
            }
        }
        #endregion

        #region MdsInfo
        public SymbolInfo GetSymbol(string ativo)
        {
            try
            {
                SymbolInfo ret = null;
                if (_dicSymbols.TryGetValue(ativo, out ret))
                {
                    return ret;
                }
                else return null;
            }
            catch 
            {
                return null;
            }
        }

        public ConcurrentDictionary<string, SymbolInfo> GetAllSymbols()
        {
            return _dicSymbols;
        }
        #endregion

    }
}
