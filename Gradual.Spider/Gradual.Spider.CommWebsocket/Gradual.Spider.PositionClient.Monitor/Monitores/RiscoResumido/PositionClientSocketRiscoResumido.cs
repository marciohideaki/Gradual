using Gradual.Spider.CommSocket;
using Gradual.Spider.DataSync.Lib;
using Gradual.Spider.DataSync.Lib.Mensagens;
using Gradual.Spider.PositionClient.Monitor.Lib.Message;
using Gradual.Spider.SupervisorRisco.Lib.Dados;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Gradual.Spider.PositionClient.Monitor.Monitores.RiscoResumido
{
    /// <summary>
    /// Classe responsável pela conexão e consumo do position client socket de risco resumido
    /// </summary>
    [Serializable]
    public class PositionClientSocketRiscoResumido : IDisposable
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

        private static PositionClientSocketRiscoResumido _Instance = null;
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
        public ConcurrentDictionary<int, ConsolidatedRiskInfo> DicConsolidatedRisk = new ConcurrentDictionary<int, ConsolidatedRiskInfo>();

        /// <summary>
        /// Event Handler para delegar um método na classe chamadora e chamar um método para infileirar o 
        /// envio das mensagens para aplicação
        /// </summary>
        public event EventHandler<MessageRiscoResumidoArgs> SendMessageClientConnected;

        /// <summary>
        /// Singleton da classe para ser usada no servidor
        /// </summary>
        public static PositionClientSocketRiscoResumido Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new PositionClientSocketRiscoResumido();
                }

                return _Instance;
            }
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
                _Logger.Info("Abrindo conexão com o Position:" + IpAddr + ":" + Port);

                //_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                _ClientSocket = new SpiderSocket();

                IPAddress lIp = IPAddress.Parse(IpAddr);
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
        /// Metodo que inicia a conexão spider para o serviço de Risco
        /// </summary>
        /// <param name="pSubscriptions">Lista de assinaturas que irão conectar no Serviço de </param>
        public void StartClientConnect(List<int> pSubscriptions)
        {
            try
            {
                _ClientSocket = new SpiderSocket();

                _ClientSocket.IpAddr = this.IpAddr;
                _ClientSocket.Port = this.Port;

                _Logger.Info("Conectando na máquina: " + _ClientSocket.IpAddr + " na porta: " + _ClientSocket.Port);

                _ClientSocket.AddHandler<ConsolidatedRiskSyncMsg>(new ProtoObjectReceivedHandler<ConsolidatedRiskSyncMsg>(OnConsolidatedRiskSync));
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
        /// Método de recebimento da mensagem de Postion Client de Risco Resumido e trata de acordo com a tipo de mensagem 
        /// Como snapshot, Insert, Update, Delete. e insere no Dictionary de Risco Resumido (ConsolidatesRisk) que irá ficar na memória
        /// </summary>
        /// <param name="sender">Classe que solicitou/chamou o método</param>
        /// <param name="clientNumber">(Não está sendo usado)</param>
        /// <param name="clientSock">(Não está sendo usado)</param>
        /// <param name="args">Event Args do tipo PositionClientSyncMsg preenchido com os dados da mensagem de risco do Risco Resumido</param>
        private void OnConsolidatedRiskSync(object sender, int clientNumber, Socket clientSock, ConsolidatedRiskSyncMsg args)
        {
            try
            {
                switch (args.SyncAction)
                {
                    case SyncMsgAction.SNAPSHOT:
                        DicConsolidatedRisk.Clear();
                        DicConsolidatedRisk = args.ConsolidatedRisk;
                        _Logger.Info("Recebeu snapshot de position client com " + args.ConsolidatedRisk.Count() + " itens");
                        break;
                    case SyncMsgAction.INSERT:
                    case SyncMsgAction.UPDATE:
                        
                        foreach (KeyValuePair<int, ConsolidatedRiskInfo> item in args.ConsolidatedRisk)
                        {
                            DicConsolidatedRisk.AddOrUpdate(item.Key, item.Value, (key, oldValue) => item.Value);

                            _Logger.Info("Recebeu Consolited Risk do cliente: " +item.Key + "  PL Bovespa: " + item.Value.PLBovespa.ToString("n2") + " PL BMF:" + item.Value.PLBmf.ToString("n2"));
                        }

                        EventHandler<MessageRiscoResumidoArgs> lHandler = SendMessageClientConnected;

                        if (lHandler != null)
                        {
                            var e = new MessageRiscoResumidoArgs();

                            if (args.ConsolidatedRisk != null && args.ConsolidatedRisk.Count > 0)
                            {
                                e.CodigoCliente = args.ConsolidatedRisk.ToList()[0].Key;

                                e.Message = args.ConsolidatedRisk;

                                e.SyncAction = args.SyncAction;

                                lHandler(this, e);
                            }
                        }

                        break;
                    case SyncMsgAction.DELETE:

                        ConsolidatedRiskInfo lInfo = null;

                        foreach (KeyValuePair<int, ConsolidatedRiskInfo> item in args.ConsolidatedRisk)
                        {
                            DicConsolidatedRisk.TryRemove(item.Key, out lInfo);
                        }
                        break;
                    default:
                        _Logger.Error("Acao [" + args.SyncAction + "] invalida para dicionario/lista de restriction symbol");
                        break;
                }
            }
            catch (Exception ex)
            {
                
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
                    _Logger.Error("Erro encontrado no método MonitorSonda()-" + ex.Message, ex);
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
    }
}
