using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.RoteadorOrdens.Lib;
using log4net;
using Gradual.OMS.RoteadorOrdens.Lib.Mensagens;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using Gradual.OMS.RoteadorOrdensAdm.Lib;
using Gradual.OMS.RoteadorOrdensAdm.Lib.Mensagens;
using Gradual.OMS.RoteadorOrdensAdm.Lib.Dados;
using System.Collections.Concurrent;

namespace Gradual.OMS.ServicoRoteador
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
	public class ServicoRoteadorOrdens: IServicoControlavel, IRoteadorOrdens, 
                                        IAssinaturasRoteadorOrdensCallback, IRoteadorOrdensCallback,
                                        IRoteadorOrdensAdmin
	{
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ServicoStatus _status = ServicoStatus.Parado;
		private List<IRoteadorOrdensCallback> _executionsReportsSubscribers = new List<IRoteadorOrdensCallback>();
		private List<IRoteadorOrdensCallback> _exchangeStatusSubscribers = new List<IRoteadorOrdensCallback>();
        private RoteadorOrdensConfig _config = null;
        private PersistentQueue<OrdemInfo> _orderReportInfo = new PersistentQueue<OrdemInfo>();
        private bool _bOrdemInfoSent = false;


        private Hashtable _canais = new Hashtable();
        private Thread _thrMonitorCanais = null;
        private bool _bKeepRunning = false;
        private Thread _thrSendReport = null;
        private Queue<OrdemInfo> queueReport = new Queue<OrdemInfo>();
        private Queue<StatusConexaoBolsaInfo> queueStatus = new Queue<StatusConexaoBolsaInfo>();
        private Semaphore semGoHorse = new Semaphore(0, Int32.MaxValue);
        private Hashtable hstOrderedReports = new Hashtable();
        private ConcurrentQueue<ExecutarCancelamentoOrdemRequest> queueCancelamento = new ConcurrentQueue<ExecutarCancelamentoOrdemRequest>();
        private Thread thCancelProcessor = null;

		
        #region IAssinaturasRoteadorOrdensCallback Members
        /// <summary>
        /// AssinarExecutacaoOrdens - trata os pedidos de assinatura de eventos de execucao de ordens
        /// </summary>
        /// <param name="request">objeto do tipo </param>
        /// <returns></returns>
        public AssinarExecucaoOrdemResponse  AssinarExecucaoOrdens(AssinarExecucaoOrdemRequest request)
        {
            IRoteadorOrdensCallback subscriber = Ativador.GetCallback<IRoteadorOrdensCallback>();

            logger.Debug("Recebeu pedido de assinatura de ordens: " + ((IContextChannel)subscriber).RemoteAddress.ToString());

            // Guarda a referencia do assinante na lista interna de
            // assinante
            lock( _executionsReportsSubscribers )
            {
                if (subscriber != null)
                {
                    _executionsReportsSubscribers.Add(subscriber);
                }
            }

            // Envia as respostas de ordens ja recebidas para o assinante
            List<OrdemInfo> queuedInfos = null;
            lock(_orderReportInfo)
            {
                if (_orderReportInfo.Count > 0)
                {
                     queuedInfos = _orderReportInfo.ToList();
                }
            }

            //if ( queuedInfos != null && queuedInfos.Count > 0 )
            //{
            //    logger.Info("Existem " + queuedInfos.Count + " msgs enfileiradas, encaminhando para assinante");

            //    ThreadPool.QueueUserWorkItem(
            //        new WaitCallback(
            //            delegate(object required)
            //            {

            //                foreach (OrdemInfo info in queuedInfos)
            //                {
            //                    subscriber.OrdemAlterada(info);
            //                    _bOrdemInfoSent = true;
            //                }
            //            }
            //        )
            //    );
            //}


            return new AssinarExecucaoOrdemResponse();
        }

        /// <summary>
        /// AssinarStatusConexaoBolsa - trata os pedidos de assinatura dos eventos de conexao e desconexao a bolsa
        /// </summary>
        /// <param name="request">obejto do tipo AssinarStatusConexaoBolsaRequest</param>
        /// <returns>objeto do tipo AssinarStatusConexaoBolsaResponse</returns>
        public AssinarStatusConexaoBolsaResponse  AssinarStatusConexaoBolsa(AssinarStatusConexaoBolsaRequest request)
        {
            IRoteadorOrdensCallback subscriber = Ativador.GetCallback<IRoteadorOrdensCallback>();

            logger.Debug("Recebeu pedido de assinatura de status de conexao: " + ((IContextChannel)subscriber).RemoteAddress.ToString());

            lock (_exchangeStatusSubscribers)
            {
                _exchangeStatusSubscribers.Add(subscriber);
            }

            return new AssinarStatusConexaoBolsaResponse();
        }

        /// <summary>
        /// Trata as requisicoes de ping - deve ser invocado para manter o canal WCF ativo
        /// </summary>
        /// <param name="request">objeto do tipo PingRequest</param>
        /// <returns>objeto do tipo PingResponse</returns>
        public PingResponse Ping(PingRequest request)
        {
            PingResponse response = new PingResponse();

            response.Timestamp = DateTime.Now;

            return response;
        }
        #endregion

		

        #region IServicoControlavel Members
        /// <summary>
        /// 
        /// </summary>
		public void IniciarServico()
		{
            logger.Info("Iniciando Servico Roteador de Ordens.....");

            _bKeepRunning = true;
            _config = GerenciadorConfig.ReceberConfig<RoteadorOrdensConfig>();
            if (_config == null)
            {
                throw new Exception("Erro ao carregar configuracoes");
            }

            // Carrega fila
            _orderReportInfo.PersistenceFile = _config.PathFilaOrdens;
            logger.Info("Carregando reports enfileirados em [" + _orderReportInfo.PersistenceFile + "]");
            _orderReportInfo.LoadQueue();

            logger.Info("Iniciando thread de tratamento da fila de cancelamentos");
            thCancelProcessor = new Thread(new ThreadStart(ProcessadorCancelamentos));
            thCancelProcessor.Name = "procCancelamentos";
            thCancelProcessor.Start();


            logger.Info("Iniciando canais.....");

            _carregarCanais();

            // Iniciando thread de monitoracao dos canais
            _bKeepRunning = true;
            _thrMonitorCanais = new Thread(new ThreadStart(RunMonitor));
            _thrMonitorCanais.Priority = ThreadPriority.BelowNormal;
            _thrMonitorCanais.Start();

            this._thrSendReport = new Thread(new ThreadStart(RunInfoDispacher));
            _thrSendReport.Start();

            _status = ServicoStatus.EmExecucao;

            logger.Info("Servico Roteador de Ordens iniciado");
        }

        /// <summary>
        /// 
        /// </summary>
		public void PararServico()
		{
            logger.Info("Finalizando servico Roteador de Ordens....");

            // Para o monitor de canais e sinaliza para terminar a thread
            _bKeepRunning = false;

            // Aguarda o termino da thread antes de finalizar
            while (_thrMonitorCanais.IsAlive)
            {
                logger.Debug("Aguardando finalizacao do monitor de canais");
                Thread.Sleep(250);
            }


            // Aguarda o termino da thread antes de finalizar
            while (_thrMonitorCanais.IsAlive)
            {
                logger.Debug("Aguardando finalizacao do monitor de canais");
                Thread.Sleep(250);
            }

			// Code to stop the main thread
            _fecharCanais();

            // Se enviou os reports de ordens para algum assinante
            // limpa a fila de envio
            if (_bOrdemInfoSent)
            {
                _orderReportInfo.Clear();
            }
            _orderReportInfo.SaveQueue();
            

            _status = ServicoStatus.Parado;

            logger.Info("Servico Roteador de Ordens iniciado");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServicoStatus ReceberStatusServico()
		{
			return _status;
		}
        #endregion //IServicoControlavel Members


        #region IRoteadorOrdens Members
        /// <summary>
        /// Envia um pedido de execucao de ordem para o canal correspondente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecutarOrdemResponse ExecutarOrdem(ExecutarOrdemRequest request)
        {
            ExecutarOrdemResponse response = new ExecutarOrdemResponse();
            CanalInfo _canal = null;
            StatusRoteamentoEnum status = StatusRoteamentoEnum.Sucesso;
            string msg = "Ordem Enviada";

            logger.Debug("*** ExecutarOrdem()");

            TradutorFix.DumpOrdemInfo(request.info);

            try
            {
                _canal = (CanalInfo) _canais[request.info.Exchange+request.info.ChannelID];

                if (_canal == null )
                {
                    msg = "Nao ha canal configurado para " + request.info.Exchange + "-" + request.info.ChannelID;
                    status = StatusRoteamentoEnum.Erro;
                    logger.Info(msg);
                    response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaEO(msg, status);
                }
                else
                {
                    if (_canal.roteador == null || _canal.Conectado == false)
                    {
                        status = StatusRoteamentoEnum.Erro;
                        msg = "Nao ha canal ativo e conectado para " + request.info.Exchange + "-" + request.info.ChannelID;
                        logger.Info(msg);
                        response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaEO(msg, status);
                    }
                    else
                    {
                        _criaReportStore(request.info.ClOrdID);

                        _notificaEnvioParaCanal(request.info);

                        response = _canal.roteador.ExecutarOrdem(request);

                    }
                }
            }
            catch (Exception ex)
            {
                msg = "Error ExecutarOrdem(): " + ex.Message;
                status = StatusRoteamentoEnum.Erro;
                logger.Error(msg + "-" + ex.StackTrace);
                response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaEO(msg, status);

                if (_canal != null)
                {
                    _resetCanal(_canal);
                }
            }

            logger.Debug("*** End of ExecutarOrdem()");

            return response;
        }

        /// <summary>
        /// Envia um pedido de cancelamento de ordem para o canal correspondente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecutarCancelamentoOrdemResponse CancelarOrdem(ExecutarCancelamentoOrdemRequest request)
        {
            ExecutarCancelamentoOrdemResponse response = new ExecutarCancelamentoOrdemResponse();
            CanalInfo _canal = null;
            StatusRoteamentoEnum status = StatusRoteamentoEnum.Sucesso;
            string msg = "Cancelamento Enviado";

            logger.Debug("Request de Cancelamento Recebido:");
            logger.Debug("Cliente ......: " + request.info.Account);
            logger.Debug("Bolsa ........: " + request.info.Exchange);
            logger.Debug("Canal ........: " + request.info.ChannelID);
            logger.Debug("Order ID .....: " + request.info.OrderID);
            logger.Debug("OrigClOrdID ..: " + request.info.OrigClOrdID);
            logger.Debug("ClOrdID ......: " + request.info.ClOrdID);


            try
            {
                _canal = (CanalInfo)_canais[request.info.Exchange + request.info.ChannelID];

                if (!_bKeepRunning)
                {
                    msg = "Servico esta em finalizacao";
                    status = StatusRoteamentoEnum.Erro;
                    logger.Info(msg);
                    response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaCancelamento(msg, status);
                    return response;
                }

                if (_canal == null )
                {
                    msg = "Nao ha canal configurado para " + request.info.Exchange + "-" + request.info.ChannelID;
                    status = StatusRoteamentoEnum.Erro;
                    logger.Info(msg);
                    response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaCancelamento(msg, status);
                }
                else
                {
                    if (_canal.roteador == null || _canal.Conectado == false)
                    {
                        status = StatusRoteamentoEnum.Erro;
                        msg = "Nao ha canal ativo e conectado para " + request.info.Exchange + "-" + request.info.ChannelID;
                        logger.Info(msg);
                        response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaCancelamento(msg, status);
                    }
                    else
                    {
                        response = EnfileirarCancelamento(request);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "CancelarOrdem(): " + ex.Message;
                status = StatusRoteamentoEnum.Erro;
                logger.Error(msg + "-" + ex.StackTrace);
                response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaCancelamento(msg, status);

                if (_canal != null)
                {
                    _resetCanal(_canal);
                }
            }

            logger.Info(msg);

            logger.Debug("Request de Cancelamento Recebido:");

            return response;
        }

        private ExecutarCancelamentoOrdemResponse EnfileirarCancelamento(ExecutarCancelamentoOrdemRequest request)
        {
            ExecutarCancelamentoOrdemResponse response = new ExecutarCancelamentoOrdemResponse();

            string msg = "Cancelamento de Ordem enviado com sucesso";
            StatusRoteamentoEnum status = StatusRoteamentoEnum.Sucesso;

            response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaCancelamento(msg, status);

            queueCancelamento.Enqueue(request);

            return response;
        }

        /// <summary>
        /// Envia um pedido de cancel/replace para o canal correspondente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecutarModificacaoOrdensResponse ModificarOrdem(ExecutarModificacaoOrdensRequest request)
        {
            ExecutarModificacaoOrdensResponse response = new ExecutarModificacaoOrdensResponse();
            StatusRoteamentoEnum status = StatusRoteamentoEnum.Sucesso;
            string msg = "Modificacao de ordem enviada";
            CanalInfo _canal = null;

            logger.Debug("*** ModificarOrdem()");

            TradutorFix.DumpOrdemInfo(request.info);

            try
            {
                _canal = (CanalInfo)_canais[request.info.Exchange + request.info.ChannelID];

                if (_canal == null)
                {
                    msg = "Nao ha canal configurado para " + request.info.Exchange + "-" + request.info.ChannelID;
                    status = StatusRoteamentoEnum.Erro;
                    logger.Info(msg);
                    response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaModificacao(msg, status);

                }
                else
                {
                    if (_canal.roteador == null || _canal.Conectado == false)
                    {
                        status = StatusRoteamentoEnum.Erro;
                        msg = "Nao ha canal ativo e conectado para " + request.info.Exchange + "-" + request.info.ChannelID;
                        logger.Info(msg);
                        response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaModificacao(msg, status);
                    }
                    else
                    {
                        _criaReportStore(request.info.ClOrdID);

                        _notificaEnvioParaCanal(request.info);

                        response = _canal.roteador.ModificarOrdem(request);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "ModificarOrdem(): " + ex.Message;
                logger.Error(msg + "-" + ex.StackTrace);
                response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaModificacao(msg, status);

                if (_canal != null)
                {
                    _resetCanal(_canal);
                }
            }

            logger.Info(msg);

            return response;
        }

        /// <summary>
        /// Envia uma nova ordem cruzada (ordem com pontas de compra e venda ja informadas)
        /// </summary>
        /// <param name="request">objeto do tipo ExecutarOrdemCrossRequest</param>
        /// <returns>objeto do tipo ExecutarOrdemCrossResponse</returns>
        public ExecutarOrdemCrossResponse ExecutarOrdemCross(ExecutarOrdemCrossRequest request)
        {
            ExecutarOrdemCrossResponse response = new ExecutarOrdemCrossResponse();
            CanalInfo _canal = null;
            StatusRoteamentoEnum status = StatusRoteamentoEnum.Sucesso;
            string msg = "Ordem Enviada";

            logger.Debug("*** ExecutarOrdemCross()");

            logger.Debug("OrdemCross - Perna de compra");
            TradutorFix.DumpOrdemInfo(request.info.OrdemInfoCompra);
            logger.Debug("OrdemCross - Perna de venda");
            TradutorFix.DumpOrdemInfo(request.info.OrdemInfoVenda);

            try
            {
                _canal = (CanalInfo)_canais[request.info.Exchange + request.info.ChannelID];

                if (_canal == null)
                {
                    msg = "Nao ha canal configurado para " + request.info.Exchange + "-" + request.info.ChannelID;
                    status = StatusRoteamentoEnum.Erro;
                    logger.Info(msg);
                    response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaEOX(msg, status);
                }
                else
                {
                    if (_canal.roteador == null || _canal.Conectado == false)
                    {
                        status = StatusRoteamentoEnum.Erro;
                        msg = "Nao ha canal ativo e conectado para " + request.info.Exchange + "-" + request.info.ChannelID;
                        logger.Info(msg);
                        response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaEOX(msg, status);
                    }
                    else
                    {
                        _criaReportStore(request.info.OrdemInfoCompra.ClOrdID);
                        _criaReportStore(request.info.OrdemInfoVenda.ClOrdID);

                        _notificaEnvioParaCanal(request.info.OrdemInfoCompra);
                        _notificaEnvioParaCanal(request.info.OrdemInfoVenda);

                        response = _canal.roteador.ExecutarOrdemCross(request);

                    }
                }
            }
            catch (Exception ex)
            {
                msg = "Error ExecutarOrdemCross(): " + ex.Message;
                status = StatusRoteamentoEnum.Erro;
                logger.Error(msg + "-" + ex.StackTrace);
                response.DadosRetorno = RoteadorOrdensUtil.FormatarRespostaEOX(msg, status);

                if (_canal != null)
                {
                    _resetCanal(_canal);
                }
            }

            logger.Debug("*** End of ExecutarOrdem()");

            return response;
        }
        #endregion

        #region IRoteadorOrdensCallback
        /// <summary>
        /// 
        /// </summary>
        /// <param name="report"></param>
        public void OrdemAlterada(OrdemInfo report)
        {
            // Envia o report para os assinantes
            logger.Debug("On OrdemAlterada(): recebeu report ");

            TradutorFix.DumpOrdemInfo(report);

            lock (_executionsReportsSubscribers)
            {
                if (_executionsReportsSubscribers.Count > 0)
                {
                    _sendExecutionReport(report);
                }
                else
                {
                    _enqueueExecutionReport(report);
                }
            }


            // Reseta o contador do heartbeat
            lock (_canais)
            {
                if (_canais.ContainsKey(report.Exchange + report.ChannelID))
                {
                    CanalInfo canal = (CanalInfo)_canais[report.Exchange + report.ChannelID];
                    canal.Conectado = true;
                    canal.LastHeartbeat = _getSecsFromTicks(DateTime.Now.Ticks);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        public void StatusConexaoAlterada(StatusConexaoBolsaInfo status)
        {
            logger.Debug("On StatusConexaoAlterada(): recebeu status ");
 
            logger.Debug("Bolsa ..: " + status.Bolsa);
            logger.Debug("Operador ..: " + status.Operador);
            logger.Debug("Conectado .: " + status.Conectado.ToString());

            lock (_canais)
            {
                if (_canais.ContainsKey(status.Bolsa + status.Operador))
                {
                    CanalInfo canal = (CanalInfo) _canais[status.Bolsa + status.Operador];

                    canal.LastHeartbeat = _getSecsFromTicks(DateTime.Now.Ticks);
                    canal.Conectado = status.Conectado;
                    canal.roteador.Ping(new PingRequest());
                }
            }

            _sendConnectionStatus(status);
        }
        #endregion // IRoteadorOrdensCallback

        /// <summary>
        /// Monitor de conexoes aos canais de comunicacao
        /// </summary>
        private void RunMonitor()
        {
            logger.Info("Iniciando thread de monitoracao de canais");
            int _iMonitorConexoes = 0;

            while (_bKeepRunning)
            {
                // 4 * 250 = 1 segundo
                if (_iMonitorConexoes == 30 * 4)
                {
                    lock (_canais)
                    {
                        foreach (CanalInfo canal in _canais.Values)
                        {
                            // Se nao receber um Status info do canal em 45 segundos
                            // marca como desconectado
                            if ( (_getSecsFromTicks(DateTime.Now.Ticks) - canal.LastHeartbeat) > 45)
                            {
                                StatusConexaoBolsaInfo status = new StatusConexaoBolsaInfo();
                                status.Bolsa = canal.Exchange;
                                status.Operador = Convert.ToInt32(canal.ChannelID);
                                status.Conectado = canal.Conectado = false;

                                _resetCanal(canal);

                                _sendConnectionStatus(status);
                            }
                        }
                    }

                    _iMonitorConexoes = 0;
                }
                else
                {
                    _iMonitorConexoes++;
                }
                
                Thread.Sleep(250);
            }

            logger.Info("Thread de monitoracao de canais finalizada");
        }

        private void _carregarCanais()
        {
            // Carrega a tabela de canais
            foreach (CanalConfig canal in _config.Canais)
            {
                logger.Info("Carregando informação do canal: " + canal.Exchange );
                logger.Info("Porta ........................: " + canal.ChannelID);
                logger.Info("EndPointRoteader .............: " + canal.EndPointRoteador);
                logger.Info("EndPointAssinatura ...........: " + canal.EndPointAssinatura);
                logger.Info("EndPointRoteadorAdm ..........: " + canal.EndPointRoteadorAdm);

                CanalInfo info = new CanalInfo();

                info.ChannelID = canal.ChannelID;
                info.Exchange = canal.Exchange;
                info.RoteadorAddress = canal.EndPointRoteador;
                info.AssinaturaAddress = canal.EndPointAssinatura;
                info.RoteadorAdmAddress = canal.EndPointRoteadorAdm;

                info.assinatura = null;
                info.roteador = null;
                info.roteadorAdm = null;

                _canais.Add( info.Exchange+info.ChannelID, info);
            }

            logger.Info("Obtendo instancias de assinatura e envio de ordens");

            // Assina cada um dos eventos dos canais
            foreach (CanalInfo canal in _canais.Values)
            {
                try
                {
                    canal.roteador = RoteadorCanalComunic.GetChannel<IRoteadorOrdens>(canal.RoteadorAddress, null);
                    canal.assinatura = RoteadorCanalComunic.GetChannel<IAssinaturasRoteadorOrdensCallback>(canal.AssinaturaAddress, this);
                    canal.roteadorAdm = RoteadorCanalComunic.GetChannel<IRoteadorOrdensAdmin>(canal.RoteadorAdmAddress, null);

                    canal.assinatura.AssinarExecucaoOrdens(new AssinarExecucaoOrdemRequest());
                }
                catch (EndpointNotFoundException enfex)
                {
                    logger.Error("Nao pode conectar no canal" + canal.Exchange + "-" + canal.ChannelID + " [" + enfex.Message +"]");
                    canal.Conectado = false;
                }
            }

            logger.Info(_canais.Count + " canais carregados");
        }


        /// <summary>
        /// Envia o relatorio de execucao para os assinantes
        /// </summary>
        /// <param name="report">objeto OrdemInfo</param>
        private void _sendExecutionReport(OrdemInfo report)
        {
            // Verifica se deve ordenar e enviar os reports
            bool bOrdered = false;

            lock (hstOrderedReports)
            {
                bOrdered = hstOrderedReports.ContainsKey(report.ClOrdID);
            }

            if (bOrdered)
            {
                OrderReportStore store;
                lock (hstOrderedReports)
                {
                     store = (OrderReportStore)hstOrderedReports[report.ClOrdID];
                }

                switch (report.OrdStatus)
                {
                    case OrdemStatusEnum.ENVIADAPARAABOLSA:
                        store.addReport101(report);
                        break;
                    case OrdemStatusEnum.ENVIADAPARAOCANAL:
                        store.addReport102(report);
                        break;
                    default:
                        store.addExecutionReport(report);
                        break;
                }

                lock (hstOrderedReports)
                {
                    hstOrderedReports[report.ClOrdID] = store;
                }

                if (store.ShouldFlush)
                {
                    List<OrdemInfo> lista = store.GetOrderedReports();
                    foreach( OrdemInfo ordem in lista )
                    {
                        lock (queueReport)
                        {
                            queueReport.Enqueue(ordem);
                        }
                    }

                    lock (hstOrderedReports)
                    {
                        hstOrderedReports.Remove(report.ClOrdID);
                    }
                }
            }
            else
            {
                lock (queueReport)
                {
                    queueReport.Enqueue(report);
                }
            }

            semGoHorse.Release();
        }

        /// <summary>
        /// Envia o relatorio de execucao para os assinantes
        /// </summary>
        /// <param name="order">objeto OrdemInfo</param>
        private void _dispatchExecutionReport(OrdemInfo order)
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
                logger.Error("Erro em _sendExecutionReport(): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Envia o relatorio de execucao para os assinantes
        /// </summary>
        /// <param name="order">objeto OrdemInfo</param>
        private void _sendConnectionStatus(StatusConexaoBolsaInfo conexao)
        {
            lock (queueStatus)
            {
                queueStatus.Enqueue(conexao);
            }
            semGoHorse.Release();
        }

        /// <summary>
        /// Envia o relatorio de execucao para os assinantes
        /// </summary>
        /// <param name="order">objeto OrdemInfo</param>
        private void _dispatchConnectionStatus(StatusConexaoBolsaInfo conexao)
        {
            List<IRoteadorOrdensCallback> toDelete = new List<IRoteadorOrdensCallback>();

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



        /// <summary>
        /// Aborta um canal 
        /// </summary>
        /// <param name="canal"></param>
        private void _resetCanal(CanalInfo canal)
        {
            try
            {
                Ativador.AbortChannel(canal.roteador);
                Ativador.AbortChannel(canal.assinatura);
            }
            catch (Exception ex)
            {
                logger.Error("_resetCanal():" + ex.Message, ex);
            }

            canal.Conectado = false;

            try
            {
                logger.Info( "Canal: " + canal.Exchange + "-" + canal.ChannelID);

                canal.roteador = RoteadorCanalComunic.GetChannel<IRoteadorOrdens>(canal.RoteadorAddress, null);
                canal.assinatura = RoteadorCanalComunic.GetChannel<IAssinaturasRoteadorOrdensCallback>(canal.AssinaturaAddress, this);
                canal.roteadorAdm = RoteadorCanalComunic.GetChannel<IRoteadorOrdensAdmin>(canal.RoteadorAdmAddress, null);

                logger.Info( "Canal: " + canal.Exchange + "-" + canal.ChannelID + " assinando eventos de status");
                canal.assinatura.AssinarExecucaoOrdens(new AssinarExecucaoOrdemRequest());
                canal.assinatura.AssinarStatusConexaoBolsa(new AssinarStatusConexaoBolsaRequest());
            }
            catch (Exception ex)
            {
                logger.Error("_resetCanal():" + ex.Message, ex);
            }
        }


        /// <summary>
        /// Aborta um canal 
        /// </summary>
        /// <param name="canal"></param>
        private void _fecharCanais()
        {
            foreach( CanalInfo canal in _canais.Values )
            {
                try
                {
                    canal.Conectado = false;
                    Ativador.AbortChannel(canal.roteador);
                    Ativador.AbortChannel(canal.assinatura);
                }
                catch (Exception ex)
                {
                    logger.Error("_fecharCanais():" + ex.Message, ex);
                }
            }
        }


        /// <summary>
        /// Converte DateTime.Ticks em segundos
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        private long _getSecsFromTicks(long ticks)
        {
            // From fucking MSDN:
            //A single tick represents one hundred nanoseconds or one
            //ten-millionth of a second. There are 10,000 ticks in a millisecond. 
            return ticks/10000/1000;
        }


        /// <summary>
        /// Enfileira e persiste os relatorios de execucao
        /// </summary>
        /// <param name="info">OrdemInfo object</param>
        private void _enqueueExecutionReport(OrdemInfo info)
        {
            lock (_orderReportInfo)
            {
                logger.Info("Ordem " + info.ClOrdID + " inserida na fila de respostas");

                _orderReportInfo.Enqueue(info);
            }

        }


        /// <summary>
        /// Cria a classe para ordenar corretamente os reports
        /// </summary>
        /// <param name="clordid">id da ordem</param>
        private void _criaReportStore(string clordid)
        {
            // Cria a classe para ordenar corretamente os reports
            OrderReportStore store;
            if (hstOrderedReports.ContainsKey(clordid))
            {
                store = (OrderReportStore)hstOrderedReports[clordid];
            }
            else
            {
                store = new OrderReportStore();
                hstOrderedReports.Add(clordid, store);
            }

            store.Reset();
        }

        private void _notificaEnvioParaCanal(OrdemInfo ordem)
        {
            AcompanhamentoOrdemInfo acompanhamento = new AcompanhamentoOrdemInfo();

            if (ordem.Acompanhamentos != null && ordem.Acompanhamentos.Count > 0)
            {
                ordem.Acompanhamentos.Clear();
            }

            acompanhamento.CodigoDoCliente = ordem.Account;
            acompanhamento.CanalNegociacao = ordem.ChannelID;

            acompanhamento.Instrumento = ordem.Symbol;
            acompanhamento.NumeroControleOrdem = ordem.ClOrdID;
            acompanhamento.Preco = Convert.ToDecimal(ordem.Price);
            acompanhamento.StatusOrdem = OrdemStatusEnum.ENVIADAPARAOCANAL;
            acompanhamento.DataAtualizacao = DateTime.Now;
            acompanhamento.QuantidadeSolicitada = ordem.OrderQty;
            acompanhamento.Descricao = "Ordem enviada para o servico de canal";

            ordem.Acompanhamentos.Add(acompanhamento);
            ordem.OrdStatus = OrdemStatusEnum.ENVIADAPARAOCANAL;

            _sendExecutionReport(ordem);
        }

                /// <summary>
        /// Monitor de conexoes aos canais de comunicacao
        /// </summary>
        private void RunInfoDispacher()
        {
            logger.Info("Iniciando thread de despacho de status");

            while (_bKeepRunning)
            {
                semGoHorse.WaitOne(250);
                
                OrdemInfo[] arrInfo = null;
                StatusConexaoBolsaInfo[] arrStatus = null;

                // Obtem e envia os reports pendentes de despacho
                lock (queueReport)
                {
                    if (queueReport.Count > 0)
                    {
                        arrInfo = queueReport.ToArray();
                        queueReport.Clear();
                    }
                }


                if (arrInfo != null && arrInfo.Length > 0)
                {
                    foreach (OrdemInfo info in arrInfo)
                    {
                        _dispatchExecutionReport(info);
                    }
                }

                // Obtem e envia os status de conexao pendentes de despacho
                lock (queueStatus)
                {
                    if (queueStatus.Count > 0)
                    {
                        arrStatus = queueStatus.ToArray();
                        queueStatus.Clear();
                    }
                }


                if (arrStatus != null && arrStatus.Length > 0)
                {
                    foreach (StatusConexaoBolsaInfo info in arrStatus)
                    {
                        _dispatchConnectionStatus(info);
                    }
                }
            }
        }


        private void ProcessadorCancelamentos()
        {
            logger.Info("Inicio da thread de processamento de cancelamentos");
            
            while (_bKeepRunning)
            {
                ExecutarCancelamentoOrdemRequest request;
                if (queueCancelamento.TryDequeue(out request))
                {
                    CanalInfo _canal = (CanalInfo)_canais[request.info.Exchange + request.info.ChannelID];

                    _canal.roteador.CancelarOrdem(request);

                    continue;
                }

                Thread.Sleep(100);
            }

            logger.Info("Final da thread de processamento de cancelamentos");
        }

		// Some kind of main routine
        //void Run()
        //{
        //    (...)
        //    foreach( IRoteadorOrdensCallback subscriber in __executionsReportsSubscribers )
        //    {
        //        if (Ativador.IsValidChannel(subscriber))
        //        {
        //            subscriber.OnOrderExecution( executionReportObject.... );
        //        }
        //    }
        //}
		
        //void OnExchangeDisconection()
        //{
        //    (...)
        //    foreach( IRoteadorOrdensCallback subscriber in __executionsReportsSubscribers )
        //    {
        //        if (Ativador.IsValidChannel(subscriber))
        //        {
        //            subscriber.OnExchangeStatus( executionReportObject.... );
        //        }
        //    }
        //}


        #region IRoteadorOrdensAdmin
        public FixTestResponse ExecutarFixTest( FixTestRequest request)
        {
            throw new NotImplementedException();
        }

        public FixResendResponse ExecutarFixResend( FixResendRequest request)
        {
            FixResendResponse response = new FixResendResponse();
            CanalInfo _canal = null;
            StatusRoteamentoEnum status = StatusRoteamentoEnum.Sucesso;
            string msg = "Ordem Enviada";

            logger.Debug("*** ExecutarFixResend()");

            try
            {
                _canal = (CanalInfo)_canais[request.Bolsa + request.Canal];

                if (_canal == null)
                {
                    msg = "Nao ha canal configurado para " + request.Bolsa + "-" + request.Canal;
                    status = StatusRoteamentoEnum.Erro;
                    logger.Info(msg);
                    response.DadosRetorno = new DadosAdmRetornoBase();
                    response.DadosRetorno.DataResposta = DateTime.Now;
                    response.DadosRetorno.Erro = true;
                    response.DadosRetorno.Ocorrencias.Add(msg);
                }
                else
                {
                    if (_canal.roteador == null || _canal.Conectado == false)
                    {
                        status = StatusRoteamentoEnum.Erro;
                        msg = "Nao ha canal conectado para " + request.Bolsa + "-" + request.Canal;
                        logger.Info(msg);
                        response.DadosRetorno = new DadosAdmRetornoBase();
                        response.DadosRetorno.DataResposta = DateTime.Now;
                        response.DadosRetorno.Erro = true;
                        response.DadosRetorno.Ocorrencias.Add(msg);
                    }
                    else
                    {
                        response = _canal.roteadorAdm.ExecutarFixResend(request);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error ExecutarFixResend():" + ex.Message, ex);

                msg = "Error ExecutarFixResend(): " + ex.Message;
                status = StatusRoteamentoEnum.Erro;
                response.DadosRetorno = new DadosAdmRetornoBase();
                response.DadosRetorno.DataResposta = DateTime.Now;
                response.DadosRetorno.Erro = true;
                response.DadosRetorno.Ocorrencias.Add(msg);


                if (_canal != null)
                {
                    _resetCanal(_canal);
                }
            }

            logger.Debug("*** End of ExecutarOrdem()");

            return response;
        }

        public FixSequenceResetResponse ExecutarFixSequenceReset( FixSequenceResetRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion //IRoteadorOrdensAdmin
    }
}
