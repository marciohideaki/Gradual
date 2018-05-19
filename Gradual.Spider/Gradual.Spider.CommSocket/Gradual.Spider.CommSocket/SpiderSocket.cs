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
using System.IO;
using ProtoBuf;
using System.ComponentModel;


namespace Gradual.Spider.CommSocket
{
    public delegate void UnmappedObjectReceivedHandler(object sender, int clientNumber, Socket clientSock, Type objectType, object objeto);
    public delegate void ProtoObjectReceivedHandler<T>(object sender, int clientNumber, Socket clientSock, T args);
    public delegate void MessageReceivedHandler(object sender, MessageEventArgs args);
    public delegate void ClientConnectedHandler(object sender, ClientConnectedEventArgs args);
    public delegate void ConnectionOpenedHandler(object sender, ConnectionOpenedEventArgs args);
    public delegate void ClientDisconnectedHandler(object sender, ClientDisconnectedEventArgs args);

    //public class ProtoObjectEventArgs<T> : EventArgs
    //{
    //    public Type ObjectType { get; set; }
    //    public T ProtoObject { get; set; }
    //}

    public class MessageEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
        public int DataLen { get; set; }
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

    [ProtoContract]
    public class Envelope
    {
        [ProtoMember(1)]
        public Type Tipo { get; set; }
        [ProtoMember(2)]
        public byte[] Data { get; set; }
    }


    public class SpiderSocket : IDisposable
    {
        #region [local variables]

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Socket MainSocket = null;
        private ArrayList lstCom = new ArrayList();
        private AsyncCallback WorkerCallBack;
        private string RemoteIP = "";
        private string RemotePort = "";
        private bool _bKeepRunning = false;
        private Thread thQueueProcessor = null;
        private Thread thProtoProcessor = null;
        private Socket _ClientSocket;
        private bool bConectado;
        private ConcurrentQueue<MessageEventArgs> queueData = new ConcurrentQueue<MessageEventArgs>();
        private ConcurrentQueue<MessageEventArgs> queueProtos = new ConcurrentQueue<MessageEventArgs>();

        int ClientCount;

        #endregion

        #region [ Const variables ]

        public string IpAddr { get; set; }
        public int Port { get; set; }

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

        protected EventHandlerList listEventDelegates = new EventHandlerList();
        //public event [] OnObjectReceived;

        /// <summary>
        /// Event raised when a message was received (bytearray message)
        /// </summary>
        public event MessageReceivedHandler OnMessageReceived;

        /// <summary>
        /// Event raised when a new client has connected 
        /// </summary>
        public event ClientConnectedHandler OnClientConnected;

        /// <summary>
        /// Event raised when the connection was established
        /// </summary>
        public event ConnectionOpenedHandler OnConnectionOpened;

        /// <summary>
        /// Event raised when a connected client disconnects from the server
        /// </summary>
        public event ClientDisconnectedHandler OnClientDisconnected;

        /// <summary>
        /// Event raised when a object not mapped via AddHandler() was received
        /// </summary>
        public event UnmappedObjectReceivedHandler OnUnmappedObjectReceived;


        public SpiderSocket()
        {
            _bKeepRunning = true;

            thQueueProcessor = new Thread(new ThreadStart(queueProcessor));
            thQueueProcessor.Start();

            thProtoProcessor = new Thread(new ThreadStart(queueProtoProcessor));
            thProtoProcessor.Start();
        }


        /// <summary>
        /// Acrescenta um handler para tratar a recepção via socket de objetos T serializados
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void AddHandler<T>(ProtoObjectReceivedHandler<T> handler)
        {
            listEventDelegates.AddHandler(typeof(T), handler);
        }

        [MTAThread]
        private void OnDataReceived(IAsyncResult asyn)
        {
            ClientSocketPacket socketData = (ClientSocketPacket) asyn.AsyncState;
            try
            {
                int ResultBytesLength = socketData.CurrentSocket.EndReceive(asyn);

                if (ResultBytesLength <= 0)
                {
                    logger.Warn("Encerrou conexao (" + ResultBytesLength + ") recebendo tamanho");
                    ThreadPool.QueueUserWorkItem(new WaitCallback(NotificaClienteDesconectado), socketData);
                    this.CloseSocket(socketData.CurrentSocket);
                    return;
                }

                // Se recebeu menos que 4 bytes, recebe os bytes restantes para compor o tamanho 
                // ATP: Usa 5 bytes, 4 para o tamanho, mais um byte de flag para indicar que
                // o conteudo é um objecto serializado
                while (ResultBytesLength < 5)
                {
                    int BytesReceived = socketData.CurrentSocket.Receive(socketData.Header,
                                                                ResultBytesLength,
                                                                5 - ResultBytesLength,
                                                                SocketFlags.None);
                    if (BytesReceived <= 0)
                    {
                        logger.Warn("Encerrou conexao (" + ResultBytesLength + ") recebendo complemento do tamanho");
                        ThreadPool.QueueUserWorkItem(new WaitCallback(NotificaClienteDesconectado), socketData);
                        this.CloseSocket(socketData.CurrentSocket);
                        return;
                    }

                    ResultBytesLength += BytesReceived;
                }
                char[] charsLength = new char[ResultBytesLength];

                System.Text.Decoder _Decoder = System.Text.Encoding.UTF8.GetDecoder();

                _Decoder.GetChars(
                    socketData.Header,
                    0,
                    ResultBytesLength,
                    charsLength,
                    0);

                int BufferSize = 0;
                int pos = 0;

                BufferSize += (((int)socketData.Header[pos++]) & 0xFF) << 24;
                BufferSize += (((int)socketData.Header[pos++]) & 0xFF) << 16;
                BufferSize += (((int)socketData.Header[pos++]) & 0xFF) << 8;
                BufferSize += (((int)socketData.Header[pos++]) & 0xFF);

                bool bIsObject = (((int)socketData.Header[pos++]) & 0xFF) == 0x01;

                int TotalBytesReceived = 0;
                string Message = string.Empty;

                byte [] data = new byte[BufferSize];

                while (TotalBytesReceived < BufferSize)
                {
                    int ToReceive = BufferSize - TotalBytesReceived;

                    int BytesReceived = socketData.CurrentSocket.Receive(data,
                                                                    TotalBytesReceived,
                                                                    ToReceive,
                                                                    SocketFlags.None );
                    if (BytesReceived <= 0)
                    {
                        logger.Warn("Encerrou conexao (" + ResultBytesLength + ") recebendo dados");
                        this.CloseSocket(socketData.CurrentSocket);
                        return;
                    }

                    TotalBytesReceived += BytesReceived;
                }

                LastPacket = DateTime.Now;

                MessageEventArgs args = new MessageEventArgs();
                args.ClientNumber = socketData.ClientNumber;
                args.ClientSocket = socketData.CurrentSocket;
                args.Data = data;
                args.DataLen = BufferSize;

                if (bIsObject)
                    queueProtos.Enqueue(args);
                else
                    queueData.Enqueue(args);

                this.WaitForData(socketData.CurrentSocket, socketData.ClientNumber);
            }
            catch (SocketException se)
            {
                //if (se.ErrorCode == (int)(SocketError.ConnectionReset)){
                //    this.CloseSocket(socketData.CurrentSocket);
                //}
                logger.Error("OnDataReceived(SocketEx): " + se.Message + " ErrorCode: " + se.ErrorCode +
                    " Native: " + se.NativeErrorCode +
                    " SocketError: " + se.SocketErrorCode, se);

                logger.Warn("OnDataReceived(): Encerrando conexao com MDS");

                ThreadPool.QueueUserWorkItem(new WaitCallback(NotificaClienteDesconectado), socketData);

                // Falha de conexão
                this.CloseSocket(socketData.CurrentSocket);
            }
            catch (Exception ex)
            {
                logger.Error("OnDataReceived(): " + ex.Message, ex);
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


        private void NotificaClienteDesconectado(object objeto)
        {
            ClientSocketPacket socketData = (ClientSocketPacket)objeto;

            ClientDisconnectedEventArgs args = new ClientDisconnectedEventArgs();
            args.ClientNumber = socketData.ClientNumber;
            args.ClientSocket = socketData.CurrentSocket;

            if (OnClientDisconnected != null)
            {
                OnClientDisconnected(this, args);
            }
        }

        private void ClearSocketReset(object _currentSocket)
        {

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
            try
            {
                if (WorkerCallBack == null)
                {
                    WorkerCallBack = new AsyncCallback(OnDataReceived);
                }

                ClientSocketPacket _SocketPacket =
                    new ClientSocketPacket(socket, clientNumber);


                socket.BeginReceive(_SocketPacket.Header, 0,
                      _SocketPacket.Header.Length,
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
            if (MainSocket != null)
            {
                if (MainSocket.Connected == true)
                {
                    MainSocket.Close();
                }
            }
        }

        public void StartListen(int PortNumber)
        {
            try
            {
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

                MainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
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
        public void SendToAll(string msg)
        {
            lock (HashActiveClients)
            {
                foreach (Socket socket in HashActiveClients.Values)
                {
                    SendData(msg, socket);
                }
            }
        }

        /// <summary>
        /// Envia uma mensagem para todos os clientes conectados
        /// </summary>
        /// <param name="msg"></param>
        public void SendToAll(byte[] data, int len)
        {
            lock (HashActiveClients)
            {
                foreach (Socket socket in HashActiveClients.Values)
                {
                    SendData(data, len, socket);
                }
            }
        }

        /// <summary>
        /// Envia um objeto para todos os clientes conectados
        /// </summary>
        /// <param name="msg"></param>
        public void SendToAll(Object obj)
        {
            lock (HashActiveClients)
            {
                foreach (Socket socket in HashActiveClients.Values)
                {
                    SendObject(obj, socket);
                }
            }
        }

        public static void SendData(string msg, Socket socket, bool isObject=false)
        {
            try
            {
                if (socket.Connected)
                {

                    System.Text.Encoding enc = System.Text.Encoding.ASCII;

                    byte[] msgLenBuf = new byte[5];
                    int msgLen = msg.Length;

                    msgLenBuf[0] = (byte)((msgLen & 0xFF000000) >> 24);
                    msgLenBuf[1] = (byte)((msgLen & 0x00FF0000) >> 16);
                    msgLenBuf[2] = (byte)((msgLen & 0x0000FF00) >> 8);
                    msgLenBuf[3] = (byte)(msgLen & 0x000000FF);
                    msgLenBuf[4] = (byte) (isObject?0x01:0x00);

                    byte[] ByteArray = enc.GetBytes(msg);
                    byte[] ByteVector = new byte[5 + msg.Length];

                    System.Buffer.BlockCopy(msgLenBuf, 0, ByteVector, 0, 5);
                    System.Buffer.BlockCopy(ByteArray, 0, ByteVector, 5, msg.Length);

                    socket.Send(ByteVector);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Send(string msg)
        {
            SendData(msg, this._ClientSocket);
        }

        public void Send(byte[] data)
        {
            SendData(data, data.Length, this._ClientSocket);
        }

        public void Send(byte[] data, int len)
        {
            SendData(data, len, this._ClientSocket);
        }

        public void SendObject(Object obj)
        {
            MemoryStream xxx = new MemoryStream();

            Serializer.NonGeneric.Serialize(xxx, obj);

            Envelope envelope = new Envelope();
            envelope.Tipo = obj.GetType();
            envelope.Data = xxx.ToArray();

            xxx = new MemoryStream();
            Serializer.Serialize<Envelope>(xxx, envelope);

            byte[] arr = xxx.ToArray();

            SendData(arr, arr.Length, this._ClientSocket, true);
        }

        public static void SendData(byte[] data, int len, Socket socket, bool isObject = false)
        {
            try
            {
                if (socket.Connected)
                {
                    System.Text.Encoding enc = System.Text.Encoding.ASCII;

                    byte[] msgLenBuf = new byte[5];
                    int msgLen = len;

                    msgLenBuf[0] = (byte)((msgLen & 0xFF000000) >> 24);
                    msgLenBuf[1] = (byte)((msgLen & 0x00FF0000) >> 16);
                    msgLenBuf[2] = (byte)((msgLen & 0x0000FF00) >> 8);
                    msgLenBuf[3] = (byte)(msgLen & 0x000000FF);
                    msgLenBuf[4] = (byte)(isObject ? 0x01 : 0x00);

                    byte[] ByteVector = new byte[5 + msgLen];

                    System.Buffer.BlockCopy(msgLenBuf, 0, ByteVector, 0, 5);
                    System.Buffer.BlockCopy(data, 0, ByteVector, 5, len);

                    socket.Send(ByteVector, 5 + msgLen, SocketFlags.None);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendObject(Object obj, Socket socket)
        {
            MemoryStream xxx = new MemoryStream();

            Serializer.NonGeneric.Serialize(xxx, obj);

            Envelope envelope = new Envelope();
            envelope.Tipo = obj.GetType();
            envelope.Data = xxx.ToArray();

            xxx = new MemoryStream();
            Serializer.Serialize<Envelope>(xxx, envelope);

            byte[] arr = xxx.ToArray();

            SendData(arr, arr.Length, socket, true);
        }

        public void OpenConnection()
        {
            try
            {
                logger.Info("Abrindo conexao com: " + IpAddr + ":" + Port);

                bConectado = false;

                _ClientSocket =
                    new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp
                        );
                IPAddress IP;
                
                if (!IPAddress.TryParse(IpAddr, out IP))
                {
                    IP = Dns.GetHostEntry(IpAddr).AddressList[0];
                }
                IPEndPoint ipEnd = new IPEndPoint(IP, Port);

                _ClientSocket.Connect(ipEnd);

                bConectado = true;

                ConnectionOpenedEventArgs args = new ConnectionOpenedEventArgs();
                if (OnConnectionOpened != null)
                    OnConnectionOpened(this, args);

                this.LastPacket = DateTime.Now;

                this.WaitForData(_ClientSocket, 0);
            }
            catch (Exception ex)
            {
                logger.Error("OpenConnection(): " + ex.Message, ex);
            }
        }



        public void Dispose()
        {
            try
            {
                _bKeepRunning = false;

                if (thQueueProcessor != null)
                {
                    while (thQueueProcessor.IsAlive)
                    {
                        Thread.Sleep(100);
                    }
                }

                if (thProtoProcessor != null)
                {
                    while (thProtoProcessor.IsAlive)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void queueProcessor()
        {
            logger.Info("queueProcessor: iniciando thread de repasse dos pacotes");

            while (_bKeepRunning)
            {
                try
                {
                    MessageEventArgs args = null;
                    if (queueData.TryDequeue(out args))
                    {
                        if ( OnMessageReceived != null )
                        {
                            OnMessageReceived( this, args);
                        }

                        continue;
                    }

                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    logger.Error("queueProcessor: " + ex.Message, ex);
                }
            }

            logger.Info("queueProcessor: Finalizando thread de repasse dos pacotes");
        }

        private void queueProtoProcessor()
        {
            logger.Info("queueProtoProcessor: iniciando thread de deserializacao de objetos");

            while (_bKeepRunning)
            {
                try
                {
                    MessageEventArgs args = null;
                    if (queueProtos.TryDequeue(out args))
                    {
                        MemoryStream xxx = new MemoryStream(args.Data, 0, args.DataLen);

                        Envelope envelope = Serializer.Deserialize<Envelope>(xxx);

                        if (this.listEventDelegates[envelope.Tipo] != null)
                        {
                            xxx = new MemoryStream(envelope.Data);

                            Object objeto = Serializer.NonGeneric.Deserialize(envelope.Tipo, xxx);

                            object[] aux = new object[4];

                            aux[0] = this;
                            aux[1] = args.ClientNumber;
                            aux[2] = args.ClientSocket;
                            aux[3] = objeto;

                            this.listEventDelegates[envelope.Tipo].DynamicInvoke(aux);
                        }
                        else
                        {
                            if (OnUnmappedObjectReceived != null)
                            {
                                xxx = new MemoryStream(envelope.Data);

                                Object objeto = Serializer.NonGeneric.Deserialize(envelope.Tipo, xxx);

                                OnUnmappedObjectReceived(this, args.ClientNumber, args.ClientSocket, envelope.Tipo, objeto);
                            }
                            else
                            {
                                logger.Warn("Nao ha handler para objeto do tipo [" + envelope.Tipo + "]");
                            }
                        }


                        continue;
                    }

                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    logger.Error("queueProtoProcessor: " + ex.Message, ex);
                }
            }

            logger.Info("queueProtoProcessor: Finalizando thread de repasse dos pacotes");
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

        // 4 bytes para o tamanho
        // 1 byte flag de objecto
        public byte[] Header = new byte[5];

        public string Message;
        public long MsgLen;

        #endregion
    }
}
