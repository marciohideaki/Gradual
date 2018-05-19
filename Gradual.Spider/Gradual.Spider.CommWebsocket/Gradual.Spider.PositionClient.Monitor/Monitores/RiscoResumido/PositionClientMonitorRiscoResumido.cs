using Gradual.OMS.Library.Servicos;
using Gradual.Spider.PositionClient.Monitor.Lib;
using Gradual.Spider.PositionClient.Monitor.Lib.Message;
using Gradual.Spider.PositionClient.Monitor.Monitores.RiscoResumido;
using Gradual.Spider.SupervisorRisco.Lib.Dados;
using log4net;
using Newtonsoft.Json;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketEngine;
using SuperWebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.ServiceModel.Web;
using Cors;
namespace Gradual.Spider.PositionClient.Monitor
{
    /// <summary>
    /// Classe responsável pelo Broadcast, monitoração e consumo de socket de postion client com os resumos de 
    /// monitoramento de risco dos clientes
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PositionClientMonitorRiscoResumido: IServicoControlavel, IPositionClientRiscoResumido
    {
        #region Atributos
        /// <summary>
        /// Atributo responsável por montar as configurações de websocket configuradas no *.config
        /// </summary>
        private static IBootstrap _Bootstrap = null;

        /// <summary>
        /// Atributo que sinaliza se o serviço de monitaramento está ativo ou não.
        /// </summary>
        private bool _KeepRunning = false;

        /// <summary>
        /// Atributo responsável pela log da classe
        /// </summary>
        private static readonly log4net.ILog _Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Atributo da classe de socket responsável pela conexão e consumo do serviço de 
        /// socket do Postion Client
        /// </summary>
        private static PositionClientSocketRiscoResumido _Socket = null;

        /// <summary>
        /// Atributo da classe que trata assinaturas de novos clientes solicitadas pelo Websocket-Cliente.   
        /// Instancia da classe para delegar e disparar eventos.
        /// </summary>
        //private SUBSCRIBE _CommandAssemblySUBSCRIBE = null;

        /// <summary>
        /// Atributo de classe de controle do web socket server.
        /// </summary>
        private static WebSocketServer _WebSocketServer = null;

        /// <summary>
        /// Atributo que identifica se o serviço está para rodando ou não.
        /// </summary>
        private ServicoStatus _ServicoStatus;

        /// <summary>
        /// Thread de controle de enfileiramento de mensagem de posições de clientes.
        /// </summary>
        private Thread _ThreadQueueSendRiscoResumido = null;

        /// <summary>
        /// Atributo publico que armazena uma lista de  Session message position Client.
        /// </summary>
        public ConcurrentQueue<SessionMessagePostionClient> _QueueSessionMessagesRiscoResumido = new ConcurrentQueue<SessionMessagePostionClient>();

        /// <summary>
        /// Singleton da classe usado para controle de lock
        /// </summary>
        private object _Singleton = new object();

        /// <summary>
        /// Singleton  da classe para ser como instancia em outras classes externas
        /// </summary>
        private static PositionClientMonitorRiscoResumido _instance = null;

        /// <summary>
        /// WebService Sel HOsting para HOstiar as chamadas REST 
        /// </summary>
        private WebServiceHost _SelfHost = null;
        #endregion

        #region Propriedades
        /// <summary>
        /// Propriedades do bootstrap
        /// </summary>
        public IBootstrap Bootstrap
        {
            get
            {
                return _Bootstrap;
            }
        }

        #endregion

        #region Construtores
        /// <summary>
        /// Construtor da classe 
        /// </summary>
        public PositionClientMonitorRiscoResumido()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        #region Métodos
        public void IniciarServico()
        {
            try
            {
                _Logger.InfoFormat("Iniciando o Serviço de Position Client via WebSocket");

                _Bootstrap = BootstrapFactory.CreateBootstrap();

                if (!_Bootstrap.Initialize())
                {
                    //_Logger.Error("Falha ao iniciar o bootstrap!");

                    throw new Exception("Falha ao iniciar o bootstrap!");

                    return;
                }

                var lResult = _Bootstrap.Start();

                if (lResult == StartResult.Failed)
                {
                    _Logger.Error("Falha no start do bootstrap!");
                }

                //_CommandAssemblySUBSCRIBE = new SUBSCRIBE();

                _Socket = PositionClientSocketRiscoResumido.Instance;// new PositionClientPackageSocket();

                _Socket.IpAddr = ConfigurationManager.AppSettings["ASConnPositionClientIp"].ToString();
                _Socket.Port = Convert.ToInt32(ConfigurationManager.AppSettings["ASConnPositionClientPort"].ToString());

                //_Socket.OpenConnection();

                var lSubscriptions = new List<int>();

                lSubscriptions.Add(14);

                _Socket.StartClientConnect(lSubscriptions);

                var socketServer = _Bootstrap.AppServers.FirstOrDefault(s => s.Name.Equals("SuperWebSocket")) as WebSocketServer;

                socketServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(socketServer_NewMessageReceived);

                socketServer.NewSessionConnected += new SessionHandler<WebSocketSession>(socketServer_NewSessionConnected);

                _WebSocketServer = socketServer;

                _ThreadQueueSendRiscoResumido = new Thread(new ThreadStart(QueueSendMessageRiscoResumido));

                _ThreadQueueSendRiscoResumido.Name = "ThreadQueueSendPositionClient";

                _ThreadQueueSendRiscoResumido.Start();

                _KeepRunning = true;

                _ServicoStatus = ServicoStatus.EmExecucao;

                _Socket.SendMessageClientConnected += SendMessageRiscoResumidoToQueue;

                //_CommandAssemblySUBSCRIBE.SendMessageClientConnected += SendMessageMemoryPositionClientToQueue;

                string lSelfHost = ConfigurationManager.AppSettings["RestRiscoResumido"].ToString();

                _SelfHost = new WebServiceHost(typeof(RestRiscoResumido), new Uri(lSelfHost));

                var lEndPoint = _SelfHost.AddServiceEndpoint(typeof(IServicoRestRiscoResumido), new WebHttpBinding(WebHttpSecurityMode.None), "");

                lEndPoint.Behaviors.Add(new CorsBehaviorAttribute());

                foreach (var operation in lEndPoint.Contract.Operations)
                {
                    //add support for cors (and for operation to be able to not  
                    //invoke the operation if we have a preflight cors request)  
                    operation.Behaviors.Add(new CorsBehaviorAttribute());
                }

                try
                {
                    _Logger.Info("Tetando iniciar Self Hosting  do WebServiceHost");

                    // Step 2 Start the service.
                    _SelfHost.Open();

                    _Logger.Info("Self Hosting iniciado com sucesso WebServiceHost");

                }
                catch (CommunicationException ex)
                {
                    Console.WriteLine("An exception occurred: {0}", ex.Message);

                    _SelfHost.Abort();
                }

                _Logger.Info("Servico de Position Client via WebSocket iniciado com sucesso");
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado ao iniciar serviço de WebSocketServer", ex);

                throw (ex);
            }
        }

        /// <summary>
        /// Evento que é disparado quando chega um mensagem de Risco Resumido. O método
        /// varre as sessions que estão ativas no socket e verifica se está cliente da
        /// mensagem Está com com conexão de socket aberto para enviar a session para o
        /// enfileiramento da classe.
        /// </summary>
        /// <param name="sender">Classe que está chamando o evento, normalmente a classe
        /// PositionClientSocketRiscoResumido</param>
        /// <param name="e">Event Args com a classe com o conteúdo a ser enviado ao
        /// aplicativo conectado via socket</param>
        [System.ComponentModel.Description("Evento que é disparado quando chega um mensagem de Risco Resumido. O método varre as sessions que estão ativas no socket e verifica se está cliente da mensagem Está com com conexão de socket aberto para enviar a session para o enfileiramento da classe.")]
        private void SendMessageRiscoResumidoToQueue(object sender, MessageRiscoResumidoArgs e)
        {
            try
            {
                var lSessions = _WebSocketServer.GetAllSessions();

                if (lSessions == null) return;

                foreach (var session in lSessions)
                {
                    if (session.lRiscoResumidoRequest == null)
                    {
                        continue;
                    }

                    if (session.lRiscoResumidoRequest.CodigoCliente == 0)
                    {
                        continue;
                    }
                   
                     _Logger.InfoFormat("Numero de Sessoes conectadas {0}", lSessions.Count());

                     bool lVazia = false;

                    foreach ( KeyValuePair <int,ConsolidatedRiskInfo> info in e.Message)
                    {
                        var lPosicao = info.Value;

                        if (TrataFiltroEfetuado(session, lPosicao))
                        {
                            var lSession = new SessionMessagePostionClient();

                            lSession.Session = session;

                            lSession.MessageString = JsonConvert.SerializeObject(lPosicao);

                            lVazia = _QueueSessionMessagesRiscoResumido.IsEmpty;

                            _QueueSessionMessagesRiscoResumido.Enqueue(lSession);
                        }

                        if (lVazia)
                        {
                            lock (_Singleton)
                            {
                                System.Threading.Monitor.Pulse(_Singleton);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no método SendMessagePositionClientToQueue() -> ", ex);
            }
        }

        /// <summary>
        /// Evento que busca as posição do cliente na memória (Dictionary) e 
        /// envia a mensagem para a enfileiramento Thread de envio de mensagens. 
        /// </summary>
        /// <param name="sender">Classe que está requisitando o evento</param>
        /// <param name="e">Classe preenchida com os dados de Risco Resumido como argumento para ser 
        /// tratado e enviado para a Fila de Posições a ser enviado</param>
        public void SendMessageMemoryRiscoResumidoToQueue(object sender, MessageRiscoResumidoArgs e)
        {
            try
            {
                _Logger.InfoFormat("Usuário solicitou a assinatura do cliente: {0}", e.CodigoCliente);

                var lDicConsolidatedRisk = _Socket.DicConsolidatedRisk;

                if (e.Session.lRiscoResumidoRequest == null)
                {
                    return;
                }

                if (e.Session.lRiscoResumidoRequest.CodigoCliente == 0)
                {
                    return;
                }

                foreach (KeyValuePair<int, ConsolidatedRiskInfo> Info in lDicConsolidatedRisk)
                {
                    var lPosicao = Info.Value;

                    if (TrataFiltroEfetuado(e.Session, lPosicao))
                    {
                        var lSession = new SessionMessagePostionClient();

                        lSession.Session = e.Session;

                        lSession.MessageString = JsonConvert.SerializeObject(lPosicao);
                        
                        _QueueSessionMessagesRiscoResumido.Enqueue(lSession);

                        _Logger.InfoFormat("SendMessageMemoryRiscoResumidoToQueue -> Position Clients enfileirados {0}", _QueueSessionMessagesRiscoResumido.Count);
                    }

                    bool lVazia = _QueueSessionMessagesRiscoResumido.IsEmpty;

                    if (lVazia)
                    {
                        lock (_Singleton)
                        {
                            System.Threading.Monitor.Pulse(_Singleton);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no método SendMessageMemoryPositionClientToQueue() -> ", ex);
            }
        }

        /// <summary>
        /// Método que desenfileira a mensagem de postion client da fila e 
        /// envia a mensagem para as sessões conectadas
        /// </summary>
        private void QueueSendMessageRiscoResumido()
        {
            _Logger.Info("Entrou na função de enviar mensagens de Risco Resumido para as sessões conectadas");

            while (_KeepRunning || !_QueueSessionMessagesRiscoResumido.IsEmpty)
            {
                try
                {
                    SessionMessagePostionClient lSessionMessage;

                    if (_QueueSessionMessagesRiscoResumido.TryPeek(out lSessionMessage))
                    {
                        lSessionMessage.Session.Send(lSessionMessage.MessageString);

                        while (!_QueueSessionMessagesRiscoResumido.TryDequeue(out lSessionMessage)) ;

                        _Logger.InfoFormat("Quantidade na fila: [{0}]", _QueueSessionMessagesRiscoResumido.Count);

                        continue;
                    }

                    lock (_Singleton)
                    {
                        System.Threading.Monitor.Wait(_Singleton, 150);
                    }
                }
                catch (Exception ex)
                {
                    _Logger.Error("Erro encontrado no método QueueSendMessagePositionClient() -> ", ex);
                }
            }
        }

        /// <summary>
        /// Evento que é disparado quando uma nova sessão(Cliente) conecta no serviço
        /// </summary>
        /// <param name="session">Session que acabou de conectar</param>
        private void socketServer_NewSessionConnected(WebSocketSession session)
        {
            try
            {
                //session.

                //session.ListClientSubscriptions.Add(session.Items)
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no evento socketServer_NewSessionConnected ->", ex);

                throw (ex);
            }
        }

        /// <summary>
        /// Evento que é disparado quando chega uma requisição do cliente(usuário) 
        /// conectado no socket
        /// </summary>
        /// <param name="session">Session que acabou de mandar uma mensagem na sessão</param>
        /// <param name="value">Mensagem armazenada no </param>
        private void socketServer_NewMessageReceived(WebSocketSession session, string value)
        {
            try
            {
                var lMensagemSplit = value.Split(' ');

                var lTipo = lMensagemSplit[0];

                //int lClienteInt;

                //var lCliente = int.TryParse(lMensagemSplit[1], out lClienteInt);

                BuscarRiscoResumidoSocketRequest lRequest = JsonConvert.DeserializeObject(lMensagemSplit[1], typeof(BuscarRiscoResumidoSocketRequest)) as BuscarRiscoResumidoSocketRequest;

                if (lTipo == "SUBSCRIBE")
                {
                    //if (!session.ListClientSubscriptions.Contains(lClienteInt))
                    //{
                    //    session.ListClientSubscriptions.Add(lClienteInt);

                    var e = new MessageRiscoResumidoArgs();

                    e.Session = session;

                    e.Session.lRiscoResumidoRequest = lRequest;

                    SendMessageMemoryRiscoResumidoToQueue(this, e);
                    //}
                }
                else if (lTipo == "UNSUBSCRIBE")
                {
                    session.lRiscoResumidoRequest = new BuscarRiscoResumidoSocketRequest();
                    //if (session.ListClientSubscriptions.Contains(lClienteInt))
                    //{
                    //    session.ListClientSubscriptions.Remove(lClienteInt);
                    //}
                }
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no evento socketServer_NewMessageReceived ->", ex);

                throw (ex);
            }
        }

        /// <summary>
        /// Método que para o serviço de monitoramento e consumo de position client(Risco).
        /// Método padrão da Dll Gradual.OMS.Library que é acionado quando o serviço Está sendo parado.
        /// </summary>
        public void PararServico()
        {
            _Logger.Info("Parando o servico de Monitoramento e consumo do Position Client Risco Resumido");

            _ServicoStatus = ServicoStatus.Parado;

            _Logger.Info("Servico parado com sucesso.");

            //Parando o servidor bootstrap
            _Bootstrap.Stop();

            //Parando serviço de HOsting do REST
            _SelfHost.Close();

            while (_ThreadQueueSendRiscoResumido.IsAlive)
            {
                _Logger.Info("Aguardando finalizar thThreadCarregarMonitorMemoria");
                Thread.Sleep(250);
                _ThreadQueueSendRiscoResumido.Abort();
            }
        }

        /// <summary>
        /// Método que retorna o status do serviço.
        /// </summary>
        /// <returns>Retorna o status do serviço de Monitoramento do Risco Resumido</returns>
        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        /// <summary>
        /// Método que realiza a busca do objeto de Risco Resumido armazenado na memória para 
        /// ser retornado a aplicação cliente conectada
        /// </summary>
        /// <param name="lRequest">Dados de request com o filtro configurado pelo cliente</param>
        /// <returns>Retorna o objeto de Risco Resumido filtrado pelas propriedades do request </returns>
        public BuscarRiscoResumidoResponse BuscarRiscoResumido(BuscarRiscoResumidoRequest lRequest)
        {
            _Logger.InfoFormat("Buscando informações de Risco Resumido com o filtro de Cliente: {0}, OpcaoPL: {1}, SPF Atingido: {2}, Prejuízo Atingido: {3}",
                lRequest.CodigoCliente,
                lRequest.OpcaoPL,
                lRequest.OpcaoPrejuizoAtingido,
                lRequest.OpcaoSFPAtingido);

            _Logger.InfoFormat("Quantidade de Posições no DicConsolidatedRisk {0}", _Socket.DicConsolidatedRisk.Count);

            var lRetorno = new BuscarRiscoResumidoResponse();

            try
            {
                var lList = new List<ConsolidatedRiskInfo>();

                var lDic = new ConcurrentDictionary<int, ConsolidatedRiskInfo>();

                lList.AddRange(_Socket.DicConsolidatedRisk.Values);

                var lFiltrado = from a in lList select a;

                if (lRequest.CodigoCliente != 0)
                {
                    lFiltrado = from a in lFiltrado where a.Account == lRequest.CodigoCliente select a;
                }

                //Opção de PL negativo ou com lucro
                if ((lRequest.OpcaoPL & Lib.Dados.OpcaoPL.SomenteComLucro) .Equals(Lib.Dados.OpcaoPL.SomenteComLucro))
                {
                    lFiltrado = from a in lFiltrado where a.PLTotal > 0 select a;

                }else if ((lRequest.OpcaoPL & Lib.Dados.OpcaoPL.SomentePLnegativo).Equals(Lib.Dados.OpcaoPL.SomentePLnegativo))
                {
                    lFiltrado = from a in lFiltrado where a.PLTotal < 0 select a;
                }

                //Opção de SFP Atingido
                if((lRequest.OpcaoSFPAtingido & Lib.Dados.OpcaoSFPAtingido.Ate25).Equals( Lib.Dados.OpcaoSFPAtingido.Ate25))
                {
                    lFiltrado = from a in lFiltrado where a.TotalPercentualAtingido < 25 select a;
                }

                if((lRequest.OpcaoSFPAtingido & Lib.Dados.OpcaoSFPAtingido.Entre25e50).Equals( Lib.Dados.OpcaoSFPAtingido.Entre25e50))
                {
                    lFiltrado = from a in lFiltrado where (a.TotalPercentualAtingido > 25 && a.TotalPercentualAtingido < 50  ) select a;
                }

                if ((lRequest.OpcaoSFPAtingido & Lib.Dados.OpcaoSFPAtingido.Entre50e75).Equals(Lib.Dados.OpcaoSFPAtingido.Entre50e75))
                {
                    lFiltrado = from a in lFiltrado where (a.TotalPercentualAtingido > 50 && a.TotalPercentualAtingido < 75 ) select a;
                }

                if ((lRequest.OpcaoSFPAtingido & Lib.Dados.OpcaoSFPAtingido.Acima75).Equals(Lib.Dados.OpcaoSFPAtingido.Acima75))
                {
                    lFiltrado = from a in lFiltrado where ( a.TotalPercentualAtingido > 75 ) select a;
                }

                //Prejuízo Atingido
                if ((lRequest.OpcaoPrejuizoAtingido & Lib.Dados.OpcaoPrejuizoAtingido.Ate2K).Equals(Lib.Dados.OpcaoPrejuizoAtingido.Ate2K))
                {
                    lFiltrado = from a in lFiltrado where (a.PLTotal <= 2000) select a;
                }

                if ((lRequest.OpcaoPrejuizoAtingido & Lib.Dados.OpcaoPrejuizoAtingido.Entre2Ke5K).Equals(Lib.Dados.OpcaoPrejuizoAtingido.Entre2Ke5K))
                {
                    lFiltrado = from a in lFiltrado where (a.PLTotal >= 2000 && a.PLTotal <= 5000 ) select a;
                }

                if ((lRequest.OpcaoPrejuizoAtingido & Lib.Dados.OpcaoPrejuizoAtingido.Entre5Ke10K).Equals(Lib.Dados.OpcaoPrejuizoAtingido.Entre5Ke10K))
                {
                    lFiltrado = from a in lFiltrado where (a.PLTotal >= 10000 && a.PLTotal <= 20000) select a;
                }

                if ((lRequest.OpcaoPrejuizoAtingido & Lib.Dados.OpcaoPrejuizoAtingido.Entre10Ke20K).Equals(Lib.Dados.OpcaoPrejuizoAtingido.Entre10Ke20K))
                {
                    lFiltrado = from a in lFiltrado where (a.PLTotal >= 10000 && a.PLTotal <= 20000) select a;
                }

                if ((lRequest.OpcaoPrejuizoAtingido & Lib.Dados.OpcaoPrejuizoAtingido.Entre20Ke50K).Equals(Lib.Dados.OpcaoPrejuizoAtingido.Entre20Ke50K))
                {
                    lFiltrado = from a in lFiltrado where (a.PLTotal >= 20000 && a.PLTotal <= 50000) select a;
                }

                if ((lRequest.OpcaoPrejuizoAtingido & Lib.Dados.OpcaoPrejuizoAtingido.Acima50K).Equals(Lib.Dados.OpcaoPrejuizoAtingido.Acima50K))
                {
                    lFiltrado = from a in lFiltrado where (a.PLTotal >= 50000) select a;
                }

                lRetorno.ListRiscoResumido = lFiltrado.ToList();

                _Logger.InfoFormat("Foram Encontrados {0} itens de Risco Resumido", lRetorno.ListRiscoResumido.Count);
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no método BuscarRiscoResumido", ex);
            }

            return lRetorno;

        }

        /// <summary>
        /// Método que trata Filtro Efetuado pela aplicação de objetos de Risco Resumido
        /// </summary>
        /// <param name="pSession"></param>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public bool TrataFiltroEfetuado(WebSocketSession pSession, ConsolidatedRiskInfo pInfo)
        {
            try
            {
                if (pSession.lRiscoResumidoRequest == null)
                    return false;

                var lFiltro = pSession.lRiscoResumidoRequest;

                if (lFiltro.CodigoCliente != 0 && (pInfo.Account != lFiltro.CodigoCliente))
                {
                    return false;
                }

                if (lFiltro.OpcaoPLSomenteComLucro && pInfo.PLTotal >= 0)
                {
                    return true;
                }

                if (lFiltro.OpcaoPLSomentePLnegativo && pInfo.PLTotal < 0)
                {
                    return true;
                }

                if (lFiltro.OpcaoPrejuizoAtingidoAcima50K && pInfo.PLTotal < (-50000))
                {
                    return true;
                }

                if (lFiltro.OpcaoPrejuizoAtingidoAte2K && (pInfo.PLTotal >= (-2000) && pInfo.PLTotal < 0))
                {
                    return true;
                }

                if (lFiltro.OpcaoPrejuizoAtingidoEntre10Ke20K && (pInfo.PLTotal >= (-20000) && pInfo.PLTotal <= (-10000) ))
                {
                    return true;
                }

                if (lFiltro.OpcaoPrejuizoAtingidoEntre20Ke50K && (pInfo.PLTotal >= (-50000) && pInfo.PLTotal <= (-20000) ))
                {
                    return true;
                }

                if (lFiltro.OpcaoPrejuizoAtingidoEntre2Ke5K && (pInfo.PLTotal >= (-5000) && pInfo.PLTotal <= (-2000) ))
                {
                    return true;
                }

                if (lFiltro.OpcaoPrejuizoAtingidoEntre5Ke10K && (pInfo.PLTotal >= (-10000) && pInfo.PLTotal <= (-5000) ))
                {
                    return true;
                }

                if (lFiltro.OpcaoSFPAtingidoAte25 && (pInfo.TotalPercentualAtingido >= (-25) && pInfo.TotalPercentualAtingido < 0) )
                {
                    return true;
                }

                if (lFiltro.OpcaoSFPAtingidoEntre25e50 && (pInfo.TotalPercentualAtingido <= (-25) && pInfo.TotalPercentualAtingido <= 50))
                {
                    return true;
                }

                if (lFiltro.OpcaoSFPAtingidoEntre50e75 && (pInfo.TotalPercentualAtingido <= (-50) && pInfo.TotalPercentualAtingido <= 75))
                {
                    return true;
                }

                if (lFiltro.OpcaoSFPAtingidoAcima75 && pInfo.TotalPercentualAtingido <= (-75))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no método de tratamento TrataFiltroEfetuado -> ", ex);
            }

            return true;
        }
        #endregion
    }
}
