using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;

namespace Gradual.OMS.Comunicacao.Automacao.Ordens
{
    public class ASEventArgs : EventArgs
    {
        public string Message { get; set; }

        public ASEventArgs(string msg)
        {
            this.Message = msg;
        }
    }

    public class ASSocketConnection
    {
        #region [Declarações]
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Socket _ClientSocket = null;
        private AsyncCallback WorkerCallBack;
        private IAsyncResult AsyncResult;

        public string IpAddr{
            get{
                return ConfigurationManager.AppSettings["ASConnIp"].ToString();
            }
        }

        public string Port{
            get{
                return ConfigurationManager.AppSettings["ASConnPort"].ToString();
            }
        }

        private enum EStatusRequest
        {
            Sucesso = 1,
            FalhaAutenticacao = 0,
            UsuarioLogado = 2
        };

        public delegate void ASAuthenticationResponseEventHandler(object Response, ASEventArgs e);
        public event ASAuthenticationResponseEventHandler OnASAuthenticationResponse;

        #endregion

        public void ASSocketOpen()
        {
            try
            {
                _ClientSocket =
                    new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp
                        );


                IPAddress IP = IPAddress.Parse(IpAddr);
                IPEndPoint ipEnd = new IPEndPoint(IP, Convert.ToInt32(Port));

                _ClientSocket.Connect(ipEnd);
                this.ThreadWaitOne();
            }
            catch (SocketException se)
            {
                throw (se);
            }

        }

        private void ASSocketClose(ASSocketPacket _SocketPacket)
        {
            _SocketPacket._Socket.Shutdown(SocketShutdown.Both);
            _SocketPacket._Socket.Close();
            _SocketPacket._Socket = null;
            _SocketPacket = null;
        }

        public void CloseConnection()
        {
            try
            {
                if (_ClientSocket != null && _ClientSocket.Connected)
                {
                    _ClientSocket.Shutdown(SocketShutdown.Both);
                    _ClientSocket.Close();
                    _ClientSocket = null;
                }
            }
            catch (Exception ex)
            {
                logger.Debug("CloseConnection():" + ex.Message, ex);
            }
        }

        public void SendData(string msg)
        {
            try
            {
                if (_ClientSocket.Connected)
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

                    _ClientSocket.Send(ByteVector);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ThreadWaitOne()
        {
            try
            {

                if (WorkerCallBack == null)
                {
                    //Cria o evento assíncrono que será disparado quando ocorrer um imput
                    //de dados
                    WorkerCallBack = new AsyncCallback(OnDataReceived);
                }

                // Encapsula o client socket solicitante e o clientNumber.
                ASSocketPacket _SocketPacket = new ASSocketPacket();
                _SocketPacket._Socket = _ClientSocket;

                //Inicializa a recepção assíncrona dos Dados
                AsyncResult = _ClientSocket.BeginReceive(_SocketPacket.dataBuffer,
                    0, _SocketPacket.dataBuffer.Length,
                    SocketFlags.None,
                    WorkerCallBack,
                    _SocketPacket);
            }
            catch (SocketException se)
            {
                throw (se);
            }
        }

#if SUNDA
        [MTAThread]
        private void OnDataReceived(IAsyncResult asyn)
        {
            ASSocketPacket socketData = (ASSocketPacket)asyn.AsyncState;

            try
            {
                lock (this)
                {
                    int ResultBytesLength = socketData._Socket.EndReceive(asyn);

                    if (ResultBytesLength <= 0)
                    {
                        this.ASSocketClose(socketData);
                        return;
                    }

                    char[] charsLength =
                        new char[ResultBytesLength];

                    System.Text.Decoder _Decoder = System.Text.Encoding.UTF8.GetDecoder();

                    _Decoder.GetChars(
                        socketData.dataBuffer,
                        0,
                        ResultBytesLength,
                        charsLength,
                        0);


                    int BufferSize = 0;
                    int pos = 0;

                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 24;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 16;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 8;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF);

                    byte[] dataBuffer = new byte[BufferSize];

                    int BytesReceive = socketData._Socket.Receive(dataBuffer,
                                                                  dataBuffer.Length,
                                                                  SocketFlags.None
                                                                  );

                    char[] charLenght = new char[BytesReceive];

                    string Message = string.Empty;

                    System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();

                    decoder.GetChars(
                        dataBuffer, 0, BytesReceive, charLenght, 0);

                    Message += new string(charLenght);            
        
                    if (BytesReceive < BufferSize)
                    {
                        Message +=
                            ReceiveData(
                            BytesReceive,
                            BufferSize,
                            socketData._Socket
                            );
                    }
               
                    
                    // Rebate a mensagem apenas substituindo o tipo para A3
                    Message = Message.Replace("A2", "A3");

                    // Realiza a conexão com o MDS e solicita Login.
                    MDSPackageSocket _ClientSocket = new MDSPackageSocket();
                    _ClientSocket.OpenConnection();
                    _ClientSocket.SendData(Message.ToString(), true);

                    // Apos envias a mensagem fecha a conexão.
                    this.ASSocketClose(socketData);
               
                }
            }
            catch (SocketException se)
            {
                if (se.ErrorCode == (int)(SocketError.ConnectionReset))
                {
                    this.ASSocketClose(socketData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ReceiveData(int BytesReceive, int BufferSize, Socket SocketData)
        {
            string Message = string.Empty;

            if ((BufferSize - BytesReceive) != 0){

                while (BytesReceive < BufferSize)
                {
                    byte[] Buffer =
                        new byte[(BufferSize - BytesReceive)];

                    int ByteRest = SocketData.Receive(Buffer,
                                                              (BufferSize - BytesReceive),
                                                              SocketFlags.None
                                                              );

                    char[] charLenght = new char[ByteRest];

                    System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();

                    decoder.GetChars(
                        Buffer,
                        0,
                        ByteRest,
                        charLenght,
                        0
                     );

                    BytesReceive += ByteRest;
                    Message += new string(charLenght);
                }
            }
            return Message;
        }

#else
        [MTAThread]
        private void OnDataReceived(IAsyncResult asyn)
        {
            string Instrumento = string.Empty;

            ASSocketPacket socketData = (ASSocketPacket)asyn.AsyncState;
            try
            {
                lock (this)
                {
                    int ResultBytesLength = socketData._Socket.EndReceive(asyn);

                    // Falha de conexão.
                    if (ResultBytesLength <= 0)
                    {
                        logger.Warn("Encerrou conexao com MDS (" + ResultBytesLength + ") recebendo tamanho");
                        this.ASSocketClose(socketData);
                        return;
                    }

                    // Se recebeu menos que 4 bytes, recebe os bytes restantes para compor o tamanho 
                    while (ResultBytesLength < 4)
                    {
                        int BytesReceived = socketData._Socket.Receive(socketData.dataBuffer,
                                                                    ResultBytesLength,
                                                                    4 - ResultBytesLength,
                                                                    SocketFlags.None);
                        if (BytesReceived <= 0)
                        {
                            logger.Warn("Encerrou conexao com MDS (" + ResultBytesLength + ") recebendo complemento do tamanho");
                            this.ASSocketClose(socketData);
                            return;
                        }

                        ResultBytesLength += BytesReceived;
                    }

                    char[] charsLength = new char[ResultBytesLength];

                    System.Text.Decoder _Decoder = System.Text.Encoding.UTF8.GetDecoder();

                    _Decoder.GetChars(
                        socketData.dataBuffer,
                        0,
                        ResultBytesLength,
                        charsLength,
                        0);

                    int BufferSize = 0;
                    int pos = 0;

                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 24;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 16;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF) << 8;
                    BufferSize += (((int)socketData.dataBuffer[pos++]) & 0xFF);

                    int TotalBytesReceived = 0;
                    string Message = string.Empty;

                    while (TotalBytesReceived < BufferSize)
                    {
                        int ToReceive = BufferSize - TotalBytesReceived;

                        byte[] dataBuffer = new byte[ToReceive];

                        int BytesReceived = socketData._Socket.Receive(dataBuffer,
                                                                      ToReceive,
                                                                      SocketFlags.None
                                                                      );
                        if (BytesReceived <= 0)
                        {
                            logger.Warn("Encerrou conexao com AuthenticationServer (" + ResultBytesLength + ") recebendo dados");
                            this.ASSocketClose(socketData);
                            return;
                        }


                        char[] charLenght = new char[BytesReceived];

                        System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();

                        decoder.GetChars(dataBuffer, 0, BytesReceived, charLenght, 0);

                        Message += new string(charLenght);

                        TotalBytesReceived += BytesReceived;
                    }

                    if ((Message != null) || (Message != ""))
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(
                         delegate(object required)
                         {
                             this.MessageBroker(Message);
                         }));
                    }

                    ThreadWaitOne();
                }
            }
            catch (SocketException se)
            {
                logger.Error("OnDataReceived(SocketEx): " + se.Message + " ErrorCode: " + se.ErrorCode +
                    " Native: " + se.NativeErrorCode +
                    " SocketError: " + se.SocketErrorCode, se);

                logger.Warn("OnDataReceived(): Encerrando conexao com MDS");

                // Falha de conexão
                this.ASSocketClose(socketData);
            }
            catch (Exception ex)
            {
                logger.Error("OnDataReceived(): " + ex.Message, ex);
                this.ASSocketClose(socketData);
            }
        }

#endif

        public void MessageBroker(string msg)
        {
            ASEventArgs args = new ASEventArgs(msg);

            if (this.OnASAuthenticationResponse != null)
            {
                this.OnASAuthenticationResponse(this, args);
            }
        }
    }    


    public sealed class ASSocketPacket
    {
        #region [Members]

        public Socket _Socket;
        public byte[] dataBuffer = new byte[4];

        #endregion
    }

   

}
