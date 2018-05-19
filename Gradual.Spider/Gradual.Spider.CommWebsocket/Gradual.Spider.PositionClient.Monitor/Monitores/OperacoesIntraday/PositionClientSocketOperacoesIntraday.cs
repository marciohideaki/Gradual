using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Gradual.Spider.SupervisorRisco.Lib.Dados;
using Gradual.Spider.CommSocket;
using Gradual.Spider.DataSync.Lib.Mensagens;
using Gradual.Spider.DataSync.Lib;
using System.Collections.Concurrent;
using Gradual.Spider.PositionClient.Monitor.Lib.Message;
using Gradual.Spider.PositionClient.Monitor.Dados;


namespace Gradual.Spider.PositionClient.Monitor
{
    /// <summary>
    /// Classe responsável pela conexão e consumo do position client socket
    /// </summary>
    [Serializable]
    public class PositionClientPackageSocket : IDisposable
    {
        #region Atributos
        /// <summary>
        /// Propriedade de controle para verificar se está conectado com o serviço.
        /// </summary>
        private bool bKeepRuning = false;

        /// <summary>
        /// Atributo responsável pela log da classe
        /// </summary>
        private static readonly log4net.ILog _Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Atributo que sinaliza se o a conexão com o serviço de POsition Client está ativa
        /// </summary>
        private bool _bConnected = false;

        /// <summary>
        /// Atributo de conexão socket para consumir as informações de risco de clientes no serviço de position client.
        /// </summary>
        private SpiderSocket _ClientSocket { get; set; }

        /// <summary>
        /// Atributo de callback no assíncrono para disparar evento quando houver algum input de dados.
        /// </summary>
        private AsyncCallback _WorkerCallBack;

        /// <summary>
        /// Atributo de Resultado assíncrono do evento disparado do WorkerCallBack
        /// </summary>
        private IAsyncResult _AsyncResult;

        private static PositionClientPackageSocket _Instance = null;
        #endregion

        #region Propriedades
        /// <summary>
        /// Ip de conexão com o serviço de Postion client
        /// </summary>
        public string IpAddr { get; set; }

        /// <summary>
        /// Porta de conexão com o Serviço de Position Client
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Thread de gerenciamento da sonda de verificação de conexão
        /// </summary>
        [NonSerialized]
        private Thread ThreadSonda = null;

        /// <summary>
        /// Dictionary de Position Client. Armazena as posições dos clientes na memória com o Snapshot que retorna do serviço 
        /// e as posições de risco que vem durante o dia.
        /// </summary>
        public ConcurrentDictionary<int, List<PosClientSymbolInfo>> DicPositionClient = new ConcurrentDictionary<int, List<PosClientSymbolInfo>>();

        /// <summary>
        /// Event Handler para delegar um método na classe chamadora e chamar um método para infileirar o 
        /// envio das mensagens para aplicação
        /// </summary>
        public event EventHandler<MessagePositionClientArgs> SendMessageClientConnected;

        /// <summary>
        /// Dictionary de Symbol Info. Armazena as Lista de cadastro de papéis na memória com o 
        /// Snapshot que retorna do serviço Supervisor risco
        /// </summary>
        public ConcurrentDictionary<string, SymbolInfo> DicSymbolList = new ConcurrentDictionary<string, SymbolInfo>();
        
        /// <summary>
        /// Singleton da classe para ser usada no servidor
        /// </summary>
        public static PositionClientPackageSocket Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new PositionClientPackageSocket();
                }

                return _Instance;
            }
        }
        #endregion

        #region Classes auxiliares
        /// <summary>
        /// Classe auxiliar para 
        /// </summary>
        public sealed class PositionClientSocketPackage
        {
            public SpiderSocket _Socket;
            public byte[] _DataBuffer = new byte[4];
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método que abre conexão via socket com o serviço de Position Client 
        /// </summary>
        public void OpenConnection()
        {
            try
            {
                _Logger.Info("Abrindo conexão com o Position:" + IpAddr + ":" + Port );

                //_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                _ClientSocket = new SpiderSocket();

                IPAddress lIp     = IPAddress.Parse(IpAddr);
                IPEndPoint lIpEnd = new IPEndPoint(lIp, Convert.ToInt32(Port));

                //_ClientSocket.Connect(lIpEnd);

                _bConnected = true;

                //this.ThreadWaitOne();
            }
            catch (Exception ex)
            {
                _Logger.Error("OpenConnection():" + ex.Message, ex);
            }
        }


        /// <summary>
        /// Método que consulta estado de conexão do socket 
        /// </summary>
        /// <returns>Retorna o estado de conexão do socket</returns>
        public bool IsConnected()
        {
            return _bConnected;
        }

        /// <summary>
        /// Método de recebimento da mensagem de Postion Client e trata de acordo com a tipo de mensagem 
        /// Como snapshot, Insert, Update, Delete. e insere no Dictionary de Postion Client que irá ficar na memória
        /// </summary>
        /// <param name="sender">Classe que solicitou/chamou o método</param>
        /// <param name="pClientNumber">(Não está sendo usado)</param>
        /// <param name="pClientSocket">(Nâo está sendo usado)</param>
        /// <param name="args">Event Args do tipo PositionClientSyncMsg preenchido com os dados da 
        /// mensagem de risco do position client</param>
        private void OnPositionClientSync(object sender, int pClientNumber, Socket pClientSocket, PositionClientSyncMsg args)
        {
            try 
	        {
                switch(args.SyncAction)
                {
                    case SyncMsgAction.SNAPSHOT:
                        DicPositionClient.Clear();
                        
                        _Logger.Info("Recebeu snapshot de position client com " + args.PositionClient.Count() + " itens");

                        //foreach (KeyValuePair<int, List<PosClientSymbolInfo>> item in args.PositionClient)
                        //{
                        //    DicPositionClient.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        //}

                        DicPositionClient = args.PositionClient;
                        
                        break;

                    case SyncMsgAction.INSERT:
                    case SyncMsgAction.UPDATE:
                        #region Update
                        foreach (KeyValuePair<int, List<PosClientSymbolInfo>> item in args.PositionClient)
                        {
                            List<PosClientSymbolInfo> lstOut = null;
                                
                            if (DicPositionClient.TryGetValue(item.Key, out lstOut))
                            {
                                PosClientSymbolInfo lPositionClient = null;

                                //lock (lstOut)
                                //{
                                    try
                                    {
                                        //_Logger.InfoFormat("Recebendo Posição cliente: {0} de ativo: {1} quantidade Total: {2}", item.Key, item.Value[0].Ativo, (item.Value[0].QtdExecC + item.Value[0].QtdExecV) );

                                        lPositionClient = lstOut.FirstOrDefault(x => x.Ativo.Equals(item.Value[0].Ativo));

                                        //if ( lPositionClient != null &&  (lPositionClient.QtdExecC == 0 && lPositionClient.QtdExecV == 0) && (lPositionClient.QtdAbC == 0 && lPositionClient.QtdAbV == 0))
                                        //{
                                        //    continue;
                                        //}
                                        
                                    }
                                    catch (Exception ex)
                                    {
                                        _Logger.ErrorFormat("Erro encontrato no processo de foreach do position cliente -> {0}", ex.Message);
                                    }

                                    if (lPositionClient != null)
                                    {
                                        lstOut.Remove(lPositionClient);

                                        lPositionClient = item.Value[0];

                                        lstOut.Add(lPositionClient);

                                        DicPositionClient.AddOrUpdate(item.Key, lstOut, (key, oldValue) => lstOut);
                                    }
                                    else
                                    {

                                        lstOut.Add(item.Value[0]);
                                    }
                                //}
                            }
                            else
                            {

                                DicPositionClient.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                            }
                        }
                        
                        
                        EventHandler<MessagePositionClientArgs> lHandler = SendMessageClientConnected;

                        if (lHandler != null)
                        {
                            var e = new MessagePositionClientArgs();

                            if (args.PositionClient != null && args.PositionClient.Count > 0)
                            {
                                e.CodigoCliente = args.PositionClient.ToList()[0].Key;

                                e.Message = args.PositionClient;

                                e.SyncAction = args.SyncAction;

                                lHandler(this, e);
                            }
                        }
                        #endregion
                        break;
                    case SyncMsgAction.DELETE:
                        
                        List<PosClientSymbolInfo> lInfo = null;



                        //foreach (KeyValuePair<int, List<PosClientSymbolInfo>> item in args.PositionClient)
                        //{

                        //    DicPositionClient.TryRemove(item.Key, out lInfo);
                        //}
                        break;
                    default:
                        _Logger.Error("Acao [" + args.SyncAction + "] invalida para dicionario/lista de restriction symbol");
                        break;
                }
	        }
	        catch (Exception ex)
	        {
                _Logger.Error("Erro encontrado no método OnPositionClientSync - " + ex.Message, ex);
	        }
        }

        /// <summary>
        /// Metodo que inicia a conexão spider para o serviço de Risco
        /// </summary>
        /// <param name="pSubscriptions">Lista de assinaturas que irão conectar no Serviço de </param>
        public void StartClientConnect (List<int> pSubscriptions )
        {
            try 
	        {
		        _ClientSocket = new SpiderSocket();

                _ClientSocket.IpAddr = this.IpAddr;
                _ClientSocket.Port   = this.Port;

                _Logger.Info("Conectando na máquina: " + _ClientSocket.IpAddr + " na porta: " + _ClientSocket.Port);

                _ClientSocket.AddHandler<PositionClientSyncMsg>(new ProtoObjectReceivedHandler<PositionClientSyncMsg>(OnPositionClientSync));
                //_ClientSocket.AddHandler<SymbolListSyncMsg>(new ProtoObjectReceivedHandler<SymbolListSyncMsg>(OnSymbolListSync));
                _ClientSocket.OnUnmappedObjectReceived += new UnmappedObjectReceivedHandler(_ClientSocket_OnUnmappedObjectReceived);
                _ClientSocket.OpenConnection();

                //_ClientSocket.AddHandler<SymbolListSyncMsg>(new ProtoObjectReceivedHandler<SymbolListSyncMsg>(OnSymbolListSync));

                ThreadSonda = new Thread(new ThreadStart(MonitorSonda));
                ThreadSonda.Start();

                bKeepRuning = true;
	        }
	        catch (Exception ex)
	        {
                _Logger.Error("Erro encontrado no método StartClientConnect():" + ex.Message, ex);
	        }
        }

        /// <summary>
        /// Evento disparado quando chega mensagem de cadastro de pepéis e preenche o dictionary
        /// </summary>
        /// <param name="sender">(Não está sendo usado)</param>
        /// <param name="clientNumber">(Não está sendo usado)</param>
        /// <param name="clientSock">(Não está sendo usado)</param>
        /// <param name="args"></param>
        private void OnSymbolListSync(object sender, int clientNumber, Socket clientSock, SymbolListSyncMsg args)
        {
            try
            {
                switch(args.SyncAction)
                {
                    case SyncMsgAction.SNAPSHOT:
                        DicSymbolList.Clear();
                        DicSymbolList = args.Symbols;

                        _Logger.Info("Recebeu snapshot de cadastro de papel com " + args.Symbols.Count() + " itens");

                        break;
                    case SyncMsgAction.UPDATE:
                        //DicSymbolList.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);
                        break;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error("Erro encontrado no método OnSymbolListSync():" + ex.Message, ex);
            }
        }

        private void _ClientSocket_OnUnmappedObjectReceived(object sender, int clientNumber, Socket clientSock, Type objectType, object objeto)
        {
            
        }

        /// <summary>
        /// Método de que fecha a conexão com o socket do Risco client
        /// </summary>
        public void StopClientConnect()
        {
            bKeepRuning = false;

            if (_ClientSocket != null)
            {
                _ClientSocket.OnUnmappedObjectReceived -= ClientSocket_OnUnmappedObjectReceived;
                _ClientSocket.CloseSocket();
                _ClientSocket = null;
            }
        }

        /// <summary>
        /// Método usado para tirar o delegate do evento do socket
        /// </summary>
        /// <param name="sender">(Não está sendo usado)</param>
        /// <param name="clientNumber">(Não está sendo usado)</param>
        /// <param name="clientSock">(Não está sendo usado)</param>
        /// <param name="objectType">(Não está sendo usado)</param>
        /// <param name="objeto">(Não está sendo usado)</param>
        public void ClientSocket_OnUnmappedObjectReceived(object sender, int clientNumber, Socket clientSock, Type objectType, object objeto)
        {

        }

        /// <summary>
        /// Método que monitora a conexão socket com o serviço de risco position client
        /// </summary>
        public void MonitorSonda()
        {
            long lLastSonda = 0;

            while (bKeepRuning)
            {
                try
                {
                    if (!_ClientSocket.IsConectado())
                    {
                        _ClientSocket.OpenConnection();
                    }

                    TimeSpan lTimeSpan = new TimeSpan(DateTime.Now.Ticks - lLastSonda);

                    if (lTimeSpan.TotalMilliseconds > 30000)
                    {
                        lLastSonda = DateTime.Now.Ticks;
                    }

                    Thread.Sleep(150);
                }
                catch (Exception ex)
                {
                    _Logger.Error("Erro encontrado no método MonitorSonda()-"+ ex.Message, ex);
                }
            }

        }

        /// <summary>
        /// Método de Dispose da classe que gerencia as chamadas para limpeza dos atributos 
        /// e recursos
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Método de Dispose para que libera os recursos gerenciados.
        /// </summary>
        /// <param name="disposing">Controle de dispose para limpar os atributos</param>
        protected virtual void Dispose(bool pDisposing)
        {
            if (pDisposing)
            {
                if (_ClientSocket != null)
                {
                    _ClientSocket.Dispose();
                    _ClientSocket = null;
                }
            }
        }
        #endregion

        #region Destrutores
        /// <summary>
        /// Destrutor da classe para limpar a memória usado pelo Serviço
        /// </summary>
        ~PositionClientPackageSocket()
        {
            Dispose(false);
        }
        #endregion
    }
}
