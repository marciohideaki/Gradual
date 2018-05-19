using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using log4net;

using QuickFix;
// using QuickFix.FIX44;
using QuickFix.Fields;

using Gradual.Core.OMS.FixServerLowLatency.Util;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

using Cortex.OMS.FixUtilities.Lib;
using Cortex.OMS.ServidorFIX;
using System.Configuration;
using System.IO;
using Gradual.Core.OMS.LimiteManager.Streamer;
using Gradual.Core.OMS.LimiteManager.Lib.Dados;
using Gradual.Core.OMS.FixServerLowLatency.Lib.Dados;
using Gradual.Core.OMS.DropCopy.Lib.Dados;
using Gradual.Core.OMS.FixServerLowLatency.Database;


namespace Gradual.Core.OMS.FixServerLowLatency.Rede
{
    [Serializable]
    public class SessionAcceptor
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        #region Private Variables
        
#if _CQUEUE
        ConcurrentQueue<TOMessage> _queueMsg;
#else
        Queue<TOMessage> _queueMsg;
#endif
        
        Thread _thQueue = null;
        bool _running;       
        #endregion

        public FixInitiator Initiator
        {
            get;
            set;
        }

        public FixDropCopy DropCopy
        {
            get;
            set;
        }

        public FixAcceptor ParentAcceptor
        {
            get;
            set;
        }
        public FixSessionItem Config
        {
            get;
            set;
        }
        public SessionID Sessao
        {
            get;
            set;
        }
        public bool CalcularLimite
        {
            get;
            set;
        }

        public bool Conectado
        {
            get;
            set;
        }
        /*
        public LimiteManager LimitControl
        {
            get;
            internal set;
        }
        */

        public SessionAcceptor()
        {
            this.Initiator = null;
            this.Config = null;
            this.CalcularLimite = false;
            // this.LimitControl = null;
            _running = false;
            this.DropCopy = null;
            this.Conectado = false;
            this.Sessao = null;
            this.ParentAcceptor = null;
        }
        
        
        ~SessionAcceptor()
        {
            Stop();
        }
        #region Thread Controls
        /// <summary>
        /// Iniciar a thread de "listening" da queue
        /// </summary>
        public void Start()
        {
            try
            {
#if _CQUEUE
                _queueMsg = new ConcurrentQueue<TOMessage>();
#else                
                _queueMsg = new Queue<TOMessage>();
#endif
                
                _running = true;
                _thQueue = new Thread(new ThreadStart(SendToClient));
                _thQueue.Priority = ThreadPriority.AboveNormal;
                _thQueue.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Start(): Problemas no start da thread. " + ex.Message, ex);
                throw ex;
            }
        }

        public void Stop()
        {
            if (!_running)
                return;
            _running = false;
            
            /*
            logger.Info("Parando gerenciador de limites para sessao: " + this.Config.SenderCompID);
            if (null != LimitControl)
            {
                LimitControl.Stop();
                LimitControl = null;
            }
            */

            if (_thQueue.IsAlive)
            {
                Thread.Sleep(100);
                _thQueue.Abort();
                _thQueue = null;
            }

#if _CQUEUE
            if (null != _queueMsg)
            {
                _queueMsg = null;
            }
#else
            if (null != _queueMsg)
            {
                _queueMsg.Clear();
                _queueMsg = null;
            }
#endif
        }

        #endregion

        #region Queue Controls
        public void AddMessage(TOMessage to)
        {
            try
            {
#if _CQUEUE
                _queueMsg.Enqueue(to);
                lock (_queueMsg)
                    Monitor.Pulse(_queueMsg);
                //_queueRstEvt.Set();
#else
                lock (_queueMsg)
                {
                    _queueMsg.Enqueue(to);
                    Monitor.Pulse(_queueMsg);
                }
#endif
            }
            catch (Exception ex)
            {
                logger.Error("AddMessage() - Erro na adicao da mensagem da fila: " + ex.Message, ex);
            }
        }

        private void SendToClient()
        {
            try
            {
                while (_running)
                {
                    try
                    {
                        TOMessage to = null;
#if _CQUEUE
                        if (!_queueMsg.TryDequeue(out to))
                        {
                            lock (_queueMsg)
                                Monitor.Wait(_queueMsg,50);
                            
                        }
                        
#else
                        lock (_queueMsg)
                        {
                            if (_queueMsg.Count > 0)
                            {
                                to = _queueMsg.Dequeue();
                            }
                            else
                            {
                                Monitor.Wait(_queueMsg, 5);
                                continue;
                            }
                        }
#endif
                        if (null!=to)
                            this._processMessage(to);
                        to = null;
                    }
                    catch (Exception ex)
                    {
                        logger.Error("erro no envio pro acceptor:" + ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("SendToClient() - Erro no envio para client: " + ex.Message, ex);
            }

        }

        public void Send2DropCopy(QuickFix.Message msg)
        {
            try
            {
                if (null != this.DropCopy)
                {
                    TODropCopy toDC = new TODropCopy();
                    toDC.Canal = this.Config.Operador;
                    toDC.MensagemQF = new QuickFix.Message(msg);
                    this.DropCopy.AddMessage(toDC);
                    toDC = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Send2DropCopy() - Erro no envio para Sessao DropCopy: " + ex.Message, ex);

            }
        }

        private void _processMessage(TOMessage to)
        {
            try
            {
                if (null != to)
                {
                    
                    QuickFix.Message msgQF = to.MensagemQF;
                    string msgType = msgQF.Header.GetField(Tags.MsgType);
                    int account = 0;
                    string strAcc = string.Empty;
                    #region Limit Validation
                    // Atualizacao dos valores de limite, de acordo com o execution report
                    if (this.CalcularLimite)
                    {
                        if (this.Config.Bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                        {
                            strAcc = msgQF.IsSetField(Tags.Account) ? msgQF.GetField(Tags.Account) : string.Empty;
                            if (!string.IsNullOrEmpty(strAcc))
                                account = Convert.ToInt32(strAcc.Remove(strAcc.Length - 1));
                            else
                                account = 0;
                        }
                        else
                        {
                            strAcc = msgQF.IsSetField(Tags.Account) ? msgQF.GetField(Tags.Account) : string.Empty;
                            if (!string.IsNullOrEmpty(strAcc))
                                account = Convert.ToInt32(strAcc);
                            else
                                account = 0;
                        }
                        bool bProfileInst = false;
                        LimitResponse retProfile = LimiteManager.LimitControl.GetInstance().VerifyInstitutionalProfile(account);

                        if (0 != retProfile.ErrorCode)
                            bProfileInst = true;
                        if (msgType == MsgType.EXECUTION_REPORT && !bProfileInst)
                        {
                            
                            // Validar o perfil instituicional 
                            
                            QuickFix.FIX44.ExecutionReport er = (QuickFix.FIX44.ExecutionReport)msgQF;
                            // Bovespa
                            if (this.Config.Bolsa.Equals(ExchangePrefixes.BOVESPA, StringComparison.InvariantCultureIgnoreCase))
                            {
                                // BOVESPA
                                decimal ultimoPreco = StreamerManager.GetInstance().GetLastPrice(er.Symbol.ToString());
                                if (Decimal.Zero != ultimoPreco)
                                {
                                    LimitResponse ret = new LimitResponse();
                                    decimal valorAlocado = er.OrderQty.getValue() * ultimoPreco;
                                    // Retirar o digito verificador
                                    
                                    string chaveAtual = er.ClOrdID.ToString() + "-" + strAcc + "-" + er.Symbol.ToString();

                                    switch (er.OrdStatus.getValue())
                                    {
                                        // Alocar limite da parte e contra parte
                                        case OrdStatus.NEW:
                                        case OrdStatus.REPLACED:
                                        case OrdStatus.CANCELED:
                                            {
                                                // Buscar o "new order" referencial para busca do TipoLimite
                                                // toOrder = this.Initiator.GetTOOrderSession(chaveAtual);
                                                if (null == to.Order)
                                                {
                                                    logger.Info("RefOrder não encontrada (New): " + chaveAtual);
                                                }
                                                else
                                                {
                                                    ret = LimiteManager.LimitControl.GetInstance().UpdateOperationalLimitBovespa(account, to.TipoLimite, valorAlocado, ultimoPreco, Convert.ToInt32(er.Side.ToString()), er.OrdStatus.getValue());
                                                    if (0 != ret.ErrorCode)
                                                    {
                                                        logger.Info("Erro na atualizacao do limite operacional Bovespa (New): " + ret.ErrorMessage);
                                                    }
                                                }
                                            }
                                            break;
                                        // Atualizar a quantidade executada 
                                        case OrdStatus.FILLED:
                                        case OrdStatus.PARTIALLY_FILLED:
                                            if (null != to.Order)
                                            {
                                                to.Order.CumQty = Convert.ToInt32(er.CumQty.getValue());
                                                valorAlocado = er.CumQty.getValue() * ultimoPreco;
                                                ret = LimiteManager.LimitControl.GetInstance().UpdateOperationalLimitBovespa(account, to.TipoLimite, valorAlocado, ultimoPreco, Convert.ToInt32(er.Side.ToString()), er.OrdStatus.getValue());
                                                if (0 != ret.ErrorCode)
                                                {
                                                    logger.Info("Erro na atualizacao do limite operacional Bovespa (Filled / Partially Filled): " + ret.ErrorMessage);
                                                }
                                            }
                                            else
                                            {
                                                logger.Info("RefOrder não encontrada (New): " + chaveAtual);
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    string symbol = er.IsSetSymbol() ? er.Symbol.getValue() : string.Empty;
                                    logger.Info("Preco base (ultimo preco) zerado, ignorando o calculo do limite: " + symbol);
                                }
                            }
                            //bmf
                            else
                            {
                                // BMF
                                // account = er.IsSetField(Tags.Account)?  Convert.ToInt32(er.Account.getValue()): 0;
                                LimitResponse ret = null;
                                string side = er.Side.getValue() == '1' ? "C" : "V";
                                switch (er.OrdStatus.getValue())
                                {
                                    case OrdStatus.NEW:
                                        ret = LimiteManager.LimitControl.GetInstance()
                                        .UpdateOperationalLimitBMF(Convert.ToInt32(er.Account.getValue()), to.TipoLimite,
                                                                  er.Symbol.getValue(), Convert.ToInt32(er.OrderQty.getValue()), 0, side);
                                        break;
                                    case OrdStatus.REPLACED:
                                        {
                                            int qtdResult = (-1) * (to.Order.OrderQty - to.Order.CumQty) ;
                                            ret = LimiteManager.LimitControl.GetInstance()
                                            .UpdateOperationalLimitBMF(Convert.ToInt32(er.Account.getValue()), to.TipoLimite,
                                                                      er.Symbol.getValue(), qtdResult, Convert.ToInt32(er.OrderQty.getValue()), side);
                                        }
                                        break;
                                    case OrdStatus.CANCELED:
                                        {
                                            int qtdResult = (-1) * (to.Order.OrderQty - to.Order.CumQty);
                                            ret = LimiteManager.LimitControl.GetInstance()
                                            .UpdateOperationalLimitBMF(Convert.ToInt32(er.Account.getValue()), to.TipoLimite,
                                                                      er.Symbol.getValue(), qtdResult, 0, side);
                                        
                                        }
                                        break;
                                    // Atualizando a quantidade de 
                                    case OrdStatus.FILLED:
                                    case OrdStatus.PARTIALLY_FILLED:
                                        if (null != to.Order)
                                            to.Order.CumQty = Convert.ToInt32(er.CumQty.getValue());
                                        break;
                                }
                            }
                        }
                    }
                    #endregion
                    
                    // Efetuar verificacao da integracao utilizada, para efetuar a conversao do 
                    switch (this.Config.IntegrationID)
                    {
                        case IntegrationId.BBG:
                            this._sendMessageBBGIntegration(msgType, msgQF, to, account);
                            break;
                        case IntegrationId.GRD:
                        case IntegrationId.IVF:
                        default:
                            this._sendDefaultMessageIntegration(msgQF, to, account);
                            break;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                logger.Error("_processMessage(): " + ex.Message, ex);
            }
        }

        
        private void _sendMessageBBGIntegration(string msgType, QuickFix.Message msgQF, TOMessage to, int account)
        {
            try
            {
                this.Send2DropCopy(msgQF); // Envio de mensagem 4.4 para a sessao drop copy 
                // Validar se deve efetuar o cast da conta para mnemonico 
                if (this.Config.ParseAccount)
                {
                    string mnemonic = this.ParentAcceptor.GetMnemonicFromAccount(account);
                    if (!string.IsNullOrEmpty(mnemonic))
                        msgQF.SetField(new Account(mnemonic), true);
                }
                QuickFix.Message msgQF42 = null;
                if (msgType == MsgType.EXECUTION_REPORT)
                    msgQF42 = Fix42TranslatorBBG.Fix44ER_2_Fix42ER((QuickFix.FIX44.ExecutionReport)msgQF);
                if (msgType == MsgType.ORDER_CANCEL_REJECT)
                    msgQF42 = Fix42TranslatorBBG.Fix44OCR_2_Fix42OCR((QuickFix.FIX44.OrderCancelReject)msgQF);

                Session.SendToTarget(msgQF42, to.Sessao);
                
                msgQF.Clear();
                msgQF = null;
                if (null!=msgQF42)
                    msgQF42.Clear();
                msgQF42 = null;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio de mensagem de integração BBG: " + ex.Message, ex);
            }


        }
        private void _sendDefaultMessageIntegration(QuickFix.Message msgQF, TOMessage to, int account)
        {
            try
            {
                this.Send2DropCopy(msgQF);
                // Validar se deve efetuar o cast da conta para mnemonico 
                if (this.Config.ParseAccount)
                {
                    string mnemonic = this.ParentAcceptor.GetMnemonicFromAccount(account);
                    if (!string.IsNullOrEmpty(mnemonic))
                        msgQF.SetField(new Account(mnemonic), true);
                }
                Session.SendToTarget(msgQF, to.Sessao);
                
                msgQF.Clear();
                msgQF = null;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas no envio da mensagem de integracao default: " + ex.Message, ex); 
            }
        }

        #endregion
    }
}
