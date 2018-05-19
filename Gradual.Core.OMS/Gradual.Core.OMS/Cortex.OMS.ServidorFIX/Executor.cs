using System;
using System.Collections.Generic;
using QuickFix;
using log4net;
using QuickFix.Fields;
using Cortex.OMS.ServidorFIXAdm.Lib.Dados;
using System.Threading;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.OMS.Library.Servicos;

using System.Text;
using Cortex.OMS.FixUtilities.Lib;

namespace Cortex.OMS.ServidorFIX
{
    public class Executor : QuickFix.MessageCracker, QuickFix.Application, IRoteadorOrdensCallback
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<string,SessionFixInfo> fixSessions = new Dictionary<string,ServidorFIXAdm.Lib.Dados.SessionFixInfo>();
        private bool _bKeepRunning = false;
        private long _lastRoteadorMsg = 0;
        private IAssinaturasRoteadorOrdensCallback _roteador = null;
        private Dictionary<string, OrderSessionInfo> _orderSessions = new Dictionary<string, OrderSessionInfo>();
        private Queue<OrdemInfo> _queueAcompanhamento = new Queue<OrdemInfo>();
        private Thread _thProcAcompanhamento = null;
        private Thread _thMonCallbackRot = null;
        private static Executor _me = null;

        int orderID = 0;
        int execID = 0;

        private string GenOrderID() { return (++orderID).ToString(); }
        private string GenExecID() { return (++execID).ToString(); }

        public static Executor Instance { get { return GetInstance(); }}

        public static Executor GetInstance()
        {
            if (_me == null)
            {
                _me = new Executor();
            }

            return _me;
        }

        /// <summary>
        /// 
        /// </summary>
        public Executor()
        {
            try
            {

                CamadaDeDados db = new CamadaDeDados();

                List<SessionFixInfo> fxSess = db.BuscarSessoesFIX();

                foreach (SessionFixInfo sess in fxSess)
                {
                    fixSessions.Add(sess.TargetCompID, sess);
                }

                _orderSessions = PersistenciaEstado.LoadOrderSessions();

                _bKeepRunning = true;
            }
            catch (Exception ex)
            {
                logger.Error("Executor(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            _bKeepRunning = true;
            _thProcAcompanhamento = new Thread(new ThreadStart(_processadorAcompanhamentos));
            _thProcAcompanhamento.Name = "_processadorAcompanhamentos";
            _thProcAcompanhamento.Start();

            _thMonCallbackRot = new Thread(new ThreadStart(_monitorCallbackRoteador));
            _thMonCallbackRot.Name = "_monitorCallbackRoteador";
            _thMonCallbackRot.Start();

        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            _bKeepRunning = false;

            while (_thMonCallbackRot != null && _thMonCallbackRot.IsAlive)
            {
                logger.Info("Aguardando finalizar thread de monitoracao de callback do roteador");
                Thread.Sleep(250);
            }

            while (_thProcAcompanhamento!= null && _thProcAcompanhamento.IsAlive)
            {
                logger.Info("Aguardando finalizar thread de processamento de acompanhamentos");
                Thread.Sleep(250);
            }

            
        }

        /// <summary>
        /// SaveOrderSessions - Persistem a tabela de ordems x sessoes fix ativas
        /// </summary>
        public void SaveOrderSessions()
        {
            try
            {
                PersistenciaEstado.SaveOrderSessions(_orderSessions);
            }
            catch (Exception ex)
            {
                logger.Error("Error SaveOrderSessions(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// PruneOrderSessions - remove da tabela de controle as ordens expiradas, canceladas ou executadas
        /// </summary>
        public void PruneOrderSessions()
        {
            try
            {
                DateTime EndOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                lock (_orderSessions)
                {
                    OrderSessionInfo [] tmp = new OrderSessionInfo[_orderSessions.Count];

                    _orderSessions.Values.CopyTo(tmp, 0);

                    _orderSessions.Clear();

                    foreach (OrderSessionInfo info in tmp)
                    {
                        if (info.ExpireDate >= EndOfDay &&
                            info.OrdStatus != OrdemStatusEnum.CANCELADA &&
                            info.OrdStatus != OrdemStatusEnum.REJEITADA &&
                            info.OrdStatus != OrdemStatusEnum.EXPIRADA &&
                            info.OrdStatus != OrdemStatusEnum.EXECUTADA)
                        {
                            _orderSessions.Add(info.ClOrderID, info);
                        }
                    }
                }

                PersistenciaEstado.SaveOrderSessions(_orderSessions);
            }
            catch (Exception ex)
            {
                logger.Error("Error PruneOrderSessions(): " + ex.Message, ex);
            }
        }

        #region QuickFix.Application Methods
        /// <summary>
        /// FromApp - every inbound application level message will pass through this method,
        /// such as orders, executions, secutiry definitions, and market data.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sessionID"></param>
        public void FromApp(Message message, SessionID sessionID)
        {
            string errormsg = "Error: ";
            try
            {
                logger.Debug("IN:  " + message);
                Crack(message, sessionID);
            }
            catch (Exception ex)
            {
                errormsg += ex.Message;
                logger.Error("FromApp: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// all outbound application level messages pass through this callback before they are sent. 
        /// If a tag needs to be added to every outgoing message, this is a good place to do that
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sessionID"></param>
        public void ToApp(Message message, SessionID sessionID)
        {
            string errormsg = "Error: ";
            try
            {
                logger.Debug("OUT: " + message);
            }
            catch (Exception ex)
            {
                errormsg += ex.Message;
                logger.Error("ToApp: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// every inbound admin level message will pass through this method,
        /// such as heartbeats, logons, and logouts.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sessionID"></param>
        public void FromAdmin(Message message, SessionID sessionID)
        {
            string errormsg = "Error: ";
            try
            {
                string msgType = message.Header.GetString(35);
                logger.Debug("FromAdmin: " + msgType);

                switch (msgType)
                {
                    case QuickFix.FIX44.Logon.MsgType:
                        {
                            QuickFix.FIX44.Logon logonMsg = message as QuickFix.FIX44.Logon;

                            if (logonMsg != null)
                            {
                                // Validates sender & target compids
                                string sndCompID = sessionID.SenderCompID;
                                string tgtCompID = sessionID.TargetCompID;
                                string password = logonMsg.GetString(96);

                                logger.DebugFormat("snd[{0}] tgt[{1}] pwd[{2}]", sndCompID, tgtCompID, password);

                                if (_validateLogon(sndCompID, tgtCompID, password))
                                {
                                    //logonMsg = new QuickFix.FIX44.Logon();
                                    //logonMsg.TestMessageIndicator = new TestMessageIndicator(TestMessageIndicator.YES);
                                    //Session.SendToTarget(logonMsg, sessionID);
                                }
                                else
                                {
                                    QuickFix.FIX44.Logout logout = new QuickFix.FIX44.Logout();
                                    logout.Text = new Text("This 4.4 session was not authorized");

                                    Session.SendToTarget(logout, sessionID);
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                errormsg += ex.Message;
                logger.Error("FromAdmin: " + ex.Message, ex);

            }
        }

        private bool _validateLogon(string sndCompID, string tgtCompID, string password)
        {
            SessionFixInfo fixSession;

            if (fixSessions.ContainsKey(tgtCompID))
            {
                fixSession = fixSessions[tgtCompID];

                if (fixSession.LogonPassword.Equals(password))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// all outbound admin level messages pass through this callback.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sessionID"></param>
        public void ToAdmin(Message message, SessionID sessionID)
        { 
            string errormsg = "Error: ";


            try
            {
                string msgType = message.Header.GetString(35);
                logger.Debug("ToAdmin: " + msgType);

                switch (msgType)
                {
                    case QuickFix.FIX44.Logon.MsgType:
                        {
                            //QuickFix.FIX44.Logon logonMsg = message as QuickFix.FIX44.Logon;

                            //// Validates sender & target compids
                            //string sndCompID = sessionID.SenderCompID;
                            //string tgtCompID = sessionID.TargetCompID;
                            //string password = logonMsg.GetString(96);

                            //logger.DebugFormat("snd[{0}] tgt[{1}] pwd[{2}]", sndCompID, tgtCompID, password);

                            //if ( /* ValidateLogon(sndCompID, tgtCompID, password) */ true )
                            //{
                            //    logonMsg = new QuickFix.FIX44.Logon();
                            //    logonMsg.TestMessageIndicator = new TestMessageIndicator(TestMessageIndicator.YES);
                            //    Session.SendToTarget(logonMsg, sessionID);
                            //}
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                errormsg += ex.Message;
                logger.Error("ToAdmin: " + ex.Message, ex);

            }

        }


        public void OnCreate(SessionID sessionID)
        {
        }


        public void OnLogout(SessionID sessionID)
        {
        }

        public void OnLogon(SessionID sessionID)
        {
            string errormsg = "Invalid logon";

            try
            {
                //TODO: Load from db all itens related to this
                //client. Limits, cash, permissions, etc

            }
            catch (Exception ex)
            {
                // Trouble? quick out him
                errormsg += ex.Message;
                QuickFix.RejectLogon rejLogon = new RejectLogon(errormsg);

                logger.Error("OnLogon: " + ex.Message, ex);
            }
        }


        #endregion

        #region MessageCracker overloads
        public void OnMessage(QuickFix.FIX42.NewOrderSingle nos, SessionID s)
        {
            try
            {
                StringBuilder execText = new StringBuilder();
                Symbol symbol = nos.Symbol;
                Side side = nos.Side;
                OrdType ordType = nos.OrdType;
                OrderQty orderQty = nos.OrderQty;
                Price price = nos.Price;
                ClOrdID clOrdID = nos.ClOrdID;

                SessionFixInfo fixSession = fixSessions[s.TargetCompID];

                CamadaDeDados db = new CamadaDeDados();

                QuickFix.FIX42.ExecutionReport rejreport = FIX42Utilities.FIX42_NOS2ER(nos);

                //TODO: filll the fucking order contents
                OrdemInfo ordem = FIX42Utilities.FIX42_NOS2OrderInfo(nos);
                //ordem.ChannelID = fixSession.Operador;

                //db.InserirOrdem(ordem);

                if (nos.IsSetClearingAccount())
                {
                    logger.Debug("Clearing acount [" + nos.ClearingAccount.getValue() + "]");
                }

                if (nos.IsSetClearingFirm())
                {
                    logger.Debug("Clearing firm [" + nos.ClearingFirm.getValue() + "]");
                }

                // Check if this session is BMF or BOV
                if (fixSession.Bolsa.Equals("BVSP"))
                {
                    ordem.ChannelID = LimiteManager.GetInstance().ObterOperadorBovespa(ordem.Account);
                    if (ordem.ChannelID < 0)
                    {
                        logger.Error("Nao ha canal BVSP configurado para conta/instituicao [" + ordem.Account + "]");

                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        rejreport.Set(new ExecTransType(ExecTransType.NEW));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("No channel was set for order routing (XBVSP)"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;
                        db.InserirOrdem(ordem, "No channel was set for order routing (XBVSP)");

                        return;
                    }
                    ordem.Exchange = "BOVESPA";

                    if (nos.IsSetHandlInst())
                    {
                        if (nos.HandlInst.getValue() == HandlInst.MANUAL_ORDER)
                        {
                            logger.InfoFormat("Order [{0}] requested manual execution", nos.ClOrdID.getValue());

                            ordem.OrdStatus = OrdemStatusEnum.ORDEMADMINISTRADA;
                            db.InserirOrdem(ordem, "Ordem administrada");

                            return;
                        }
                    }

                    if (LimiteManager.GetInstance().VerificarOrdemAdministrada(ordem))
                    {
                        logger.InfoFormat("Order [{0}] tagged for manual execution", nos.ClOrdID.getValue());

                        ordem.OrdStatus = OrdemStatusEnum.ORDEMADMINISTRADA;
                        db.InserirOrdem(ordem, "Ordem administrada");

                        return;
                    }

                    // Get & prealocate limits for the order/account/client
                    if (LimiteManager.GetInstance().PrealocarLimite(ordem) == false)
                    {
                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        rejreport.Set(new ExecTransType(ExecTransType.NEW));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("Account has reached the allocation limit"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;
                        db.InserirOrdem(ordem, "Account has reached the allocation limit");

                        return;
                    }

                    db.InserirOrdem(ordem);
                }
                else
                {
                    ordem.ChannelID = LimiteManager.GetInstance().ObterOperadorBMF(ordem.Account);
                    if (ordem.ChannelID < 0)
                    {
                        logger.Error("Nao ha canal BMF configurado para conta/instituicao [" + ordem.Account + "]");

                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        rejreport.Set(new ExecTransType(ExecTransType.NEW));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("No channel was set for order routing (XBMF)"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;
                        db.InserirOrdem(ordem, "No channel was set for order routing (XBMF)");

                        return;
                    }
                    ordem.Exchange = "BMF";

                    // Tratar repasse
                    // Caso a ordem ja venha marcada como repasse, nem checar no limite
                    if (nos.IsSetHandlInst())
                    {
                        if (nos.HandlInst.getValue() == HandlInst.MANUAL_ORDER)
                        {
                            logger.InfoFormat("Order [{0}] requested manual execution", nos.ClOrdID.getValue());

                            ordem.OrdStatus = OrdemStatusEnum.REPASSE;
                            db.InserirOrdem(ordem, "Ordem de repasse");

                            return;
                        }
                    }

                    // Caso a ordem nao venha marcada, verificar se deve marcar como repasse
                    if (LimiteManager.GetInstance().VerificarOrdemRepasse(ordem))
                    {
                        logger.InfoFormat("Order [{0}] tagged for manual execution", nos.ClOrdID.getValue());

                        ordem.OrdStatus = OrdemStatusEnum.REPASSE;
                        db.InserirOrdem(ordem, "Ordem de repasse");

                        return;
                    }

                    // Get & prealocate limits for the order/account/client
                    if (LimiteManager.GetInstance().PrealocarLimiteBMF(ordem) == false)
                    {
                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        rejreport.Set(new ExecTransType(ExecTransType.NEW));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("Account has reached the allocation limit"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;
                        db.InserirOrdem(ordem, "Account has reached the allocation limit");

                        return;
                    }

                    db.InserirOrdem(ordem);
                }

                IRoteadorOrdens servico = Ativador.Get<IRoteadorOrdens>();

                ExecutarOrdemRequest req = new ExecutarOrdemRequest();

                req.info = ordem;

                ExecutarOrdemResponse resp = servico.ExecutarOrdem(req);

                if (resp.DadosRetorno.StatusResposta != StatusRoteamentoEnum.Sucesso)
                {
                    logger.Error("Erro ao submeter ordem para o roteador, rejeitando");
                    // Report orden delivering fail. Other reports will come from RoteadorOrdens
                    rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                    rejreport.Set(new ExecType(ExecType.REJECTED));
                    rejreport.Set(new ExecTransType(ExecTransType.NEW));
                    //TODO: Generate proper ids for execution....
                    rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                    rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));

                    StringBuilder rejtext = new StringBuilder();

                    foreach (OcorrenciaRoteamentoOrdem ocorr in resp.DadosRetorno.Ocorrencias)
                    {
                        logger.Error("Erro: [" + ocorr.Ocorrencia + "]");
                        rejtext.Append(ocorr.Ocorrencia);
                        rejtext.Append("-");
                    }
                    rejreport.Set(new Text(rejtext.ToString()));

                    Session.SendToTarget(rejreport, s);

                    return;
                }

                OrderSessionInfo info = new OrderSessionInfo();
                info.ClOrderID = nos.ClOrdID.ToString();
                info.SenderCompID = s.SenderCompID;
                info.TargetCompID = s.TargetCompID;
                info.SessionID = s;
                if (ordem.ExpireDate != null && ordem.ExpireDate.HasValue)
                    info.ExpireDate = ordem.ExpireDate.Value;
                else
                    info.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                lock (_orderSessions)
                {
                    if (_orderSessions.ContainsKey(info.ClOrderID))
                        _orderSessions[info.ClOrderID] = info;
                    else
                        _orderSessions.Add(info.ClOrderID, info);
                }

            }
            catch (SessionNotFound ex)
            {
                logger.Error("NOS42(): " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.Error("NOS42(): " + ex.Message, ex);
                QuickFix.FIX42.Reject rej = new QuickFix.FIX42.Reject();
                rej.Set(new RefMsgType(QuickFix.FIX42.NewOrderSingle.MsgType));
                rej.Set(new RefSeqNum(nos.Header.GetInt(34)));
                rej.Set(new Text("System unavaliable"));
                Session.SendToTarget(rej, s);
            }
        }



        /// <summary>
        /// NewOrdemSingle FIX 4.4 version
        /// </summary>
        /// <param name="nos"></param>
        /// <param name="s"></param>
        public void OnMessage(QuickFix.FIX44.NewOrderSingle nos, SessionID s)
        {
            try
            {
                Symbol symbol = nos.Symbol;
                Side side = nos.Side;
                OrdType ordType = nos.OrdType;
                OrderQty orderQty = nos.OrderQty;
                Price price = nos.Price;
                ClOrdID clOrdID = nos.ClOrdID;

                SessionFixInfo fixSession = fixSessions[s.TargetCompID];

                CamadaDeDados db = new CamadaDeDados();

                QuickFix.FIX44.ExecutionReport rejreport = FIX44Utilities.FIX44_NOS2ER(nos);

                //TODO: filll the fucking order contents
                OrdemInfo ordem = FIX44Utilities.FIX44_NOS2OrderInfo(nos);
                //ordem.ChannelID = fixSession.Operador;

                //db.InserirOrdem(ordem);

                // Check if this session is BMF or BOV
                if (fixSession.Bolsa.Equals("BVSP"))
                {
                    ordem.ChannelID = LimiteManager.GetInstance().ObterOperadorBovespa(ordem.Account);
                    if (ordem.ChannelID < 0)
                    {
                        logger.Error("Nao ha canal BVSP configurado para conta/instituicao [" + ordem.Account + "]");

                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("No channel was set for order routing (XBVSP)"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;
                        db.InserirOrdem(ordem, "No channel was set for order routing (XBVSP)");

                        return;
                    }
                    ordem.Exchange = "BOVESPA";

                    if (nos.IsSetHandlInst())
                    {
                        if (nos.HandlInst.getValue() == HandlInst.MANUAL_ORDER)
                        {
                            logger.InfoFormat("Order [{0}] requested manual execution", nos.ClOrdID.getValue());

                            ordem.OrdStatus = OrdemStatusEnum.ORDEMADMINISTRADA;
                            db.InserirOrdem(ordem, "Ordem administrada");

                            return;
                        }
                    }

                    if (LimiteManager.GetInstance().VerificarOrdemAdministrada(ordem))
                    {
                        logger.InfoFormat("Order [{0}] tagged for manual execution", nos.ClOrdID.getValue());

                        ordem.OrdStatus = OrdemStatusEnum.ORDEMADMINISTRADA;
                        db.InserirOrdem(ordem, "Ordem administrada");

                        return;
                    }

                    // Get & prealocate limits for the order/account/client
                    if (LimiteManager.GetInstance().PrealocarLimite(ordem) == false)
                    {
                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("Account has reached the allocation limit"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;
                        db.InserirOrdem(ordem, "Account has reached the allocation limit");

                        return;
                    }

                    db.InserirOrdem(ordem);
                }
                else
                {
                    ordem.ChannelID = LimiteManager.GetInstance().ObterOperadorBMF(ordem.Account);
                    if (ordem.ChannelID < 0)
                    {
                        logger.Error("Nao ha canal BMF configurado para conta/instituicao [" + ordem.Account + "]");

                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("No channel was set for order routing (XBMF)"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;
                        db.InserirOrdem(ordem, "No channel was set for order routing (XBMF)");

                        return;
                    }
                    ordem.Exchange = "BMF";

                    // Tratar repasse
                    // Caso a ordem ja venha marcada como repasse, nem checar no limite
                    if (nos.IsSetHandlInst())
                    {
                        if (nos.HandlInst.getValue() == HandlInst.MANUAL_ORDER)
                        {
                            logger.InfoFormat("Order [{0}] requested manual execution", nos.ClOrdID.getValue());

                            ordem.OrdStatus = OrdemStatusEnum.REPASSE;
                            db.InserirOrdem(ordem, "Ordem de repasse");

                            return;
                        }
                    }

                    // Caso a ordem nao venha marcada, verificar se deve marcar como repasse
                    if (LimiteManager.GetInstance().VerificarOrdemRepasse(ordem))
                    {
                        logger.InfoFormat("Order [{0}] tagged for manual execution", nos.ClOrdID.getValue());

                        ordem.OrdStatus = OrdemStatusEnum.REPASSE;
                        db.InserirOrdem(ordem, "Ordem de repasse");

                        return;
                    }

                    if (LimiteManager.GetInstance().PrealocarLimiteBMF(ordem) == false)
                    {
                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("Account has reached the allocation limit for " + ordem.Symbol));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;
                        db.InserirOrdem(ordem, "Account has reached the allocation limit for " + ordem.Symbol);

                        return;
                    }

                    db.InserirOrdem(ordem);
                }

                IRoteadorOrdens servico = Ativador.Get<IRoteadorOrdens>();

                ExecutarOrdemRequest req = new ExecutarOrdemRequest();

                req.info = ordem;

                ExecutarOrdemResponse resp = servico.ExecutarOrdem(req);

                if (resp.DadosRetorno.StatusResposta != StatusRoteamentoEnum.Sucesso)
                {
                    logger.Error("Erro ao submeter ordem para o roteador, rejeitando");
                    // Report orden delivering fail. Other reports will come from RoteadorOrdens
                    rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                    rejreport.Set(new ExecType(ExecType.REJECTED));
                    //TODO: Generate proper ids for execution....
                    rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                    rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));

                    StringBuilder rejtext = new StringBuilder();

                    foreach( OcorrenciaRoteamentoOrdem ocorr in resp.DadosRetorno.Ocorrencias)
                    {
                        logger.Error("Erro: [" + ocorr.Ocorrencia + "]");
                        rejtext.Append(ocorr.Ocorrencia);
                        rejtext.Append("-");
                    }
                    rejreport.Set(new Text(rejtext.ToString()));

                    Session.SendToTarget(rejreport, s);

                    return;
                }

                OrderSessionInfo info = new OrderSessionInfo();
                info.ClOrderID = nos.ClOrdID.ToString();
                info.SenderCompID = s.SenderCompID;
                info.TargetCompID = s.TargetCompID;
                info.SessionID = s;
                if (ordem.ExpireDate != null && ordem.ExpireDate.HasValue)
                    info.ExpireDate = ordem.ExpireDate.Value;
                else
                    info.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                lock (_orderSessions)
                {
                    if (_orderSessions.ContainsKey(info.ClOrderID))
                        _orderSessions[info.ClOrderID] = info;
                    else
                        _orderSessions.Add(info.ClOrderID, info);
                }

            }
            catch (SessionNotFound ex)
            {
                logger.Error("NOS44(): " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.Error("NOS44(): " + ex.Message, ex);
                QuickFix.FIX44.Reject rej = new QuickFix.FIX44.Reject();
                rej.Set( new RefMsgType(QuickFix.FIX44.NewOrderSingle.MsgType));
                rej.Set( new RefSeqNum(nos.Header.GetInt(34)));
                rej.Set( new Text("System unavaliable"));
                Session.SendToTarget(rej, s);
            }
        }

        public void OnMessage(QuickFix.FIX50.NewOrderSingle n, SessionID s)
        {
            Symbol symbol = n.Symbol;
            Side side = n.Side;
            OrdType ordType = n.OrdType;
            OrderQty orderQty = n.OrderQty;
            Price price = n.Price;
            ClOrdID clOrdID = n.ClOrdID;
            try
            {

                QuickFix.FIX50.ExecutionReport exReport = new QuickFix.FIX50.ExecutionReport(
                    new OrderID(GenOrderID()),
                    new ExecID(GenExecID()),
                    new ExecType(ExecType.FILL),
                    new OrdStatus(OrdStatus.FILLED),
                    side,
                    new LeavesQty(0),
                    new CumQty(orderQty.getValue()));

                exReport.Set(clOrdID);
                exReport.Set(symbol);
                exReport.Set(orderQty);
                exReport.Set(new LastQty(orderQty.getValue()));
                exReport.Set(new LastPx(price.getValue()));
                exReport.Set(new AvgPx(price.getValue()));

                if (n.IsSetAccount())
                    exReport.SetField(n.Account);

                Session.SendToTarget(exReport, s);

                OrderSessionInfo info = new OrderSessionInfo();
                info.ClOrderID = n.ClOrdID.ToString();
                info.SenderCompID = s.SenderCompID;
                info.TargetCompID = s.TargetCompID;
                info.SessionID = s;

                lock (_orderSessions)
                {
                    if (_orderSessions.ContainsKey(info.ClOrderID))
                        _orderSessions[info.ClOrderID] = info;
                    else
                        _orderSessions.Add(info.ClOrderID, info);
                }
            }
            catch (SessionNotFound ex)
            {
                logger.Debug("==session not found exception!==");
                logger.Debug(ex.ToString());
            }
            catch (Exception ex)
            {
                logger.Debug(ex.ToString());
            }
        }


        public void OnMessage(QuickFix.FIX42.OrderCancelRequest ocr, SessionID s)
        {
            try
            {
                string orderid = (ocr.IsSetOrderID()) ? ocr.OrderID.Obj : "NONE";

                QuickFix.FIX42.OrderCancelReject ocrej = FIX42Utilities.FIX42_OCR2OCRJ(ocr);
                ocrej.CxlRejResponseTo = new CxlRejResponseTo(CxlRejResponseTo.ORDER_CANCEL_REQUEST);
                ocrej.CxlRejReason = new CxlRejReason(CxlRejReason.OTHER);
                ocrej.OrderID = new OrderID(orderid);

                SessionFixInfo fixSession = fixSessions[s.TargetCompID];

                CamadaDeDados db = new CamadaDeDados();

                OrdemCancelamentoInfo ordem = FIX42Utilities.FIX42_OCR2OrdemCancelInfo(ocr);

                // Check if this session is BMF or BOV
                if (fixSession.Bolsa.Equals("BVSP"))
                {
                    ordem.ChannelID = LimiteManager.GetInstance().ObterOperadorBovespa(ordem.Account);

                    if (ordem.ChannelID < 0)
                    {
                        logger.Error("Nao ha canal BVSP configurado para conta/instituicao [" + ordem.Account + "]");

                        ocrej.Text = new Text("No channel was set for order routing (XBVSP)");

                        Session.SendToTarget(ocrej, s);

                        return;
                    }

                    ordem.Exchange = "BOVESPA";
                }
                else
                {
                    ordem.ChannelID = LimiteManager.GetInstance().ObterOperadorBMF(ordem.Account);
                    if (ordem.ChannelID < 0)
                    {
                        logger.Error("Nao ha canal BMF configurado para conta/instituicao [" + ordem.Account + "]");

                        ocrej.Text = new Text("No channel was set for order routing (XBVSP)");

                        Session.SendToTarget(ocrej, s);
                        return;
                    }
                    ordem.Exchange = "BMF";
                }

                IRoteadorOrdens servico = Ativador.Get<IRoteadorOrdens>();

                ExecutarCancelamentoOrdemRequest req = new ExecutarCancelamentoOrdemRequest();

                req.info = ordem;

                ExecutarCancelamentoOrdemResponse resp = servico.CancelarOrdem(req);

                if (resp.DadosRetorno.StatusResposta != StatusRoteamentoEnum.Sucesso)
                {
                    logger.ErrorFormat("Erro ao cancelor ordem [{0}], rejeitando", ordem.OrigClOrdID);

                    StringBuilder rejtext = new StringBuilder();

                    foreach (OcorrenciaRoteamentoOrdem ocorr in resp.DadosRetorno.Ocorrencias)
                    {
                        logger.Error("Erro: [" + ocorr.Ocorrencia + "]");
                        rejtext.Append(ocorr.Ocorrencia);
                        rejtext.Append("-");
                    }

                    ocrej.Text = new Text(rejtext.ToString());

                    Session.SendToTarget(ocrej, s);

                    return;
                }

                OrderSessionInfo info = new OrderSessionInfo();
                info.ClOrderID = ocr.ClOrdID.ToString();
                info.SenderCompID = s.SenderCompID;
                info.TargetCompID = s.TargetCompID;
                info.SessionID = s;
                info.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                lock (_orderSessions)
                {
                    if (_orderSessions.ContainsKey(info.ClOrderID))
                        _orderSessions[info.ClOrderID] = info;
                    else
                        _orderSessions.Add(info.ClOrderID, info);
                }
            }
            catch (SessionNotFound ex)
            {
                logger.Error("OCR42(): " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.Error("OCR42(): " + ex.Message, ex);
                QuickFix.FIX42.Reject rej = new QuickFix.FIX42.Reject();
                rej.Set(new RefMsgType(QuickFix.FIX42.OrderCancelRequest.MsgType));
                rej.Set(new RefSeqNum(ocr.Header.GetInt(34)));
                rej.Set(new Text("System unavaliable"));
                Session.SendToTarget(rej, s);
            }
        }

        /// <summary>
        /// OrderCancelRequest FIX4.4 version
        /// </summary>
        /// <param name="ocr"></param>
        /// <param name="s"></param>
        public void OnMessage(QuickFix.FIX44.OrderCancelRequest ocr, SessionID s)
        {
            try
            {
                string orderid = (ocr.IsSetOrderID()) ? ocr.OrderID.Obj : "NONE";

                QuickFix.FIX44.OrderCancelReject ocrej = FIX44Utilities.FIX44_OCR2OCRJ(ocr);
                ocrej.CxlRejResponseTo = new CxlRejResponseTo(CxlRejResponseTo.ORDER_CANCEL_REQUEST);
                ocrej.CxlRejReason = new CxlRejReason(CxlRejReason.OTHER);
                ocrej.OrderID = new OrderID(orderid);

                SessionFixInfo fixSession = fixSessions[s.TargetCompID];

                CamadaDeDados db = new CamadaDeDados();

                OrdemCancelamentoInfo ordem = FIX44Utilities.FIX44_OCR2OrdemCancelInfo(ocr);

                // Check if this session is BMF or BOV
                if (fixSession.Bolsa.Equals("BVSP"))
                {
                    ordem.ChannelID = LimiteManager.GetInstance().ObterOperadorBovespa(ordem.Account);

                    if (ordem.ChannelID < 0)
                    {
                        logger.Error("Nao ha canal BVSP configurado para conta/instituicao [" + ordem.Account + "]");

                        ocrej.Text = new Text("No channel was set for order routing (XBVSP)");

                        Session.SendToTarget(ocrej, s);

                        return;
                    }

                    ordem.Exchange = "BOVESPA";
                }
                else
                {
                    ordem.ChannelID = LimiteManager.GetInstance().ObterOperadorBMF(ordem.Account);
                    if (ordem.ChannelID < 0)
                    {
                        logger.Error("Nao ha canal BMF configurado para conta/instituicao [" + ordem.Account + "]");

                        ocrej.Text = new Text("No channel was set for order routing (XBVSP)");

                        Session.SendToTarget(ocrej, s);
                        return;
                    }
                    ordem.Exchange = "BMF";
                }

                IRoteadorOrdens servico = Ativador.Get<IRoteadorOrdens>();

                ExecutarCancelamentoOrdemRequest req = new ExecutarCancelamentoOrdemRequest();
                
                req.info = ordem;

                ExecutarCancelamentoOrdemResponse resp = servico.CancelarOrdem(req);

                if (resp.DadosRetorno.StatusResposta != StatusRoteamentoEnum.Sucesso)
                {
                    logger.ErrorFormat("Erro ao cancelor ordem [{0}], rejeitando", ordem.OrigClOrdID);

                    StringBuilder rejtext = new StringBuilder();

                    foreach (OcorrenciaRoteamentoOrdem ocorr in resp.DadosRetorno.Ocorrencias)
                    {
                        logger.Error("Erro: [" + ocorr.Ocorrencia + "]");
                        rejtext.Append(ocorr.Ocorrencia);
                        rejtext.Append("-");
                    }

                    ocrej.Text = new Text(rejtext.ToString());

                    Session.SendToTarget(ocrej, s);

                    return;
                }

                OrderSessionInfo info = new OrderSessionInfo();
                info.ClOrderID = ocr.ClOrdID.ToString();
                info.SenderCompID = s.SenderCompID;
                info.TargetCompID = s.TargetCompID;
                info.SessionID = s;
                info.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                lock (_orderSessions)
                {
                    if (_orderSessions.ContainsKey(info.ClOrderID))
                        _orderSessions[info.ClOrderID] = info;
                    else
                        _orderSessions.Add(info.ClOrderID, info);
                }
            }
            catch (SessionNotFound ex)
            {
                logger.Error("OCR44(): " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.Error("OCR44(): " + ex.Message, ex);
                QuickFix.FIX44.Reject rej = new QuickFix.FIX44.Reject();
                rej.Set(new RefMsgType(QuickFix.FIX44.OrderCancelRequest.MsgType));
                rej.Set(new RefSeqNum(ocr.Header.GetInt(34)));
                rej.Set(new Text("System unavaliable"));
                Session.SendToTarget(rej, s);
            }
        }

        public void OnMessage(QuickFix.FIX50.OrderCancelRequest msg, SessionID s)
        {
            string orderid = (msg.IsSetOrderID()) ? msg.OrderID.Obj : "unknown orderID";
            QuickFix.FIX50.OrderCancelReject ocj = new QuickFix.FIX50.OrderCancelReject(
                new OrderID(orderid), msg.ClOrdID, msg.OrigClOrdID, new OrdStatus(OrdStatus.REJECTED), new CxlRejResponseTo(CxlRejResponseTo.ORDER_CANCEL_REQUEST));
            ocj.CxlRejReason = new CxlRejReason(CxlRejReason.OTHER);
            ocj.Text = new Text("Executor does not support order cancels");

            try
            {
                Session.SendToTarget(ocj, s);
            }
            catch (Exception ex)
            {
                logger.Debug(ex.ToString());
            }
        }


        /// <summary>
        /// OrderCancelReplaceRequest FIX4.2 version
        /// </summary>
        /// <param name="ccreq"></param>
        /// <param name="s"></param>
        public void OnMessage(QuickFix.FIX42.OrderCancelReplaceRequest crreq, SessionID s)
        {
            try
            {
                Symbol symbol = crreq.Symbol;
                Side side = crreq.Side;
                OrdType ordType = crreq.OrdType;
                OrderQty orderQty = crreq.OrderQty;
                Price price = crreq.Price;
                ClOrdID clOrdID = crreq.ClOrdID;

                SessionFixInfo fixSession = fixSessions[s.TargetCompID];

                CamadaDeDados db = new CamadaDeDados();

                QuickFix.FIX42.ExecutionReport rejreport = FIX42Utilities.FIX42_OCRREQ2ER(crreq);

                //TODO: filll the fucking order contents
                OrdemInfo ordem = FIX42Utilities.FIX42_OCRREQ2OrderInfo(crreq);
                OrdemInfo ordemOriginal = db.BuscarOrdemClordID(ordem.OrigClOrdID);
                ordem.IdOrdem = ordemOriginal.IdOrdem;
                //ordem.ChannelID = fixSession.Operador;

                //db.InserirOrdem(ordem);

                // Check if this session is BMF or BOV
                if (fixSession.Bolsa.Equals("BVSP"))
                {
                    ordem.ChannelID = LimiteManager.GetInstance().ObterOperadorBovespa(ordem.Account);
                    if (ordem.ChannelID < 0)
                    {
                        logger.Error("Nao ha canal BVSP configurado para conta/instituicao [" + ordem.Account + "]");

                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("No channel was set for order routing (XBVSP)"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;
                        db.InserirOrdem(ordem);

                        return;
                    }
                    ordem.Exchange = "BOVESPA";

                    if (crreq.IsSetHandlInst())
                    {
                        if (crreq.HandlInst.getValue() == HandlInst.MANUAL_ORDER)
                        {
                            logger.InfoFormat("Order [{0}] requested manual execution", crreq.ClOrdID.getValue());

                            ordem.OrdStatus = OrdemStatusEnum.ORDEMADMINISTRADA;
                            db.InserirOrdem(ordem);

                            return;
                        }
                    }

                    if (LimiteManager.GetInstance().VerificarOrdemAdministrada(ordem))
                    {
                        logger.InfoFormat("Order [{0}] tagged for manual execution", crreq.ClOrdID.getValue());

                        ordem.OrdStatus = OrdemStatusEnum.ORDEMADMINISTRADA;
                        db.InserirOrdem(ordem);

                        return;
                    }

                    // Get & prealocate limits for the order/account/client
                    if (LimiteManager.GetInstance().PrealocarLimite(ordem, true) == false)
                    {
                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("Account has reached the allocation limit (XBVSP) - order financial volume is greater than original order"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;

                        return;
                    }

                    db.InserirOrdemBackup(ordemOriginal);
                    db.InserirOrdem(ordem);
                }
                else
                {
                    ordem.ChannelID = LimiteManager.GetInstance().ObterOperadorBMF(ordem.Account);
                    if (ordem.ChannelID < 0)
                    {
                        logger.Error("Nao ha canal BMF configurado para conta/instituicao [" + ordem.Account + "]");

                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("No channel was set for order routing (XBMF)"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;
                        db.InserirOrdem(ordem);

                        return;
                    }
                    ordem.Exchange = "BMF";

                    // Tratar repasse
                    // Caso a ordem ja venha marcada como repasse, nem checar no limite
                    if (crreq.IsSetHandlInst())
                    {
                        if (crreq.HandlInst.getValue() == HandlInst.MANUAL_ORDER)
                        {
                            logger.InfoFormat("Order [{0}] requested manual execution", crreq.ClOrdID.getValue());

                            ordem.OrdStatus = OrdemStatusEnum.REPASSE;
                            db.InserirOrdem(ordem);

                            return;
                        }
                    }

                    // Caso a ordem nao venha marcada, verificar se deve marcar como repasse
                    if (LimiteManager.GetInstance().VerificarOrdemRepasse(ordem))
                    {
                        logger.InfoFormat("Order [{0}] tagged for manual execution", crreq.ClOrdID.getValue());

                        ordem.OrdStatus = OrdemStatusEnum.REPASSE;
                        db.InserirOrdem(ordem);

                        return;
                    }

                    // Get & prealocate limits for the order/account/client
                    if (LimiteManager.GetInstance().PrealocarLimiteBMF(ordem, true) == false)
                    {
                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("Account has reached the allocation limit (XBMF) - order qty is greater than original order"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;

                        return;
                    }

                    db.InserirOrdemBackup(ordemOriginal);
                    db.InserirOrdem(ordem);
                }

                IRoteadorOrdens servico = Ativador.Get<IRoteadorOrdens>();

                ExecutarModificacaoOrdensRequest req = new ExecutarModificacaoOrdensRequest();

                req.info = ordem;

                ExecutarModificacaoOrdensResponse resp = servico.ModificarOrdem(req);

                if (resp.DadosRetorno.StatusResposta != StatusRoteamentoEnum.Sucesso)
                {
                    logger.Error("Erro ao submeter modificacao de ordem para o roteador, rejeitando");
                    // Report orden delivering fail. Other reports will come from RoteadorOrdens
                    rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                    rejreport.Set(new ExecType(ExecType.REJECTED));
                    //TODO: Generate proper ids for execution....
                    rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                    rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));

                    StringBuilder rejtext = new StringBuilder();

                    foreach (OcorrenciaRoteamentoOrdem ocorr in resp.DadosRetorno.Ocorrencias)
                    {
                        logger.Error("Erro: [" + ocorr.Ocorrencia + "]");
                        rejtext.Append(ocorr.Ocorrencia);
                        rejtext.Append("-");
                    }
                    rejreport.Set(new Text(rejtext.ToString()));

                    Session.SendToTarget(rejreport, s);

                    return;
                }

                OrderSessionInfo info = new OrderSessionInfo();
                info.ClOrderID = crreq.ClOrdID.ToString();
                info.SenderCompID = s.SenderCompID;
                info.TargetCompID = s.TargetCompID;
                info.SessionID = s;
                if (ordem.ExpireDate != null && ordem.ExpireDate.HasValue)
                    info.ExpireDate = ordem.ExpireDate.Value;
                else
                    info.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                lock (_orderSessions)
                {
                    if (_orderSessions.ContainsKey(info.ClOrderID))
                        _orderSessions[info.ClOrderID] = info;
                    else
                        _orderSessions.Add(info.ClOrderID, info);
                }

            }
            catch (SessionNotFound ex)
            {
                logger.Error("OCRR42(): " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.Error("OCRR42(): " + ex.Message, ex);
                QuickFix.FIX44.Reject rej = new QuickFix.FIX44.Reject();
                rej.Set(new RefMsgType(QuickFix.FIX44.NewOrderSingle.MsgType));
                rej.Set(new RefSeqNum(crreq.Header.GetInt(34)));
                rej.Set(new Text("System unavaliable"));
                Session.SendToTarget(rej, s);
            }
        }

        public void OnMessage(QuickFix.FIX44.OrderCancelReplaceRequest crreq, SessionID s)
        {
            string orderid = (crreq.IsSetOrderID()) ? crreq.OrderID.Obj : "unknown orderID";
            QuickFix.FIX44.OrderCancelReject ocj = new QuickFix.FIX44.OrderCancelReject(
                new OrderID(orderid), crreq.ClOrdID, crreq.OrigClOrdID, new OrdStatus(OrdStatus.REJECTED), new CxlRejResponseTo(CxlRejResponseTo.ORDER_CANCEL_REPLACE_REQUEST));
            ocj.CxlRejReason = new CxlRejReason(CxlRejReason.OTHER);
            ocj.Text = new Text("Executor does not support order cancel/replaces");

            try
            {
                Symbol symbol = crreq.Symbol;
                Side side = crreq.Side;
                OrdType ordType = crreq.OrdType;
                OrderQty orderQty = crreq.OrderQty;
                Price price = crreq.Price;
                ClOrdID clOrdID = crreq.ClOrdID;

                SessionFixInfo fixSession = fixSessions[s.TargetCompID];

                CamadaDeDados db = new CamadaDeDados();

                QuickFix.FIX44.ExecutionReport rejreport = FIX44Utilities.FIX44_OCRREQ2ER(crreq);

                //TODO: filll the fucking order contents
                OrdemInfo ordem = FIX44Utilities.FIX44_OCRREQ2OrderInfo(crreq);
                OrdemInfo ordemOriginal = db.BuscarOrdemClordID(ordem.OrigClOrdID);
                ordem.IdOrdem = ordemOriginal.IdOrdem;
                //ordem.ChannelID = fixSession.Operador;

                //db.InserirOrdem(ordem);

                // Check if this session is BMF or BOV
                if (fixSession.Bolsa.Equals("BVSP"))
                {
                    ordem.ChannelID = LimiteManager.GetInstance().ObterOperadorBovespa(ordem.Account);
                    if (ordem.ChannelID < 0)
                    {
                        logger.Error("Nao ha canal BVSP configurado para conta/instituicao [" + ordem.Account + "]");

                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("No channel was set for order routing (XBVSP)"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;
                        db.InserirOrdem(ordem);

                        return;
                    }
                    ordem.Exchange = "BOVESPA";

                    if (crreq.IsSetHandlInst())
                    {
                        if (crreq.HandlInst.getValue() == HandlInst.MANUAL_ORDER)
                        {
                            logger.InfoFormat("Order [{0}] requested manual execution", crreq.ClOrdID.getValue());

                            ordem.OrdStatus = OrdemStatusEnum.ORDEMADMINISTRADA;
                            db.InserirOrdem(ordem);

                            return;
                        }
                    }

                    if (LimiteManager.GetInstance().VerificarOrdemAdministrada(ordem))
                    {
                        logger.InfoFormat("Order [{0}] tagged for manual execution", crreq.ClOrdID.getValue());

                        ordem.OrdStatus = OrdemStatusEnum.ORDEMADMINISTRADA;
                        db.InserirOrdem(ordem);

                        return;
                    }

                    // Get & prealocate limits for the order/account/client
                    if (LimiteManager.GetInstance().PrealocarLimite(ordem, true) == false)
                    {
                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("Account has reached the allocation limit (XBVSP) - order financial volume is greater than original order"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;

                        return;
                    }

                    db.InserirOrdemBackup(ordemOriginal);
                    db.InserirOrdem(ordem);
                }
                else
                {
                    ordem.ChannelID = LimiteManager.GetInstance().ObterOperadorBMF(ordem.Account);
                    if (ordem.ChannelID < 0)
                    {
                        logger.Error("Nao ha canal BMF configurado para conta/instituicao [" + ordem.Account + "]");

                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("No channel was set for order routing (XBMF)"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;
                        db.InserirOrdem(ordem);

                        return;
                    }
                    ordem.Exchange = "BMF";

                    // Tratar repasse
                    // Caso a ordem ja venha marcada como repasse, nem checar no limite
                    if (crreq.IsSetHandlInst())
                    {
                        if (crreq.HandlInst.getValue() == HandlInst.MANUAL_ORDER)
                        {
                            logger.InfoFormat("Order [{0}] requested manual execution", crreq.ClOrdID.getValue());

                            ordem.OrdStatus = OrdemStatusEnum.REPASSE;
                            db.InserirOrdem(ordem);

                            return;
                        }
                    }

                    // Caso a ordem nao venha marcada, verificar se deve marcar como repasse
                    if (LimiteManager.GetInstance().VerificarOrdemRepasse(ordem))
                    {
                        logger.InfoFormat("Order [{0}] tagged for manual execution", crreq.ClOrdID.getValue());

                        ordem.OrdStatus = OrdemStatusEnum.REPASSE;
                        db.InserirOrdem(ordem);

                        return;
                    }

                    // Get & prealocate limits for the order/account/client
                    if (LimiteManager.GetInstance().PrealocarLimiteBMF(ordem, true) == false)
                    {
                        rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                        rejreport.Set(new ExecType(ExecType.REJECTED));
                        //TODO: Generate proper ids for execution....
                        rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                        rejreport.Set(new Text("Account has reached the allocation limit (XBMF) - order qty is greater than original order"));

                        Session.SendToTarget(rejreport, s);

                        ordem.OrdStatus = OrdemStatusEnum.REJEITADA;

                        return;
                    }

                    db.InserirOrdemBackup(ordemOriginal);
                    db.InserirOrdem(ordem);
                }

                IRoteadorOrdens servico = Ativador.Get<IRoteadorOrdens>();

                ExecutarModificacaoOrdensRequest req = new ExecutarModificacaoOrdensRequest();

                req.info = ordem;

                ExecutarModificacaoOrdensResponse resp = servico.ModificarOrdem(req);

                if (resp.DadosRetorno.StatusResposta != StatusRoteamentoEnum.Sucesso)
                {
                    logger.Error("Erro ao submeter modificacao de ordem para o roteador, rejeitando");
                    // Report orden delivering fail. Other reports will come from RoteadorOrdens
                    rejreport.Set(new OrdStatus(OrdStatus.REJECTED));
                    rejreport.Set(new ExecType(ExecType.REJECTED));
                    //TODO: Generate proper ids for execution....
                    rejreport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                    rejreport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));

                    StringBuilder rejtext = new StringBuilder();

                    foreach (OcorrenciaRoteamentoOrdem ocorr in resp.DadosRetorno.Ocorrencias)
                    {
                        logger.Error("Erro: [" + ocorr.Ocorrencia + "]");
                        rejtext.Append(ocorr.Ocorrencia);
                        rejtext.Append("-");
                    }
                    rejreport.Set(new Text(rejtext.ToString()));

                    Session.SendToTarget(rejreport, s);

                    return;
                }

                OrderSessionInfo info = new OrderSessionInfo();
                info.ClOrderID = crreq.ClOrdID.ToString();
                info.SenderCompID = s.SenderCompID;
                info.TargetCompID = s.TargetCompID;
                info.SessionID = s;
                if (ordem.ExpireDate != null && ordem.ExpireDate.HasValue)
                    info.ExpireDate = ordem.ExpireDate.Value;
                else
                    info.ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                lock (_orderSessions)
                {
                    if (_orderSessions.ContainsKey(info.ClOrderID))
                        _orderSessions[info.ClOrderID] = info;
                    else
                        _orderSessions.Add(info.ClOrderID, info);
                }

            }
            catch (SessionNotFound ex)
            {
                logger.Error("OCRREQ44(): " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.Error("OCRREQ44(): " + ex.Message, ex);
                QuickFix.FIX44.Reject rej = new QuickFix.FIX44.Reject();
                rej.Set(new RefMsgType(QuickFix.FIX44.OrderCancelReplaceRequest.MsgType));
                rej.Set(new RefSeqNum(crreq.Header.GetInt(34)));
                rej.Set(new Text("System unavaliable"));
                Session.SendToTarget(rej, s);
            }
        }

        public void OnMessage(QuickFix.FIX50.OrderCancelReplaceRequest msg, SessionID s)
        {
            string orderid = (msg.IsSetOrderID()) ? msg.OrderID.Obj : "unknown orderID";
            QuickFix.FIX50.OrderCancelReject ocj = new QuickFix.FIX50.OrderCancelReject(
                new OrderID(orderid), msg.ClOrdID, msg.OrigClOrdID, new OrdStatus(OrdStatus.REJECTED), new CxlRejResponseTo(CxlRejResponseTo.ORDER_CANCEL_REPLACE_REQUEST));
            ocj.CxlRejReason = new CxlRejReason(CxlRejReason.OTHER);
            ocj.Text = new Text("Executor does not support order cancel/replaces");

            try
            {
                Session.SendToTarget(ocj, s);
            }
            catch (Exception ex)
            {
                logger.Debug(ex.ToString());
            }
        }




        // FIX40-41 don't have rejects
        public void OnMessage(QuickFix.FIX42.BusinessMessageReject n, SessionID s) { }
        public void OnMessage(QuickFix.FIX43.BusinessMessageReject n, SessionID s) { }
        public void OnMessage(QuickFix.FIX44.BusinessMessageReject n, SessionID s) { }
        public void OnMessage(QuickFix.FIX50.BusinessMessageReject n, SessionID s) { }
        #endregion //MessageCracker overloads

        private bool GeraExecutionReport(OrdemInfo report)
        {
            SessionID sessionID;
            OrderSessionInfo sessionInfo;

            try
            {

                lock (_orderSessions)
                {
                    if (_orderSessions.ContainsKey(report.ClOrdID))
                    {
                        sessionInfo = _orderSessions[report.ClOrdID];
                        sessionInfo.OrdStatus = report.OrdStatus;
                        _orderSessions[report.ClOrdID] = sessionInfo;
                    }
                    else
                    {
                        logger.ErrorFormat("Nao encontrou Fix Session para ordem [{0}:{1}:{2}] [{3}] [{4}] Qtd:[{5}] Rest:[{6}] Cum:[{7}] Price:[{8}]",
                            report.Exchange,
                            report.ChannelID,
                            report.ClOrdID,
                            report.Account,
                            report.Symbol,
                            report.OrderQty,
                            report.OrderQtyRemmaining,
                            report.CumQty,
                            report.Price);
                        return false;
                    }
                }

                //TODO: verificar se é o melhor ponto
                switch (report.OrdStatus)
                {
                    case OrdemStatusEnum.NOVA:
                    case OrdemStatusEnum.PARCIALMENTEEXECUTADA:
                    case OrdemStatusEnum.EXECUTADA:
                    case OrdemStatusEnum.SUBSTITUIDA:
                        if (report.Exchange.Equals("BOVESPA"))
                            LimiteManager.GetInstance().RecalcularLimite(report);
                        else
                            LimiteManager.GetInstance().RecalcularLimiteBMF(report);
                        break;
                    case OrdemStatusEnum.REJEITADA:
                    case OrdemStatusEnum.CANCELADA:
                        if (report.Exchange.Equals("BOVESPA"))
                            LimiteManager.GetInstance().EstornarLimite(report);
                        else
                            LimiteManager.GetInstance().EstornarLimiteBMF(report);
                        break;
                    default:
                        logger.Debug("Ignorando status [" + report.OrdStatus + "] para fins de movimentacao de limite");
                        break;
                }
                sessionID = sessionInfo.SessionID;

                if (sessionID.BeginString.Equals("FIX.4.2"))
                    GeraFIX42ExecutionReport(report, sessionID);

                if (sessionID.BeginString.Equals("FIX.4.4"))
                    GeraFIX44ExecutionReport(report, sessionID);

                if (sessionID.BeginString.Equals("FIX.5.0"))
                    GeraFIX50ExecutionReport(report, sessionID);
            }
            catch (Exception ex)
            {
                logger.Error("GeraExecutionReport(): " + ex.Message, ex);
                return false;
            }

            return true;
        }

        private void GeraFIX42ExecutionReport(OrdemInfo report, SessionID sessionID)
        {
            StringBuilder execText = new StringBuilder();

            // Report orden delivering fail. Other reports will come from RoteadorOrdens
            QuickFix.FIX42.ExecutionReport exReport = new QuickFix.FIX42.ExecutionReport();


            exReport.Set(new Account(report.Account.ToString()));
            exReport.Set(new Symbol(report.Symbol));
            exReport.Set(FixMessageUtilities.deOrdemTipoParaOrdType(report.OrdType));
            exReport.Set(new ClOrdID(report.ClOrdID));
            exReport.Set(new OrderQty(report.OrderQty));
            exReport.Set(new Price(Convert.ToDecimal(report.Price)));
            exReport.Set(new CumQty(report.CumQty));
            exReport.Set(new LeavesQty(report.OrderQtyRemmaining));
            exReport.Set(new LastPx(report.Acompanhamentos[0].LastPx));
            exReport.Set(new AvgPx(report.Acompanhamentos[0].LastPx));  //TODO Acertar preco médio
            exReport.Set(new LastShares(Convert.ToDecimal(report.Acompanhamentos[0].QuantidadeExecutada)));

            exReport.Set(FixMessageUtilities.deOrdemValidadeParaTimeInForce(report.TimeInForce));
            exReport.Set(FixMessageUtilities.deOrdemStatusParaOrdStatus(report.OrdStatus));
            exReport.Set(FixMessageUtilities.deOrdemStatusParaExecType(report.OrdStatus));


            if (report.Acompanhamentos[0].ExchangeExecID != null &&
                report.Acompanhamentos[0].ExchangeExecID.Length > 0)
            {
                exReport.Set(new ExecID(report.Acompanhamentos[0].ExchangeExecID));
            }
            else
            {
                exReport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            }
            exReport.Set(new ExecTransType(ExecTransType.STATUS));

            //TODO: obter orderid correto da nossa base
            if (report.ExchangeNumberID != null &&
                report.ExchangeNumberID.Length > 0)
            {
                exReport.Set(new OrderID(report.ExchangeNumberID));
            }
            else
            {
                exReport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            }

            if (report.Side == OrdemDirecaoEnum.Compra)
                exReport.Set(new Side(Side.BUY));
            else
                exReport.Set(new Side(Side.SELL));

            if (report.Acompanhamentos[0].Descricao != null &&
                report.Acompanhamentos[0].Descricao.Length > 0)
            {
                execText.AppendFormat("[{0}]", report.Acompanhamentos[0].Descricao);
            }

            if (report.Memo5149 != null && report.Memo5149.Length > 0)
            {
                execText.AppendFormat("[{0}]", report.Memo5149);
            }

            if ( execText.Length > 0 )
                exReport.Set(new Text(execText.ToString()));

            Session.SendToTarget(exReport, sessionID);
        }


        private void GeraFIX44ExecutionReport(OrdemInfo report, SessionID sessionID)
        {
            StringBuilder execText = new StringBuilder();

            // Report orden delivering fail. Other reports will come from RoteadorOrdens
            QuickFix.FIX44.ExecutionReport exReport = new QuickFix.FIX44.ExecutionReport();

            exReport.Set(new Account(report.Account.ToString()));
            exReport.Set(new Symbol(report.Symbol));
            exReport.Set(FixMessageUtilities.deOrdemTipoParaOrdType(report.OrdType));
            exReport.Set(new ClOrdID(report.ClOrdID));
            exReport.Set(new OrderQty(report.OrderQty));
            exReport.Set(new Price(Convert.ToDecimal(report.Price)));
            exReport.Set(new CumQty(report.CumQty));
            exReport.Set(new LeavesQty(report.OrderQtyRemmaining));
            exReport.Set(new LastPx(report.Acompanhamentos[0].LastPx));
            exReport.Set(new AvgPx(report.Acompanhamentos[0].LastPx));  //TODO Acertar preco médio
            exReport.Set(new LastQty(Convert.ToDecimal(report.Acompanhamentos[0].QuantidadeExecutada)));
            exReport.Set(FixMessageUtilities.deOrdemValidadeParaTimeInForce(report.TimeInForce));
            
            // TODO: preencher corretamente os retornos
            exReport.Set(FixMessageUtilities.deOrdemStatusParaOrdStatus(report.OrdStatus));
            exReport.Set(FixMessageUtilities.deOrdemStatusParaExecType(report.OrdStatus));

            if ( report.Acompanhamentos[0].ExchangeExecID != null && 
                report.Acompanhamentos[0].ExchangeExecID.Length > 0 )
            {
                exReport.Set(new ExecID(report.Acompanhamentos[0].ExchangeExecID));
            }
            else
            {
                exReport.Set(new ExecID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            }

            if (report.SecurityID != null && report.SecurityID.Length > 0)
                exReport.Set(new SecurityID(report.SecurityID));
            else
                exReport.Set(new SecurityID(report.Symbol));
            exReport.Set(new SecurityIDSource(SecurityIDSource.EXCHANGE_SYMBOL));

            //TODO: obter orderid correto da nossa base
            if ( report.ExchangeNumberID != null && 
                report.ExchangeNumberID.Length > 0 )
            {
                exReport.Set(new OrderID(report.ExchangeNumberID));
            }
            else
            {
                exReport.Set(new OrderID(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            }

            if ( report.Side==OrdemDirecaoEnum.Compra )
                exReport.Set(new Side(Side.BUY));
            else
                exReport.Set(new Side(Side.SELL));

            // Cliente
            QuickFix.FIX44.ExecutionReport.NoPartyIDsGroup PartyIDGroup1 = new QuickFix.FIX44.ExecutionReport.NoPartyIDsGroup();
            //PartyIDGroup1.set(new PartyID(ordem.Account.ToString()));
            PartyIDGroup1.Set(new PartyID("227"));
            PartyIDGroup1.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup1.Set(new PartyRole(PartyRole.ENTERING_TRADER));

            // Corretora
            QuickFix.FIX44.ExecutionReport.NoPartyIDsGroup PartyIDGroup2 = new QuickFix.FIX44.ExecutionReport.NoPartyIDsGroup();
            PartyIDGroup2.Set(new PartyID("GRA"));
            PartyIDGroup2.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup2.Set(new PartyRole(PartyRole.ENTERING_FIRM));

            // Location ID
            QuickFix.FIX44.ExecutionReport.NoPartyIDsGroup PartyIDGroup3 = new QuickFix.FIX44.ExecutionReport.NoPartyIDsGroup();
            PartyIDGroup3.Set(new PartyID("GRA"));
            PartyIDGroup3.Set(new PartyIDSource(PartyIDSource.PROPRIETARY_CUSTOM_CODE));
            PartyIDGroup3.Set(new PartyRole(PartyRole.SENDER_LOCATION));

            exReport.AddGroup(PartyIDGroup1);
            exReport.AddGroup(PartyIDGroup2);
            exReport.AddGroup(PartyIDGroup3);

            if (report.Acompanhamentos[0].Descricao != null &&
                report.Acompanhamentos[0].Descricao.Length > 0)
            {
                execText.AppendFormat("[{0}]", report.Acompanhamentos[0].Descricao);
            }

            if (report.Memo5149 != null && report.Memo5149.Length > 0)
            {
                execText.AppendFormat("[{0}]", report.Memo5149);
            }

            if (execText.Length > 0)
                exReport.Set(new Text(execText.ToString()));

            Session.SendToTarget(exReport, sessionID);
        }

        private void GeraFIX50ExecutionReport(OrdemInfo report, SessionID sessionID)
        {
            throw new NotImplementedException();
        }

        private void _processadorAcompanhamentos()
        {
            logger.Info("Inicio da thread de processamento de acompanhamentos");

            while (_bKeepRunning)
            {
                List<OrdemInfo> reports = new List<OrdemInfo>();
                lock (_queueAcompanhamento)
                {
                    reports.AddRange(_queueAcompanhamento.ToArray());
                    _queueAcompanhamento.Clear();
                }

                foreach (OrdemInfo report in reports)
                {
                    int maxTries = 0;
                    while (GeraExecutionReport(report) == false && maxTries < 3)
                    {
                        logger.Error("Erro ao gerar execution report");
                        Thread.Sleep(100);
                        maxTries++;
                    }
                }

                if (_queueAcompanhamento.Count == 0)
                {
                    Thread.Sleep(100);
                }
            }

            logger.Info("Finalizacao da thread de processamento de acompanhamentos");
        }

        #region IAssinaturasRoteadorOrdensCallback Members
        public void OrdemAlterada(OrdemInfo report)
        {
            _lastRoteadorMsg = DateTime.Now.Ticks;

            if (report.ChannelID == 700)
                return;

            logger.Debug("OrdemAlterada: " + report.ClOrdID);

            lock (_queueAcompanhamento)
            {
                _queueAcompanhamento.Enqueue(report);
            }
        }

        public void StatusConexaoAlterada(StatusConexaoBolsaInfo status)
        {
            _lastRoteadorMsg = DateTime.Now.Ticks;

            logger.DebugFormat("[{0}] [{1}] [{2}]", status.Bolsa, status.Operador, status.Conectado);
        }
        #endregion // IAssinaturasRoteadorOrdensCallback Members

        /// <summary>
        /// 
        /// </summary>
        private void _monitorCallbackRoteador()
        {
            logger.Info("Iniciando thread de monitoracao de callback com roteador de Ordens");

            while (_bKeepRunning)
            {
                TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - _lastRoteadorMsg);

                if ( ts.TotalMilliseconds > 30000)
                {
                    logger.Info("Reassinando callback com Roteador de Ordens" );

                    try
                    {
                        if (_roteador != null)
                        {
                            Ativador.AbortChannel(_roteador);
                            _roteador = null;
                        }
                        else
                        {
                            _roteador = Ativador.Get<IAssinaturasRoteadorOrdensCallback>(this);

                            AssinarExecucaoOrdemResponse resp = _roteador.AssinarExecucaoOrdens(new AssinarExecucaoOrdemRequest());
                            AssinarStatusConexaoBolsaResponse cnxresp = _roteador.AssinarStatusConexaoBolsa(new AssinarStatusConexaoBolsaRequest());
                            _lastRoteadorMsg = DateTime.Now.Ticks;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("thCallbackRoteador(): " + ex.Message, ex);
                        _roteador = null;
                    }
                }

                Thread.Sleep(250);
            }
        }
    }
}
