using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using log4net;
using QuickFix;
using QuickFix.Fields;

using Gradual.Core.OMS.LimiteManager.Database;
using Gradual.Core.OMS.LimiteManager.Lib.Dados;
using Gradual.Core.OMS.LimiteManager.Lib.Mensageria;
using Gradual.Core.OMS.LimiteManager.Streamer;
using Gradual.Core.OMS.LimiteManager.Db;
using Gradual.Core.OMS.LimiteManager.Dados;
using System.Configuration;


namespace Gradual.Core.OMS.LimiteManager
{
    public class LimitControl
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables

        private static LimitControl _me = null;
        private static object _objTrava = new object();

        DbLimite _db;
        protected Dictionary <string, SymbolInfo> _dicSymbols;
        protected Dictionary<int, ClientParameterPermissionInfo> _dicParameters;
        protected Dictionary<int, FatFingerInfo> _dicFatFinger;
        protected Dictionary<int, RiskExposureClientInfo> _dicRiskExposureClient;
        protected Dictionary<SymbolKey, BlockedInstrumentInfo> _dicBlockedSymbolGlobal;
        protected Dictionary<SymbolKey, BlockedInstrumentInfo> _dicBlockedSymbolGroupClient;
        protected Dictionary<SymbolKey, BlockedInstrumentInfo> _dicBlockedSymbolClient;
        protected Dictionary<string, OptionBlockInfo> _dicOptionSeriesBlocked;
        protected Dictionary<int, List<OperatingLimitInfo>> _dicOperatingLimit;
        protected Dictionary<int, ClientLimitBMFInfo> _dicClientLimitBMF;
        protected Dictionary<string, TestSymbolInfo> _dicTestSymbols;
        List<int> _lstIdClient;
        protected List<RiskExposureGlobalInfo> _lstRiskExposureGlobal;

        Dictionary<TipoLimiteEnum, TipoLimiteEnum> _dicDualTipoLimite;

        protected bool _isLoaded;
        //bool _isLoading = true;
        object _sync = new object();
        Dictionary<string, PersistenciaArquivo> _dicPersistArq;
        Queue<OperatingLimitInfo> _queueBovespa;
        Queue<ClientLimitBMFInfo> _queueBMF;
        Thread _thPersistMovto;

        LimitUpdate _lmtDb;
        bool _accBvspBmf;
        string[] _strAccBvspBmf;
        protected Dictionary<int, string> _dummyDic;

        List<Dictionary> _lstDics;
        // StreamerManager _stm;
        #endregion

        private const string EXCHANGEBOVESPA = "BOVESPA";
        private const string EXCHANGEBMF = "BMF";


        public LimitControl()
        {
            logger.Info("LimitControl - Constructor");
            _isLoaded = false;
            //_isLoading = false;
            _accBvspBmf = ConfigurationManager.AppSettings.AllKeys.Contains("AccountBvspBMF");
            if (_accBvspBmf)
                _strAccBvspBmf = ConfigurationManager.AppSettings["AccountBvspBMF"].ToString().Split(';');
        }

        public static LimitControl GetInstance()
        {
            lock (_objTrava)
            {
                if (_me == null)
                {
                    _me = new LimitControl();
                }
            }
            return _me;
        }


        /// <summary>
        /// Efetuar a inicializacao de todas as estruturas necessarias e efetuar a carga
        /// </summary>
        public void LoadData(bool loadSecurityList = true)
        {
            try
            {
                lock (_sync)
                {
                    logger.Info("LoadData(): Carregando dados LimitControl... ");
                    //if (loadSecurityList)
                    //    _dicSymbols = new Dictionary<string, SymbolInfo>();
                    _dicTestSymbols = new Dictionary<string, TestSymbolInfo>();
                    _dicParameters = new Dictionary<int, ClientParameterPermissionInfo>();
                    _dicFatFinger = new Dictionary<int, FatFingerInfo>();
                    _dicRiskExposureClient = new Dictionary<int, RiskExposureClientInfo>();
                    _dicBlockedSymbolGlobal = new Dictionary<SymbolKey, BlockedInstrumentInfo>();
                    _dicBlockedSymbolGroupClient = new Dictionary<SymbolKey, BlockedInstrumentInfo>();
                    _dicBlockedSymbolClient = new Dictionary<SymbolKey, BlockedInstrumentInfo>();
                    _dicOptionSeriesBlocked = new Dictionary<string, OptionBlockInfo>();
                    _dicOperatingLimit = new Dictionary<int, List<OperatingLimitInfo>>();
                    _dicClientLimitBMF = new Dictionary<int, ClientLimitBMFInfo>();
                    _lstIdClient = new List<int>();
                    _lstRiskExposureGlobal = new List<RiskExposureGlobalInfo>();

                    // Operacoes Dual de Limites (atualizacao) - bovespa
                    _dicDualTipoLimite = new Dictionary<TipoLimiteEnum, TipoLimiteEnum>();
                    _dicDualTipoLimite.Add(TipoLimiteEnum.COMPRAAVISTA, TipoLimiteEnum.VENDAAVISTA);
                    _dicDualTipoLimite.Add(TipoLimiteEnum.VENDAAVISTA, TipoLimiteEnum.COMPRAAVISTA);
                    _dicDualTipoLimite.Add(TipoLimiteEnum.COMPRAOPCOES, TipoLimiteEnum.VENDAOPCOES);
                    _dicDualTipoLimite.Add(TipoLimiteEnum.VENDAOPCOES, TipoLimiteEnum.COMPRAOPCOES);

                    // Controles de persistencia de arquivos
                    _dicPersistArq = new Dictionary<string, PersistenciaArquivo>();

                    _queueBMF = new Queue<ClientLimitBMFInfo>();
                    _queueBovespa = new Queue<OperatingLimitInfo>();
                    

                    logger.Info("Criando camada banco de dados - consulta");
                    if (null == _db)
                        _db = new DbLimite();
                    logger.Info("Criando camada banco de dados - atualizacao");
                    if (null == _lmtDb)
                        _lmtDb = new LimitUpdate();

                    logger.Info("Carregando clientes disponiveis com tipo de permissao HFT");
                    if (false == _loadClientAccounts())
                    {
                        throw new Exception("Problema no carregamento da lista de id_clientes");
                    }

                    logger.Info("Carregando Parametros e Permissoes globais");
                    if (false == _loadPermissionAndParameters())
                    {
                        throw new Exception("Problema no carregamento de parametros e permissoes globais");
                    }

                    if (loadSecurityList)
                    {
                        logger.Info("Carregando cadastro de papeis");
                        if (false == _loadInstrument())
                        {
                            logger.Info("Cadastro de papeis zerado ou nulo");
                            throw new Exception("Problema no carregamento do cadastro de papeis");
                        }

                        logger.Info("Carregando cadastro de papeis teste");
                        if (false == _loadTestInstrument())
                        {
                            logger.Info("Cadastro de papeis zerado ou nulo");
                            throw new Exception("Problema no carregamento do cadastro de papeis");
                        }
                    }
                    else
                        logger.Info("Flag de Carregamento de Cadastro de papeis nao habilitado");

                    logger.Info("Carregando parametros de fatfinger");
                    if (false == _loadFatFingerParameters())
                    {
                        throw new Exception("Problemas no carregamento de parametros de fatfinger");
                    }

                    logger.Info("Carregando parametros de exposicao de risco por cliente");
                    if (false == _loadClientRiskExposure())
                    {
                        throw new Exception("Problemas no carregamento de parametros de exposicao de risco por cliente");
                    }

                    logger.Info("Carregando instrumentos bloqueados globais");
                    if (false == _loadBlockedInstrument())
                    {
                        throw new Exception("Problemas no carregamento de instrumentos bloqueados globais");
                    }

                    logger.Info("Carregando instrumentos bloqueados por grupo de clientes");
                    if (false == _loadBlockedInstrumentPerGroup())
                    {
                        throw new Exception("Problemas no carregamento de instrumentos bloqueados por grupo de clientes");
                    }

                    logger.Info("Carregando instrumentos bloqueados por cliente");
                    if (false == _loadBlockedInstrumentPerClient())
                    {
                        throw new Exception("Problemas no carregamento de instrumentos bloqueados por cliente");
                    }

                    logger.Info("Carregando serie de vencimento de opcoes");
                    if (false == _loadOptionSeries())
                    {
                        throw new Exception("Problemas no carregamento de serie de vencimento de opcoes");
                    }

                    logger.Info("Carregando limites operacionais");
                    if (false == _loadOperationalLimit())
                    {
                        throw new Exception("Problemas no carregamento de limites operacionais");
                    }

                    logger.Info("Carregando limites BMF");
                    if (false == _loadClientLimitBMF())
                    {
                        throw new Exception("Problemas no carregamento de limites bmf");
                    }
                    _isLoaded = true;
                    logger.Info("Iniciando Thread de Movimentacao de Limite");
                    _thPersistMovto = new Thread(new ThreadStart(_processMvto));
                    _thPersistMovto.Start();
                    logger.Info("LoadData(): Dados carregados - LimitControl... ");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na cargas das informacoes de limite: " + ex.Message, ex);
                _isLoaded = false;
                throw ex;
            }
        }

        public void UnloadData(bool unloadSecurityList = true)
        {
            try
            {
                lock (_sync)
                {
                    int len = 0;
                    logger.Info("Unload Control Data...");
                    _isLoaded = false;
                    if (unloadSecurityList)
                    {
                        _dicSymbols.Clear(); _dicSymbols = null;
                        _dicTestSymbols.Clear(); _dicTestSymbols = null;
                    }
                    _dicParameters.Clear(); _dicParameters = null;
                    _dicFatFinger.Clear(); _dicFatFinger = null;
                    _dicRiskExposureClient.Clear(); _dicRiskExposureClient = null;
                    _dicBlockedSymbolGlobal.Clear(); _dicBlockedSymbolGlobal = null;
                    _dicBlockedSymbolGroupClient.Clear(); _dicBlockedSymbolGroupClient = null;
                    _dicBlockedSymbolClient.Clear(); _dicBlockedSymbolClient = null;
                    _dicOptionSeriesBlocked.Clear(); _dicOptionSeriesBlocked = null;
                    _dicOperatingLimit.Clear(); _dicOperatingLimit = null;
                    _dicClientLimitBMF.Clear(); _dicClientLimitBMF = null;
                    _lstIdClient.Clear(); _lstIdClient = null;
                    _lstRiskExposureGlobal.Clear(); _lstRiskExposureGlobal = null;
                    _dicDualTipoLimite.Clear(); _dicDualTipoLimite = null;
                    
                    if (null != _thPersistMovto && _thPersistMovto.IsAlive)
                    {
                        try
                        {
                            _thPersistMovto.Abort();
                        }
                        catch (Exception ex)
                        {
                            logger.Info("Aborting thread 'Movimento'...: " + ex.Message);
                        }
                    }

                    _dicPersistArq.Clear(); _dicPersistArq = null;
                    lock (_queueBovespa)
                    {
                        _queueBovespa.Clear(); 
                    }
                    lock (_queueBMF)
                    {
                        _queueBMF.Clear();
                    }

                    //_db = null;
                    //_lmtDb = null;
                    logger.Info("Fim Unload data...");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na limpeza das estruturas internas...: " + ex.Message, ex);
            }
        }

        public void LoadClientData(int codCliente)
        {
            try
            {
                lock (_sync)
                {
                    if (0 == codCliente)
                    {
                        logger.Info("Carregando instrumentos bloqueados globais - nao necessita de codigo cliente");
                        if (false == _loadBlockedInstrument())
                        {
                            throw new Exception("Problemas no carregamento de instrumentos bloqueados globais");
                        }
                    }
                    else
                    {
                        logger.Info("Inserindo codigo de cliente na lista de clientes:  " + codCliente);
                        if (false == _loadClientAccounts(codCliente))
                        {
                            throw new Exception("Problema na insercao do codigo de cliente: " + codCliente);
                        }
                        logger.Info("Carregando Parametros e Permissoes do cliente: " + codCliente);
                        if (false == _loadPermissionAndParameters(codCliente))
                        {
                            throw new Exception("Problema no carregamento de parametros e permissoes do cliente " + codCliente);
                        }
                        logger.Info("Carregando parametros de fatfinger do cliente: " + codCliente);
                        if (false == _loadFatFingerParameters(codCliente))
                        {
                            throw new Exception("Problemas no carregamento de parametros de fatfinger do cliente: " + codCliente);
                        }
                        logger.Info("Carregando parametros de exposicao de risco por cliente: " + codCliente);
                        if (false == _loadClientRiskExposure(codCliente))
                        {
                            throw new Exception("Problemas no carregamento de parametros de exposicao de risco por cliente: " + codCliente);
                        }
                        logger.Info("Carregando instrumentos bloqueados por grupo de clientes: " + codCliente);
                        if (false == _loadBlockedInstrumentPerGroup(codCliente))
                        {
                            throw new Exception("Problemas no carregamento de instrumentos bloqueados por grupo de clientes: " + codCliente);
                        }

                        logger.Info("Carregando instrumentos bloqueados por cliente: " + codCliente);
                        if (false == _loadBlockedInstrumentPerClient(codCliente))
                        {
                            throw new Exception("Problemas no carregamento de instrumentos bloqueados por cliente: " + codCliente);
                        }

                        logger.Info("Carregando limites operacionais do cliente: " + codCliente);
                        if (false == _loadOperationalLimit(codCliente))
                        {
                            throw new Exception("Problemas no carregamento de limites operacionais do cliente: " + codCliente);
                        }

                        logger.Info("Carregando limites BMF do cliente: " + codCliente);
                        if (false == _loadClientLimitBMF(codCliente))
                        {
                            throw new Exception("Problemas no carregamento de limites bmf do cliente: " + codCliente);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Problemas na cargas das informacoes de limite do cliente: {0} {1}", codCliente, ex.Message), ex);
                throw ex;
            }

        }

        public void UnloadClientData(int codCliente)
        {
            try
            {
                lock (_sync)
                {
                    logger.Info("Inicio da desalocacao de informacoes de limites do cliente: " + codCliente);
                    // Efetuar limpeza somente do parametro global
                    if (0 == codCliente)
                    {
                        logger.Info("Efetuando limpeza somente dos ativos bloqueados");
                        lock (_dicBlockedSymbolGlobal)
                        {
                            _dicBlockedSymbolGlobal.Clear();
                        }
                    }
                    else
                    {
                        // Removendo parametros de limite parametrizados a partir de codigo de cliente
                        // OBS: Outros parametros globais NAO SAO RECARREGADOS
                        
                        lock (_lstIdClient)
                        {
                            if (_lstIdClient.Contains(codCliente))
                                _lstIdClient.Remove(codCliente);
                        }

                        lock (_dicParameters)
                        {
                            if (_dicParameters.ContainsKey(codCliente))
                                _dicParameters.Remove(codCliente);
                        }
                        lock (_dicFatFinger)
                        {
                            if (_dicFatFinger.ContainsKey(codCliente))
                                _dicFatFinger.Remove(codCliente);
                        }
                        lock (_dicRiskExposureClient)
                        {
                            if (_dicRiskExposureClient.ContainsKey(codCliente))
                                _dicRiskExposureClient.Remove(codCliente);
                        }
                        lock (_dicBlockedSymbolGroupClient)
                        {
                            var list = _dicBlockedSymbolGroupClient.Where(x => x.Key.Account == codCliente).Select(x => x.Key).ToList();
                            foreach (SymbolKey aux in list)
                            {
                                _dicBlockedSymbolGroupClient.Remove(aux);
                            }
                        }
                        lock (_dicBlockedSymbolClient)
                        {
                            var list = _dicBlockedSymbolClient.Where(x => x.Key.Account == codCliente).Select(x => x.Key).ToList();
                            foreach (SymbolKey aux in list)
                            {
                                _dicBlockedSymbolClient.Remove(aux);
                            }
                        }

                        lock (_dicOperatingLimit)
                        {
                            if (_dicOperatingLimit.ContainsKey(codCliente))
                                _dicOperatingLimit.Remove(codCliente);
                        }
                        lock (_dicClientLimitBMF)
                        {
                            int auxCodCliente = this.ParseAccount(codCliente);
                            if (_dicClientLimitBMF.ContainsKey(auxCodCliente))
                                _dicClientLimitBMF.Remove(auxCodCliente);
                        }
                    }
                    logger.Info("Fim da desalocacao de informacoes do cliente: " + codCliente);
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Problemas na cargas das informacoes de limite do cliente: {0} {1}", codCliente, ex.Message), ex);
                throw ex;
            }
        }


        #region Movto Storage Control
        private void _processMvto()
        {
            try
            {
                while (_isLoaded)
                {
                    if ((_queueBovespa.Count > 0) || (_queueBMF.Count > 0))
                    {
                        
                        if (_queueBovespa.Count > 0)
                        {
                            List<OperatingLimitInfo> lstMvtoBov = new List<OperatingLimitInfo>();
                            lock (_queueBovespa)
                            {
                                lstMvtoBov.AddRange(_queueBovespa.ToArray());
                                _queueBovespa.Clear();
                            }

                            int len = lstMvtoBov.Count;
                            for (int i = 0; i < len; i++)
                            {
                                string chave = EXCHANGEBOVESPA + "." + lstMvtoBov[i].CodigoCliente;
                            
                                if (!_dicPersistArq.ContainsKey(chave))
                                {
                                    PersistenciaArquivo pstArq = new PersistenciaArquivo(EXCHANGEBOVESPA, lstMvtoBov[i].CodigoCliente);
                                    _dicPersistArq.Add(chave, pstArq);
                                }
                                _dicPersistArq[chave].InserirMvtoBovespa(lstMvtoBov[i]);
                                InserirMvtoBvspRequest reqBvsp = new InserirMvtoBvspRequest();
                                reqBvsp.LimiteBovespa = lstMvtoBov[i];
                                _lmtDb.InserirMvtoBvsp(reqBvsp);
                            }
                            // Atualizar o estado mais atual de cada tipo de Limite (Compra Vista, Venda Vista, Comra Opcao, Venda Opcao)
                            AtualizaMvtoBvspRequest req = new AtualizaMvtoBvspRequest();
                            OperatingLimitInfo item = lstMvtoBov.LastOrDefault(x=>x.TipoLimite == TipoLimiteEnum.COMPRAAVISTA);
                            if (null != item)
                            {
                                req.LimiteBovespa = item;
                                _lmtDb.AtualizaMvtoBvsp(req);
                            }
                            item = lstMvtoBov.LastOrDefault(x=>x.TipoLimite == TipoLimiteEnum.VENDAAVISTA);
                            if (null != item)
                            {
                                req.LimiteBovespa = item;
                                _lmtDb.AtualizaMvtoBvsp(req);
                            }
                            item = lstMvtoBov.LastOrDefault(x=>x.TipoLimite == TipoLimiteEnum.COMPRAOPCOES);
                            if (null != item)
                            {
                                req.LimiteBovespa = item;
                                _lmtDb.AtualizaMvtoBvsp(req);
                            }
                            item = lstMvtoBov.LastOrDefault(x => x.TipoLimite == TipoLimiteEnum.VENDAOPCOES);
                            if (null != item)
                            {
                                req.LimiteBovespa = item;
                                _lmtDb.AtualizaMvtoBvsp(req);
                            }
                            lstMvtoBov.Clear();
                            lstMvtoBov = null;
                        }
                        if (_queueBMF.Count > 0)
                        {
                            List<ClientLimitBMFInfo> lstMvtoBmf = new List<ClientLimitBMFInfo>();
                            lock (_queueBMF)
                            {
                                lstMvtoBmf.AddRange(_queueBMF.ToArray());
                                _queueBMF.Clear();
                            }

                            int len = lstMvtoBmf.Count;
                            for (int i = 0; i < len; i++)
                            {
                                string chave = EXCHANGEBMF + "." + lstMvtoBmf[i].Account;
                            
                                if (!_dicPersistArq.ContainsKey(chave))
                                {
                                    PersistenciaArquivo pstArq = new PersistenciaArquivo(EXCHANGEBMF, lstMvtoBmf[i].Account);
                                    _dicPersistArq.Add(chave, pstArq);
                                }
                                _dicPersistArq[chave].InserirMvtoBmf(lstMvtoBmf[i]);
                                // _lmtDb.InserirMvtoBmf(lstMvtoBmf[i]);
                            }
                            // Atualizar os saldos, (todos os lados, para contrato e instrumento)
                            AtualizaMvtoBMFContratoRequest req = new AtualizaMvtoBMFContratoRequest();
                            ClientLimitBMFInfo aux = lstMvtoBmf.LastOrDefault(x => x.ContractLimit[0].Sentido == "C");
                            if (null != aux)
                            {
                                req.ContractLimit = aux.ContractLimit[0];
                                _lmtDb.AtualizaMvtoBMFContrato(req);
                            }
                            aux = lstMvtoBmf.LastOrDefault(x => x.ContractLimit[0].Sentido == "V");
                            if (null != aux)
                            {
                                req.ContractLimit = aux.ContractLimit[0];
                                _lmtDb.AtualizaMvtoBMFContrato(req);
                            }

                            AtualizaMvtoBMFInstrumentoRequest req2 = new AtualizaMvtoBMFInstrumentoRequest();
                            aux = lstMvtoBmf.LastOrDefault(x => x.InstrumentLimit[0].Sentido == "C");
                            if (null != aux)
                            {
                                req2.InstrumentLimit = aux.InstrumentLimit[0];
                                _lmtDb.AtualizaMvtoBMFInstrumento(req2);
                            }
                            aux = lstMvtoBmf.LastOrDefault(x => x.InstrumentLimit[0].Sentido == "V");
                            if (null != aux)
                            {
                                req2.InstrumentLimit = aux.InstrumentLimit[0];
                                _lmtDb.AtualizaMvtoBMFInstrumento(req2);
                            }
                            lstMvtoBmf.Clear();
                            lstMvtoBmf = null;
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

        private void _addBvspMvto(OperatingLimitInfo item)
        {
            lock (_queueBovespa)
            {
                _queueBovespa.Enqueue(item);
                //Monitor.Pulse(_queueBovespa);
            }
        }

        private void _addBmfMvto(ClientLimitBMFInfo item)
        {
            lock (_queueBMF)
            {
                _queueBMF.Enqueue(item);
                //Monitor.Pulse(_queueBMF);
            }
        }

        #endregion


        #region Loading DATA functions
        private bool _loadClientAccounts(int codCliente = 0)
        {
            try
            {
                if (codCliente == 0)
                {
                    _lstIdClient = _db.ObterAccountHFT();
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

        /// <summary>
        /// Carregamento do cadastro de papeis
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Carregamento do cadastro de papeis
        /// </summary>
        /// <returns></returns>
        private bool _loadTestInstrument()
        {
            try
            {
                bool retBvsp = false;
                bool retBmf = false;
                List<TestSymbolInfo> testBvsp = null;
                List<TestSymbolInfo> testBmf = null;
                testBvsp = _db.CarregarPapeisDeTeste(EXCHANGEBOVESPA);
                if (null != testBvsp)
                {
                    logger.Info("_loadTestInstruments() - No. Registros Bovespa: " + testBvsp.Count);
                    for (int i = 0; i < testBvsp.Count; i++)
                    {
                        _dicTestSymbols.Add(EXCHANGEBOVESPA + "/" + testBvsp[i].Instrument, testBvsp[i]);
                    }
                    retBvsp = true;
                }
                testBmf = _db.CarregarPapeisDeTeste(EXCHANGEBMF);
                if (null != testBmf)
                {
                    logger.Info("_loadTestInstruments() - No. Registros BMF: " + testBmf.Count);
                    for (int i = 0; i < testBmf.Count; i++)
                    {
                        _dicTestSymbols.Add(EXCHANGEBMF + "/" + testBmf[i].Instrument, testBmf[i]);
                    }
                    retBmf = true;
                }
                if (retBvsp && retBmf)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga dos instrumentos de teste: " + ex.Message, ex);
                return false;
            }
        }
        /// <summary>
        /// Carregamento dos parametros e permissoes globais
        /// </summary>
        /// <returns></returns>
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
                                _dicParameters.Add(acc, aux);
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
                        lock (_dicParameters) { _dicParameters.Add(account, aux); }
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

        private bool _loadFatFingerParameters(int account = 0)
        {
            try
            {
                if (account == 0)
                {
                    if (null != _lstIdClient && _lstIdClient.Count > 0)
                    {
                        foreach (int acc in _lstIdClient)
                        {
                            FatFingerInfo fat = _db.CarregarParametrosFatFinger(acc);
                            if (null != fat)
                                _dicFatFinger.Add(acc, fat);
                        }
                    }
                }
                // Load only one account
                else
                {
                    FatFingerInfo fat = _db.CarregarParametrosFatFinger(account);
                    if (null != fat)
                    {
                        lock (_dicFatFinger) { _dicFatFinger.Add(account, fat); }
                    }
                }
                logger.Info("_loadFatFingerParameters() - No. Registros: " + _dicFatFinger.Count);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga das permissoes de FatFinger: " + ex.Message, ex);
                return false;
            }
        }

        private bool _loadClientRiskExposure(int account=0)
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
                                _dicRiskExposureClient.Add(acc, item);
                        }
                    }
                }
                // Load only one account
                else
                {
                    RiskExposureClientInfo item = _db.CarregarExposicaoRiscoCliente(account, DateTime.Now);
                    if (null != item)
                    {
                        lock (_dicRiskExposureClient) { _dicRiskExposureClient.Add(account, item); }
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

        private bool _loadGlobalRiskExposure()
        {
            try
            {
                _lstRiskExposureGlobal = _db.CarregarExposicaoRiscoGlobal();
                logger.Info("_loadGlobalRiskExposure() - No. Registros: " + _lstRiskExposureGlobal.Count);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na carga dos parametros de Exposicao de risco global: " + ex.Message, ex);
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
                                _dicBlockedSymbolGlobal.Add(instKey, item);
                        }
                        else
                        {
                            // Ambos, adicionara Side C /V
                            instKey.Side = SentidoBloqueioEnum.Compra;
                            if (!_dicBlockedSymbolGlobal.ContainsKey(instKey))
                                _dicBlockedSymbolGlobal.Add(instKey, item);
                            SymbolKey instKey2 = new SymbolKey();
                            instKey2.Instrument = item.Instrumento.ToUpper();
                            instKey2.Side = SentidoBloqueioEnum.Venda;
                            if (!_dicBlockedSymbolGlobal.ContainsKey(instKey2))
                                _dicBlockedSymbolGlobal.Add(instKey2, item);
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
                                            _dicBlockedSymbolGroupClient.Add(instKey, item);
                                    }
                                    else
                                    {
                                        // Adicionar C / V
                                        instKey.Side = SentidoBloqueioEnum.Compra;
                                        if (!_dicBlockedSymbolGroupClient.ContainsKey(instKey))
                                            _dicBlockedSymbolGroupClient.Add(instKey, item);
                                        SymbolKey instKey2 = new SymbolKey();
                                        instKey2.Instrument = item.Instrumento.ToUpper();
                                        instKey2.Side = SentidoBloqueioEnum.Venda;
                                        instKey2.Account = acc;
                                        if (!_dicBlockedSymbolGroupClient.ContainsKey(instKey2))
                                            _dicBlockedSymbolGroupClient.Add(instKey2, item);

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
                                        _dicBlockedSymbolGroupClient.Add(instKey, item);
                                }
                            }
                            else
                            {
                                // Adicionar C / V
                                instKey.Side = SentidoBloqueioEnum.Compra;
                                if (!_dicBlockedSymbolGroupClient.ContainsKey(instKey))
                                    _dicBlockedSymbolGroupClient.Add(instKey, item);
                                SymbolKey instKey2 = new SymbolKey();
                                instKey2.Instrument = item.Instrumento.ToUpper();
                                instKey2.Side = SentidoBloqueioEnum.Venda;
                                instKey2.Account = account;
                                lock (_dicBlockedSymbolGroupClient)
                                {
                                    if (!_dicBlockedSymbolGroupClient.ContainsKey(instKey2))
                                        _dicBlockedSymbolGroupClient.Add(instKey2, item);
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
                                            _dicBlockedSymbolClient.Add(instKey, item);
                                    }
                                    else
                                    {
                                        // Cadastrar C/V
                                        // Adicionar C / V
                                        instKey.Side = SentidoBloqueioEnum.Compra;
                                        if (!_dicBlockedSymbolClient.ContainsKey(instKey))
                                            _dicBlockedSymbolClient.Add(instKey, item);
                                        SymbolKey instKey2 = new SymbolKey();
                                        instKey2.Instrument = item.Instrumento.ToUpper();
                                        instKey2.Side = SentidoBloqueioEnum.Venda;
                                        instKey2.Account = acc;
                                        if (!_dicBlockedSymbolClient.ContainsKey(instKey2))
                                            _dicBlockedSymbolClient.Add(instKey2, item);
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
                                        _dicBlockedSymbolClient.Add(instKey, item);
                                }
                            }
                            else
                            {
                                // Cadastrar C/V
                                // Adicionar C / V
                                instKey.Side = SentidoBloqueioEnum.Compra;
                                if (!_dicBlockedSymbolClient.ContainsKey(instKey))
                                    _dicBlockedSymbolClient.Add(instKey, item);
                                SymbolKey instKey2 = new SymbolKey();
                                instKey2.Instrument = item.Instrumento.ToUpper();
                                instKey2.Side = SentidoBloqueioEnum.Venda;
                                instKey2.Account = account;
                                lock (_dicBlockedSymbolClient)
                                {
                                    if (!_dicBlockedSymbolClient.ContainsKey(instKey2))
                                        _dicBlockedSymbolClient.Add(instKey2, item);
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
                                _dicOperatingLimit.Add(acc, item);
                        }
                    }
                }
                else
                {
                    List<OperatingLimitInfo> item = _db.CarregarLimiteOperacional(account);
                    if (null != item)
                        lock (_dicOperatingLimit)
                        {
                            _dicOperatingLimit.Add(account, item);
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
                            int accountaux = this.ParseAccount(acc);
                            ClientLimitBMFInfo item = _db.CarregarLimitesBMF(accountaux);
                            
                            if (null != item)
                                _dicClientLimitBMF.Add(accountaux, item);
                        }
                    }
                }
                else
                {
                    int accountaux = this.ParseAccount(account);
                    ClientLimitBMFInfo item = _db.CarregarLimitesBMF(accountaux);
                    if (null != item)
                        lock (_dicClientLimitBMF)
                        {
                            _dicClientLimitBMF.Add(accountaux, item);
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


        /// <summary>
        /// Busca a leitura a partir do parametro para conta bovespa e bmf diferentes
        /// Caso seja passado a conta bovespa, retorna bmf. Caso passe bmf, retorna o codigo bovespa
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public int ParseAccount(int account)
        {
            // Verificar se ha parse de contas bmf
            int conta = 0;
            if (_accBvspBmf)
            {
                conta = account;
                for (int i = 0; i < _strAccBvspBmf.Length; i++)
                {
                    if (!string.IsNullOrEmpty(_strAccBvspBmf[i]))
                    {
                        string[] aux = _strAccBvspBmf[i].Split('=');
                        if (aux[0].Equals(account.ToString()))
                        {
                            conta = Convert.ToInt32(aux[1]);
                        }
                        if (aux[1].Equals(account.ToString()))
                        {
                            conta = Convert.ToInt32(aux[0]);
                        }
                    }
                }
            }
            else
            {
                conta = account;
            }
            return conta;
        }

        #endregion

        #region Limit Validation Functions

        private LimitResponse _fillResponse(int code, string msg, string stack="")
        {
            LimitResponse ret = new LimitResponse();
            ret.ErrorMessage = msg;
            ret.ErrorCode = code;
            ret.ErrorStack = stack;
            return ret;
        }

        public LimitResponse FormatLimitResponse(int code, string msg)
        {
            return _fillResponse(code, msg);
        }


        public LimitResponse VerifyTestInstrument(string symbol)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }
                TestSymbolInfo aux = null;
                string key1 = EXCHANGEBOVESPA + "/" + symbol;
                string key2 = EXCHANGEBMF + "/" + symbol;
                if (_dicTestSymbols.TryGetValue(key1, out aux) || _dicTestSymbols.TryGetValue(key1, out aux))
                {
                    return ret;
                }
                else
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_TEST_INSTRUMENT_NOT_FOUND, ErrorMessages.ERR_TEST_INSTRUMENT_NOT_FOUND);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na validacao papeis de teste: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }


        /// <summary>
        /// Validacao de exposicao patrimonial
        /// Baseado em ProcessarOrdem, region REGRA DE EXPOSICAO PATRIMONIAL
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public LimitResponse VerifyRiskExposure(int account)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                ClientParameterPermissionInfo clientPermission = null;
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }
                // Verificar se existe parametro / permissao do cliente (ValidarExposicaoPatrimonial)
                if (!_dicParameters.TryGetValue(account, out clientPermission))
                {
                    // Só retorna ok, não tem permissão configurada 
                    // ret =  _fillResponse(ErrorMessages.ERR_CODE_PARAM_PERM_NOT_FOUND, ErrorMessages.ERR_PARAM_PERM_NOT_FOUND);
                    return ret;
                }
                // Verificar permissao de validar exposicao patrimonial
                var item = clientPermission.Permissoes.Find(x => x.Permissao == RiscoPermissoesEnum.ValidarExposicaoPatrimonial);
                if (null == item)
                {
                    // Só retorna ok, não tem permissão configurada
                    //ret = _fillResponse(ErrorMessages.ERR_CODE_PARAM_PERM_NOT_FOUND, ErrorMessages.ERR_PARAM_PERM_NOT_FOUND);
                    return ret;
                }
                // Risco de exposicao por cliente
                RiskExposureClientInfo riskExposureClient = null;
                if (!_dicRiskExposureClient.TryGetValue(account, out riskExposureClient))
                {
                    // Se nao achou, o codigo original passa como ok, para continuar a validacao
                    // Cliente ainda não possui ordens enviadas para o dia
                    
                    ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
                    return ret;
                }

                logger.Debug("Posicao encontrada...");
                decimal oscilacaoMaxima = decimal.Zero;
                decimal prejuizoMaximo = decimal.Zero;
                decimal patrimonioLiquido = decimal.Zero;
                decimal lucroPrejuizo = decimal.Zero;
                
                logger.Debug("Patrimonio Liquido.............:" + riskExposureClient.PatrimonioLiquido.ToString());
                logger.Debug("Lucro Prejuizo.................:" + riskExposureClient.LucroPrejuizo.ToString());
                logger.Debug("Data Atualizacao...............:" + riskExposureClient.DataAtualizacao.ToString());
                
                // Risco de exposicao maxima
                RiskExposureGlobalInfo riskExposureGlobal = _lstRiskExposureGlobal[0];
                oscilacaoMaxima = riskExposureGlobal.OscilacaoMaxima / 100;
                prejuizoMaximo = riskExposureGlobal.PrejuizoMaximo;
                patrimonioLiquido = riskExposureClient.PatrimonioLiquido;
                logger.Debug("Parametros de prejuizo de oscilacao maximo carregados com sucesso.");

                if (lucroPrejuizo != 0)
                {
                    decimal percPerdaPatrimonial = ((lucroPrejuizo / patrimonioLiquido) * 100);
                    if ((oscilacaoMaxima > 0) && (prejuizoMaximo != 0))
                    {
                        if (percPerdaPatrimonial >= oscilacaoMaxima)
                        {
                            string mensagem = string.Format(ErrorMessages.ERR_MAX_PATRIMONIAL_ACHIEVED, percPerdaPatrimonial.ToString());
                            logger.Debug(mensagem);
                            ret = _fillResponse(ErrorMessages.ERR_CODE_MAX_PATRIMONIAL_ACHIEVED, mensagem);
                            return ret;
                        }
                        if (lucroPrejuizo > prejuizoMaximo)
                        {
                            string mensagem = string.Format(ErrorMessages.ERR_MAX_LOSS_ACHIEVED, lucroPrejuizo.ToString());
                            logger.Debug(mensagem);
                            ret = _fillResponse(ErrorMessages.ERR_CODE_MAX_LOSS_ACHIEVED, mensagem);
                            return ret;
                        }
                    }

                    if ((oscilacaoMaxima > 0) && (prejuizoMaximo == 0))
                    {
                        if (percPerdaPatrimonial >= oscilacaoMaxima)
                        {
                            string mensagem = string.Format(ErrorMessages.ERR_MAX_PATRIMONIAL_ACHIEVED, percPerdaPatrimonial.ToString());
                            logger.Debug(mensagem);
                            ret = _fillResponse(ErrorMessages.ERR_CODE_MAX_PATRIMONIAL_ACHIEVED, mensagem);
                            return ret;
                        }
                    }

                    if ((oscilacaoMaxima == 0) && (prejuizoMaximo != 0))
                    {
                        if (lucroPrejuizo > prejuizoMaximo)
                        {
                            string mensagem = string.Format(ErrorMessages.ERR_MAX_PATRIMONIAL_ACHIEVED, percPerdaPatrimonial.ToString());
                            logger.Debug(mensagem);
                            ret = _fillResponse(ErrorMessages.ERR_CODE_MAX_PATRIMONIAL_ACHIEVED, mensagem);
                            return ret;
                        }
                    }
                }
                logger.Debug("Cliente encontra-se fora da zona de risco");
                logger.Debug("Continuar validacao....");
                ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na validacao de exposicao de risco: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public LimitResponse VerifyOMSOrderSend(int account)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                ClientParameterPermissionInfo clientPermission = null;
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }

                // Verificar se existe parametro / permissao do cliente 
                if (!_dicParameters.TryGetValue(account, out clientPermission))
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_PARAM_PERM_NOT_FOUND, ErrorMessages.ERR_PARAM_PERM_NOT_FOUND);
                    return ret;
                }

                ParameterPermissionClientInfo item = null;
                item = clientPermission.Permissoes.Find(x => x.Permissao == RiscoPermissoesEnum.BloquearEnvioOrdemOMS);
                if (null != item)
                {
                    string msg = string.Format(ErrorMessages.ERR_OMS_SENDING_ORDER, account);
                    ret = _fillResponse(ErrorMessages.ERR_CODE_OMS_SENDING_ORDER, msg);
                    return ret;
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na validacao de permissao de envio de ordens: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }

        /// <summary>
        /// Permissao de perfil institucional
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public LimitResponse VerifyInstitutionalProfile(int account)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                ClientParameterPermissionInfo clientPermission = null;
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }

                // Verificar se existe parametro / permissao do cliente 
                if (!_dicParameters.TryGetValue(account, out clientPermission))
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_PARAM_PERM_NOT_FOUND, ErrorMessages.ERR_PARAM_PERM_NOT_FOUND);
                    return ret;
                }
                // Verificar se o perfil institucional esta atribuido
                ParameterPermissionClientInfo item = null;
                item = clientPermission.Permissoes.Find(x => x.Permissao == RiscoPermissoesEnum.PerfilInstitucional);
                if (null != item)
                {
                    string msg = string.Format(ErrorMessages.ERR_INSTITUTIONAL_PROFILE_FOUND, account);
                    ret = _fillResponse(ErrorMessages.ERR_CODE_INSTITUTIONAL_PROFILE_FOUND, msg);
                    return ret;
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na validacao de permissao institucional: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }



        /// <summary>
        /// Verificar permissao de segmento mercado pelo cliente
        /// Baseado em ProcessarOrdem, region EXPOSICAO PATRIMONIAL E PERMISSOES POR SEGMENTO DE MERCADO.
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public LimitResponse VerifyMarketPermission(int account, string symbol, string exchange)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                
                ClientParameterPermissionInfo clientPermission = null;
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }
                
                // Buscar informacoes do cadastro de papel
                SymbolInfo instrument = null;
                if (!_dicSymbols.TryGetValue(symbol, out instrument))
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_SYMBOL_NOT_FOUND, ErrorMessages.ERR_SYMBOL_NOT_FOUND);
                    return ret;
                }
                
                if (!exchange.Equals(EXCHANGEBMF, StringComparison.InvariantCultureIgnoreCase))
                    StreamerManager.GetInstance().AddInstrument(symbol, instrument);

                // Verificar se existe parametro / permissao do cliente (ValidarExposicaoPatrimonial)
                if (!_dicParameters.TryGetValue(account, out clientPermission))
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_PARAM_PERM_NOT_FOUND, ErrorMessages.ERR_PARAM_PERM_NOT_FOUND);
                    return ret;
                }
                ParameterPermissionClientInfo item = null;
                switch (instrument.SegmentoMercado)
                {
                    case SegmentoMercadoEnum.AVISTA:
                        item = clientPermission.Permissoes.Find(x => x.Permissao == RiscoPermissoesEnum.OperarMercadoAVista);
                        if (null == item)
                        {
                            string msg = string.Format(ErrorMessages.ERR_NO_PERM_VISTA, account);
                            ret = _fillResponse(ErrorMessages.ERR_CODE_NO_PERM_VISTA, msg);
                            return ret;
                        }
                        break;
                    case SegmentoMercadoEnum.OPCAO:
                        item = clientPermission.Permissoes.Find(x => x.Permissao == RiscoPermissoesEnum.OperarMercadoOpcoes);
                        if (null == item)
                        {
                            string msg = string.Format(ErrorMessages.ERR_NO_PERM_OPCAO, account);
                            ret = _fillResponse(ErrorMessages.ERR_CODE_NO_PERM_OPCAO, msg);
                            return ret;
                        }
                        break;
                    case SegmentoMercadoEnum.FUTURO:
                        item = clientPermission.Permissoes.Find(x => x.Permissao == RiscoPermissoesEnum.OperarMercadoFuturo);
                        if (null == item)
                        {
                            string msg = string.Format(ErrorMessages.ERR_NO_PERM_FUT, account);
                            ret = _fillResponse(ErrorMessages.ERR_CODE_NO_PERM_FUT, msg);
                            return ret;
                        }
                        break;
                }
                ret.InfoObject = instrument;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na validacao de permissoes de mercado: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }
        /// <summary>
        /// Verificar permissoes de negociacao do instrumento
        /// Baseado em ProcessarOrdens, region PERMISSAO GLOBAL DO INSTRUMENTO
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public LimitResponse VerifyInstrumentGlobalPermission(string symbol, int side)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }
                BlockedInstrumentInfo security = null;
                SymbolKey aux = new SymbolKey();
                aux.Instrument = symbol.ToUpper();
                switch (side)
                {
                    case 1:
                        aux.Side = SentidoBloqueioEnum.Compra; break;
                    case 2:
                        aux.Side = SentidoBloqueioEnum.Venda; break;
                    default:
                        aux.Side = SentidoBloqueioEnum.Ambos; break;
                }
                // Se nao encontrado, retorna ok, instrumento nao esta bloqueado
                if (!_dicBlockedSymbolGlobal.TryGetValue(aux, out security))
                {
                    return ret;
                }
                // Encontrado, instrumento esta bloqueado
                else
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_INSTRUMENT_GLOBAL_BLOCKED, ErrorMessages.ERR_INSTRUMENT_GLOBAL_BLOCKED);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na validacao de permissao global de instrumento: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }
        /// <summary>
        /// Validar bloqueio de instrumento por grupo
        /// Baseado em ProcessarOrdem, region PERMISSAO INSTRUMENTO X GRUPO
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="side"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public LimitResponse VerifyInstrumentPerGroupPermission(string symbol, int side, int account)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }

                BlockedInstrumentInfo security = null;
                SymbolKey aux = new SymbolKey();
                aux.Instrument = symbol.ToUpper();
                switch (side)
                {
                    case 1:
                        aux.Side = SentidoBloqueioEnum.Compra; break;
                    case 2:
                        aux.Side = SentidoBloqueioEnum.Venda; break;
                    default:
                        aux.Side = SentidoBloqueioEnum.Ambos; break;
                }
                aux.Account = account;
                // Se nao encontrado, retorna ok, instrumento nao esta bloqueado
                if (!_dicBlockedSymbolGroupClient.TryGetValue(aux, out security))
                {
                    return ret;
                }
                // instrumento encontrado, entao esta bloqueado por grupo
                else
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_INSTRUMENT_GROUP_BLOCKED, ErrorMessages.ERR_INSTRUMENT_GROUP_BLOCKED);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na validacao de permissao por grupo do instrumento: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }

        /// <summary>
        /// Validar bloqueio de instrumento por cliente
        /// Baseado em ProcessarOrdem, region PERMISSAO INSTRUMENTO CLIENTE
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="side"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public LimitResponse VerifyInstrumentClientPermission(string symbol, int side, int account)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }

                BlockedInstrumentInfo security = null;
                SymbolKey aux = new SymbolKey();
                aux.Instrument = symbol.ToUpper();
                switch (side)
                {
                    case 1:
                        aux.Side = SentidoBloqueioEnum.Compra; break;
                    case 2:
                        aux.Side = SentidoBloqueioEnum.Venda; break;
                    default:
                        aux.Side = SentidoBloqueioEnum.Ambos; break;
                }
                aux.Account = account;
                // Se nao encontrado, retorna ok, instrumento nao esta bloqueado
                if (!_dicBlockedSymbolClient.TryGetValue(aux, out security))
                {
                    return ret;
                }
                // instrumento encontrado, entao esta bloqueado por grupo
                else
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_INSTRUMENT_CLIENT_BLOCKED, ErrorMessages.ERR_INSTRUMENT_CLIENT_BLOCKED);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na validacao de permissao por cliente do instrumento: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }
        /// <summary>
        /// Validacao de regras de FatFinger (volume por boleta)
        /// Baseado em ProcessarOrdem, region FATFINGER
        /// </summary>
        /// <param name="account"></param>
        /// <param name="symbol"></param>
        /// <param name="orderQty"></param>
        /// <returns></returns>
        public LimitResponse VerifyFatFinger(int account, SymbolInfo symbol, decimal orderQty)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }

                // Buscar informacoes da regra FatFinger
                FatFingerInfo fatFinger = null;
                if (!_dicFatFinger.TryGetValue(account, out fatFinger))
                {
                    logger.Debug("Regra FatFinger nao encontrada para " + account);
                    // Retorna ok
                    //ret = _fillResponse(ErrorMessages.ERR_CODE_FAT_FINGER_NOT_FOUND, ErrorMessages.ERR_FAT_FINGER_NOT_FOUND);
                    return ret;
                }

                // Validar o volume da oferta
                decimal valorMaximo, quantidade, volume = decimal.Zero;
                valorMaximo = fatFinger.ValorRegra;
                quantidade = orderQty;
                decimal vlrCotacao = StreamerManager.GetInstance().GetLastPrice(symbol.Instrumento);
                
                if (vlrCotacao == Decimal.Zero)
                {
                    // Erro de preco base de calculo
                    ret = _fillResponse(ErrorMessages.ERR_CODE_FAT_FINGER_BASE_PRICE_ZEROED, ErrorMessages.ERR_FAT_FINGER_BASE_PRICE_ZEROED);
                    return ret;
                }

                if (symbol.LotePadrao > 100)
                    volume = (vlrCotacao * (quantidade / symbol.LotePadrao));
                else
                    volume = (vlrCotacao * quantidade);
                if (volume > valorMaximo)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_ORDER_LIMIT_EXCEEDED, ErrorMessages.ERR_ORDER_LIMIT_EXCEEDED);
                    return ret;
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na validacao de fat finger por cliente: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }
        /// <summary>
        /// Verifica se a serie da opcao esta bloqueada
        /// baseado em ProcessarOrdem, region VALIDA SE A SERIE DE OPCAO ESTA BLOQUEADA
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        public LimitResponse VerifiyOptionSeriesBlocked(string serie)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }

                // Buscar informacoes da regra FatFinger
                OptionBlockInfo optionBlock = null;
                if (_dicOptionSeriesBlocked.TryGetValue(serie, out optionBlock))
                {
                    string msg = string.Format(ErrorMessages.ERR_SERIES_OPTION_BLOCKED, serie);
                    ret = _fillResponse(ErrorMessages.ERR_CODE_SERIES_OPTION_BLOCKED, msg);
                    return ret;
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na validacao de bloqueio de serie de opcao: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }


        /// <summary>
        /// Verificacao dos limites de operacao
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="tpLimite"></param>
        /// <param name="valueToCompare"></param>
        /// <returns></returns>
        public LimitResponse VerifyOperatingLimit(int account, TipoLimiteEnum tpLimite, decimal valueToCompare)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }

                // Buscar informacoes da regra de operacao de limites
                List<OperatingLimitInfo> opLimitLst = null;
                if (!_dicOperatingLimit.TryGetValue(account, out opLimitLst))
                {
                    string msg = string.Format(ErrorMessages.ERR_OPERATING_LIMIT_NOT_FOUND, tpLimite.ToString());
                    ret = _fillResponse(ErrorMessages.ERR_CODE_OPERATING_LIMIT_NOT_FOUND, msg);
                    return ret;
                }
                
                
                OperatingLimitInfo opLimit = opLimitLst.Find(x=> x.TipoLimite == tpLimite);
                OperatingLimitInfo opLimitDual = opLimitLst.Find(x => x.TipoLimite == _dicDualTipoLimite[tpLimite]);
                // Valida se existe os 2 lados do limite (ex... compra a vista e venda a vista)
                // OBS: regra baseada em ProcessarOrdem.cs
                if (null != opLimit && opLimitDual !=null)
                {
                    if (valueToCompare > opLimit.ValorDisponivel)
                    {
                        ret = _fillResponse(ErrorMessages.ERR_CODE_OPERATING_LIMIT_EXCEEDS, ErrorMessages.ERR_OPERATING_LIMIT_EXCEEDS);
                        return ret;
                    }
                }
                else
                {
                    string msg = string.Format(ErrorMessages.ERR_OPERATING_LIMIT_NOT_FOUND, tpLimite.ToString());
                    ret = _fillResponse(ErrorMessages.ERR_CODE_OPERATING_LIMIT_NOT_FOUND, msg);
                    return ret;
                }
                ret.InfoObject = opLimit;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na validacao no limite operacional: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }

        public LimitResponse VerifyClientBMFLimit(int account, string symbol, string side, decimal orderQty, decimal originalQty)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }
                //int acc = this.ParseAccount(account);
                int qtdContrato = 0;
                int qtdInstrumento = 0;
                int idClienteParametroBmf = 0;
                string instrument = string.Empty;
                char stUtilizacaoInstrumento = 'N';

                string sentido = side.Equals("1") ? "C" : "V";

                // Buscar informacoes da regra BMF
                ClientLimitBMFInfo  bmfLimit = null;
                if (!_dicClientLimitBMF.TryGetValue(account, out bmfLimit))
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_NOT_FOUND, ErrorMessages.ERR_BMF_LIMIT_NOT_FOUND);
                    return ret;
                }

                // Contrato base
                string contract = symbol.Substring(0,3);
                ClientLimitContractBMFInfo contractLimit = bmfLimit.ContractLimit.Find(x => x.Contrato == contract && x.Sentido == sentido);
                if (null == contractLimit)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_CONTRACT_EXCEEDS, ErrorMessages.ERR_BMF_LIMIT_CONTRACT_EXCEEDS);
                    return ret;
                }
                
                // Contrato pai
                idClienteParametroBmf = contractLimit.IdClienteParametroBMF;
                qtdContrato = contractLimit.QuantidadeDisponivel;
                
                //int QuantidadeContrato = contractLimit.QuantidadeDisponivel;
                if (originalQty > contractLimit.QuantidadeMaximaOferta)
                {
                    string msg = string.Format(ErrorMessages.ERR_BMF_LIMIT_QTD_EXCEEDS, contractLimit.QuantidadeMaximaOferta);
                    ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_QTD_EXCEEDS, msg);
                    return ret;
                }

                if (orderQty > contractLimit.QuantidadeDisponivel)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_OPERATING_EXCEEDS_CONTRACT, ErrorMessages.ERR_BMF_LIMIT_OPERATING_EXCEEDS_CONTRACT);
                    return ret;
                }

                if (bmfLimit.InstrumentLimit.Count > 0)
                {
                    ClientLimitInstrumentBMFInfo instLimit = bmfLimit.InstrumentLimit.Find(x => x.Instrumento== symbol && x.Sentido == sentido);
                    if (null != instLimit)
                    {
                        if (originalQty > instLimit.QuantidadeMaximaOferta)
                        {
                            string msg = string.Format(ErrorMessages.ERR_BMF_LIMIT_QTD_EXCEEDS, instLimit.QuantidadeMaximaOferta);
                            ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_QTD_EXCEEDS, msg);
                            return ret;
                        }

                        if (orderQty > instLimit.QtDisponivel)
                        {
                            ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_OPERATING_EXCEEDS_INST, ErrorMessages.ERR_BMF_LIMIT_OPERATING_EXCEEDS_INST);
                            return ret;
                        }
                    }
                    else
                    {
                        ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_INSTRUMENT_NOT_FOUND, ErrorMessages.ERR_BMF_LIMIT_INSTRUMENT_NOT_FOUND);
                        return ret;
                    }
                }
                /*

                if (bmfLimit.InstrumentLimit.Count > 0)
                {

                    // QUANTIDADE DO INSTRUMENTO PARA O MESMO VENCIMENTO.
                    foreach (var vcto in bmfLimit.InstrumentLimit)
                    {
                        if (vcto.Instrumento == symbol)
                        {
                            if (orderQty > vcto.QuantidadeMaximaOferta)
                            {
                                string msg = string.Format(ErrorMessages.ERR_BMF_LIMIT_QTD_EXCEEDS, vcto.QuantidadeMaximaOferta);
                                ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_QTD_EXCEEDS, msg);
                                return ret;
                            }

                            if (vcto.IdClienteParametroBMF == idClienteParametroBmf)
                            {

                                qtdInstrumento += vcto.QtDisponivel;
                                instrument = vcto.Instrumento;
                                stUtilizacaoInstrumento = 'S';
                            }
                        }
                    }

                    if ((qtdContrato == 0) && (qtdInstrumento == 0))
                    {
                        logger.Debug("LIMITE CONTRATO ..........:" + qtdContrato.ToString());
                        logger.Debug("LIMITE INSTRUMENTO ..........:" + qtdInstrumento.ToString());
                        logger.Debug("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");
                        ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_OPERATING_EXCEEDS, ErrorMessages.ERR_BMF_LIMIT_OPERATING_EXCEEDS);
                        return ret;
                    }

                    decimal qtdSolicitada = orderQty;

                    // LIMITE SOMENTE PARA O CONTRATO
                    if ((qtdContrato > 0) && (qtdInstrumento > 0))
                    {
                        logger.Debug("LIMITE CONTRATO .............: " + qtdContrato.ToString());
                        logger.Debug("LIMITE INSTRUMENTO ..........: " + qtdInstrumento.ToString());
                        logger.Debug("QUANTIDADE SOLICITADA .......: " + qtdSolicitada.ToString());

                        if (qtdSolicitada > qtdInstrumento)
                        {
                            logger.Debug("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");
                            ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_OPERATING_EXCEEDS, ErrorMessages.ERR_BMF_LIMIT_OPERATING_EXCEEDS);
                            return ret;
                        }

                    }

                    // LIMITE PARA O CONTRATO E PARA O INSTRUMENTO (VENCIMENTO)
                    if ((qtdContrato > 0) && (qtdInstrumento == 0))
                    {
                        logger.Debug("LIMITE CONTRATO .............: " + qtdContrato.ToString());
                        logger.Debug("LIMITE INSTRUMENTO ..........: " + qtdInstrumento.ToString());
                        logger.Debug("QUANTIDADE SOLICITADA .......: " + qtdSolicitada.ToString());

                        if (stUtilizacaoInstrumento == 'S')
                        {
                            logger.Debug("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");
                            ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_OPERATING_EXCEEDS, ErrorMessages.ERR_BMF_LIMIT_OPERATING_EXCEEDS);
                            return ret;
                        }

                        if (qtdSolicitada > qtdContrato)
                        {
                            logger.Debug("LIMITE OPERACIONAL INSUFICIENTE PARA OPERAR O CONTRATO.");
                            ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_OPERATING_EXCEEDS, ErrorMessages.ERR_BMF_LIMIT_OPERATING_EXCEEDS);
                            return ret;
                        }
                    }
                }
                 */
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na validacao no limite operacional: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }

        public LimitResponse UpdateOperationalLimitBovespa(int account, TipoLimiteEnum tpLimite, decimal vlrAlocado, decimal precoBase, int side, char st)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }

                // Buscar informacoes da regra de limite operacional
                List<OperatingLimitInfo> opLimitLst = null;
                if (!_dicOperatingLimit.TryGetValue(account, out opLimitLst))
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
                        case OrdStatus.CANCELED:
                            vlrAux = (-1) * vlrAlocado;
                            opLimit.ValorAlocado += vlrAux;
                            break;
                        case OrdStatus.NEW:
                            vlrAux = vlrAlocado;
                            opLimit.ValorAlocado += vlrAux;
                            break;
                        case OrdStatus.REPLACED:
                            vlrAux = vlrAlocado;
                            opLimit.ValorAlocado = (vlrAux - opLimit.ValorAlocado) + opLimit.ValorAlocado;
                            break;
                        case OrdStatus.FILLED:
                        case OrdStatus.PARTIALLY_FILLED:
                            vlrAux = (-1) * vlrAlocado;
                            opLimit.ValorAlocado+= vlrAux; // devolvendo a ordem em aberto
                            break;

                    }
                    opLimit.ValorDisponivel = opLimit.ValotTotal - opLimit.ValorAlocado;
                    opLimit.PrecoBase = precoBase;
                    opLimit.ValorMovimento = vlrAux;
                    opLimit.StNatureza = opLimit.ValorMovimento < 0 ? "C" : "D"; // Rever a logica
                    this._addBvspMvto(opLimit);
                //}
                    // If filled or partially filled, then calculate the dual limit
                    if (st.Equals(OrdStatus.FILLED) || st.Equals(OrdStatus.PARTIALLY_FILLED))
                    {
                        // Debita o limite de compra / venda
                        opLimit.ValorAlocado +=vlrAlocado;
                        opLimit.ValorDisponivel = opLimit.ValotTotal - opLimit.ValorAlocado;
                        opLimit.PrecoBase = precoBase;
                        opLimit.ValorMovimento = vlrAux;
                        opLimit.StNatureza = opLimit.ValorMovimento < 0 ? "C" : "D"; // Rever a logica
                        this._addBvspMvto(opLimit);
                        
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
                    }
                //}
                //else
                //{
                    // Filled or partially filled
                //    opLimit.ValorAlocado = 
                    
                    // Devolver a ordem em aberto

                    // Debita a compra

                    // Credita a venda

                //}

                //OperatingLimitInfo opLimitDual = opLimitLst.Find(x => x.TipoLimite == _dicDualTipoLimite[tpLimite]);
                //if (null == opLimitDual)
                //{
                //    string msg = string.Format(ErrorMessages.ERR_OPERATING_LIMIT_NOT_FOUND, _dicDualTipoLimite[tpLimite].ToString());
                //    ret = _fillResponse(ErrorMessages.ERR_CODE_OPERATING_LIMIT_NOT_FOUND, msg);
                //    return ret;
                //}
                
                //// Tratamento de calculo dos valores (contraparte)
                //switch (st)
                //{
                //    case OrdStatus.NEW:
                //    case OrdStatus.CANCELED:
                //        opLimitDual.ValorAlocado += ((-1) * vlrAux);
                //        break;
                //    case OrdStatus.REPLACED:
                //        opLimitDual.ValorAlocado = (((-1) * vlrAux) - opLimitDual.ValorAlocado) + (opLimitDual.ValorAlocado);
                //        break;
                //}

                //opLimitDual.ValorDisponivel = opLimitDual.ValotTotal - opLimitDual.ValorAlocado;
                //opLimitDual.PrecoBase = precoBase;
                //opLimitDual.ValorMovimento = (-1) * vlrAux;
                //opLimitDual.StNatureza = opLimitDual.ValorMovimento < 0 ? "C" : "D"; // Rever a logica
                // this._addBvspMvto(opLimit);
                //this._addBvspMvto(opLimitDual);
                
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao dos valores dos limites operacionais BOVESPA: "  + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }

        public LimitResponse UpdateOperationalLimitBMF(int account, TipoLimiteEnum tpLimite, string symbol, int orderQty, int newOrderQty, string sentido)
        {
            LimitResponse ret = _fillResponse(ErrorMessages.OK, ErrorMessages.MSG_OK);
            try
            {
                if (!_isLoaded)
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_DATA_NOT_LOADED, ErrorMessages.ERR_DATA_NOT_LOADED);
                    return ret;
                }

                // Buscar informacoes da regra de limite operacional
                ClientLimitBMFInfo cliLimitBmfInfo = null;
                if (!_dicClientLimitBMF.TryGetValue(account, out cliLimitBmfInfo))
                {
                    ret = _fillResponse(ErrorMessages.ERR_CODE_BMF_LIMIT_NOT_FOUND, ErrorMessages.ERR_BMF_LIMIT_NOT_FOUND);
                    return ret;
                }
                string contract = symbol.Substring(0, 3);
                
                // Contrato Base
                ClientLimitContractBMFInfo cliContractBMF = cliLimitBmfInfo.ContractLimit.Find(x => x.Contrato.Equals(contract) && x.Sentido.Equals(sentido, StringComparison.InvariantCultureIgnoreCase));
                if (null != cliContractBMF)
                {
                    // Em teoria, nao deve chegar neste caso
                    if (cliContractBMF.QuantidadeDisponivel - orderQty < 0)
                        cliContractBMF.QuantidadeDisponivel = 0;
                    else
                        cliContractBMF.QuantidadeDisponivel = (cliContractBMF.QuantidadeDisponivel - orderQty) - newOrderQty;
                    cliContractBMF.DataMovimento = DateTime.Now;
                }
                else
                {

                }

                // Verificar limite instrumento
                ClientLimitInstrumentBMFInfo cliInstrumentBMF = cliLimitBmfInfo.InstrumentLimit.Find(x => x.Instrumento.Equals(symbol) && x.Sentido.Equals(sentido, StringComparison.InvariantCultureIgnoreCase));
                if (null != cliInstrumentBMF)
                {
                    // Em teoria, nao deve chegar neste caso
                    if (cliInstrumentBMF.QtDisponivel - orderQty < 0)
                        cliInstrumentBMF.QtDisponivel = 0;
                    else
                        cliInstrumentBMF.QtDisponivel = (cliInstrumentBMF.QtDisponivel - orderQty) - newOrderQty;
                    cliInstrumentBMF.dtMovimento = DateTime.Now;
                }

                ClientLimitBMFInfo item = new ClientLimitBMFInfo();
                item.Account = account;
                item.ContractLimit.Add(cliContractBMF);
                item.InstrumentLimit.Add(cliInstrumentBMF);
                this._addBmfMvto(item);
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao dos valores dos limites operacionais BMF: " + ex.Message, ex);
                ret = _fillResponse(ErrorMessages.ERROR, ex.Message, ex.StackTrace);
                return ret;
            }
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Memory Management
        /*
        /// <summary>
        /// Atualizar informacoes da collection de SymbolInfo
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public SymbolUpdateResponse UpdateSymbol(string key, SymbolInfo item, DbOperation type)
        {
            
            try
            {
                SymbolUpdateResponse ret = new SymbolUpdateResponse();
                if (!_isLoaded)
                {
                    logger.Info("Dados não carregados");
                    ret.DescricaoErro = "Dados não carregados";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }

                bool exists = _dicSymbols.ContainsKey(key);
                switch (type)
                {
                    case DbOperation.Add:
                        if (!exists)
                        {
                            lock (_dicSymbols)
                            {
                                _dicSymbols.Add(key, item);
                            }
                        }
                        break;
                    case DbOperation.Delete:
                        if (exists)
                        {
                            lock (_dicSymbols)
                            {
                                _dicSymbols.Remove(key);
                            }
                        }
                        break;
                    case DbOperation.Update:
                        if (exists)
                        {
                            lock (_dicSymbols[key])
                            {
                                _dicSymbols[key] = item;
                            }
                        }
                        break;
                }
                logger.Info("DicSymbols: " + _dicSymbols.Count);
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao de cadastro de papel: " + ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Efetua a atualizao, insercao dos parametros referente ao clienteid passado no parametro
        /// </summary>
        /// <returns></returns>
        public ClientParameterUpdateResponse UpdateClientParameter(int clientid, RiscoParametrosEnum keyParam, ParameterPermissionClientInfo item, DbOperation type)
        {
            try
            {
                ClientParameterUpdateResponse ret = new ClientParameterUpdateResponse();
                if (!_isLoaded)
                {
                    logger.Info("Dados não carregados");
                    ret.DescricaoErro = "Dados não carregados";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }

                ClientParameterPermissionInfo aux = null;
                if (!_dicParameters.TryGetValue(clientid, out aux))
                {
                    ret.DescricaoErro = "Parametro para cliente: " + clientid + " não encontrado";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }
                switch (type)
                {
                    case DbOperation.Add:
                        lock (_dicParameters[clientid])
                        {
                            ParameterPermissionClientInfo auxParam = _dicParameters[clientid].Parametros.FirstOrDefault(x => x.Parametro == keyParam);
                            if (null == auxParam)
                                _dicParameters[clientid].Parametros.Add(item);
                        }
                        break;
                    case DbOperation.Delete:
                        lock (_dicParameters[clientid])
                        {
                            ParameterPermissionClientInfo auxParam = _dicParameters[clientid].Parametros.FirstOrDefault(x => x.Parametro == keyParam);
                            if (null != auxParam)
                                _dicParameters[clientid].Parametros.Remove(auxParam);
                        }
                        break;
                    case DbOperation.Update:
                        lock (_dicParameters[clientid])
                        {
                            ParameterPermissionClientInfo auxParam = _dicParameters[clientid].Parametros.FirstOrDefault(x => x.Parametro == keyParam);
                            if (null != auxParam)
                                _dicParameters[clientid].Parametros.Remove(auxParam);
                            _dicParameters[clientid].Parametros.Add(item);
                        }
                        break;
                }
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao de parametros de cliente: " + ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Efetua a atualizao, insercao das permissoes referente ao clienteid passado no parametro
        /// </summary>
        /// <returns></returns>
        public ClientPermissionUpdateResponse UpdateClientPermission(int clientid, RiscoPermissoesEnum keyParam, ParameterPermissionClientInfo item, DbOperation type)
        {
            try
            {
                ClientPermissionUpdateResponse ret = new ClientPermissionUpdateResponse();
                if (!_isLoaded)
                {
                    logger.Info("Dados não carregados");
                    ret.DescricaoErro = "Dados não carregados";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }

                ClientParameterPermissionInfo aux = null;
                if (!_dicParameters.TryGetValue(clientid, out aux))
                {
                    
                    ret.DescricaoErro = "Permissao não encontrada para cliente: " + clientid;
                    logger.Info(ret.DescricaoErro);
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }

                switch (type)
                {
                    case DbOperation.Add:
                        lock (_dicParameters[clientid])
                        {
                            ParameterPermissionClientInfo auxParam = _dicParameters[clientid].Parametros.FirstOrDefault(x => x.Permissao == keyParam);
                            if (null == auxParam)
                                _dicParameters[clientid].Parametros.Add(item);
                        }
                        break;
                    case DbOperation.Delete:
                        lock (_dicParameters[clientid])
                        {
                            ParameterPermissionClientInfo auxParam = _dicParameters[clientid].Parametros.FirstOrDefault(x => x.Permissao == keyParam);
                            if (null != auxParam)
                                _dicParameters[clientid].Parametros.Remove(auxParam);
                        }
                        break;
                    case DbOperation.Update:
                        lock (_dicParameters[clientid])
                        {
                            ParameterPermissionClientInfo auxParam = _dicParameters[clientid].Parametros.FirstOrDefault(x => x.Permissao == keyParam);
                            if (null != auxParam)
                                _dicParameters[clientid].Parametros.Remove(auxParam);
                            _dicParameters[clientid].Parametros.Add(item);
                        }
                        break;
                }
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao de permissoes de cliente: " + ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Atualizacao de parametros fat finger
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public FatFingerUpdateResponse UpdateFatFinger(int key, FatFingerInfo item, DbOperation type)
        {
            try
            {
                FatFingerUpdateResponse ret = new FatFingerUpdateResponse();
                if (!_isLoaded)
                {
                    logger.Info("Dados não carregados");
                    ret.DescricaoErro = "Dados não carregados";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }

                bool exists = _dicFatFinger.ContainsKey(key);
                switch (type)
                {
                    case DbOperation.Add:
                        if (!exists)
                        {
                            lock (_dicFatFinger)
                            {
                                _dicFatFinger.Add(key, item);
                            }
                        }
                        break;
                    case DbOperation.Delete:
                        if (exists)
                        {
                            lock (_dicFatFinger)
                            {
                                _dicFatFinger.Remove(key);
                            }
                        }
                        break;
                    case DbOperation.Update:
                        if (exists)
                        {
                            lock (_dicFatFinger[key])
                            {
                                _dicFatFinger[key] = item;
                            }
                        }
                        break;
                }
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao dos parametro fatfinger: " + ex.Message, ex);
                throw ex;
            }
        }
        /// <summary>
        /// Atualizacao de parametros de Exposicao de Risco
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public RiskExposureClientUpdateResponse UpdateRiskExposureClient(int key, RiskExposureClientInfo item, DbOperation type)
        {
            try
            {
                RiskExposureClientUpdateResponse ret = new RiskExposureClientUpdateResponse();
                if (!_isLoaded)
                {
                    logger.Info("Dados não carregados");
                    ret.DescricaoErro = "Dados não carregados";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }

                bool exists = _dicRiskExposureClient.ContainsKey(key);
                switch (type)
                {
                    case DbOperation.Add:
                        if (!exists)
                        {
                            lock (_dicRiskExposureClient)
                            {
                                _dicRiskExposureClient.Add(key, item);
                            }
                        }
                        break;
                    case DbOperation.Delete:
                        if (exists)
                        {
                            lock (_dicRiskExposureClient)
                            {
                                _dicRiskExposureClient.Remove(key);
                            }
                        }
                        break;
                    case DbOperation.Update:
                        if (exists)
                        {
                            lock (_dicRiskExposureClient[key])
                            {
                                _dicRiskExposureClient[key] = item;
                            }
                        }
                        break;
                }
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao dos parametro RiskExposureClient: " + ex.Message, ex);
                throw ex;
            }
        }

        public BlockedSymbolGlobalUpdateResponse UpdateBlockedSymbolGlobal(SymbolKey key, BlockedInstrumentInfo item, DbOperation type)
        {
            try
            {
                BlockedSymbolGlobalUpdateResponse ret = new BlockedSymbolGlobalUpdateResponse();
                if (!_isLoaded)
                {
                    logger.Info("Dados não carregados");
                    ret.DescricaoErro = "Dados não carregados";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }

                bool exists = _dicBlockedSymbolGlobal.ContainsKey(key);
                switch (type)
                {
                    case DbOperation.Add:
                        if (!exists)
                        {
                            lock (_dicBlockedSymbolGlobal)
                            {
                                _dicBlockedSymbolGlobal.Add(key, item);
                            }
                        }
                        break;
                    case DbOperation.Delete:
                        if (exists)
                        {
                            lock (_dicBlockedSymbolGlobal)
                            {
                                _dicBlockedSymbolGlobal.Remove(key);
                            }
                        }
                        break;
                    case DbOperation.Update:
                        if (exists)
                        {
                            lock (_dicBlockedSymbolGlobal[key])
                            {
                                _dicBlockedSymbolGlobal[key] = item;
                            }
                        }
                        break;
                }
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao dos parametros BlockedSymbolsGlobal: " + ex.Message, ex);
                throw ex;
            }
        }

        public BlockedSymbolGroupClientUpdateResponse UpdateBlockedSymbolGroupClient(SymbolKey key, BlockedInstrumentInfo item, DbOperation type)
        {
            try
            {
                BlockedSymbolGroupClientUpdateResponse ret = new BlockedSymbolGroupClientUpdateResponse();
                if (!_isLoaded)
                {
                    logger.Info("Dados não carregados");
                    ret.DescricaoErro = "Dados não carregados";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }

                bool exists = _dicBlockedSymbolGroupClient.ContainsKey(key);
                switch (type)
                {
                    case DbOperation.Add:
                        if (!exists)
                        {
                            lock (_dicBlockedSymbolGroupClient)
                            {
                                _dicBlockedSymbolGroupClient.Add(key, item);
                            }
                        }
                        break;
                    case DbOperation.Delete:
                        if (exists)
                        {
                            lock (_dicBlockedSymbolGroupClient)
                            {
                                _dicBlockedSymbolGroupClient.Remove(key);
                            }
                        }
                        break;
                    case DbOperation.Update:
                        if (exists)
                        {
                            lock (_dicBlockedSymbolGroupClient[key])
                            {
                                _dicBlockedSymbolGroupClient[key] = item;
                            }
                        }
                        break;
                }
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao dos parametros BlockedSymbol Group Client: " + ex.Message, ex);
                throw ex;
            }
        }

        public BlockedSymbolClientUpdateResponse UpdateBlockedSymbolClient(SymbolKey key, BlockedInstrumentInfo item, DbOperation type)
        {
            try
            {
                BlockedSymbolClientUpdateResponse ret = new BlockedSymbolClientUpdateResponse();
                if (!_isLoaded)
                {
                    logger.Info("Dados não carregados");
                    ret.DescricaoErro = "Dados não carregados";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }
                bool exists = _dicBlockedSymbolClient.ContainsKey(key);
                switch (type)
                {
                    case DbOperation.Add:
                        if (!exists)
                        {
                            lock (_dicBlockedSymbolClient)
                            {
                                _dicBlockedSymbolClient.Add(key, item);
                            }
                        }
                        break;
                    case DbOperation.Delete:
                        if (exists)
                        {
                            lock (_dicBlockedSymbolClient)
                            {
                                _dicBlockedSymbolClient.Remove(key);
                            }
                        }
                        break;
                    case DbOperation.Update:
                        if (exists)
                        {
                            lock (_dicBlockedSymbolGroupClient[key])
                            {
                                _dicBlockedSymbolGroupClient[key] = item;
                            }
                        }
                        break;
                }
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao dos parametros BlockedSymbol Group Client: " + ex.Message, ex);
                throw ex;
            }
        }

        public OptionSeriesBlockedUpdateResponse UpdateOptionSeriesBlocked(string key, OptionBlockInfo item, DbOperation type)
        {
            try
            {
                OptionSeriesBlockedUpdateResponse ret = new OptionSeriesBlockedUpdateResponse();
                if (!_isLoaded)
                {
                    logger.Info("Dados não carregados");
                    ret.DescricaoErro = "Dados não carregados";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }

                bool exists = _dicOptionSeriesBlocked.ContainsKey(key);
                switch (type)
                {
                    case DbOperation.Add:
                        if (!exists)
                        {
                            lock (_dicOptionSeriesBlocked)
                            {
                                _dicOptionSeriesBlocked.Add(key, item);
                            }
                        }
                        break;
                    case DbOperation.Delete:
                        if (exists)
                        {
                            lock (_dicOptionSeriesBlocked)
                            {
                                _dicOptionSeriesBlocked.Remove(key);
                            }
                        }
                        break;
                    case DbOperation.Update:
                        if (exists)
                        {
                            lock (_dicOptionSeriesBlocked[key])
                            {
                                _dicOptionSeriesBlocked[key] = item;
                            }
                        }
                        break;
                }
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao dos parametros Optiion Series Blocked: " + ex.Message, ex);
                throw ex;
            }
        }


        /// <summary>
        /// Atualizacao do OperatingLimit dos clientes. Somente atualiza / exclui / adiciona tipos de limite,
        /// nao adicionando um novo registro. (Basicamente a atualizacao estah referenciando key2 e não key)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="key2"></param>
        /// <param name="item"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public OperatingLimitUpdateResponse UpdateOperatingLimit(int clientid, int key2, OperatingLimitInfo item, DbOperation type)
        {
            try
            {
                OperatingLimitUpdateResponse ret = new OperatingLimitUpdateResponse();
                if (!_isLoaded)
                {
                    logger.Info("Dados não carregados");
                    ret.DescricaoErro = "Dados não carregados";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }

                List<OperatingLimitInfo> listOperating = null;
                if (!_dicOperatingLimit.TryGetValue(clientid, out listOperating))
                {
                    ret.DescricaoErro = "Limite operacional não encontrado para cliente: " + clientid;
                    logger.Info(ret.DescricaoErro);
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }
                switch (type)
                {
                    case DbOperation.Add:
                        lock (_dicOperatingLimit[clientid])
                        {
                            OperatingLimitInfo aux = _dicOperatingLimit[clientid].FirstOrDefault(x => x.CodigoParametroCliente == key2);
                            if (null == aux)
                                _dicOperatingLimit[clientid].Add(item);
                        }
                        break;
                    case DbOperation.Delete:
                        lock (_dicOperatingLimit[clientid])
                        {
                            OperatingLimitInfo aux = _dicOperatingLimit[clientid].FirstOrDefault(x => x.CodigoParametroCliente == key2);
                            if (null != aux)
                                _dicOperatingLimit[clientid].Remove(aux);
                        }
                        break;
                    case DbOperation.Update:
                        lock (_dicOperatingLimit[clientid])
                        {
                            // Por via das duvidas, excluir e adicionar novamente
                            OperatingLimitInfo aux = _dicOperatingLimit[clientid].FirstOrDefault(x => x.CodigoParametroCliente == key2);
                            if (null != aux)
                                _dicOperatingLimit[clientid].Remove(aux);
                            _dicOperatingLimit[clientid].Add(item);
                        }
                        break;
                }
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao dos parametros Optiion Series Blocked: " + ex.Message, ex);
                throw ex;
            }
        }

        public ClientLimitContractUpdateResponse UpdateClientLimitContractBMF(int clientid, int idClienteParametroBMF, ClientLimitContractBMFInfo item, DbOperation type)
        {
            try
            {
                ClientLimitContractUpdateResponse ret = new ClientLimitContractUpdateResponse();
                if (!_isLoaded)
                {
                    logger.Info("Dados não carregados");
                    ret.DescricaoErro = "Dados não carregados";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }

                ClientLimitBMFInfo aux = null;
                if (!_dicClientLimitBMF.TryGetValue(clientid, out aux))
                {
                    ret.DescricaoErro = "Limite contrato bmf não encontrado para cliente: " + clientid;
                    logger.Info(ret.DescricaoErro);
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }
                switch (type)
                {
                    case DbOperation.Add:
                        lock (_dicClientLimitBMF[clientid])
                        {
                            ClientLimitContractBMFInfo auxContract = _dicClientLimitBMF[clientid].ContractLimit.FirstOrDefault(x => x.IdClienteParametroBMF == idClienteParametroBMF);
                            if (null == auxContract)
                                _dicClientLimitBMF[clientid].ContractLimit.Add(item);
                        }
                        break;
                    case DbOperation.Delete:
                        lock (_dicClientLimitBMF[clientid])
                        {
                            ClientLimitContractBMFInfo auxContract = _dicClientLimitBMF[clientid].ContractLimit.FirstOrDefault(x => x.IdClienteParametroBMF == idClienteParametroBMF);
                            if (null != auxContract)
                                _dicClientLimitBMF[clientid].ContractLimit.Remove(auxContract);
                        }
                        break;
                    case DbOperation.Update:
                        lock (_dicClientLimitBMF[clientid])
                        {
                            ClientLimitContractBMFInfo auxContract = _dicClientLimitBMF[clientid].ContractLimit.FirstOrDefault(x => x.IdClienteParametroBMF == idClienteParametroBMF);
                            if (null != auxContract)
                                _dicClientLimitBMF[clientid].ContractLimit.Remove(auxContract);
                            _dicClientLimitBMF[clientid].ContractLimit.Add(item);
                        }
                        break;
                }
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao dos parametros de ContractBMF: " + ex.Message, ex);
                throw ex;
            }
        }


        public ClientLimitInstrumentUpdateResponse UpdateClientLimitInstrumentBMF(int clientid, int idClienteParametroInstrumento, ClientLimitInstrumentBMFInfo item, DbOperation type)
        {
            try
            {
                ClientLimitInstrumentUpdateResponse ret = new ClientLimitInstrumentUpdateResponse();
                if (!_isLoaded)
                {
                    logger.Info("Dados não carregados");
                    ret.DescricaoErro = "Dados não carregados";
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }
                ClientLimitBMFInfo aux = null;
                if (!_dicClientLimitBMF.TryGetValue(clientid, out aux))
                {
                    ret.DescricaoErro = "Limite instrumento bmf não encontrado para cliente: " + clientid;
                    logger.Info(ret.DescricaoErro);
                    ret.StatusResponse = LimitMessages.ERRO;
                    return ret;
                }
                switch (type)
                {
                    case DbOperation.Add:
                        lock (_dicClientLimitBMF[clientid])
                        {
                            ClientLimitInstrumentBMFInfo auxInst = _dicClientLimitBMF[clientid].InstrumentLimit.FirstOrDefault(x => x.IdClienteParametroInstrumento == idClienteParametroInstrumento);
                            if (null == auxInst)
                                _dicClientLimitBMF[clientid].InstrumentLimit.Add(item);
                        }
                        break;
                    case DbOperation.Delete:
                        lock (_dicClientLimitBMF[clientid])
                        {
                            ClientLimitInstrumentBMFInfo auxInst = _dicClientLimitBMF[clientid].InstrumentLimit.FirstOrDefault(x => x.IdClienteParametroInstrumento == idClienteParametroInstrumento);
                            if (null != auxInst)
                                _dicClientLimitBMF[clientid].InstrumentLimit.Remove(auxInst);
                        }
                        break;
                    case DbOperation.Update:
                        lock (_dicClientLimitBMF[clientid])
                        {
                            ClientLimitInstrumentBMFInfo auxInst = _dicClientLimitBMF[clientid].InstrumentLimit.FirstOrDefault(x => x.IdClienteParametroInstrumento == idClienteParametroInstrumento);
                            if (null != auxInst)
                                _dicClientLimitBMF[clientid].InstrumentLimit.Remove(auxInst);
                            _dicClientLimitBMF[clientid].InstrumentLimit.Add(item);
                        }
                        break;
                }
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na atualizacao dos parametros de Instrument BMF: " + ex.Message, ex);
                throw ex;
            }
        }
        */
        public ReloadLimitsResponse ReloadLimits(bool reloadSecurityList)
        {
            try
            {
                ReloadLimitsResponse ret = new ReloadLimitsResponse();
                logger.Info("ReloadLimits(): desalocando esturuturas de controle de Limite");
                this.UnloadData(reloadSecurityList);
                logger.Info("ReloadLimits(): atualizando esturuturas de controle de Limite");
                this.LoadData(reloadSecurityList);
                logger.Info("ReloadLimits(): carga finalizada");
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na recarga dos parâmetros de Limite: " + ex.Message, ex);
                throw ex;
            }
        }

        public ReloadClientLimitResponse ReloadClientLimits(int codCliente, bool deleteOnly = false)
        {
            try
            {
                ReloadClientLimitResponse ret = new ReloadClientLimitResponse();
                logger.Info("ReloadClientLimits() - INICIO");
                
                this.UnloadClientData(codCliente);
                if (false == deleteOnly)
                    this.LoadClientData(codCliente);
                else
                    logger.Info("ReloadClientLimits() - SOMENTE EXCLUINDO o cliente: " + codCliente);

                logger.Info("ReloadClientLimits() - FIM");
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na recarga dos parâmetros de Limite: " + ex.Message, ex);
                throw ex;
            }
        }

        public ReloadClientLimitResponse ReloadClientLimitsBMF(int codCliente, bool deleteOnly = false)
        {
            try
            {
                ReloadClientLimitResponse ret = new ReloadClientLimitResponse();
                logger.Info("ReloadClientLimits() - INICIO");

                this.UnloadClientData(codCliente);
                if (false == deleteOnly)
                    this.LoadClientData(codCliente);
                else
                    logger.Info("ReloadClientLimits() - SOMENTE EXCLUINDO o cliente: " + codCliente);

                logger.Info("ReloadClientLimits() - FIM");
                ret.StatusResponse = LimitMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na recarga dos parâmetros de Limite: " + ex.Message, ex);
                throw ex;
            }
        }


        #endregion

    }
}
