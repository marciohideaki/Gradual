using System;
using System.Collections.Generic;
using System.ServiceModel;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using log4net;
using QuickFix;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;

namespace Gradual.OMS.ServicoRoteador
{
    /// <summary>
    /// Implementa um canal de comunicacao fix para a Bovespa
    /// </summary>
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single,ConcurrencyMode=ConcurrencyMode.Multiple)]
    public abstract class CanalNegociacaoBase : QuickFix.MessageCracker, IServicoControlavel,
                                        IAssinaturasRoteadorOrdensCallback, QuickFix.Application
                                        
    {
        protected static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected ServicoStatus _status = ServicoStatus.Parado;
        protected bool _bConectadoBolsa = false;
        protected CanalNegociacaoConfig _config;
        protected SessionID _session;

        protected SocketInitiator _socketInitiator;
        protected SocketAcceptor _socketAcceptor;

        protected List<IRoteadorOrdensCallback> _executionsReportsSubscribers = new List<IRoteadorOrdensCallback>();
        protected List<IRoteadorOrdensCallback> _exchangeStatusSubscribers = new List<IRoteadorOrdensCallback>();

        protected bool finalizarSinalizado = false;
        protected bool logonEfetuado = false;


        #region IServicoControlavel Members
        public virtual void IniciarServico()
        {
            logger.Info("IniciarServico(): iniciando canal Bovespa ....");

            // Carrega configurações
            _config = GerenciadorConfig.ReceberConfig<CanalNegociacaoConfig>();

            // Cria sessao que será usada para mandar as mensagens
            _session =
                new SessionID(
                    new BeginString(_config.BeginString),
                    new SenderCompID(_config.SenderCompID),
                    new TargetCompID(_config.TargetCompID));


            // Cria dicionario da configuracao 
            Dictionary mainDic = new Dictionary();

            if ( _config.SocketAcceptPort > 0 )
                mainDic.setLong("SocketAcceptPort", _config.SocketAcceptPort);
            mainDic.setLong("SocketConnectPort", _config.SocketConnectPort);
            mainDic.setLong("HeartBtInt", _config.HeartBtInt);
            mainDic.setLong("ReconnectInterval", _config.ReconnectInterval);

            mainDic.setBool("ResetOnLogon", _config.ResetSeqNum);
            mainDic.setBool("PersistMessages", _config.PersistMessages);

            // Ver
            // ret.setString("ConnectionType", ConnectionType.ToLower());
            mainDic.setString("SocketConnectHost", _config.Host);
            mainDic.setString("FileStorePath", _config.FileStorePath);
            
            logger.Debug("FileLogPath: " + _config.FileLogPath);
            mainDic.setString("FileLogPath", _config.FileLogPath);
            mainDic.setString("StartTime", _config.StartTime);
            mainDic.setString("EndTime", _config.EndTime);
            if (_config.Initiator)
                mainDic.setString("ConnectionType", "initiator");
            else
                mainDic.setString("ConnectionType", "acceptor");

            Dictionary sessDic = new Dictionary();

            sessDic.setString("BeginString", _config.BeginString);
            sessDic.setString("SenderCompID", _config.SenderCompID);
            sessDic.setString("TargetCompID", _config.TargetCompID);
            sessDic.setString("DataDictionary", _config.DataDictionary);
            sessDic.setBool("UseDataDictionary", true);


            // Configure the session settings
            SessionSettings settings = new SessionSettings();

            settings.set(mainDic);
            settings.set(_session,sessDic);

            FileStoreFactory store = new FileStoreFactory(settings);
            FileLogFactory logs = new FileLogFactory(settings);
            QuickFix.MessageFactory msgs = new DefaultMessageFactory();

            // Cria o socket
            if (_config.Initiator)
            {
                _socketInitiator = new SocketInitiator(this, store, settings, logs, msgs);
                _socketInitiator.start();
            }
            else
            {
                _socketAcceptor = new SocketAcceptor( this, store, settings, logs, msgs);
                _socketAcceptor.start();
            }

            _status = ServicoStatus.EmExecucao;

            logger.Info("IniciarServico(): canal Bovespa em execucao....");
        }

        public virtual void PararServico()
        {
            logger.Info("Finalizando canal Bovespa");

            finalizarSinalizado = true;

            try
            {
                // Para o socket
                if (_config.Initiator)
                {
                    _socketInitiator.stop();
                    _socketInitiator.Dispose();
                    _socketInitiator = null;
                }
                else
                {
                    _socketAcceptor.stop();
                    _socketAcceptor.Dispose();
                    _socketAcceptor = null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em PararServico():" + ex.Message, ex);
            }
            finally
            {
                _status = ServicoStatus.Parado;
            }


            logger.Info("*** Canal Bovespa finalizado ***");
        }

        public virtual ServicoStatus ReceberStatusServico()
        {
            return _status;
        }

        #endregion

        #region IAssinaturasRoteadorOrdensCallback Members

        /// <summary>
        /// AssinarExecutacaoOrdens - trata os pedidos de assinatura de eventos de execucao de ordens
        /// </summary>
        /// <param name="request">objeto do tipo </param>
        /// <returns></returns>
        public virtual AssinarExecucaoOrdemResponse AssinarExecucaoOrdens(AssinarExecucaoOrdemRequest request)
        {
            lock (_executionsReportsSubscribers)
            {
                _executionsReportsSubscribers.Add(Ativador.GetCallback<IRoteadorOrdensCallback>());
            }

            return new AssinarExecucaoOrdemResponse();
        }

        /// <summary>
        /// AssinarStatusConexaoBolsa - trata os pedidos de assinatura dos eventos de conexao e desconexao a bolsa
        /// </summary>
        /// <param name="request">obejto do tipo AssinarStatusConexaoBolsaRequest</param>
        /// <returns>objeto do tipo AssinarStatusConexaoBolsaResponse</returns>
        public virtual AssinarStatusConexaoBolsaResponse AssinarStatusConexaoBolsa(AssinarStatusConexaoBolsaRequest request)
        {
            lock (_exchangeStatusSubscribers)
            {
                _exchangeStatusSubscribers.Add(Ativador.GetCallback<IRoteadorOrdensCallback>());
            }

            return new AssinarStatusConexaoBolsaResponse();
        }

        #endregion

        #region Quickfix Application Members

        public virtual void fromAdmin(QuickFix.Message message, QuickFix.SessionID session)
        {
            try
            {
                logger.Debug("fromAdmin().Session id : " + session.ToString() + " Msg: " + message.GetType().ToString());
                this.crack(message, session);
            }
            catch (Exception ex)
            {
                logger.Error("fromApp() Erro: " + ex.Message, ex );
            }
        }

        public virtual void fromApp(QuickFix.Message message, QuickFix.SessionID session)
        {
            // Faz o processamento
            try
            {
                logger.Debug("fromApp().Session id : " + session.ToString() + " Msg: " + message.GetType().ToString());

                this.crack(message, session);
            }
            catch (Exception ex)
            {
                logger.Error("fromApp() Erro: " + ex.Message, ex );
            }
        }

        public virtual void onCreate(QuickFix.SessionID session)
        {
            logger.Debug("onCreate().Session id : " + session.ToString());
        }

        public virtual void onLogon(QuickFix.SessionID session)
        {
            logger.Info("onLogon()");

            _bConectadoBolsa = true;

            _sendConnectionStatus();
        }

        public virtual void onLogout(QuickFix.SessionID session)
        {
            logger.Info("onLogout()");

            _bConectadoBolsa = false;

            _sendConnectionStatus();
        }


        public virtual void toAdmin(QuickFix.Message message, QuickFix.SessionID session)
        {
            // Faz o processamento
            try
            {
                logger.Debug("toAdmin(). Session id : " + session.ToString() + " Msg: " + message.GetType().ToString());

                if (message.getHeader().getField(MsgType.FIELD) != QuickFix.MsgType.Heartbeat)
                    this.crack(message, session);
            }
            catch (Exception ex)
            {
                logger.Error("toAdmin() Erro: " + ex.Message, ex);
            }
            
        }

        public virtual void toApp(QuickFix.Message message, QuickFix.SessionID session)
        {
            // Faz o processamento
            try
            {
                logger.Debug("toApp(). Session id : " + session.ToString() + " Msg: " + message.GetType().ToString());

                this.crack(message, session);
            }
            catch (Exception ex)
            {
                logger.Error("toApp() Erro: " + ex.Message, ex);
            }
            
        }

        #endregion // Quickfix Application Members

        /// <summary>
        /// Envia o relatorio de execucao para os assinantes
        /// </summary>
        /// <param name="order">objeto OrdemInfo</param>
        protected virtual void _sendExecutionReport(OrdemInfo order)
        {
            List<IRoteadorOrdensCallback> toDelete = new List<IRoteadorOrdensCallback>();

            try
            {
                lock (_executionsReportsSubscribers)
                {
                    foreach (IRoteadorOrdensCallback subscriber in _executionsReportsSubscribers)
                    {
                        if (Ativador.IsValidChannel(subscriber))
                        {
                            try
                            {
                                subscriber.OrdemAlterada(order);
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex);

                                logger.Info("Abortando channel para assinante: " + subscriber.ToString());
                                Ativador.AbortChannel(subscriber);

                                logger.Info("Removendo assinante da lista: " + subscriber.ToString());
                                toDelete.Add(subscriber);
                            }
                        }
                        else
                        {
                            logger.Info("Removendo assinante da lista: " + subscriber.ToString());
                            toDelete.Add(subscriber);
                        }
                    }

                    // Remove os assinantes abandonados/falhos da lista
                    foreach (IRoteadorOrdensCallback subscriber in toDelete)
                    {
                        _executionsReportsSubscribers.Remove(subscriber);
                    }

                    toDelete.Clear();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em _sendExecutionReport(): " + ex.Message,ex);
            }
        }

        /// <summary>
        /// Envia o relatorio de execucao para os assinantes
        /// </summary>
        /// <param name="order">objeto OrdemInfo</param>
        protected virtual void _sendConnectionStatus()
        {
            List<IRoteadorOrdensCallback> toDelete = new List<IRoteadorOrdensCallback>();

            StatusConexaoBolsaInfo conexao = new StatusConexaoBolsaInfo();

            conexao.Bolsa = _config.Bolsa;
            conexao.Operador = _config.Operador;
            conexao.Conectado = _bConectadoBolsa;

            try
            {
                lock (_exchangeStatusSubscribers)
                {
                    foreach (IRoteadorOrdensCallback subscriber in _exchangeStatusSubscribers)
                    {
                        if (Ativador.IsValidChannel(subscriber))
                        {
                            try
                            {
                                subscriber.StatusConexaoAlterada(conexao);
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex);

                                logger.Info("Abortando channel para assinante: " + subscriber.ToString());
                                Ativador.AbortChannel(subscriber);

                                logger.Info("Removendo assinante da lista: " + subscriber.ToString());
                                toDelete.Add(subscriber);
                            }
                        }
                        else
                        {
                            logger.Info("Removendo assinante da lista: " + subscriber.ToString());
                            toDelete.Add(subscriber);
                        }
                    }

                    // Remove os assinantes abandonados/falhos da lista
                    foreach (IRoteadorOrdensCallback subscriber in toDelete)
                    {
                        _exchangeStatusSubscribers.Remove(subscriber);
                    }

                    toDelete.Clear();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Erro em _sendExecutionReport(): " + ex.Message, ex);
            }
        }

    }
}
