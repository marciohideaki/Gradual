using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading;

using log4net;
using QuickFix;
using QuickFix.Fields;


// Cortex Servidor Fix
// using Cortex.OMS.ServidorFIX;
using Cortex.OMS.ServidorFIXAdm.Lib;
using Cortex.OMS.ServidorFIXAdm.Lib.Dados;
using Cortex.OMS.FixUtilities.Lib;

using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.Core.OMS.FixServerLowLatency.Regras;
using Gradual.Core.OMS.FixServerLowLatency.Database;

using Gradual.Core.OMS.LimiteManager;
// using QuickFix.FIX44;

using System.Text.RegularExpressions;
using Gradual.Core.OMS.LimiteManager.Streamer;
using Gradual.Core.OMS.LimiteManager.Lib.Dados;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using Gradual.Core.OMS.FixServerLowLatency.Util;
using Gradual.Core.OMS.DropCopy.Lib.Dados;


namespace Gradual.Core.OMS.FixServerLowLatency.Rede
{
    public class FixAcceptor : QuickFix.MessageCracker, QuickFix.IApplication
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        FixConfig _config;
        List<FixSessionItem> _sessionsFix;
        List<FixInitiator> _lstInitiator;
        // List<FixDropCopy> _lstDropCopy;
        FixDropCopy _fixDropCopy;
        Dictionary<SessionID, SessionAcceptor> _dicSessionsFix;
        bool _finalizarSinalizado;
        //bool _bConectado;
        ThreadedSocketAcceptor _socketAcceptor;
        bool _running = false;
        Dictionary<int, ClientMnemonicInfo> _dicMnemonic;
        #endregion

        #region Properties
        //public bool Conectado
        //{
        //    get
        //    {
        //        return _bConectado && !_finalizarSinalizado;
        //    }
        //}

        #endregion
        public FixAcceptor(List<FixSessionItem>  lstConfig, List<FixInitiator> lstFixInitiator, FixDropCopy fixDc)
        {
            _sessionsFix = lstConfig;
            _socketAcceptor = null;
            _finalizarSinalizado = false;
            //_bConectado = false;
            _dicSessionsFix = new Dictionary<SessionID, SessionAcceptor>();
            _dicMnemonic = new Dictionary<int, ClientMnemonicInfo>();
            _lstInitiator = lstFixInitiator;
            _fixDropCopy = fixDc;
            //_lstDropCopy = listDropCopy;
        }

        #region Start/Stop Controls
        public void Start()
        {
            try
            {
                logger.Info("FixAcceptor Start(): Iniciando ThreadedSocketAcceptor....");
                _running = true;


                logger.Info("Iniciando controle de limites, carregando informações (LoadData)");
                LimiteManager.LimitControl.GetInstance().LoadData();
                StreamerManager.GetInstance().Start();
                logger.Info("Fim controle de limites, fim da carga de informações");

                // Buscar Application Path para compor diretorios de dicionario e afins(Substring(6) para retirar 'file:\')
                string path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(FixAcceptor)).CodeBase).Substring(6);
                logger.Info("Application Path: " + path);               
                _config = GerenciadorConfig.ReceberConfig<FixConfig>();

                

                // Cria dicionario da configuracao 
                Dictionary mainDic = new Dictionary();

                if (_config.SocketAcceptPort > 0)
                    mainDic.SetLong("SocketAcceptPort", _config.SocketAcceptPort);
                
                mainDic.SetLong("HeartBtInt", _config.HeartBtInt);
                mainDic.SetLong("ReconnectInterval", _config.ReconnectInterval);
                mainDic.SetString("FileStorePath", _config.FileStorePath);
                logger.Debug("FileLogPath: " + _config.FileLogPath);
                mainDic.SetString("FileLogPath", _config.FileLogPath);
                logger.Debug("DebugFileLogPath: " + _config.FileLogPath);
                mainDic.SetString("DebugFileLogPath", _config.DebugFileLogPath);
                mainDic.SetString("StartTime", _config.StartTime);
                mainDic.SetString("EndTime", _config.EndTime);
                mainDic.SetString("ConnectionType", "acceptor");
                

                // DbFix db = new DbFix();
            
                // Configure the session settings
                SessionSettings settings = new SessionSettings();

                settings.Set(mainDic);

                foreach (FixSessionItem info in _sessionsFix)
                {
                    Dictionary sessDic = new Dictionary();
                    string strdictionary = path + Path.DirectorySeparatorChar + info.DataDictionary;

                    sessDic.SetString("DataDictionary", strdictionary);
                    sessDic.SetBool("UseDataDictionary", info.UseDataDictionary);
                    sessDic.SetBool("ResetOnLogon", info.ResetSeqNum);
                    sessDic.SetBool("PersistMessages", info.PersistMessages);
                    if (!string.IsNullOrEmpty(info.StartTime))
                        sessDic.SetString("StartTime", info.StartTime);
                    if (!string.IsNullOrEmpty(info.EndTime))
                        sessDic.SetString("EndTime", info.EndTime);

                    logger.InfoFormat("Criando sessao S:[{0}] SsID:[{1}] T:[{2}] TsID:[{3}] UseDic:[{4}] Dic:[{5}] Rst:[{6}] Pers:[{7}] Begstr:[{8}] FinancialLimit:[{9}] Port:[{10}] IdClient[{11}]",
                        info.SenderCompID,
                        info.SenderSubID,
                        info.TargetCompID,
                        info.TargetSubID,
                        info.UseDataDictionary,
                        strdictionary,
                        info.ResetSeqNum,
                        info.PersistMessages,
                        info.BeginString,
                        info.FinancialLimit,
                        info.Operador, 
                        info.IdCliente
                        );

                    // Adicionar no dicionario para validacao de logon
                    // Atribui respectivo Initiator para cada sessao
                    SessionAcceptor ssAcceptor = new SessionAcceptor();
                    FixInitiator aux = null;
                    foreach (FixInitiator init in _lstInitiator)
                    {
                        if (init.Canal == info.Operador)
                        {
                            aux = init;
                            break;
                        }
                    }

                    ssAcceptor.Config = info;
                    ssAcceptor.Initiator = aux;
                    ssAcceptor.DropCopy = _fixDropCopy;
                    ssAcceptor.ParentAcceptor = this;
                    ssAcceptor.CalcularLimite = info.FinancialLimit;
                    // Efetuar carga dos mnemonicos
                    if (info.ParseAccount)
                    {
                        lock (_dicMnemonic)
                        {
                            if (_dicMnemonic.Count == 0)
                            {
                                DbFix db = new DbFix();
                                _dicMnemonic = db.BuscarMnemonicoCliente();
                            }
                        }
                    }

                    ssAcceptor.Start();

                    SessionID session;

                    if (string.IsNullOrEmpty(info.SenderSubID) || string.IsNullOrEmpty(info.TargetSubID))
                        session = new SessionID(info.BeginString, info.TargetCompID, info.SenderCompID);
                    else
                        session = new SessionID(info.BeginString, info.TargetCompID, info.TargetSubID, info.SenderCompID, info.SenderSubID);

                    _dicSessionsFix.Add(session, ssAcceptor);
                    
                    // Adicionar a sessao no initiator, para conseguir "referencia cruzada"
                    // Verificar se nao ocorre erros
                    if (null != aux)
                    {
                        ssAcceptor.Initiator.AddSessionAcceptor(session, ssAcceptor);
                    }
                    settings.Set(session, sessDic);
                }

                FileStoreFactory store = new FileStoreFactory(settings);
                //MemoryStoreFactory store = new MemoryStoreFactory();

                FileLogFactory logs = new FileLogFactory(settings);
                IMessageFactory msgs = new DefaultMessageFactory();

                logger.Info("Iniciando ThreadedSocketAcceptor...");
                _socketAcceptor = new ThreadedSocketAcceptor(this, store, settings, logs, msgs);
                _socketAcceptor.Start();
                logger.Info("Start(): ThreadedSocketAcceptor em execucao....");
                logger.Info("FixAcceptor Start(): Fim de Inicializacao do ThreadedSocketAcceptor....");
                // db = null;
            }

            catch (Exception ex)
            {
                logger.Error("Erro ao inicializar Fix Acceptor: " + ex.Message, ex);
                throw ex;
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("Finalizando ThreadedSocketAcceptor");
                _running = false;
                _finalizarSinalizado = true;
                try
                {
                    if (_socketAcceptor != null)
                    {
                        _socketAcceptor.Stop();
                        _socketAcceptor = null;
                    }

                    foreach (KeyValuePair<SessionID, SessionAcceptor> pair in _dicSessionsFix)
                    {
                        pair.Value.Stop();
                        pair.Value.Initiator = null;
                        pair.Value.Config = null;
                    }
                    _dicSessionsFix.Clear();
                    _dicSessionsFix = null;

                    logger.Info("LimitControl - Efetuando 'UnloadData'");
                    LimiteManager.LimitControl.GetInstance().UnloadData();
                    StreamerManager.GetInstance().Stop();

                }
                catch (Exception ex)
                {
                    logger.Error("Erro em Finalizar SocketAcceptor():" + ex.Message, ex);
                }
                finally
                {
                }
                logger.Info("*** SocketAcceptor finalizado ***");
            }
            catch (Exception ex)
            {
                logger.Error("Erro ao parar Fix Acceptor", ex);
            }
        }
        #endregion
        
        
        #region QuickFix Methods (signs)

        /// <summary>
        /// every inbound admin level message will pass through this method,
        /// such as heartbeats, logons, and logouts.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sessionID"></param>
        public void FromAdmin(QuickFix.Message message, SessionID sessionID)
        {
            string errormsg = "Error: ";
            try
            {
                string msgType = message.Header.GetField(Tags.MsgType);
                if (msgType == MsgType.LOGON)
                {

                    if (sessionID.BeginString == QuickFix.FixValues.BeginString.FIX42)
                    {
                        QuickFix.FIX42.Logon logonMsg = message as QuickFix.FIX42.Logon;
                        if (logonMsg != null)
                        {
                            // Validates sender & target compids
                            string sndCompID = sessionID.SenderCompID;
                            string tgtCompID = sessionID.TargetCompID;
                            string password = logonMsg.IsSetField(96)? logonMsg.GetString(96): string.Empty;

                            logger.DebugFormat("snd[{0}] tgt[{1}] pwd[{2}]", sndCompID, tgtCompID, password);

                            //if (!string.IsNullOrEmpty(password))
                            //{
                                if (!_validateLogon(sessionID, password))
                                {
                                    logger.Info("Não foi possivel autenticar sessao acceptor");
                                    QuickFix.FIX42.Logout logout = new QuickFix.FIX42.Logout();

                                    // Logout logout = new Logout();
                                    logout.Set(new Text("This 4.2 session was not authorized"));
                                    Session.SendToTarget(logout, sessionID);
                                }
                            //}
                        }
                    }
                    if (sessionID.BeginString == QuickFix.FixValues.BeginString.FIX44)
                    {
                        QuickFix.FIX44.Logon logonMsg = message as QuickFix.FIX44.Logon;
                        if (logonMsg != null)
                        {
                            // Validates sender & target compids
                            string sndCompID = sessionID.SenderCompID;
                            string tgtCompID = sessionID.TargetCompID;
                            string password = logonMsg.IsSetField(96)? logonMsg.GetString(96): string.Empty;

                            logger.DebugFormat("snd[{0}] tgt[{1}] pwd[{2}]", sndCompID, tgtCompID, password);

                            //if (!string.IsNullOrEmpty(password))
                            //{
                                if (!_validateLogon(sessionID, password))
                                {
                                    logger.Info("Não foi possivel autenticar sessao acceptor");
                                    QuickFix.FIX44.Logout logout = new QuickFix.FIX44.Logout();

                                    // Logout logout = new Logout();
                                    logout.Set(new Text("This 4.4 session was not authorized"));
                                    Session.SendToTarget(logout, sessionID);
                                }
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errormsg += ex.Message;
                logger.Error("FromAdmin: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// FromApp - every inbound application level message will pass through this method,
        /// such as orders, executions, secutiry definitions, and market data.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sessionID"></param>
        public void FromApp(QuickFix.Message message, SessionID sessionID)
        {
            //string errormsg = "Error: ";
            //try
            //{
                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                //TOCrack aux = new TOCrack(message, sessionID);
                //lock (_queueMsgToCrack)
                //{
                //    _queueMsgToCrack.Enqueue(aux);
                //    Monitor.Pulse(_queueMsgToCrack);
                //}
                Crack(message, sessionID);
                //sw.Stop();
            //}
            //catch (Exception ex)
            //{
            //    errormsg += ex.Message;
            //    logger.Error("FromApp: " + ex.Message, ex);
            //}
        }

        public void OnCreate(SessionID sessionID)
        {
            logger.Info("Acceptor OnCreate()");
        }

        public void OnLogout(SessionID sessionID)
        {
            logger.Info("FixAcceptor OnLogout(): " + sessionID.ToString());
            this._validateLogout(sessionID.SenderCompID, sessionID.TargetCompID);
            //_bConectado = false;
        }

        public void OnLogon(SessionID sessionID)
        {
            string errormsg = "Invalid logon";
            logger.Info("FixAcceptor OnLogon(): " + sessionID.ToString());

            //_bConectado = true;
            try
            {

            }
            catch (Exception ex)
            {
                //_bConectado = false;
                // Trouble? quick out him
                errormsg += ex.Message;
                //QuickFix.RejectLogon rejLogon = new RejectLogon(errormsg);

                logger.Error("OnLogon: " + ex.Message, ex);
                QuickFix.RejectLogon rejLogon = new QuickFix.RejectLogon(errormsg);
                //rejLogon. = errormsg;

                
            }
        }

        /// <summary>
        /// all outbound admin level messages pass through this callback.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sessionID"></param>
        public void ToAdmin(QuickFix.Message message, SessionID sessionID)
        {
            
        }

        /// <summary>
        /// all outbound application level messages pass through this callback before they are sent. 
        /// If a tag needs to be added to every outgoing message, this is a good place to do that
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sessionID"></param>
        public void ToApp(QuickFix.Message message, SessionID sessionID)
        {
            
        }
        #endregion


        #region QuickFix Messages Methods


        #region Fix 4.4
        /// <summary>
        /// NewOrdemSingle FIX 4.4 version
        /// </summary>
        /// <param name="nos"></param>
        /// <param name="s"></param>
        public void OnMessage(QuickFix.FIX44.NewOrderSingle nos, SessionID s)
        {
            SessionAcceptor ssAcceptor = null;
            try
            {
                TipoLimiteEnum tpLimite = TipoLimiteEnum.INDEFINIDO;
                OrdemInfo ordemInfo = null;
                // Validacao de existencia da sessao acceptor
                if (!_dicSessionsFix.TryGetValue(s, out ssAcceptor))
                {
                    throw new Exception("Session acceptor does not exist");
                }

                // Validar se o account deve ser substituido (mnemonico por client id)
                string strAux = nos.Account.getValue();
                int account;
                if (!ssAcceptor.Config.ParseAccount)
                {
                    if (ssAcceptor.Config.Bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                        account = Convert.ToInt32(strAux.Remove(strAux.Length - 1));
                    else
                        account = Convert.ToInt32(strAux);
                }
                else
                {
                    account = this.GetAccountFromMnemonic(strAux);
                    if (account == 0)
                    {
                        logger.InfoFormat("Account invalido, nao foi possivel efetuar a conversao");
                        this._generateRejectMessage(nos, s, MsgType.NEWORDERSINGLE, null, ssAcceptor, RejectionMessages.INVALID_ACCOUNT);
                        return;
                    }
                    else
                    {
                        // Reparsear a conta
                        int aux = GeneralFunctions.CalcularCodigoCliente(227, account);
                        nos.SetField(new Account(aux.ToString()), true);
                    }
                }

                // Efetuar o redirecionamento da mensagem para sessao dropcopy
                // Atribuir informacoes da sessao original para se gravar na tabela
                QuickFix.Message nosDC = new QuickFix.Message(nos);
                nosDC.SetField(new StringField(CustomTags.ORIG_SESSION, ssAcceptor.Sessao.ToString()));
                nosDC.SetField(new IntField(CustomTags.FIXID, ssAcceptor.Config.IdSessaoFix));
                ssAcceptor.Send2DropCopy(nosDC);
                nosDC.Clear();
                nosDC = null;

                // Validar se o Initiator estah conectado ou instanciado
                if (ssAcceptor.Initiator == null || !ssAcceptor.Initiator.Conectado)
                {
                    logger.Info("Initiator nao existente ou nao disponivel (desconectado) - Gerando rejeicao da requisicao: ClOrdID:" + nos.ClOrdID.ToString());
                    this._generateRejectMessage(nos, s, MsgType.NEWORDERSINGLE, null, ssAcceptor, RejectionMessages.INITIATOR_NOT_AVAILABLE);
                    return;
                }
                
                
                // Valida se os campos basicos estao ok. Caso nao, estoura uma excecao de argument exception
                // onde sera gerado um execution report de retorno
                OrdensConsistencia.ConsistirNovaOrdem(nos, ssAcceptor.Config.Bolsa);

                // Calculo do Limite
                #region Limit Validation
                if (ssAcceptor.CalcularLimite)
                {
                    ordemInfo = FIX44Utilities.FIX44_NOS2OrderInfo(nos);
                    if (!_processLimitVerification(nos, s, account, ssAcceptor, out tpLimite, 0)) // tipo 0 - new order
                        return;
                }
                #endregion

                TOMessage to = new TOMessage();
                to.Sessao = s;
                // to.MensagemQF = new QuickFix.Message(nos);
                to.MensagemQF = nos;
                to.TipoLimite = tpLimite;
                to.Order = ordemInfo;
                ssAcceptor.Initiator.AddMessage(to);
                to = null;
            }
            catch (ArgumentException argEx)
            {
                this._generateRejectMessage(nos, s, MsgType.NEWORDERSINGLE, argEx, ssAcceptor, argEx.Message);
            }
            catch (Exception ex)
            {
                this._generateRejectMessage(nos, s, MsgType.NEWORDERSINGLE, ex, ssAcceptor, string.Empty);
            }
        }
        
        /// <summary>
        /// OrderCancelReplaceRequest
        /// </summary>
        /// <param name="crreq"></param>
        /// <param name="s"></param>
        public void OnMessage(QuickFix.FIX44.OrderCancelReplaceRequest crreq, SessionID s)
        {
            SessionAcceptor ssAcceptor = null;
            TOOrderSession origOrd = null;
            try
            {
                TipoLimiteEnum tpLimite = TipoLimiteEnum.INDEFINIDO;
                OrdemInfo ordemInfo = null;
                // Validacao de existencia da sessao acceptor
                if (!_dicSessionsFix.TryGetValue(s, out ssAcceptor))
                {
                    throw new Exception("Session acceptor does not exist");
                }
                // Validar se o account deve ser substituido (mnemonico por client id)
                string strAux = crreq.Account.getValue();
                int account;
                if (!ssAcceptor.Config.ParseAccount)
                {
                    if (ssAcceptor.Config.Bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                        account = Convert.ToInt32(strAux.Remove(strAux.Length - 1));
                    else
                        account = Convert.ToInt32(strAux);
                }
                else
                {
                    account = this.GetAccountFromMnemonic(strAux);
                    if (account == 0)
                    {
                        logger.InfoFormat("Account invalido, nao foi possivel efetuar a conversao");
                        this._generateRejectMessage(crreq, s, MsgType.ORDERCANCELREPLACEREQUEST, null, ssAcceptor, RejectionMessages.INVALID_ACCOUNT);
                        return;
                    }
                    else
                    {
                        // Reparsear a conta
                        int aux = GeneralFunctions.CalcularCodigoCliente(227, account);
                        crreq.SetField(new Account(aux.ToString()), true);
                    }
                }
                
                // Efetuar o redirecionamento da mensagem para sessao dropcopy
                ssAcceptor.Send2DropCopy(crreq);

                // Validar se a conexao initiator estah ok
                if (ssAcceptor.Initiator == null || !ssAcceptor.Initiator.Conectado)
                {
                    logger.Info("Initiator nao existente ou nao disponivel (desconectado) - Gerando rejeicao da requisicao: ClOrdID:" + crreq.ClOrdID.ToString());
                    this._generateRejectMessage(crreq, s, MsgType.ORDERCANCELREPLACEREQUEST, null, ssAcceptor, RejectionMessages.INITIATOR_NOT_AVAILABLE);
                    return;
                }

                // Buscar TOOrderSession da ordem original
                // Composicao das chaves
                
                string keyExchange = string.Empty;
                string keyOrigClOrdID = string.Empty;
                if (crreq.IsSetOrderID())
                    keyExchange = crreq.OrderID.getValue() + "-" + strAux + "-" + crreq.Symbol.getValue();
                keyOrigClOrdID = crreq.OrigClOrdID.getValue() + "-" + strAux + "-" + crreq.Symbol.getValue();
                origOrd = ssAcceptor.Initiator.GetOrigTOOrderSession(keyOrigClOrdID, keyExchange);
                
                //if (ssAcceptor.Config.IdCliente != account)
                //{
                //    logger.Info("Account nao correspondente a sessao fix. Rejeitando alteracao de ordem: ClOrdID:" + crreq.ClOrdID.ToString());
                //    this._generateRejectMessage(crreq, s, MsgType.ORDERCANCELREPLACEREQUEST, null, ssAcceptor, RejectionMessages.INVALID_ACCOUNT, origOrd);
                //    return;
                //}

                // Consiste os campos basicos da mensagem de alteracao de ordem
                // Caso haja erro, entao gera-se um ArgumentException, e gerar-se-a um 
                // ExecutionReport de rejeicao
                OrdensConsistencia.ConsistirAlteracaoOrdem(crreq, ssAcceptor.Config.Bolsa);
                
                #region Limit Validation
                if (ssAcceptor.CalcularLimite)
                {
                    LimitResponse ret = new LimitResponse();
                    ordemInfo = FIX44Utilities.FIX44_OCRREQ2OrderInfo(crreq);
                    if (!_processLimitVerification(crreq, s, account, ssAcceptor, out tpLimite, 1, 0, origOrd)) // tipo 1 = alteracao
                        return;
                }
                #endregion

                TOMessage to = new TOMessage();
                to.Sessao = s;
                // to.MensagemQF = new QuickFix.Message(crreq);
                to.MensagemQF = crreq;
                to.TipoLimite = tpLimite;
                to.Order = ordemInfo;
                ssAcceptor.Initiator.AddMessage(to);
                to = null;
            }
            catch (ArgumentException argEx)
            {
                this._generateRejectMessage(crreq, s, MsgType.ORDERCANCELREPLACEREQUEST, argEx, ssAcceptor, argEx.Message);
            }
            catch (Exception ex)
            {
                this._generateRejectMessage(crreq, s, MsgType.ORDERCANCELREPLACEREQUEST, ex, ssAcceptor, string.Empty);
            }
        }


        /// <summary>
        /// OrderCancelRequest FIX4.4 version
        /// </summary>
        /// <param name="ocr"></param>
        /// <param name="s"></param>
        public void OnMessage(QuickFix.FIX44.OrderCancelRequest ocr, SessionID s)
        {
            SessionAcceptor ssAcceptor = null;
            TOOrderSession origOrd = null;
            try
            {
                // Validacao de existencia da sessao acceptor
                if (!_dicSessionsFix.TryGetValue(s, out ssAcceptor))
                {
                    throw new Exception("Session acceptor does not exist");
                }
                string strAux = ocr.Account.getValue();
                // Validar se o account deve ser substituido (mnemonico por client id)
                int account;
                if (!ssAcceptor.Config.ParseAccount)
                {
                    if (ssAcceptor.Config.Bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                        account = Convert.ToInt32(strAux.Remove(strAux.Length - 1));
                    else
                        account = Convert.ToInt32(strAux);
                }
                else
                {
                    account = this.GetAccountFromMnemonic(strAux);
                    if (account == 0)
                    {
                        logger.InfoFormat("Account invalido, nao foi possivel efetuar a conversao");
                        this._generateRejectMessage(ocr, s, MsgType.ORDERCANCELREQUEST, null, ssAcceptor, RejectionMessages.INVALID_ACCOUNT);
                        return;
                    }
                    else
                    {
                        // Reparsear a conta
                        int aux = GeneralFunctions.CalcularCodigoCliente(227, account);
                        ocr.SetField(new Account(aux.ToString()), true);
                    }
                }
                // Efetuar o redirecionamento da mensagem para sessao dropcopy
                ssAcceptor.Send2DropCopy(ocr);

                // Validar se a conexao initiator estah ok
                if (ssAcceptor.Initiator == null || !ssAcceptor.Initiator.Conectado)
                {
                    logger.Info("Initiator nao existente ou nao disponivel (desconectado) - Gerando rejeicao da requisicao: ClOrdID:" + ocr.ClOrdID.ToString());
                    this._generateRejectMessage(ocr, s, MsgType.ORDERCANCELREQUEST, null, ssAcceptor, RejectionMessages.INITIATOR_NOT_AVAILABLE);
                    return;
                }

                // Buscar TOOrderSession da ordem original
                // Composicao das chaves
                
                string keyExchange = string.Empty;
                string keyOrigClOrdID = string.Empty;
                if (ocr.IsSetOrderID())
                    keyExchange = ocr.OrderID.getValue() + "-" + strAux + "-" + ocr.Symbol.getValue();
                keyOrigClOrdID = ocr.OrigClOrdID.getValue() + "-" + strAux + "-" + ocr.Symbol.getValue();
                origOrd = ssAcceptor.Initiator.GetOrigTOOrderSession(keyOrigClOrdID, keyExchange);

                
                
                //if (ssAcceptor.Config.IdCliente != account)
                //{
                //    logger.Info("Account nao correspondente a sessao fix. Rejeitando cancelamento de ordem: ClOrdID:" + ocr.ClOrdID.ToString());
                //    this._generateRejectMessage(ocr, s, MsgType.ORDERCANCELREQUEST, null, ssAcceptor, RejectionMessages.INVALID_ACCOUNT, origOrd);
                //    return;
                //}

                // Consiste os campos basicos da mensagem de cancelamento de ordem
                // Caso haja erro, entao gera-se um ArgumentException, e gerar-se-a um 
                // ExecutionReport de rejeicao
                OrdensConsistencia.ConsistirCancelamentoOrdem(ocr, ssAcceptor.Config.Bolsa);

                // Nao validarah limite, pois devera ser feito a partir do retorno do execution report
                TOMessage to = new TOMessage();
                to.Sessao = s;
                // to.MensagemQF = new QuickFix.Message(ocr);
                to.MensagemQF = ocr;
                ssAcceptor.Initiator.AddMessage(to);
                to = null;
            }
            catch (ArgumentException argEx)
            {
                this._generateRejectMessage(ocr, s, MsgType.ORDERCANCELREQUEST, argEx, ssAcceptor, argEx.Message);
            }
            catch (Exception ex)
            {
                this._generateRejectMessage(ocr, s, MsgType.ORDERCANCELREQUEST, ex, ssAcceptor, string.Empty);
            }
        }


        public void OnMessage(QuickFix.FIX44.NewOrderCross noc, SessionID s)
        {
            SessionAcceptor ssAcceptor = null;
            TipoLimiteEnum tpLimite = TipoLimiteEnum.INDEFINIDO;
            try
            {
                // Validacao de existencia da sessao acceptor
                if (!_dicSessionsFix.TryGetValue(s, out ssAcceptor))
                {
                    throw new Exception("Session acceptor does not exist");
                }
                // Efetuar o redirecionamento da mensagem para sessao dropcopy
                ssAcceptor.Send2DropCopy(noc);
                // Validar se a conexao initiator estah ok
                if (ssAcceptor.Initiator == null || !ssAcceptor.Initiator.Conectado)
                {
                    logger.Info("Initiator nao existente ou nao disponivel (desconectado) - Gerando rejeicao da requisicao: CrossID:" + noc.CrossID.ToString());
                    QuickFix.FIX44.Reject rej = new QuickFix.FIX44.Reject();
                    rej.Set(new RefMsgType(MsgType.NEW_ORDER_CROSS));
                    rej.Set(new RefSeqNum(noc.Header.GetInt(Tags.MsgSeqNum)));// Header.GetInt(34)));
                    rej.Set(new Text("Initiator not available for current session"));
                    List<QuickFix.FIX44.ExecutionReport> er = Fix44Translator.Fix44Rejection2ExecutionReportNOC(rej, noc);
                    Session.SendToTarget(er[0], s);
                    Session.SendToTarget(er[1], s);
                    ssAcceptor.Send2DropCopy(er[0]);
                    ssAcceptor.Send2DropCopy(er[1]);
                    return;
                }
                
                // Validar se um dos dois account corresponde a sessao
                // Para NewOrderCross há dois accounts presentes nos PartyIDs
                string strAccount1 = string.Empty;
                string strAccount2 = string.Empty;
                int account1 = 0;
                int account2 = 0;
                if (noc.IsSetNoSides() && noc.NoSides.getValue() == 2)
                {
                    QuickFix.Group grpNoSides = null; noc.GetGroup(1, Tags.NoSides);
                    int len = noc.NoSides.getValue();
                    for (int i = 0; i < len; i++)
                    {
                        grpNoSides = noc.GetGroup(i+1, Tags.NoSides);
                        switch (grpNoSides.GetField(Tags.Side))
                        {
                            case "1":
                                strAccount1 = grpNoSides.IsSetField(Tags.Account)? grpNoSides.GetField(Tags.Account): string.Empty;
                                break;
                            case "2":
                                strAccount2 = grpNoSides.IsSetField(Tags.Account)? grpNoSides.GetField(Tags.Account): string.Empty;
                                break;
                        }
                    }
                    
                    if (ssAcceptor.Config.Bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                    {
                        account1 = Convert.ToInt32(strAccount1.Remove(strAccount1.Length - 1));
                        account2 = Convert.ToInt32(strAccount2.Remove(strAccount2.Length - 1));
                    }
                    else
                    {
                        account1 = Convert.ToInt32(strAccount1);
                        account2 = Convert.ToInt32(strAccount2);
                    }
                }

                // OrdensConsistencia 
                OrdensConsistencia.ConsistirNovaOrdemCross(noc, ssAcceptor.Config.Bolsa);
                // Calculo do Limite
                OrdemInfo aux = null;
                #region Limit Validation
                if (ssAcceptor.CalcularLimite)
                {
                    List<OrdemInfo> ordemInfo = FIX44Utilities.FIX44_NOC2OrderInfo(noc);
                    //if (account1 == ssAcceptor.Config.IdCliente)
                    //{
                        // aux = ordemInfo[0];
                        if (!_processLimitVerification(noc, s, account1, ssAcceptor, out tpLimite, 0, 1)) // tipo 0 - new order
                            return;
                    //}
                    //else
                    //{
                        // aux = ordemInfo[1];
                        if (!_processLimitVerification(noc, s, account2, ssAcceptor, out tpLimite, 0, 2)) // tipo 0 - new order
                            return;
                    //}
                }
                #endregion


                TOMessage to = new TOMessage();
                to.Sessao = s;
                // to.MensagemQF = new QuickFix.Message(noc);
                to.MensagemQF = noc;
                // to.Order = aux;
                to.TipoLimite = tpLimite;
                ssAcceptor.Initiator.AddMessage(to);
                to = null;
            }
            catch (ArgumentException argEx)
            {
                logger.Error("QuickFix44.NewOrderCross(): " + argEx.Message, argEx);
                QuickFix.FIX44.Reject rej = new QuickFix.FIX44.Reject();
                rej.Set(new RefMsgType(MsgType.NEWORDERCROSS));
                rej.Set(new RefSeqNum(noc.Header.GetInt(Tags.MsgSeqNum)));
                rej.Set(new Text(argEx.Message));
                List<QuickFix.FIX44.ExecutionReport> er = Fix44Translator.Fix44Rejection2ExecutionReportNOC(rej, noc);
                Session.SendToTarget(er[0], s);
                Session.SendToTarget(er[1], s);
                if (null != ssAcceptor)
                {
                    ssAcceptor.Send2DropCopy(er[0]);
                    ssAcceptor.Send2DropCopy(er[1]);
                }
            }
            catch (Exception ex)
            {
                logger.Error("QuickFix44.NewOrderCross(): " + ex.Message, ex);
                QuickFix.FIX44.Reject rej = new QuickFix.FIX44.Reject();
                rej.Set(new RefMsgType(MsgType.NEWORDERCROSS));
                rej.Set(new RefSeqNum(noc.Header.GetInt(Tags.MsgSeqNum)));
                rej.Set(new Text("System unavaliable"));
                List<QuickFix.FIX44.ExecutionReport> er = Fix44Translator.Fix44Rejection2ExecutionReportNOC(rej, noc);
                Session.SendToTarget(er[0], s);
                Session.SendToTarget(er[1], s);
                if (null != ssAcceptor)
                {
                    ssAcceptor.Send2DropCopy(er[0]);
                    ssAcceptor.Send2DropCopy(er[1]);
                }
            }
        }

        
        /*
        private void _generate44RejectMessage(QuickFix.Message msg, SessionID s, string msgType, Exception ex, SessionAcceptor ssAcceptor, string msgText)
        {
            try
            {
                if (null != ex)
                {
                    string aux = string.Format("QuickFix44 MsgType: [{0}], Message: [{1}]", msgType, ex.Message);
                    logger.Error(aux, ex);
                }
                
                QuickFix.FIX44.Reject rej = new QuickFix.FIX44.Reject();
                rej.Set(new RefMsgType(msgType));
                rej.Set(new RefSeqNum(msg.Header.GetInt(Tags.MsgSeqNum)));
                if (string.IsNullOrEmpty(msgText))
                    rej.Set(new Text("System unavaliable")); // Mensagem generica para nao expor possiveis erros de aplicacao
                else
                    rej.Set(new Text(msgText));

                if (msgType.Equals(MsgType.ORDER_CANCEL_REQUEST) || msgType.Equals(MsgType.ORDER_CANCEL_REPLACE_REQUEST))
                {
                    QuickFix.FIX44.OrderCancelReject ocr = Fix44Conversions.Fix44Reject2OrderCancelReject(rej, msg);
                    Session.SendToTarget(ocr, s);
                    if (null != ssAcceptor) ssAcceptor.Send2DropCopy(ocr);
                }
                else
                {
                    QuickFix.FIX44.ExecutionReport er = Fix44Conversions.Fix44Rejection2ExecutionReport(rej, msg);
                    Session.SendToTarget(er, s);
                    if (null != ssAcceptor) ssAcceptor.Send2DropCopy(er);
                }
            }
            catch (Exception exC)
            {
                logger.Error("Problemas na geracao de mensagem de reject (tratamento de excecoes) de mensagem fix 4.2: " + exC.Message, exC);
            }
        }
         */ 
        #endregion

        #region Fix 4.2
        public void OnMessage(QuickFix.FIX42.NewOrderSingle nos, SessionID s)
        {
            try
            {
                
                SessionAcceptor ssAcceptor = null;
                // Validacao de existencia da sessao acceptor
                if (!_dicSessionsFix.TryGetValue(s, out ssAcceptor))
                {
                    throw new Exception("Session acceptor does not exist");
                }

                QuickFix.FIX44.NewOrderSingle nos44 = null;
                switch (ssAcceptor.Config.IntegrationID)
                {
                    case IntegrationId.BBG:
                        nos44 = Fix42TranslatorBBG.Fix42NOS_2_Fix44NOS(nos);
                        break;
                }


                // Compor PartyIDs (evitar erro na mensagem que será enviada para o dropcopy
                List<PartyIDItem> lstAux = ssAcceptor.Initiator.GetPartyIDs();
                for (int i =0; i < lstAux.Count; i++)
                {
                    QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup grp = new QuickFix.FIX44.NewOrderSingle.NoPartyIDsGroup();
                    grp.Set(new PartyID(lstAux[i].PartyID));
                    grp.Set(new PartyIDSource(lstAux[i].PartyIDSource));
                    grp.Set(new PartyRole(lstAux[i].PartyRole));
                    nos44.AddGroup(grp);
                    nos44.SetField(new NoPartyIDs(lstAux.Count),true);
                }
                // Validar se a ordem é manual ou automatica.
                // Caso manual, somente enviar para a sessao dropcopy para registrar e NAO rotear a mensagem
                if (nos.IsSetHandlInst())
                {
                    if (nos.HandlInst.getValue() == HandlInst.MANUAL_ORDER)
                    {
                        string clordid = nos.IsSetClOrdID() ? nos.ClOrdID.getValue() : string.Empty;
                        logger.InfoFormat("NOS - Ordem Manual / Administrada. ClOrdID: {0}", clordid);
                        string memo = nos.IsSetField(Tags.Memo) ? nos.GetField(Tags.Memo) : string.Empty;
                        memo += " - Manual Order";
                        nos44.SetField(new Memo(memo));
                        ssAcceptor.Send2DropCopy(nos44);
                        return;
                    }
                }
                this.OnMessage(nos44, s);
            }
            catch (Exception ex)
            {
                logger.Error("OnMessage NewOrderSingle 4.2: Erro: " + ex.Message, ex);
            }
        }

        public void OnMessage(QuickFix.FIX42.OrderCancelReplaceRequest crreq, SessionID s)
        {
            // SessionAcceptor ssAcceptor = null;
            try
            {
                // Validacao de existencia da sessao acceptor
                SessionAcceptor ssAcceptor = null;
                if (!_dicSessionsFix.TryGetValue(s, out ssAcceptor))
                {
                    throw new Exception("Session acceptor does not exist");
                }
                QuickFix.FIX44.OrderCancelReplaceRequest ocrr44 = null; // Fix44Translator.Fix42OCRR_2_Fix44OCRR(crreq);

                // Validar integracao
                switch (ssAcceptor.Config.IntegrationID)
                {
                    case IntegrationId.BBG:
                        ocrr44 = Fix42TranslatorBBG.Fix42OCRR_2_Fix44OCRR(crreq);
                        break;
                }

                // Compor PartyIDs (evitar erro na mensagem que será enviada para o dropcopy
                List<PartyIDItem> lstAux = ssAcceptor.Initiator.GetPartyIDs();
                for (int i = 0; i < lstAux.Count; i++)
                {
                    QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup grp = new QuickFix.FIX44.OrderCancelReplaceRequest.NoPartyIDsGroup();
                    grp.Set(new PartyID(lstAux[i].PartyID));
                    grp.Set(new PartyIDSource(lstAux[i].PartyIDSource));
                    grp.Set(new PartyRole(lstAux[i].PartyRole));
                    ocrr44.AddGroup(grp);
                    ocrr44.SetField(new NoPartyIDs(lstAux.Count), true);
                }
                // Validar se a ordem é manual ou automatica.
                // Caso manual, somente enviar para a sessao dropcopy para registrar e NAO rotear a mensagem
                if (crreq.IsSetHandlInst())
                {
                    if (crreq.HandlInst.getValue() == HandlInst.MANUAL_ORDER)
                    {
                        string clordid = crreq.IsSetClOrdID()? crreq.ClOrdID.getValue(): string.Empty;
                        string origclordid = crreq.IsSetOrigClOrdID()? crreq.OrigClOrdID.getValue(): string.Empty;
                        logger.InfoFormat("OCRR - Ordem Manual / Administrada. ClOrdID: {0} OrigClOrdID: {1}", clordid, origclordid); 
                        string memo = crreq.IsSetField(Tags.Memo)? crreq.GetField(Tags.Memo): string.Empty;
                        memo += " - Manual Order";
                        ocrr44.SetField(new Memo(memo));
                        ssAcceptor.Send2DropCopy(ocrr44);

                        return;
                    }
                }
                this.OnMessage(ocrr44, s);
            }
            
            catch (Exception ex)
            {
                logger.Error("OnMessage OrderCancelReplaceRequest 4.2: Erro: " + ex.Message, ex);
            }
        }

        public void OnMessage(QuickFix.FIX42.OrderCancelRequest ocr, SessionID s)
        {
            try
            {
               
                // Validacao de existencia da sessao acceptor
                SessionAcceptor ssAcceptor = null;
                if (!_dicSessionsFix.TryGetValue(s, out ssAcceptor))
                {
                    throw new Exception("Session acceptor does not exist");
                }
                QuickFix.FIX44.OrderCancelRequest ocr44 = null; //Fix44Translator.Fix42OCR_2_Fix44OCR(ocr);
                switch (ssAcceptor.Config.IntegrationID)
                {
                    case IntegrationId.BBG:
                        ocr44 = Fix42TranslatorBBG.Fix42OCR_2_Fix44OCR(ocr);
                        break;
                }

                // Compor PartyIDs (evitar erro na mensagem que será enviada para o dropcopy
                List<PartyIDItem> lstAux = ssAcceptor.Initiator.GetPartyIDs();
                for (int i = 0; i < lstAux.Count; i++)
                {
                    QuickFix.FIX44.OrderCancelRequest.NoPartyIDsGroup grp = new QuickFix.FIX44.OrderCancelRequest.NoPartyIDsGroup();
                    grp.Set(new PartyID(lstAux[i].PartyID));
                    grp.Set(new PartyIDSource(lstAux[i].PartyIDSource));
                    grp.Set(new PartyRole(lstAux[i].PartyRole));
                    ocr44.AddGroup(grp);
                    ocr44.SetField(new NoPartyIDs(lstAux.Count), true);
                }
                this.OnMessage(ocr44, s);
            }
            catch (Exception ex)
            {
                logger.Error("OnMessage OrderCancelRequest 4.2: Erro: " + ex.Message, ex);
            }
        }
        #endregion
        

        private bool _processLimitVerification(QuickFix.Message msg, SessionID s, int acc, SessionAcceptor ssAcceptor, out TipoLimiteEnum tpLimite, int tipoMsg, int iSide = 0, TOOrderSession to = null)
        {
            tpLimite = TipoLimiteEnum.INDEFINIDO; // initiate
            try
            {
                LimitResponse ret = new LimitResponse();
                int account = 0;
                if (ssAcceptor.Config.Bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                    account = acc;
                else
                    account = LimitControl.GetInstance().ParseAccount(acc);
                //int account = LimitControl.GetInstance().ParseAccount(acc);
                int seqnum = msg.Header.GetInt(Tags.MsgSeqNum);
                string msgt = msg.Header.GetField(Tags.MsgType);
                string symbol = msg.IsSetField(Tags.Symbol) ? msg.GetField(Tags.Symbol) : string.Empty;
                Decimal orderQty = Decimal.Zero;
                //int acc = 0;
                string orderId = string.Empty;
                string origClOrdID= string.Empty;

                // Validar se eh order cross e qual o lado, para buscar as informacoes no lado correto 
                if (msgt == MsgType.NEWORDERCROSS)
                {
                    int len = msg.IsSetField(Tags.NoSides)? msg.GetInt(Tags.NoSides): 0;
                    for (int k = 0; k < len; k++)
                    {
                        QuickFix.Group grp = msg.GetGroup(k+1, Tags.NoSides);
                        if (iSide == grp.GetInt(Tags.Side))
                        {
                            orderQty = grp.IsSetField(Tags.OrderQty) ? grp.GetDecimal(Tags.OrderQty) : Decimal.Zero;
                            orderId = string.Empty;
                        }
                    }
                }
                else
                {
                    orderQty = msg.IsSetField(Tags.OrderQty) ? msg.GetDecimal(Tags.OrderQty) : Decimal.Zero;
                    orderId = msg.IsSetField(Tags.OrderID) ? msg.GetString(Tags.OrderID) : string.Empty;
                    origClOrdID = msg.IsSetField(Tags.OrigClOrdID) ? msg.GetString(Tags.OrigClOrdID) : string.Empty;
                }


                // Regra de exposicao patrimonial
                // Verificacao de papeis teste (bmf e bovespa)
                ret = LimitControl.GetInstance().VerifyTestInstrument(symbol);
                if (0 == ret.ErrorCode)
                    return true;

                // Permissao por bloqueio de envio de ordens no OMS
                ret = LimitControl.GetInstance().VerifyOMSOrderSend(account);
                if (0 != ret.ErrorCode)
                {
                    this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                    return false;
                }


                // Permissao por segmento de mercado (ira retornar o SymbolInfo para futuras utilizacoes
                ret = LimitControl.GetInstance().VerifyMarketPermission(account, symbol, ssAcceptor.Config.Bolsa);
                SymbolInfo symbolInfo = ret.InfoObject as SymbolInfo;
                if (0 != ret.ErrorCode)
                {
                    this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                    //_generateLimitRejectMessage(ret, msgt, seqnum, s, msg, ssAcceptor);
                    return false;
                }
                // Permissao global do instrumento
                char side = iSide ==0 ? msg.GetChar(Tags.Side): Convert.ToChar(iSide.ToString());
                ret = LimitControl.GetInstance().VerifyInstrumentGlobalPermission(symbolInfo.Instrumento, Convert.ToInt32(side.ToString()));
                if (0 != ret.ErrorCode)
                {
                    this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                    return false;
                }
                // Permissao instrumento x grupo
                ret = LimitControl.GetInstance().VerifyInstrumentPerGroupPermission(symbolInfo.Instrumento, Convert.ToInt32(side.ToString()), account);
                if (0 != ret.ErrorCode)
                {
                    this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                    return false;
                }
                // Permissao Instrumento x Cliente
                ret = LimitControl.GetInstance().VerifyInstrumentClientPermission(symbolInfo.Instrumento, Convert.ToInt32(side.ToString()), account);
                if (0 != ret.ErrorCode)
                {
                    this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                    return false;
                }

                // FatFinger - nao se aplica a bmf 
                if (ssAcceptor.Config.Bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                {
                    ret = LimitControl.GetInstance().VerifyFatFinger(account, symbolInfo, orderQty);
                    if (0 != ret.ErrorCode)
                    {
                        this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                        return false;
                    }
                }

                // Permissao de perfil institucional. Caso achado, ignora-se o 
                // o calculo de limite operacional (motivo pelo qual se retorna true a funcao)
                ret = LimitControl.GetInstance().VerifyInstitutionalProfile(account);
                if (0 != ret.ErrorCode)
                {
                    //this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                    return true;
                }

                /////////////////////////// PARAMETROS
                // Limite Operacional (compra a vista, venda a vista, compra de opcao, venda de opcao)
                // Bovespa

                if (ssAcceptor.Config.Bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                {
                    // No original, utiliza valor da ultima cotacao
                    // Validar o tipo de Saldo operacional
                    // Compra
                    if ('1' == side)
                    {
                        switch (symbolInfo.SegmentoMercado)
                        {
                            case SegmentoMercadoEnum.AVISTA:
                            case SegmentoMercadoEnum.FRACIONARIO:
                            case SegmentoMercadoEnum.INTEGRALFRACIONARIO:
                                tpLimite = TipoLimiteEnum.COMPRAAVISTA;
                                break;
                            case SegmentoMercadoEnum.OPCAO:
                                {
                                    tpLimite = TipoLimiteEnum.COMPRAOPCOES;
                                    string strSerieOpcao = Regex.Replace(symbolInfo.Instrumento, "[^A-Za-z _]", string.Empty);
                                    strSerieOpcao = strSerieOpcao.Substring(strSerieOpcao.Length - 1, 1);
                                    ret = LimitControl.GetInstance().VerifiyOptionSeriesBlocked(strSerieOpcao);
                                    if (0 != ret.ErrorCode)
                                    {
                                        this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                                        return false;
                                    }
                                }
                                break;
                        }
                    }
                    // Venda
                    if (side == '2')
                    {
                        switch (symbolInfo.SegmentoMercado)
                        {
                            case SegmentoMercadoEnum.AVISTA:
                            case SegmentoMercadoEnum.FRACIONARIO:
                            case SegmentoMercadoEnum.INTEGRALFRACIONARIO:
                                tpLimite = TipoLimiteEnum.VENDAAVISTA;
                                break;
                            case SegmentoMercadoEnum.OPCAO:
                                {
                                    // Validar se a opcao esta bloqueada 
                                    string strSerieOpcao = Regex.Replace(symbolInfo.Instrumento, "[^A-Za-z _]", string.Empty);
                                    strSerieOpcao = strSerieOpcao.Substring(strSerieOpcao.Length - 1, 1);
                                    ret = LimitControl.GetInstance().VerifiyOptionSeriesBlocked(strSerieOpcao);
                                    if (0 != ret.ErrorCode)
                                    {
                                        this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                                        return false;
                                    }
                                    tpLimite = TipoLimiteEnum.VENDAOPCOES;
                                }
                                break;
                        }
                    }
                    // new order
                    if (0 == tipoMsg)
                    {
                        // Fazer marcacao a mercado
                        decimal ultimoPreco = StreamerManager.GetInstance().GetLastPrice(symbolInfo.Instrumento);
                        if (Decimal.Zero == ultimoPreco)
                        {
                            // Erro de preco base de calculo
                            // Usando mensagem do "fat finger" somente para nao criar nova constante
                            logger.Info("Preco de cotacao e Valor de fechamento zerado - new order");
                            ret = LimitControl.GetInstance().FormatLimitResponse(ErrorMessages.ERR_CODE_FAT_FINGER_BASE_PRICE_ZEROED, ErrorMessages.ERR_FAT_FINGER_BASE_PRICE_ZEROED);
                            this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                            return false;
                        }
                        decimal volumeOrdem = orderQty * ultimoPreco;
                        ret = LimitControl.GetInstance().VerifyOperatingLimit(account, tpLimite, volumeOrdem);
                    }
                    // replace
                    else
                    {
                        decimal volumeOriginal = decimal.Zero;
                        // Fazer marcacao a mercado
                        decimal ultimoPreco = StreamerManager.GetInstance().GetLastPrice(symbolInfo.Instrumento);
                        if (Decimal.Zero == ultimoPreco)
                        {
                            // Erro de preco base de calculo
                            // Usando mensagem do "fat finger" somente para nao criar nova constante
                            logger.Info("Preco de cotacao e Valor de fechamento zerado - order cancel replace request");
                            ret = LimitControl.GetInstance().FormatLimitResponse(ErrorMessages.ERR_CODE_FAT_FINGER_BASE_PRICE_ZEROED, ErrorMessages.ERR_FAT_FINGER_BASE_PRICE_ZEROED);
                            this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                            return false;
                        }
                        
                        decimal volumeAlteracao = orderQty * ultimoPreco;
                        decimal diferencial = decimal.Zero;

                        string chave = origClOrdID + "-" + msg.GetField(Tags.Account) + "-" + msg.GetString(Tags.Symbol);

                        string chaveExch = string.Empty;
                        if  (msg.IsSetField(Tags.OrderID))
                            chaveExch = msg.GetString(Tags.OrderID) + "-" + msg.GetField(Tags.Account) + "-" + msg.GetString(Tags.Symbol);
                        TOOrderSession toOrd = ssAcceptor.Initiator.GetOrigTOOrderSession(chave, chaveExch);
                        if (null != toOrd)
                        {
                            volumeOriginal = (decimal)(toOrd.Order.OrderQty * ultimoPreco);
                            diferencial = volumeAlteracao - volumeOriginal;
                        }
                        else
                        {
                            ret = LimitControl.GetInstance().FormatLimitResponse(ErrorMessages.ERR_CODE_TO_ORDER_NOT_FOUND, ErrorMessages.ERR_TO_ORDER_NOT_FOUND + ": " + chave);
                            this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                            return false;
                        }
                        ret = LimitControl.GetInstance().VerifyOperatingLimit(account, tpLimite, diferencial);
                    }
                    if (0 != ret.ErrorCode)
                    {
                        this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                        return false;
                    }
                }
                // BMF
                else
                {
                    // Limite operacional BMF
                    if (0 == tipoMsg)
                    {
                        ret = LimitControl.GetInstance().VerifyClientBMFLimit(acc, symbolInfo.Instrumento, side.ToString(), orderQty, orderQty);
                    }
                    else
                    {
                        decimal qtdOriginal = decimal.Zero;
                        decimal qtdAlteracao = msg.GetDecimal(Tags.OrderQty);
                        decimal diferencial = decimal.Zero;
                        string chave = msg.GetString(Tags.OrigClOrdID) + "-" + msg.GetField(Tags.Account) + "-" + msg.GetString(Tags.Symbol);
                        string chaveExch = string.Empty;
                        if (msg.IsSetField(Tags.OrderID))    
                            chaveExch = msg.GetString(Tags.OrderID) + "-" + msg.GetField(Tags.Account) + "-" + msg.GetString(Tags.Symbol);
                        TOOrderSession toOrd = ssAcceptor.Initiator.GetOrigTOOrderSession(chave, chaveExch);

                        if (null != toOrd)
                        {
                            qtdOriginal = toOrd.Order.OrderQty;
                            diferencial = qtdAlteracao - qtdOriginal;
                        }
                        else
                        {
                            ret = LimitControl.GetInstance().FormatLimitResponse(ErrorMessages.ERR_CODE_TO_ORDER_NOT_FOUND, ErrorMessages.ERR_TO_ORDER_NOT_FOUND + ": " + chave);
                            this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                            return false;
                        }
                        ret = LimitControl.GetInstance().VerifyClientBMFLimit(acc, symbolInfo.Instrumento, side.ToString(), diferencial, qtdAlteracao);
                    }
                    if (0 != ret.ErrorCode)
                    {
                        this._generateRejectMessage(msg, s, msgt, null, ssAcceptor, ret.ErrorMessage);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Erro na verificacao do limite: " + ex.Message, ex);
                return false;
            }
        }

        private void _generateRejectMessage(QuickFix.Message msg, SessionID s, string msgType, Exception ex, SessionAcceptor ssAcceptor, string msgText)
        {
            try
            {
                // Verificar parse de account para mnemonico
                if (ssAcceptor.Config.ParseAccount)
                {
                    string strAcc = msg.IsSetField(Tags.Account) ? msg.GetString(Tags.Account) : string.Empty;
                    if (!string.IsNullOrEmpty(strAcc))
                    {
                        int intAccount;
                        // 
                        if (Int32.TryParse(strAcc, out intAccount))
                        {
                            if (ssAcceptor.Config.Bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                            {
                                intAccount = Convert.ToInt32(strAcc.Remove(strAcc.Length - 1));
                            }
                            string strMnemonico = this.GetMnemonicFromAccount(intAccount);
                            msg.SetField(new Account(strMnemonico), true);
                        }
                    }
                }
                switch (ssAcceptor.Config.IntegrationID)
                {
                    case IntegrationId.BBG:
                        //this._generate42RejectMessage(msg, s, msgType, ex, ssAcceptor, msgText);
                        Fix42TranslatorBBG.Generate42RejectMessage(msg, s, msgType, ex, ssAcceptor, msgText);
                        break;
                    case IntegrationId.GRD:
                    case IntegrationId.IVF:
                        Fix44Translator.Generate44RejectMessage(msg, s, msgType, ex, ssAcceptor, msgText);
                        break;
                    default:
                        Fix44Translator.Generate44RejectMessage(msg, s, msgType, ex, ssAcceptor, msgText);
                        break;
                }
            }
            catch (Exception exR)
            {
                logger.Error("Problemas na geracao da mensagem de rejeição: " + exR.Message, exR);
            }
        }
        /// <summary>
        /// Validar logon da sessao fix
        /// </summary>
        /// <returns></returns>
        private bool _validateLogon(SessionID ss, string password)
        {
            foreach (KeyValuePair<SessionID, SessionAcceptor> pair in _dicSessionsFix)
            {
                // Deve-se inverter o sndCompID e tgtCompID para validacao, 
                // pois a outra ponta eh initiator e o server eh acceptor
                if (pair.Value.Config.SenderCompID.Equals(ss.TargetCompID) &&
                    pair.Value.Config.TargetCompID.Equals(ss.SenderCompID) &&
                    pair.Value.Config.LogonPassword.Equals(password))
                {
                    pair.Value.Conectado = true;
                    pair.Value.Sessao = ss;
                    return true;
                }
            }
            return false;
        }
        private bool _validateLogout(string sndCompID, string tgtCompID)
        {
            foreach (KeyValuePair<SessionID, SessionAcceptor> pair in _dicSessionsFix)
            {
                // Deve-se inverter o sndCompID e tgtCompID para validacao, 
                // pois a outra ponta eh initiator e o server eh acceptor
                if (pair.Value.Config.SenderCompID.Equals(tgtCompID) &&
                    pair.Value.Config.TargetCompID.Equals(sndCompID))
                {
                    pair.Value.Conectado = false;
                    return true;
                }
            }
            return false;
        }
        #endregion


        #region "Limit Controllers"
        public void UpdateClientLimit(int account, decimal vlrTotal)
        {
            /*
            try
            {
                foreach (KeyValuePair<SessionID, SessionAcceptor> item in _dicSessionsFix)
                {
                    SessionAcceptor ss = item.Value;
                    ss.LimitControl.AtualizarClienteLimite(account, vlrTotal);
                }
            }
            catch (Exception ex)
            {
                logger.Error("UpdateClientLimit(): Erro ao atualizar informacoes do limite de cliente. " + ex.Message, ex);
                throw ex;
            }
            */
        }

        public void ResetDailyLimit()
        {
            /*
            try
            {
                foreach (KeyValuePair<SessionID, SessionAcceptor> item in _dicSessionsFix)
                {
                    SessionAcceptor ss = item.Value;
                    ss.LimitControl.ResetarLimiteDiario();
                }
            }
            catch (Exception ex)
            {
                logger.Error("ResetDailyLimit(): Erro ao resetar limite diario. " + ex.Message, ex);
                throw ex;
            }
            */
        }
        public int GetAccountFromMnemonic(string mnemonic)
        {
            try
            {
                KeyValuePair<int, ClientMnemonicInfo> key = _dicMnemonic.FirstOrDefault(x => x.Value.Mnemonic == mnemonic);
                if (!key.Equals(default(KeyValuePair<int, ClientMnemonicInfo>)))
                    return key.Value.IdCliente;
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public string GetMnemonicFromAccount(int account)
        {
            try
            {
                ClientMnemonicInfo aux = null;
                if (_dicMnemonic.TryGetValue(account, out aux))
                {
                    return aux.Mnemonic;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }


        public void ReloadLimits()
        {
            
        }

        public void SignalUpdateLimit(bool valor)
        {
 
        }

        #endregion

       

    }
}
