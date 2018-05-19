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


namespace Gradual.OMS.Alertas
{
    public delegate void MessageReceivedHandler(object sender, MessageEventArgs args);
    public delegate void ClientConnectedHandler(object sender, ClientConnectedEventArgs args);
    public delegate void ConnectionOpenedHandler(object sender, ConnectionOpenedEventArgs args);

    public class MessageEventArgs : EventArgs
    {
        public string TipoMsg { get; set; }
        public string Message { get; set; }
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

    public class SocketPackage
    {
        #region [local variables]

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Socket MainSocket = null;
        private ArrayList lstCom = new ArrayList();
        private AsyncCallback WorkerCallBack;
        private const string SignIn = "A1";
        private const string SignOut = "A5";
        private const int BytesOfUID = 36;
        private string RemoteIP = "";
        private string RemotePort = "";

        int ClientCount;

        private enum ERetorno
        {
            Sucesso = 1,
            FalhaAutenticacao = 0,
            UsuarioLogado = 2
        };

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
        private Socket _ClientSocket;
        private bool bConectado;

        [MTAThread]
        private void OnDataReceived(IAsyncResult asyn)
        {
            ClientSocketPacket socketData = (ClientSocketPacket) asyn.AsyncState;
            try
            {
                lock (this)
                {
                    int ResultBytesLength = socketData.CurrentSocket.EndReceive(asyn);

                    if (ResultBytesLength <= 0)
                    {
                        logger.Warn("Encerrou conexao (" + ResultBytesLength + ") recebendo tamanho");
                        this.CloseSocket(socketData.CurrentSocket);
                        return;
                    }

                    // Se recebeu menos que 4 bytes, recebe os bytes restantes para compor o tamanho 
                    while (ResultBytesLength < 4)
                    {
                        int BytesReceived = socketData.CurrentSocket.Receive(socketData.sizeBuffer,
                                                                    ResultBytesLength,
                                                                    4 - ResultBytesLength,
                                                                    SocketFlags.None);
                        if (BytesReceived <= 0)
                        {
                            logger.Warn("Encerrou conexao (" + ResultBytesLength + ") recebendo complemento do tamanho");
                            this.CloseSocket(socketData.CurrentSocket);
                            return;
                        }

                        ResultBytesLength += BytesReceived;
                    }
                    char[] charsLength = new char[ResultBytesLength];

                    System.Text.Decoder _Decoder = System.Text.Encoding.UTF8.GetDecoder();

                    _Decoder.GetChars(
                        socketData.sizeBuffer,
                        0,
                        ResultBytesLength,
                        charsLength,
                        0);

                    int BufferSize = 0;
                    int pos = 0;

                    BufferSize += (((int)socketData.sizeBuffer[pos++]) & 0xFF) << 24;
                    BufferSize += (((int)socketData.sizeBuffer[pos++]) & 0xFF) << 16;
                    BufferSize += (((int)socketData.sizeBuffer[pos++]) & 0xFF) << 8;
                    BufferSize += (((int)socketData.sizeBuffer[pos++]) & 0xFF);

                    int TotalBytesReceived = 0;
                    string Message = string.Empty;

                    while (TotalBytesReceived < BufferSize)
                    {
                        int ToReceive = BufferSize - TotalBytesReceived;

                        byte[] dataBuffer = new byte[ToReceive];

                        int BytesReceived = socketData.CurrentSocket.Receive(dataBuffer,
                                                                      ToReceive,
                                                                      SocketFlags.None
                                                                      );
                        if (BytesReceived <= 0)
                        {
                            logger.Warn("Encerrou conexao (" + ResultBytesLength + ") recebendo dados");
                            this.CloseSocket(socketData.CurrentSocket);
                            return;
                        }


                        char[] charLenght = new char[BytesReceived];

                        System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();

                        decoder.GetChars(dataBuffer, 0, BytesReceived, charLenght, 0);

                        Message += new string(charLenght);

                        TotalBytesReceived += BytesReceived;
                    }

                    LastPacket = DateTime.Now;

                    ClientSocketPacket pkt = new ClientSocketPacket();

                    pkt.ClientNumber = socketData.ClientNumber;
                    pkt.CurrentSocket = socketData.CurrentSocket;
                    pkt.Message = Message;
                    pkt.MsgLen = BufferSize;
                    Array.Copy(socketData.sizeBuffer, pkt.sizeBuffer, 4);

                    ThreadPool.QueueUserWorkItem( new WaitCallback(MessageBroker), pkt );                  

                    this.WaitForData( socketData.CurrentSocket, socketData.ClientNumber );
                }
            }
            catch (SocketException se){
                //if (se.ErrorCode == (int)(SocketError.ConnectionReset)){
                //    this.CloseSocket(socketData.CurrentSocket);
                //}
                logger.Error("OnDataReceived(SocketEx): " + se.Message + " ErrorCode: " + se.ErrorCode +
                    " Native: " + se.NativeErrorCode +
                    " SocketError: " + se.SocketErrorCode, se);

                logger.Warn("OnDataReceived(): Encerrando conexao com MDS");

                // Falha de conexão
                this.CloseSocket(socketData.CurrentSocket);
            }
            catch (Exception ex){
                logger.Error("OnDataReceived(): " + ex.Message, ex);
            }
        }

        private void CloseSocket(Socket CurrentSocket){

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

        private void MessageBroker(object objeto)
        {
            ClientSocketPacket packet = (ClientSocketPacket) objeto;

            MessageEventArgs args = new MessageEventArgs();

            args.Message = packet.Message;
            args.ClientSocket = packet.CurrentSocket;
            args.ClientNumber = packet.ClientNumber;
            args.TipoMsg = packet.Message.Substring(0, 2);

            if (OnRequestReceived != null)
                OnRequestReceived(this, args);
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
                if (WorkerCallBack == null){
                    WorkerCallBack = new AsyncCallback(OnDataReceived);
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
            catch (SocketException se){
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
        public void SendToAll(string msg)
        {
            lock (HashActiveClients)
            {
                foreach( Socket socket in HashActiveClients.Values )
                {
                    SendData(msg, socket);
                }
            }
        }


        public void SendData(string msg, Socket socket)
        {
            try
            {
                if (socket.Connected)
                {

                    System.Text.Encoding enc = System.Text.Encoding.ASCII;

                    byte[] msgLenBuf = new byte[4];
                    int msgLen = msg.Length;

                    msgLenBuf[0] = (byte)((msgLen & 0xFF000000) >> 24);
                    msgLenBuf[1] = (byte)((msgLen & 0x00FF0000) >> 16);
                    msgLenBuf[2] = (byte)((msgLen & 0x0000FF00) >> 8);
                    msgLenBuf[3] = (byte)(msgLen & 0x000000FF);

                    byte[] ByteArray = enc.GetBytes(msg);
                    byte[] ByteVector = new byte[4 + msg.Length];

                    System.Buffer.BlockCopy(msgLenBuf, 0, ByteVector, 0, 4);
                    System.Buffer.BlockCopy(ByteArray, 0, ByteVector, 4, msg.Length);

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

        public void OpenConnection()
        {
            try
            {
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
        public byte[] sizeBuffer = new byte[4];

        public string Message;
        public long MsgLen;

        #endregion
    }
}
