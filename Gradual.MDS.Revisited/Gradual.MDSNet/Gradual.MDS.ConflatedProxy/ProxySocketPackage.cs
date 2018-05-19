using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using log4net;
using System.Collections.Concurrent;


namespace Gradual.MDS.ConflatedProxy
{
    public delegate void MessageReceivedHandler(object sender, MessageEventArgs args);
    public delegate void ClientConnectedHandler(object sender, ClientConnectedEventArgs args);
    public delegate void ConnectionOpenedHandler(object sender, ConnectionOpenedEventArgs args);
    public delegate void ClientDisconnectedHandler(object sender, ClientDisconnectedEventArgs args);

    public class MessageEventArgs : EventArgs
    {
        public byte [] Message { get; set; }
        public long MessageLen { get; set; }
        public Socket ClientSocket { get; set; }
        public int ClientNumber { get; set; }
    }

    public class ClientConnectedEventArgs : EventArgs
    {
        public Socket ClientSocket { get; set; }
        public int ClientNumber { get; set; } 
    }

    public class ConnectionOpenedEventArgs : EventArgs
    {
    }

    public class ClientDisconnectedEventArgs : EventArgs
    {
        public Socket ClientSocket { get; set; }
        public int ClientNumber { get; set; }
    }

    public class ProxySocketPackage : IDisposable
    {
        #region [local variables]

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const int SOCKET_BUFFER_SIZE = 32768;

        private Socket MainSocket = null;
        private ArrayList lstCom = new ArrayList();
        private AsyncCallback WorkerCallBack;
        private string RemoteIP = "";
        private string RemotePort = "";
        private ConcurrentQueue<MessageEventArgs> qMsg = new ConcurrentQueue<MessageEventArgs>();
        Thread thMsgDispatcher = null;
        int ClientCount;


        #endregion

        #region [ Const variables ]

        private int DataBuffer
        {
            get { return int.Parse(ConfigurationSettings.AppSettings["DataBuffer"].ToString()); }
        }

        public string IpAddr { get;set;}
        public string Port { get; set; }

        #endregion

        #region [ Stock variables ]
        /// <summary>
        /// Armazena os ID's das conexões Ativas.
        /// </summary>	
        private Hashtable HashActiveSockets =
             new Hashtable();

        /// <summary>
        /// Armazena os ID's das conexões Ativas.
        /// </summary>	
        private Hashtable HashActiveClients =
             new Hashtable();

        #endregion

        public DateTime LastPacket { get; set; }

        public event MessageReceivedHandler OnRequestReceived;
        public event ClientConnectedHandler OnClientConnected;
        public event ConnectionOpenedHandler OnConnectionOpened;
        public event ClientDisconnectedHandler OnClientDisconnected;
        private Socket _ClientSocket;
        private bool bConectado;
        private bool bKeepRunning;
        private long lastLogSEC = 0;


        [MTAThread]
        private void OnDataReceivedNoSize(IAsyncResult asyn)
        {
            ClientSocketPacket socketData = (ClientSocketPacket)asyn.AsyncState;
            try
            {
                lock (this)
                {
                    int ResultBytesLength = socketData.CurrentSocket.EndReceive(asyn);

                    if (ResultBytesLength <= 0)
                    {
                        logger.Warn("Encerrou conexao (" + ResultBytesLength + ") recebendo tamanho");
                        ThreadPool.QueueUserWorkItem(new WaitCallback(NotificaClienteDesconectado), socketData);
                        this.CloseSocket(socketData.CurrentSocket);
                        return;
                    }

                    LastPacket = DateTime.Now;

                    MessageEventArgs args = new MessageEventArgs();

                    args.ClientNumber = socketData.ClientNumber;
                    args.ClientSocket = socketData.CurrentSocket;
                    args.Message = new byte[ResultBytesLength];
                    Array.Copy(socketData.sizeBuffer, args.Message, ResultBytesLength);
                    args.MessageLen = ResultBytesLength;

                    qMsg.Enqueue(args);

                    if (DateTime.UtcNow.Ticks - lastLogSEC > TimeSpan.TicksPerSecond)
                    {
                        lastLogSEC = DateTime.UtcNow.Ticks;
                        logger.Info("Pacotes compactados na fila: " + qMsg.Count);
                    }

                    //ThreadPool.QueueUserWorkItem(new WaitCallback(MessageBrokerNoSize), args);

                    this.WaitForData(socketData.CurrentSocket, socketData.ClientNumber);
                }
            }
            catch (SocketException se)
            {
                //if (se.ErrorCode == (int)(SocketError.ConnectionReset)){
                //    this.CloseSocket(socketData.CurrentSocket);
                //}
                logger.Error("OnDataReceivedNoSize(SocketEx): " + se.Message + " ErrorCode: " + se.ErrorCode +
                    " Native: " + se.NativeErrorCode +
                    " SocketError: " + se.SocketErrorCode, se);

                logger.Warn("OnDataReceivedNoSize(): Encerrando conexao com o gateway");

                ThreadPool.QueueUserWorkItem(new WaitCallback(NotificaClienteDesconectado), socketData);

                // Falha de conexão
                this.CloseSocket(socketData.CurrentSocket);
            }
            catch (Exception ex)
            {
                logger.Error("OnDataReceivedNoSize(): " + ex.Message, ex);
            }
        }


        private void CloseSocket(Socket CurrentSocket)
        {

            try
            {
                CurrentSocket.Shutdown(SocketShutdown.Both);
                CurrentSocket.Close();
                CurrentSocket = null;
            }
            catch (Exception ex)
            {
                logger.Error("CloseSocket(): " + ex.Message, ex);
            }
            finally
            {
                bConectado = false;
            }
        }

        public void CloseSocket()
        {

            try
            {
                _ClientSocket.Shutdown(SocketShutdown.Both);
                _ClientSocket.Close();
                _ClientSocket = null;
            }
            catch (Exception ex)
            {
                logger.Error("CloseSocket(): " + ex.Message, ex);
            }
            finally
            {
                bConectado = false;
            }
        }

        public bool IsConectado()
        {
            return bConectado;
        }

        
        private void messageDispatcherProc()
        {
            while (bKeepRunning)
            {
                try
                {
                    MessageEventArgs args = null ;
                    if (qMsg.TryDequeue(out args))
                    {
                        if (OnRequestReceived != null)
                            OnRequestReceived(this, args);

                        continue;
                    }

                    Thread.Sleep(25);
                }
                catch (Exception ex)
                {
                    logger.Error("messageDispatcherProc: " + ex.Message, ex);
                }
            }
        }

        private void NotificaClienteDesconectado(object objeto)
        {
            ClientSocketPacket socketData = (ClientSocketPacket) objeto;

            ClientDisconnectedEventArgs args = new ClientDisconnectedEventArgs();
            args.ClientNumber = socketData.ClientNumber;
            args.ClientSocket = socketData.CurrentSocket;

            if (OnClientDisconnected != null)
            {
                OnClientDisconnected(this, args);
            }
        }

        private void ClearSocketReset(object _currentSocket){

            Socket currentSocket =
                (Socket)(_currentSocket);

            foreach (DictionaryEntry Dictionary in HashActiveClients)
            {
                if ((IntPtr)Dictionary.Value == currentSocket.Handle)
                {
                    lock (HashActiveClients)
                    {
                        HashActiveClients.Remove
                            (Dictionary.Key.ToString());
                    }
                    break;
                }
            }
        }

        private void MessageAuthentication(object Instance)
        {
            List<object> lstInstance =
                (List<object>)Instance;

            string Recv = (string)lstInstance[0];
            Socket socketData = (Socket)lstInstance[1];

        }

        private void WaitForData(Socket socket, int clientNumber)
        {
            try{
                if (WorkerCallBack == null)
                {
                        WorkerCallBack = new AsyncCallback(OnDataReceivedNoSize);
                }


                ClientSocketPacket _SocketPacket =
                    new ClientSocketPacket(socket, clientNumber);


                socket.BeginReceive(_SocketPacket.sizeBuffer, 0,
                      _SocketPacket.sizeBuffer.Length,
                      SocketFlags.None,
                      WorkerCallBack,
                      _SocketPacket
                      );
            }
            catch (SocketException se)
            {
                logger.Error("WaitForData(): " + se.Message, se);
                throw se;
            }
        }

        public void StopListen()
        {
            if (MainSocket != null){
                if (MainSocket.Connected == true){
                    MainSocket.Close();
                }
            }
        }

        public void StartListen(int PortNumber)
        {
            try
            {
                if (thMsgDispatcher == null)
                {
                    bKeepRunning = true;
                    thMsgDispatcher = new Thread(new ThreadStart(messageDispatcherProc));
                    thMsgDispatcher.Start();
                }

                MainSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);

                IPEndPoint ipLocal =
                    new IPEndPoint(IPAddress.Any, PortNumber);

                MainSocket.Bind(ipLocal);
                MainSocket.Listen(4);

                MainSocket.BeginAccept(
                    new AsyncCallback(OnClientConnect), null
                    );
            }
            catch (SocketException se)
            {
                logger.Error("StartListen(): " + se.Message, se);
                throw se;
            }
        }

        private void FillRemoteHost(object RemoteEndPoint)
        {
            try
            {
                if (RemoteEndPoint != null)
                {
                    string[] Host = RemoteEndPoint.ToString().Split(':');
                    RemoteIP = Host[0].ToString();
                    RemotePort = Host[1].ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Error("FillRemoteHost(): " + ex.Message, ex);
                throw ex;
            }
        }

        private void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                Socket ActiveSocket = MainSocket.EndAccept(asyn);

                Interlocked.Increment(ref ClientCount);
                this.WaitForData(ActiveSocket, ClientCount);

                lock (HashActiveClients)
                {
                    if (HashActiveClients.ContainsKey(ClientCount))
                        HashActiveClients[ClientCount] = ActiveSocket;
                    else
                        HashActiveClients.Add(ClientCount, ActiveSocket);
                }

                ClientConnectedEventArgs args = new ClientConnectedEventArgs();

                args.ClientNumber = ClientCount;
                args.ClientSocket = ActiveSocket;

                if (OnClientConnected != null)
                    OnClientConnected(this, args);
                       
                MainSocket.BeginAccept( new AsyncCallback(OnClientConnect), null );
            }

            catch (SocketException se)
            {
                logger.Error("OnClientConnect(): " + se.Message, se);
                throw se;
            }
        }

        /// <summary>
        /// Envia uma mensagem para todos os clientes conectados
        /// </summary>
        /// <param name="msg"></param>
        public void SendToAll(byte[] msg)
        {
            lock (HashActiveClients)
            {
                foreach( Socket socket in HashActiveClients.Values )
                {
                    SendData(msg, socket);
                }
            }
        }


        public static void SendData(byte [] msg, Socket socket)
        {
            try
            {
                if (socket.Connected)
                {
                    if (logger.IsDebugEnabled)
                        logger.Debug("SendData [" + System.Text.Encoding.ASCII.GetString(msg) + "]");

                    socket.Send(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Send(byte[] msg)
        {
            SendData(msg, this._ClientSocket);
        }

        public void OpenConnection()
        {
            try
            {
                if (thMsgDispatcher == null)
                {
                    bKeepRunning = true;
                    thMsgDispatcher = new Thread(new ThreadStart(messageDispatcherProc));
                    thMsgDispatcher.Start();
                }

                logger.Info("Abrindo conexao com MDS: " + IpAddr + ":" + Port);

                bConectado = false;

                _ClientSocket =
                    new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp
                        );


                IPAddress IP = IPAddress.Parse(IpAddr);
                IPEndPoint ipEnd = new IPEndPoint(IP, Convert.ToInt32(Port));

                _ClientSocket.Connect(ipEnd);

                bConectado = true;

                ConnectionOpenedEventArgs args = new ConnectionOpenedEventArgs();
                if (OnConnectionOpened != null)
                    OnConnectionOpened(this, args);

                this.LastPacket = DateTime.Now;

                this.WaitForData(_ClientSocket,0);
            }
            catch (Exception ex)
            {
                logger.Error("OpenConnection(): " + ex.Message, ex);
            }
        }



        public void Dispose()
        {
            bKeepRunning = false;
            if ( thMsgDispatcher != null )
            {
                while(thMsgDispatcher.IsAlive )
                {
                    Thread.Sleep(250);
                }
            }
        }
    }

    public sealed class ClientSocketPacket
    {
        #region [Constructor]

        //Construtor responsável por encapsular o socket e número do cliente corrente.
        public ClientSocketPacket(System.Net.Sockets.Socket socket, int _clientNumber)
        {
            //Encapsula os membros.
            CurrentSocket = socket;
            ClientNumber = _clientNumber;
        }

        public ClientSocketPacket()
        {
        }
        #endregion

        #region [Members]strData

        public Socket CurrentSocket;
        public int ClientNumber;
        public byte[] sizeBuffer = new byte[ProxySocketPackage.SOCKET_BUFFER_SIZE];

        public byte[] Message;
        public long MsgLen;

        #endregion
    }
}
