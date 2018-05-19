using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Configuration;
using System.Globalization;
using System.ServiceModel;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.WebSocket.Lib;
using SuperSocket.SocketBase;
using SuperSocket.Common;
using SuperSocket.SocketEngine;
using SuperWebSocket;
using System.Threading;
using System.Collections.Concurrent;
using Gradual.Spider.PositionClient.Monitor.Transporte;
using Gradual.Spider.PositionClient.Monitor;
using Newtonsoft.Json;
using Gradual.Spider.DataSync.Lib.Mensagens;
//using Gradual.Spider.PositionClient.CommandAssembly;
using Gradual.Spider.PositionClient.Monitor.Lib.Message;
using Gradual.Spider.PositionClient.Monitor.Lib;
using Gradual.Spider.SupervisorRisco.Lib.Dados;
using Gradual.Spider.PositionClient.Monitor.Utils;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using Gradual.Spider.PositionClient.Monitor.Monitores.OperacoesIntraday;
using Gradual.Spider.PositionClient.Monitor.Lib.Dados;
using System.ServiceModel.Description;
using Cors;
using System.ServiceModel.Web;


namespace Gradual.Spider.PositionClient.Monitor
{
    /// <summary>
    /// Classe responsável pelo Broadcast, monitoração e consumo de socket de postion client com os resumos de 
    /// monitoramento de risco dos clientes
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    [AspNetCompatibilityRequirements(RequirementsMode=  AspNetCompatibilityRequirementsMode.Allowed)]
    public class PositionClientMonitor : IServicoControlavel, IServicoWebSocket, IServicoPositionClientMonitor
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
        private static readonly ILog _Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Atributo da classe de socket responsável pela conexão e consumo do serviço de 
        /// socket do Postion Client
        /// </summary>
        private static PositionClientPackageSocket _Socket = null; 

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
        private Thread _ThreadQueueSendPositionClient = null;

        /// <summary>
        /// Atributo publico que armazena uma lista de  Session message position Client.
        /// </summary>
        public ConcurrentQueue<SessionMessagePostionClient> _QueueSessionMessagesPositionClients = new ConcurrentQueue<SessionMessagePostionClient>();

        /// <summary>
        /// Singleton da classe usado para controle de lock
        /// </summary>
        private object _Singleton = new object();

        /// <summary>
        /// Singleton  da classe para ser como instancia em outras classes externas
        /// </summary>
        private static PositionClientMonitor _instance = null;

        /// <summary>
        /// WebService Sel HOsting para HOstiar as chamadas REST 
        /// </summary>
        private WebServiceHost _SelfHost = null;
        #endregion

        #region Propriedades
        /// <summary>
        /// Bootstrap da conexão de socket
        /// </summary>
        public IBootstrap Bootstrap
        {
            get
            {
                return _Bootstrap;
            }
        }

        /// <summary>
        /// Singleton da classe para instancia em outras classes externas
        /// </summary>
        public static PositionClientMonitor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (PositionClientMonitor)OperationContext.Current.InstanceContext.GetServiceInstance();
                }

                return _instance;
            }
        }
        #endregion

        #region Construtores
        /// <summary>
        /// Construtor da classe 
        /// </summary>
        public PositionClientMonitor()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        /// <summary>
        /// Método que inicia o serviço de monitoramento e consumo de Position Client(Risco)
        /// Método padrão da Gradual.OMS.Library e é acionado sempre que o 
        /// serviço inicializa.
        /// </summary>
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

                _Socket = PositionClientPackageSocket.Instance;// new PositionClientPackageSocket();
                
                _Socket.IpAddr = ConfigurationManager.AppSettings["ASConnPositionClientIp"].ToString();
                _Socket.Port   = Convert.ToInt32(ConfigurationManager.AppSettings["ASConnPositionClientPort"].ToString());

                //_Socket.OpenConnection();
                var lSubscriptions = new List<int>();

                lSubscriptions.Add(14);

                _Socket.StartClientConnect(lSubscriptions);

                var socketServer = _Bootstrap.AppServers.FirstOrDefault(s => s.Name.Equals("SuperWebSocket")) as WebSocketServer;

                socketServer.NewMessageReceived  += new SessionHandler<WebSocketSession,string>(socketServer_NewMessageReceived);

                socketServer.NewSessionConnected += new SessionHandler<WebSocketSession>(socketServer_NewSessionConnected);

                _WebSocketServer = socketServer;

                _Socket.SendMessageClientConnected += SendMessagePositionClientToQueue;

                _ThreadQueueSendPositionClient = new Thread(new ThreadStart(QueueSendMessagePositionClient));

                _ThreadQueueSendPositionClient.Name = "ThreadQueueSendPositionClient";

                _ThreadQueueSendPositionClient.Start();

                _KeepRunning                        = true;

                _ServicoStatus = ServicoStatus.EmExecucao;

                //_CommandAssemblySUBSCRIBE.SendMessageClientConnected += SendMessageMemoryPositionClientToQueue;

                string lSelfHost = ConfigurationManager.AppSettings["RestOperacoesIntraday"].ToString();

                _SelfHost = new WebServiceHost(typeof(RestOperacoesIntraday), new Uri(lSelfHost));

                var lEndPoint = _SelfHost.AddServiceEndpoint(typeof(IServicoRestOperacoesIntraday), new WebHttpBinding(WebHttpSecurityMode.None), "");

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
        /// Evento que é disparado quando uma nova sessão(Cliente) conecta no serviço
        /// </summary>
        /// <param name="session">Session que acabou de conectar</param>
        public void socketServer_NewSessionConnected(WebSocketSession session)
        {
            try
            {
                //session.

                //session.ListClientSubscriptions.Add(session.Items)
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no evento socketServer_NewSessionConnected ->",ex );

                throw(ex);
            }
        }

        /// <summary>
        /// Evento que é disparado quando chega uma requisição do cliente(usuário) 
        /// conectado no socket
        /// </summary>
        /// <param name="session">Session que acabou de mandar uma mensagem na sessão</param>
        /// <param name="value">Mensagem armazenada no </param>
        public void socketServer_NewMessageReceived(WebSocketSession session, string value)
        {
            try
            {
                var lMensagemSplit = value.Split(' ');
                
                var lTipo = lMensagemSplit[0];

                BuscarOperacoesIntradaySocketRequest lRequest = JsonConvert.DeserializeObject(lMensagemSplit[1], typeof(BuscarOperacoesIntradaySocketRequest)) as BuscarOperacoesIntradaySocketRequest;

                if (lTipo == "SUBSCRIBE")
                {
                    session.lOperacoesIntradayRequest = lRequest;
                    
                    //if (!session.ListClientSubscriptions.Contains(lClienteInt))
                    //{
                       // session.ListClientSubscriptions.Add(lClienteInt);

                        var e  = new MessagePositionClientArgs();

                        e.Session = session;

                        //e.CodigoCliente = lClienteInt;

                        SendMessageMemoryPositionClientToQueue(this, e);
                    //}
                }
                else if (lTipo == "UNSUBSCRIBE")
                {
                    session.lOperacoesIntradayRequest = new BuscarOperacoesIntradaySocketRequest();

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
            try
            {
                _Logger.Info("Parando o servico de Monitoramento e consumo do Operações Intraday");

                _ServicoStatus = ServicoStatus.Parado;
                
                _Logger.Info("Servico parado com sucesso.");

                //Parando o servidor bootstrap
                _Bootstrap.Stop();

                //Parando serviço de HOsting do REST
                _SelfHost.Close();

                while (_ThreadQueueSendPositionClient.IsAlive)
                {
                    _Logger.Info("Aguardando finalizar thThreadCarregarMonitorMemoria");
                    Thread.Sleep(250);
                    _ThreadQueueSendPositionClient.Abort();
                }
            }
            catch (Exception ex)
            {
                _Logger.Error("Ocorreu um erro ao parar o Servico de Compliance.", ex);
            }
             
        }

        /// <summary>
        /// Método que retorna o status do serviço.
        /// </summary>
        /// <returns>Retorna o status do serviço de Monitoramento do Position Client</returns>
        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        /// <summary>
        /// Método que retorna se a thread principal do serviço está rodando.
        /// </summary>
        /// <returns>Retorna se a thread principal do serviço está rodando.</returns>
        public bool ReturnIniciator()
        {
            return _KeepRunning;
        }

        /// <summary>
        /// Evento que é disparado quando chega um mensagem de PositionClient. O método
        /// varre as sessions que estão ativas no socket e verifica se está cliente da
        /// mensagem Está com com conexão de socket aberto para enviar a session para o
        /// enfileiramento da classe.
        /// </summary>
        /// <param name="sender">Classe que está chamando o evento, normalmente a classe
        /// PositionClientSocketOPeracoesIntraday</param>
        /// <param name="e">Event Args com a classe com o conteúdo a ser enviado ao
        /// aplicativo conectado via socket</param>
        [System.ComponentModel.Description("Evento que é disparado quando chega um mensagem de PositionClient. O método varre as sessions que estão ativas no socket e verifica se está cliente da mensagem Está com com conexão de socket aberto para enviar a session para o enfileiramento da classe.")]
        private void SendMessagePositionClientToQueue(object sender, MessagePositionClientArgs e)
        {
            try
            {
                var lSessions = _WebSocketServer.GetAllSessions();

                if (lSessions == null) return;

                foreach (var session in lSessions)
                {
                    if (session.lOperacoesIntradayRequest == null)
                    {
                        continue;
                    }

                    if (session.lOperacoesIntradayRequest.CodigoCliente == 0)
                    {
                        continue;
                    }

                    //_Logger.InfoFormat("Numero de Sessoes conectadas {0}", lSessions.Count());

                    bool lVazia = false;

                    foreach (List<PosClientSymbolInfo> info in e.Message.Values)
                    {
                        PosClientSymbolInfo lPosicao = info[0];

                        if (TrataFiltroEfetuado(session, lPosicao))
                        {
                            var lSession = new SessionMessagePostionClient();

                            lSession.Session = session;

                            lSession.MessageString = JsonConvert.SerializeObject(lPosicao);

                            lVazia = _QueueSessionMessagesPositionClients.IsEmpty;

                            _QueueSessionMessagesPositionClients.Enqueue(lSession);

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
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no método SendMessagePositionClientToQueue() -> ", ex);
            }
        }

        /// <summary>
        /// Método que trata Filtro Efetuado pela aplicação de objetos de OPeração Intraday
        /// </summary>
        /// <param name="pSession"></param>
        /// <param name="pInfo"></param>
        /// <returns></returns>
        public bool TrataFiltroEfetuado(WebSocketSession pSession, PosClientSymbolInfo pInfo)
        {
            try
            {
                if (pSession.lOperacoesIntradayRequest == null)
                    return false;

                var lFiltro = pSession.lOperacoesIntradayRequest;

                if (pInfo.QtdExecC == 0 && pInfo.QtdExecV == 0 && pInfo.NetExec == 0 && pInfo.QtdAbC == 0 && pInfo.QtdAbV == 0 && pInfo.NetAb == 0)
                {
                    return false;
                }

                if (lFiltro.CodigoCliente != 0 && (pInfo.Account != lFiltro.CodigoCliente))
                {
                    return false;
                }

                if (lFiltro.CodigoInstrumento != "" && 
                    (pInfo.Ativo != lFiltro.CodigoInstrumento && pInfo.Ativo != string.Concat(lFiltro.CodigoInstrumento, "F")))
                {
                    return false;
                }

                if (!lFiltro.OpcaoMarketTodosMercados)
                {
                    var lTiposMercado = new List<string> { "vis", "fut", "opf", "dis", "opc", "opv" };

                    if (!lFiltro.OpcaoMarketAVista)
                    {
                        lTiposMercado.Remove("vis");
                    }

                    if (!lFiltro.OpcaoMarketFuturos)
                    {
                        lTiposMercado.Remove("fut");
                        lTiposMercado.Remove("opf");
                        lTiposMercado.Remove("dis");
                    }

                    if (!lFiltro.OpcaoMarketOpcao)
                    {
                        lTiposMercado.Remove("opc");
                        lTiposMercado.Remove("opv");
                    }

                    if (lFiltro.OpcaoMarketAVista || lFiltro.OpcaoMarketFuturos || lFiltro.OpcaoMarketOpcao)
                    {
                        if (!lTiposMercado.Contains(pInfo.TipoMercado.ToLower()))
                        {
                            return false;
                        }
                    }
                }

                //Opção de Parametros Intraday
                if (lFiltro.OpcaoParametroIntradayNetNegativo)
                {
                    if (pInfo.NetExec >= 0)
                    { return false; }
                }

                if (lFiltro.OpcaoParametroIntradayOfertasPedra)
                {
                    if (pInfo.NetAb <= 0)
                    { return false; }
                    //lFiltrado = from a in lFiltrado where a.NetAb > 0 select a;
                }

                if (lFiltro.OpcaoParametroIntradayPLNegativo)
                {
                    if (pInfo.LucroPrej >= 0)
                    { return false; }
                    //lFiltrado = from a in lFiltrado where a.LucroPrej < 0 select a;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no método de tratamento TrataFiltroEfetuado -> ", ex);
            }
            return true;
        }

        /// <summary>
        /// Evento que busca as posição do cliente na memória (Dictionary) e 
        /// envia a mensagem para a enfileiramento Thread de envio de mensagens. 
        /// </summary>
        /// <param name="sender">Classe que está requisitando o evento</param>
        /// <param name="e">Classe preenchida com os dados de Position Client como argumento para ser 
        /// tratado e enviado para a Fila de Posições a ser enviado</param>
        public void SendMessageMemoryPositionClientToQueue(object sender, MessagePositionClientArgs e)
        {
            try
            {
                int lCodigoCliente = 0;

                var lDicPositionClient = _Socket.DicPositionClient;
                
                List<PosClientSymbolInfo> lListOut = new List<PosClientSymbolInfo>();

                var lPos = _Socket.DicPositionClient;

                if (e.Session.lOperacoesIntradayRequest == null)
                {
                    return;
                }

                if (e.Session.lOperacoesIntradayRequest.CodigoCliente == 0)
                {
                    return;
                }

                lCodigoCliente = e.Session.lOperacoesIntradayRequest.CodigoCliente;

                _Logger.InfoFormat("Usuário solicitou a assinatura do cliente: {0}", lCodigoCliente);

                lock (lPos)
                {
                    foreach (KeyValuePair<int, List<PosClientSymbolInfo>> lList in lPos)//.TryGetValue(lCodigoCliente, out lListOut))
                    {
                        lListOut.AddRange(lList.Value);
                    }
                }

                var lListaVerifica = from a in lListOut where (a.QtdExecC != 0 || a.QtdExecV != 0) || (a.QtdAbC != 0 || a.QtdAbV == 0) select a;

                if (lListaVerifica.Count() == 0)
                {
                    return;
                }

                if (e.Session.lOperacoesIntradayRequest.CodigoCliente != 0)
                {
                    var lPosicaoEnumerable = from a in lListaVerifica where a.Account == e.Session.lOperacoesIntradayRequest.CodigoCliente select a;

                    _Logger.InfoFormat("O sistema encontrou {0} de posições de do cliente {1}", lPosicaoEnumerable.Count(), e.Session.lOperacoesIntradayRequest.CodigoCliente);
                }
                //var lPosicao = new ConcurrentDictionary<int, List<PosClientSymbolInfo>>(lPosicaoEnumerable); //  lPosicaoEnumerable.ToConcurrentDictionary(p => p.Key);
                
                foreach (PosClientSymbolInfo lPosicao in lListOut)
                {
                    //lock (lPosicao)
                    //{
                        if (TrataFiltroEfetuado(e.Session, lPosicao))
                        {
                            bool lVazia = _QueueSessionMessagesPositionClients.IsEmpty;

                            var lSession = new SessionMessagePostionClient();

                            lSession.Session = e.Session;

                            lSession.MessageString = JsonConvert.SerializeObject(lPosicao);

                            _Logger.InfoFormat("Enfileirando Cliente ->{0} Ativo-> {1}", lPosicao.Account, lPosicao.Ativo);

                            _QueueSessionMessagesPositionClients.Enqueue(lSession);

                            _Logger.InfoFormat("SendMessageMemoryPositionClientToQueue -> Position Clients enfileirados {0}", _QueueSessionMessagesPositionClients.Count);

                            if (lVazia)
                            {
                                lock (_Singleton)
                                {
                                    System.Threading.Monitor.Pulse(_Singleton);
                                }
                            }
                        }
                    //}
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
        private void QueueSendMessagePositionClient()
        {
            _Logger.Info("Entrou na função de enviar mensagens de position client para as sessões conectadas");

            try
            {
                while (_KeepRunning || !_QueueSessionMessagesPositionClients.IsEmpty)
                {
                    //_Logger.InfoFormat("Keep Running....");

                    //if (!_QueueSessionMessagesPositionClients.IsEmpty)
                    //{

                        SessionMessagePostionClient lSessionMessage;

                        if (_QueueSessionMessagesPositionClients.TryPeek(out lSessionMessage))
                        {
                            _Logger.InfoFormat("Enviando Mensagem para aplicação  {0}", lSessionMessage.MessageString);

                            lSessionMessage.Session.Send(lSessionMessage.MessageString);

                            while (!_QueueSessionMessagesPositionClients.TryDequeue(out lSessionMessage)) ;

                            _Logger.InfoFormat("Quantidade na fila: [{0}]", _QueueSessionMessagesPositionClients.Count);

                            continue;
                        }
                    //}

                    lock (_Singleton)
                    {
                        System.Threading.Monitor.Wait(_Singleton, 50);
                    }
                }
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no método QueueSendMessagePositionClient() -> ", ex);
            }
        }
        
        /// <summary>
        /// Método que realiza a busca do objeto position client armazenado na memória para 
        /// ser retornado a aplicação cliente conectada
        /// </summary>
        /// <param name="lRequest">Dados de request com o código do cliente</param>
        /// <returns>Retorna o objeto de Position Client filtrado pelo código do cliente</returns>
        public BuscarPositionClientResponse BuscarPositionClient(BuscarPositionClientRequest lRequest)
        {
            var lRetorno = new BuscarPositionClientResponse();
            try
            {
                _Logger.InfoFormat("Buscando informações de position Client do cliente : {0}", lRequest.CodigoCliente);
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no método BuscarPositionClient() -> ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que realiza a busca do objeto de Operações intraday armazenado na memória para 
        /// ser retornado a aplicação cliente conectada
        /// </summary>
        /// <param name="lRequest">Dados de request com o filtro configurado pelo cliente</param>
        /// <returns>Retorna o objeto de Operacoes Intraday filtrado pelas propriedades do request </returns>
        public BuscarOperacoesIntradayResponse BuscarOperacoesIntraday(BuscarOperacoesIntradayRequest lRequest)
        {
            _Logger.InfoFormat("Buscando informações de Operações Intraday com o filtro de Cliente: {0}, Ativo: {1}, Parametros intraday {2}, Opção Market {3}", 
                lRequest.CodigoCliente, 
                lRequest.Ativo,
                lRequest.OpcaoParametrosIntraday, 
                lRequest.OpcaoMarket);

            _Logger.InfoFormat("Quantidade de Posições no DicPositionClient {0}", _Socket.DicPositionClient.Count);

            var lRetorno = new BuscarOperacoesIntradayResponse();

            try
            {
                var lList = new List<PosClientSymbolInfo>();

                var lDic = new ConcurrentDictionary<int, List<PosClientSymbolInfo>>();

                foreach (KeyValuePair<int, List<PosClientSymbolInfo>> pos in _Socket.DicPositionClient)
                {
                    lList.AddRange(pos.Value);
                }

                var lFiltrado = from a in lList where (a.QtdExecC != 0 || a.QtdExecV != 0) || (a.QtdAbC != 0 || a.QtdAbV == 0) select a;

                if (!string.IsNullOrEmpty( lRequest.Ativo ))
                {
                    lFiltrado = from a in lFiltrado where a.Ativo == lRequest.Ativo select a;
                }

                if (lRequest.CodigoCliente != 0)
                {
                    lFiltrado = from a in lFiltrado where a.Account == lRequest.CodigoCliente select a;
                }

                //Opção Market
                if (!lRequest.OpcaoMarket.Equals(Lib.Dados.OpcaoMarket.TodosMercados))
                {
                    var lTiposMercado = new List<string> { "vis","fut", "opf", "dis", "opc", "opv" };

                    if (!(lRequest.OpcaoMarket & Lib.Dados.OpcaoMarket.Avista).Equals(Lib.Dados.OpcaoMarket.Avista))
                    {
                        lTiposMercado.Remove("vis");
                    }

                    if (!(lRequest.OpcaoMarket & Lib.Dados.OpcaoMarket.Futuros).Equals(Lib.Dados.OpcaoMarket.Futuros))
                    {
                        lTiposMercado.Remove("fut");
                        lTiposMercado.Remove("opf");
                        lTiposMercado.Remove("dis");
                    }

                    if (!(lRequest.OpcaoMarket & Lib.Dados.OpcaoMarket.Opcoes).Equals(Lib.Dados.OpcaoMarket.Opcoes))
                    {
                        lTiposMercado.Remove("opc");
                        lTiposMercado.Remove("opv");
                    }

                    if ((lRequest.OpcaoMarket & Lib.Dados.OpcaoMarket.Avista).Equals(Lib.Dados.OpcaoMarket.Avista) ||
                        (lRequest.OpcaoMarket & Lib.Dados.OpcaoMarket.Opcoes).Equals(Lib.Dados.OpcaoMarket.Opcoes) ||
                        (lRequest.OpcaoMarket & Lib.Dados.OpcaoMarket.Futuros).Equals(Lib.Dados.OpcaoMarket.Futuros))
                    {
                        lFiltrado = from a in lFiltrado where lTiposMercado.Contains(a.TipoMercado.ToLower()) select a;
                    }
                }

                //Opção de Parametros Intraday
                if ((lRequest.OpcaoParametrosIntraday & Lib.Dados.OpcaoParametrosIntraday.NetIntradayNegativo ).Equals( Lib.Dados.OpcaoParametrosIntraday.NetIntradayNegativo))
                {
                    lFiltrado = from a in lFiltrado where a.NetExec < 0  select a;
                }

                if ((lRequest.OpcaoParametrosIntraday & Lib.Dados.OpcaoParametrosIntraday.OfertasPedra ).Equals( Lib.Dados.OpcaoParametrosIntraday.OfertasPedra))
                {
                    lFiltrado = from a in lFiltrado where a.NetAb > 0 select a;
                }

                if ((lRequest.OpcaoParametrosIntraday & Lib.Dados.OpcaoParametrosIntraday.PLNegativo ).Equals(Lib.Dados.OpcaoParametrosIntraday.PLNegativo))
                {
                    lFiltrado = from a in lFiltrado where a.LucroPrej < 0 select a;
                }
                /*
                foreach (PosClientSymbolInfo obj in lFiltrado)
                {
                    var lLista = lFiltrado.Where(op => (op.Account == obj.Account));

                    lDic.AddOrUpdate(obj.Account, lLista.ToList(), (key, oldValue) => lLista.ToList());
                }
                */

                lRetorno.ListOperacoesIntraday = lFiltrado.ToList();

                _Logger.InfoFormat("Foram Encontrados {0} itens", lRetorno.ListOperacoesIntraday.Count);
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no método BuscarOperacoesIntraday", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que realiza a busca do objeto de Risco Resumido armazenado na memória para 
        /// ser retornado a aplicação cliente conectada
        /// </summary>
        /// <param name="lRequest">Dados de request com o filtro configurado pelo cliente</param>
        /// <returns>Retorna o objeto de Risco Resumido filtrado pelas propriedades do request </returns>
        public BuscarRiscoResumidoResponse BuscarRiscoResumido(BuscarRiscoResumidoRequest lRequest)
        {
            var lRetorno = new BuscarRiscoResumidoResponse();

            try
            {

            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no método BuscarRiscoResumido", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método teste para retornar JSON
        /// </summary>
        /// <returns></returns>
        public string BuscarOperacoesIntradayJSON()
        {
            string lRetorno = string.Empty;

            try
            {
                var lList = new List<PosClientSymbolInfo>();

                var lListTrans = new List<TransporteOperacoesIntraday>();

                var lDic = new ConcurrentDictionary<int, List<PosClientSymbolInfo>>();

                foreach (KeyValuePair<int, List<PosClientSymbolInfo>> pos in _Socket.DicPositionClient)
                {
                    var lTrans = new TransporteOperacoesIntraday(pos.Value);
                    
                    lListTrans.AddRange(lTrans.ListaTransporte);
                }

                lRetorno = JsonConvert.SerializeObject(lListTrans);

            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no método BuscarOperacoesIntradayJSON", ex);
            }

            return lRetorno;
          
        }
    }
}
